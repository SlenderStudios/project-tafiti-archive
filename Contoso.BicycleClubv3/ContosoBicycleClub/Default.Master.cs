using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Profile;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub
{
	public partial class Default : System.Web.UI.MasterPage
	{
		protected override void OnPreRender(EventArgs e)
		{
			// Get the ScriptManager
			if (ScriptManager.GetCurrent(this.Page) == null)
				throw (new Exception("ASP.NET AJAX Required. Please add ScriptManager to the page."));
		}

		//Fix Bug : 170715
		protected string CurrentUserDisplayName
		{
			get 
			{
				return Server.HtmlEncode(WebProfile.Current.DisplayName);
			}
		}
	
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.Title = RenderPageTitle();
			SelectNavItem();
			WebProfile userProfile = new WebProfile();
		}

		private void SelectNavItem()
		{
			string HomeUrl = SiteMap.RootNode.Url.ToLower();
			string RidesUrl = Request.ApplicationPath.ToLower() + "rides/default.aspx";
			string EventsUrl = Request.ApplicationPath.ToLower() + "events/default.aspx";
			string currentNode = SiteMap.CurrentNode.Url.ToLower();
			string parentNode = (SiteMap.CurrentNode.Equals(SiteMap.RootNode) ? "" : SiteMap.CurrentNode.ParentNode.Url.ToLower());

			if (currentNode == HomeUrl)
			{
				HomeLink.CssClass += " selected";
			}
            else if (currentNode == RidesUrl || parentNode == RidesUrl || Request.QueryString.ToString() == "type=Ride")
			{
				RidesLink.CssClass += " selected";
			}
            else if (currentNode == EventsUrl || parentNode == EventsUrl || Request.QueryString.ToString() == "type=Event")
			{
				EventsLink.CssClass += " selected";
			}
		}

		private string RenderPageTitle()
		{
			if (SiteMap.CurrentNode != null)
			{
				string pageTitle = SiteMap.CurrentNode.Title + Resources.ContosoBicycleClubWeb.WebsiteLabelSeparator + Resources.ContosoBicycleClubWeb.WebsiteLabel;
				return pageTitle;
			}

			return Resources.ContosoBicycleClubWeb.WebsiteLabel;
		}

	}
}
