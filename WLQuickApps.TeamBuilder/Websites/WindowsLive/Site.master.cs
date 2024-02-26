using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Live.ServerControls;
using Microsoft.Security.Application;
using WindowsLive;

public partial class Site : System.Web.UI.MasterPage
{
    // Initialize the WindowsLiveLogin module.
    static WindowsLiveLogin WLLogin = new WindowsLiveLogin(true);

    protected void Page_Load(object sender, EventArgs e)
    {
        // Check WindowsLiveUser authentication
        WindowsLiveLogin.User wllUser = WindowsLiveUser;
        if (wllUser != null)
        {
            // Set Sign Out
            SignIn.Text = "Sign Out";
            SignIn.NavigateUrl = WLLogin.GetLogoutUrl();

            if (!IsPostBack)
            {
                // Check Page.User authentication
                if (Page.User.Identity.IsAuthenticated)
                {
                    if (Page.User.IsInRole("Admin"))
                    {
                        // Get App settings
                        NameValueCollection appSettings = WebConfigurationManager.AppSettings;

                        // Populate Calendar settings
                        CalendarURL.Text = GetConfigurationSetting(appSettings, "calendar");

                        // Populate News Feed settings
                        NewsFeed1.Text = GetConfigurationSetting(appSettings, "newsfeed1");
                        NewsFeed2.Text = GetConfigurationSetting(appSettings, "newsfeed2");
                        NewsFeed3.Text = GetConfigurationSetting(appSettings, "newsfeed3");

                        // Set the SharePhotosLink for Spaces.ReadWrite access
                        SharePhotosLink.NavigateUrl = WLLogin.GetConsentUrl("SpacesPhotos.ReadWrite", null, GetConfigurationSetting(appSettings, "wll_returnurl"));

                        // Display Settings panel and link
                        Settings.Visible = true;
                        SettingsPanel.Visible = true;
                    }

                    // Populate Presence settings in case they are overwritten 
                    // when we handle the presence request in the next section
                    string id = Profile.Presence.ID;
                    if (!string.IsNullOrEmpty(id))
                    {
                        // Set decline message
                        string declineMessage =
                            "You can remove your online presence by clicking " +
                            "<a href=\"" + GetPresenceUrl() + "\">here</a> and selecting " +
                            "Cancel from the Windows Live screen when prompted.";

                        PresenceMessage.Text = declineMessage;

                        // Enable presence controls
                        DisplayNameLabel.Visible = true;
                        DisplayName.Visible = true;
                        PresenceButton.Visible = true;

                        // Set the presence fields
                        PresenceID.Value = id;
                        DisplayName.Text = Profile.Presence.DisplayName;

                        // Display Save message
                        PresenceSaveMessage.Visible = true;

                        // Make sure validation works
                        DisplayNameRequired.Enabled = true;
                    }
                    else
                    {
                        string inactiveMessage =
                            "If you would like to share your online presence, click " +
                            "<a href=\"" + GetPresenceUrl() + "\">here</a>.";

                        PresenceMessage.Text = inactiveMessage;
                    }

                    // Display Presence panel and link
                    PresenceControl.Visible = true;
                    PresencePanel.Visible = true;

                    // Display Invite link
                    Invite.Visible = true;

                    // Process request parameters
                    ProcessRequest();
                }
                else
                {
                    // Check membership association
                    LiveMembershipProvider provider = (LiveMembershipProvider)Membership.Provider;
                    string userName = provider.GetLiveAssociation(wllUser.Id);
                    if (!string.IsNullOrEmpty(userName))
                    {
                        // Authenticate associated user
                        FormsAuthentication.RedirectFromLoginPage(userName, false);
                    }
                    else
                    {
                        // Display Registration panel and link
                        RegistrationPanel.Visible = true;
                    }
                }
            }
        }
        else
        {
            // A user should never be authenticated
            // when not signed into Windows Live
            if (Page.User.Identity.IsAuthenticated)
            {
                // Prevent SignOut from redirecting
                FormsAuthentication.SetAuthCookie(null, false);
                FormsAuthentication.SignOut();
            }

            // Set Sign In
            SignIn.Text = "Sign In";
            SignIn.NavigateUrl = WLLogin.GetLoginUrl();
        }
    }

    protected void PresenceButton_Click(object sender, EventArgs e)
    {
        // Save presence to profile
        if (Page.User.Identity.IsAuthenticated)
        {
            Profile.Presence.ID = PresenceID.Value;
            Profile.Presence.DisplayName = DisplayName.Text;
        }

        // Reload page
        Response.Redirect(GetPageUrl());
        Response.End();
    }

    protected void SettingsButton_Click(object sender, EventArgs e)
    {
        if (Page.User.Identity.IsAuthenticated &&
            Page.User.IsInRole("Admin"))
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

            // Calendar settings
            string calendar = CalendarURL.Text;

            // If the URL is a direct copy from Windows Live Calendar,
            // replace webcal:// with http://
            calendar = calendar.Replace("webcal://", "http://");
            if (string.IsNullOrEmpty(calendar) || Utilities.IsValidUrl(calendar))
            {
                SetConfigurationSetting(config, "calendar", calendar);
            }

            // News Feed settings
            string newsfeed1 = NewsFeed1.Text;
            if (string.IsNullOrEmpty(newsfeed1) || Utilities.IsValidUrl(newsfeed1))
            {
                SetConfigurationSetting(config, "newsfeed1", newsfeed1);
            }
            string newsfeed2 = NewsFeed2.Text;
            if (string.IsNullOrEmpty(newsfeed2) || Utilities.IsValidUrl(newsfeed2))
            {
                SetConfigurationSetting(config, "newsfeed2", newsfeed2);
            }
            string newsfeed3 = NewsFeed3.Text;
            if (string.IsNullOrEmpty(newsfeed3) || Utilities.IsValidUrl(newsfeed3))
            {
                SetConfigurationSetting(config, "newsfeed3", newsfeed3);
            }

            // Save configuration
            config.Save();
        }

