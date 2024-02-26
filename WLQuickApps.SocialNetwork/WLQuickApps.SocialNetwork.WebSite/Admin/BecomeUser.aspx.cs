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
        string userName = HttpUtility.HtmlEncode(this.Request.QueryString[WebConstants.QueryVariables.UserName]);
        if (!String.IsNullOrEmpty(userName))
        {
            this._userNameTextBox.Visible = false;
            this._userNameLabel.Text = String.Format("Becoming {0}...", userName);
            this._becomeButton.Text = "Do it!";
        }
    }

    protected void _becomeButton_Click(object sender, EventArgs e)
    {
        string userName = this.Request.QueryString[WebConstants.QueryVariables.UserName];
        if (String.IsNullOrEmpty(userName))
        {
            Response.Redirect(String.Format("~/Admin/BecomeUser.aspx?userName={0}", this._userNameTextBox.Text));
        }
        else
        {
            FormsAuthentication.RedirectFromLoginPage(userName, false);
        }
    }
}
