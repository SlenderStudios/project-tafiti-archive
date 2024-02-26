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
using System.Web.Services;
using System.Collections.Generic;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Friend_Share : System.Web.UI.Page
{
    private int _baseItemID;

    private bool _sentInvites
    {
        get
        {
            bool? sent = this.ViewState[WebConstants.ViewStateVariables.SentInvites] as bool?;
            return sent.GetValueOrDefault(false);
        }
        set { this.ViewState[WebConstants.ViewStateVariables.SentInvites] = value; }
    }

    private Group GroupItem
    {
        get { return BaseItemManager.GetBaseItem(this._baseItemID) as Group; }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        this.RegisterRequiresControlState(this);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            using (InvitationHelper invitationHelper = new InvitationHelper())
            {
                invitationHelper.ClearInviteList();
            }
        }

        if (!Int32.TryParse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID], out this._baseItemID))
        {
            HealthMonitoringManager.LogWarning("Share.aspx reached without group or event ID from {0}", this.Request.UrlReferrer);
            Response.Redirect("~/");
        }

        this.Title = string.Format("Share {0}", this.GroupItem.GetType().Name);
        
        this._friendsPanel.Visible = (FriendHelper.GetFriendSummary().Count > 0);

        if (!this.IsPostBack)
        {
            this._friendsCheckList.DataBind();
            this._specialGroupsDataList.DataSource = SettingsWrapper.SpecialGroups;
            this._specialGroupsDataList.DataBind();
        }
    }

    protected void _refreshGrid_Click(object sender, EventArgs e)
    {
        this._inviteWizard.MoveTo(this._finishStep);
    }

    [WebMethod]
    static public void ReceiveContactData(object[] contacts)
    {
        using (InvitationHelper invitationHelper = new InvitationHelper())
        {
            foreach (Dictionary<string, object> contact in contacts)
            {
                if (!contact.ContainsKey("email"))
                {
                    continue;
                }

                string email = (string)contact["email"];
                if (String.Compare(email, UserManager.LoggedInUser.Email, true) == 0)
                {
                    continue;
                }

                object name;
                if (!contact.TryGetValue("name", out name))
                {
                    name = email;
                }

                invitationHelper.AddInvite(email, (string)name);
            }
        }

        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected void _finishStep_Activate(object sender, EventArgs e)
    {
        List<string> emails = new List<string>();

        using (InvitationHelper invitationHelper = new InvitationHelper())
        {
            foreach (string email in this._email.Text.Split(new char[] { '\r', '\n', ' ', ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                invitationHelper.AddInvite(email, email);
            }
        }

        foreach (ListItem item in this._friendsCheckList.Items)
        {
            if (item.Selected)
            {
                User user = UserManager.GetUserByUserName(item.Value);
                GroupManager.AddUserToGroup(user, this.GroupItem);
                emails.Add(user.Email);
            }
        }

        foreach (ListItem item in this._groupCheckList.Items)
        {
            if (item.Selected)
            {
                foreach (User user in ((Group) BaseItemManager.GetBaseItem(int.Parse(item.Value))).Users)
                {
                    GroupManager.AddUserToGroup(user, this.GroupItem);
                    emails.Add(user.Email);
                }
            }
        }

        foreach (ListItem item in this._eventCheckList.Items)
        {
            if (item.Selected)
            {
                foreach (User user in ((Event)BaseItemManager.GetBaseItem(int.Parse(item.Value))).Users)
                {
                    GroupManager.AddUserToGroup(user, this.GroupItem);
                    emails.Add(user.Email);
                }
            }
        }

        foreach (DataListItem dataItem in this._specialGroupsDataList.Items)
        {
            foreach (ListItem item in ((CheckBoxList)dataItem.FindControl("_specialGroupCheckList")).Items)
            {
                if (item.Selected)
                {
                    foreach (User user in UserManager.GetAllUsersForGroup(Int32.Parse(item.Value), UserGroupStatus.Joined))
                    {
                        if (!GroupManager.HasUserJoinedGroup(user, this.GroupItem))
                        {
                            GroupManager.AddUserToGroup(user, this.GroupItem);
                            emails.Add(user.Email);
                        }
                    }
                }
            }
        }

        using (InvitationHelper invitationHelper = new InvitationHelper())
        {
            emails.AddRange(invitationHelper.GetInviteList().Keys);

            invitationHelper.ClearInviteList();
        }

        if (emails.Count > 0)
        {
            this._sentInvites = true;
        }

        string name = this.GroupItem.Title;
        string query = String.Format("{0}={1}", WebConstants.QueryVariables.BaseItemID, this._baseItemID);
        string path = null;
        string itemType = null;

        if (this.GroupItem is Event)
        {
            path = "~/Event/ViewEvent.aspx";
            itemType = "event";
        }
        else
        {
            path = "~/Group/ViewGroup.aspx";
            itemType = "group";
        }

        UriBuilder itemUrlBuilder = new UriBuilder(HttpContext.Current.Request.Url);
        itemUrlBuilder.Path = VirtualPathUtility.ToAbsolute(path);
        itemUrlBuilder.Query = query;
        string itemUrl = itemUrlBuilder.ToString();

        CommunicationManager.SendMessage(emails, 
            string.Format("{0} has invited you to join {1}", UserManager.LoggedInUser.Title, name),
            itemUrl);

        foreach (string email in emails)
        {
            User user;
            if (!UserManager.TryGetUserByEmail(email, out user))
            {
                GroupManager.CreatePendingEmailInvite(this._baseItemID, email);
            }
        }
        
        this._itemName.Text = name;
        this._itemLink.NavigateUrl = itemUrl;
        this._itemType.Text = itemType;
        if (!this._sentInvites)
        {
            this._successPanel.Visible = false;
            this._failurePanel.Visible = true;
        }
    }

    protected void _friendsStep_Activate(object sender, EventArgs e)
    {
        if (FriendManager.GetFriends().Count == 0)
        {
            this._inviteWizard.MoveTo(this._finishStep);
        }
    }

    protected void _inviteWizard_Cancel(object sender, EventArgs e)
    {
        this._inviteWizard.ActiveStepIndex++;
        if (this._inviteWizard.ActiveStepIndex == 3)
        {
            this._inviteWizard.DisplayCancelButton = false;
        }
    }
}
