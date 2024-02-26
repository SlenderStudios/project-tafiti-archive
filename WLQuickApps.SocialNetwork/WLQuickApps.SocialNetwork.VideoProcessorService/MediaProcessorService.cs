using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.IO;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    public partial class MediaProcessorService : ServiceBase
    {
        MediaProcessorManager _manager;


        public MediaProcessorService()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "Failed to create test service: {0}\n{1}",
                    ex.Message, ex.StackTrace);
                EventLogWrapper.LogError(msg);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                // Set the current directory (services start at Windows\System32).
                Directory.SetCurrentDirectory(MediaProcessorSettingsWrapper.ServiceWorkingDir);

                this._manager = new MediaProcessorManager();

                this._manager.Start();

                EventLogWrapper.LogInfo("Started media processing service.");

            }
            catch (Exception ex)
            {
                string msg = string.Format(
                     "Failed to start test service: {0}\n{1}",
                     ex.Message, ex.StackTrace);
                EventLogWrapper.LogError(msg);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (this._manager != null)
                {
                    this._manager.Stop();
                }
                
                EventLogWrapper.LogInfo("Stopped media processing service.");
            }
            catch (Exception ex)
            {
                string msg = string.Format(
                    "Failed to properly stop test service: {0}\n{1}",
                    ex.Message, ex.StackTrace);
                EventLogWrapper.LogError(msg);
            }
        }
    }
}
