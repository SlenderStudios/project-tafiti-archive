using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Text;
using System.Web;

using WLQuickApps.Tafiti.Data;
using WLQuickApps.Tafiti.Data.UserDataSetTableAdapters;

namespace WLQuickApps.Tafiti.Business
{
    public class UserManager
    {
        const string UserIDKey = "LoggedInUser.UserID";

        static private string HashEmail(string email)
        {
            return "";
        }

        static public bool IsUserLoggedIn
        {
            get { return (UserManager.LoggedInUser != null); }
        }

        static public User LoggedInUser
        {
            get 
            {
                if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session[UserIDKey] != null))
                {
                    return UserManager.LogIn(HttpContext.Current.Session[UserIDKey] as string);
                }

                return null; 
            }
        }

        static public void VerifyUserIsLoggedIn()
        {
            if (!UserManager.IsUserLoggedIn) 
            {
                throw new SecurityException("You must be logged in to do this"); 
            }
        }

        // TODO: Optimize
        static public User LogIn(string userID)
        {
            UserManager.LogOut();
            User user = UserManager.GetUserByID(userID);
            if (user == null)
            {
                user = UserManager.CreateUser(userID, "Anonymous");
            }
            HttpContext.Current.Session.Add(UserIDKey, user.UserID);
            return user;
        }

        static public void LogOut()
        {
            if (HttpContext.Current.Session[UserIDKey] != null)
            {
                HttpContext.Current.Session.Remove(UserIDKey);
            }
        }

        static private ReadOnlyCollection<User> GetUsersFromTable(UserDataSet.UsersDataTable table)
        {
            List<User> list = new List<User>();
            foreach (UserDataSet.UsersRow row in table)
            {
                User user = new User(row);
                list.Add(user);
            }
            return list.AsReadOnly();
        }

        static public User CreateUser(string userID, string displayName)
        {
            using (UsersTableAdapter adapter = new UsersTableAdapter())
            {
                adapter.CreateUser(userID, displayName);
            }

            return UserManager.GetUserByID(userID);
        }

        static public User GetUserByID(string userID)
        {
            using (UsersTableAdapter adapter = new UsersTableAdapter())
            {
                UserDataSet.UsersDataTable table = adapter.GetUserByUserID(userID);
                if (table.Rows.Count == 0) { return null; }
                return new User(table[0]);
            }
        }

        static public ReadOnlyCollection<User> GetShelfOwners(Guid shelfID)
        {
            using (UsersTableAdapter adapter = new UsersTableAdapter())
            {
                return UserManager.GetUsersFromTable(adapter.GetShelfStackOwners(shelfID));
            }
        }

        static public User GetUserByEmailHash(string emailHash)
        {
            using (UsersTableAdapter adapter = new UsersTableAdapter())
            {
                UserDataSet.UsersDataTable table = adapter.GetUserByEmailHash(emailHash);
                if (table.Rows.Count == 0) { return null; }
                return new User(table[0]);
            }
        }

        static public ReadOnlyCollection<User> GetAllUsers()
        {
            using (UsersTableAdapter adapter = new UsersTableAdapter())
            {
                return UserManager.GetUsersFromTable(adapter.GetAllUsers());
            }
        }

        static public void DeleteUser(User user)
        {
            using (UsersTableAdapter adapter = new UsersTableAdapter())
            {
                adapter.DeleteUser(user.UserID);
            }
        }

        static public void UpdateUser(User user)
        {
            ShelfStackManager.UpdatePendingInvites(user);

            using (UsersTableAdapter adapter = new UsersTableAdapter())
            {
                adapter.UpdateUser(user.UserID, user.EmailCount, user.EmailCountTimestamp, 
                    user.LastLoginTimestamp, user.EmailHash, user.DisplayName, user.MessengerPresenceID, 
                    user.AlwaysSendMessages);
            }
        }


    }
}
