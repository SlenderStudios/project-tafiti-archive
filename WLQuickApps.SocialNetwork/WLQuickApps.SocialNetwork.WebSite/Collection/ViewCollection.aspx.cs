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
using System.Collections.Generic;

public partial class Collection_ViewCollection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void _itemList_PreRender(object sender, EventArgs e)
    {
        DataList itemList = (DataList)sender;

        itemList.ShowHeader = (itemList.Items.Count == 0);
    }

    protected void _deleteLink_Command(object sender, CommandEventArgs e)
    {
        int baseItemID = int.Parse(e.CommandArgument.ToString());

        CollectionManager.GetCollection(baseItemID).Delete();
        this.Response.Redirect("~/Collection/ViewCollections.aspx");
    }

    protected void _copyLink_Command(object sender, CommandEventArgs e)
    {
        int baseItemID = int.Parse(e.CommandArgument.ToString());

        Collection collection = CollectionManager.CopyCollectionToCurrentUserAccount(CollectionManager.GetCollection(baseItemID));
        this.Response.Redirect(WebUtilities.GetViewItemUrl(collection));
    }

}
