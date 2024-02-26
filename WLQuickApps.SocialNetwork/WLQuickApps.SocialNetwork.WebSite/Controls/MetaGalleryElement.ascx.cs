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
using System.ComponentModel;
using WLQuickApps.SocialNetwork.WebSite;
using AjaxControlToolkit;

public partial class MetaGalleryElement : System.Web.UI.UserControl, MetaGallery.IMetaGalleryElement
{
    [Bindable(true)]
    [ToolboxItem(true)]
    public object DataItem
    {
        get { return this._dataItem; }
        set { this._dataItem = (value as BaseItem); }
    }
    private BaseItem _dataItem;

    [ToolboxItem(true)]
    public GalleryViewMode ViewMode
    {
        get { return this._viewMode; }
        set { this._viewMode = value; }
    }
    private GalleryViewMode _viewMode;

    private FormView[] _formViews;

    public MetaGalleryElement()
    {
        this._viewMode = GalleryViewMode.Square;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public override void DataBind()
    {
        base.DataBind();

        if (this._dataItem != null)
        {
            this._formViews = new FormView[] { this._squareViewForm, this._listViewForm, this._iconViewForm, 
                this._albumViewForm, this._eventViewForm, this._groupViewForm, this._mediaViewForm , this._userViewForm, 
                this._textViewForm, this._thumbnailViewForm, this._mobileViewForm };
            this._formViews[(int)this._viewMode].DataSource = new BaseItem[] { this._dataItem };
            this._formViews[(int)this._viewMode].DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        this._elementView.ActiveViewIndex = (int)this._viewMode;

        base.OnPreRender(e);
    }

    protected void _joinLink_PreRender(object sender, EventArgs e)
    {
        LinkButton joinLink = (LinkButton)sender;

        if (this._dataItem is User)
        {
            joinLink.ToolTip = "Add Friend";
        }
    }

    protected void _joinConfirm_PreRender(object sender, EventArgs e)
    {
        ConfirmButtonExtender joinConfirm = (ConfirmButtonExtender)sender;

        if (this._dataItem is User)
        {
            joinConfirm.ConfirmText = string.Format("Are you sure you want to send a friend request to {0}?",
                ((User)this._dataItem).Title);
        }
    }

    protected void _leaveLink_PreRender(object sender, EventArgs e)
    {
        LinkButton leaveLink = (LinkButton)sender;

        if (this._dataItem is User)
        {
            leaveLink.ToolTip = "Remove Friend";
        }
    }

    protected void _leaveConfirm_PreRender(object sender, EventArgs e)
    {
        ConfirmButtonExtender leaveConfirm = (ConfirmButtonExtender)sender;

        if (this._dataItem is User)
        {
            leaveConfirm.ConfirmText = string.Format("Are you sure you want to remove your friendship with {0}?",
                ((User)this._dataItem).Title);
        }
    }

    public string GetItemType()
    {
        if (this._dataItem == null)
        {
            return string.Empty;
        }
        if (this._dataItem is Media)
        {
            return ((Media)this._dataItem).MediaType.ToString();
        }
        return this._dataItem.GetType().Name;
    }

    protected string FormatLastSeen(DateTime timestamp)
    {
        string day;
        string time;

        TimeSpan difference = DateTime.Now.Date - timestamp.Date;

        if (difference.TotalSeconds < 0)
        {
            return "Online Now";
        }

        if (DateTime.Now.Date == timestamp.Date)
        {
            day = "Today";
        }
        else if (difference.Days == 1)
        {
            day = "Yesterday";
        }
        else if (difference.Days < 7)
        {
            day = String.Format("{0} days ago", difference.Days);
        }
        else
        {
            day = timestamp.Date.ToShortDateString();
        }

        if (((DateTime.Now.Hour - timestamp.Hour) < 2) && (day == "Today"))
        {
            time = "Just Now";
        }
        else
        {
            time = timestamp.ToShortTimeString();
        }

        return String.Format("{0}, {1}", day, time);
    }

    #region Actions

    protected void _delete_Click(object sender, EventArgs e)
    {
        this._dataItem.Delete();
        this._dataItem = null;
        this.Visible = false;

        this.RaiseBubbleEvent(this, GalleryElementModifiedEventArgs.Empty);
    }

    protected void _edit_Click(object sender, EventArgs e)
    {
        Response.Redirect(WebUtilities.GetEditItemUrl(this._dataItem));
    }

    protected void _approve_Click(object sender, EventArgs e)
    {
        this.AddUser();

        this.RaiseBubbleEvent(this, GalleryElementModifiedEventArgs.Empty);
    }

    protected void _reject_Click(object sender, EventArgs e)
    {
        this.RemoveUser();

        this.RaiseBubbleEvent(this, GalleryElementModifiedEventArgs.Empty);
    }

    protected void _cancel_Click(object sender, EventArgs e)
    {
        this.RemoveUser();

        this.RaiseBubbleEvent(this, GalleryElementModifiedEventArgs.Empty);
    }

    protected void _join_Click(object sender, EventArgs e)
    {
        this.AddUser();

        this.RaiseBubbleEvent(this, GalleryElementModifiedEventArgs.Empty);
    }

    protected void _leave_Click(object sender, EventArgs e)
    {
        this.RemoveUser();

        this.RaiseBubbleEvent(this, GalleryElementModifiedEventArgs.Empty);
    }

    private void AddUser()
    {
        UriBuilder uriBuilder = new UriBuilder(this.Request.Url);
        uriBuilder.Path = VirtualPathUtility.ToAbsolute("~/User/Dashboard.aspx");
        uriBuilder.Query = string.Empty;

        if (this._dataItem is Group)
        {
            Group targetGroup = this._dataItem as Group;

            string joinText = "has joined";
            if ((targetGroup.PrivacyLevel == PrivacyLevel.Private) && !GroupManager.IsUserInvitedToGroup(UserManager.LoggedInUser, targetGroup))
            {
                joinText = "has requested to join";
            }

            CommunicationManager.SendMessage(targetGroup.Owner.Email,
                string.Format("{0} {1} {2}", UserManager.LoggedInUser.Title, joinText, targetGroup.Title),
                uriBuilder.ToString());

            GroupManager.AddUserToGroup(UserManager.LoggedInUser, (Group)this._dataItem);
        }
        else if (this._dataItem is User)
        {
            User targetUser = this._dataItem as User;
            CommunicationManager.SendMessage(targetUser.Email,
                string.Format("{0} has added you as a friend on {1}", UserManager.LoggedInUser.Title, WLQuickApps.SocialNetwork.WebSite.SettingsWrapper.SiteTitle),
                uriBuilder.ToString());

            FriendManager.AddFriend((User)this._dataItem);
        }
    }

    private void RemoveUser()
    {
        if (this._dataItem is Group)
        {
            GroupManager.RemoveUserFromGroup(UserManager.LoggedInUser, (Group)this._dataItem);
        }
        else if (this._dataItem is User)
        {
            FriendManager.RemoveFriendship((User)this._dataItem);
        }
    }

    #endregion Actions
}