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

public partial class Classifieds : System.Web.UI.Page
{
    protected override void Render(HtmlTextWriter output)
    {
        string appKey = ConfigurationManager.AppSettings["ExpoAppKey"];
        string siteId = ConfigurationManager.AppSettings["ExpoSiteId"];

        ExpoService expoService = new ExpoService(appKey, siteId);

        // Call the ExpoService
        XmlDocument xmlDocument = expoService.GetListings("82", "","bicycle");

        if(xmlDocument != null)
        {
            if (xmlDocument.SelectSingleNode("error") != null)
            {
                // Render an error.
                output.Write("<div id=\"classifieds\">");
                output.Write("<h1>An error has occured retrieving from Windows Live Expo.</h1><br />");
                output.Write(xmlDocument.OuterXml.Replace("<", "&lt;").Replace(">", "&gt;<br />"));
                output.Write("</div>");
            }
            else
            {
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDocument.NameTable);
                nsmgr.AddNamespace("classifieds", "http://expo.live.com/ns/2006/1.0");

                XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/rss/channel/item", nsmgr);

                int lines = xmlNodeList.Count;

                if (lines > 10) lines = 10;
                output.Write("<div id=\"classifieds\">");

                output.Write("<h1>Bikes &amp; Kit</h1>");

                output.Write("<table>");
                for (int i = 0; i < lines; i++)
                {
                    XmlNode xmlNode = xmlNodeList[i];

                    XmlNode enclosure = xmlNode.SelectSingleNode("enclosure");

                    output.Write("<tr>");

                    output.Write("<td>");
                    if (enclosure != null)
                        output.Write(string.Format("<img src=\"{0}\"/>", xmlNode.SelectSingleNode("enclosure").Attributes["url"].Value));
                    output.Write("</td>");
                    output.Write("<td valign=\"top\">");

                    output.Write(string.Format("<div class=\"title\"><a target=\"_blank\" href=\"{1}\">{0}</a></div>", xmlNode["title"].InnerText, xmlNode["link"].InnerText));
                    output.Write(string.Format("<div class=\"description\">{0}", xmlNode["description"].InnerText));

                    output.Write(string.Format("<span class=\"price\"> {0} {1}</span></div>", xmlNode["classifieds:price"].InnerText, xmlNode["classifieds:currency"].InnerText));
                    output.Write("</td>");
                    output.Write("</tr>");
                }
                output.Write("</table>");
                output.Write("</div>");
            }
        }
    }
}
