using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

using WLQuickApps.SocialNetwork.Business;

/// <summary>
/// Summary description for ViewRewriteHandler
/// </summary>
namespace WLQuickApps.SocialNetwork.WebSite
{
    public class ViewRewriteHandler : IHttpHandler
    {
        public ViewRewriteHandler()
        {
        }

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string requestString = Path.GetFileNameWithoutExtension(context.Request.Url.LocalPath);

            try
            {
                UserManager.GetUserByUserName(requestString);
                context.Server.Transfer(string.Format("~/Friend/ViewProfile.aspx?{0}={1}", WebConstants.QueryVariables.UserName, requestString));
            }
            catch (ArgumentException)
            {
                int baseItemID;
                Int32.TryParse(requestString, out baseItemID);

                try
                {
                    BaseItem baseItem = BaseItemManager.GetBaseItem(baseItemID);

                    if (baseItem is Album)
                    {
                        context.Server.Transfer(string.Format("~/Media/ViewAlbum.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, requestString));
                    }
                    else
                    {
                        context.Server.Transfer(string.Format("~/{0}/View{0}.aspx?{0}ID={1}", baseItem.GetType().Name, baseItemID));
                    }
                }
                catch (ArgumentException)
                {
                    context.Server.Transfer("~/Default.aspx");
                }
            }
        }

        #endregion
    }
}