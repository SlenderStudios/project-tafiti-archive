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
using WLQuickApps.SocialNetwork.Business;
using AjaxControlToolkit;
using System.IO;
using System.Drawing.Imaging;

public partial class Forum_ViewForum : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) {}

    protected void _deleteDiscussion_Command(object sender, CommandEventArgs e)
    {
        Forum forum = ForumManager.GetForum(Int32.Parse(e.CommandArgument.ToString()));
        forum.Delete();
        Response.Redirect("~/Forum/ViewForums.aspx");
    }

    protected void _rating_Changed(object sender, RatingEventArgs e)
    {
//        MediaManager.GetMedia(Int32.Parse(e.Tag)).Rate(Int32.Parse(e.Value));
    }
}
