using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Management;
using System.Web;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class HealthMonitoringManager
    {
        public static void LogError(string format, params object[] args)
        {
            HealthMonitoringManager.LogError(null, format, args);
        }

        public static void LogError(Exception exception, string format, params object[] args)
        {
            HealthMonitoringManager.LogError(exception, string.Format(format, args));
        }

        public static void LogError(string message)
        {
            HealthMonitoringManager.LogError(null, message);
        }

        public static void LogError(Exception exception, string message)
        {
            if (HttpContext.Current == null) { return; }
            if (exception == null) { exception = new Exception(message); }

            CustomWebErrorEvent errorEvent = new CustomWebErrorEvent(message, exception);
            errorEvent.Raise();

            HttpContext.Current.Trace.Warn("Custom Web Event", message, exception);
        }

        public static void LogWarning(string format, params object[] args)
        {
            HealthMonitoringManager.LogWarning(null, format, args);
        }

        public static void LogWarning(Exception exception, string format, params object[] args)
        {
            HealthMonitoringManager.LogWarning(exception, string.Format(format, args));
        }

        public static void LogWarning(string message)
        {
            HealthMonitoringManager.LogWarning(null, message);
        }

        public static void LogWarning(Exception exception, string message)
        {
            if (HttpContext.Current == null) { return; }
            if (exception == null) { exception = new Exception(message); }

            CustomWebWarningEvent warningEvent = new CustomWebWarningEvent(message, exception);
            warningEvent.Raise();

            HttpContext.Current.Trace.Warn("Custom Web Event", message, exception);
        }

        public static void LogInfo(string format, params object[] args)
        {
            HealthMonitoringManager.LogInfo(string.Format(format, args));
        }

        public static void LogInfo(string message)
        {
            if (HttpContext.Current == null) { return; }

            CustomWebInfoEvent infoEvent = new CustomWebInfoEvent(message);
            infoEvent.Raise();

            HttpContext.Current.Trace.Write("Custom Web Event", message);
        }

        private class CustomWebErrorEvent : WebErrorEvent
        {
            public CustomWebErrorEvent(string message, Exception exception)
                : base(message, null, WebEventCodes.WebExtendedBase + 1, exception)
            {
            }
        }

        private class CustomWebWarningEvent : WebErrorEvent
        {
            public CustomWebWarningEvent(string message, Exception exception)
                : base(message, null, WebEventCodes.WebExtendedBase + 2, exception)
            {
            }
        }

        private class CustomWebInfoEvent : WebBaseEvent
        {
            public CustomWebInfoEvent(string message)
                : base(message, null, WebEventCodes.WebExtendedBase + 3)
            {
            }
        }
    }
}
