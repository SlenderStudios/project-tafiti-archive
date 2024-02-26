using System;
using System.Web;
using System.Net;
using System.IO;
using System.Collections;
using Microsoft.LiveFX.Client;
using Microsoft.LiveFX.ResourceModel;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub
{
	public partial class Register : System.Web.UI.Page
	{
		// Initialize the WindowsLiveLogin module.
		static WindowsLiveLogin wll = new WindowsLiveLogin(true);

        private LiveOperatingEnvironment loe = new LiveOperatingEnvironment();

		protected void Page_Load(object sender, EventArgs e)
		{
            HttpRequest req = HttpContext.Current.Request;
            string offer = "";
            if (req.Cookies[Constants.OfferCookie] != null) offer = req.Cookies[Constants.OfferCookie].Value;
            
            WebProfile userProfile = WebProfile.Current;
            if (IsPostBack) return;
                
            DisplayNameTextBox.Text = Server.HtmlEncode(userProfile.DisplayName);

            if (req.QueryString["LiveProfile"] != null && req.QueryString["LiveProfile"] == "1") 
            {
                LiveItemAccessOptions liao = new LiveItemAccessOptions(true);
                loe.Connect(WebProfile.Current.ProfilesDelToken, AuthenticationTokenType.DelegatedAuthToken, new Uri(Constants.ServiceEndPoint), liao);
                foreach (Profile profile in loe.Profiles.Entries)
                {
                    switch (profile.Resource.Type)
                    {
                        case ProfileResource.ProfileType.AboutYou:
                            AboutYouProfile ap = (AboutYouProfile)profile.Resource.ProfileInfo;
                            break;

                        case ProfileResource.ProfileType.ContactInfo:
                            ContactProfile cp = (ContactProfile)profile.Resource.ProfileInfo;
                            EmailTextBox.Text = cp.PersonalEmail;
                            AddressTextBox.Text = cp.HomeAddress;
                            CityTextBox.Text = cp.City;
                            StateTextBox.Text = cp.State;
                            PostalCodeTextBox.Text = cp.PostalCode;
                            break;

                        case ProfileResource.ProfileType.General:
                            GeneralProfile gp = (GeneralProfile)profile.Resource.ProfileInfo;
                            FirstNameTextBox.Text = gp.LastName;
                            LastNameTextBox.Text = gp.FirstName;
                            break;

                        case ProfileResource.ProfileType.Interests:
                            InterestsProfile ip = (InterestsProfile)profile.Resource.ProfileInfo;
                            break;

                        case ProfileResource.ProfileType.WorkInfo:
                            WorkProfile wp = (WorkProfile)profile.Resource.ProfileInfo;
                            if (EmailTextBox.Text.Length == 0) EmailTextBox.Text = wp.WorkEmail;
                            break;
                    }
                }
            }
            else
            {
                //Fix Bug: 170716
                FirstNameTextBox.Text = Server.HtmlEncode(userProfile.FirstName);
                LastNameTextBox.Text = Server.HtmlEncode(userProfile.LastName);
                LiveMessengerIDLabel.Text = Server.HtmlEncode(userProfile.LiveMessengerID);
                EmailTextBox.Text = Server.HtmlEncode(userProfile.Email);
                AddressTextBox.Text = Server.HtmlEncode(userProfile.Address);
                CityTextBox.Text = Server.HtmlEncode(userProfile.City);
                StateTextBox.Text = Server.HtmlEncode(userProfile.State);
                PostalCodeTextBox.Text = Server.HtmlEncode(userProfile.PostalCode);
            }
		}

		protected void SaveButton_Click(object sender, EventArgs e)
		{
            WebProfile userProfile = WebProfile.Current;

			userProfile.DisplayName = DisplayNameTextBox.Text;
			userProfile.FirstName = FirstNameTextBox.Text;
			userProfile.LastName = LastNameTextBox.Text;
			userProfile.Email = EmailTextBox.Text;
            userProfile.Address = AddressTextBox.Text;
            userProfile.City = CityTextBox.Text;
            userProfile.State = StateTextBox.Text;
            userProfile.PostalCode = PostalCodeTextBox.Text;

			userProfile.Save();
            Response.Redirect(Constants.LoginPage);
		}

		protected void CancelButton_Click(object sender, EventArgs e)
		{
            Response.Redirect(Constants.LoginPage);
		}

        protected void InviteButton_Click(object sender, EventArgs e)
        {
            
            AuthenticationHelperClass authHelper = new AuthenticationHelperClass();
            WindowsLiveLogin.ConsentToken consentToken = authHelper.getNewTokenwithRefreshToken(Constants.InviteOffers, WebProfile.Current.ContactsRefresh, WebProfile.Current.ContactsDelToken);
            if (consentToken != null)
            {
                WindowsLiveLogin.ConsentToken consent = wll.ProcessConsentToken(consentToken.Token);
                WebProfile.Current.ContactsDelToken = consent.DelegationToken;
                WebProfile.Current.Save();
                Response.Redirect(Constants.InvitePage);
            }
            else
            {
                string consentUrl = wll.GetConsentUrl(Constants.InviteOffers);
                Response.Redirect(consentUrl);
            }
        }

        protected void ProfileButton_Click(object sender, EventArgs e)
        {
            
            AuthenticationHelperClass authHelper = new AuthenticationHelperClass();
            WindowsLiveLogin.ConsentToken consentToken = authHelper.getNewTokenwithRefreshToken(Constants.ProfileOffers, WebProfile.Current.ProfilesRefresh, WebProfile.Current.ProfilesDelToken);
            if (consentToken != null)
            {
                WindowsLiveLogin.ConsentToken consent = wll.ProcessConsentToken(consentToken.Token);
                WebProfile.Current.ProfilesDelToken = consent.DelegationToken;
                WebProfile.Current.Save();
                Response.Redirect(Constants.ProfilePage + Constants.LiveProfileQuery);
            }
            else
            {
                string consentUrl = wll.GetConsentUrl(Constants.ProfileOffers);
                Response.Redirect(consentUrl);
            }
        }
    }
}
