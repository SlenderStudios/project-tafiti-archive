using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace WLQuickApps.FieldManager.WebSite
{
    /// <summary>
    /// Summary description for Constants
    /// </summary>
    public class Constants
    {
        private Constants() { }

        sealed public class AppSettingsKeys
        {
            public const string EnableLiveAlerts = "EnableLiveAlerts";
            public const string LiveAlertsChangeUrl = "LiveAlertsChangeUrl";
            public const string LiveAuthID = "wll_appid";
        }
    }
}