using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using WLQuickApps.SocialNetwork.Business.Properties;

using SubscriptionService = WLQuickApps.SocialNetwork.Business.LiveAlertsSubscriptionService;
using MessageService = WLQuickApps.SocialNetwork.Business.LiveAlertsMessageService;


namespace WLQuickApps.SocialNetwork.Business
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
            if (!UserManager.IsUserLoggedIn()) { throw new SecurityException("User must be logged in"); }

            using (SubscriptionService.AlertsWebServicesService service = new SubscriptionService.AlertsWebServicesService())
            {
                Headers headers = LiveAlertsWrapper.CreateHeaders();

                SubscriptionService.RecAlertsRequestResponse response;
                try
                {
                    response = service.InitiateSignup(headers.SubscriptionHeader, headers.SubscriptionID, UserManager.LoggedInUser.UserName, returnUrl, "MSNA");
                }
                catch (Exception e)
                {
                    HealthMonitoringManager.LogError(e, 
                        "Unable to retrieve data from the Live Alerts service. " +
                        "This is often due to invalid credentials or attempting " +
                        "to make a request from an IP address that has not been " +
                        "registered with the Live Alerts network.");
                    throw;
                }

                if (response.URL != null)
                {
                    return response.URL;
                }

                return string.Empty;
            }
        }

        static public void UnsubscribeAll()
        {
            if (!SettingsWrapper.EnableLiveAlerts) { throw new InvalidOperationException("Alerts are not enabled"); }

            if (!UserManager.IsUserLoggedIn()) { throw new SecurityException("User must be logged in"); }

            using (SubscriptionService.AlertsWebServicesService service = new SubscriptionService.AlertsWebServicesService())
            {
                Headers headers = LiveAlertsWrapper.CreateHeaders();
                SubscriptionService.RecServicesRequestResponse response =
                    service.UnsubscribeAll(headers.SubscriptionHeader, headers.SubscriptionID, string.Empty, UserManager.LoggedInUser.UserName);
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
                        HealthMonitoringManager.LogError(stringBuilder.ToString());
                        break;
                }
            }
        }
    }
}
