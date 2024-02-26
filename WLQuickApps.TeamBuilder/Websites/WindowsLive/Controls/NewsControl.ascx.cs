using System;
using System.ServiceModel.Syndication;
using System.Collections.Generic;
using System.Linq;

public partial class Controls_NewsControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var feedData = from feed in GetFeeds()
                       select new
                       {
                           Title = feed.Title.Text,
                           Link = feed.Links.Count > 0 ? feed.Links[0].Uri.ToString() : "#",
                           Items = from item in feed.Items.Take(3)
                                   select new
                                   {
                                       Title = item.Title.Text,
                                       PublishDate = item.PublishDate.DateTime.ToShortDateString(),
                                       Link = item.Links.Count > 0 ? item.Links[0].Uri.ToString() : "#"
                                   }
                       };

        NewsList.DataSource = feedData;
        NewsList.DataBind();
    }

    protected List<SyndicationFeed> GetFeeds()
    {
        List<SyndicationFeed> feeds = new List<SyndicationFeed>();

        feeds.Add(SyndicationFeed.Load(System.Xml.XmlReader.Create("http://msn-slapshots.spaces.live.com/blog/feed.rss")));
        feeds.Add(SyndicationFeed.Load(System.Xml.XmlReader.Create("http://www.thescore.ca/blogs/junior_hockey.xml")));
        feeds.Add(SyndicationFeed.Load(System.Xml.XmlReader.Create("http://www.nhl.com/rss/news.xml")));

        return feeds;
    }
}
