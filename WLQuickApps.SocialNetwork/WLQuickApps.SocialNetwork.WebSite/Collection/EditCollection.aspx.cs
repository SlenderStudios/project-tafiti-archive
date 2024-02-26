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

public partial class Collection_EditCollection : System.Web.UI.Page
{
    private int _collectionID;
     
    protected void Page_Load(object sender, EventArgs e)
    {
        this._collectionID = int.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]);

        if (!this.IsPostBack)
        {
            Collection collection = CollectionManager.GetCollection(this._collectionID);

            if (collection.HasThumbnail)
            {
                this._existingThumbnail.ImageUrl = WebUtilities.GetViewImageUrl(collection.BaseItemID, 128, 128);
                this._existingThumbnail.Visible = true;
                this._existingThumbnailLabel.Visible = true;
            }

            this._name.Text = collection.Title;
            this._description.Text = collection.Description;
        }
    }

    protected void _cancelButton_Click(object sender, EventArgs e)
    {
        this.RedirectToViewCollection();
    }

    protected void _updateButton_Click(object sender, EventArgs e)
    {
        Collection collection = CollectionManager.GetCollection(this._collectionID);

        if (this._pictureFileUpload.HasFile)
        {
            try
            {
                collection.SetThumbnail(this._pictureFileUpload.FileBytes);
            }
            catch (ArgumentException)
            {
                this._invalidThumbnailLabel.Visible = true;
                return;
            }
        }

        collection.Title = this._name.Text;
        collection.Description = this._description.Text;

        collection.Update();

        foreach (GridViewRow row in this._associationsGridView.Rows)
        {
            if (!((CheckBox)row.FindControl("_keepCheckBox")).Checked)
            {
                BaseItem baseItem = BaseItemManager.GetBaseItem(Convert.ToInt32(((Label)row.FindControl("_baseItemIDLabel")).Text));

                if (baseItem is Group)
                {
                    ((Group)baseItem).RemoveBaseItemAssociation(collection, true);
                }
            }
        }

        this.RedirectToViewCollection();
    }

    protected void RedirectToViewCollection()
    {
        this.Response.Redirect(WebUtilities.GetViewItemUrl(CollectionManager.GetCollection(this._collectionID)));
    }

    protected void _associationsGridView_DataBound(object sender, EventArgs e)
    {
        if (this._associationsGridView.Rows.Count == 0)
        {
            this._associationsVisiblePanel.Visible = false;
        }
    }

    protected void _associatedItemDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["baseItem"] = CollectionManager.GetCollection(this._collectionID);
    }

    public string GetViewItemUrl(BaseItem baseItem)
    {
        string urlString = "";
        if (baseItem is Event)
            urlString = WebUtilities.GetViewItemUrl(baseItem);
        else if (baseItem is Group)
        {
            urlString = WebUtilities.GetViewItemUrl(baseItem);
        }
        return urlString;
    }
}
