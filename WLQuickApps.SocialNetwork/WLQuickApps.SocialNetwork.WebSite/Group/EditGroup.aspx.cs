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

public partial class Group_EditGroup : System.Web.UI.Page
{
    private int _groupID;

    protected void Page_Load(object sender, EventArgs e)
    {
        this._groupID = Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]);
        if (!this.IsPostBack)
        {
            this._editGroupForm.GroupItem = GroupManager.GetGroup(this._groupID);
        }
    }

    protected void _editGroupForm_Save(object sender, EventArgs e)
    {
        WLQuickApps.SocialNetwork.Business.Group groupItem = GroupManager.GetGroup(this._groupID);

        groupItem.Title = this._editGroupForm.Name;
        groupItem.Description = this._editGroupForm.Description;
        groupItem.Location = this._editGroupForm.Location;
        groupItem.PrivacyLevel = this._editGroupForm.PrivacyLevel;
        if (this._editGroupForm.PictureBits != null)
        {
            groupItem.SetThumbnail(this._editGroupForm.PictureBits);
        }

        GroupManager.UpdateGroup(groupItem);

        this.Response.Redirect(WebUtilities.GetViewItemUrl(groupItem));
    }
}
