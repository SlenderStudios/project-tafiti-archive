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

public partial class Media_EditAlbum : System.Web.UI.Page
{
    private int _albumID;

    protected void Page_Load(object sender, EventArgs e)
    {
        this._albumID = Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]);
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (this.IsPostBack)
        {
            // DetailsView & FormView don't automatically update when
            // the user triggers postback by hitting the enter key.
            this.DetailsView1.UpdateItem(true);
        }
    }

    protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        Album album = AlbumManager.GetAlbum((int)e.Keys[0]);

        album.Title = (string)e.NewValues["Title"];

        FileUpload fileUpload = (FileUpload)this.DetailsView1.FindControl("_pictureFileUpload");
        if (fileUpload.HasFile)
        {
            try
            {
                album.SetThumbnail(fileUpload.FileBytes);
            }
            catch (ArgumentException)
            {
                this.DetailsView1.FindControl("_invalidThumbnailLabel").Visible = true;
                e.Cancel = true;
                return;
            }
        }

        AlbumManager.UpdateAlbum(album);

        GridView gridView = (GridView)this.DetailsView1.FindControl("_associationsGridView");
        foreach (GridViewRow row in gridView.Rows)
        {
            if (!((CheckBox)row.FindControl("_keepCheckBox")).Checked)
            {
                BaseItem baseItem = BaseItemManager.GetBaseItem(Convert.ToInt32(((Label)row.FindControl("_baseItemIDLabel")).Text));

                if (baseItem is Group)
                {
                    ((Group)baseItem).RemoveBaseItemAssociation(album, true);
                }
            }
        }

        this.RedirectToMyAlbums();
    }

    protected void DetailsView1_ModeChanged(object sender, EventArgs e)
    {
        this.RedirectToMyAlbums();
    }

    private void RedirectToMyAlbums()
    {
        this.Response.Redirect("~/Media/ViewAlbums.aspx");
    }

    protected void _associationsGridView_DataBound(object sender, EventArgs e)
    {
        if (((GridView)sender).Rows.Count == 0)
        {
            this.DetailsView1.FindControl("_associationsVisiblePanel").Visible = false;   
        }
    }

    protected void _associatedItemDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["baseItem"] = AlbumManager.GetAlbum(this._albumID);
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
