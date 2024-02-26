using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace WLQuickApps.SocialNetwork.WebSite
{
    public class WindowsLiveLoginModule : IHttpModule
    {
        public WindowsLiveLoginModule()
        {
        }

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AcquireRequestState);
        }

        private void context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpApplication context = (HttpApplication)sender;
            HttpSessionState session;

            try
            {
                session = context.Session;
            }
            catch (HttpException)
            {
                return;
            }

            string vrpath = VirtualPathUtility.ToAppRelative(context.Request.Url.AbsolutePath);
            bool IsSpecified = (( string.Compare(vrpath, "~/Register.aspx", true) == 0 )
                                ||( string.Compare(vrpath, "~/ProcessMessengerConsent.aspx", true) == 0 )
                                ||( string.Compare(vrpath, "~/PhotoAlbumPermission.aspx", true) == 0 ));

            // If the user is logged into WIndows Live but isn't logged into Forms Authentication;
            // and the Request isn't going to Register.aspx or ProcessMessengerConsent.aspx
            if (  WindowsLiveLogin.IsUserAuthenticated() &&  !context.User.Identity.IsAuthenticated && !IsSpecified  )
            {
                // User has logged into Windows Live auth but not into our site. Redirect
                // to Register.aspx to log them into our site.
                context.Response.Redirect("~/Register.aspx");
            }
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

        #endregion
    }
}
