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

public partial class Friend_ViewUsers : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            BaseItem baseItem = BaseItemManager.GetBaseItem(int.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]));

            this._friendsGallery.DataSourceID = this._attendeesDataSource.ID; 
            this._titleLabel.Text = baseItem is Event ? "Attendees" : "Members";
        }
        else 
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
           
            this._friendsGallery.DataSourceID = this._friendsDataSource.ID;
            this._friendsDataSource.SelectParameters.Add(new Parameter("userID", TypeCode.Object, user.UserID.ToString()));
            this._titleLabel.Text = user == UserManager.LoggedInUser ? "My Friends" : "Friends";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

}
