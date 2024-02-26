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
using WLQuickApps.ContosoBicycleClub.Data;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub.UserControls
{
	public partial class EventMultimediaControl : System.Web.UI.UserControl
	{
		public Ride CurrentRide { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			ClientScriptManager cs = Page.ClientScript;
			string globalVariablesScriptName = "GlobalVariables";
			if (!cs.IsStartupScriptRegistered(globalVariablesScriptName))
			{
				string appPath = Request.ApplicationPath;
				StringBuilder scripts = new StringBuilder();
				scripts.AppendFormat("WEBROOT = '{0}';", (appPath == "/" ? "" : appPath));
				scripts.AppendFormat("CID = '{0}';", Server.HtmlEncode(CurrentRide.VECollectionId)); //170712
				scripts.AppendFormat("ALBUM = '{0}';", Server.HtmlEncode(CurrentRide.PhotoAlbumLink)); //170712
				scripts.AppendFormat("VIDEO = '{0}';", Server.HtmlEncode(CurrentRide.VideoLink)); //170712
				cs.RegisterStartupScript(this.GetType(), globalVariablesScriptName, scripts.ToString(), true);
			}

			if (!string.IsNullOrEmpty(CurrentRide.PhotoAlbumLink))
			{
				SlidesExtender.ContextKey = CurrentRide.PhotoAlbumLink;
			}
			else
			{
				SlidesExtender.Enabled = false;
			}
		}
	}
}