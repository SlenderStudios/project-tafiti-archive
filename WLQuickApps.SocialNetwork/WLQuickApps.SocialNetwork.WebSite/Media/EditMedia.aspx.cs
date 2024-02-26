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

public partial class Media_EditMedia : System.Web.UI.Page
{
    private int _mediaID;
    private LocationControl _locationControl;

    protected void Page_Load(object sender, EventArgs e)
    {
        this._mediaID = Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]);

    }

    protected void _locationControl_Init(object sender, EventArgs e)
    {
        this._locationControl = (LocationControl)sender;

        if (!this.IsPostBack)
        {
            this._locationControl.LocationItem = MediaManager.GetMedia(this._mediaID).Location;
        }
    }

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        Media media = MediaManager.GetMedia((int)e.Keys[0]);
        media.Title = (string)e.NewValues["Title"];
        media.Description = (string)e.NewValues["Description"];
        media.Location = this._locationControl.LocationItem;

        FileUpload fileUpload = (FileUpload)this.DetailsView1.FindControl("_newThumbnailUpload");
        if ((media.MediaType != MediaType.Picture) && fileUpload.HasFile)
        {
            try
            {
                media.SetThumbnail(fileUpload.FileBytes);
            }
            catch (ArgumentException)
            {
                this.DetailsView1.FindControl("_invalidThumbnailLabel").Visible = true;
                e.Cancel = true;
                return;
            }
        }

        MediaManager.UpdateMedia(media);

        this.RedirectToMedia();
    }

    protected void DetailsView1_ModeChanged(object sender, EventArgs e)
    {
        this.RedirectToMedia();
    }

    private void RedirectToMedia()
    {
        this.Response.Redirect(WebUtilities.GetViewItemUrl(MediaManager.GetMedia(this._mediaID)));
    }
}
