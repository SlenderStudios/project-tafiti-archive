using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class LatestPostsControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LatestPostRepeater.DataSource = ForumLogic.GetLatestForumPosts(5);
                LatestPostRepeater.DataBind();
            }
        }

        protected void LatestPostRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Item)
            {
                HyperLink link = (HyperLink) e.Item.FindControl("forumHyperLink");
                link.NavigateUrl = "../ForumThread.aspx?ID=" + ((ForumSubject) e.Item.DataItem).ForumSubjectID;

                Label subject = (Label) e.Item.FindControl("forumSubject");
                string subjectText = ((ForumSubject) e.Item.DataItem).Subject;
                if (subjectText.Length > 55)
                {
                    subjectText = subjectText.Substring(0, 52) + "...";
                }
                subject.Text = subjectText;
            }
        }
    }
}