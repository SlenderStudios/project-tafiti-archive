using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Media_SynchronizeAlbum : System.Web.UI.Page
{
    PhotoToken _token
    {
        get
        {
            object ob = ViewState["TOKEN"];
            if (ob != null) return (PhotoToken)ob;
            User user = UserManager.LoggedInUser;
            if (user == null) return null;
            ob = ViewState["TOKEN"] = user.PhotoPermissionToken;
            if (ob != null) return (PhotoToken)ob;
            return null;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!Page.IsPostBack)
        {
            // user hasn't delegated permission - show - "you must delegate permission"
            if (_token == null)
            {
                pnlNoTokenPresent.Visible = true;
                AlbumPanel.Visible = false;
                return;
            }
            this.pnlNoTokenPresent.Visible = false;
            btnLoad.Visible = true;
            btnSubmit.Visible = false;
            

        }
        imgPreview.Visible = false;
        this.lblStatusUpdate.Text = "";

    }

    private void PopulateAlbums()
    {
        try
        {
            LiveNet.Photos.LivePhotos lv = new LiveNet.Photos.LivePhotos(_token.OwnerHandle, _token.DomainAuthenticationToken, LiveNet.Authentication.AuthenticationToken.DomainAuthenticationToken);
            AlbumPanel.Visible = true;
            LiveNet.Photos.Album[] albums = lv.ListAlbums();
            if (albums == null) return;
            if (albums.Length < 1) return;
            lstSpacesAlbums.DataSource = albums;
            lstSpacesAlbums.DataTextField = "Name";
            lstSpacesAlbums.DataValueField = "AlbumUrl";
            lstSpacesAlbums.DataBind();
            SelectAllAlbum(false);

        }
        catch (Exception ex)
        {
            btnSubmit.Enabled = false;
            this.lblStatusUpdate.Text = ex.Message;
        }
    }

    private void SelectAllAlbum(bool LoadAllPhotos)
    {
        foreach (ListItem li in lstSpacesAlbums.Items)
        {
            li.Selected = true;
        }

        if (LoadAllPhotos)
        {
            ListPhotoFromSpace();
        }
    }

    /// <summary>
    /// Synchronize all photos from Spaces to SocialNetwork
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (lstSpacesAlbums.Items.Count < 1) return;
            
// enumerate the albums
        bool bflag = false;
        for( int counter = 0; counter < lstSpacesAlbums.Items.Count; counter++)
        {
            if (lstSpacesAlbums.Items[counter].Selected == true)
            {
                AlbumManager.SynchronizeAlbumFromSpacesToLocalDatabase(new Uri(lstSpacesAlbums.Items[counter].Value), true);
                bflag = true;
                lstSpacesAlbums.Items.RemoveAt(counter--);
            }
         }
        // update the status
        if (!bflag) return;
        lstPhoto.Items.Clear();

        if (lstSpacesAlbums.Items.Count < 1)
        {
            panEmpty.Visible = true;
            pnlNoTokenPresent.Visible = false;
            AlbumPanel.Visible = false;
        }
        else
        {
            this.lblStatusUpdate.Text = "The selected photo albums have been synchronized - click on <a href='ViewAlbums.aspx'>My Galleries</a> to see your changes.";
        }
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        btnLoad.Visible = false;
        btnSubmit.Visible = true;
        PopulateAlbums();
    }

    protected void lstPhoto_SelectedIndexChanged(object sender, EventArgs e)
    {
        DisplayPhoto();
    }



    protected void lstSpacesAlbums_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListPhotoFromSpace();
    }

    private void DisplayPhoto()
    {
        if (_token == null || lstPhoto.SelectedIndex < 0 ) return;
        imgPreview.Visible = true;
        Session["DomainAuthToken"] = _token.DomainAuthenticationToken;
        this.imgPreview.ImageUrl = string.Format("RawImageHandler.ashx?IMAGEURL={0}", lstPhoto.SelectedValue);

    }

    private void ListPhotoFromSpace()
    {
        lstPhoto.Items.Clear();
        if (lstSpacesAlbums.Items.Count < 1 || _token == null) return;
        try
        {
            LiveNet.Photos.LivePhotos lv = new LiveNet.Photos.LivePhotos(_token.OwnerHandle, _token.DomainAuthenticationToken, LiveNet.Authentication.AuthenticationToken.DomainAuthenticationToken);
            for (int counter = 0; counter < lstSpacesAlbums.Items.Count; counter++)
            {
                if (lstSpacesAlbums.Items[counter].Selected == true)
                {
                    LiveNet.Photos.Photo[] photos = lv.ListPhotos(new Uri(lstSpacesAlbums.Items[counter].Value));
                    if (photos != null)
                    {
                        foreach (LiveNet.Photos.Photo ph in photos)
                        {
                            lstPhoto.Items.Add(new ListItem(ph.Name, ph.PhotoUrl.ToString()));
                        }
                    }
                
                }
            }
        }
        catch {}

    }


}