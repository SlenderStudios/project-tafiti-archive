using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

public partial class webauth_handler : System.Web.UI.Page
{
    const string LoginPage = "Default.aspx";
    const string LogoutPage = "Default.aspx";
    protected static string LoginCookie = ConfigurationManager.AppSettings["wll_logincookie"];
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
            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            loginCookie.Expires = ExpireCookie;
            res.Cookies.Add(loginCookie);
            res.Redirect(LogoutPage);
            res.End();
        }
        else if (action == "clearcookie")
        {
            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            loginCookie.Expires = ExpireCookie;
            res.Cookies.Add(loginCookie);

            string type;
            byte[] content;
            wll.GetClearCookieResponse(out type, out content);
            res.ContentType = type;
            res.OutputStream.Write(content, 0, content.Length);

            res.End();
        }
        else
        {
            // req.Form consists of the HTTP POST params "stoken", "appctx"
            WindowsLiveLogin.User user = wll.ProcessLogin(req.Form);

            HttpCookie loginCookie = new HttpCookie(LoginCookie);
            if (user != null)
            {
                loginCookie.Value = user.Token;

                if (user.UsePersistentCookie)
                {
                    loginCookie.Expires = PersistCookie;
                }
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
