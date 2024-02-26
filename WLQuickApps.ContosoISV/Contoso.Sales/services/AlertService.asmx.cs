/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Services;
using System.Web.Services;
using Contoso.Alerts.Live.Alerts.Message;
using Contoso.Common.Entity;

namespace Contoso.Sales.services
{
    /// <summary>
    /// Summary description for AlertService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [GenerateScriptType(typeof(Appointment))]
    [ToolboxItem(false)]
    [ScriptService]
    public class AlertService : WebService
    {

        [WebMethod()]
        [ScriptMethod]
        public string CheckUserSignup(string userId)
        {
            return Contoso.Alerts.Alert.CheckUserSignup(userId, "http://alerts.msn.com");
        }

        [WebMethod()]
        [ScriptMethod]
        public bool SendGroupRequest(Appointment appointment, string currentCulture)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentCulture);

            string content = MergeTemplate("AlertContent", appointment);
            string emailMessage = MergeTemplate("AlertEmail", appointment);
            string messengerMessage = content;
            string mobile = MergeTemplate("AlertMobile", appointment);
            string sendtoTransport = string.Empty;

            List<string> users = new List<string>();
            users.Add("");
            RecServicesGroupMessage message = global::Contoso.Alerts.Alert.CreateGroupMessage(ConfigurationManager.AppSettings["MessageGroup"], content, emailMessage, messengerMessage, mobile, global::Contoso.Alerts.Alert.CreateMessageContacts(users));
            global::Contoso.Alerts.Alert.SendGroupRequest(message, sendtoTransport);
            return true;
        }
        private static string MergeTemplate(string cacheKey, Appointment appointment)
        {
            //first check if data is in the cache
            string template = Resource.ResourceManager.GetString(cacheKey);
 
            //replace tokens with content
            template = template.Replace("[APTDATE]", appointment.AptDate);
            template = template.Replace("[APTTIME]", appointment.AptTime);
            template = template.Replace("[CLIENTADDRESS]", appointment.ClientAddress);
            template = template.Replace("[CLIENTCOMPANY]", appointment.ClientCompany);
            template = template.Replace("[CLIENTNAME]", appointment.ClientName);
            template = template.Replace("[CLIENTPHONE]", appointment.ClientPhone);
            return template;
        }

    }
}