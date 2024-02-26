using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WLQuickApps.ContosoBicycleClub.Data;
using WLQuickApps.ContosoBicycleClub.Business;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub.UserControls
{
	public partial class EventInfoHeader : System.Web.UI.UserControl
	{
		public Ride CurrentRide { get; set; }
		public string EventType { get; set; }

		public string Title
		{
			get 
			{
				return Server.HtmlEncode(CurrentRide.Title); //Fix Bug: 170711
			}
		}

		public DateTime? EventDate
		{
			get { return CurrentRide.EventDate; }
		}

		public string OwnerName
		{
			get 
			{
				return Server.HtmlEncode(CurrentRide.OwnerName); //Fix Bug: 170711 
			}
		}

		public string OwnerId
		{
			get
			{
//				WebProfile ownerProfile = WebProfile.Create( WebProfile.Create(CurrentRide.OwnerId);
				//return (ownerProfile.LiveMessengerID);
				WebProfile ownerProfile = new WebProfile(ProfileBase.Create(CurrentRide.OwnerId));
				return (ownerProfile.LiveMessengerID);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (EventType == "event")
			{
				BackHyperLink.NavigateUrl = "~/events/default.aspx";
				BackHyperLink.Text = Resources.ContosoBicycleClubWeb.ReturnToEventsLabel;
			}
			else
			{
				BackHyperLink.NavigateUrl = "~/rides/default.aspx";
				BackHyperLink.Text = Resources.ContosoBicycleClubWeb.ReturnToRidesLabel;
			}			
		}
	}
}