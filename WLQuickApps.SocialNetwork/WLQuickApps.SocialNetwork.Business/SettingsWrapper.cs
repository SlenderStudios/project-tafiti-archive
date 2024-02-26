using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace WLQuickApps.SocialNetwork.Business
{
    internal class SettingsWrapper
    {
        static public string MapPointUserName { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.MapPointUserName]; } }
        static public string MapPointPassword { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.MapPointPassword]; } }
        static public string MapPointDataSource { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.MapPointDataSource]; } }
        static public string SilverlightStreamingIDPrefix { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.SilverlightStreamingIDPrefix]; } }
        static public string SilverlightStreamingUserName { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.SilverlightStreamingUserName]; } }
        static public string SilverlightStreamingPassword { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.SilverlightStreamingPassword]; } }
        static public bool AutomaticallyApproveNewUsers { get { return Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AppSettingKeys.AutomaticallyApproveNewUsers]); } }
        static public bool AutomaticallyApproveNewGroups { get { return Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AppSettingKeys.AutomaticallyApproveNewGroups]); } }
        static public bool AutomaticallyApproveNewMedia { get { return Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AppSettingKeys.AutomaticallyApproveNewMedia]); } }
        static public bool AutomaticallyApproveNewCollections { get { return Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AppSettingKeys.AutomaticallyApproveNewCollections]); } }
        static public string LiveAlertsMessageUrl { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.LiveAlertsMessageUrl]; } }
        static public string LiveAlertsSubscriptionUrl { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.LiveAlertsSubscriptionUrl]; } }
        static public string LiveAlertsPassword { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.LiveAlertsPassword]; } }
        static public int LiveAlertsPin { get { return Convert.ToInt32(ConfigurationManager.AppSettings[Constants.AppSettingKeys.LiveAlertsPin]); } }
        static public string MediaDropPath { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.MediaDropPath]; } }
        static public string ProcessorQueuePath { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.ProcessorQueuePath]; } }
        static public string MapPointFindServiceUrl { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.MapPointFindServiceUrl]; } }
        static public bool EnableLiveAlerts { get { return Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AppSettingKeys.EnableLiveAlerts]); } }
        static public string ExpressionMediaEncoderPath { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.ExpressionMediaEncoderPath]; } }
        static public int MediaThumbnailTimeout { get { return Convert.ToInt32(ConfigurationManager.AppSettings[Constants.AppSettingKeys.MediaThumbnailTimeout]); } }
        static public string SiteFromEmailAddress { get { return ConfigurationManager.AppSettings[Constants.AppSettingKeys.SiteFromEmailAddress]; } }
    
    }
}
