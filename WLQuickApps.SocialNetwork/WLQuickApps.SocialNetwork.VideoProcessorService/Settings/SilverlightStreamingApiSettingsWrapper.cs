using System;
using System.Collections.Generic;
using System.Text;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    sealed internal class SilverlightStreamingApiSettingsWrapper
    {
        static Settings.SilverlightStreamingApiSettings _settings;

        static SilverlightStreamingApiSettingsWrapper()
        {
            _settings = new Settings.SilverlightStreamingApiSettings();
        }

        private SilverlightStreamingApiSettingsWrapper()
        { }

        internal static string ServiceRoot
        {
            get
            {
                return _settings.ServiceRoot;
            }
        }
    }
}
