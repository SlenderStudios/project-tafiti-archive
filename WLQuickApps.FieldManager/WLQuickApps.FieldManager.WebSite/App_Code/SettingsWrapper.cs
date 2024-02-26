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
    /// Summary description for SettingsWrapper
    /// </summary>
    public class SettingsWrapper
    {
        private SettingsWrapper() { }

        static public bool EnableLiveAlerts { get { return Convert.ToBoolean(ConfigurationManager.AppSettings[Constants.AppSettingsKeys.EnableLiveAlerts]); } }
        static public string LiveAlertsChangeUrl { get { return ConfigurationManager.AppSettings[Constants.AppSettingsKeys.LiveAlertsChangeUrl]; } }
        static public string LiveAuthID { get { return ConfigurationManager.AppSettings[Constants.AppSettingsKeys.LiveAuthID]; } }

    }
}