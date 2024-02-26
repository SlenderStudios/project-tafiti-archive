using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Friend_AddFriend : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void _addFriendButton_Click(object sender, EventArgs e)
    {
        User requestedUser = UserManager.GetUserByUserName(Request.QueryString[WebConstants.QueryVariables.UserName]);
        if (UserManager.IsUserLoggedIn() && UserManager.LoggedInUser.UserID != requestedUser.UserID)
        {
            FriendManager.AddFriend(requestedUser);

            UriBuilder returnUrlBuilder = new UriBuilder(HttpContext.Current.Request.Url);
            returnUrlBuilder.Path = VirtualPathUtility.ToAbsolute("~/User/Dashboard.aspx");

            CommunicationManager.SendMessage(
                new string[] { requestedUser.Email }, 
                string.Format("{0} has added you as a friend on {1}", UserManager.LoggedInUser.Title, SettingsWrapper.SiteTitle), 
                returnUrlBuilder.ToString());
        }
        Response.Redirect(WebUtilities.GetViewItemUrl(requestedUser));
    }
}
