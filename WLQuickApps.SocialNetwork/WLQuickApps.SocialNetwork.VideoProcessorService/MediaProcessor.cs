using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Timers;
using System.IO;
using System.Threading;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    // Note: using Expression Media Encoder Preview Version 1.0.2780.0.
    public class MediaProcessor
    {
        string _sourceArg = string.Empty;
        string _targetArg = string.Empty;
        string _processorArgumentTemplate = string.Empty;
        
        int _processorTimeout = 0;

        public MediaProcessor(string sourceArg, string targetArg, string argTemplate, int timeout)
        {
            if (sourceArg == null)
            { throw new ArgumentNullException("sourceArg"); }
            if (targetArg == null)
            { throw new ArgumentNullException("targetArg"); }
            if (argTemplate == null)
            { throw new ArgumentNullException("argTemplate"); }
            if (timeout < 0)
            { throw new ArgumentOutOfRangeException(Resources.ErrorStringsResource.InvalidTimeoutValue); }

            this._sourceArg = sourceArg;
            this._targetArg = targetArg;
            this._processorArgumentTemplate = argTemplate;

            this._processorTimeout = timeout;
        }

        public VideoProcessorResult ProcessMedia(string presetsFileName)
        {
            if (presetsFileName == null)
            { throw new ArgumentNullException("presetsFileName"); }

            string args = this._processorArgumentTemplate.Replace(
                Constants.EMESourceArgVariable, this._sourceArg);
            args = args.Replace(
                Constants.EMETargetArgVariable, this._targetArg);
            args = args.Replace(
                Constants.EMEPresetArgVariable, presetsFileName);

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Arguments = args;
            psi.CreateNoWindow = true;
            psi.FileName = MediaProcessorSettingsWrapper.CommandLineTarget;
            psi.UseShellExecute = false;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.RedirectStandardError = true;

            Process processor = new Process();
            processor.StartInfo = psi;

            try
            {
                processor.Start();
            }
            catch (Exception ex)
            {
                string msg = string.Format("Failed to start media processor {0} with the following args: \n{1}", 
                    psi.FileName, psi.Arguments);
                throw new MediaProcessingException(msg, ex);
            }

            ProcessTimeoutMonitor timeoutThreadMonitor = new ProcessTimeoutMonitor(processor, this._processorTimeout);
            Thread timeoutThread = new Thread(timeoutThreadMonitor.MonitorThread);
            timeoutThread.Start();
            

            StringBuilder stdErrorSb = new StringBuilder();

            while (!processor.HasExited)
            {
                stdErrorSb.Append(processor.StandardError.ReadToEnd());
            }
            stdErrorSb.Append(processor.StandardError.ReadToEnd());

            timeoutThread.Join();

            //Note: EME process will use exit code 0 even if there is an error, so we
            //can't rely upon that to indicate success or failure. For now the best option
            //seems to be checking to see if standard error has text.
            string stdError = string.Empty;
            if (timeoutThreadMonitor.TimeoutFlag)
            {
                stdError = Resources.ErrorStringsResource.ProcessingTimeout;
            }
            else
            {
                stdError = stdErrorSb.ToString();
            }
            
            if (stdError != string.Empty)
            {
                // Error condition.
                return new VideoProcessorResult(false, stdErrorSb.ToString(), psi);
            }
            else
            {
                // Success condition.
                return new VideoProcessorResult(true, string.Empty, psi);
            }

            //int exitCode = processor.ExitCode;
            //if (exitCode == 0)
            //{
            //    return new VideoProcessorResult(true, string.Empty, psi);
            //}
            //else
            //{
            //    return new VideoProcessorResult(false, stdErrorSb.ToString(), psi);
            //}
        }

    }

   
    public class VideoProcessorResult
    {
        bool _success;
        string _stdErrorText;
        ProcessStartInfo _psi;

        public VideoProcessorResult(bool success, string stdError, ProcessStartInfo psi)
        {
            if (stdError == null)
            { throw new ArgumentNullException("stdError"); }
            if (psi == null)
            { throw new ArgumentNullException("psi"); }

            this._success = success;
            this._stdErrorText = stdError;
            this._psi = psi;
        }

        public ProcessStartInfo ProcessInfo
        {
            get
            {
                return this._psi;
            }
        }

        public bool Success
        {
            get
            {
                return this._success;
            }
        }

        public string StdErrorText
        {
            get
            {
                return this._stdErrorText;
            }
        }
    }

    internal class ProcessTimeoutMonitor
    {
        Process _process = null;
        int _timeout;
        bool _timeoutFlag = false;

        public ProcessTimeoutMonitor(Process process, int timeout)
        {
            if (process == null)
            { throw new ArgumentNullException("process"); }

            this._process = process;
            this._timeout = timeout;
        }

        public void MonitorThread()
        {
            if (this._process == null)
            {
                return;
            }

            try
            {
                while (!this._process.HasExited)
                {
                    TimeSpan timeSpan = DateTime.Now - this._process.StartTime;
                    if (timeSpan.TotalMinutes > this._timeout)
                    {
                        try
                        {
                            // In case of timeout, we attempt to kill the process.
                            // Note: we catch InvalidOperationException because the 
                            // process may have exited before we attempted the kill.
                            this._process.Kill();
                            this._process.WaitForExit();
                            this._timeoutFlag = true;
                        }
                        catch (InvalidOperationException)
                        {
                            // This could happen if the process ends before we kill it.
                        }
                        finally
                        {
                            string msg = string.Format("Media processor process was killed due to a configured timeout of {0} minute(s).",
                               this._timeout);
                            EventLogWrapper.LogWarning(msg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("Unexpected exception occurred in thread responsible for monitoring timeout.\n{0}",
                    ex.Message);
                EventLogWrapper.LogWarning(msg);
            }
        }

        public bool TimeoutFlag
        {
            get
            {
                return this._timeoutFlag;
            }
        }
    }
   
}
