using System;
using System.DHTML;
using ScriptFX;
using ScriptFX.UI;

namespace WLQuickApps.Tafiti.Scripting
{
    /// <summary>
    ///     Utilities class for various HTML processes
    /// </summary>
    public class Utilities
    {
        public static DOMElement CreateSpan(string innerText, string cssClass)
        {
            return Utilities.CreateNode("span", innerText, cssClass);
        }

        public static DOMElement CreateListItem(string innerText)
        {
            return Utilities.CreateNode("li", innerText, string.Empty);
        }

        public static DOMElement CreateTertiaryHeader(string innerText)
        {
            return Utilities.CreateNode("h3", innerText, string.Empty);
        }

        public static DOMElement CreateParagraph(string innerText)
        {
            return Utilities.CreateNode("p", innerText, string.Empty);
        }

        public static DOMElement CreateNode(string elementType, string innerText, string cssClass)
        {
            DOMElement node = Document.CreateElement(elementType);
            node.InnerText = innerText;
            node.ClassName = cssClass;
            return node;
        }

        static public string GetSiteUrlRoot()
        {
            string url = Window.Location.Protocol + "//" + Window.Location.Hostname;

            // No other way to check if port is null or empty since it's a javascript var.
            if (Window.Location.Port.ToString() != string.Empty)
            {
                url += ":" + Window.Location.Port;
            }

            return url;
        }

        public static bool UserIsLoggedIn()
        {
            return (Document.GetElementById(Constants.SignInFrameID) != null);
        }

        public static string Hash(string clearText)
        {
            return CryptographyManager.GetMD5Hash(clearText.ToLowerCase());
        }
    }
}
