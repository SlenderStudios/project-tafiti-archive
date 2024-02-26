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

public partial class Collection_ViewCollections : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            BaseItem baseItem = BaseItemManager.GetBaseItem(Convert.ToInt32(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]));

            this._collectionsGallery.DataSourceID = this._baseItemCollectionsDataSource.ID;

            this._associateCollectionLink.Visible = baseItem.CanAssociate;
            this._associateCollectionLink.NavigateUrl = String.Format("~/Collection/AssociateCollection.aspx?{0}={1}", 
                WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
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

            if (user == null) { FormsAuthentication.RedirectToLoginPage(); }
            
            this._titleLabel.Text = user == UserManager.LoggedInUser ? "My Maps & Places" : "Maps & Places";
            this._collectionsGallery.DataSourceID = this._userCollectionsDataSource.ID;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }
}
