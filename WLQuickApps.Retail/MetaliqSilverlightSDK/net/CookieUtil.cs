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
    public class CookieUtil
    {
        /// <summary>
        /// sets a persistent cookie with huge expiration date
        /// </summary>
        /// <param name="key">the cookie key</param>
        /// <param name="value">the cookie value</param>
        public static void SetCookie(string key, string value)
        {
            DateTime expiration = DateTime.UtcNow + TimeSpan.FromDays(2000);
            SetCookie(key, value, expiration);
        }
        /// <summary>
        /// sets a persistent cookie with specified expiration date
        /// </summary>
        /// <param name="key">the cookie key</param>
        /// <param name="value">the cookie value</param>
        /// <param name="expiration">the expiration time of the cookie</param>
        public static void SetCookie(string key, string value, DateTime expiration)
        {
            string oldCookie = HtmlPage.Document.GetProperty("cookie") as String;
            string cookie = String.Format("{0}={1};expires={2}", key, value, expiration.ToString("R"));
            HtmlPage.Document.SetProperty("cookie", cookie);
        }

        /// <summary>
        /// Retrieves an existing cookie
        /// </summary>
        /// <param name="key">cookie key</param>
        /// <returns>null if the cookie does not exist, otherwise the cookie value</returns>
        public static string GetCookie(string key)
        {
            string[] cookies = HtmlPage.Document.Cookies.Split(';');
            key += '=';
            foreach (string cookie in cookies)
            {
                string cookieStr = cookie.Trim();
                if (cookieStr.StartsWith(key, StringComparison.OrdinalIgnoreCase))
                {
                    string[] vals = cookieStr.Split('=');

                    if (vals.Length >= 2)
                    {
                        return vals[1];
                    }

                    return string.Empty;
                }
            }

            return null;
        }

        /// <summary>
        /// Deletes a specified cookie by setting its value to empty and expiration to -1 days
        /// </summary>
        /// <param name="key">the cookie key to delete</param>
        public static void DeleteCookie(string key)
        {
            string oldCookie = HtmlPage.Document.GetProperty("cookie") as String;
            DateTime expiration = DateTime.UtcNow - TimeSpan.FromDays(1);
            string cookie = String.Format("{0}=;expires={1}", key, expiration.ToString("R"));
            HtmlPage.Document.SetProperty("cookie", cookie);
        }
    }
}
