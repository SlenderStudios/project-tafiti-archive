using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace VESilverlight
{
    public class Utilities
    {
        static public string AppUrlRoot
        {
            get
            {
                if (Utilities._appUrlRoot == null)
                {
                    Utilities._appUrlRoot = string.Format("{0}://{1}:{2}",
                        HtmlPage.Document.DocumentUri.Scheme,
                        HtmlPage.Document.DocumentUri.Host,
                        HtmlPage.Document.DocumentUri.Port);
                }

                return Utilities._appUrlRoot;
            }
        }
        static private string _appUrlRoot;

        static public string PageUrlRoot
        {
            get
            {
                if (Utilities._pageUrlRoot == null)
                {
                    int lastSlash = HtmlPage.Document.DocumentUri.AbsolutePath.LastIndexOf('/');
                    Utilities._pageUrlRoot = string.Format("{0}{1}", Utilities.AppUrlRoot, HtmlPage.Document.DocumentUri.AbsolutePath.Substring(0, lastSlash + 1));
                }

                return Utilities._pageUrlRoot;
            }
        }
        static private string _pageUrlRoot;

        static public string GetAbsolutePath(string pathPart)
        {
            if (string.IsNullOrEmpty(pathPart))
            {
                throw new ArgumentException("pathPart cannot be null or empty");
            }

            if (pathPart.Contains("://"))
            {
                return pathPart;
            }

            if (pathPart[0] == '/')
            {
                return Utilities.AppUrlRoot + pathPart;
            }

            return Utilities.PageUrlRoot + pathPart;
        }

    }
}
