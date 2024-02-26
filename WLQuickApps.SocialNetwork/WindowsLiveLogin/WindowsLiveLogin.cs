using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Security.Principal;
using System.IO;
using System.Security.Cryptography;

/// <summary>
/// Provides forms authentication via the Windows Live ID
/// Web Authentication service.
/// </summary>
public class WindowsLiveLogin
{
    #region Private Data
    private static string appId = WebConfigurationManager.AppSettings["WindowsLiveAppID"];
    private static string liveSecret = WebConfigurationManager.AppSettings["WindowsLiveSecret"];
    private static string securityAlgorithm = WebConfigurationManager.AppSettings["Security"];
    private static string policyUrl = WebConfigurationManager.AppSettings["PolicyURL"];
    private static string returnUrl = WebConfigurationManager.AppSettings["ReturnURL"];
    private static string baseUrl = WebConfigurationManager.AppSettings["BaseURL"];
    private static string secureUrl = WebConfigurationManager.AppSettings["SecureURL"];
    private static string consentReturnUrl = WebConfigurationManager.AppSettings["ConsentReturnURL"];
    private static string offer = WebConfigurationManager.AppSettings["Offer"];
    private static bool forceDelAuthNonProvisioned = (WebConfigurationManager.AppSettings["ForceDelauthNonprovisioned"] == "true" ? true : false);
    #endregion

    #region Properties
    public static string AppId
    {
        get { return appId; }
        set { appId = value; }
    }

    public static string Secret
    {
        get { return liveSecret; }
        set { liveSecret = value; }
    }

    public static string SecurityAlgorithm
    {
        get { return securityAlgorithm; }
        set { securityAlgorithm = value; }
    }

    public static string PolicyUrl
    {
        get { return policyUrl; }
        set { policyUrl = value; }
    }

    public static string ReturnUrl
    {
        get { return returnUrl; }
        set { returnUrl = value; }
    }

    public static string BaseUrl
    {
        get { return baseUrl; }
        set { baseUrl = value; }
    }

    public static string SecureUrl
    {
        get { return secureUrl; }
        set { secureUrl = value; }
    }

    public static string ConsentReturnUrl
    {
        get { return consentReturnUrl; }
        set { consentReturnUrl = value; }
    }

    public static string Offer
    {
        get { return offer; }
        set { offer = value; }
    }

    public static bool ForceDelAuthNonProvisioned
    {
        get { return forceDelAuthNonProvisioned; }
        set { forceDelAuthNonProvisioned = value; }
    }
    #endregion

    public static bool AuthenticateUser(out string windowsLiveUUID)
    {
        windowsLiveUUID = null;
        string action = HttpContext.Current.Request[WebConstants.WindowsLiveVariables.Action];

        if (action == WebConstants.WindowsLiveVariables.LogoutAction)
        {
            WindowsLiveLogin.LogoutUser();
            HttpContext.Current.Response.Redirect(FormsAuthentication.DefaultUrl);
            Helper.debug("Logout");
            return false;
        }
        else if (action == WebConstants.WindowsLiveVariables.ClearCookieAction)
        {
            WindowsLiveLogin.LogoutUser();
            HttpContext.Current.Response.Redirect("~/Images/signout_good.gif");
            Helper.debug("ClearCookie");
            return false;
        }
        else
        {
            string token = HttpContext.Current.Request[WebConstants.WindowsLiveVariables.Token];
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            token = Helper.DecodeToken(token, Secret);
            if (string.IsNullOrEmpty(token))
            {
                Helper.debug("Unable to decode Windows Live token.");
                return false;
            }

            NameValueCollection parsedToken = HttpUtility.ParseQueryString(token);
            if (parsedToken == null || parsedToken.Count < 3)
            {
                Helper.debug("Unable to parse Windows Live token.");
                return false;
            }

            string appID = parsedToken[WebConstants.WindowsLiveVariables.AppID];
            if (appID != AppId)
            {
                Helper.debug("Received Windows Live token with incorrect AppID.");
                return false;
            }

            windowsLiveUUID = parsedToken[WebConstants.WindowsLiveVariables.UID];
            HttpContext.Current.Session[WebConstants.SessionVariables.WindowsLiveUUID] = windowsLiveUUID;
            Helper.debug("windowsLiveUUID=" + windowsLiveUUID);

            return true;
        }
    }

