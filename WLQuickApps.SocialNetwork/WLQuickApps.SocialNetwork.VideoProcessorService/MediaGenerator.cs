using System;
using System.Collections.Generic;
using System.Text;
using System.Messaging;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    public class MediaGenerator
    {
        static string _queuePath;

        
        public static SilverlightStreamingSubmission GetNextSubmission()
        {
            if (_queuePath == null)
            {
                _queuePath = MediaProcessorSettingsWrapper.MediaSubmissionQueue;
            }

            // Grab the queue.
            using (MessageQueue queue = new MessageQueue(_queuePath))
            {
                // Set its formatter to binary.
                queue.Formatter = new BinaryMessageFormatter();

                try
                {
                    // Take the next item from the queue.
                    using (System.Messaging.Message message = queue.Receive(new TimeSpan(0)))
                    {
                        SilverlightStreamingSubmission submission = (SilverlightStreamingSubmission)message.Body;
                        if (submission == null)
                        {
                            string msg = String.Format("Message received from queue {0} was not of the expected type.",
                                _queuePath);

                            throw new MediaProcessingException(msg);
                        }

                        return submission;
                    }
                }
                catch (MessageQueueException ex)
                {
                    // Check to see this this was just a timeout.
                    if (ex.MessageQueueErrorCode == MessageQueueErrorCode.IOTimeout)
                    {
                        return null;
                    }

                    // If not a timeout, something went wrong.
                    string msg = String.Format("Failed to receive next submission from queue {0}.", 
                        _queuePath);

                    throw new MediaProcessingException(msg, ex);
                }
            }
        }
    }
}
