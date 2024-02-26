using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Title = WLQuickApps.SocialNetwork.WebSite.SettingsWrapper.SiteTitle;

        if (UserManager.IsUserLoggedIn())
        {
            this._userDetailsForm.Visible = true;
            this._userDetailsForm.DataSource = new User[] { UserManager.LoggedInUser };
            this._userDetailsForm.DataBind();

            // Sync the user's photo albums
            // we could put some type of timer on this but really the users
            // won't spent a large amoutn of time on the homepage
            // and it makes it easy to demo.
            if (!IsPostBack)
            {
                AlbumManager.SyncUserAlbumsAsynchronous();
            }
         }

        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SearchBoxScript",
            String.Format(@"
                <script type=""text/javascript"">
                <!--
                
                    function TextEnter(e)
                    {{
                        var evt = e ? e : window.event;
                        if ((evt.keyCode == 13) && (document.getElementById(""{0}"").value != """"))
                        {{
                            window.location.href = ""{1}"" + document.getElementById(""{0}"").value;
                            return false;
                        }}
                    }}
                //-->
                </script>", this._searchTextBox.ClientID, this.ResolveClientUrl("~/Search.aspx?tag=")));

        // Setup the smooth "ENTER" button press for search
        this._searchTextBox.Attributes.Add("onkeypress", "return TextEnter(event)");

        this._analyticsPanel.Visible = !string.IsNullOrEmpty(SettingsWrapper.MicrosoftAnalyticsID);
      
    }
   
  
   
}


