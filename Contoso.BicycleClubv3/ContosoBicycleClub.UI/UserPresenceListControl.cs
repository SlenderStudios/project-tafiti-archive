using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Profile;

namespace WLQuickApps.ContosoBicycleClub.UI
{
	[ToolboxData("<{0}:UserPresenceListControl runat=server></{0}:UserPresenceListControl>")]
	public class UserPresenceListControl : Repeater
	{
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (!Page.IsPostBack)
			{
				List<UserPresenceInfo> list = new List<UserPresenceInfo>();

				ProfileInfoCollection pc = ProfileManager.GetAllProfiles(ProfileAuthenticationOption.All);
				foreach (ProfileInfo info in pc)
				{
					UserPresenceInfo usrPresenceInfo = new UserPresenceInfo();
					
					WebProfile profile = new WebProfile(ProfileBase.Create(info.UserName));
					
					if (!string.IsNullOrEmpty(profile.LiveMessengerID) 
						&& WebProfile.Current.LiveMessengerID != profile.LiveMessengerID 
						&& !string.IsNullOrEmpty(profile.DisplayName))
					{
						usrPresenceInfo.LiveMessengerID = profile.LiveMessengerID;
						usrPresenceInfo.DisplayName = HttpUtility.HtmlEncode(profile.DisplayName);
						usrPresenceInfo.Name = HttpUtility.HtmlEncode(profile.FirstName + " " + profile.LastName);
						usrPresenceInfo.Email = HttpUtility.HtmlEncode(profile.Email);

						list.Add(usrPresenceInfo);
					}
				}

				this.DataSource = list;
				this.DataBind();
			}
		}
	}

	public class UserPresenceInfo
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public string LiveMessengerID { get; set; }
		public string Email { get; set; }
	}
}
