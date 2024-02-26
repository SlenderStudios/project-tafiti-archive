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
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Media_AssociateAlbum : System.Web.UI.Page
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
        this._albumDisplay.DataBind();
    }

    protected override void OnPreRender(EventArgs e)
    {
        this._alreadyAssociatedErrorLabel.Visible = this._groupItem.Albums.Contains(this.SelectedAlbum);
        this._associateButton.Enabled = !this._alreadyAssociatedErrorLabel.Visible;

        base.OnPreRender(e);
    }

    public Album SelectedAlbum
    {
        get
        {
            if (this._albumList.SelectedIndex < 0)
            {
                try
                {
                    return ((List<Album>)this._albumDataSource.Select())[0];
                }
                catch (ArgumentOutOfRangeException)
                {
                    Response.Redirect(string.Format("~/Media/CreateAlbum.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
                }
            }
            return ((List<Album>)this._albumDataSource.Select())[this._albumList.SelectedIndex];
        }
    }

    protected void _createNewButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(String.Format("~/Media/CreateAlbum.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, this._groupItem.BaseItemID));
    }

    protected void _associateButton_Click(object sender, EventArgs e)
    {
        this._groupItem.Associate(this.SelectedAlbum);

        Response.Redirect(WebUtilities.GetViewItemUrl(this._groupItem));
    }

    protected void _mediaDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[WebConstants.DataBindingParameters.AlbumBaseItemID] = this.SelectedAlbum.BaseItemID;
    }
}
