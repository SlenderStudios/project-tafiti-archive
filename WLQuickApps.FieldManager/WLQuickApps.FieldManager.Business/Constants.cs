using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WLQuickApps.FieldManager.Business
{
    sealed internal class Constants
    {
        private Constants() { }

        sealed public class AppSettingKeys
        {
            private AppSettingKeys() { }

            public const string EnableLiveAlerts = "EnableLiveAlerts";
            public const string LiveAlertsMessageUrl = "LiveAlertsMessageUrl";
            public const string LiveAlertsPassword = "LiveAlertsPassword";
            public const string LiveAlertsPin = "LiveAlertsPin";
            public const string LiveAlertsSubscriptionUrl = "LiveAlertsSubscriptionUrl";
        }

        sealed public class ContextCacheKeys
        {
            public const string LoggedInUser = "LoggedInUser";
        }

    }
}