﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace WLQuickApps.ContosoBicycleClub.UI
{
	/// <summary>
	/// Summary description for ShareOnlinePresenceLink
	/// </summary>
	[ParseChildren(false)]
	[ControlBuilder(typeof(HyperLinkControlBuilder))]
	[Designer("System.Web.UI.Design.WebControls.HyperLinkDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ToolboxData(@"<{0}:ShareOnlinePresenceLink runat=""server"" />")]
	[DefaultProperty("Text")]
	public class ShareOnlinePresenceLink : HyperLink
	{
		private bool isDebug = true;

		public ShareOnlinePresenceLink()
			: base()
		{
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			WindowsLiveLogin wl = new WindowsLiveLogin(true);

			if (this.Page.User.Identity.IsAuthenticated)
			{
				//only show this link if the user has sign up for this.
				if (string.IsNullOrEmpty(WebProfile.Current.LiveMessengerID) || isDebug)
				{
					base.Text = "Share Your Online Presence";
					string baseUrl = UriHelper.GetServerRoot();

					base.NavigateUrl = wl.GetOnlinePresenceInvitationUrl(
						baseUrl + this.ResolveUrl("MessengerConsentResult.aspx"),
						baseUrl + this.ResolveUrl("Privacy.aspx"));
				}
				else
				{
					this.Visible = false;
				}
			}
			else
			{
				this.Visible = false;
			}
		}
	}
}
