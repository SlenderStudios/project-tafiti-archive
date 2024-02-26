using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.Live.ServerControls;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class UserLogic
    {
        // Find if a user exits by their live id
        public static bool UserIDExists(string userid)
        {
            ContosoBankDataContext db = new ContosoBankDataContext();
            var temp = from user in db.UserProfiles
                       where user.LiveID == userid
                       select user;
            return temp.FirstOrDefault() != null;
        }

        // get a user by their live id
        public static UserProfile GetUserByID(string userid)
        {
            ContosoBankDataContext db = new ContosoBankDataContext();
            var temp = from user in db.UserProfiles
                       where user.LiveID == userid
                       select user;
            return temp.FirstOrDefault();
        }

        // get the currently logged in user
        public static UserProfile GetCurrentUser()
        {
            return HttpContext.Current.Session["ApplicationUserID"] != null ? GetUserByID(HttpContext.Current.Session["ApplicationUserID"].ToString()) : null;
        }

        // get users within a viewing area
        public static List<ClusteredPin> GetUserLocationsByBounds(Bounds bounds)
        {
            ContosoBankDataContext db = new ContosoBankDataContext();
            var temp = from user in db.UserProfiles
                       where
                           user.Latitude > bounds.SE.Lat && user.Latitude < bounds.NW.Lat &&
                           user.Longitude > bounds.NW.Lon && user.Longitude < bounds.SE.Lon
                       select
                           new ClusteredPin
                               {Loc = new LatLong {Lat = user.Latitude, Lon = user.Longitude}, RecordCount = 1};

            return temp.ToList();
        }

        // check if the username already exists
        public static bool UserNameExists(string userName)
        {
            Membership.ApplicationName = ConfigurationManager.AppSettings["applicationName"];
            MembershipUserCollection users = Membership.FindUsersByName(userName);
            foreach (MembershipUser tempUser in users)
            {
                if (tempUser.UserName == userName)
                {
                    return true;
                }
            }
            return false;
        }

        //create a new user
        public static void CreateUser(UserProfile user)
        {
            user.LiveID = HttpContext.Current.Session["ApplicationUserID"].ToString();

            // do mp webservice lookup
            MapPointLogic mp = new MapPointLogic();
            double latitude;
            double longitude;
            mp.GeocodePostCode(user.Postcode, out latitude, out longitude);
            user.Latitude = latitude;
            user.Longitude = longitude;

            // Create membership user
            Membership.ApplicationName = ConfigurationManager.AppSettings["applicationName"];
            MembershipCreateStatus status;
            MembershipUser membershipUser = Membership.CreateUser(user.DisplayName, Membership.GeneratePassword(12, 3),
                                                                  null, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), true, out status);
            // add them to the RegisterUser role
            Roles.ApplicationName = Membership.ApplicationName;
            Roles.AddUserToRole(user.DisplayName, "RegisteredUser");

            // Associate the LiveID with the newly created user
            AssociationManager.AssociateAuthentication(user.LiveID, membershipUser.UserName);

            // Save the user's profile
            ContosoBankDataContext db = new ContosoBankDataContext();
            db.UserProfiles.InsertOnSubmit(user);
            db.SubmitChanges();

            // Log the user into forms authentication.
            SetContextUser(membershipUser);
        }

        public static void SetContextUser(MembershipUser user)
        {
            FormsAuthentication.SetAuthCookie(user.UserName, false);
        }
    }
}