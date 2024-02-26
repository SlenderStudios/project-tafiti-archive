using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Text.RegularExpressions;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Media_PhotoAlbumPermission : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetPhotoToken();
    }

    private void GetPhotoToken()
    {
        PhotoToken photoToken = null;
        if (this.IsPostBack) return;

        Regex rxResponse = new Regex(@"^[a-zA-Z0-9]{1,32}$");
        Regex rxOwner = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$");
        Regex rxDat = new Regex(@"^A:\w{16,32}:.{3,128}:[a-zA-Z0-9+/]{342}==$");

        //Go through the posted form and extract the values.
        System.Collections.Specialized.NameValueCollection postedValues = Request.Form;
        for (int i = 0; i < postedValues.AllKeys.Length; i++)
        {
            if (photoToken == null) photoToken = new PhotoToken();
            String nextKey = postedValues.AllKeys[i];
            switch (nextKey)
            {
                   case "OwnerHandle":
                    if (rxOwner.IsMatch(postedValues[i]))
                        photoToken.OwnerHandle = postedValues[i];
                    break;
                case "DomainAuthenticationToken":
                    if (rxDat.IsMatch(postedValues[i]))
                        photoToken.DomainAuthenticationToken = postedValues[i];
                    break;
                default:
                    break;
            }
        }

        if (photoToken == null) return;


        // get the current logged in user
        User user = UserManager.LoggedInUser;

        // is the user currently logged in?
        if (user != null)
        {
            // set the token
            user.PhotoPermissionToken = photoToken;

            // update the user
            UserManager.UpdateUser(user);
        }
        
        // set the javascript outputs
        this.litDomainAuthenticationToken.Text = photoToken.DomainAuthenticationToken;
        this.litOwnerHandle.Text = photoToken.OwnerHandle;

        // set the JS to be visible.
        this.pnlCloseAndRefresh.Visible = true;
    }
   
}