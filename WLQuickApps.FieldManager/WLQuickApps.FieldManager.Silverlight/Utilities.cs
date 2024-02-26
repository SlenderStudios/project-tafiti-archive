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

namespace WLQuickApps.FieldManager.Silverlight
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

    }
}
