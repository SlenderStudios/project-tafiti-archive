using System;
using System.Collections;
using System.Web;
using WLQuickApps.ContosoBicycleClub.Data;
using WLQuickApps.ContosoBicycleClub.Business;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub.Rides
{
	public partial class RideInfo : System.Web.UI.Page
	{
        [System.Web.Services.WebMethod]
		[System.Web.Script.Services.ScriptMethod]
		public static AjaxControlToolkit.Slide[] GetSlides(string contextKey)
		{
            // Parse the photo url and Ride Id from the context;
            string[] tokens = contextKey.Split(';');
            ArrayList slides = new ArrayList(SlideShow.GetSlides(tokens[0]));
            
            if (tokens.Length > 2)
            {
                ImageManager mgr = new ImageManager();
                foreach (Guid imageID in mgr.GetRideImages(new Guid(tokens[1])))
                {
                    string pic_url = tokens[2] + @"/ImageHandler.ashx?ImageID=" + imageID.ToString();
                    slides.Add(new AjaxControlToolkit.Slide(pic_url, "", ""));
                }
            }
            return (AjaxControlToolkit.Slide[])(slides.ToArray(typeof(AjaxControlToolkit.Slide)));
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			Ride currentRide;

            // Only allow signed-in users to upload pictures.
            if (WebProfile.Current.IsAnonymous)
            {
                UploadButton.Enabled = false;
                UploadButton.Text = "Sign-in to " + Resources.ContosoBicycleClubWeb.UploadPhotoLabel;
            }
            else
            {
                UploadButton.Enabled = true;
                UploadButton.Text = Resources.ContosoBicycleClubWeb.UploadPhotoLabel;
            }

            if (string.IsNullOrEmpty(Request.QueryString["RideId"]))
			{
				Server.Transfer("~/rides/default.aspx");
			}
			else
			{
				Guid id = new Guid(Request.QueryString["RideId"]);
				RideManager mgr = new RideManager();
				currentRide = mgr.GetRide(id);

				EventMultimedia.CurrentRide = currentRide;
				InfoHeader.CurrentRide = currentRide;
				BlogPost.Title = Resources.ContosoBicycleClubWeb.RideDescriptionLabel;
				BlogPost.BlogId = currentRide.BlogPostId;
				CommentList.BlogId = currentRide.BlogPostId;
			}
		}

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            HttpCookie returnCookie = new HttpCookie(Constants.ReturnCookie);
            returnCookie.Value = Constants.RidesReturnURL; ;
            Response.Cookies.Add(returnCookie);
            
            AuthenticationHelperClass authHelper = new AuthenticationHelperClass();
            WindowsLiveLogin.ConsentToken consentToken = authHelper.getNewTokenwithRefreshToken(Constants.MeshObjOffers, WebProfile.Current.PicturesRefresh, WebProfile.Current.PicturesDelToken);
            if (consentToken != null)
            {
                WindowsLiveLogin.ConsentToken consent = Constants.wll.ProcessConsentToken(consentToken.Token);
                WebProfile.Current.ContactsDelToken = consent.DelegationToken;
                WebProfile.Current.Save();
                Response.Redirect(Constants.UploadPage + "?RideID=" + Request.QueryString["RideId"]);
            }
            else
            {
                string consentUrl = Constants.wll.GetConsentUrl(Constants.MeshObjOffers) + "&appctx=" + Request.QueryString["RideId"];
                Response.Redirect(consentUrl);
            }
        }
	}
}
