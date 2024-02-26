using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    public class StreamUtils
    {
        public static void StreamCopy(Stream source, Stream destination)
        {
            if (source == null || destination == null)
            { throw new ArgumentNullException((source == null) ? "source" : "destination"); }

            byte[] buff = new byte[source.Length];

            ReadAllBytesFromStream(source, buff);

            WriteAllBytesToStream(destination, buff);
        }

        public static int ReadAllBytesFromStream(Stream stream, byte[] buffer)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }
            if (buffer == null)
            { throw new ArgumentNullException("buffer"); }

            const int chunkSize = 1024 * 64;

            int offset = 0;
            while (true)
            {
                int remainingBytes = (int)stream.Length - offset;
                int bytesToCopy = Math.Min(remainingBytes, chunkSize);

                int bytesRead = stream.Read(buffer, offset, bytesToCopy);
                if (bytesRead == 0)
                {
                    break;
                }
                offset += bytesRead;
            }
            return offset;
        }

        public static void WriteAllBytesToStream(Stream stream, byte[] buffer)
        {
            if (stream == null)
            { throw new ArgumentNullException("stream"); }
            if (buffer == null)
            { throw new ArgumentNullException("buffer"); }

            const int chunkSize = 1024 * 64;
            int offset = 0;

            while (offset < buffer.Length)
            {
                int remainingBytes = buffer.Length - offset;
                int bytesToCopy = Math.Min(remainingBytes, chunkSize);

                stream.Write(buffer, offset, bytesToCopy);

                offset += bytesToCopy;
            }
        }
    }
}
