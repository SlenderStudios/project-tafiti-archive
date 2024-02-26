using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WLQuickApps.SocialNetwork.WebSite
{
    public class P3PCompactPolicyModule : System.Web.IHttpModule
    {
        public P3PCompactPolicyModule()
        {
        }

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += new EventHandler(this.context_PreSendRequestHeaders);
        }

        private void context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication context = (HttpApplication)sender;

            context.Response.AddHeader(WebConstants.HttpHeaderKeys.P3P, string.Format("CP=\"{0}\",policyref=\"/w3c/p3p.xml\"", SettingsWrapper.P3PCompactPolicy));
        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

        #endregion
    }
}
