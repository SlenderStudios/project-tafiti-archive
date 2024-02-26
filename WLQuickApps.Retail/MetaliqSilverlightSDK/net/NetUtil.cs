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

namespace MetaliqSilverlightSDK.net
{
    public class NetUtil
    {
        public static Uri ToAbsoluteUri(string relativeUri)
        {
            string uri = relativeUri;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (relativeUri.StartsWith("http") == false && relativeUri.StartsWith("mailto") == false)
            {
                if (relativeUri.StartsWith("/"))
                {
                    relativeUri = relativeUri.Substring(1);
                }
                sb.Append("http://");
                sb.Append(HtmlPage.Document.DocumentUri.Host);
                sb.Append(":");
                sb.Append(HtmlPage.Document.DocumentUri.Port);
                sb.Append(HtmlPage.Document.DocumentUri.LocalPath.Substring(0, HtmlPage.Document.DocumentUri.LocalPath.LastIndexOf('/') + 1));
                sb.Append(relativeUri);
                uri = sb.ToString();
            }

            return new Uri(uri, UriKind.Absolute);
        }
    }
}
