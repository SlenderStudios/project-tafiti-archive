using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class ForumSummaryControl : UserControl
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
            ForumSubjectGridView.DataSource = ForumLogic.GetAllForumPosts();
            ForumSubjectGridView.DataBind();
        }

        protected void ForumSubjectGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ForumSubject item = (ForumSubject) e.Row.DataItem;
                Image tempImage = (Image) e.Row.FindControl("SubjectTypeImage");
                tempImage.ImageUrl = item.SubjectImageLocation;

                HyperLink tempLink = (HyperLink) e.Row.FindControl("forumpostLink");
                tempLink.NavigateUrl = "../ForumThread.aspx?ID=" + item.ForumSubjectID;
                
                tempLink.Text = item.Subject;
                if (item.Subject.Length > 55)
                {
                    tempLink.Text = item.Subject.Substring(0, 52) + "...";
                }

                Label tempLabel = (Label) e.Row.FindControl("RepliesLabel");
                tempLabel.Text = item.ForumReplies.Count.ToString();

                tempLabel = (Label) e.Row.FindControl("LastPostLabel");
                tempLabel.Text = getLastPostText(item);

                tempLabel = (Label) e.Row.FindControl("PostByLabel");
                tempLabel.Text = item.UserProfile.DisplayName;
            }
        }

        private static string getLastPostText(ForumSubject rowData)
        {
            int timediff;
            string period;
            TimeSpan diff = DateTime.Now.Subtract(rowData.PostDate);
            if (diff.Days > 0)
            {
                timediff = diff.Days;
                period = timediff == 1 ? "day" : "days";
            }
            else if (diff.Hours > 0)
            {
                timediff = diff.Hours;
                period = timediff == 1 ? "hour" : "hours";
            }
            else if (diff.Minutes > 0)
            {
                timediff = diff.Minutes;
                period = timediff == 1 ? "minute" : "minutes";
            }
            else
            {
                timediff = diff.Seconds;
                period = timediff == 1 ? "second" : "seconds";
            }


            return timediff + " " + period + " ago";
        }
    }
}