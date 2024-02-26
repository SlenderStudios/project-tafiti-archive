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

public partial class Friend_ViewFriendRequests : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this._groupGallery.DataBind();
        this._eventGallery.DataBind();
    }

    protected override void OnPreRender(EventArgs e)
    {
        this._groupRequests.DataSource = SettingsWrapper.SpecialGroups;
        this._groupRequests.DataBind();
        base.OnPreRender(e);
    }

    protected void Gallery_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item)
        {
            e.Item.NamingContainer.Parent.Visible = true;
        }
    }
}
