using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using WLQuickApps.ContosoBicycleClub.Data;
using WLQuickApps.ContosoBicycleClub.Business;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub.Events
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			UpcomingEventsRepeater.ItemTemplate = new DetailEventItemTemplate(true, 40, 90, "EventInfo.aspx?EventId={0}");
			PastEventsRepeater.ItemTemplate = new DetailEventItemTemplate(false, 53, 160, "EventInfo.aspx?EventId={0}");

			if (!Page.IsPostBack)
			{
				RideManager mgr = new RideManager();
				UpcomingEventsRepeater.DataSource = mgr.GetUpcomingEvents();
				UpcomingEventsRepeater.DataBind();
				PastEventsRepeater.DataSource = mgr.GetPastEvents();
				PastEventsRepeater.DataBind();
				EventsFeedLink.NavigateUrl = ConfigurationSettings.AppSettings["EventsFeed"];
			}
		}
	}
}
