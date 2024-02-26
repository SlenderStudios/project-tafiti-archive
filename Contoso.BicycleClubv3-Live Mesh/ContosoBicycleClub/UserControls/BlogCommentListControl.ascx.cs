using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;

namespace WLQuickApps.ContosoBicycleClub.UserControls
{
	public partial class BlogCommentListControl : System.Web.UI.UserControl
	{
		protected XmlNamespaceManager commentsNsManager;
		protected XmlNodeList comments;

		public string BlogId { get; set;}

		protected void Page_Load(object sender, EventArgs e)
		{
			
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			try
			{
				string metaWeblogUser = ConfigurationManager.AppSettings["MetaWeblogUser"];
				string commentsFeedUrl = string.Format("http://{0}.spaces.live.com/blog/cns!{1}/comments/feed.rss", metaWeblogUser, BlogId);

				CommentsFeedLink.NavigateUrl = commentsFeedUrl;

				XmlDocument commentsFeed = new XmlDocument();
				commentsFeed.Load(commentsFeedUrl);

				commentsNsManager = new XmlNamespaceManager(commentsFeed.NameTable);
				commentsNsManager.AddNamespace("live", "http://schemas.microsoft.com/live/spaces/2006/rss");

				string addCommentUrl = commentsFeed.SelectSingleNode("rss/channel/link").InnerText;
				AddCommentLink.NavigateUrl = addCommentUrl;

				comments = commentsFeed.SelectNodes("rss/channel/item");

				if (comments != null && comments.Count > 0)
				{
					RideCommentsItems.DataSource = comments;
					RideCommentsItems.DataBind();
				}
				else
				{
					HtmlGenericControl noComments = new HtmlGenericControl("p");
					noComments.Attributes.Add("class", "clearBoth");
					noComments.InnerText = "No comments found.";
					CommentsPanel.Controls.Add(noComments);
				}

			}
			catch (Exception ex)
			{
				HtmlGenericControl error = new HtmlGenericControl("p");
				error.Attributes.Add("class", "clearBoth");
				error.InnerHtml = "ERROR: " + "<br />" + ex.Message;
				CommentsPanel.Controls.Add(error);
				CommentsPanel.Controls.Remove(EventCommentsLinks);
			}
		}
	}
}