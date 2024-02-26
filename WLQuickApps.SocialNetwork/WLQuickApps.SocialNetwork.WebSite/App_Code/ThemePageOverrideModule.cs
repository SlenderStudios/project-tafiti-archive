using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.SessionState;

namespace WLQuickApps.SocialNetwork.WebSite
{
    public class ThemePageOverrideModule : IHttpModule
    {
        public ThemePageOverrideModule()
        {
        }

        #region IHttpModule Members

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += new EventHandler(context_PreRequestHandlerExecute);
        }

        void context_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            HttpContext context = app.Context;
            UriBuilder urlBuilder = new UriBuilder(context.Request.Url);

            string appRelativePath = VirtualPathUtility.ToAppRelative(urlBuilder.Path);

            Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            PagesSection pagesSection = (PagesSection)configuration.GetSection("system.web/pages");
            string theme = pagesSection.Theme;

            if (appRelativePath.StartsWith(string.Format("~/{0}/", theme), StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            string themeFilePath = string.Format("~/{0}/{1}", theme, appRelativePath.Substring(2));

            if (File.Exists(app.Server.MapPath(themeFilePath)))
            {
                if (urlBuilder.Query.Length > 0)
                {
                    themeFilePath = string.Format("{0}{1}", themeFilePath, urlBuilder.Query);
                }

                app.Response.Redirect(themeFilePath);
                return;
            }

        }

        public void Dispose()
        {
            // Nothing to dispose.
        }

        #endregion
    }
}