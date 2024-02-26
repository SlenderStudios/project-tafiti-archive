using System;
using System.Collections.Specialized;
using System.Web.Configuration;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Get App settings
            NameValueCollection appSettings = WebConfigurationManager.AppSettings;

            // Set Calendar Web and RSS links
            string calendar = appSettings["calendar"];
            if (!string.IsNullOrEmpty(calendar) || !Utilities.IsValidUrl(calendar))
            {
                string calendarPath = calendar.Substring(0, calendar.LastIndexOf('/'));
                CalendarWeb.NavigateUrl = calendarPath + "/index.html";
                CalendarRSS.NavigateUrl = calendarPath + "/calendar.xml";
            }
            else
            {
                CalendarWeb.NavigateUrl = "";
                CalendarRSS.NavigateUrl = "";
            }

        }
    }
}
