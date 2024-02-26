using System;
using System.Collections.Generic;
using System.Text;
using java.util;
using java.util.zip;

// Code from June 2003 MSDN article by Ianier Munoz,
// "Using the Zip Classes in the J# Class Libraries to Compress Files and Data with C#"
// http://msdn.microsoft.com/msdnmag/issues/03/06/ZipCompression/

namespace WLQuickApps.SocialNetwork.VideoProcessorService
{
    public delegate Enumeration EnumerationMethod();

    /// <summary>
    /// Wraps java enumerators 
    /// </summary>
    public class EnumerationAdapter : System.Collections.IEnumerable
    {
        private class EnumerationWrapper : System.Collections.IEnumerator
        {
            private EnumerationMethod m_Method;
            private Enumeration m_Wrapped;
            private object m_Current;

            public EnumerationWrapper(EnumerationMethod method)
            {
                m_Method = method;
            }

            // IEnumerator
            public object Current
            {
                get { return m_Current; }
            }

            public void Reset()
            {
                m_Wrapped = m_Method();
                if (m_Wrapped == null)
                    throw new InvalidOperationException();
            }

            public bool MoveNext()
            {
                if (m_Wrapped == null)
                    Reset();
                bool Result = m_Wrapped.hasMoreElements();
                if (Result)
                    m_Current = m_Wrapped.nextElement();
                return Result;
            }
        }

        private EnumerationMethod m_Method;

        public EnumerationAdapter(EnumerationMethod method)
        {
            if (method == null)
                throw new ArgumentException();
            m_Method = method;
        }

        // IEnumerable
        public System.Collections.IEnumerator GetEnumerator()
        {
            return new EnumerationWrapper(m_Method);
        }
    }

    public delegate bool FilterEntryMethod(ZipEntry e);

    /// <summary>
    /// Zip stream utils
    /// </summary>
    public class ZipUtils
    {
        public static void CopyStream(java.io.InputStream from,
            java.io.OutputStream to)
        {
            sbyte[] buffer = new sbyte[8192];
            int got;
            while ((got = from.read(buffer, 0, buffer.Length)) > 0)
                to.write(buffer, 0, got);
        }

        public static void ExtractZipFile(ZipFile file, string path,
            FilterEntryMethod filter)
        {
            foreach (ZipEntry entry in new EnumerationAdapter(new
                EnumerationMethod(file.entries)))
            {
                if (!entry.isDirectory())
                {
                    if ((filter == null || filter(entry)))
                    {
                        java.io.InputStream s = file.getInputStream(entry);
                        try
                        {
                            string fname =
                              System.IO.Path.GetFileName(entry.getName());
                            string newpath = System.IO.Path.Combine(path,
                              System.IO.Path.GetDirectoryName(entry.getName()));

                            System.IO.Directory.CreateDirectory(newpath);

                            java.io.FileOutputStream dest = new
                              java.io.FileOutputStream(System.IO.Path.Combine(
                              newpath, fname));
                            try
                            {
                                CopyStream(s, dest);
                            }
                            finally
                            {
                                dest.close();
                            }
                        }
                        finally
                        {
                            s.close();
                        }
                    }
                }
            }
        }

        public static ZipFile CreateEmptyZipFile(string fileName)
        {
            new ZipOutputStream(new
              java.io.FileOutputStream(fileName)).close();
            return new ZipFile(fileName);
        }

        public static ZipFile UpdateZipFile(ZipFile file,
            FilterEntryMethod filter, string[] newFiles)
        {
            string prev = file.getName();
            string tmp = System.IO.Path.GetTempFileName();
            ZipOutputStream to = new ZipOutputStream(new
                                 java.io.FileOutputStream(tmp));
            try
            {
                CopyEntries(file, to, filter);
                // add entries here
                if (newFiles != null)
                {
                    foreach (string f in newFiles)
                    {
                        //ZipEntry z = new ZipEntry(f.Remove(0, 
                        //  System.IO.Path.GetPathRoot(f).Length));
                        
                        ZipEntry z = new ZipEntry(System.IO.Path.GetFileName(f));
                        z.setMethod(ZipEntry.DEFLATED);
                        to.putNextEntry(z);
                        try
                        {
                            java.io.FileInputStream s = new
                              java.io.FileInputStream(f);
                            try
                            {
                                CopyStream(s, to);
                            }
                            finally
                            {
                                s.close();
                            }
                        }
                        finally
                        {
                            to.closeEntry();
                        }
                    }
                }
            }
            finally
            {
                to.close();
            }
            file.close();

            // now replace the old file with the new one
            System.IO.File.Copy(tmp, prev, true);
            System.IO.File.Delete(tmp);

            return new ZipFile(prev);
        }

        public static void CopyEntries(ZipFile from, ZipOutputStream to)
        {
            CopyEntries(from, to, null);
        }

        public static void CopyEntries(ZipFile from, ZipOutputStream to,
          FilterEntryMethod filter)
        {
            foreach (ZipEntry entry in new EnumerationAdapter(new
              EnumerationMethod(from.entries)))
            {
                if (filter == null || filter(entry))
                {
                    java.io.InputStream s = from.getInputStream(entry);
                    try
                    {
                        to.putNextEntry(entry);
                        try
                        {
                            CopyStream(s, to);
                        }
                        finally
                        {
                            to.closeEntry();
                        }
                    }
                    finally
                    {
                        s.close();
                    }
                }
            }
        }
    }
}
