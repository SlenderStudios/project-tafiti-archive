using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class ProcessMessengerConsent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Pull the ID out of the querystring
        string MessengerPresenceID = Request.QueryString["ID"];

        // Check if it is null
        if (MessengerPresenceID == null)
        {
            // Default it to emptystring
            MessengerPresenceID = "";
        }

        // Clean the code for Malicious attacks
        MessengerPresenceID = Microsoft.Security.Application.AntiXss.JavaScriptEncode(MessengerPresenceID);

        // create stringbuilder
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        // Set the Startup javascript which calls the handleMessengerPermissionResponse and passes the presenceID back to the parent window;
        // this functiona also closes the window
        sb.Append("<script language=javascript>");
        sb.Append("  window.opener.handleMessengerPermissionResponse(" + MessengerPresenceID + ");");
        sb.Append("  window.close();");
        sb.Append("</script>");

        // Register startup script.
        Page.ClientScript.RegisterStartupScript(typeof(string), "MessengerResponse", sb.ToString());
    }
}