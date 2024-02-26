using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Web.Security;

using WLQuickApps.FieldManager.Business;
using WLQuickApps.FieldManager.Data;
using System.Web;

namespace WLQuickApps.FieldManager.WebSite
{
    [ServiceContract(Namespace = "WLQuickApps.FieldManager.WebSite")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SiteService
    {
        [OperationContract]
        public LeagueItem CreateLeague(string title, string description, string type)
        {
            return LeagueItem.CreateFromLeague(LeagueManager.CreateLeague(title, description, type));
        }

        [OperationContract]
        public FieldItem CreateField(string title, string description, string address, double latitude, double longitude, int numberOfFields, string parkingLot, string phoneNumber, string status)
        {
            return FieldItem.CreateFromField(FieldsManager.CreateField(title, description, address, latitude, longitude, numberOfFields, parkingLot, phoneNumber, status));
        }

        [OperationContract]
        public LeagueItem GetLeague(int leagueID)
        {
            return LeagueItem.CreateFromLeague(LeagueManager.GetLeague(leagueID));
        }

        [OperationContract]
        public LeagueItem[] GetAllLeagues()
        {
            return LeagueItem.CreateFromLeagueList(LeagueManager.GetAllLeagues(0, LeagueManager.GetAllLeaguesCount()));
        }

        [OperationContract]
        public LeagueItem[] GetLeagues()
        {
            if (UserManager.UserIsLoggedIn())
            {
                return LeagueItem.CreateFromLeagueList(LeagueManager.GetLeaguesForUser(UserManager.LoggedInUser.UserID, 0, LeagueManager.GetLeaguesForUserCount(UserManager.LoggedInUser.UserID)));
            }
            
            return LeagueItem.CreateFromLeagueList(LeagueManager.GetAllLeagues(0, 10));
        }

        [OperationContract]
        public FieldItem GetField(int fieldID)
        {
            return FieldItem.CreateFromField(FieldsManager.GetField(fieldID));
        }

        [OperationContract]
        public Weather[] GetWeather(double latitude, double longitude)
        {
            return WeatherManager.GetWeather(latitude, longitude);
        }


        [OperationContract]
        public void ChangeFieldStatus(int fieldID, bool isOpen, string status)
        {
            FieldsManager.ChangeFieldStatus(fieldID, isOpen, status);
        }

        [OperationContract]
        public FieldItem[] GetMyFields()
        {
            if (!UserManager.UserIsLoggedIn()) { return new FieldItem[0]; }

            return FieldItem.CreateFromFieldList(FieldsManager.GetFieldsForUser(UserManager.LoggedInUser.UserID, 0, FieldsManager.GetFieldsForUserCount(UserManager.LoggedInUser.UserID)));
        }

        [OperationContract]
        public FieldItem[] GetFieldsForUser(int startRowIndex, int maximumRows)
        {
            return FieldItem.CreateFromFieldList(FieldsManager.GetFieldsForUser(UserManager.LoggedInUser.UserID, startRowIndex, maximumRows));
        }

        [OperationContract]
        public int GetFieldsForUserCount()
        {
            return FieldsManager.GetFieldsForUserCount(UserManager.LoggedInUser.UserID);
        }

        [OperationContract]
        public FieldItem[] GetFieldsInRange(double nLatitude, double sLatitude, double eLongitude, double wLongitude)
        {
            return FieldItem.CreateFromFieldList(FieldsManager.GetFieldsInRange(nLatitude, sLatitude, eLongitude, wLongitude, 25));
        }

        [OperationContract]
        public FieldItem[] GetFieldsForLeague(int leagueID)
        {
            return FieldItem.CreateFromFieldList(FieldsManager.GetFieldsForLeague(leagueID));
        }

        [OperationContract]
        public UserItem[] GetUsersForField(int fieldID)
        {
            return UserItem.CreateFromUserList(UserManager.GetUsersForField(fieldID, 0, UserManager.GetUsersForFieldCount(fieldID)));
        }

        [OperationContract]
        public string GetUserNameByEmail(string email)
        {
            return Membership.GetUserNameByEmail(email);
        }

        [OperationContract]
        public string GetDisplayName()
        {
            if (UserManager.UserIsLoggedIn())
            {
                if (UserManager.LoggedInUser.DisplayName.Length > 0)
                {
                    return UserManager.LoggedInUser.DisplayName;
                }

                return Membership.GetUser().Email;
            }

            return string.Empty;
        }


        [OperationContract]
        public void AddToMyFields(int fieldID)
        {
            FieldsManager.AddFieldUser(fieldID);
        }

        [OperationContract]
        public void AddToMyLeagues(int leagueID)
        {
            LeagueManager.AddLeagueUser(leagueID);
        }

        [OperationContract]
        public void AddFieldAdmin(int fieldID, string userID)
        {
            FieldsManager.AddFieldAdmin(fieldID, (Guid) Membership.GetUser(userID).ProviderUserKey);
        }

        [OperationContract]
        public void AddFieldToLeague(int fieldID, int leagueID)
        {
            FieldsManager.AddFieldToLeague(fieldID, leagueID);
        }

        [OperationContract]
        public void UpdateLeague(int leagueID, string title, string description, string type)
        {
            LeagueManager.UpdateLeague(leagueID, title, description, type);
        }

        [OperationContract]
        public void UpdateField(int fieldID, string title, string description, string address, double latitude, double longitude, int numberOfFields, string parkingLot, string phoneNumber, bool isOpen, string status)
        {
            Field field = FieldsManager.GetField(fieldID);
            if (SettingsWrapper.EnableLiveAlerts)
            {
                UriBuilder uriBuilder = new UriBuilder(HttpContext.Current.Request.Url);
                uriBuilder.Path = VirtualPathUtility.ToAbsolute(string.Format("~/Field/ViewField.aspx"));
                uriBuilder.Query = string.Format("fieldID={0}", fieldID);
                uriBuilder.Fragment = string.Empty;
                FieldsManager.SendMessageForField(
                    fieldID,
                    string.Format("{0} is now {1}. Status: {2}", title, (isOpen) ? "open" : "closed", status),
                    uriBuilder.ToString());
            }

            FieldsManager.UpdateField(fieldID, title, description, address, latitude, longitude, numberOfFields, parkingLot, phoneNumber, isOpen, status);
        }

        [OperationContract]
        public void UpdateMessengerID(string messengerPresenceID)
        {
            UserManager.UpdateUser(UserManager.LoggedInUser.DisplayName, UserManager.LoggedInUser.Address, messengerPresenceID);
        }

        [OperationContract]
        public void DeleteLeague(int leagueID)
        {
            LeagueManager.DeleteLeague(leagueID);
        }

        [OperationContract]
        public void DeleteField(int fieldID)
        {
            FieldsManager.DeleteField(fieldID);
        }

        [OperationContract]
        public void RemoveLeague(int leagueID)
        {
            LeagueManager.RemoveLeague(leagueID);
        }

        [OperationContract]
        public void RemoveField(int fieldID)
        {
            FieldsManager.RemoveField(fieldID);
        }

    }
}