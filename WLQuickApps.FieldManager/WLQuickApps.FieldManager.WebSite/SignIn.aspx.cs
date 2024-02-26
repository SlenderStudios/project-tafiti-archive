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

using WLQuickApps.FieldManager.Business;

namespace WLQuickApps.FieldManager.WebSite
{
    public partial class SignIn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (UserManager.UserIsLoggedIn())
            {
                this.Response.Redirect(string.Format("http://login.live.com/logout.srf?appid={0}", SettingsWrapper.LiveAuthID));
                return;
            }

            string path = "~/Default.aspx";
            if (!string.IsNullOrEmpty(this.Request.QueryString["ReturnUrl"]))
            {
                path = HttpUtility.UrlEncode(this.Request.QueryString["ReturnUrl"]);
            }

            this.Response.Redirect(string.Format("http://login.live.com/wlogin.srf?appid={0}&appctx={1}", SettingsWrapper.LiveAuthID, path));
        }
    }
}