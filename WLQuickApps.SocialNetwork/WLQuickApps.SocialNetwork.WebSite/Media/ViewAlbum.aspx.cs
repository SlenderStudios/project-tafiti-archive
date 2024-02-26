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

public partial class Media_ViewAlbum : System.Web.UI.Page
{
    protected override void OnPreRender(EventArgs e)
    {
        int albumID = Int32.Parse(Request.QueryString[WebConstants.QueryVariables.BaseItemID]);

        int pictureCount = MediaManager.GetMediaCountByAlbum(albumID, MediaType.Picture);
        int videoCount = MediaManager.GetMediaCountByAlbum(albumID, MediaType.Video);
        int audioCount = MediaManager.GetMediaCountByAlbum(albumID, MediaType.Audio);

        // TODO: Add support for files.
        this._pictureTabPanel.HeaderText = string.Format("Pictures ({0})", pictureCount);
        this._videoTabPanel.HeaderText = string.Format("Video ({0})", videoCount);
        this._audioTabPanel.HeaderText = string.Format("Audio ({0})", audioCount);

        if (AlbumManager.CanModifyAlbum(albumID))
        {
            this._noAudio.Visible = (audioCount == 0);
            this._noPictures.Visible = (pictureCount == 0);
            this._noVideo.Visible = (videoCount == 0);
            
            this._addAudioHyperLink.NavigateUrl = string.Format("~/Media/AddAudio.aspx?{0}={1}",
                WebConstants.QueryVariables.BaseItemID, albumID);
            this._addPictureHyperLink.NavigateUrl = string.Format("~/Media/AddPicture.aspx?{0}={1}",
                WebConstants.QueryVariables.BaseItemID, albumID);
            this._addVideoHyperLink.NavigateUrl = string.Format("~/Media/AddVideo.aspx?{0}={1}",
                WebConstants.QueryVariables.BaseItemID, albumID);
        }
        else
        {
            this._addAudioHyperLink.Visible = false;
            this._addPictureHyperLink.Visible = false;
            this._addVideoHyperLink.Visible = false;

            if (pictureCount + videoCount + audioCount == 0)
            {
                this._albumTabContainer.Visible = false;
                this._noMedia.Visible = true;
            }
            else
            {
                if (pictureCount == 0) { this._pictureTabPanel.Enabled = false; }
                if (videoCount == 0) { this._videoTabPanel.Enabled = false; }
                if (audioCount == 0) { this._audioTabPanel.Enabled = false; }
            }
        }

        if (pictureCount == 0)
        {
            if (videoCount > 0)
            {
                this._albumTabContainer.ActiveTab = this._videoTabPanel;
            }
            else if (audioCount > 0)
            {
                this._albumTabContainer.ActiveTab = this._audioTabPanel;
            }
        }

        base.OnPreRender(e);
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
    }  
}
