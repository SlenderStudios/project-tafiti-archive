using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq;
using System.Text;

using WLQuickApps.FieldManager.Data;
using System.Security;
using System.Web;

namespace WLQuickApps.FieldManager.Business
{
    public class FieldsManager
    {
        static private ReadOnlyCollection<Field> GetListFromTable(IEnumerable<Field> items)
        {
            return (new List<Field>(items)).AsReadOnly();
        }

        static public ReadOnlyCollection<Field> GetAllFields(int startRowIndex, int maximumRows)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return FieldsManager.GetListFromTable(
                    (from items in context.Fields select items)
                    .Skip(startRowIndex)
                    .Take(maximumRows));
            }
        }

        static public ReadOnlyCollection<Field> GetFieldsForUser(Guid userID, int startRowIndex, int maximumRows)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return FieldsManager.GetListFromTable(
                    (from items in context.Fields
                     where (context.UserFields.Count(
                        countItem => ((countItem.UserID == userID) && (countItem.FieldID == items.FieldID))) > 0)
                     select items)
                    .Skip(startRowIndex)
                    .Take(maximumRows));
            }
        }

        static public int GetFieldsForUserCount(Guid userID)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return context.UserFields.Count(item => item.UserID == userID);
            }
        }

        static public ReadOnlyCollection<Field> GetFieldsForLeague(int leagueID)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return FieldsManager.GetListFromTable(
                    (from items in context.Fields
                     where items.LeagueFields.Contains(
                        (from leagueFieldItem in context.LeagueFields
                         where leagueFieldItem.LeagueID == leagueID
                         select leagueFieldItem).Single())
                     select items));
            }
        }

        static public int GetFieldsForFieldCount(int fieldID)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return context.LeagueFields.Count(item => item.FieldID == fieldID);
            }
        }

        static public ReadOnlyCollection<Field> GetFieldsInRange(double nLatitude, double sLatitude, double eLongitude, double wLongitude, int maximumResults)
        {
            if (nLatitude < sLatitude)
            {
                double temp = nLatitude;
                nLatitude = sLatitude;
                sLatitude = temp;
            }
            if (wLongitude > eLongitude)
            {
                double temp = wLongitude;
                wLongitude = eLongitude;
                eLongitude = temp;
            }

            if (maximumResults > 25) { maximumResults = 25; }

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return FieldsManager.GetListFromTable(
                    (from items in context.Fields
                     where (
                        (items.Latitude <= nLatitude) && 
                        (items.Latitude >= sLatitude) &&
                        (items.Longitude <= eLongitude) &&
                        (items.Longitude >= wLongitude))
                     select items)
                    .Take(maximumResults));
            }
        }

        static public Field GetField(int fieldID)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return (from items in context.Fields select items).Single(item => item.FieldID == fieldID);
            }
        }

        static public bool IsFieldAdmin(int fieldID)
        {
            if (!UserManager.UserIsLoggedIn()) { return false; }

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return (context.FieldAdmins.Count(items => ((items.UserID == UserManager.LoggedInUser.UserID) && (items.FieldID == fieldID))) > 0);
            }
        }

        static public Field CreateField(string title, string description, string address, double latitude, double longitude, int numberOfFields, string parkingLot, string phoneNumber, string status)
        {
            UserManager.VerifyAUserIsLoggedIn();

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                Field field = new Field();
                field.Address = address;
                field.Description = description;
                field.IsOpen = true;
                field.Latitude = latitude;
                field.Longitude = longitude;
                field.NumberOfFields = numberOfFields;
                field.ParkingLot = parkingLot;
                field.PhoneNumber = phoneNumber;
                field.Title = title;
                field.Status = status;

                context.Fields.InsertOnSubmit(field);
                context.SubmitChanges();

                FieldAdmin fieldAdmin = new FieldAdmin();
                fieldAdmin.UserID = UserManager.LoggedInUser.UserID;
                fieldAdmin.FieldID = field.FieldID;
                context.FieldAdmins.InsertOnSubmit(fieldAdmin);
                context.SubmitChanges();

                FieldsManager.AddFieldUser(field.FieldID);

                return field;
            }
        }

        static public void AddFieldToLeague(int fieldID, int leagueID)
        {
            LeagueManager.IsLeagueAdmin(leagueID);

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                context.LeagueFields.DeleteAllOnSubmit(
                    from items in context.LeagueFields
                    where ((items.FieldID == fieldID) && (items.LeagueID == leagueID))
                    select items);
                context.SubmitChanges();

                LeagueField leagueField = new LeagueField();
                leagueField.FieldID = fieldID;
                leagueField.LeagueID = leagueID;
                context.LeagueFields.InsertOnSubmit(leagueField);

                context.SubmitChanges();
            }
        }

        static public void AddFieldAdmin(int fieldID, Guid userID)
        {
            FieldsManager.VerifyOwnerAction(fieldID);

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                context.UserFields.DeleteAllOnSubmit(
                    from items in context.UserFields
                    where ((items.FieldID == fieldID) && (items.UserID == userID))
                    select items);

                context.FieldAdmins.DeleteAllOnSubmit(
                    from items in context.FieldAdmins
                    where ((items.FieldID == fieldID) && (items.UserID == userID))
                    select items);
                context.SubmitChanges();

                Field field = (from items in context.Fields select items).Single(item => item.FieldID == fieldID);
                
                FieldAdmin fieldAdmin = new FieldAdmin();
                fieldAdmin.UserID = userID;
                fieldAdmin.FieldID = field.FieldID;
                context.FieldAdmins.InsertOnSubmit(fieldAdmin);

                UserField userField = new UserField();
                userField.UserID = userID;
                userField.FieldID = field.FieldID;
                context.UserFields.InsertOnSubmit(userField);
                
                context.SubmitChanges();
            }
        }

        static public void AddFieldUser(int fieldID)
        {
            UserManager.VerifyAUserIsLoggedIn();

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                Field field = (from items in context.Fields select items).Single(item => item.FieldID == fieldID);
                UserField userField = new UserField();
                userField.UserID = UserManager.LoggedInUser.UserID;
                userField.FieldID = field.FieldID;
                context.UserFields.InsertOnSubmit(userField);
                context.SubmitChanges();
            }
        }

        static public void UpdateField(int fieldID, string title, string description, string address, double latitude, double longitude, int numberOfFields, string parkingLot, string phoneNumber, bool isOpen, string status)
        {
            FieldsManager.VerifyOwnerAction(fieldID);
            
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                Field field = (from items in context.Fields select items).Single(item => item.FieldID == fieldID);
                field.Address = address;
                field.Description = description;
                field.IsOpen = isOpen;
                field.Latitude = latitude;
                field.Longitude = longitude;
                field.NumberOfFields = numberOfFields;
                field.ParkingLot = parkingLot;
                field.PhoneNumber = phoneNumber;
                field.Status = status;
                field.Title = title;

                context.SubmitChanges();
            }
        }

        static public void ChangeFieldStatus(int fieldID, bool isOpen, string status)
        {
            FieldsManager.VerifyOwnerAction(fieldID);

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                Field field = (from items in context.Fields select items).Single(item => item.FieldID == fieldID);
                field.IsOpen = isOpen;
                field.Status = status;
                context.SubmitChanges();
            }
        }


        static public void DeleteField(int fieldID)
        {
            FieldsManager.VerifyOwnerAction(fieldID);

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                Field field = (from items in context.Fields select items).Single(item => item.FieldID == fieldID);
                context.Fields.DeleteOnSubmit(field);
                context.SubmitChanges();
            }
        }
        
        static public void RemoveField(int fieldID)
        {
            UserManager.VerifyAUserIsLoggedIn();

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                UserField userField = (from items in context.UserFields select items).Single(item => ((item.FieldID == fieldID) && (item.UserID == UserManager.LoggedInUser.UserID)));
                context.UserFields.DeleteOnSubmit(userField);
                context.SubmitChanges();

                if (FieldsManager.IsFieldAdmin(fieldID))
                {
                    if (context.FieldAdmins.Count(items => (items.FieldID == fieldID)) == 1)
                    {
                        FieldsManager.DeleteField(fieldID);
                    }
                    else
                    {
                        FieldAdmin fieldAdmin = (from items in context.FieldAdmins select items).Single(item => ((item.FieldID == fieldID) && (item.UserID == UserManager.LoggedInUser.UserID)));
                        context.FieldAdmins.DeleteOnSubmit(fieldAdmin);
                        context.SubmitChanges();
                    }
                }
            }
        }

        static public void RemoveFieldFromLeague(int fieldID, int leagueID)
        {
            LeagueManager.VerifyOwnerAction(leagueID);

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                context.LeagueFields.DeleteAllOnSubmit(
                    from items in context.LeagueFields 
                    where ((items.FieldID == fieldID) && (items.LeagueID == leagueID)) 
                    select items);
                context.SubmitChanges();
            }
        }

        static public void SendMessageForField(int fieldID, string message, string url)
        {
            Dictionary<string, bool> userIDs = new Dictionary<string, bool>();

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                foreach (var user in(
                    from users in context.Users
                    from userFields in context.UserFields
                    where (userFields.FieldID == fieldID) && (userFields.UserID == users.UserID)
                        select new { UserID = users.UserID}))
                {
                    userIDs[user.UserID.ToString("n")] = true;
                }

                LiveAlertsWrapper.SendAlert(userIDs.Keys, message, url, UserManager.LoggedInUser.DisplayName);
            }

        }

        static internal void VerifyOwnerAction(int fieldID)
        {
            UserManager.VerifyAUserIsLoggedIn();

            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                if (context.FieldAdmins.Count(item => ((item.UserID == UserManager.LoggedInUser.UserID) && (item.FieldID == fieldID))) == 0)
                {
                    throw new SecurityException("You must be an administrator for this field to perform this action");
                }
            }

        }

    }
}
