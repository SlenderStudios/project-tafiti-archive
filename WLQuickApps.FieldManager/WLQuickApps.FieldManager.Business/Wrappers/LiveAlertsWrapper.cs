using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WLQuickApps.FieldManager.Business.Properties;

using SubscriptionService = WLQuickApps.FieldManager.Business.LiveAlertsSubscriptionService;
using MessageService = WLQuickApps.FieldManager.Business.LiveAlertsMessageService;

namespace WLQuickApps.FieldManager.Business
{
    public class LiveAlertsWrapper
    {
        private struct Headers
        {
            public SubscriptionService.RecServicesHeader SubscriptionHeader;
            public SubscriptionService.RecServicesIdentification SubscriptionID;
            public MessageService.RecServicesHeader MessageHeader;
            public MessageService.RecServicesIdentification MessageID;
        }

        static private Headers CreateHeaders()
        {
            Headers headers = new Headers();
            headers.SubscriptionHeader = new SubscriptionService.RecServicesHeader();
            headers.SubscriptionHeader.messageID = DateTime.Now.ToString("yyyy'-'MM'-'dd") + "." + Guid.NewGuid().ToString() + "." + SettingsWrapper.LiveAlertsPin.ToString();
            headers.SubscriptionHeader.timestamp = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssK");
            headers.SubscriptionHeader.version = "1.0";
            headers.MessageHeader = new MessageService.RecServicesHeader();
            headers.MessageHeader.messageID = headers.SubscriptionHeader.messageID;
            headers.MessageHeader.timestamp = headers.SubscriptionHeader.timestamp;
            headers.MessageHeader.version = headers.SubscriptionHeader.version;

            headers.SubscriptionID = new SubscriptionService.RecServicesIdentification();
            headers.SubscriptionID.PINID = SettingsWrapper.LiveAlertsPin;
            headers.SubscriptionID.PW = SettingsWrapper.LiveAlertsPassword;
            headers.MessageID = new MessageService.RecServicesIdentification();
            headers.MessageID.PINID = SettingsWrapper.LiveAlertsPin;
            headers.MessageID.PW = SettingsWrapper.LiveAlertsPassword;

            return headers;
        }

        static public string GetAlertsSignupUrl(string returnUrl)
        {
            if (!SettingsWrapper.EnableLiveAlerts) { return string.Empty; }
            if (string.IsNullOrEmpty(returnUrl)) { throw new ArgumentException("returnUrl cannot be null or empty"); }
            UserManager.VerifyAUserIsLoggedIn();

            using (SubscriptionService.AlertsWebServicesService service = new SubscriptionService.AlertsWebServicesService())
            {
                Headers headers = LiveAlertsWrapper.CreateHeaders();

                SubscriptionService.RecAlertsRequestResponse response = null ;
                try
                {
                    response = service.InitiateSignup(headers.SubscriptionHeader, headers.SubscriptionID, UserManager.LoggedInUser.UserID.ToString("n"), returnUrl, "MSNA");
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Trace.Write("Error sending alert. " + ex.Message);
                    //throw new Exception("Error getting alert URL", e);
                }

                if (response != null)
                {
                    if (response.URL != null)
                    {
                        return response.URL;
                    }
                }
                return string.Empty;
            }
        }

        static public string TestAlertConnectivity()
        {
            string returnUrl = "http://foo.com";

            if (!SettingsWrapper.EnableLiveAlerts)
            {
                throw new Exception("Alerts are not enabled in the web.config");
            }

            // Verify the current user is signed in.
            UserManager.VerifyAUserIsLoggedIn();

            using (SubscriptionService.AlertsWebServicesService service = new SubscriptionService.AlertsWebServicesService())
            {
                Headers headers = LiveAlertsWrapper.CreateHeaders();

                SubscriptionService.RecAlertsRequestResponse response = null;

                response = service.InitiateSignup(headers.SubscriptionHeader, headers.SubscriptionID, UserManager.LoggedInUser.UserID.ToString("n"), returnUrl, "MSNA");

                return (response.URL);
            }
        }

        static public void UnsubscribeAll()
        {
            if (!SettingsWrapper.EnableLiveAlerts) { throw new InvalidOperationException("Alerts are not enabled"); }
            UserManager.VerifyAUserIsLoggedIn();

            using (SubscriptionService.AlertsWebServicesService service = new SubscriptionService.AlertsWebServicesService())
            {
                Headers headers = LiveAlertsWrapper.CreateHeaders();
                SubscriptionService.RecServicesRequestResponse response =
                    service.UnsubscribeAll(headers.SubscriptionHeader, headers.SubscriptionID, string.Empty, UserManager.LoggedInUser.DisplayName);
            }
        }

        static public void SendAlert(IEnumerable<string> recipients, string messageText, string url, string fromName)
        {
            if (!SettingsWrapper.EnableLiveAlerts) { throw new InvalidOperationException("Alerts are not enabled"); }

            MessageService.RecServicesMessage message = new MessageService.RecServicesMessage();
            if (!string.IsNullOrEmpty(url)) { message.actionURL = url; }
            message.content = messageText;
            message.emailMessage = messageText;
            message.messengerMessage = messageText;
            message.mobileMessage = messageText;
            message.superToastMessage = messageText;


            List<MessageService.RecServicesContact> contacts = new List<MessageService.RecServicesContact>();
            foreach (string recipient in recipients)
            {
                MessageService.RecServicesContact contact = new MessageService.RecServicesContact();
                contact.from = fromName;
                contact.to = recipient;
                contact.transport = "MSNA";
                contacts.Add(contact);
            }
            message.contacts = contacts.ToArray();

            Headers headers = LiveAlertsWrapper.CreateHeaders();

            using (MessageService.MessageWebServicesService service = new MessageService.MessageWebServicesService())
            {
                MessageService.RecDetailRequestResponse response =
                    service.Deliver(headers.MessageHeader, headers.MessageID, message);

                switch (response.response.statusCode)
                {
                    case 0:
                    case 5:
                        return;

                    default:
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendFormat("Unable to send alert. Code: \"{0}\". Reason text: \"{1}\".\r\n\r\n", response.response.statusCode, response.response.statusReason);
                        foreach (MessageService.RecServicesInformation info in response.info)
                        {
                            stringBuilder.AppendFormat("Unable to send: {0}. Code: \"{1}\". Reason text: \"{2}\".\r\n", info.item, info.response.statusCode, info.response.statusReason);
                        }                        
                        break;
                }
            }
        }
    }
}
