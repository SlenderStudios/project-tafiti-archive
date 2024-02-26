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
using System.Text.RegularExpressions;
using System.Xml;

public partial class Item : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string feed = Request.QueryString["feed"];
        string item = Request.QueryString["item"];

        if (feed.StartsWith("http://") || feed.StartsWith("https://"))
        {
            // Load the datasource with the RSS content
            if (feed != null)
            {
                itemDataSource.XPath = string.Format("/rss/channel/item[guid='{0}']", item);
                itemDataSource.DataFile = feed;
                XmlDocument x = itemDataSource.GetXmlDocument();
                itemDataList.DataBind();
            }
        }
        else
        {
            throw new Exception("Unknown Feed Format - must start with http:// or https://");
        }    
    }
}
