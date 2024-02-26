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
	public partial class EventInfo : System.Web.UI.Page
	{
		[System.Web.Services.WebMethod]
		[System.Web.Script.Services.ScriptMethod]
		public static AjaxControlToolkit.Slide[] GetSlides(string contextKey)
		{
			return SlideShow.GetSlides(contextKey);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			Ride currentRide;

			if (string.IsNullOrEmpty(Request.QueryString["EventId"]))
			{
				Server.Transfer("~/events/default.aspx");
			}
			else
			{
				Guid id = new Guid(Request.QueryString["EventId"]);
				RideManager mgr = new RideManager();
				currentRide = mgr.GetRide(id);

				EventMultimedia.CurrentRide = currentRide;
				InfoHeader.CurrentRide = currentRide;
				BlogPost.Title = Resources.ContosoBicycleClubWeb.EventDescriptionLabel;
				BlogPost.BlogId = currentRide.BlogPostId;
				CommentList.BlogId = currentRide.BlogPostId;
			}
		}
	}
}
