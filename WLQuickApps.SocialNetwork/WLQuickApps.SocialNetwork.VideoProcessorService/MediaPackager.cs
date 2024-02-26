using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using java.util.zip;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    // Give the location to a Silverlight Streaming application, this class
    // will produce the Zip package with necessary components that is expected
    // by the SS Api.
    public class MediaPackager
    {
        public static void PackageFiles(string[] filenames, string path, string outputFile)
        {
            if (filenames == null)
            { throw new ArgumentNullException("filenames"); }
            foreach (string str in filenames)
            {
                if (str == null)
                { throw new ArgumentNullException("filenames"); }
            }
            if (outputFile == null)
            { throw new ArgumentNullException("outputFile"); }


            string[] filesWithPaths = new string[filenames.Length];

            for (int i = 0; i < filenames.Length; i++)
            {
                filesWithPaths[i] = Path.Combine(path, filenames[i]);
            }

            ZipUtils.CreateEmptyZipFile(outputFile);
            ZipUtils.UpdateZipFile(new ZipFile(outputFile), null, filesWithPaths);
        }
    }
}
