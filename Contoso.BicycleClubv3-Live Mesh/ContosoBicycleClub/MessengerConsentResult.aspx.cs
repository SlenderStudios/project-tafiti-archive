using System;
using System.Collections;
using System.Collections.Specialized;
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
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub
{
	public partial class MessengerConsentResult : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string result = string.Empty;
			string userId = string.Empty;

			if (!IsPostBack)
			{
				// Go through and extract the returned QueryString values.
				NameValueCollection returnParams = Request.QueryString;

				for (int i = 0; i < returnParams.AllKeys.Length; i++)
				{
					String nextKey = returnParams.AllKeys[i];
					if (nextKey == "result")
						result = returnParams[i];
					else if (nextKey == "id")
						userId = returnParams[i];
				}
				// If the result is success, save values to session.
				if ((result == "Accepted") && (userId != null))
				{
					WebProfile.Current.LiveMessengerID = userId;
					WebProfile.Current.Save();
					ReturnMessageLabel.Text = "Your Windows Live Messenger online presence will be shared with other Contoso riders.";
				}
				// If the result does not succeed, display an error.
				else if (result != "Accepted")
				{
					if (result == "Declined")
					{
						WebProfile.Current.LiveMessengerID = null;
						WebProfile.Current.Save();
						ReturnMessageLabel.Text = "Contoso riders will NOT be able to see your Windows Live Messenger online presence.";
					}
					else if (result == "NoPrivacyUrl")
					{
						ReturnMessageLabel.Text = "[" + result + "]" + " No privacy URL was supplied.";
					}
				}
			}
		}
	}
}
