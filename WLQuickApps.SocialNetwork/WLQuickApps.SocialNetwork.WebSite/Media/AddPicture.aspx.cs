using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.SocialNetwork.Business;
using System.Net;
using System.Drawing;
using System.IO;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Media_AddPhoto : System.Web.UI.Page
{
    private string _pictureTempName;

    private byte[] PictureBits
    {
        get
        {
            return this.Cache[this._pictureTempName] as byte[];
        }
        set
        {
            this.Cache.Remove(this._pictureTempName);

            if (value != null)
            {
                this.Cache.Insert(this._pictureTempName, value, null, Cache.NoAbsoluteExpiration,
                    TimeSpan.FromMinutes(this.Session.Timeout), CacheItemPriority.NotRemovable, null);
            }
        }
    }

    private string PhotoFileName
    {
        get
        {
            if (ViewState["FileName"] == null) return string.Empty;
            return ViewState["FileName"].ToString();

        }
        set
        {
            ViewState["FileName"] = value;
        }
    }

    private void VerifyPictureAuthentication()
    {
        // show the no permissions hyperlink
        pnlNoPhotoPermissions.Visible = (UserManager.LoggedInUser.PhotoPermissionToken == null);

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        this.RegisterRequiresControlState(this);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._pictureTempName = string.Format("{0}${1}", WebConstants.CacheKeys.Picture, Guid.NewGuid());
        }

        VerifyPictureAuthentication();

    }

    protected override void LoadControlState(object savedState)
    {
        Hashtable stateData = (Hashtable)savedState;

        base.LoadControlState(stateData[WebConstants.ControlStateVariables.BaseState]);
        this._pictureTempName = stateData[WebConstants.ControlStateVariables.PictureTempName] as string;
    }

    protected override object SaveControlState()
    {
        Hashtable stateData = new Hashtable(2);

        stateData[WebConstants.ControlStateVariables.BaseState] = base.SaveControlState();
        stateData[WebConstants.ControlStateVariables.PictureTempName] = this._pictureTempName;

        return stateData;
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

    protected void _importPicture_Click(object sender, EventArgs e)
    {
        using (WebClient wc = new WebClient())
        {
            this.PictureBits = wc.DownloadData(this._pictureUrl.Value);
        }

        this._addPictureWizard.MoveTo(this._pictureDetailsStep);
    }

    protected void _albumDropDownList_DataBound(object sender, EventArgs e)
    {
        if (Request.QueryString[WebConstants.QueryVariables.BaseItemID] != null)
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

    protected void _addPictureWizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (e.CurrentStepIndex == 0)
        {

            this._addPictureWizard.ActiveStepIndex = (e.NextStepIndex + int.Parse(this._pictureTypeList.SelectedValue));

        }
        else if (e.CurrentStepIndex == 1)
        {

            this.PictureBits = this._pictureFileUpload.FileBytes;
            
            this._addPictureWizard.ActiveStepIndex = (e.NextStepIndex + 1);
            
            PhotoFileName = _pictureFileUpload.FileName;
            
            try
            {
                Bitmap bitmap = new Bitmap(new MemoryStream(this.PictureBits));
            }
            catch (ArgumentException)
            {
                e.Cancel = true;
                this._uploadError.Visible = true;
            }

            // enable/disable the target saving location buttons
            SetupTargetButtons();

        }
        else if (e.CurrentStepIndex == 2)
        {

            this._importPictureLabel.Visible = true;

            e.Cancel = true;

        }
    }

    /// <summary>
    /// Enables/disables the spaces radio buttons
    /// </summary>
    private void SetupTargetButtons()
    {
        ListItem _spacesRadioButton;

        // does the token exist for the user (i.e. has the user granted permission)
        if (UserManager.LoggedInUser.PhotoPermissionToken == null)
        {
            // User has not granted permissions

            // Get the radio button
            _spacesRadioButton = _rdoStorage.Items.FindByText("Windows Live Spaces");

            // Does it exist?
            if (_spacesRadioButton == null)
            {
                // no - refind it with a new string
                _spacesRadioButton = _rdoStorage.Items.FindByText("Windows Live Spaces - click to enable");
            }

            // does it exist?
            if (_spacesRadioButton != null)
            {
                // yes - ask the user to click to enable
                _spacesRadioButton.Text = "Windows Live Spaces - click to enable";

                // add the event which will open a window to the PGUX
                _spacesRadioButton.Attributes.Add("onClick", "openPGUX(true);");
            }
        }
        else
        {
            // Permissions have been granted

            // find the control if permissions have just been granted
            _spacesRadioButton = _rdoStorage.Items.FindByText("Windows Live Spaces - click to enable");

            // does it exist?
            if (_spacesRadioButton != null)
            {
                // reset the text on the control
                _spacesRadioButton.Text = "Windows Live Spaces (permission granted)";
            }
        }
    }

    protected void _addPictureWizard_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        this._addPictureWizard.ActiveStepIndex = 0;
    }

    protected void _importPictureRequired_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = !string.IsNullOrEmpty(this._pictureUrl.Value);
    }

    protected void _addPictureWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        // was the image uploaded
        if ((this.PictureBits == null) || (this.PictureBits.Length == 0))
        {
            // no - throw exception
            throw new InvalidOperationException("No picture was uploaded or imported.");
        }

        Media media = null;
        Album album = null;

        // was the user creating an album
        if (this._createAlbumText.Visible)
        {
            // yes - create the album
            album = AlbumManager.CreateAlbum(this._createAlbumText.Text);
        }
        else
        {
            // no - get the album
            album = AlbumManager.GetAlbum(int.Parse(this._albumDropDownList.SelectedValue));
        }

        // Is the media being persisted to spaces?
        if (_rdoStorage.SelectedIndex == 1)
        {
            media = MediaManager.CreatePicture(this.PictureBits, album.BaseItemID, album.Title, this._captionTextBox.Text, this._descriptionTextBox.Text, this._locationControl.LocationItem, PhotoFileName, string.Empty);
        }
        else
        {
            media = MediaManager.CreatePicture(this.PictureBits, album.BaseItemID, this._captionTextBox.Text, this._descriptionTextBox.Text);
        }

        if (!album.HasThumbnail)
        {
            album.SetThumbnail(this.PictureBits);
        }

        this.PictureBits = null;
        this.Response.Redirect(WebUtilities.GetViewItemUrl(media));
    }
}