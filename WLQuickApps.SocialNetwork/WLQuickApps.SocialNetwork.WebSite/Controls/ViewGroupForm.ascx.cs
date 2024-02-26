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
using WLQuickApps.SocialNetwork.WebSite;
using System.Collections.Generic;

public enum UserStatus
{
    Nonmember,
    Invited,
    WaitingForApproval,
    Joined
}

public partial class ViewGroupForm : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e) { }

    [Bindable(true)]
    [ToolboxItem(true)]
    public Group GroupItem
    {
        get { return this._groupItem; }
        set
        {
            this._groupItem = value;
            this._datePanel.Visible = this.IsEvent();
        }
    }
    private Group _groupItem;

    protected override void OnPreRender(EventArgs e)
    {
        this.DataBind();

        if (this._approveChecked)
        {
            foreach (User user in this._selectedUsers)
                GroupManager.AddUserToGroup(user, this._groupItem);
            this.DataBind();
        }
        else if (this._rejectChecked)
        {
            foreach (User user in this._selectedUsers)
                GroupManager.RemoveUserFromGroup(user, this._groupItem);
            this.DataBind();
        }

        this._albums.DataBind();

        base.OnPreRender(e);
    }

    public bool IsEvent()
    {
        return this._groupItem is Event;
    }

    public string GetStartDateTime()
    {
        if (this.IsEvent())
        {
            return String.Format("{0:D} at {0:t}", ((Event)this.GroupItem).StartDateTime);
        }
        return string.Empty;
    }

    public string GetEndDateTime()
    {
        if (this.IsEvent())
        {
            return String.Format("{0:D} at {0:t}", ((Event)this.GroupItem).EndDateTime);
        }
        return string.Empty;
    }

    public string GetPageURL(string pageURL)
    {
        return String.Format("{0}?{1}={2}", pageURL, WebConstants.QueryVariables.BaseItemID, this.GroupItem.BaseItemID);
    }

    public string GetPageURL(string pageURL, bool escapeURL)
    {
        return this.GetPageURL(String.Format(pageURL, this.IsEvent() ? "Event" : "Group"));
    }

    public UserStatus GetUserStatus()
    {
        if (!UserManager.IsUserLoggedIn()) { return UserStatus.Nonmember; }

        if (GroupManager.HasUserRequestedToJoinGroup(UserManager.LoggedInUser, this._groupItem))
        {
            return UserStatus.WaitingForApproval;
        }
        else if (GroupManager.IsUserInvitedToGroup(UserManager.LoggedInUser, this._groupItem))
        {
            return UserStatus.Invited;
        }
        else if (GroupManager.HasUserJoinedGroup(UserManager.LoggedInUser, this._groupItem))
        {
            return UserStatus.Joined;
        }
        return UserStatus.Nonmember;
    }

    // TODO: is there a better way to do this?
    private List<User> _selectedUsers = new List<User>();
    private bool _approveChecked;
    private bool _rejectChecked;
    protected void _requesteesCheckList_DataBinding(object sender, EventArgs e)
    {
        foreach (ListItem item in this._requesteesCheckList.Items)
        {
            if (item.Selected)
            {
                this._selectedUsers.Add(UserManager.GetUserByUserName(item.Value));
            }
        }
    }

    protected void _requesteesCheckList_DataBound(object sender, EventArgs e)
    {
        if (this._requesteesCheckList.Items.Count == 0)
        {
            this._approveRequestees.Visible = false;
            this._noRequesteesLabel.Visible = true;
            this._rejectRequestees.Visible = false;
        }
    }

    protected void _approveRequestees_Click(object sender, EventArgs e)
    {
        this._approveChecked = true;
    }

    protected void _rejectRequestees_Click(object sender, EventArgs e)
    {
        this._rejectChecked = true;
    }

    protected void _joinGroupLink_Click(object sender, EventArgs e)
    {
        GroupManager.AddUserToGroup(UserManager.LoggedInUser, this._groupItem);

        UriBuilder uriBuilder = new UriBuilder(this.Request.Url);
        uriBuilder.Path = VirtualPathUtility.ToAbsolute("~/User/Dashboard.aspx");
        uriBuilder.Query = string.Empty;

        string joinText = "has joined";
        if ((this._groupItem.PrivacyLevel == PrivacyLevel.Private) && !GroupManager.IsUserInvitedToGroup(UserManager.LoggedInUser, this._groupItem))
        {
            joinText = "has requested to join";
        }

        CommunicationManager.SendMessage(this._groupItem.Owner.Email,
            string.Format("{0} {1} {2}", UserManager.LoggedInUser.Title, joinText, this._groupItem.Title),
            uriBuilder.ToString());
        
        this.DataBind();
    }

    protected void _leaveGroupLink_Click(object sender, EventArgs e)
    {
        GroupManager.LeaveGroup(this._groupItem);
        if (this.IsEvent())
        {
            Response.Redirect("~/Event/ViewCalendar.aspx");
        }
        else
        {
            Response.Redirect("~/Group/ViewGroups.aspx");
        }
    }

    protected void _deleteGroupLink_Click(object sender, EventArgs e)
    {
        if (this.IsEvent())
        {
            this.GroupItem.Delete();
            Response.Redirect("~/Event/ViewCalendar.aspx");
        }
        else
        {
            this.GroupItem.Delete();
            Response.Redirect("~/Group/ViewGroups.aspx");
        }
    }
}
