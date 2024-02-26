using System;
using System.Collections.Generic;
using System.Text;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    sealed internal class MediaProcessorSettingsWrapper
    {
        static Settings.MediaProcessorSettings _settings;

        static MediaProcessorSettingsWrapper()
        {
            _settings = new Settings.MediaProcessorSettings();
        }

        private MediaProcessorSettingsWrapper()
        { }

        internal static string CommandLineTarget
        {
            get
            {
                return _settings.CommandLineTarget;
            }
        }

        internal static int CommandLineTimeout
        {
            get
            {
                return _settings.CommandLineTimeout;
            }
        }

        internal static string ServiceWorkingDir
        {
            get
            {
                return _settings.ServiceWorkingDir;
            }
        }

        internal static string MediaSubmissionQueue
        {
            get
            {
                return _settings.MediaSubmissionQueue;
            }
        }

        internal static string EventLogMachineName
        {
            get
            {
                return _settings.EventLogMachineName;
            }
        }

        internal static string EventLogName
        {
            get
            {
                return _settings.EventLogName;
            }
        }

        internal static string EventLogSourceName
        {
            get
            {
                return _settings.EventLogSourceName;
            }
        }

        internal static int EventLogThreshold
        {
            get
            {
                return _settings.EventLogThreshold;
            }
        }

        internal static string ProcessorWorkingDir
        {
            get
            {
                return _settings.ProcessorWorkingDir;
            }
        }

        internal static bool DebugFlag
        {
            get
            {
                return _settings.DebugFlag;
            }
        }

        internal static string[] EMEFilesToPackage
        {
            get
            {
                return _settings.EMEFilesToPackage.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
    }
}
