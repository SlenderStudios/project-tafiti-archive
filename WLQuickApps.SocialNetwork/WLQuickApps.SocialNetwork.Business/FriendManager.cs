using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Text.RegularExpressions;
using System.Net.Mail;

using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.FriendDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class FriendManager
    {
        static public List<User> GetFriends()
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return FriendManager.GetFriendsByUser(UserManager.LoggedInUser);
        }

        static private List<User> GetFriendsFromDataTable(FriendDataSet.FriendDataTable friendDataTable, Guid userID)
        {
            List<User> list = new List<User>();

            foreach (FriendDataSet.FriendRow row in friendDataTable)
            {
                if (((Guid) row.RequestedUserID) == userID)
                {
                    list.Add(UserManager.GetUser(row.RequesterUserID));
                }
                else
                {
                    list.Add(UserManager.GetUser(row.RequestedUserID));
                }                    
            }

            return list;
        }

        static public List<User> GetFriendsByUser(User user)
        {
            return FriendManager.GetFriendsByUserID(user.UserID);
        }

        static public List<User> GetFriendsByUserID(Guid userID)
        {
            return FriendManager.GetFriendsByUserID(userID, 0, FriendManager.GetFriendsCountByUserID(userID));
        }

        static public List<User> GetFriendsByUserID(Guid userID, int startRowIndex, int maximumRows)
        {
            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                return FriendManager.GetFriendsFromDataTable(friendTableAdapter.GetFriendsByUserID(userID, startRowIndex, maximumRows), userID);
            }
        }

        static public int GetFriendsCountByUserID(Guid userID)
        {
            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                return (int)friendTableAdapter.GetFriendsByUserIDCount(userID);
            }
        }

        static public List<User> GetPendingFriendRequests()
        {
            UserManager.AssertThatAUserIsLoggedIn();
            Guid userID = UserManager.LoggedInUser.UserID;
            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                return FriendManager.GetFriendsFromDataTable(friendTableAdapter.GetPendingFriendRequests(userID), userID);
            }
        }

        static public List<User> GetPendingFriendInvitations()
        {
            UserManager.AssertThatAUserIsLoggedIn();
            Guid userID = UserManager.LoggedInUser.UserID;
            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                return FriendManager.GetFriendsFromDataTable(friendTableAdapter.GetPendingFriendInvitations(userID), userID);
            }
        }

        static public void AddFriend(User requested)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            if (requested == null) { throw new ArgumentNullException("requested"); }

            FriendManager.AddFriend(UserManager.LoggedInUser, requested);
        }

        static public void AddFriend(User requester, User requested)
        {
            if (requested == null) { throw new ArgumentNullException("requested"); }
            if (requester == null) { throw new ArgumentNullException("requester"); }

            if (requested == requester) { throw new InvalidOperationException("User cannot add themselves as a friend."); }
            FriendManager.VerifyOwnerActionOnFriendship(requester);

            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                if (!FriendManager.CanAddFriend(requester, requested))
                {
                    return;
                }
                
                // Check to see if there's a pending friendship to confirm.
                if (((int)friendTableAdapter.ConfirmFriendship(requested.UserID, requester.UserID)) > 0) { return; }

                int id = friendTableAdapter.CreateFriendRequest(requester.UserID, requested.UserID);
            }
        }

        static public void RemoveFriendship(User requested)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            if (requested == null) { throw new ArgumentNullException("requested"); }

            FriendManager.RemoveFriendship(UserManager.LoggedInUser, requested);
        }

        static public void RemoveFriendship(User requester, User requested)
        {
            if (requested == null) { throw new ArgumentNullException("requested"); }
            if (requester == null) { throw new ArgumentNullException("requester"); }

            if (!FriendManager.CanModifyFriendship(requester))
            {
                FriendManager.VerifyOwnerActionOnFriendship(requested);
            }

            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                friendTableAdapter.RemoveFriendship(requester.UserID, requested.UserID);
            }
        }

        static public bool LookupFriendRequest(User requester, User requested)
        {
            if (requested == null) { throw new ArgumentNullException("requested"); }
            if (requester == null) { throw new ArgumentNullException("requester"); }

            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                return (friendTableAdapter.LookupFriendRequest(requester.UserID, requested.UserID).Rows.Count > 0);
            }
        }

        static public bool ConfirmRelationship(User firstUser, User secondUser)
        {
            if (firstUser == null) { throw new ArgumentNullException("firstUser"); }
            if (secondUser == null) { throw new ArgumentNullException("secondUser"); }

            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                return (friendTableAdapter.LookupFriendStatus(firstUser.UserID, secondUser.UserID).Rows.Count > 0);
            }
        }

        static public bool ConfirmFriendship(User firstUser, User secondUser)
        {
            if (firstUser == null) { throw new ArgumentNullException("firstUser"); }
            if (secondUser == null) { throw new ArgumentNullException("secondUser"); }

            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                FriendDataSet.FriendDataTable friendDataTable = friendTableAdapter.LookupFriendStatus(firstUser.UserID, secondUser.UserID);
                return ((friendDataTable.Rows.Count > 0) && (friendDataTable[0].IsConfirmed));
            }
        }

        static public void RemoveUserFriendships(User user)
        {
            if (user == null) { throw new ArgumentNullException("user"); }

            using (FriendTableAdapter friendTableAdapter = new FriendTableAdapter())
            {
                friendTableAdapter.RemoveUserFriendships(user.UserID);
            }
        }

        static internal void VerifyOwnerActionOnFriendship(User user)
        {
            if (!FriendManager.CanModifyFriendship(user))
            {
                throw new SecurityException("Friendship cannot be modified because it does not belong to the logged in user");
            }
        }

        static public bool CanModifyFriendship(User user)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            return ((UserManager.LoggedInUser != null) && ((UserManager.LoggedInUser.UserID == user.UserID) || UserManager.LoggedInUser.IsAdmin));
        }

        static public bool CanRemoveFriend(User requested)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            if (requested == null) { return false; }

            return FriendManager.CanRemoveFriend(UserManager.LoggedInUser, requested);
        }

        static public bool CanRemoveFriend(User requester, User requested)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            if (requested == null) { return false; }
            if (requester == null) { return false; }
            if (requester.UserID == requested.UserID) { return false; }

            return FriendManager.ConfirmFriendship(requester, requested);
        }
        static public bool CanAddFriend(User requested)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            if (requested == null) { return false; }

            return FriendManager.CanAddFriend(UserManager.LoggedInUser, requested);
        }

        static public bool CanAddFriend(User requester, User requested)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            if (requested == null) { return false; }
            if (requester == null) { return false; }
            if (requester.UserID == requested.UserID) { return false; }

            // Can't add friend if already friends or requester already made request for friend
            return (!FriendManager.ConfirmFriendship(requester, requested) &&
                !FriendManager.LookupFriendRequest(requester, requested));
        }
    }
}
