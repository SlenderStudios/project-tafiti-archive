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

public partial class Friend_RemoveFriend : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void _removeFriendButton_Click(object sender, EventArgs e)
    {
        User requestedUser = UserManager.GetUserByUserName(Request.QueryString[WebConstants.QueryVariables.UserName]);
        if (UserManager.IsUserLoggedIn() && UserManager.LoggedInUser.UserID != requestedUser.UserID)
        {
            FriendManager.RemoveFriendship(requestedUser);
        }
        Response.Redirect(WebUtilities.GetViewItemUrl(requestedUser));
    }
}
