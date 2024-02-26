/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
using System;
using System.Collections.Generic;
using System.Configuration;
using Contoso.Alerts.Live.Alerts.Message;
using Contoso.Alerts.Live.Alerts.Subscription;
using Contoso.Common.Exceptions;
using RecAlertsRequestResponse=Contoso.Alerts.Live.Alerts.Subscription.RecAlertsRequestResponse;
using RecServicesContact=Contoso.Alerts.Live.Alerts.Message.RecServicesContact;
using RecServicesHeader=Contoso.Alerts.Live.Alerts.Message.RecServicesHeader;
using RecServicesIdentification=Contoso.Alerts.Live.Alerts.Message.RecServicesIdentification;
using RecServicesRequestResponse=Contoso.Alerts.Live.Alerts.Subscription.RecServicesRequestResponse;

namespace Contoso.Alerts
{
    public class Alert
    {
        public static string CheckUserSignup(string userID, string returnURL)
        {
            
            string result = string.Empty;
            AlertsWebServicesService alertsService = new AlertsWebServicesService();
            RecAlertsRequestResponse response =
                alertsService.InitiateSignup(createRequestHeader(), createRequestID(), userID, returnURL,
                                             ConfigurationManager.AppSettings["TransportType"]);
            switch (response.response.statusCode)
            {
                case 0:
                    result = response.URL;
                    break;
                case 326:
                    // user exists
                    break;
                default:
                    throw new AlertException(
                        string.Format("Error code: {0}.  Error Message: {1}", response.response.statusCode,
                                      response.response.statusReason));
            }
            return result;
        }

        public static void AddUserToGroup(string user, string group)
        {
            AlertsWebServicesService alertsService = new AlertsWebServicesService();
            RecServicesRequestResponse response =
                alertsService.AddGroup(createRequestHeader(), createRequestID(), group, group);
            switch (response.response.statusCode)
            {
                case 0:
                    addUserToGroup(alertsService, group, user);
                    break;
            }
        }

        private static void addUserToGroup(AlertsWebServicesService alertsService, string group, string user)
        {
            string[] groups = new string[1];
            groups[0] = group;
            RecServicesRequestResponse response =
                alertsService.ChangeSubscription(createRequestHeader(), createRequestID(), user, groups, "add", user, 0,
                                                 "en-us", "en-us");
            switch (response.response.statusCode)
            {
                case 0:
                    break;
            }
        }

        public static void SendGroupRequest(RecServicesGroupMessage message, string sendtoTransport)
        {
            MessageWebServicesService messageService = new MessageWebServicesService();
            Live.Alerts.Message.RecServicesRequestResponse response =
                messageService.GroupDeliver(createMessageHeader(), createMessageID(), message);
            switch (response.response.statusCode)
            {
                case 0:
                    // success
                    break;
                default:
                    throw new AlertException(
                        string.Format("Error code: {0}.  Error Message: {1}", response.response.statusCode,
                                      response.response.statusReason));
            }
        }

        public static List<RecServicesContact> CreateMessageContacts(List<string> to)
        {
            List<RecServicesContact> result = new List<RecServicesContact>();
            foreach (string person in to)
            {
                result.Add(CreateMessageContact(person));
            }
            return result;
        }

        private static RecServicesContact CreateMessageContact(string to)
        {
            RecServicesContact contact = new RecServicesContact();
            contact.from = ConfigurationManager.AppSettings["AlertFrom"];
            contact.to = to;
            contact.transport = ConfigurationManager.AppSettings["TransportType"];

            return contact;
        }

        public static RecServicesMessage CreateMessage(string content, string emailMessage, string messengerMessage,
                                                       string mobileMessage, List<RecServicesContact> contacts)
        {
            RecServicesMessage message = new RecServicesMessage();
            message.content = content;
            message.emailMessage = emailMessage;
            message.messengerMessage = messengerMessage;
            message.mobileMessage = mobileMessage;
            message.contacts = contacts.ToArray();
            return message;
        }

        public static RecServicesGroupMessage CreateGroupMessage(string groupName, string content, string emailMessage,
                                                                 string messengerMessage, string mobileMessage,
                                                                 List<RecServicesContact> contacts)
        {
            RecServicesGroupMessage message = new RecServicesGroupMessage();
            message.content = content;
            message.emailMessage = emailMessage;
            message.messengerMessage = messengerMessage;
            message.mobileMessage = mobileMessage;
            message.fromContacts = contacts.ToArray();
            message.groupName = groupName;
            return message;
        }

        private static RecServicesIdentification createMessageID()
        {
            RecServicesIdentification recID = new RecServicesIdentification();
            recID.PINID = Convert.ToInt32(ConfigurationManager.AppSettings["AlertPIN"]);
            recID.PW = ConfigurationManager.AppSettings["AlertPassword"];
            return recID;
        }

        private static RecServicesHeader createMessageHeader()
        {
            string messageID = Guid.NewGuid().ToString();
            RecServicesHeader recHeader = new RecServicesHeader();
            recHeader.version = "1.0";
            DateTime now = DateTime.Now;
            recHeader.timestamp = String.Format("{0}T{1}", now.ToString("yyyy-MM-dd"), now.ToString("HH:mm:sszzz"));

            recHeader.messageID =
                string.Format("{0}.{1}.{2}", now.ToString("yyyy-MM-dd"), messageID,
                              Convert.ToInt32(ConfigurationManager.AppSettings["AlertPIN"]));
            return recHeader;
        }

        private static Live.Alerts.Subscription.RecServicesIdentification createRequestID()
        {
            Live.Alerts.Subscription.RecServicesIdentification recID =
                new Live.Alerts.Subscription.RecServicesIdentification();
            recID.PINID = Convert.ToInt32(ConfigurationManager.AppSettings["AlertPIN"]);
            recID.PW = ConfigurationManager.AppSettings["AlertPassword"];
            return recID;
        }

        private static Live.Alerts.Subscription.RecServicesHeader createRequestHeader()
        {
            string messageID = Guid.NewGuid().ToString();
            Live.Alerts.Subscription.RecServicesHeader recHeader = new Live.Alerts.Subscription.RecServicesHeader();
            recHeader.version = "1.0";
            DateTime now = DateTime.Now;
            recHeader.timestamp = String.Format("{0}T{1}", now.ToString("yyyy-MM-dd"), now.ToString("HH:mm:sszzz"));

            recHeader.messageID =
                string.Format("{0}.{1}.{2}", now.ToString("yyyy-MM-dd"), messageID,
                              Convert.ToInt32(ConfigurationManager.AppSettings["AlertPIN"]));
            return recHeader;
        }

    }
}