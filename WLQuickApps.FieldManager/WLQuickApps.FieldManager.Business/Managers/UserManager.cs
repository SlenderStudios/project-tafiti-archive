using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Linq;
using System.Transactions;
using System.Text;
using System.Web.Security;

using WLQuickApps.FieldManager.Data;
using System.Security;
using System.Web.UI;
using System.Web;

namespace WLQuickApps.FieldManager.Business
{
    public class UserManager
    {
        static public void VerifyAUserIsLoggedIn()
        {
            if (!UserManager.UserIsLoggedIn())
            {
                throw new SecurityException("You must be logged in to do this");
            }
        }

        static public bool UserIsLoggedIn()
        {
            return (UserManager.LoggedInUser != null);
        }

        static public void LogOut()
        {
            if (HttpContext.Current.Items.Contains(Constants.ContextCacheKeys.LoggedInUser))
            {
                HttpContext.Current.Items.Add(Constants.ContextCacheKeys.LoggedInUser, null);
            }

            FormsAuthentication.SignOut();
        }

        static public User LoggedInUser
        {
            get
            {
                User user = null;

                if (HttpContext.Current.Items.Contains(Constants.ContextCacheKeys.LoggedInUser))
                {
                    user = HttpContext.Current.Items[Constants.ContextCacheKeys.LoggedInUser] as User;
                }
                else
                {
                    MembershipUser membershipUser = Membership.GetUser();
                    if (membershipUser != null)
                    {
                        user = UserManager.GetUser((Guid)membershipUser.ProviderUserKey);
                        HttpContext.Current.Items.Add(Constants.ContextCacheKeys.LoggedInUser, user);
                    }
                }
                
                return user;
            }
        }

        static private ReadOnlyCollection<User> GetListFromTable(IEnumerable<User> items)
        {
            return (new List<User>(items)).AsReadOnly();
        }

        static public ReadOnlyCollection<User> GetAllUsers(int startRowIndex, int maximumRows)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return UserManager.GetListFromTable(
                    (from items in context.Users select items)
                    .Skip(startRowIndex)
                    .Take(maximumRows));
            }
        }

        static public bool UserExists(string liveIDToken)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return context.Users.Count(item => item.LiveIDToken == liveIDToken) > 0;
            }
        }

        static public User GetUser(string liveIDToken)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return (from items in context.Users select items).Single(item => item.LiveIDToken == liveIDToken);
            }
        }

        static public User GetUser(Guid userID)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return (from items in context.Users select items).Single(item => item.UserID == userID);
            }
        }

        static public ReadOnlyCollection<User> GetUsersForLeague(int leagueID, int startRowIndex, int maximumRows)
        {
            using (FieldManagerDataContext usersContext = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                //return UserManager.GetListFromTable(
                //    (from users in usersContext.Users
                //     where 
                //        (users.MessengerPresenceID.Length > 0) &&
                //        usersContext.UserLeagues.Contains(
                //            (from userLeague in usersContext.UserLeagues
                //             // where userLeague.LeagueID == leagueID
                //             where userLeague.LeagueID == leagueID && userLeague.UserID == users.UserID
                //             select userLeague).Single())
                //     select users)
                //    .Skip(startRowIndex)
                //    .Take(maximumRows));

                 //return UserManager.GetListFromTable(
                 //   (from users in usersContext.Users
                 //    join userLeagues in usersContext.UserLeagues on users.UserID equals userLeagues.UserID
                 //    where (users.MessengerPresenceID.Length > 0 && userLeagues.LeagueID == leagueID)
                 //    select users)
                 //   .Skip(startRowIndex)
                 //   .Take(maximumRows));

                return UserManager.GetListFromTable(
                    (from userLeagues in usersContext.UserLeagues
                     where (userLeagues.LeagueID == leagueID && userLeagues.User.MessengerPresenceID.Length > 0)
                     select userLeagues.User)
                    .Skip(startRowIndex)
                    .Take(maximumRows));
            }
        }

        static public int GetUsersForLeagueCount(int leagueID)
        {
            using (FieldManagerDataContext usersContext = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                //return usersContext.UserLeagues.Count(item => item.LeagueID == leagueID);
                return (from userLeagues in usersContext.UserLeagues
                             where (userLeagues.LeagueID == leagueID && userLeagues.User.MessengerPresenceID.Length > 0)
                             select userLeagues.User).Count();
            }
        }

        static public ReadOnlyCollection<User> GetUsersForField(int fieldID, int startRowIndex, int maximumRows)
        {
            using (FieldManagerDataContext usersContext = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                return UserManager.GetListFromTable(
                    (from userFields in usersContext.UserFields
                     where (userFields.FieldID == fieldID && userFields.User.MessengerPresenceID.Length > 0)
                     select userFields.User)
                    .Skip(startRowIndex)
                    .Take(maximumRows));
            }
        }

        static public int GetUsersForFieldCount(int fieldID)
        {
            using (FieldManagerDataContext usersContext = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                //return usersContext.UserLeagues.Count(item => item.LeagueID == leagueID);
                return (from userFields in usersContext.UserFields
                        where (userFields.FieldID == fieldID && userFields.User.MessengerPresenceID.Length > 0)
                        select userFields.User).Count();
            }
        }

        static public User CreateUser(string email, string liveIDToken)
        {
            User user;
            using (TransactionScope scope = new TransactionScope())
            {
                MembershipUser membershipUser = Membership.CreateUser(email, Membership.GeneratePassword(10, 4), email);

                using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
                {
                    user = new User();
                    user.UserID = (Guid) membershipUser.ProviderUserKey;
                    user.LiveIDToken = liveIDToken;
                    user.Address = string.Empty;
                    user.DisplayName = email;
                    user.MessengerPresenceID = string.Empty;

                    context.Users.InsertOnSubmit(user);
                    context.SubmitChanges();
                }
                scope.Complete();
            }

            return user;
        }

        static public void UpdateUser(string displayName, string address, string messengerPresenceID)
        {
            using (FieldManagerDataContext context = new FieldManagerDataContext(SettingsWrapper.ConnectionString))
            {
                User user = context.Users.Single(item => item.UserID == UserManager.LoggedInUser.UserID);
                user.Address = address;
                user.DisplayName = displayName;
                user.MessengerPresenceID = messengerPresenceID;
                context.SubmitChanges();
            }
        }

    }
}
