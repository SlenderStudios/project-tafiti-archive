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

public partial class Group : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        User user = null;
        if (!string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.UserName]))
        { 
            user = UserManager.GetUserByUserName(this.Request.QueryString[WebConstants.QueryVariables.UserName]);
        }
        else if (UserManager.IsUserLoggedIn())
        {
            user = UserManager.LoggedInUser;
        }

        if (user == null) { FormsAuthentication.RedirectToLoginPage(); }

        this._titleLabel.Text = user == UserManager.LoggedInUser ? "My Groups" : "Groups";   
    }
}
