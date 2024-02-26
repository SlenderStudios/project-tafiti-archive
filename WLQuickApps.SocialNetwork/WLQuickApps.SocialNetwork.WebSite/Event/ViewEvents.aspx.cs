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

public partial class ViewEvents : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {   
        if (!string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            BaseItem baseItem = BaseItemManager.GetBaseItem(int.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]));

            if (string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.Mode]))
            {
                this._titleLabel.Text = "Upcoming Events";
                this._eventsGallery.DataSourceID = this._baseItemEventsDataSource.ID;
                this._upcomingEventsHyperLink.Visible = false;
                this._pastEventsHyperLink.Visible = true;
                this._pastEventsHyperLink.NavigateUrl = string.Format("~/Event/ViewEvents.aspx?{0}={1}&{2}={3}",
                    WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID, WebConstants.QueryVariables.Mode, "past");
            }
            else 
            {
                this._titleLabel.Text = "Past Events";
                this._eventsGallery.DataSourceID = this._baseItemPastEventsDataSource.ID;
                this._pastEventsHyperLink.Visible = false;
                this._upcomingEventsHyperLink.Visible = true;
                this._upcomingEventsHyperLink.NavigateUrl = string.Format("~/Event/ViewEvents.aspx?{0}={1}",
                    WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            
            this._associateEventLink.Visible = baseItem.CanAssociate;
            this._associateEventLink.NavigateUrl = String.Format("~/Event/AssociateEvent.aspx?{0}={1}", 
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

            if (string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.Mode]))
            {
                this._titleLabel.Text = user == UserManager.LoggedInUser ? "My Upcoming Events" : "Upcoming Events";
                this._userEventsDataSource.SelectParameters["userName"].DefaultValue = user.UserName;
                this._eventsGallery.DataSourceID = this._userEventsDataSource.ID;
                this._upcomingEventsHyperLink.Visible = false;
                this._pastEventsHyperLink.Visible = true;
                this._pastEventsHyperLink.NavigateUrl = string.Format("~/Event/ViewEvents.aspx?{0}={1}&{2}={3}", 
                    WebConstants.QueryVariables.UserName, user.UserName, WebConstants.QueryVariables.Mode, "past");
            }
            else 
            {
                this._titleLabel.Text = user == UserManager.LoggedInUser ? "My Past Events" : "Past Events";
                this._userPastEventsDataSource.SelectParameters["userName"].DefaultValue = user.UserName;
                this._eventsGallery.DataSourceID = this._userPastEventsDataSource.ID;
                this._pastEventsHyperLink.Visible = false;
                this._upcomingEventsHyperLink.Visible = true;
                this._upcomingEventsHyperLink.NavigateUrl = string.Format("~/Event/ViewEvents.aspx?{0}={1}", 
                    WebConstants.QueryVariables.UserName, user.UserName);
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
        
    }
}
