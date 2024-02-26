using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;


namespace ContosoBicycleClub
{
    public partial class Photos : System.Web.UI.Page
    {
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static AjaxControlToolkit.Slide[] GetSlides(string contextKey)
        {
            return SlideShow.GetSlides(contextKey);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Set the context key of the SlideShow to be the RSS feed.
            if ((Request.QueryString["feed"].StartsWith("http://")) ||
                (Request.QueryString["feed"].StartsWith("https://")))
            {
                slideshowextend1.ContextKey = Request.QueryString["feed"];
            }
            else
                throw new Exception("Unknown Feed Format - must start with http:// or https://");

        }
    }
}