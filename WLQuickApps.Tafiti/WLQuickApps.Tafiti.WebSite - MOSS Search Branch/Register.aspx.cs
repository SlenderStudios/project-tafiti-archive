using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using WLQuickApps.Tafiti.WebSite;
using WLQuickApps.Tafiti.Business;

public partial class Register : System.Web.UI.Page
{
    const string LoginPage = "Default.aspx";
    const string LogoutPage = LoginPage;
    const string LoginCookie = "webauthtoken";
    static DateTime ExpireCookie = DateTime.Now.AddYears(-10);
    static DateTime PersistCookie = DateTime.Now.AddYears(10);

    // Initialize the WindowsLiveLogin module.
    static WindowsLiveLogin wll = new WindowsLiveLogin(true);

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpRequest req = HttpContext.Current.Request;
        HttpResponse res = HttpContext.Current.Response;

        // Extract the 'action' parameter from the request, if any.
        string action = req.QueryString.Get("action");

        /*
          If action is 'logout', clear the login cookie and redirect
          to the logout page.

          If action is 'clearcookie', clear the login cookie and
          return a GIF as response to signify success.

          By default, try to process a login. If login was
          successful, cache the user token in a cookie and redirect
          to the site's main page.  If login failed, clear the cookie
          and redirect to the main page.
        */

        if (action == "logout")
        {
            UserManager.LogOut();

            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            loginCookie.Expires = ExpireCookie;
            res.Cookies.Add(loginCookie);
            res.Redirect(LogoutPage);
            res.End();
        }
        else if (action == "clearcookie")
        {
            UserManager.LogOut();

            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            loginCookie.Expires = ExpireCookie;
            res.Cookies.Add(loginCookie);

            res.Redirect("~/Images/signout_good.gif");

            res.End();
        }
        else
        {
            WindowsLiveLogin.User user = wll.ProcessLogin(req.Form);

            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            if (user != null)
            {
                UserManager.LogIn(user.Id);
                AnonymousUserManager.MergeShelfStacks();

                //loginCookie.Value = user.Token;

                //if (user.UsePersistentCookie)
                //{
                //    loginCookie.Expires = PersistCookie;
                //}
            }
            else
            {
                loginCookie.Expires = ExpireCookie;
            }

            res.Cookies.Add(loginCookie);
            res.Redirect(LogoutPage);
            res.End();
        }
    }
}
