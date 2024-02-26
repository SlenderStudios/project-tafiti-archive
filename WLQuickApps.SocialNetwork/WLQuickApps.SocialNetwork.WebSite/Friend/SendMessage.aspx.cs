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
using System.Collections.Generic;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Friend_SendMessage : System.Web.UI.Page
{
    private int? _eventID;
    private int? _groupID;
    private string _userName;

    protected void Page_Init(object sender, EventArgs e)
    {
        this.RegisterRequiresControlState(this);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this._friendsCheckList.DataSource = FriendHelper.GetFriendSummary();
            this._friendsCheckList.DataBind();

            string[] groupTypes = new string[SettingsWrapper.SpecialGroups.Length + 1];
            for (int i = 0; i < SettingsWrapper.SpecialGroups.Length; i++) groupTypes[i] = SettingsWrapper.SpecialGroups[i];
            groupTypes[SettingsWrapper.SpecialGroups.Length] = string.Empty;
            this._groupsDataList.DataSource = groupTypes;
            this._groupsDataList.DataBind();

            this._eventCheckList.DataSource = EventHelper.GetEventSummary(UserManager.LoggedInUser.UserName);
            this._eventCheckList.DataBind();

            int baseItemID;
            if (Int32.TryParse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID], out baseItemID))
            {
                BaseItem baseItem = BaseItemManager.GetBaseItem(baseItemID);
                
                if (baseItem is Event)
                {
                    this._eventID = baseItemID;
                    
                    ListItem item = this._eventCheckList.Items.FindByValue(this._eventID.ToString());
                    if (!(item == null))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        // Special case of event in the past not in checklist yet
                        Event eventItem = (Event)baseItem;
                        if (eventItem.HasMember(UserManager.LoggedInUser))
                        {
                            EventSummary eventSummary = new EventSummary(eventItem);
                            ListItem listItem = new ListItem(eventSummary.DisplayName, eventItem.BaseItemID.ToString());
                            listItem.Selected = true;
                            this._eventCheckList.Items.Add(listItem);
                        }
                    }
                }
                else if (baseItem is Group)
                {
                    this._groupID = baseItemID;

                    foreach (DataListItem dataItem in this._groupsDataList.Items)
                    {
                        ListItem item = ((CheckBoxList)dataItem.FindControl("_groupCheckList")).Items.FindByValue(this._groupID.ToString());
                        if (!(item == null))
                        {
                            item.Selected = true;
                        }
                    }        
                }
            }

            string userName = this.Request.QueryString[WebConstants.QueryVariables.UserName];
            if (!string.IsNullOrEmpty(userName)) 
            {
                this._userName = userName;
                ListItem item = this._friendsCheckList.Items.FindByValue(this._userName);
                if (!(item == null))
                {
                    item.Selected = true;
                }         
            }
        }
    }

    protected override void LoadControlState(object savedState)
    {
        Dictionary<string, object> stateData = (Dictionary<string, object>)savedState;

        base.LoadControlState(stateData[WebConstants.ControlStateVariables.BaseState]);
        this._eventID = stateData[WebConstants.ControlStateVariables.EventID] as int?;
        this._groupID = stateData[WebConstants.ControlStateVariables.GroupID] as int?;
        this._userName = stateData[WebConstants.ControlStateVariables.UserName] as string;
    }

    protected override object SaveControlState()
    {
        Dictionary<string, object> stateData = new Dictionary<string, object>(3);

        stateData[WebConstants.ControlStateVariables.BaseState] = base.SaveControlState();
        stateData[WebConstants.ControlStateVariables.EventID] = this._eventID;
        stateData[WebConstants.ControlStateVariables.GroupID] = this._groupID;
        stateData[WebConstants.ControlStateVariables.UserName] = this._userName;

        return stateData;
    }

    protected void _cancelButton_Click(object sender, EventArgs e)
    {
        this.RedirectToCallingPage();
    }

    protected void _sendButton_Click(object sender, EventArgs e)
    {
        List<string> emails = new List<string>();
       
        foreach (ListItem item in this._friendsCheckList.Items)
        {
            if (item.Selected)
            {
                emails.Add(UserManager.GetUserByUserName(item.Value).UserName);
            }
        }

        foreach (DataListItem dataItem in this._groupsDataList.Items)
        {
            foreach (ListItem item in ((CheckBoxList)dataItem.FindControl("_groupCheckList")).Items)
            {
                if (item.Selected)
                {
                    foreach (User user in GroupManager.GetGroup(Convert.ToInt32(item.Value)).Users)
                    {
                        emails.Add(user.Email);
                    }
                }
            }
        }

        foreach (ListItem item in this._eventCheckList.Items)
        {
            if (item.Selected)
            {
                foreach (User user in EventManager.GetEvent(Convert.ToInt32(item.Value)).Users)
                {
                    emails.Add(user.Email);
                }
            }
        }

        if (emails.Count == 0)
        {
            this._sendMessageWizard.MoveTo(this._firstStep);
            return;
        }

        UriBuilder returnUrlBuilder = new UriBuilder(HttpContext.Current.Request.Url);
        returnUrlBuilder.Path = VirtualPathUtility.ToAbsolute("~/");

        CommunicationManager.SendMessage(emails, 
            string.Format("You have received a message from {0}: {1}", UserManager.LoggedInUser.Title, this._message.Text), 
            returnUrlBuilder.ToString());

        this.RedirectToCallingPage();
    }

    protected void RedirectToCallingPage()
    {
        if (this._groupID.HasValue)
        {
            Response.Redirect(WebUtilities.GetViewItemUrl(GroupManager.GetGroup(this._groupID.Value)));
        }
        else if (this._eventID.HasValue)
        {
            Response.Redirect(WebUtilities.GetViewItemUrl(EventManager.GetEvent(this._eventID.Value)));
        }
        else if (!string.IsNullOrEmpty(this._userName))
        {
            Response.Redirect(WebUtilities.GetViewItemUrl(UserManager.GetUserByUserName(this._userName)));
        }
    }
}
