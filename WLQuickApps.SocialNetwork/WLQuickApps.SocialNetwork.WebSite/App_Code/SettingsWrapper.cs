using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.WebSite
{
    public class SettingsWrapper
    {
        private SettingsWrapper()
        {
        }

        static private string GetSettingsString(string name)
        {
            // The ToString() is added here to force an exception if the item doesn't exist.
            return ConfigurationManager.AppSettings[name].ToString();
        }

        static private bool GetSettingsBool(string name)
        {
            return bool.Parse(ConfigurationManager.AppSettings[name]);
        }

        static private int GetSettingsInt(string name)
        {
            return int.Parse(ConfigurationManager.AppSettings[name]);
        }

        public static string[] SpecialGroups
        {
            get
            {
                try
                {
                    if (SettingsWrapper._specialGroups == null)
                    {
                        SettingsWrapper._specialGroups = SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.SpecialGroups).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("A SpecialGroups setting was not found in Web.config, so no special groups will be used.");
                    SettingsWrapper._specialGroups = new string[0];
                }
                return SettingsWrapper._specialGroups;
            }
        }
        private static string[] _specialGroups;

        public static string[] SpecialEvents
        {
            get
            {
                try
                {
                    if (SettingsWrapper._specialEvents == null)
                    {
                        SettingsWrapper._specialEvents = SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.SpecialEvents).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("A SpecialEvents setting was not found in Web.config, so no special events will be used.");
                    SettingsWrapper._specialEvents = new string[0];
                }
                return SettingsWrapper._specialEvents;
            }
        }
        private static string[] _specialEvents;

        public static string[] SpecialForums
        {
            get
            {
                try
                {
                    if (SettingsWrapper._specialForums == null)
                    {
                        SettingsWrapper._specialForums = SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.SpecialForums).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("A SpecialForums setting was not found in Web.config, so no special forums will be used.");
                    SettingsWrapper._specialForums = new string[0];
                }
                return SettingsWrapper._specialForums;
            }
        }
        private static string[] _specialForums;

        public static string SiteTitle
        {
            get
            {
                try
                {
                    if (SettingsWrapper._siteTitle == null)
                    {
                        SettingsWrapper._siteTitle = SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.SiteTitle);
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("A SiteTitle setting was not found in Web.config, so the default value will be used.");
                    SettingsWrapper._siteTitle = string.Empty;
                }
                return SettingsWrapper._siteTitle;
            }
        }
        private static string _siteTitle;

        public static string FeedbackEmail
        {
            get
            {
                try
                {
                    if (SettingsWrapper._feedbackEmail == null)
                    {
                        SettingsWrapper._feedbackEmail = SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.FeedbackEmail);
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("A FeedbackEmail setting was not found in Web.config, so the default value will be used.");
                    SettingsWrapper._feedbackEmail = string.Empty;
                }
                return SettingsWrapper._feedbackEmail;
            }
        }
        private static string _feedbackEmail;

        public static string P3PCompactPolicy
        {
            get
            {
                try
                {
                    if (SettingsWrapper._p3PCompactPolicy == null)
                    {
                        SettingsWrapper._p3PCompactPolicy = SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.P3PCompactPolicy);
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("A P3PCompactPolicy setting was not found in Web.config, so the default value will be used.");
                    SettingsWrapper._p3PCompactPolicy = string.Empty;
                }
                return SettingsWrapper._p3PCompactPolicy;
            }
        }
        private static string _p3PCompactPolicy;

        public static string WindowsLiveAppID
        {
            get
            {
                try
                {
                    if (SettingsWrapper._windowsLiveAppID == null)
                    {
                        SettingsWrapper._windowsLiveAppID = SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.WindowsLiveAppID);
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("An WindowsLiveAppID setting was not found in Web.config, so the default value will be used.");
                    SettingsWrapper._windowsLiveAppID = string.Empty;
                }
                return SettingsWrapper._windowsLiveAppID;
            }
        }
        private static string _windowsLiveAppID;

        public static string WindowsLiveSecret
        {
            get
            {
                try
                {
                    if (SettingsWrapper._windowsLiveSecret == null)
                    {
                        SettingsWrapper._windowsLiveSecret = SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.WindowsLiveSecret);
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("An WindowsLiveSecret setting was not found in Web.config, so the default value will be used.");
                    SettingsWrapper._windowsLiveSecret = string.Empty;
                }
                return SettingsWrapper._windowsLiveSecret;
            }
        }
        private static string _windowsLiveSecret;

        public static string LiveAlertsChangeUrl
        {
            get
            {
                try
                {
                    if (SettingsWrapper._liveAlertsChangeUrl == null)
                    {
                        SettingsWrapper._liveAlertsChangeUrl = SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.LiveAlertsChangeUrl);
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("An LiveAlertsChangeUrl setting was not found in Web.config, so the default value will be used.");
                    SettingsWrapper._liveAlertsChangeUrl = string.Empty;
                }
                return SettingsWrapper._liveAlertsChangeUrl;
            }
        }
        private static string _liveAlertsChangeUrl;

        public static bool EnableLiveAlerts
        {
            get
            {
                try
                {
                    if (SettingsWrapper._enableLiveAlerts == false)
                    {
                        SettingsWrapper._enableLiveAlerts = bool.Parse(SettingsWrapper.GetSettingsString(WebConstants.AppSettingKeys.EnableLiveAlerts));
                    }
                }
                catch (NullReferenceException)
                {
                    HealthMonitoringManager.LogWarning("An EnableLiveAlerts setting was not found in Web.config, so the default value will be used.");
                    SettingsWrapper._enableLiveAlerts = false;
                }
                return SettingsWrapper._enableLiveAlerts;
            }
        }
        private static bool _enableLiveAlerts;

        static public string SilverlightStreamingIDPrefix
        {
            get
            {
                return ConfigurationManager.AppSettings[Constants.AppSettingKeys.SilverlightStreamingIDPrefix];
            }
        }

        static public string SilverlightStreamingUserName
        {
            get
            {
                return ConfigurationManager.AppSettings[Constants.AppSettingKeys.SilverlightStreamingUserName];
            }
        }
        static public string PrivacyStatementOverrideUrl
        {
            get
            {
                return ConfigurationManager.AppSettings[Constants.AppSettingKeys.PrivacyStatementOverrideUrl];
            }
        }
        static public string HostingLabel
        {
            get
            {
                return ConfigurationManager.AppSettings[Constants.AppSettingKeys.HostingLabel];
            }
        }

        static public string MicrosoftAnalyticsID
        {
            get
            {
                return ConfigurationManager.AppSettings[WebConstants.AppSettingKeys.MicrosoftAnalyticsID];
            }
        }

    }
}