    public static string GetWindowsLiveUUID()
    {
        if (HttpContext.Current == null)
        {
            return null;
        }
        else
        {
            return (HttpContext.Current.Session[WebConstants.SessionVariables.WindowsLiveUUID] as string);
        }
    }

    public static bool IsUserAuthenticated()
    {
        return (!string.IsNullOrEmpty(WindowsLiveLogin.GetWindowsLiveUUID()));
    }
    
    /// <summary>
    /// Signs out of Windows Live ID and forms authentication.
    /// </summary>
    public static void LogoutUser()
    {
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        // Sign out of forms auth.
        HttpContext.Current.Session.Abandon();
        FormsAuthentication.SignOut();
    }

    public static string GetLoginUrl()
    {
        return string.Format("http://login.live.com/wlogin.srf?{0}={1}&{2}={3}&alg=wsignin1.0",
            WebConstants.WindowsLiveVariables.AppID, AppId,
            WebConstants.WindowsLiveVariables.AppContext, HttpUtility.UrlEncode(WindowsLiveLogin.GetReturnUrl()));
    }

    public static string GetLogoutUrl()
    {
        return string.Format("http://login.live.com/logout.srf?{0}={1}", WebConstants.WindowsLiveVariables.AppID, AppId);
    }

    private static string GetReturnUrl()
    {
        HttpContext context = HttpContext.Current;

        if (!string.IsNullOrEmpty(context.Request.QueryString[WebConstants.QueryVariables.ReturnURL]))
        {
            return context.Request.QueryString[WebConstants.QueryVariables.ReturnURL];
        }
        else
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.Url.PathAndQuery))
            {
                return HttpContext.Current.Request.Url.PathAndQuery;
            }
            else
            {
                return FormsAuthentication.DefaultUrl;
            }
        }
    }

    public static string GetConsentUrl()
    {
        return string.Format("https://consent.live.com/Delegation.aspx?RU={0}&ps={1}&pl={2}&mkt=en-US", ConsentReturnUrl, Offer, PolicyUrl);
    }

    public static string GetRefreshConsentUrl(string refreshToken)
    {
        return string.Format("https://consent.live.com/RefreshToken.aspx?RU={0}&ps={1}&reft={2}", ConsentReturnUrl, Offer, refreshToken);
    }

    public static void LoginUser(string userName)
    {
        HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(userName, "FormsAuthentication"), null);
        FormsAuthentication.SetAuthCookie(userName, false);

        // Redirect user to original destination.
        string returnUrl = HttpContext.Current.Request[WebConstants.WindowsLiveVariables.AppContext];
        if (string.IsNullOrEmpty(returnUrl))
        {
            returnUrl = FormsAuthentication.DefaultUrl;
        }

        Uri nextUrl = new Uri(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + returnUrl);
        UriBuilder returnUrlBuilder = new UriBuilder(HttpContext.Current.Request.Url);
        returnUrlBuilder.Query = nextUrl.Query;
        returnUrlBuilder.Path = nextUrl.AbsolutePath;

        string alertsUrl = string.Empty;
/*
        // Get the Alerts Signup URL
        alertsUrl = LiveAlertsWrapper.GetAlertsSignupUrl(returnUrlBuilder.ToString());

        if (alertsUrl.Length > 0)
        {
            HttpContext.Current.Response.Redirect(alertsUrl);
        }
        else
        {
            HttpContext.Current.Response.Redirect(returnUrl);
        }
 */
    }
}

