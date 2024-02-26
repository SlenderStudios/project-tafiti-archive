/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com
///            Dr. Neil Roodyn - neil.roodyn@aptovita.com
using System;
using System.Collections.Generic;
using System.Web.UI;
using Contoso.Common.Entity;
using Contoso.Common.Logic;

namespace Contoso
{
	public partial class Default_BIG : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ////Fix for demo - clear favs from session
            //Session.Clear();

            if (!IsPostBack)
            {

                //Look for WebIM/Presence ID on querystring, if exists modify the first news item and the WebIM window for demo purpose only
                string imUserID = Request.Params["id"];

                if (string.IsNullOrEmpty(imUserID))
                {
                    // use the default email address if the parameter is null
                    imUserID = GetLocalResourceObject("DefaultRecipient").ToString();
                }

                string sanitized_imUserID = "";

                if (imUserID.Contains("@"))
                {
                    sanitized_imUserID =    Microsoft.Security.Application.AntiXss.HtmlAttributeEncode(imUserID.Split("@".ToCharArray())[0]) + 
                                            "@" + 
                                            Microsoft.Security.Application.AntiXss.HtmlAttributeEncode((imUserID.Split("@".ToCharArray())[1]));
                }
                else
                {
                    sanitized_imUserID = Microsoft.Security.Application.AntiXss.HtmlAttributeEncode(imUserID) +
                                                System.Configuration.ConfigurationSettings.AppSettings["WindowsLiveMessengerIMControl_RecipientSuffix"];
                }

                // Get the default URI from the web.config (will have invitee={0})
                string MessengerIMControlURI = System.Configuration.ConfigurationSettings.AppSettings["WindowsLiveMessengerIMControl_URI"];

                // merge in the Messenger User ID and Clean the output
                MessengerIMControlURI = string.Format(MessengerIMControlURI, Request.UserLanguages[0], sanitized_imUserID);

                ChatFrame.Attributes.Add("src", MessengerIMControlURI);
            }
        }
    }
}