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
using WLQuickApps.SocialNetwork.WebSite;

public partial class Media_ViewMedia : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) 
    {
        MediaManager.GetMedia(int.Parse(this.Request[WebConstants.QueryVariables.BaseItemID])).IncrementViews();
    }

    protected void _approveMedia_Command(object sender, CommandEventArgs e)
    {
        Media media = MediaManager.GetMedia(Int32.Parse(e.CommandArgument.ToString()));
        media.Approve();
        Response.Redirect(WebUtilities.GetViewItemUrl(media));
    }

    protected void _deleteMedia_Command(object sender, CommandEventArgs e)
    {
        Media media = MediaManager.GetMedia(Int32.Parse(e.CommandArgument.ToString()));
        media.Delete();
        Response.Redirect(WebUtilities.GetViewItemUrl(media.ParentAlbum));
    }

    protected void _setAsAlbumThumbnail_Click(object sender, EventArgs e)
    {
        Media media = MediaManager.GetMedia(Int32.Parse(Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
        MemoryStream ms = new MemoryStream();
        media.GetThumbnail(256, 256).Save(ms, ImageFormat.Jpeg);
        media.ParentAlbum.SetThumbnail(ms.ToArray());
    }

    protected void _setAsProfilePicture_Click(object sender, EventArgs e)
    {
        Media media = MediaManager.GetMedia(Int32.Parse(Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
        MemoryStream ms = new MemoryStream();
        media.GetThumbnail(256, 256).Save(ms, ImageFormat.Jpeg);
        UserManager.LoggedInUser.SetThumbnail(ms.ToArray());
    }

    protected void _rating_Changed(object sender, RatingEventArgs e)
    {
        MediaManager.GetMedia(Int32.Parse(e.Tag)).Rate(Int32.Parse(e.Value));
    }
}
