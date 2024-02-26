using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class StickyPosts : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bindData();
            }
        }

        private void bindData()
        {
            StickyPostRepeater.DataSource = ForumLogic.GetStickyPosts();
            StickyPostRepeater.DataBind();
        }

        protected void StickyPostRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Item)
            {
                Image temp = (Image)e.Item.FindControl("stickyImage");
                temp.ImageUrl = ((ForumSubject)e.Item.DataItem).SubjectImageLocation;
                Avatar userAvatar = (Avatar)e.Item.FindControl("userAvatar");
                ((IAvatarData)userAvatar).Item =
                    AvatarLogic.GetAvatarByLocation(((ForumSubject)e.Item.DataItem).UserProfile.Avatar);

                Label tempLabel = (Label)e.Item.FindControl("lastPostLabel");
                tempLabel.Text = getStickyLastPostText((ForumSubject)e.Item.DataItem);

                tempLabel = (Label)e.Item.FindControl("postUserName");
                if (((ForumSubject)e.Item.DataItem).ForumReplies.Count > 0)
                    tempLabel.Text = ((ForumSubject)e.Item.DataItem).ForumReplies.Last().UserProfile.DisplayName;

                tempLabel = (Label)e.Item.FindControl("posttextLabel");
                if (((ForumSubject)e.Item.DataItem).ForumReplies.Count > 0)
                    tempLabel.Text = ((ForumSubject)e.Item.DataItem).ForumReplies.First().ReplyText;

                tempLabel = (Label)e.Item.FindControl("SubjectLabel");
                string forumSubject = ((ForumSubject)e.Item.DataItem).Subject;
                tempLabel.Text = (forumSubject.Length > 42) ? forumSubject.Substring(0, 39) + "..." : forumSubject;


                if ((((ForumSubject)e.Item.DataItem).ForumReplies.Count>0)&&!string.IsNullOrEmpty(((ForumSubject)e.Item.DataItem).ForumReplies.First().Tags))
                {
                    tempLabel = (Label)e.Item.FindControl("posttestTags");
                    tempLabel.Visible = true;

                    tempLabel = (Label)e.Item.FindControl("posttagsLabel");
                    tempLabel.Text = ((ForumSubject)e.Item.DataItem).ForumReplies.First().Tags;
                }

                tempLabel = (Label)e.Item.FindControl("stickyDisplayName");
                tempLabel.Text = ((ForumSubject)e.Item.DataItem).UserProfile.DisplayName;

                tempLabel = (Label)e.Item.FindControl("numberofpostsLabel");
                tempLabel.Text = getStickyNumberofPosts((ForumSubject)e.Item.DataItem);

                Rating rating = (Rating)e.Item.FindControl("UserRating");
                rating.CurrentRating = ((ForumSubject)e.Item.DataItem).UserProfile.Rating;
            }
        }

        private static string getStickyLastPostText(ForumSubject rowData)
        {
            if (rowData.ForumReplies.Count > 0)
                return rowData.ForumReplies.Last().ReplyDate.ToString("MMMM dd, yyyy hh:mm tt") + " by ";
            else
                return string.Empty;
        }

        private static string getStickyNumberofPosts(ForumSubject rowData)
        {
            int numPosts = rowData.UserProfile.ForumReplies.Count();
            DateTime firstPostDate=new DateTime();
            if( rowData.UserProfile.ForumReplies.Count>0)
             firstPostDate= rowData.UserProfile.ForumReplies.First().ReplyDate;

            return numPosts + " posts since " + firstPostDate.ToString("MMMM dd, yyyy");
        }
    }
}