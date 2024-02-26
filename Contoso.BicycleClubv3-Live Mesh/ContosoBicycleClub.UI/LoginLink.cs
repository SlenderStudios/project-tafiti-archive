using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace WLQuickApps.ContosoBicycleClub.UI
{
    /// <summary>
    /// Summary description for LoginLink
    /// </summary>
    [ParseChildren(false)]
    [ControlBuilder(typeof(HyperLinkControlBuilder))]
    [Designer("System.Web.UI.Design.WebControls.HyperLinkDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    [ToolboxData(@"<{0}:LoginLink runat=""server"" />")]
    [DefaultProperty("Text")]
    public class LoginLink : HyperLink
    {
        public LoginLink()
            : base()
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            WindowsLiveLogin wl = new WindowsLiveLogin(true);

            if (this.Page.User.Identity.IsAuthenticated)
            {
				base.Text = "Sign out";

                if (WindowsLiveLogin.IsUserAuthenticated())
                {
                    base.NavigateUrl = wl.GetLogoutUrl();
                }
                else
                {
                    //base.NavigateUrl = "~/Logout.aspx";
                    base.NavigateUrl = wl.GetLogoutUrl();
                }
            }
            else
            {
								base.Text = "Sign in";
                base.NavigateUrl = wl.GetLoginUrl();
            }
        }
    }
}
