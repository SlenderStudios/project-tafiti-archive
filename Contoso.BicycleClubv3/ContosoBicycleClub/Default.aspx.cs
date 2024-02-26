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

namespace WLQuickApps.ContosoBicycleClub
{
	public partial class Default1 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			PastRidesRepeater.ItemTemplate = new DetailEventItemTemplate(false, 53, 160, "Rides/RideInfo.aspx?RideId={0}");
			UpcomingRidesRepeater.ItemTemplate = new DetailEventItemTemplate(true, 40, 90, "Rides/RideInfo.aspx?RideId={0}");
			UpcomingEventsRepeater.ItemTemplate = new DetailEventItemTemplate(true, 40, 90, "Events/EventInfo.aspx?EventId={0}");
		}
	}
}
