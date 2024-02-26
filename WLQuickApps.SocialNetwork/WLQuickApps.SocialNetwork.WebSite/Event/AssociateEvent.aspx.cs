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
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Event_AssociateEvent : System.Web.UI.Page
{
    Group _groupItem = null;

    protected void Page_Load(object sender, EventArgs e) 
    {
        if (!String.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            this._groupItem = GroupManager.GetGroup(Int32.Parse(Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
            if (UserManager.IsUserLoggedIn() && !this._groupItem.CanAssociate)
            {
                this.Response.Redirect("~/Default.aspx");
            }
        }
        else
        {
            Response.Redirect("~/Group/ViewGroups.aspx");
        }
        
        this._groupName.Text = this._groupItem.Title;
        this._eventDetails.DataBind();
    }

    protected override void OnPreRender(EventArgs e)
    {
        this._alreadyAssociatedErrorLabel.Visible = this._groupItem.Events.Contains(this.SelectedEvent);
        this._associateButton.Enabled = !this._alreadyAssociatedErrorLabel.Visible;

        base.OnPreRender(e);
    }

    public Event SelectedEvent
    {
        get
        {
            if (this._eventList.SelectedIndex < 0)
            {
                try
                {
                    return ((ReadOnlyCollection<Event>)this._dataSource.Select())[0];
                }
                catch (ArgumentOutOfRangeException)
                {
                    Response.Redirect(string.Format("~/Event/AddEvent.aspx?{0}={1}", 
                        WebConstants.QueryVariables.BaseItemID, Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
                }
            }
            return ((ReadOnlyCollection<Event>)this._dataSource.Select())[this._eventList.SelectedIndex];
        }
    }

    protected void _createNewButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(String.Format("~/Event/AddEvent.aspx?{0}={0}", 
            WebConstants.QueryVariables.BaseItemID, this._groupItem.BaseItemID));
    }

    protected void _associateButton_Click(object sender, EventArgs e)
    {
        this._groupItem.Associate(this.SelectedEvent);
        Response.Redirect(WebUtilities.GetViewItemUrl(this._groupItem));
    }

    protected void _eventDetailsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[WebConstants.DataBindingParameters.BaseItemID] = this.SelectedEvent.BaseItemID;
    }
}
