using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using WLQuickApps.ContosoBicycleClub.Business;

namespace WLQuickApps.ContosoBicycleClub
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ImageHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            ImageManager mgr = new ImageManager();
            string type = "";
            byte[] imageBytes = null;
            mgr.GetImage(new Guid(context.Request.QueryString["ImageId"]), ref type, ref imageBytes);
            context.Response.ContentType = type;
            context.Response.BinaryWrite(imageBytes); 
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
