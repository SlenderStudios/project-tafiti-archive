using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class ForumReplies : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void BindData(List<ForumReply> replies)
        {
            PostRepeater.DataSource = replies;
            PostRepeater.DataBind();
        }

        protected void PostRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem ||
                e.Item.ItemType == ListItemType.Item)
            {
                ForumReply item = (ForumReply)e.Item.DataItem;

                Avatar userAvatar = (Avatar)e.Item.FindControl("userAvatar");
                ((IAvatarData)userAvatar).Item =
                    AvatarLogic.GetAvatarByLocation(((ForumReply)e.Item.DataItem).UserProfile.Avatar);

                Label tempLabel = (Label)e.Item.FindControl("postDisplayName");
                tempLabel.Text = item.UserProfile.DisplayName;

                tempLabel = (Label)e.Item.FindControl("posttextLabel");
                tempLabel.Text = item.ReplyText;


                if (!string.IsNullOrEmpty(item.Tags))
                {
                    tempLabel = (Label)e.Item.FindControl("posttestTags");
                    tempLabel.Visible = true;

                    tempLabel = (Label)e.Item.FindControl("posttagsLabel");
                    tempLabel.Text = item.Tags;
                }

                tempLabel = (Label)e.Item.FindControl("numberofpostsLabel");
                tempLabel.Text = getNumberofPosts(item);

                Rating rating = (Rating)e.Item.FindControl("UserRating");
                rating.CurrentRating = item.UserProfile.Rating;
            }
            else if (e.Item.ItemType == ListItemType.Header)
            {
                ForumSubject item = ForumLogic.GetRecord(Convert.ToInt32(Request.QueryString["ID"]));
                Image temp = (Image)e.Item.FindControl("subjectImage");
                temp.ImageUrl = item.SubjectImageLocation;

                Label tempLabel = (Label)e.Item.FindControl("postTimeLabel");
                tempLabel.Text = getLastPostText(item);

                tempLabel = (Label)e.Item.FindControl("postUserName");
                if (item.ForumReplies.Count > 0)
                    tempLabel.Text = (item.ForumReplies.Last().UserProfile.DisplayName);

                tempLabel = (Label)e.Item.FindControl("SubjectLabel");
                string forumSubject = item.Subject;
                tempLabel.Text = (forumSubject.Length > 42) ? forumSubject.Substring(0, 39) + "..." : forumSubject;
            }
        }

        private static string getLastPostText(ForumSubject rowData)
        {
            if (rowData.ForumReplies.Count > 0)
                return rowData.ForumReplies.Last().ReplyDate.ToString("MMMM dd, yyyy hh:mm tt") + " by ";
            else
                return string.Empty;
        }


        private static string getNumberofPosts(ForumReply rowData)
        {
            int numPosts = rowData.UserProfile.ForumReplies.Count();
            DateTime firstPostDate = new DateTime();
            if (rowData.UserProfile.ForumReplies.Count > 0)
                firstPostDate = rowData.UserProfile.ForumReplies.First().ReplyDate;

            return numPosts + " posts since " + firstPostDate.ToString("MMMM dd, yyyy");
        }
    }
}