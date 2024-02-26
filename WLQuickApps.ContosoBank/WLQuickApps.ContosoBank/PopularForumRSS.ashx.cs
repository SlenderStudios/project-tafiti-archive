/*****************************************************************************
 * PopularForumRSS.ashx
 * Notes: Provides RSS feed for most popular forum posts (Most Views)
 * **************************************************************************/

using System.Text;
using System.Web;
using System.Web.Services;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PopularForumRSS : IHttpHandler
    {
        #region IHttpHandler Members

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            context.Response.ContentEncoding = Encoding.UTF8;

            ContosoRss rssGenerator = new ContosoRss();
            rssGenerator.Desc = "Australian Small Business Portal Popular Forum Items";
            rssGenerator.OutputStream = context.Response.OutputStream;
            rssGenerator.Title = "Australian Small Business Portal Forum";
            rssGenerator.Url = getForumPage(context);
            rssGenerator.ItemSource = ForumLogic.GetForumPostsByViews(5);

            rssGenerator.PublishRSS(rssGenerator);

            context.Response.End();
        }

        public bool IsReusable
        {
            get { return false; }
        }

        #endregion

        private static string getForumPage(HttpContext context)
        {
            string baseUrl = context.Request.Url.Scheme + "://";
            baseUrl += context.Request.Url.Host;
            baseUrl += context.Request.Url.Port.ToString().Length > 0
                           ? ":" + HttpContext.Current.Request.Url.Port
                           : string.Empty;
            baseUrl += context.Request.ApplicationPath;
            baseUrl += "Community.aspx";

            return baseUrl;
        }
    }
}