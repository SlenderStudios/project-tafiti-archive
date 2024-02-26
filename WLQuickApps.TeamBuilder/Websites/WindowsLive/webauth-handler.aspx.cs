using System;
using System.Web;
using WindowsLive;

/////////////////////////////////////////////////////////////////////////////
// THIS PAGE HAS BEEN MODIFIED FROM THE SDK
/////////////////////////////////////////////////////////////////////////////


/// <summary>
/// This page handles the 'login', 'logout', 'clearcookie' and 'delauth'
/// Web Authentication and Delegated Authentication actions.
/// </summary>
public partial class HandlerPage : System.Web.UI.Page
{
    // Initialize the WindowsLiveLogin module.
    static WindowsLiveLogin WLLogin = new WindowsLiveLogin(true);

    protected void Page_Load(object sender, EventArgs e)
    {
        HttpRequest request = HttpContext.Current.Request;
        HttpResponse response = HttpContext.Current.Response;

        // Extract the 'appctx' parameter from the request, if any.
        string appctx = request["appctx"];
        if (string.IsNullOrEmpty(appctx) || !Utilities.IsValidUrl(appctx, true))
        {
            appctx = "~/Default.aspx";
        }

        // Extract the 'action' parameter from the request, if any.
        string action = request["action"];

        /*
            If action is 'logout', clear the login cookie and redirect to the
            logout page.

            If action is 'clearcookie', clear the login cookie and return a GIF
            as a response to signify success.

            If action is 'login', try to process sign-in. If the sign-in is  
            successful, cache the user token in a cookie and redirect to the  
            site's main page. If sign-in failed, clear the cookie and redirect 
            to the main page.

            If action is 'delauth', get user token from the cookie. Process the 
            consent token. If the consent token is valid, store the raw consent 
            token in persistent storage. Redirect to the site's main page.
        */

        if (action == "logout")
        {
            Session.Remove("ConsentToken");
            HttpCookie loginCookie = new HttpCookie("webauthtoken");
            loginCookie.Expires = DateTime.Now.AddYears(-10);
            response.Cookies.Add(loginCookie);
            response.Redirect(appctx);
            response.End();
        } 
        else if (action == "clearcookie")
        {
            HttpCookie loginCookie = new HttpCookie("webauthtoken");
            loginCookie.Expires = DateTime.Now.AddYears(-10);
            response.Cookies.Add(loginCookie);

            string type;
            byte[] content;
            WLLogin.GetClearCookieResponse(out type, out content);
            response.ContentType = type;
            response.OutputStream.Write(content, 0, content.Length);

            response.End();
        } 
        else if (action == "login")
        {
            HttpCookie loginCookie = new HttpCookie("webauthtoken");

            WindowsLiveLogin.User user = WLLogin.ProcessLogin(request.Form);

            if (user != null)
            {
                loginCookie.Value = user.Token;

                if (user.UsePersistentCookie)
                {
                    loginCookie.Expires = DateTime.Now.AddYears(10);
                }
            } 
            else 
            {
                loginCookie.Expires = DateTime.Now.AddYears(-10);
            }

            response.Cookies.Add(loginCookie);
            response.Redirect(appctx);
            response.End();
        }
        else if (action == "delauth")
        {
            ////////////////////////////////////////////////////////////////
            // CUSTOM LOGIC
            ////////////////////////////////////////////////////////////////
            
            // Get the consent token
            WindowsLiveLogin.ConsentToken ct = WLLogin.ProcessConsent(request.Form);

            //HACK: If we aren't doing contacts
            if (appctx != "Invite.aspx")
            {
                // Set the app token in the web.config
                // if there is an error render the token to the screen as it needs to be manually embedded in web.config
                Utilities.SetAppConsentToken(ct, true);
            }
            else
            {
                // we are redirecting to invite.aspx so put the CT in the session variable
                Session["ConsentToken"] = ct;
            }
            
            // redirect to the context
            response.Redirect(appctx);
            response.End();
        }
        else 
        {
            response.Redirect(appctx);
            response.End();
        }
    }
}
