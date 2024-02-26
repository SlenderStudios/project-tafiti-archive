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

public partial class SocialNetwork_SidebarControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BaseItem baseItem = null;
        if (!string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            try 
            {
                baseItem = BaseItemManager.GetBaseItem(Convert.ToInt32(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
            }
            catch { }
        }
        else if (!string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.UserName]))
        {
            try
            { 
                baseItem = (BaseItem)UserManager.GetUserByUserName(this.Request.QueryString[WebConstants.QueryVariables.UserName]);
            }
            catch { }
        }
        else if (UserManager.IsUserLoggedIn())
        {
            baseItem = (BaseItem)UserManager.LoggedInUser;
        }
        else 
        {
            this._loginView.Visible = true;
        }

        if (baseItem != null)
        {
             
            if (baseItem is Media)
            {
                baseItem = (BaseItem)((Media)baseItem).ParentAlbum;
            }

            this._baseItemFormView.DataSource = this._baseItemDataSource;
            this._baseItemDataSource.SelectParameters["baseItemID"].DefaultValue = baseItem.BaseItemID.ToString(); 
            this._baseItemFormView.Visible = true;
            this._baseItemFormView.DataBind();
            
            this._mainHyperLink.NavigateUrl = WebUtilities.GetViewItemUrl(baseItem);
            this._mainPanel.Visible = true;

            if (baseItem is User)
            {
                string userName = ((User)baseItem).UserName;

                this._usersPanel.Visible = true;
                this._usersHyperLink.Text = "Friends";
                this._usersHyperLink.NavigateUrl = string.Format("~/Friend/ViewUsers.aspx?{0}={1}", 
                    WebConstants.QueryVariables.UserName, userName);

                this._groupsPanel.Visible = true;
                this._groupsHyperLink.NavigateUrl = string.Format("~/Group/ViewGroups.aspx?{0}={1}", 
                    WebConstants.QueryVariables.UserName, userName);

                this._eventsPanel.Visible = true;
                this._eventsHyperLink.NavigateUrl = string.Format("~/Event/ViewEvents.aspx?{0}={1}", 
                    WebConstants.QueryVariables.UserName, userName);

                this._galleriesPanel.Visible = true;
                this._galleriesHyperLink.NavigateUrl = string.Format("~/Media/ViewAlbums.aspx?{0}={1}", 
                    WebConstants.QueryVariables.UserName, userName);
                
                this._collectionsPanel.Visible = true;
                this._collectionsHyperLink.NavigateUrl = string.Format("~/Collection/ViewCollections.aspx?{0}={1}", 
                    WebConstants.QueryVariables.UserName, userName);

                if (UserManager.IsUserLoggedIn() && UserManager.LoggedInUser.UserName == userName)
                {
                    this._usersHyperLink.Text = "My Friends";
                    this._groupsHyperLink.Text = "My Groups";
                    this._eventsHyperLink.Text = "My Events";
                    this._galleriesHyperLink.Text = "My Galleries";
                    this._collectionsHyperLink.Text = "My Maps &#38; Places";

                    this._dashboardPanel.Visible = true;
                    this._editProfilePanel.Visible = true;
                    this._calendarPanel.Visible = true;
                    
                    this._userRequestsForm.Visible = true;
                    this._userRequestsForm.DataSource = new User[] { UserManager.LoggedInUser };
                    this._userRequestsForm.DataBind();
                }
            }
            else if (baseItem is Group || baseItem is Event)
            {
                this._usersPanel.Visible = true;
                this._usersHyperLink.Text = baseItem is Event ? "Attendees" : "Members";
                this._usersHyperLink.NavigateUrl = string.Format("~/Friend/ViewUsers.aspx?{0}={1}", 
                    WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);

                this._eventsPanel.Visible = true;
                this._eventsHyperLink.NavigateUrl = string.Format("~/Event/ViewEvents.aspx?{0}={1}", 
                    WebConstants.QueryVariables.BaseItemID,baseItem.BaseItemID);
                
                this._galleriesPanel.Visible = true;
                this._galleriesHyperLink.NavigateUrl = string.Format("~/Media/ViewAlbums.aspx?{0}={1}", 
                    WebConstants.QueryVariables.BaseItemID,baseItem.BaseItemID);
                
                this._collectionsPanel.Visible = true;
                this._collectionsHyperLink.NavigateUrl = string.Format("~/Collection/ViewCollections.aspx?{0}={1}", 
                    WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
        }
      
        // Set the CssClass of the selected sidebar link
        string currentPage = this.Request.Path.Substring(this.Request.Path.LastIndexOf('/') + 1);
        string cssSidebarSelected = "sidebar-selected";

        switch (currentPage)
        {
            case "ViewProfile.aspx":
            case "ViewGroup.aspx":
            case "ViewEvent.aspx":
            case "ViewAlbum.aspx":
            case "ViewCollection.aspx":
                this._mainPanel.CssClass = cssSidebarSelected;
                break;
            case "ViewUsers.aspx":
                this._usersPanel.CssClass = cssSidebarSelected;
                break;
            case "ViewGroups.aspx":
                this._groupsPanel.CssClass = cssSidebarSelected;
                break;
            case "ViewEvents.aspx":
                this._eventsPanel.CssClass = cssSidebarSelected;
                break;
            case "ViewAlbums.aspx":
                this._galleriesPanel.CssClass = cssSidebarSelected;
                break;
            case "ViewCollections.aspx":
                this._collectionsPanel.CssClass = cssSidebarSelected;
                break;
            case "ViewCalendar.aspx":
                this._calendarPanel.CssClass = cssSidebarSelected;
                break;
            case "Dashboard.aspx":
                this._dashboardPanel.CssClass = cssSidebarSelected;
                break;
            case "EditProfile.aspx":
                this._editProfilePanel.CssClass = cssSidebarSelected;
                break;
        }
    }
}
