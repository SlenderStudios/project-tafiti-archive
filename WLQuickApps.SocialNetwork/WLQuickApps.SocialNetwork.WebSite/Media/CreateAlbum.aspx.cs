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
using System.Drawing;
using System.IO;

using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Media_CreateAlbum : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void _createButton_Click(object sender, EventArgs e)
    {
        if (this._thumbnailUpload.HasFile)
        {
            try
            {
                Bitmap b = new Bitmap(new MemoryStream(this._thumbnailUpload.FileBytes));
            }
            catch (ArgumentException)
            {
                this._invalidPictureError.Visible = true;
                return;
            }
        }
        
        Album album = AlbumManager.CreateAlbum(this._nameTextBox.Text);

        if (this._thumbnailUpload.HasFile)
        {
            try
            {
                album.SetThumbnail(this._thumbnailUpload.FileBytes);
            }
            catch (ArgumentException)
            {
                // sink error
            }
        }

        if (chkSaveToSpaces.Checked)
        {
            PhotoToken _token = UserManager.LoggedInUser.PhotoPermissionToken;

            if (_token != null)
            {
                LiveNet.Photos.LivePhotos _livePhotos = new LiveNet.Photos.LivePhotos(_token.OwnerHandle, _token.DomainAuthenticationToken, LiveNet.Authentication.AuthenticationToken.DomainAuthenticationToken);

                // Create the album
                _livePhotos.CreateAlbum(this._nameTextBox.Text);
            }
        }

        if (!String.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            Group group = GroupManager.GetGroup(Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
            group.Associate(album);
            if (EventManager.EventExists(group.BaseItemID))
            {
                Response.Redirect(WebUtilities.GetViewItemUrl(group as Event));
            }
            else
            {
                Response.Redirect(WebUtilities.GetViewItemUrl(group));
            }
        }
        else
        {
            Response.Redirect(WebUtilities.GetViewItemUrl(album));
        }
    }
}
