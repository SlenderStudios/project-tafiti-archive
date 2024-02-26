using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using Microsoft.Security.Application;

public partial class ProcessMessengerConsent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Pull the ID out of the querystring
        string messengerPresenceID = AntiXss.JavaScriptEncode(this.Request.QueryString["ID"]);

        // Check if it is null
        if (messengerPresenceID == null)
        {
            // Default it to empty string
            messengerPresenceID = "''";
        }

        // create stringbuilder
        StringBuilder sb = new System.Text.StringBuilder();

        // Set the Startup javascript which calls the handleMessengerPermissionResponse and passes the presenceID back to the parent window;
        // this functiona also closes the window
        sb.Append("<script language=javascript>");
        sb.Append("  window.opener.handleMessengerPermissionResponse(" + messengerPresenceID + ");");
        sb.Append("  window.close();");
        sb.Append("</script>");

        // Register startup script.
        this.Page.ClientScript.RegisterStartupScript(typeof(string), "MessengerResponse", sb.ToString());
    }
}
