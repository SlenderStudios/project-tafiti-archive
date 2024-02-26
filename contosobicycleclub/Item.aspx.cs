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

namespace ContosoBicycleClub
{
    public partial class Item : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string feed="";

            if ((Request.QueryString["feed"].StartsWith("http://")) ||
                (Request.QueryString["feed"].StartsWith("https://")))
            {
                feed = Request.QueryString["feed"];
            }
            else
                throw new Exception("Unknown Feed Format - must start with http:// or https://");

            string item = Request.QueryString["item"];


            if (!String.IsNullOrEmpty(feed))
            {
                itemDataSource.XPath = string.Format("/rss/channel/item[{0}]", item);
                itemDataSource.DataFile = feed;

                itemDataList.DataBind();
            }
        }
    }
}
