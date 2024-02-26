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

public partial class Group_Transfer : System.Web.UI.Page
{
    int _groupID;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            this._groupID = Int32.Parse(Request.QueryString[WebConstants.QueryVariables.BaseItemID]);
        }

        if (!this.IsPostBack)
        {
            List<User> transferrableUsers = GroupManager.GetGroup(this._groupID).Users;
            transferrableUsers.Remove(UserManager.LoggedInUser);

            this._groupMembersList.DataSource = FriendHelper.GetFriendSummary(transferrableUsers);
            this._groupMembersList.DataBind();
            this._groupMembersList.SelectedIndex = 0;

            if (this._groupMembersList.Items.Count == 0)
            {
                this._noPeersLabel.Visible = true;
                this._chooseMember.Visible = false;
                this._noPeersLink.NavigateUrl = Request.UrlReferrer.OriginalString;
                this._noPeersLink.Text = "(Return)";
            }
        }
    }

    protected void _chooseMember_Click(object sender, EventArgs e)
    {
        WLQuickApps.SocialNetwork.Business.Group group = GroupManager.GetGroup(this._groupID);
        group.Owner = UserManager.GetUserByUserName(this._groupMembersList.SelectedValue);
        GroupManager.UpdateGroup(group);

        UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url);
        uriBuilder.Path = VirtualPathUtility.ToAbsolute(group is Event ? "~/Event/ViewEvent.aspx" : "~/Group/ViewGroup.aspx");
        uriBuilder.Query = string.Format("{0}={1}", WebConstants.QueryVariables.BaseItemID, group.BaseItemID);

        CommunicationManager.SendMessage(group.Owner.Email,
           string.Format("{0} has transferred ownership of \"{1}\" to you on {2}",
           UserManager.LoggedInUser.Title,
           group.Title,
           WLQuickApps.SocialNetwork.WebSite.SettingsWrapper.SiteTitle),
           uriBuilder.ToString());

        Response.Redirect(WebUtilities.GetViewItemUrl(group));
    }
}
