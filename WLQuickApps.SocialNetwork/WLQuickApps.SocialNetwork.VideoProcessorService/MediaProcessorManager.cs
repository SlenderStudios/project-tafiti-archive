using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using System.IO;
using System.Net;


namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    // This class will grab a video to process and store it locally, transcode
    // the video into a format good for Silverlight Streaming, create all files
    // necessary to define a Silverlight application player for the video, zip
    // the necessary application files, and finally upload the video to a 
    // Silverlight Streaming account.
    public class MediaProcessorManager
    {
        string _workingDirectory = string.Empty;
        
        Timer _checkQueueTimer;
        static object syncObject = new object();

        // Set debug flag in configuration file. If set to 'true', source submission
        // files and processor working files will not be deleted.
        bool _debugFlag = false;

        DateTime start;
        DateTime end;
        TimeSpan total;


        public MediaProcessorManager()
        {

        }

        public void Start()
        {
            //TODO: validate application configuration file values.

            this._workingDirectory = MediaProcessorSettingsWrapper.ProcessorWorkingDir;
            this._debugFlag = MediaProcessorSettingsWrapper.DebugFlag;

            if (this._debugFlag)
            {
                EventLogWrapper.LogWarning("Note: Debug flag is configured to 'true'.");
            }

            if (!Directory.Exists(this._workingDirectory))
            {
                Directory.CreateDirectory(this._workingDirectory);
            }

            this._checkQueueTimer = new Timer(Constants.SubmissionQueueCheckInterval);
            this._checkQueueTimer.Elapsed += new ElapsedEventHandler(_checkQueueTimer_Elapsed);
            this._checkQueueTimer.AutoReset = false;
            this._checkQueueTimer.Enabled = true;
            this._checkQueueTimer.Start();
        }

        void _checkQueueTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (syncObject)
            {
                // the following 'if' statement for debugging only
                //if (this._debugFlag)
                //{
                //    this._checkQueueTimer.Stop();

                //    if (!System.Diagnostics.Debugger.IsAttached)
                //    {
                //        System.Diagnostics.Debugger.Launch();
                //    }

                //    System.Diagnostics.Debugger.Break();
                //}


                try
                {
                    this._checkQueueTimer.Stop();
                    this._checkQueueTimer.Enabled = false;

                    ProcessNextSubmission();
                }
                catch (Exception ex)
                {
                    //log error
                    string msg = string.Format(
                     "An unhandled exception occurred during the media processing cycle.\nMessage:\n{0}\nStackTrace:\n{1}\nInnerMessage:\n{2}",
                     ex.Message, ex.StackTrace, (ex.InnerException==null? "null":ex.InnerException.Message));
                    EventLogWrapper.LogError(msg);

                    System.Threading.Thread.Sleep(Constants.PauseAfterError);
                }
                finally
                {
                    try
                    {
                        // clean up after working with submission....
                        CleanProcessorDir();
                    }
                    finally
                    {
                        if (this._checkQueueTimer != null)
                        {
                            this._checkQueueTimer.Enabled = true;
                            this._checkQueueTimer.Start();
                        }
                    }
                }

            }
        }

        //TODO: this method needs a lot more error checking.
        private void ProcessNextSubmission()
        {
            string msg = string.Empty;
            string errorPackage = Path.Combine(MediaProcessorSettingsWrapper.ServiceWorkingDir, Constants.SSErrorPackageFile);

            CleanProcessorDir();

            // get submission
            SilverlightStreamingSubmission submission = MediaGenerator.GetNextSubmission();

            start = DateTime.Now;

            if (submission == null)
            { return; }

            SilverlightStreamingAccount account = new SilverlightStreamingAccount(submission.SilverlightStreamingUserName, submission.SilverlightStreamingPassword);

            msg = string.Format("Started processing submission ID#{0}, File={1}, Type={2}.",
                submission.ID, Path.GetFileName(submission.Path), submission.Type.ToString());
            EventLogWrapper.LogInfo(msg);

            // save submission to configured working dir
            string submissionFile = SaveSubmissionToDisk(submission);
            msg = string.Format("Successfully copied submission from {0} to {1}.", submission.Path, submissionFile);
            EventLogWrapper.LogInfo(msg);
            // delete original submission file
            DeleteSubmissionSourceMedia(submission);
            
            
            // construct submission directory (processor working directory + submission ID)
            string submissionDir = Path.Combine(this._workingDirectory, submission.ID);
            // construct submission output file name (takes original filename, adds "_out", and
            // changes extension to the appropriate value).
            string outFileName = Path.GetFileNameWithoutExtension(submissionFile) + Constants.EMETargetFileAppend;
            if (submission.Type == MediaType.Video)
            {
                outFileName = outFileName + Constants.EMETargetVideoExtension;
            }
            else if (submission.Type == MediaType.Audio)
            {
                outFileName = outFileName + Constants.EMETargetAudioExtension;
            }
            // construct full path to target output file (using previously constructed submission directory
            // and output file name strings.
            string targetFile = Path.Combine(submissionDir, outFileName);
            

            // process submission
            MediaProcessor processor = new MediaProcessor(
                @"""" + submissionFile + @"""",
                @"""" + targetFile + @"""",
                Constants.EMECommandLineTemplate,
                MediaProcessorSettingsWrapper.CommandLineTimeout);

            VideoProcessorResult processResult = null;

            switch (submission.Type)
            {
                case MediaType.Audio:
                    processResult = processor.ProcessMedia(
                        Path.Combine(MediaProcessorSettingsWrapper.ServiceWorkingDir, Constants.EMEPresetFileAudio));
                    break;
                case MediaType.Video:
                    processResult = processor.ProcessMedia(
                        Path.Combine(MediaProcessorSettingsWrapper.ServiceWorkingDir, Constants.EMEPresetFileVideo));
                    break;
            }

            if (!processResult.Success)
            {
                // record the failure with a warning in the log.
                msg = string.Format("Media processing failed for submission with ID#{0}.\nStdError Output: {1}\nTarget: {2}\nArguments: {3}",
                    submission.ID, processResult.StdErrorText, processResult.ProcessInfo.FileName, processResult.ProcessInfo.Arguments);
                EventLogWrapper.LogWarning(msg);
                
                UploadSilverlightApplication(errorPackage, submission, account);

                return;
            }


            string packageFile = Path.Combine(submissionDir, Constants.SSPackageFile);
            try
            {
                CopyManifestTo(Path.Combine(submissionDir, Constants.SSManifestFile));

                PackageOutput(submissionDir, targetFile, submission);
            }
            catch (Exception)
            {
                UploadSilverlightApplication(errorPackage, submission, account);
                msg = string.Format("Media processing failed for submission with ID#{0}.\nStdError Output: {1}\nTarget: {2}\nArguments: {3}",
                    submission.ID, processResult.StdErrorText, processResult.ProcessInfo.FileName, processResult.ProcessInfo.Arguments);
                EventLogWrapper.LogWarning(msg);

                throw;
            }

            UploadSilverlightApplication(packageFile, submission, account);


            end = DateTime.Now;
            total = end - start;

            // TODO: the next log message really belongs in a separate data store.
            msg = string.Format("Successfully finished processing submission ID#{0}.\nTotal processing time: {1} minutes.",
                submission.ID, total.TotalMinutes);
            EventLogWrapper.LogInfo(msg);
        }

        private string SaveSubmissionToDisk(SilverlightStreamingSubmission submission)
        {
            if (submission == null)
            { throw new ArgumentNullException("submission"); }

            // each submission will be placed in a directory named after its ID.
            string submissionDir = Path.Combine(this._workingDirectory, submission.ID);
            // we use the submission directory and original filename to construct the full path
            // to the destination submission file.
            string submissionFile = Path.Combine(submissionDir, Path.GetFileName(submission.Path));

            try
            {
                Directory.CreateDirectory(submissionDir);

                // copy the submission file over to our processing directory.
                using (FileStream destStream = new FileStream(submissionFile, FileMode.Create))
                {
                    using (FileStream srcStream = new FileStream(submission.Path, FileMode.Open))
                    {
                        StreamUtils.StreamCopy(srcStream, destStream);
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = String.Format("Failed to save submission {0} to disk. Source path = {1}, destination path = {2}.", 
                    submission.ID, submission.Path, submissionFile);

                throw new MediaProcessingException(msg, ex);
            }

            return submissionFile;
        }

        private void CopyManifestTo(string destination)
        {
            if (destination == null)
            { throw new ArgumentNullException("destination"); }

            string sourceFile = Path.Combine(MediaProcessorSettingsWrapper.ServiceWorkingDir, Constants.SSManifestFile);
            string destFile = destination;
            try
            {
                File.Copy(sourceFile, destFile, true);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Failed to copy manifest file from {0} to {1}.\n{2}",
                    sourceFile, destFile, ex.Message);
                throw new MediaProcessingException(msg, ex);
            }
        }

        private void PackageOutput(string directory, string outputFile, SilverlightStreamingSubmission submission)
        {
            if (directory == null)
            { throw new ArgumentNullException("directory"); }
            if (outputFile == null)
            { throw new ArgumentNullException("outputFile"); }
            if (submission == null)
            { throw new ArgumentNullException("submission"); }

            List<string> packageFilesList = new List<string>(MediaProcessorSettingsWrapper.EMEFilesToPackage);
            packageFilesList.Add(Path.GetFileName(outputFile));

            string packageFile = Path.Combine(directory, Constants.SSPackageFile);
            try
            {
                MediaPackager.PackageFiles(
                    packageFilesList.ToArray(),
                    directory,
                    packageFile);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Failed to package processed submission files for ID#{0}.\n{1}",
                    submission.ID, ex.Message);
                throw new MediaProcessingException(msg, ex);
            }
            EventLogWrapper.LogInfo(string.Format("Successfully packaged Zip file for submission ID#{0}.", submission.ID));
        }

        private void DeleteSubmissionSourceMedia(SilverlightStreamingSubmission submission)
        {
            if (submission == null)
            { throw new ArgumentNullException("submission"); }

            try
            {
                if (!this._debugFlag)
                {
                    File.SetAttributes(submission.Path, FileAttributes.Normal);
                    File.Delete(submission.Path);
                }
            }
            catch (Exception ex)
            {
                string msg = string.Format("Failed to delete source media file {0} for submission ID#{1}.\n{2}",
                    submission.Path, submission.ID, ex.Message);
                throw new MediaProcessingException(msg, ex);
            }
        }

        private void UploadSilverlightApplication(string package, SilverlightStreamingSubmission submission, SilverlightStreamingAccount account)
        {
            if (package == null)
            { throw new ArgumentNullException("package"); }
            if (submission == null)
            { throw new ArgumentNullException("submission"); }
            if (account == null)
            { throw new ArgumentNullException("account"); }

            // delete old version
            try
            {
                SilverlightStreamingApiWrapper.DeleteApplication(account, submission.ID);
            }
            catch (System.Net.WebException)
            {
                // Want to delete the application if it exists, so ignoring any HTTP
                // errors for now. 
                // TODO: check to see if app. exists first to be cleaner.
            }
            catch (Exception ex)
            {
                string msg = string.Format("Failed to delete package from Silverlight Streaming account for ID#{0}.\n{1}",
                    submission.ID, ex.Message);
                throw new MediaProcessingException(msg, ex);
            }

            //upload package
            try
            {
                SilverlightStreamingApiWrapper.AddApplication(
                            account,
                            submission.ID,
                            package);
            }
            catch (WebException ex)
            {
                string msg = string.Format("Failed to upload package {0} to Silverlight Streaming for submission ID#{1}.\nWebException status = {2}.",
                    package, submission.ID, ex.Status.ToString());

                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    msg = msg + string.Format("\nStatus Code={0}\nStatus Description={1}",
                        ((HttpWebResponse)ex.Response).StatusCode.ToString(), ((HttpWebResponse)ex.Response).StatusDescription.ToString());
                }

                throw new MediaProcessingException(msg, ex);
            }
            catch (Exception ex)
            {
                string msg = string.Format("Failed to upload package {0} to Silverlight Streaming for submission ID#{1}.\n{2}",
                    package, submission.ID, ex.Message);
                throw new MediaProcessingException(msg, ex);
            }
            EventLogWrapper.LogInfo(string.Format("Successfully uploaded package {0} to Silverlight Streaming for submission ID#{1}.", package, submission.ID));
        }

        private void CleanProcessorDir()
        {
            if (this._debugFlag)
            {
                return;
            }

            try
            {
                foreach (string dir in Directory.GetDirectories(MediaProcessorSettingsWrapper.ProcessorWorkingDir))
                {
                    DeleteAllDirectoryFiles(dir);
                    Directory.Delete(dir, true);
                }
            }
            catch (IOException)
            {
                //Some other process is sometimes holding onto files, but the files
                //ARE eventually removed.
            }
        }

        private void DeleteAllDirectoryFiles(string path)
        {
            if (path == null)
            { throw new ArgumentNullException("path"); }

            foreach (string file in Directory.GetFiles(path))
            {
                // Set attributes to normal in case they are read-only, etc.
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
        }

        public void Stop()
        {
            lock (syncObject)
            {
                this._checkQueueTimer.Stop();
                this._checkQueueTimer.Enabled = false;
                this._checkQueueTimer.Dispose();
                this._checkQueueTimer = null;
            }
        }

    }
}
