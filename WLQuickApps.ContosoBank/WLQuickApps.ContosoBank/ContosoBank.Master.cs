/*****************************************************************************
 * ContosoBank.Master
 * Notes: Master page for the site
 * **************************************************************************/

using System;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;

using Microsoft.Live.ServerControls;

using WLQuickApps.ContosoBank.Entity;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank
{
    public partial class ContosoBank : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Load user specific backround
            ProfileBase profile = HttpContext.Current.Profile;
            Background background = BackgroundLogic.GetBackgroundByName(profile["PreferredBackground"].ToString());
            MasterPageBody.Attributes.Add("style", "background:url(" + background.Location + ")");
        }

        private void pageRedirect(string applicationContext)
        {
            // Redirect based on user context or go to the default page
            if (!string.IsNullOrEmpty(applicationContext))
            {
                Response.Redirect(applicationContext);
            }
            else
            {
                Response.Redirect("/Default.aspx");
            }
        }

        protected void MasterIDLoginStatus_ServerSignIn(object sender, AuthEventArgs e)
        {
            // Add the idtoken to the session when logged in
            Session["ApplicationUserID"] = e.ApplicationUserID;
            if (UserLogic.UserIDExists(e.ApplicationUserID))
            {
                pageRedirect(e.ApplicationContext);
            }
            else
            {
                Response.Redirect("/ProfilePage.aspx");
            }
        }

        protected void MasterIDLoginStatus_ServerSignOut(object sender, AuthEventArgs e)
        {
            // remove the token from session, sign out of forms authentication and redirect.
            Session["ApplicationUserID"] = String.Empty;
            FormsAuthentication.SignOut();
            Response.Redirect("/Default.aspx");
        }
    }
}