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

public partial class AWR : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string siteTitle = ConfigurationManager.AppSettings[WebConstants.AppSettingKeys.SiteTitle];
        string pageTitle = this.Page.Title;

        if ((String.IsNullOrEmpty(this.Page.Title)) || (this.Page.Title == "Untitled Page"))
        {
            if (SiteMap.CurrentNode == null)
            {
                pageTitle = siteTitle;
            }
            else
            {
                pageTitle = SiteMap.CurrentNode.Title;
            }
        }

        if (pageTitle != siteTitle)
        {
            pageTitle = String.Format("{0} - {1}", siteTitle, pageTitle);
        }

        this.Page.Title = pageTitle;

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

        // Setup tbe BlogThis script
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "BlogThisScript",
            @"
            <script type=""text/javascript"">
            function blogThis()
            {
                var sel=document.selection.createRange();
                sel.expand(""word"");
                window.open(""http://spaces.msn.com/BlogIt.aspx?Title=""+escape(document.title)+""&SourceURL=""+escape(window.location)+""&description=""+escape(sel.text));
            }
            </script>");

        this._blogThisLink.Visible = (UserManager.IsUserLoggedIn() && UserManager.LoggedInUser.RssFeedUrl.Contains("spaces.live.com"));


        // Was the privacy link found
        if (this._privacyLink != null)
        {
            // Is the Privacy Statement override setup?
            if (SettingsWrapper.PrivacyStatementOverrideUrl != "")
            {
                // Yes - override the URL
                this._privacyLink.NavigateUrl = SettingsWrapper.PrivacyStatementOverrideUrl;
                this._privacyLink.Target = "_blank";
            }
        }

        // Was the label found
        if (this._hostingLabel != null)
        {
            // Is the Privacy Statement override setup?
            if (SettingsWrapper.HostingLabel != "")
            {
                // Yes - override the URL
                this._hostingLabel.Text = SettingsWrapper.HostingLabel;
            }
            else
            {
                this._hostingLabel.Visible = false;
            }
        }

        if (UserManager.IsUserLoggedIn())
        {
            this._userDetailsForm.Visible = true;
            this._userDetailsForm.DataSource = new User[] { UserManager.LoggedInUser };
            this._userDetailsForm.DataBind();
        }

        this._analyticsPanel.Visible = !string.IsNullOrEmpty(SettingsWrapper.MicrosoftAnalyticsID);
    }
}


