using System;
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

using Microsoft.Security.Application;

using WLQuickApps.FieldManager.Business;
using WLQuickApps.FieldManager.Data;

namespace WLQuickApps.FieldManager.WebSite
{
    public partial class Directions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this._endAddress.Text = AntiXss.JavaScriptEncode(this.Request.QueryString["endAddress"]);

            if (UserManager.UserIsLoggedIn())
            {
                this._startAddress.Text = AntiXss.JavaScriptEncode(UserManager.LoggedInUser.Address);
            }
        }
    }
}