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

public partial class CreateGroup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e) { }

    protected void _addGroupForm_Save(object sender, EventArgs e)
    {
        WLQuickApps.SocialNetwork.Business.Group newGroup = GroupManager.CreateGroup(this._addGroupForm.Name, this._addGroupForm.Description,
            this._addGroupForm.Type, this._addGroupForm.Location, this._addGroupForm.PrivacyLevel);

        if (this._addGroupForm.PictureBits != null)
        {
            newGroup.SetThumbnail(this._addGroupForm.PictureBits);
            GroupManager.UpdateGroup(newGroup);
        }

        this.Response.Redirect(WebUtilities.GetViewItemUrl(newGroup));
    }
}
