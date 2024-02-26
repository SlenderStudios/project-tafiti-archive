/*****************************************************************************
 * ContosoRss.cs
 * Notes: Helper class to generate rss feeds.
 * **************************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank
{
    public class ContosoRss
    {
        public Stream OutputStream { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Desc { get; set; }
        public List<BaseForum> ItemSource { get; set; }
        public string ItemTitle { get; set; }
        public string ItemUrl { get; set; }
        public string ItemDescription { get; set; }
        public string ItemPubDate { get; set; }

        public void PublishRSS(ContosoRss rss)
        {
            var writer = new XmlTextWriter(rss.OutputStream, Encoding.ASCII);
            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "2.0");
            writer.WriteStartElement("channel");
            writer.WriteElementString("title", rss.Title);
            writer.WriteElementString("link", rss.Url);
            writer.WriteElementString("description", rss.Desc);

            foreach (BaseForum item in ItemSource)
            {
                writer.WriteStartElement("item");
                writer.WriteElementString("title", item.Title);
                writer.WriteElementString("link", item.PostLink);
                writer.WriteElementString("description",
                                          string.Format("<img src='{0}' width=20></img> {1}", item.Avatar, item.ForumDetail));
                writer.WriteElementString("pubDate", item.PublishDate);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.Flush();
        }
    }
}