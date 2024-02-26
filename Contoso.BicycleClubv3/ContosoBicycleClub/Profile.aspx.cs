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
using System.Web.Profile;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub
{
	public partial class Register : System.Web.UI.Page
	{
		// Initialize the WindowsLiveLogin module.
		static WindowsLiveLogin wll = new WindowsLiveLogin(true);

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				//Fix Bug: 170716
				WebProfile userProfile = WebProfile.Current;
				DisplayNameTextBox.Text = Server.HtmlEncode(userProfile.DisplayName);
				FirstNameTextBox.Text = Server.HtmlEncode(userProfile.FirstName);
				LastNameTextBox.Text = Server.HtmlEncode(userProfile.LastName);
				LiveMessengerIDLabel.Text = Server.HtmlEncode(userProfile.LiveMessengerID);
				EmailTextBox.Text = Server.HtmlEncode(userProfile.Email);
			}
		}

		protected void SaveButton_Click(object sender, EventArgs e)
		{
			WebProfile userProfile = WebProfile.Current;

			userProfile.DisplayName = DisplayNameTextBox.Text;
			userProfile.FirstName = FirstNameTextBox.Text;
			userProfile.LastName = LastNameTextBox.Text;
			userProfile.Email = EmailTextBox.Text;

			userProfile.Save();
			Response.Redirect("Default.aspx");
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
			Response.Redirect("Default.aspx");
		}
	}
}
