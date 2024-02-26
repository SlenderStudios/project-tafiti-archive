using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.Security.Application;

using WLQuickApps.Tafiti.Business;

public partial class ProcessMessengerConsent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserManager.VerifyUserIsLoggedIn();

        // Pull the ID out of the querystring
        string messengerPresenceID = Request.QueryString["ID"];

        // Check if it is null
        if (messengerPresenceID == null)
        {
            // Default it to emptystring
            messengerPresenceID = string.Empty;
        }

        // create stringbuilder
        StringBuilder sb = new System.Text.StringBuilder();

        User user = UserManager.LoggedInUser;
        user.MessengerPresenceID = messengerPresenceID;
        UserManager.UpdateUser(user);

        // Set the Startup javascript which calls the handleMessengerPermissionResponse and passes the presenceID back to the parent window;
        // this function also closes the window
        sb.Append("<script type=\"text/javascript\" language=\"javascript\">");
        sb.AppendFormat("  window.opener.handleMessengerPermissionResponse({0});", AntiXss.JavaScriptEncode(messengerPresenceID));
        sb.Append("  window.close();");
        sb.Append("</script>");

        // Register startup script.
        Page.ClientScript.RegisterStartupScript(typeof(string), "MessengerResponse", sb.ToString());
    }
}