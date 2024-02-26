using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using WLQuickApps.FieldManager.Business;

namespace WLQuickApps.FieldManager.WebSite
{
    public partial class FieldManager : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this._loginLink.NavigateUrl = UserManager.UserIsLoggedIn() ?
                string.Format("http://login.live.com/logout.srf?appid={0}", SettingsWrapper.LiveAuthID) :
                string.Format("http://login.live.com/wlogin.srf?appid={0}&appctx={1}", SettingsWrapper.LiveAuthID, this.Request.Url.PathAndQuery);
            this._loginLink.Text = UserManager.UserIsLoggedIn() ? "Sign Out" : "Sign In";
        }
    }
}