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

public partial class Collection_EditItems : System.Web.UI.Page
{
    private int _collectionID;

    protected void Page_Load(object sender, EventArgs e)
    {
        this._collectionID = int.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]);
    }

    protected void _collectionItemsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[WebConstants.DataBindingParameters.Collection] = CollectionManager.GetCollection(this._collectionID);
    }

    protected void _itemsGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        CollectionItemManager.DeleteCollectionItem(CollectionItemManager.GetCollectionItem((int)e.Keys[0]));

        e.Cancel = true;
        ((GridView)sender).DataBind();
    }

    protected void _locationRequired_ServerValidate(object source, ServerValidateEventArgs args)
    {
        Location location = this._location.LocationItem;

        if ((location.Latitude == 0) && (location.Longitude == 0))
        {
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void _addItem_Click(object sender, EventArgs e)
    {
        if (!this.IsValid)
        {
            return;
        }

        CollectionItemManager.CreateCollectionItem(CollectionManager.GetCollection(this._collectionID),
            this._location.LocationItem, this._name.Text, this._description.Text, null);

        this._name.Text = string.Empty;
        this._description.Text = string.Empty;
        this._location.LocationItem = Location.Empty;

        this._collectionView.DataBind();
    }
}
