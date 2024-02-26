using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Transactions;
using System.Web.Security;

using WLQuickApps.SocialNetwork.Business.Properties;
using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.UserDataSetTableAdapters;
using WLQuickApps.SocialNetwork.Data.BaseItemDataSetTableAdapters;
using System.Web;
using System.Security.Principal;
using System.Web.Caching;

namespace WLQuickApps.SocialNetwork.Business
{
    /// <summary>
    /// Provides data access methods for a user. 
    /// </summary>
    public static class UserManager
    {
        static private List<User> GetUserListFromTable(UserDataSet.UserDataTable userDataTable)
        {
            List<User> list = new List<User>();

            foreach (UserDataSet.UserRow row in userDataTable)
            {
                User user = new User(row, UserManager.IsUserAdmin(row.UserName));
                if (!user.CanView) { continue; }
                list.Add(user);
            }

            return list;
        }

        /// <summary>
        /// Creates a user in the database.
        /// </summary>
        /// <returns></returns>
        static public User CreateUser(string userName, string email, string firstName, string lastName, Gender gender,
            DateTime dateOfBirth, string windowsLiveUUID, string aboutMe, Location location, string rssFeedUrl, byte[] pictureBits, string MessengerPresenceID, string domainAuthenticationToken, string ownerHandle)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("userName is null or empty.", "userName");
            }
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email is null or empty.", "email");
            }
            if (String.IsNullOrEmpty(firstName))
            {
                throw new ArgumentException("firstName is null or empty.", "firstName");
            }
            if (String.IsNullOrEmpty(lastName))
            {
                throw new ArgumentException("lastName is null or empty.", "lastName");
            }
            if (String.IsNullOrEmpty(windowsLiveUUID))
            {
                throw new ArgumentException("windowsLiveUUID is null or empty.", "windowsLiveUUID");
            }

            if (location == null) { throw new ArgumentNullException("location"); }

            if (!Regex.IsMatch(userName, "^[a-z0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
            {
                throw new ArgumentException("The username contains non-alphanumeric characters.", "userName");
            }
            if (!Regex.IsMatch(email, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", RegexOptions.Compiled))
            {
                throw new ArgumentException("The email address is not valid.", "email");
            }
            if (!String.IsNullOrEmpty(rssFeedUrl) && !Regex.IsMatch(rssFeedUrl, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$", RegexOptions.Compiled))
            {
                throw new ArgumentException("The RSS feed URL is not a valid URL.", "rssFeedUrl");
            }

            if (dateOfBirth > DateTime.Now)
            {
                throw new ArgumentException("The date of birth is in the future.", "dateOfBirth");
            }
            else if (dateOfBirth > DateTime.Now.AddYears(-13))
            {
                throw new ArgumentException("The date of birth indicates an age under 13.", "dateOfBirth");
            }

            if (Membership.GetUser(userName, false) != null)
            {
                throw new ArgumentException("The specified username is already taken.", "userName");
            }

            // Creating an account is a multi-step process, so begin a transaction.
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            {
                using (UserTableAdapter userTableAdapter = new UserTableAdapter())
                {
                    if (userTableAdapter.GetUserByEmail(email).Rows.Count > 0)
                    {
                        throw new ArgumentException("A user with the specified email address already exists.", "email");
                    }
                    if (userTableAdapter.GetUserByWindowsLiveUUID(windowsLiveUUID).Rows.Count > 0)
                    {
                        throw new ArgumentException("A user with the specified Windows Live UUID already exists.", "windowsLiveUUID");
                    }

                    // Generate a random password. This will not be used since we
                    // use Windows Live for login.
                    string password = Membership.GeneratePassword(8, 2);

                    // Create an ASP.NET membership user.
                    MembershipUser membershipUser = Membership.CreateUser(userName, password, email);
                    Guid userID = (Guid)membershipUser.ProviderUserKey;

                    int baseItemID = BaseItemManager.CreateBaseItem(Constants.BaseItemTypes.User, location, userName, aboutMe, userID,
                        string.Empty, PrivacyLevel.Public, SettingsWrapper.AutomaticallyApproveNewUsers, "");

                    // Create a profile for the user.
                    userTableAdapter.CreateProfile(userID, firstName, lastName, (int)gender, dateOfBirth, 
                                                   windowsLiveUUID, rssFeedUrl, baseItemID, MessengerPresenceID, domainAuthenticationToken,  ownerHandle);

                    User user = UserManager.GetUser(userID);

                    IPrincipal originalUser;

                    // Login as the user and create a default album. Remember the current login though.
                    if (HttpContext.Current != null)
                    {
                        originalUser = HttpContext.Current.User;
                        HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(user.UserName, "FormsAuthentication"), null);
                    }
                    else
                    {
                        originalUser = Thread.CurrentPrincipal;
                        Thread.CurrentPrincipal =
                            new GenericPrincipal(new GenericIdentity(user.UserName, "FormsAuthentication"), null);
                    }

                    // Create an album
                    Album defaultAlbum = AlbumManager.CreateAlbum("My Media");

                    // Add the user's picture if they have uploaded one.
                    if (pictureBits != null)
                    {
                        user.SetThumbnail(pictureBits);
                    }


                    GroupManager.MigratePendingInvitesForUser(user);

                    // update the user
                    user.Update();

                    // Reset the logged-in user to the original.
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.User = originalUser;
                    }
                    else
                    {
                        Thread.CurrentPrincipal = originalUser;
                    }

                    // compete the transaction
                    transactionScope.Complete();

                    // return the user
                    return user;
                }
            }
        }

        static public bool IsUserOnline(User user)
        {
            if (user == null) { throw new ArgumentNullException("user"); }

            MembershipUser membershipUser = Membership.GetUser(user.UserName, false);
            if (membershipUser == null)
            {
                return false;
            }

            return membershipUser.IsOnline;
        }

        /// <summary>
        /// Retrieves a user from the database.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        static public User GetUser(Guid userID)
        {
            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                UserDataSet.UserDataTable userTable = userTableAdapter.GetUserByUserID(userID);

                if (userTable.Rows.Count != 1)  
                {
                    throw new ArgumentException("The user with the specified ID does not exist");
                }

                User user = new User(userTable[0], UserManager.IsUserAdmin(userTable[0].UserName));
                if (!user.CanView) { throw new SecurityException("The current user does not have permission to access this item"); }
                return user;
            }
        }

        static public User GetUserByBaseItemID(int baseItemID)
        {
            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                UserDataSet.UserDataTable userTable = userTableAdapter.GetUserByBaseItemID(baseItemID);

                if (userTable.Rows.Count != 1)
                {
                    throw new ArgumentException("The user with the specified ID does not exist");
                }

                User user = new User(userTable[0], UserManager.IsUserAdmin(userTable[0].UserName));
                if (!user.CanView) { throw new SecurityException("The current user does not have permission to access this item"); }
                return user;
            }
        }

        static public User GetUserByUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                if (!UserManager.IsUserLoggedIn())
                {
                    throw new ArgumentException("UserName cannot be null or empty");
                }
                else
                {
                    userName = UserManager.LoggedInUser.UserName;
                }
            }

            MembershipUser membershipUser = Membership.GetUser(userName, false);
            if (membershipUser == null) { throw new ArgumentException("No user exists by that UserName"); }

            return UserManager.GetUser((Guid) membershipUser.ProviderUserKey);
        }

        static public List<User> GetUsersByFullName(string firstName, string lastName)
        {
            return UserManager.GetUsersByFullName(firstName, lastName, 0, UserManager.GetUsersByFullNameCount(firstName, lastName));
        }

        static public int GetUsersByFullNameCount(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName)) { throw new ArgumentException("firstName cannot be null or empty"); }
            if (string.IsNullOrEmpty(lastName)) { throw new ArgumentException("lastName cannot be null or empty"); }

            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                return (int) userTableAdapter.GetUsersByFullNameCount(firstName, lastName);
            }
        }

        static public List<User> GetUsersByFullName(string firstName, string lastName, int startRowIndex, int maximumRows)
        {

            if (string.IsNullOrEmpty(firstName)) { throw new ArgumentException("firstName cannot be null or empty"); }
            if (string.IsNullOrEmpty(lastName)) { throw new ArgumentException("lastName cannot be null or empty"); }

            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                return UserManager.GetUserListFromTable(userTableAdapter.GetUsersByFullName(firstName, lastName, startRowIndex, maximumRows));
            }
        }
        
        /// <summary>
        /// Retrieves a user from the database.
        /// </summary>
        /// <param name="windowsLiveUUID"></param>
        /// <returns></returns>
        static public User GetUser(string windowsLiveUUID)
        {
            if (String.IsNullOrEmpty(windowsLiveUUID))
            {
                throw new ArgumentException("windowsLiveUUID is null or empty.", "windowsLiveUUID");
            }

            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                UserDataSet.UserDataTable userTable = userTableAdapter.GetUserByWindowsLiveUUID(windowsLiveUUID);

                if (userTable.Rows.Count != 1)
                {
                    throw new ArgumentException("The user with the specified Live CID does not exist");
                }

                return new User(userTable[0], UserManager.IsUserAdmin(userTable[0].UserName));
            }
        }

        /// <summary>
        /// Retrieves a user from the database.
        /// </summary>
        /// <param name="windowsLiveUUID"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        static public bool TryGetUserByWindowsLiveUUID(string windowsLiveUUID, out User user)
        {
            if (String.IsNullOrEmpty(windowsLiveUUID))
            {
                throw new ArgumentException("windowsLiveUUID is null or empty.", "windowsLiveUUID");
            }

            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                UserDataSet.UserDataTable userTable = userTableAdapter.GetUserByWindowsLiveUUID(windowsLiveUUID);

                if (userTable.Rows.Count != 1)
                {
                    user = null;
                    return false;
                }
                else
                {
                    user = new User(userTable[0], UserManager.IsUserAdmin(userTable[0].UserName));
                    return true;
                }
            }
        }

        static public List<User> GetUserSetByBaseItemID(int baseItemID)
        {
            return UserManager.GetUserSetByBaseItemID(baseItemID, 0, UserManager.GetUserSetByBaseItemIDCount(baseItemID));
        }

        static public int GetUserSetByBaseItemIDCount(int baseItemID)
        {
            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                return (int) userTableAdapter.GetUserSetByBaseItemIDCount(baseItemID);
            }
        }

        static public List<User> GetUserSetByBaseItemID(int baseItemID, int startRowIndex, int maximumRows)
        {
            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                return UserManager.GetUserListFromTable(userTableAdapter.GetUserSetByBaseItemID(baseItemID, startRowIndex, maximumRows));
            }
        }

        /// <summary>
        /// Retrieves a user from the database.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        static public bool TryGetUserByEmail(string email, out User user)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentException("email is null or empty.", "email");
            }

            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                UserDataSet.UserDataTable userTable = userTableAdapter.GetUserByEmail(email);

                if (userTable.Rows.Count != 1)
                {
                    user = null;
                    return false;
                }
                else
                {
                    user = new User(userTable[0], UserManager.IsUserAdmin(userTable[0].UserName));
                    return true;
                }
            }
        }

        static public List<User> GetAllUsersForEvent(int eventID, UserGroupStatus status)
        {
            int groupID = EventManager.GetEvent(eventID).BaseItemID;
            return UserManager.GetAllUsersForGroup(groupID, status);
        }

        static public List<User> GetAllUsersForGroup(int groupID, UserGroupStatus status)
        {
            return UserManager.GetAllUsersForGroup(groupID, status, 0, UserManager.GetAllUsersForGroupCount(groupID, status));
        }

        static public List<User> GetAllUsersForGroup(int groupID, UserGroupStatus status, int startRowIndex, int maximumRows)
        {
            bool isInviteAccepted;
            bool isApprovedByOwner;
            GroupManager.PersistUserGroupStatus(status, out isInviteAccepted, out isApprovedByOwner);

            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                return UserManager.GetUserListFromTable(userTableAdapter.GetAllUsersForGroup(groupID, isInviteAccepted, isApprovedByOwner, startRowIndex, maximumRows));
            }
        }

        static public int GetAllUsersForGroupCount(int groupID, UserGroupStatus status)
        {
            bool isInviteAccepted;
            bool isApprovedByOwner;
            GroupManager.PersistUserGroupStatus(status, out isInviteAccepted, out isApprovedByOwner);

            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                return (int) userTableAdapter.GetAllUsersForGroupCount(groupID, isInviteAccepted, isApprovedByOwner);
            }
        }

        /// <summary>
        /// Retrieves a user from the database.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        static public bool TryGetUserByUserName(string userName, out User user)
        {
            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("userName is null or empty.", "userName");
            }

            user = null;

            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                UserDataSet.UserDataTable userTable = userTableAdapter.GetUserByUserName(userName);

                if (userTable.Rows.Count != 1)
                {
                    return false;
                }
                else
                {
                    User tempUser = new User(userTable[0], UserManager.IsUserAdmin(userTable[0].UserName));
                    if (tempUser.CanView)
                    {
                        user = tempUser;
                    }

                    return tempUser.CanView;
                }
            }
        }

        /// <summary>
        /// Deletes a user along with all related data and profile info.
        /// </summary>
        /// <param name="userID"></param>
        static internal void DeleteUser(User user)
        {
            UserManager.VerifyOwnerActionOnUser(user);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                foreach (Album album in user.Albums)
                {
                    album.Delete();
                }

                baseItemTableAdapter.DeleteAllBaseItemsForUser(user.UserID);
                FriendManager.RemoveUserFriendships(user);

                Membership.DeleteUser(user.UserName, true);

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <param name="user"></param>
        static public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            UserManager.VerifyOwnerActionOnUser(user);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                userTableAdapter.UpdateProfile(user.FirstName, user.LastName, (int)user.Gender, user.DateOfBirth, user.WindowsLiveUUID,
                    user.RssFeedUrl, user.MessengerPresenceID, user.DomainAuthenticationToken, user.OwnerHandle, user.UserID);

                BaseItemManager.UpdateBaseItem(user);

                transactionScope.Complete();
            }
        }

        static public List<User> GetMostRecentUsers()
        {
            // TODO: Determine how we cap the number of most recent users to return.
            return UserManager.GetMostRecentUsers(0, 8);
        }

        static public List<User> GetMostRecentUsers(int startRowIndex, int maximumRows)
        {
            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                return UserManager.GetUserListFromTable(userTableAdapter.GetRecentlyCreatedUsers(startRowIndex, maximumRows));
            }
        }

        static public int GetMostRecentUsersCount()
        {
            return 20;
        }

        static public int GetUserCount()
        {
            using (UserTableAdapter userTableAdapter = new UserTableAdapter())
            {
                return (int)userTableAdapter.GetUserCount();
            }
        }

        static internal void VerifyOwnerActionOnUser(User user)
        {
            if (!UserManager.CanModifyUser(user.UserID))
            {
                throw new SecurityException("User cannot be modified because the logged in user does not have sufficient permissions.");
            }
        }

        static public bool CanViewUser(User user)
        {
            return true;
        }

        static public bool CanModifyUser(Guid userID)
        {
            User user = UserManager.GetUser(userID);
            return UserManager.CanModifyUser(user);
        }

        static internal bool CanModifyUser(User user)
        {
            return ((UserManager.LoggedInUser != null) &&
                ((user == UserManager.LoggedInUser) || UserManager.LoggedInUser.IsAdmin));
        }

        static internal bool IsUserAdmin(string userName)
        {
            return Roles.IsUserInRole(userName, "Administrators");
        }

        static public User LoggedInUser
        {
            get
            {
                string userName = null;
                User user = null;

                if (HttpContext.Current != null)
                {
                    // Try to get the username and cached User object from the context.
                    userName = HttpContext.Current.User.Identity.Name;
                    if (string.IsNullOrEmpty(userName))
                    {
                        return null;
                    }

                    user = HttpContext.Current.Cache[userName] as User;
                }

                if (user == null)
                {
                    MembershipUser membershipUser = Membership.GetUser(true);

                    if (membershipUser == null)
                    {
                        // Not logged in.
                        return null;
                    }

                    membershipUser.LastActivityDate = DateTime.Now;
                    membershipUser.LastLoginDate = DateTime.Now;
                    // TODO: This continues to cause transaction deadlocks, so we're disabling for now.
                    Membership.UpdateUser(membershipUser);
                    
                    user = UserManager.GetUser((Guid)membershipUser.ProviderUserKey);
                    
                    if (HttpContext.Current != null)
                    {
                        // Put the user object in the cache and have it expire in five seconds.
                        HttpContext.Current.Cache.Insert(userName, user, null, DateTime.Now.AddSeconds(5), Cache.NoSlidingExpiration);
                    }
                }

                return user;
            }
        }
        
        static public bool IsUserLoggedIn()
        {
            string userName = null;
            User user = null;

            if (HttpContext.Current != null)
            {
                // Try to get the username and cached User object from the context.
                userName = HttpContext.Current.User.Identity.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return false;
                }

                user = HttpContext.Current.Cache[userName] as User;
            }

            if (user == null)
            {
                MembershipUser membershipUser = Membership.GetUser(true);

                if (membershipUser == null)
                {
                    // Not logged in.
                    return false;
                }
            }
            return true;
        }

        static public void AssertThatAUserIsLoggedIn()
        {
            if (UserManager.LoggedInUser == null)
            {
                throw new SecurityException("The user must be logged in");
            }
        }
    }
}
