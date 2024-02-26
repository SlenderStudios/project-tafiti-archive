using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub
{
    public class Constants
    {
        public static WindowsLiveLogin wll = new WindowsLiveLogin(true);
        public static string MeshObjOffers = "LiveMeshFolder.Read";
        public static string ReturnCookie = "ReturnURL";
        public static string RidesReturnURL = "~/Rides/RideInfo.aspx";
        public static string UploadPage = "~/UploadPhotos.aspx";
        public static string AuthCookie = "delauthtoken";
        public static string ProfileCookie = "Profile";
        public static string LoginCookie = "webauthtoken";
        public static string OfferCookie = "Offer";
        public static string ContextCookie = "Context";
        public static string ServiceEndPoint = @"https://user-ctp.windows.net/V0.1";
        public static string ROOT_FOLDER_ID = "urn:uuid:00000000-0000-0000-0000-000000000000";
        public static string FOLDER_TYPE = "Folder";
        public static string FOLDER_OBJ_TYPE = "LiveMeshFolder";
        public static string FILE_OBJ_TYPE = "LiveMeshFiles";
        public static string LoginPage = "default.aspx";
        public static string LogoutPage = LoginPage;
        public static string ProfilePage = "Profile.aspx";
        public static string EventsReturnURL = "~/Events/EventInfo.aspx";
        public static string InvitePage = "InviteContacts.aspx";
        public static string InviteOffers = "Contacts.Read";
        public static string ProfileOffers = "Profiles.Read";
        public static string LiveProfileQuery = "?LiveProfile=1";
    }
}
