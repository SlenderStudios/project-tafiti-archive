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
using System.ComponentModel;
using WLQuickApps.SocialNetwork.Business;
using System.Drawing;
using System.IO;
using WLQuickApps.SocialNetwork.WebSite;

public partial class AddGroupForm : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._privacyLevel.DataSource = Enum.GetNames(typeof(PrivacyLevel));
            this._privacyLevel.DataBind();

            if (this._groupItem != null)
            {
                this._privacyLevel.Items.FindByText(this._groupItem.PrivacyLevel.ToString()).Selected = true;
                this._typePanel.Visible = false;
            }
            else
            {
                this._startDate.Text = DateTime.Now.AddDays(7).ToString("MM/dd/yyyy");
                this._endDate.Text = DateTime.Now.AddDays(7).ToString("MM/dd/yyyy");
                this._privacyLevel.SelectedIndex = 0;

                string[] types;
                if (this.IsEvent)
                {
                    types = SettingsWrapper.SpecialEvents;
                }
                else
                {
                    types = SettingsWrapper.SpecialGroups;
                }

                if (types.Length > 0)
                {
                    this._typePanel.Visible = true;
                    foreach (string itemType in types)
                    {
                        this._typeDropDownList.Items.Add(new ListItem(itemType, itemType));
                    }
                    this._typeDropDownList.Items.Add(new ListItem("Other", string.Empty));
                }
            }
        }
        this._invalidPictureError.Visible = false;
    }

    #region Properties

    [Bindable(true)]
    [ToolboxItem(true)]
    public string Name
    {
        get { return this._name.Text; }
        set { this._name.Text = value; }
    }

    [Bindable(true)]
    [ToolboxItem(true)]
    public string Description
    {
        get { return this._description.Text; }
        set { this._description.Text = value; }
    }

    [Bindable(true)]
    [ToolboxItem(true)]
    public string Type
    {
        get { return this._typeDropDownList.SelectedValue; }
        set { this._typeDropDownList.SelectedValue = value; }
    }

    [Bindable(true)]
    [ToolboxItem(true)]
    public DateTime StartTime
    {
        get
        {
            return DateTime.Parse(String.Format("{0} {1}:{2} {3}",
              this._startDate.Text, this._startHour.Text, this._startMinute.Text, this._startAmPm.Text));
        }
        set
        {
            this._startDate.Text = value.ToString("MM/dd/yyyy"); 
            this._startHour.Text = value.ToString("%h");
            this._startMinute.Text = value.ToString("mm");
            this._startAmPm.Text = String.Format("{0:%t}M", value);
        }
    }

    [Bindable(true)]
    [ToolboxItem(true)]
    public DateTime EndTime
    {
        get
        {
            return DateTime.Parse(String.Format("{0} {1}:{2} {3}",
            this._endDate.Text, this._endHour.Text, this._endMinute.Text, this._endAmPm.Text));
        }
        set
        {
            this._endDate.Text = value.ToString("MM/dd/yyyy"); 
            this._endHour.Text = value.ToString("%h");
            this._endMinute.Text = value.ToString("mm");
            this._endAmPm.Text = String.Format("{0:%t}M", value);
        }
    }

    [Bindable(true)]
    [ToolboxItem(true)]
    public Location Location
    {
        get { return this._location.LocationItem; }
        set { this._location.LocationItem = value; }
    }

    [ToolboxItem(true)]
    public byte[] PictureBits
    {
        get { return this._pictureBits; }
        set { this._pictureBits = value; }
    }
    private byte[] _pictureBits = null;

    [Bindable(true)]
    [ToolboxItem(true)]
    public PrivacyLevel PrivacyLevel
    {
        get { return (PrivacyLevel) Enum.Parse(typeof(PrivacyLevel), this._privacyLevel.SelectedItem.Text); }
        set { this._privacyLevel.Items.FindByText(value.ToString()).Selected = true; }
    }

    [Bindable(true)]
    [ToolboxItem(true)]
    public Group GroupItem
    {
        set
        {
            this._groupItem = value;
            this._name.Text = this._groupItem.Title;
            this._description.Text = this._groupItem.Description;
            this._location.LocationItem = this._groupItem.Location;
            if (this._groupItem.HasThumbnail)
            {
                this._existingThumbnail.ImageUrl = WebUtilities.GetViewImageUrl(this._groupItem.BaseItemID, 128, 128);
                this._existingThumbnail.Visible = true;
                this._existingThumbnailLabel.Visible = true;
            }      
            
            
            Event eventItem = this._groupItem as Event;
            if (eventItem != null)
            {
                this._startDate.Text = eventItem.StartDateTime.ToString("MM/dd/yyyy");
                this._startHour.Text = eventItem.StartDateTime.ToString("%h");
                this._startMinute.Text = eventItem.StartDateTime.ToString("mm");
                this._startAmPm.Text = String.Format("{0:%t}M", eventItem.StartDateTime);
                this._endDate.Text = eventItem.EndDateTime.ToString("MM/dd/yyyy");
                this._endHour.Text = eventItem.EndDateTime.ToString("%h");
                this._endMinute.Text = eventItem.EndDateTime.ToString("mm");
                this._endAmPm.Text = String.Format("{0:%t}M", eventItem.EndDateTime);
            }
        }
    }
    private Group _groupItem;

    #endregion

    #region Control Properties

    [ToolboxItem(true)]
    public bool IsEvent
    {
        get { return this._isEvent; }
        set
        {
            this._isEvent = value;
            this._dateTimes.Visible = this._isEvent;
        }
    }
    private bool _isEvent = true;

    [ToolboxItem(true)]
    public bool IsEditing
    {
        get { return this._isEditing; }
        set
        {
            this._isEditing = value;
        }
    }
    private bool _isEditing = false;

    #endregion Control Properties

    #region Validation

    protected void _startTimeRequired_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (String.IsNullOrEmpty(this._startHour.Text) ||
            String.IsNullOrEmpty(this._startMinute.Text) ||
            String.IsNullOrEmpty(this._startAmPm.Text))
        {
            args.IsValid = false;
        }
    }

    protected void _startTimeFuture_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (this.IsEditing)
        {
            return;
        }
        
        if (String.IsNullOrEmpty(this._startHour.Text) ||
            String.IsNullOrEmpty(this._startMinute.Text) ||
            String.IsNullOrEmpty(this._startAmPm.Text))
        {
            return;
        }
        else
        {
            // TODO: Deal with differing timezones.
            DateTime startTime = DateTime.Parse(String.Format("{0} {1}:{2} {3}",
                this._startDate.Text, this._startHour.Text, this._startMinute.Text, this._startAmPm.Text));

            args.IsValid = (startTime > DateTime.Now);
        }
    }

    protected void _endTimeRequired_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (
            String.IsNullOrEmpty(this._endHour.Text) ||
            String.IsNullOrEmpty(this._endMinute.Text) ||
            String.IsNullOrEmpty(this._endAmPm.Text))
        {
            args.IsValid = false;
        }
    }

    protected void _endTimeFuture_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (String.IsNullOrEmpty(this._startHour.Text) ||
            String.IsNullOrEmpty(this._startMinute.Text) ||
            String.IsNullOrEmpty(this._startAmPm.Text) ||
            String.IsNullOrEmpty(this._endHour.Text) ||
            String.IsNullOrEmpty(this._endMinute.Text) ||
            String.IsNullOrEmpty(this._endAmPm.Text))
        {
            return;
        }
        else
        {
            DateTime startTime = DateTime.Parse(String.Format("{0} {1}:{2} {3}",
                this._startDate.Text, this._startHour.Text, this._startMinute.Text, this._startAmPm.Text));
            DateTime endTime = DateTime.Parse(String.Format("{0} {1}:{2} {3}",
                this._endDate.Text, this._endHour.Text, this._endMinute.Text, this._endAmPm.Text));

            args.IsValid = (endTime > startTime);
        }
    }   

    #endregion Validation

    #region Events

    [ToolboxItem(true)]
    public event EventHandler Save;

    protected void _submitButton_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid)
        {
            if (this._pictureFileUpload.HasFile)
            {
                try
                {
                    Bitmap b = new Bitmap(new MemoryStream(this._pictureFileUpload.FileBytes));
                }
                catch (ArgumentException)
                {
                    this._invalidPictureError.Visible = true;
                    return;
                }
                this._pictureBits = this._pictureFileUpload.FileBytes;
                this.OnSave();
            }

            this.OnSave();
        }
    }

    protected void OnSave()
    {
        EventHandler handler = this.Save;
        if (handler != null)
        {
            handler(this, EventArgs.Empty);
        }
    }

    #endregion
}
