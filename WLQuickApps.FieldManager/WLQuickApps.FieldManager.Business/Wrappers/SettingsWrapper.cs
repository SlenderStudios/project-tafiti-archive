using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace WLQuickApps.FieldManager.Business
{
    internal class SettingsWrapper
    {
        static public bool EnableLiveAlerts { get { return Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AppSettingKeys.EnableLiveAlerts]); } }
        static public string LiveAlertsMessageUrl { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.LiveAlertsMessageUrl]; } }
        static public string LiveAlertsPassword { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.LiveAlertsPassword]; } }
        static public int LiveAlertsPin { get { return Convert.ToInt32(ConfigurationManager.AppSettings[Constants.AppSettingKeys.LiveAlertsPin]); } }
        static public string LiveAlertsSubscriptionUrl { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.LiveAlertsSubscriptionUrl]; } }
        static public string ConnectionString
        {
            get
            {
                return (System.Configuration.ConfigurationManager.ConnectionStrings["default"].ConnectionString);
            }
        }
    }
}
