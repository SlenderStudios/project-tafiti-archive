using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    sealed public class EventLogWrapper
    {
        /// <summary>
        /// Indicates whether errors are being logged. If you're an external consumer, this is provided as
        /// info only and shouldn't impact your code. In other words, always log and let the library sort it
        /// out.
        /// </summary>
        static public bool IsLoggingErrors { get { return (MediaProcessorSettingsWrapper.EventLogThreshold >= 0); } }

        /// <summary>
        /// Indicates whether warnings are being logged. If you're an external consumer, this is provided as
        /// info only and shouldn't impact your code. In other words, always log and let the library sort it
        /// out.
        /// </summary>
        static public bool IsLoggingWarnings { get { return (MediaProcessorSettingsWrapper.EventLogThreshold >= 1); } }

        /// <summary>
        /// Indicates whether info messages are being logged. If you're an external consumer, this is provided as
        /// info only and shouldn't impact your code. In other words, always log and let the library sort it
        /// out.
        /// </summary>
        static public bool IsLoggingInfo { get { return (MediaProcessorSettingsWrapper.EventLogThreshold >= 2); } }

        /// <summary>
        /// The internal event log being used by the library. This is being provided publicly for informational 
        /// purposes and should not be changed by user code. Please use the LogError, LogWarning, and LogInfo
        /// methods to write to the event log.
        /// </summary>
        static public EventLog Log
        {
            get
            {
                if (EventLogWrapper._eventLog == null)
                {
                    // If there's a log with our source name, delete it.
                    if (EventLog.Exists(MediaProcessorSettingsWrapper.EventLogSourceName, MediaProcessorSettingsWrapper.EventLogMachineName))
                    {
                        EventLog.Delete(MediaProcessorSettingsWrapper.EventLogSourceName, MediaProcessorSettingsWrapper.EventLogMachineName);
                    }

                    // If the source exists, but is associated with the wrong log, delete it.
                    if (EventLog.SourceExists(MediaProcessorSettingsWrapper.EventLogSourceName, MediaProcessorSettingsWrapper.EventLogMachineName) &&
                        (EventLog.LogNameFromSourceName(MediaProcessorSettingsWrapper.EventLogSourceName, MediaProcessorSettingsWrapper.EventLogMachineName) != MediaProcessorSettingsWrapper.EventLogName))
                    {
                        EventLog.Delete(MediaProcessorSettingsWrapper.EventLogSourceName, MediaProcessorSettingsWrapper.EventLogMachineName);
                    }

                    // Create the source, if it does not already exist.
                    if (!EventLog.SourceExists(MediaProcessorSettingsWrapper.EventLogSourceName, MediaProcessorSettingsWrapper.EventLogMachineName))
                    {
                        EventSourceCreationData sourceData = new EventSourceCreationData(MediaProcessorSettingsWrapper.EventLogSourceName, MediaProcessorSettingsWrapper.EventLogName);
                        sourceData.MachineName = MediaProcessorSettingsWrapper.EventLogMachineName;
                        EventLog.CreateEventSource(sourceData);
                    }

                    EventLogWrapper._eventLog = new EventLog(MediaProcessorSettingsWrapper.EventLogName, MediaProcessorSettingsWrapper.EventLogMachineName);
                    EventLogWrapper._eventLog.Source = MediaProcessorSettingsWrapper.EventLogSourceName;
                    EventLogWrapper._eventLog.ModifyOverflowPolicy(OverflowAction.OverwriteAsNeeded, 0);
                }

                return EventLogWrapper._eventLog;
            }
        }
        static private EventLog _eventLog;

        private EventLogWrapper() { }

        /// <summary>
        /// Logs an error to the event log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        static public void LogError(string message)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Error, message);
        }

        /// <summary>
        /// Logs a warning to the event log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        static public void LogWarning(string message)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Warning, message);
        }

        /// <summary>
        /// Logs an info message to the event log.
        /// </summary>
        /// <param name="message">The message to log.</param>
        static public void LogInfo(string message)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Information, message);
        }

        /// <summary>
        /// Logs an error to the event log.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The message to log.</param>
        static public void LogError(Exception exception, string message)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Error, exception, message);
        }

        /// <summary>
        /// Logs a warning to the event log.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The message to log.</param>
        static public void LogWarning(Exception exception, string message)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Warning, exception, message);
        }

        /// <summary>
        /// Logs an info message to the event log.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="message">The message to log.</param>
        static public void LogInfo(Exception exception, string message)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Information, exception, message);
        }

        /// <summary>
        /// Logs an error to the event log.
        /// </summary>
        /// <param name="format">The format specifier to be passed to string.Format.</param>
        /// <param name="args">The parameters to be passed to string.Format.</param>
        static public void LogError(string format, params object[] args)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Error, format, args);
        }

        /// <summary>
        /// Logs a warning message to the event log.
        /// </summary>
        /// <param name="format">The format specifier to be passed to string.Format.</param>
        /// <param name="args">The parameters to be passed to string.Format.</param>
        static public void LogWarning(string format, params object[] args)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Warning, format, args);
        }

        /// <summary>
        /// Logs an info message to the event log.
        /// </summary>
        /// <param name="format">The format specifier to be passed to string.Format.</param>
        /// <param name="args">The parameters to be passed to string.Format.</param>
        static public void LogInfo(string format, params object[] args)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Information, format, args);
        }

        /// <summary>
        /// Logs an error to the event log.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The format specifier to be passed to string.Format.</param>
        /// <param name="args">The parameters to be passed to string.Format.</param>
        static public void LogError(Exception exception, string format, params object[] args)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Error, exception, format, args);
        }

        /// <summary>
        /// Logs a warning message to the event log.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The format specifier to be passed to string.Format.</param>
        /// <param name="args">The parameters to be passed to string.Format.</param>
        static public void LogWarning(Exception exception, string format, params object[] args)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Warning, exception, format, args);
        }

        /// <summary>
        /// Logs an info message to the event log.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        /// <param name="format">The format specifier to be passed to string.Format.</param>
        /// <param name="args">The parameters to be passed to string.Format.</param>
        static public void LogInfo(Exception exception, string format, params object[] args)
        {
            EventLogWrapper.LogMessage(EventLogEntryType.Information, exception, format, args);
        }

        static private void LogMessage(EventLogEntryType type, string message)
        {
            // Make sure we're logging this type. Note that this could be done earlier in the call stack,
            // but putting it here guarantees we're performing the check and only does it once. 
            switch (type)
            {
                case EventLogEntryType.Error:
                    if (!EventLogWrapper.IsLoggingErrors)
                    {
                        return;
                    }
                    break;
                case EventLogEntryType.Warning:
                    if (!EventLogWrapper.IsLoggingWarnings)
                    {
                        return;
                    }
                    break;
                case EventLogEntryType.Information:
                    if (!EventLogWrapper.IsLoggingInfo)
                    {
                        return;
                    }
                    break;
                // If we have a different type being logged, we'll punt to avoid any mismatch errors.
                default: return;
            }

            // Note that we're swallowing exceptions here since there's nothing the caller
            // can do about it. The only exceptions we should see here are related to 
            // the event log being full, etc.
            try
            {
                EventLogWrapper.Log.WriteEntry(message, type);
            }
            catch (Exception e)
            {
                EventLogWrapper.HandleException(e);
            }
        }

        static private void LogMessage(EventLogEntryType type, string format, params object[] args)
        {
            // Note that we're swallowing exceptions here since there's nothing the caller
            // can do about it. The only exception we should see here is related to the string
            // formatting.
            try
            {
                EventLogWrapper.LogMessage(type, string.Format(format, args));
            }
            catch (Exception e)
            {
                EventLogWrapper.HandleException(e);
            }
        }

        static private void LogMessage(EventLogEntryType type, Exception exception, string message)
        {
            // Note that we're swallowing exceptions here since there's nothing the caller
            // can do about it. The only exception we should see here is related to the string
            // formatting.
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendFormat("{0}{1}{2}", message, Environment.NewLine, exception.ToString());
                if (exception.InnerException != null)
                {
                    stringBuilder.AppendFormat("{0}Inner Exception:{1}{2}", Environment.NewLine, Environment.NewLine, exception.InnerException.ToString());
                }
                EventLogWrapper.LogMessage(type, stringBuilder.ToString());
            }
            catch (Exception e)
            {
                EventLogWrapper.HandleException(e);
            }
        }

        static private void LogMessage(EventLogEntryType type, Exception exception, string format, params object[] args)
        {
            // Note that we're swallowing exceptions here since there's nothing the caller
            // can do about it. The only exception we should see here is related to the string
            // formatting.
            try
            {
                string message = string.Format(format, args);
                EventLogWrapper.LogMessage(type, exception, message);
            }
            catch (Exception e)
            {
                EventLogWrapper.HandleException(e);
            }
        }

        static private void HandleException(Exception e)
        {
            // Uncomment this for debugging help.
            //            Debug.Fail(e.Message);
        }

    }

}
