<%@ WebService Language="C#" Class="AlertService" %>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Web.Caching;
using WLQuickApps.ContosoSoda.Alerts.Live.Alerts.Message;

    /// <summary>
    /// Summary description for AlertService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class AlertService : System.Web.Services.WebService
    {

        [WebMethod()]
        [ScriptMethod]
        public string CheckUserSignup(string userId)
        {
            return WLDemo.Alerts.Alert.CheckUserSignup(userId, "http://alerts.msn.com");
        }

        [WebMethod()]
        [ScriptMethod]
        public bool SendGroupRequest(string currentCulture)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentCulture);

            string content = "New Video in Cocoa-Soda World Record Breakers 2008";
            string emailMessage = content;
            string messengerMessage = content;
            string mobile = content;
            string sendtoTransport = string.Empty;

            List<string> users = new List<string>();
            users.Add("");
            RecServicesGroupMessage message = global::WLDemo.Alerts.Alert.CreateGroupMessage(ConfigurationManager.AppSettings["MessageGroup"], content, emailMessage, messengerMessage, mobile, global::WLDemo.Alerts.Alert.CreateMessageContacts(users));
            global::WLDemo.Alerts.Alert.SendGroupRequest(message, sendtoTransport);
            return true;
        }
        [WebMethod()]
        [ScriptMethod]
        public bool SendSingleRequest(string currentCulture)
        {
            RecServicesContact contact = new RecServicesContact();
      contact.to = "dq_patrick@live.com";
      contact.transport = "MSNA";
      contact.from = "someone";

      RecServicesMessage message = new RecServicesMessage();

      message.contacts = new RecServicesContact [] {contact};
      message.content = "someuser";
      message.emailMessage = "test Alert email";
      message.messengerMessage = "IM Message";
      global::WLDemo.Alerts.Alert.SendSingleRequest(message,"");
            return true;
        }
    }

