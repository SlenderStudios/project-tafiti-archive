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

public partial class Group_ManageGroup : System.Web.UI.Page
{
    private WLQuickApps.SocialNetwork.Business.Group _groupItem;

    protected void Page_Load(object sender, EventArgs e)
    {
        int groupID;
        if (string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            groupID = Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]);
        }
        else
        {
            groupID = Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]);
        }
        this._groupItem = GroupManager.GetGroup(groupID);
    }

    protected void _associationsDataSources_Action(object sender, ObjectDataSourceMethodEventArgs e)
    {
        e.InputParameters[WebConstants.DataBindingParameters.BaseItem] = this._groupItem;
    }

    protected void _associationsParentsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!EventManager.EventExists(this._groupItem.BaseItemID))
        {
            e.Cancel = true;
            this._associationsParentsPanel.Visible = false;
        }
        e.InputParameters[WebConstants.DataBindingParameters.BaseItem] = this._groupItem;
    }

    protected void _membersDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[WebConstants.DataBindingParameters.BaseItemID] = this._groupItem.BaseItemID;
    }

    protected void _membersDataSource_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
    {
        e.InputParameters[WebConstants.DataBindingParameters.Group] = this._groupItem;
    }

    protected void _returnButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(WebUtilities.GetViewItemUrl(this._groupItem));        
    }
}
