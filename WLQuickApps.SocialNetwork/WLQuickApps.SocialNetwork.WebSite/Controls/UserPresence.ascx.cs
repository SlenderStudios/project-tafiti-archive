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

public partial class Controls_UserPresence : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    public void LoadPresence(string MessengerPresenceID)
    {
        // Is the presence ID set
        if (MessengerPresenceID != string.Empty)
        {
            // show the hyperlink (by default it is hidden)
            this.hypMessengerIMControl.Visible = true;

            // clean the Messenger Presence ID (convert the ID to have a suffix etc).
            string MessengerPresenceCleaned = WLQuickApps.SocialNetwork.Business.Utilities.CleanMessengerPresenceID(MessengerPresenceID);

            // Set the Image URI
            this.imgMessengerPresence.ImageUrl = string.Format(imgMessengerPresence.ImageUrl, MessengerPresenceCleaned);
            this.hypMessengerIMControl.NavigateUrl = string.Format(hypMessengerIMControl.NavigateUrl, MessengerPresenceCleaned);
        }
    }
}
