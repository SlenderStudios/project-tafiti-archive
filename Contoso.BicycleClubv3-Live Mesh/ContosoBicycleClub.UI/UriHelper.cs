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
    public class UriHelper
    {
        public static string GetServerRoot()
        {
            HttpRequest request = HttpContext.Current.Request;
            return string.Format("{0}://{1}:{2}", request.Url.Scheme, request.Url.Host, request.Url.Port);
        }
    }
}
