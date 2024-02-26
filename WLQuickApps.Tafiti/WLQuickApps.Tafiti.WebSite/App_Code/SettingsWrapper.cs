using System;
using System.Data;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WLQuickApps.Tafiti.WebSite
{
    /// <summary>
    /// Summary description for SettingsWrapper
    /// </summary>
    sealed public class SettingsWrapper
    {
        private SettingsWrapper() { }

        static public bool SendEmail { get { return Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.SendEmail]); } }
        static public string HostSite { get { return ConfigurationManager.AppSettings[Constants.AppSettingsKeys.HostSite]; } }        
        static public string SiteEmail { get { return ConfigurationManager.AppSettings[Constants.AppSettingsKeys.SiteEmail]; } }
        static public string LiveSearchAppID { get { return ConfigurationManager.AppSettings[Constants.AppSettingsKeys.LiveSearchAppID]; } }
        static public string LiveAnalyticsID { get { return ConfigurationManager.AppSettings[Constants.AppSettingsKeys.LiveAnalyticsID]; } }
        static public bool LiveAnalyticsEnabled { get { return !string.IsNullOrEmpty(SettingsWrapper.LiveAnalyticsID); } }
        static public string LiveAuthID { get { return ConfigurationManager.AppSettings[Constants.AppSettingsKeys.LiveAuthID]; } }
        
        
    }
}