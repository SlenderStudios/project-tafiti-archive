using System;
using System.Collections.Generic;
using System.Text;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    static class Constants
    {
        public const string EMESourceArgVariable = "@source";

        public const string EMETargetArgVariable = "@target";

        public const string EMEPresetArgVariable = "@preset";

        //public const string EMECommandLineTemplate = @"/source @source /target @target /preset @preset";
        public const string EMECommandLineTemplate = 
            @"/source " + 
            EMESourceArgVariable + " /target " + 
            EMETargetArgVariable + " /preset " + 
            EMEPresetArgVariable;

        public const string EMEPresetFileVideo = "presets.xml";

        public const string EMEPresetFileAudio = "presetsAudio.xml";

        public const string EMETargetFileAppend = "_out";

        public const string EMETargetVideoExtension = ".wmv";

        public const string EMETargetAudioExtension = ".wma";


        public const string SSManifestFile = "manifest.xml";

        public const string SSPackageFile = "out.zip";

        public const string SSErrorPackageFile = "error.zip";


        // The default HttpWebRequest needs to have the default timeout
        // properties changed to allow the uploading of larger Zip files
        // to the Silverlight Streaming service. 
        // Here we set it to 10 MINUTES.
        public const int MediaUploadTimeout = 10; //in minutes

        // How often to we check for new submissions.
        // Here we set it to 1000 ms.
        public const int SubmissionQueueCheckInterval = 1000; //in milliseconds

        public const int PauseAfterError = 10000; // in milliseconds
    }
}
