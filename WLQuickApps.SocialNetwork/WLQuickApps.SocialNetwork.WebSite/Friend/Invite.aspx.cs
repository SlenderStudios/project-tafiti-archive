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
using System.Web.Services;
using System.Web.Script.Services;
using System.Collections.Generic;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Friend_Invite : System.Web.UI.Page
{
    private bool _sentInvites
    {
        get
        {
            bool? sent = this.ViewState[WebConstants.ViewStateVariables.SentInvites] as bool?;
            return sent.GetValueOrDefault(false);
        }
        set { this.ViewState[WebConstants.ViewStateVariables.SentInvites] = value; }
    }

    protected void _emailAvailable_ServerValidate(object source, ServerValidateEventArgs args)
    {
        User user;
        args.IsValid = !(UserManager.TryGetUserByEmail(args.Value, out user));
    }

    protected void _refreshGrid_Click(object sender, EventArgs e)
    {
        this._inviteWizard_NextButtonClick(sender, new WizardNavigationEventArgs(1, 2));
    }

    /* Commented as part of Work Item 2441 - http://www.codeplex.com/WLQuickApps/WorkItem/View.aspx?WorkItemId=2441
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
    */

    [WebMethod]
    static public void PopulateDemoContactData()
    {
        using (InvitationHelper invitationHelper = new InvitationHelper())
        {
            invitationHelper.AddInvite("cconnell@hotmail.com", "Craig Connell");
            invitationHelper.AddInvite("rdavis@hotmail.com", "Richard Davis");
        }

        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
    }

    protected void _inviteWizard_CancelClick(object sender, EventArgs e)
    {
        this._inviteWizard_NextButtonClick(this._inviteWizard, new WizardNavigationEventArgs(this._inviteWizard.ActiveStepIndex, this._inviteWizard.ActiveStepIndex + 1));
        if (this._inviteWizard.ActiveStepIndex == 0)
        {
            this._inviteWizard.ActiveStepIndex++;
        }
    }

    protected void _inviteWizard_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        // Constants which make it easier to manage the steps
        int STEP_INDEX_InviteByEmail = 0;
        // REMOVED re work item 2441 http://www.codeplex.com/WLQuickApps/WorkItem/View.aspx?WorkItemId=2441
        // int STEP_INDEX_InviteByWindowsLive = 1;
        int STEP_INDEX_CheckIfExistingUser = 1;
        int STEP_INDEX_Finish = 2;
        int STEP_INDEX_Complete = 3;

        if (e.CurrentStepIndex == STEP_INDEX_InviteByEmail)
        {
            using (InvitationHelper invitationHelper = new InvitationHelper())
            {
                foreach (string email in this._email.Text.Split(new char[] { '\r', '\n', ' ', ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    invitationHelper.AddInvite(email, email);
                }
            }

            if (!this.ProcessInvitees())
            {
                this._inviteWizard.ActiveStepIndex = STEP_INDEX_InviteByEmail;
            }

            if (this._nonFriendsList.Items.Count == 0)
            {
                this._inviteWizard.ActiveStepIndex = STEP_INDEX_Finish;
            }
            else
            {
                this._inviteWizard.ActiveStepIndex = STEP_INDEX_CheckIfExistingUser;
            }
        }

        //Removed re. Work Item http://www.codeplex.com/WLQuickApps/WorkItem/View.aspx?WorkItemId=2441
        //else if (e.CurrentStepIndex == STEP_INDEX_InviteByWindowsLive)
        //{
        //    if (!this.ProcessInvitees())
        //    {
        //        this._inviteWizard.ActiveStepIndex = STEP_INDEX_InviteByEmail;
        //    }

        //    if (this._nonFriendsList.Items.Count == 0)
        //    {
        //        this._inviteWizard.ActiveStepIndex = STEP_INDEX_Finish;
        //    }
        //    else
        //    {
        //        this._inviteWizard.ActiveStepIndex = STEP_INDEX_CheckIfExistingUser;
        //    }
        //}
        if (((e.CurrentStepIndex == STEP_INDEX_CheckIfExistingUser) || (this._inviteWizard.ActiveStepIndex == STEP_INDEX_Finish)) && (this._unregisteredList.Items.Count == 0))
        {
            this._inviteWizard.ActiveStepIndex = STEP_INDEX_Complete;
        }

        this._inviteWizard.DisplayCancelButton = this._inviteWizard.ActiveStepIndex < STEP_INDEX_Finish;
    }

    private bool ProcessInvitees()
    {
        using (InvitationHelper invitationHelper = new InvitationHelper())
        {
            if (invitationHelper.GetInviteList().Count == 0)
            {
                return false;
            }

            foreach (KeyValuePair<string, string> invitee in invitationHelper.GetInviteList())
            {
                User user;
                if (UserManager.TryGetUserByEmail(invitee.Key, out user))
                {
                    if (user.Email == UserManager.LoggedInUser.Email)
                    {
                        continue;
                    }

                    if (!FriendManager.ConfirmFriendship(UserManager.LoggedInUser, user))
                    {
                        this._nonFriendsList.Items.Add(new ListItem(invitee.Value, user.UserName));
                    }
                }
                else
                {
                    this._unregisteredList.Items.Add(new ListItem(
                        String.Format("{0} ({1})", invitee.Value, invitee.Key), invitee.Key));
                }
            }
            foreach (ListItem item in this._nonFriendsList.Items) item.Selected = true;
            foreach (ListItem item in this._unregisteredList.Items) item.Selected = true;

            if (invitationHelper.GetInviteList().Count > 0)
            {
                this._sentInvites = true;
            }

            invitationHelper.ClearInviteList();
        }

        return true;
    }

    protected void _inviteWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
    {
        foreach (ListItem item in this._nonFriendsList.Items)
        {
            if (item.Selected)
            {
                FriendManager.AddFriend(UserManager.GetUserByUserName(item.Value));
            }
        }

        UriBuilder returnUrlBuilder = new UriBuilder(HttpContext.Current.Request.Url);
        returnUrlBuilder.Path = VirtualPathUtility.ToAbsolute("~/");
        string returnUrl = returnUrlBuilder.ToString();

        List<string> emails = new List<string>();

        foreach (ListItem item in this._unregisteredList.Items)
        {
            if (item.Selected)
            {
                emails.Add(item.Value);
            }
        }



        CommunicationManager.SendEmail(
            emails,
            UserManager.LoggedInUser.Email,
            UserManager.LoggedInUser.Title,
            string.Format("{0} has invited you to join {1}", UserManager.LoggedInUser.Title, SettingsWrapper.SiteTitle),
            string.Format(
            @"
<html>
    <body>
        <p>{0} has invited you to join <a href=""{1}"">{2}</a>.</p>
        <p>Personal message from {0}:</p>
        <p><em>{3}</em></p>
    </body>
</html>", UserManager.LoggedInUser.Title, returnUrl, SettingsWrapper.SiteTitle, this._personalMessage.Text));
    }

    protected void _finishStep_Activate(object sender, EventArgs e)
    {
        if (!this._sentInvites)
        {
            this._completeMessage.Text = "You have not selected any friends to invite.";
        }
    }
}
