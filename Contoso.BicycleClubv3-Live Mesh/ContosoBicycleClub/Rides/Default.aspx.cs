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

namespace WLQuickApps.ContosoBicycleClub.Rides
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			UpcomingRidesRepeater.ItemTemplate = new DetailEventItemTemplate(true, 40, 90, "RideInfo.aspx?RideId={0}");
			PastRidesRepeater.ItemTemplate = new DetailEventItemTemplate(false, 53, 160, "RideInfo.aspx?RideId={0}");

			if (!Page.IsPostBack)
			{
				RideManager mgr = new RideManager();
				UpcomingRidesRepeater.DataSource = mgr.GetUpcomingRides();
				UpcomingRidesRepeater.DataBind();
			
				PastRidesRepeater.DataSource = mgr.GetPastRides();
				PastRidesRepeater.DataBind();
				RidesFeedLink.NavigateUrl = ConfigurationSettings.AppSettings["RidesFeed"];
			}
		}
	}
}