        // Reload page
        Response.Redirect(GetPageUrl());
        Response.End();
    }

    protected void RegisterButton_Click(object sender, EventArgs e)
    {
        // Validate user
        MembershipUser user = Membership.GetUser(IDKey.Text);

       // Was the user found?
        if (user == null)
        {
            // No - The user object doesn't exist so create it.
             // get the email address
            string userName = IDKey.Text;

            // create the accoutn
            user = Membership.CreateUser(userName, userName, userName);
        }

        // get the current WLL user
        WindowsLiveLogin.User wllUser = WindowsLiveUser;

        // If the user ID is obtained successfully, associate it with the current logged in user
        if (wllUser != null)
        {
            // Associate Live ID
            LiveMembershipProvider provider = (LiveMembershipProvider)Membership.Provider;
            provider.SetLiveAssociation(wllUser.Id, user.UserName);

            // Authenticate associated user
            FormsAuthentication.RedirectFromLoginPage(user.UserName, false);
        }

    }

    private void ProcessRequest()
    {
        // Handle a presence request
        string result = Request["result"];
        if (!string.IsNullOrEmpty(result))
        {
            // A result parameter has been provided. Make sure it's actually
            // coming from the settings URL to avoid spoofing
            string referrerHost = string.Empty;
            if (Request.UrlReferrer != null)
            {
                referrerHost = Request.UrlReferrer.GetComponents(UriComponents.Host, UriFormat.Unescaped);
            }

            // Enable the presence controls if it is safe
            if (referrerHost == "settings.messenger.live.com")
            {
                ProcessPresenceRequest(result);
            }
        }

        // Handle a delegation authentication request
        string action = Request["action"];
        if (!string.IsNullOrEmpty(action))
        {
            ProcessAuthenticationRequest(action);
        }
    }

    private void ProcessPresenceRequest(string result)
    {
        if (result == "Accepted")
        {
            string id = Request["id"];

            // Set message
            PresenceMessage.Text = "";

            // Enable presence controls
            DisplayNameLabel.Visible = true;
            DisplayName.Visible = true;
            PresenceButton.Visible = true;

            // Set the presence ID field
            PresenceID.Value = id;

            // Display Save message
            PresenceSaveMessage.Visible = true;

            // Add validation
            DisplayNameRequired.Enabled = true;

            // Make sure the panel is visible
            CssStyleCollection styleCollection = PresencePanel.Style;
            styleCollection["display"] = "block";

        }
        else if (result == "Declined")
        {
            // Set message
            PresenceMessage.Text = "Online presence has been declined.";

            // Enable save if presence settings exist
            PresenceButton.Enabled = true; // PresenceExists

            // Clear and disable presence fields
            DisplayNameLabel.Visible = false;
            DisplayName.Visible = false;
            DisplayName.Text = "";
            PresenceID.Value = "";

            // Display Save message
            PresenceSaveMessage.Visible = true;

            // Remove validation
            DisplayNameRequired.Enabled = false;

            // Make sure the panel is visible
            CssStyleCollection styleCollection = PresencePanel.Style;
            styleCollection["display"] = "block";

        }
        else if (result == "NoPrivacyUrl")
        {
        }
    }

    private void ProcessAuthenticationRequest(string action)
    {
        if (action == "delauth")
        {
            Utilities.SetAppConsentToken(WLLogin.ProcessConsent(Request.Form), false);
        }
    }

    private string GetConfigurationSetting(NameValueCollection settings, string name)
    {
        string setting = settings[name];

        if (setting != null)
        {
            return setting;
        }
        else
        {
            return string.Empty;
        }
    }

    private void SetConfigurationSetting(Configuration config, string name, string value)
    {
        KeyValueConfigurationElement setting = config.AppSettings.Settings[name];

        if (setting != null)
        {
            setting.Value = value;
        }
        else
        {
            config.AppSettings.Settings.Add(name, value);
        }
    }

    private string GetPageUrl()
    {
        string uri = Request.Url.AbsoluteUri;
        int idx = uri.LastIndexOf('?');
        return uri.Substring(0, idx > 0 ? idx : uri.Length);
    }

    private string GetPresenceUrl()
    {
        string uri = GetPageUrl();
        string path = uri.Remove(uri.LastIndexOf('/'));
        return string.Format(
            "http://settings.messenger.live.com/applications/websignup.aspx?returnurl={0}&privacyurl={1}",
            AntiXss.UrlEncode(uri), AntiXss.UrlEncode(path + "/Privacy.aspx"));
    }

    private WindowsLiveLogin.User WindowsLiveUser
    {
        get
        {
            // If the user token obtained from sign-in through Web Authentication 
            // has been cached in a site cookie, attempt to process it and extract 
            // the user ID.
            HttpCookie loginCookie = Request.Cookies["webauthtoken"];

            if (loginCookie != null)
            {
                return WLLogin.ProcessToken(loginCookie.Value);
            }
            else
            {
                return null;
            }
        }
    }
}
