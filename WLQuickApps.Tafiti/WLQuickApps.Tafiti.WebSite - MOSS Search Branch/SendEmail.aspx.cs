using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

using WLQuickApps.Tafiti.WebSite;

public partial class SendEmail : System.Web.UI.Page
{
    void Page_Load(object sender, EventArgs args)
    {
        Utility.VerifyIsSelfRequest();

        Response.Cache.SetCacheability(HttpCacheability.NoCache);  // prevent caching of this page

        string emailAddress = Request.Form["emailAddress"];
        if (Utility.IsValidEmailAddress(emailAddress))
        {
            Utility.SendEmail("notify@tafiti.com", new string[] { "tafiti@microsoft.com" }, "Notify me when Tafiti is ready", emailAddress);
        }

        UriBuilder redirectUrl = new UriBuilder(Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped));
        redirectUrl.Path = "thankyou.html";
        Response.Redirect(redirectUrl.ToString());
    }
}
