using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.Tafiti.WebSite;
using WLQuickApps.Tafiti.Business;

public partial class _Default : System.Web.UI.Page
{
    protected override void OnPreLoad(EventArgs e)
    {
        if (SettingsWrapper.LiveAnalyticsEnabled)
        {
            // Setup tbe MS Analytics script
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "MSAnalyticsScript",
                string.Format(@"
<script language=""javascript"" type=""text/javascript"" src=""{0}://analytics.live.com/Analytics/msAnalytics.js""></script>
<script language=""javascript"" type=""text/javascript"">
     msAnalytics.ProfileId = ""{1}"";
     msAnalytics.TrackPage();
</script>", this.Request.Url.Scheme, SettingsWrapper.LiveAnalyticsID));
        }

        base.OnPreLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);  // prevent caching of this page

        if (ConfigurationManager.AppSettings["ShowServicePage"] == "true")
        {            
            Response.Redirect("service.html", true);
            return;
        }

        this._loggedInUserPanel.Visible = UserManager.IsUserLoggedIn;
    }

}
