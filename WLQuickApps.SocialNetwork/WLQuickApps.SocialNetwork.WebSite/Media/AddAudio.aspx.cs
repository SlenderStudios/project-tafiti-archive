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
using WLQuickApps.SocialNetwork.WebSite;

public partial class Media_AddAudio : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this._uploadError.Visible = false;
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (AlbumManager.GetAllAlbumsCount() == 0)
        {
            this._albumDropDownList.Visible = false;
            this._createAlbumText.Visible = true;
            this._galleryLabel.Text = "New Gallery Name";
        }

        base.OnPreRender(e);
    }

    protected void _uploadButton_Click(object sender, EventArgs e)
    {
        Media media = null;
        Album album = null;
        if (this._createAlbumText.Visible)
        {
            album = AlbumManager.CreateAlbum(this._createAlbumText.Text);
            media = MediaManager.CreateAudio(this._audioFileUpload.FileName, this._audioFileUpload.FileBytes, album.BaseItemID, this._captionTextBox.Text, this._descriptionTextBox.Text, this._locationControl.LocationItem);
        }
        else
        {
            media = MediaManager.CreateAudio(this._audioFileUpload.FileName, this._audioFileUpload.FileBytes, int.Parse(this._albumDropDownList.SelectedValue), this._captionTextBox.Text, this._descriptionTextBox.Text, this._locationControl.LocationItem);
        }

        if (this._thumbnailFileUpload.FileBytes.Length > 0)
        {
            try
            {
                media.SetThumbnail(this._thumbnailFileUpload.FileBytes);

                if (album == null)
                {
                    album = AlbumManager.GetAlbum(int.Parse(this._albumDropDownList.SelectedValue));
                }
                if (!album.HasThumbnail)
                {
                    album.SetThumbnail(this._thumbnailFileUpload.FileBytes);
                }
            }
            catch (ArgumentException)
            {
                this._uploadError.Visible = true;
                media.Delete();
                return;
            }
        }

        Response.Redirect(WebUtilities.GetViewItemUrl(media));
    }

    protected void _albumDropDownList_DataBound(object sender, EventArgs e)
    {
        if (this.Request.QueryString[WebConstants.QueryVariables.BaseItemID] != null)
        {
            ListItem li = _albumDropDownList.Items.FindByValue(Request.QueryString[WebConstants.QueryVariables.BaseItemID]);
            if (li != null)
            {
                li.Selected = true;
            }
        }

        this._albumDropDownList.Items.Add(new ListItem("Add to New Gallery...", string.Empty));
    }

    protected void _albumDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        this._createAlbumText.Visible = string.IsNullOrEmpty(this._albumDropDownList.SelectedValue);
    }
}
