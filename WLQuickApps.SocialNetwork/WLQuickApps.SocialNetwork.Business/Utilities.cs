using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Messaging;

using WLQuickApps.SocialNetwork.Business.Properties;
using System.Transactions;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class Utilities
    {
        static internal TransactionOptions ReadUncommittedTransaction
        {
            get
            {
                TransactionOptions options = new TransactionOptions();
                options.IsolationLevel = IsolationLevel.ReadUncommitted;

                return options;
            }
        }


        static public byte[] ConvertImageToBytes(Image picture)
        {
            if (picture == null) return null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                picture.Save(memoryStream, ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }

        static public Image ConvertBytesToImage(byte[] pictureBytes)
        {
            MemoryStream memoryStream = new MemoryStream(pictureBytes);
            return new Bitmap(memoryStream);
        }

        static public Image GenerateThumbnail(byte[] sourceImageBits, int maxWidth, int maxHeight, bool constrainProportions)
        {
            using (Image image = Utilities.ConvertBytesToImage(sourceImageBits))
            {
                return Utilities.GenerateThumbnail(image, maxWidth, maxHeight, constrainProportions);
            }
        }

        static public string GenerateSilverlightID(int baseItemID)
        {
            return string.Format("{0}{1}", SettingsWrapper.SilverlightStreamingIDPrefix, baseItemID);
        }

        /// <summary>
        /// Generates a thumbnail image.
        /// </summary>
        /// <param name="sourceImage">The source image.</param>
        /// <param name="maxWidth">The maximum width of the thumbnail.</param>
        /// <param name="maxHeight">The maximum height of the thumbnail.</param>
        /// <param name="constrainProportions">True to keep the height and width in proportion with the original.</param>
        /// <returns>The thumbnail image.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the sourceImage parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the maxHeight or maxWidth parameters are not positive.</exception>
        /// <remarks>
        /// If the constrainProportions is true, the resulting thumbnail may not have a height and width of maxHeight and maxWidth,
        /// respectively. Instead, it will be in proportion with the original image.
        /// </remarks>
        /// <example>
        /// <code>
        /// // Generate a 32x32 thumbnail image of "myImage.bmp". Note that the "true" parameter 
        /// // ensures the height and width stay in proportion with the original, so the resulting
        /// // image may not be 32x32.
        /// ThumbnailGenerator.GenerateThumbnail(new Bitmap("myImage.bmp"), 32, 32, true);
        /// </code>
        /// </example>
        static public Image GenerateThumbnail(Image sourceImage, int maxWidth, int maxHeight, bool constrainProportions)
        {
            if (sourceImage == null) { throw new ArgumentNullException("sourceImage"); }
            if ((maxWidth <= 0) || (maxHeight <= 0)) { throw new ArgumentOutOfRangeException("maxWidth and maxHeight must be positive"); }

            // If the image is smaller than the max requested, return a copy of it.
            if ((maxHeight >= sourceImage.Height) && (maxWidth >= sourceImage.Width))
            {
                return new Bitmap(sourceImage);
            }

            // By default, assume we won't constrain proportions.
            int width = maxWidth;
            int height = maxHeight;

            // But if we have to, do it here.
            if (constrainProportions)
            {
                // Determine the ratio between the relative heights and widths (max and original).
                double widthRatio = maxWidth / (sourceImage.Width * 1.0);
                double heightRatio = maxHeight / (sourceImage.Height * 1.0);

                // If the width ratio is smaller, use it to resize the image. Otherwise, use the height ratio.
                if (widthRatio < heightRatio)
                {
                    width = (int)(widthRatio * sourceImage.Width);
                    height = (int)(widthRatio * sourceImage.Height);
                }
                else
                {
                    width = (int)(heightRatio * sourceImage.Width);
                    height = (int)(heightRatio * sourceImage.Height);
                }
            }

            // Create a working image.
            Bitmap bitmap = new Bitmap(width, height);

            // Draw the thumbnail using the highest quality available.
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.InterpolationMode = InterpolationMode.High;
                graphics.DrawImage(sourceImage, 0, 0, width, height);
            }

            // Return the result.
            return bitmap;
        }

        static public void SendMediaToProcessorQueue(string id, string originalFileName, MediaType mediaType, byte[] bits, ref byte[] thumbnailBits)
        {
            // This is a small hack to prevent unit tests from failing.
            if (System.Web.HttpContext.Current == null) { return; }

            Utilities.UploadPackageToSilverlightStreaming(id, Resources.processing);

            // Accessing message queue first so the error will happen early if it doesn't exist.
            using (MessageQueue queue = new MessageQueue(SettingsWrapper.ProcessorQueuePath))
            {
                int extensionIndex = originalFileName.LastIndexOf('.');
                if ((extensionIndex < 0) || (extensionIndex >= (originalFileName.Length - 1))) { throw new ArgumentException("Original file name must have extension"); }
                string extension = originalFileName.Substring(extensionIndex, originalFileName.Length - extensionIndex);

                string newFilePath = string.Format("{0}\\{1}{2}", SettingsWrapper.MediaDropPath, id, extension);

                using (StreamWriter streamWriter = new StreamWriter(newFilePath))
                {
                    streamWriter.BaseStream.Write(bits, 0, bits.Length);
                }

                WLQuickApps.SocialNetwork.VideoProcessorService.MediaType serviceMediaType;

                switch (mediaType)
                {
                    case MediaType.Audio: serviceMediaType = WLQuickApps.SocialNetwork.VideoProcessorService.MediaType.Audio; break;
                    case MediaType.Video: serviceMediaType = WLQuickApps.SocialNetwork.VideoProcessorService.MediaType.Video; break;
                    default: throw new ArgumentException("Media type not supported");
                }

                if (mediaType == MediaType.Video)
                {
                    string outputDirectory = Path.GetTempPath();

                    string jobText = string.Format(
@"<?xml version=""1.0"" encoding=""utf-8""?>
<JobFile Version=""1.0"">
  <Job
    OutputDirectory=""{0}""
    SaveJobFile=""True"" AppendJobID=""False"">
    <MediaFiles>
      <MediaFile
       Source=""{1}""
        EndTime=""00:00:01.0000000""
        ThumbnailMode=""BestFrame""        
        ThumbnailSize=""320, 240"" />
    </MediaFiles>
  </Job>
</JobFile>", outputDirectory, newFilePath);

                    string jobFile = Path.Combine(outputDirectory, Guid.NewGuid().ToString() + ".xml");
                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(jobFile))
                        {
                            streamWriter.Write(jobText);
                        }

                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.Arguments = string.Format("/JobFile \"{0}\"", jobFile);
                        psi.CreateNoWindow = true;
                        psi.FileName = SettingsWrapper.ExpressionMediaEncoderPath;
                        psi.UseShellExecute = false;
                        psi.WindowStyle = ProcessWindowStyle.Hidden;
                        psi.RedirectStandardError = true;

                        Process processor = new Process();
                        processor.StartInfo = psi;

                        processor.Start();

                        // Wait for the thumbnail.
                        for (int lcv = 0; lcv < (SettingsWrapper.MediaThumbnailTimeout * 10); lcv++)
                        {
                            if (processor.HasExited) { break; }
                            Thread.Sleep(100);
                        }

                        if (!processor.HasExited)
                        {
                            processor.Kill();
                            throw new Exception("Encoder didn't finish getting thumbnail in allotted time");
                        }

                        if (processor.ExitCode != 0)
                        {
                            throw new Exception(string.Format("Encoder exited with error code: {0}", processor.ExitCode));
                        }

                        string thumbnailPath = Path.Combine(outputDirectory, string.Format("{0}_Thumb.jpg", id));
                        using (Bitmap bitmap = new Bitmap(thumbnailPath))
                        {
                            thumbnailBits = Utilities.ConvertImageToBytes(bitmap);
                        }
                        File.Delete(thumbnailPath);
                    }
                    catch (Exception e)
                    {
                        HealthMonitoringManager.LogWarning(e, "Couldn't get thumbnail from a video");
                    }
                    finally
                    {
                        if (File.Exists(jobFile))
                        {
                            File.Delete(jobFile);
                        }

                        string outputVideoPath = Path.Combine(outputDirectory, string.Format("{0}.wmv", id));
                        if (File.Exists(outputVideoPath))
                        {
                            try
                            {
                                File.Delete(outputVideoPath);
                            }
                            catch { }
                        }
                    }
                }

                WLQuickApps.SocialNetwork.VideoProcessorService.SilverlightStreamingSubmission submission = new WLQuickApps.SocialNetwork.VideoProcessorService.SilverlightStreamingSubmission(id, newFilePath, serviceMediaType,
                    SettingsWrapper.SilverlightStreamingUserName, SettingsWrapper.SilverlightStreamingPassword);
                System.Messaging.Message message = new System.Messaging.Message(submission, new BinaryMessageFormatter());
                queue.Send(message);
            }
        }

        static public void UploadPackageToSilverlightStreaming(string name, byte[] bits)
        {
            // This is a small hack to prevent unit tests from failing.
            if (System.Web.HttpContext.Current == null) { return; }

            string accountId = SettingsWrapper.SilverlightStreamingUserName;
            string accountKey = SettingsWrapper.SilverlightStreamingPassword;

            string apiUri = "https://silverlight.services.live.com/" + accountId + "/" + name;

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(apiUri);
            req.Credentials = new NetworkCredential(accountId, accountKey);

            req.Method = "POST";
            req.ContentType = "application/zip";

            // without updating, the following timeout properties will default to
            // 300 seconds and 100 seconds respectively (not enough for large file
            // uploads).
            int timeoutInMinutes = 1 * 1000 * 60;
            req.Timeout = timeoutInMinutes;
            req.ReadWriteTimeout = timeoutInMinutes;

            req.ContentLength = bits.Length;

            Stream reqStr = req.GetRequestStream();
            reqStr.Write(bits, 0, bits.Length);

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        }

        /// <summary>
        /// Take in a Messenger Presence ID (CID) and add the apps.messenger.live.com or 
        /// accept an email address.
        /// </summary>
        /// <param name="PresenceID"></param>
        /// <returns></returns>
        static public string CleanMessengerPresenceID(string PresenceID)
        {
            if (PresenceID.Contains("@"))
            {
                return(PresenceID.Trim());
            }
            else
            {
                return((PresenceID + "@apps.messenger.live.com").Trim());
            }
        }

        /// <summary>
        /// This is used to execute methods asynchronously in a fire and forget manner
        /// </summary>
        public class ThreadUtil
        {
            private static AsyncCallback callback = new AsyncCallback(ThreadUtil.EndWrapperInvoke);

            private static DelegateWrapper wrapperInstance = new DelegateWrapper(ThreadUtil.InvokeWrappedDelegate);

            private static void EndWrapperInvoke(IAsyncResult ar)
            {
                try
                {
                    wrapperInstance.EndInvoke(ar);
                    ar.AsyncWaitHandle.Close();
                }
                catch
                {
                    // sink
                }

            }

            public static void FireAndForget(Delegate d, params object[] args)
            {
                wrapperInstance.BeginInvoke(d, args, callback, null);
            }

            private static void InvokeWrappedDelegate(Delegate d, object[] args)
            {
                try
                {
                    d.DynamicInvoke(args);
                }
                catch { }
            }

            private delegate void DelegateWrapper(Delegate d, object[] args);

            public delegate void AsyncNoParamsDelegate();
        }
    }
}
