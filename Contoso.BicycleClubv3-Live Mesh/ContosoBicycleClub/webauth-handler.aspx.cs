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
using System.Security.Principal;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub
{
	/// <summary>
	/// This page handles the login, logout and clearcookie Web Auth
	/// actions.  When you create a Windows Live application, you must
	/// specify the URL of this handler page.
	/// </summary>
	public partial class HandlerPage : System.Web.UI.Page
	{
		const string LoginPage = "default.aspx";
		const string LogoutPage = LoginPage;
		const string LoginCookie = "webauthtoken";
        const string AuthCookie = "delauthtoken";
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
            HttpCookie authCookie = new HttpCookie(AuthCookie);

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
				//llau: FormAuth Signout
				HttpContext.Current.Session.Abandon();
				FormsAuthentication.SignOut();

				//Windows Live ID Signout
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
            else if (action == "delauth")
            {
                //Attempt to extract the consent token from the response.
                WindowsLiveLogin.ConsentToken token = wll.ProcessConsent(req.Form);

                // If a consent token is found, store it in the cookie and then 
                // redirect to the main page.
                if (token != null)
                {
                    authCookie.Value = token.Token;
                    authCookie.Expires = PersistCookie;
                }
                else
                {
                    authCookie.Expires = ExpireCookie;
                }

                res.Cookies.Add(authCookie);
                res.Redirect("InviteFriends.aspx");
                res.End();
            }
			else
			{
				string redirect = LogoutPage;

				WindowsLiveLogin.User user = wll.ProcessLogin(req.Form);

				HttpCookie loginCookie = new HttpCookie(LoginCookie);
				if (user != null)
				{
					loginCookie.Value = user.Token;

					if (user.UsePersistentCookie)
					{
						loginCookie.Expires = PersistCookie;
					}

					MembershipUserCollection users = Membership.FindUsersByName(user.Id);
					if (users.Count == 0)
					{
						//create 
						string password = Membership.GeneratePassword(8, 2);
						// Create an ASP.NET membership user.
						MembershipUser membershipUser = Membership.CreateUser(user.Id, password);

						//indicate to take the user to the register page for the first time.
						redirect = "~/Profile.aspx";
					}

					HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(user.Id, "FormsAuthentication"), null);
					FormsAuthentication.SetAuthCookie(user.Id, false);
				}
				else
				{
					loginCookie.Expires = ExpireCookie;
				}

				res.Cookies.Add(loginCookie);
				res.Redirect(redirect);
				res.End();
			}
		}
	}

}


