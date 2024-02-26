using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace WLQuickApps.SocialNetwork.Business
{
    public class UserCacheModule : System.Web.IHttpModule
    {
        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(this.context_EndRequest);
        }

        private void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication context = (HttpApplication)sender;

            if ((context.User != null) && (context.User.Identity != null) && (context.User.Identity.IsAuthenticated))
            {
                string userName = context.User.Identity.Name;
                if (!string.IsNullOrEmpty(userName))
                {
                    context.Context.Cache.Remove(userName);
                }
            }
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

        #endregion
    }
}
