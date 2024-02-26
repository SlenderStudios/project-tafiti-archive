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
using System.Drawing;
using System.IO;

using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Collection_CreateCollection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void _createButton_Click(object sender, EventArgs e)
    {
        if (this._pictureFileUpload.HasFile)
        {
            try
            {
                Bitmap b = new Bitmap(new MemoryStream(this._pictureFileUpload.FileBytes));
            }
            catch (ArgumentException)
            {
                this._invalidPictureError.Visible = true;
                return;
            }
        }

        this.CreateCollection();
    }

    private void CreateCollection()
    {
        Collection collection = CollectionManager.CreateCollection(this._name.Text, this._description.Text, string.Empty);

        if (this._pictureFileUpload.HasFile)
        {
            collection.SetThumbnail(this._pictureFileUpload.FileBytes);
        }

        if (!String.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]))
        {
            Group group = GroupManager.GetGroup(Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
            group.Associate(collection);
            Response.Redirect(WebUtilities.GetViewItemUrl(group));
        }

        this.Response.Redirect(WebUtilities.GetViewItemUrl(collection));
    }
}
