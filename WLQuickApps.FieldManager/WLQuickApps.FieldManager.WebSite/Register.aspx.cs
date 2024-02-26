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

using WLQuickApps.FieldManager.Business;
using WLQuickApps.FieldManager.Data;

namespace WLQuickApps.FieldManager.WebSite
{
    public partial class Register : System.Web.UI.Page
    {
        static private WindowsLiveLogin _windowsLiveLogin = new WindowsLiveLogin(true);
        const string LoginCookie = "webauthtoken";
        static private DateTime ExpireCookie = DateTime.Now.AddYears(-10);
        static private DateTime PersistCookie = DateTime.Now.AddYears(10);

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                return;
            }

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
                res.Redirect("~/Default.aspx");
                res.End();
                return;
            }
            else if (action == "clearcookie")
            {
                UserManager.LogOut();

                HttpCookie loginCookie = new HttpCookie(LoginCookie);
                loginCookie.Expires = ExpireCookie;
                res.Cookies.Add(loginCookie);

                res.Redirect("~/Images/signout_good.gif");
                res.End();
                return;
            }
            else
            {
                WindowsLiveLogin.User liveUser = _windowsLiveLogin.ProcessLogin(req.Form);

                string path = this.Request.Form["appctx"];
                if (string.IsNullOrEmpty(path))
                {
                    path = "~/Default.aspx";
                }

                HttpCookie loginCookie = new HttpCookie(LoginCookie);
                if (liveUser != null)
                {
                    if (UserManager.UserExists(liveUser.Id))
                    {
                        User user = UserManager.GetUser(liveUser.Id);
                        FormsAuthentication.SetAuthCookie(Membership.GetUser(user.UserID).UserName, false);

                        this.Response.Redirect(
                            string.Format("{0}://{1}:{2}{3}",
                                this.Request.Url.Scheme,
                                this.Request.Url.Host,
                                this.Request.Url.Port,
                                path));
                    }
                    else
                    {
                        this.Session["LiveIDToken"] = liveUser.Id;
                    }
                }
                else
                {
                    this.Response.Redirect(path);
                }
            }
        }

        protected void _registerButton_Click(object sender, EventArgs e)
        {
            User user = UserManager.CreateUser(this._emailTextBox.Text, (string) this.Session["LiveIDToken"]);
            FormsAuthentication.RedirectFromLoginPage(Membership.GetUser(user.UserID).UserName, false);
        }
    }
}