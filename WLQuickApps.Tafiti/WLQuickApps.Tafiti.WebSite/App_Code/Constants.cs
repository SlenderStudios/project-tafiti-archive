using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WLQuickApps.Tafiti.WebSite
{

    /// <summary>
    /// Summary description for Constants
    /// </summary>
    sealed public class Constants
    {
        private Constants() { }

        sealed public class AppSettingsKeys
        {
            static public string SendEmail = "SendEmail";
            static public string StorageProvider = "StorageProvider";
            static public string HostSite = "HostSite";
            static public string SiteEmail = "SiteEmail";
            static public string LiveSearchAppID = "LiveSearchAppID";
            static public string LiveAnalyticsID = "LiveAnalyticsID";
            static public string LiveAuthID = "wll_appid";
        }

        sealed public class Storage
        {
            // TODO: Check to see if these can be the same thing.
            static public string LiveFileName = "shelf";
            static public string FileSystemFileName = "shelf.txt";

            static public string FileSystemSnapshotFolderName = "snapshot";
        }

        sealed public class HttpHeaderKeys
        {
            static public string Puid = "Puid";

        }

        sealed public class QueryKeys
        {
            static public string SnapshotID = "sid";

        }

    }
}