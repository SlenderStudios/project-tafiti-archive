using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub
{
    public partial class DelAuthHandler : System.Web.UI.Page
    {
        private const string ContactsOfferReturn = "DeveloperCTPContacts.Read";
        private const string ProfilesOfferReturn = "DeveloperCTPProfiles.Read";
        private const string MeshObjOfferReturn = "DeveloperCTPMeshObject.Read";

        static DateTime ExpireCookie = DateTime.Now.AddYears(-10);
        static DateTime PersistCookie = DateTime.Now.AddYears(10);

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpRequest req = HttpContext.Current.Request;
            HttpResponse res = HttpContext.Current.Response;

            // Extract the 'action' parameter from the request, if any.
            string action = req.QueryString.Get("action");
            HttpCookie authCookie = new HttpCookie(Constants.AuthCookie);

			if (action == "logout")
			{
				//llau: FormAuth Signout
				HttpContext.Current.Session.Abandon();
				FormsAuthentication.SignOut();

				//Windows Live ID Signout
                HttpCookie loginCookie = new HttpCookie(Constants.LoginCookie);
				loginCookie.Expires = ExpireCookie;
				res.Cookies.Add(loginCookie);
                if (res.Cookies[Constants.AuthCookie] != null) res.Cookies.Remove(Constants.AuthCookie);
                res.Redirect(Constants.LoginPage);
                res.End();
			}
			else if (action == "clearcookie")
			{
                HttpCookie loginCookie = new HttpCookie(Constants.LoginCookie);
				loginCookie.Expires = ExpireCookie;
				res.Cookies.Add(loginCookie);

				string type;
				byte[] content;
				Constants.wll.GetClearCookieResponse(out type, out content);
				res.ContentType = type;
				res.OutputStream.Write(content, 0, content.Length);
				res.End();
			}
			else
			{
                string redirect = Constants.LogoutPage;

                WindowsLiveLogin.User user = Constants.wll.ProcessLogin(req.Form);

                HttpCookie loginCookie = new HttpCookie(Constants.LoginCookie);
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
                        redirect = Constants.ProfilePage;
					}
                    authCookie.Expires = ExpireCookie;
                    res.Cookies.Add(authCookie);

					HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(user.Id, "FormsAuthentication"), null);
					FormsAuthentication.SetAuthCookie(user.Id, false);
				}
                else if (authCookie.Name == Constants.AuthCookie)
                {
                    //Attempt to extract the consent token from the response.
                    WindowsLiveLogin.ConsentToken token = Constants.wll.ProcessConsent(req.Form);

                    // Save the app context.  Need the ride or event ID.
                    HttpCookie ctxCookie = new HttpCookie(Constants.ContextCookie);
				    ctxCookie.Expires = PersistCookie;
                    ctxCookie.Value = token.Context;
                    res.Cookies.Add(ctxCookie);

                    // If a consent token is found, store it in the cookie and then 
                    if (token != null)
                    {
                        authCookie.Value = token.DelegationToken;
                        authCookie.Expires = PersistCookie;
                    }
                    else
                    {
                        authCookie.Expires = ExpireCookie;
                    }
                    res.Cookies.Add(authCookie);

                    // Save the offers so we know what to do when the page loads.
                    HttpCookie offerCookie = new HttpCookie(Constants.OfferCookie);
                    offerCookie.Value = token.Offers[0].ToString();
                    offerCookie.Expires = PersistCookie;
                    res.Cookies.Add(offerCookie);

                    switch (token.Offers[0].ToString())
                    {
                        case ContactsOfferReturn:
                            redirect = Constants.InvitePage;
                            WebProfile.Current.ContactsRefresh = token.RefreshToken;
                            WebProfile.Current.ContactsDelToken = token.DelegationToken;
                            break;

                        case ProfilesOfferReturn:
                            redirect = Constants.ProfilePage + Constants.LiveProfileQuery;
                            WebProfile.Current.ProfilesRefresh = token.RefreshToken;
                            WebProfile.Current.ProfilesDelToken = token.DelegationToken;
                            break;

                        case MeshObjOfferReturn:
                            redirect = Constants.UploadPage + "?RideID=" + token.Context;
                            WebProfile.Current.PicturesRefresh = token.RefreshToken;
                            WebProfile.Current.PicturesDelToken = token.DelegationToken;
                            break;
                    }
                    WebProfile.Current.Save();
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
