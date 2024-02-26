using System;
using System.Data;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WindowsLive;

/// <summary>
/// Summary description for Utilities
/// </summary>
public class Utilities
{
    public static WindowsLiveLogin.ConsentToken GetAppConsentToken()
    {
        NameValueCollection appSettings = WebConfigurationManager.AppSettings;
        string setting = appSettings["token"];

        if (setting != null)
        {
            WindowsLiveLogin wll = new WindowsLiveLogin(true);
            WindowsLiveLogin.ConsentToken token = wll.ProcessConsentToken(setting);

            // If a consent token is found and is stale, try to refresh it and store  
            // it in persistent storage.
            if (!token.IsValid())
            {
                if (token.Refresh() && token.IsValid())
                {
                    SetAppConsentToken(token, false);
                }
            }

            return token;
        }
        else
        {
            return null;
        }
    }

    public static void SetAppConsentToken(WindowsLiveLogin.ConsentToken token, bool ShowConsentTokenOnError)
    {
        try
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            KeyValueConfigurationElement setting = config.AppSettings.Settings["token"];

            if (token != null)
            {
                if (setting != null)
                {
                    setting.Value = token.Token;
                }
                else
                {
                    config.AppSettings.Settings.Add("token", token.Token);
                }
            }

            // Save configuration
            config.Save();
        }
        catch (Exception ex)
        {
            if (ShowConsentTokenOnError)
            {
                HttpContext.Current.Response.Write("\n\n\n\n");
                HttpContext.Current.Response.Write("Token to be added to the web.config is below:\n\n\n\n");
                HttpContext.Current.Response.Write(token.Token);
                HttpContext.Current.Response.Write("\n\n\n\n");
                HttpContext.Current.Response.End();
            }
        }
    }

    public static bool IsValidUrl(string url)
    {
        return IsValidUrl(url, false);
    }

    public static bool IsValidUrl(string url, bool sameDomain)
    {
        //HACK: just do a work around for invite.aspx for the meantime.
        if (url == "Invite.aspx")
        {
            return (true);
        }
        else
        {
            bool match = Regex.IsMatch(url,
                @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\(\)\/\\\+=&amp;@%\$#_]*)?$");

            if (match && sameDomain)
            {
                string host = HttpContext.Current.Request.Url.Host;
                match = (new Uri(url)).Host == host;
            }

            return match;
        }
    }
}
