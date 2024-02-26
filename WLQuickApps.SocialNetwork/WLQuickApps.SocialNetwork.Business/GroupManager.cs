using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Security;

using WLQuickApps.SocialNetwork.Business.Properties;
using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.BaseItemDataSetTableAdapters;
using WLQuickApps.SocialNetwork.Data.GroupDataSetTableAdapters;
using WLQuickApps.SocialNetwork.Data.PendingEmailInvitesDataSetTableAdapters;
using System.Transactions;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class GroupManager
    {
        static private ReadOnlyCollection<Group> GetGroupsFromTable(GroupDataSet.GroupDataTable groupDataTable)
        {
            List<Group> list = new List<Group>();
            foreach (GroupDataSet.GroupRow row in groupDataTable)
            {
                Group group = new Group(row);
                if (!group.CanView) { continue; }
                list.Add(group);
            }
            return list.AsReadOnly();
        }

        #region Create

        static public Group CreateGroup(string name, string description, string groupType, PrivacyLevel privacyLevel)
        {
            return GroupManager.CreateGroup(name, description, groupType, Location.Empty, privacyLevel);
        }

        static public Group CreateGroup(string name, string description, string groupType, Location location, PrivacyLevel privacyLevel)
        {
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("Name cannot be null or empty"); }

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                int baseItemID = Convert.ToInt32(BaseItemManager.CreateBaseItem(Constants.BaseItemTypes.Group, location, name, description, UserManager.LoggedInUser, groupType, privacyLevel, SettingsWrapper.AutomaticallyApproveNewGroups, ""));
                groupTableAdapter.CreateGroup(baseItemID);

                Group group = GroupManager.GetGroup(baseItemID);

                GroupManager.AddUserToGroup(UserManager.LoggedInUser, group);

                group = GroupManager.GetGroup(baseItemID);
                group.Update();
                return group;
            }
        }

        #endregion

        static public Group GetGroup(int baseItemID)
        {
            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                object o = groupTableAdapter.GetGroup(baseItemID);

                if (o == null) { throw new ArgumentException("Group does not exist"); }

                Group group = new Group(Convert.ToInt32(o));
                if (!group.CanView) { throw new SecurityException("The current user does not have permission to access this item"); }
                return group;
            }
        }

        static public bool GroupExists(int baseItemID)
        {
            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return (groupTableAdapter.GetGroup(baseItemID) != null);
            }
        }


        #region GetGroupsByNameExact With Type

        static public ReadOnlyCollection<Group> GetGroupsByNameExact(string name, string groupType, int startRowIndex, int maximumRows)
        {
            if (string.IsNullOrEmpty(name)) { return new List<Group>().AsReadOnly(); }

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return GroupManager.GetGroupsFromTable(groupTableAdapter.GetGroupsByNameAndType(name, groupType, startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Group> GetGroupsByNameExact(string name, string groupType)
        {
            return GroupManager.GetGroupsByNameExact(name, groupType, 0, GroupManager.GetGroupsByNameExactCount(name, groupType));
        }

        static public int GetGroupsByNameExactCount(string name, string groupType)
        {
            if (string.IsNullOrEmpty(name)) { return 0; }

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return (int)groupTableAdapter.GetGroupsByNameAndTypeCount(name, groupType);
            }
        }

        #endregion

        #region GetGroupsByNameExact Without Type

        static public ReadOnlyCollection<Group> GetGroupsByNameExact(string name, int startRowIndex, int maximumRows)
        {
            return GroupManager.GetGroupsByNameExact(name, string.Empty);
        }

        static public ReadOnlyCollection<Group> GetGroupsByNameExact(string name)
        {
            return GroupManager.GetGroupsByNameExact(name, string.Empty);
        }

        static public int GetGroupsByNameExactCount(string name)
        {
            return GroupManager.GetGroupsByNameExactCount(name, string.Empty);
        }

        #endregion

        #region SearchGroupsByName With Type

        static public ReadOnlyCollection<Group> SearchGroupsByName(string name, string groupType, int startRowIndex, int maximumRows)
        {
            if (string.IsNullOrEmpty(name)) { return new List<Group>().AsReadOnly(); }
            if (groupType == null) { groupType = string.Empty; }

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return GroupManager.GetGroupsFromTable(groupTableAdapter.GetGroupsByNameAndType(string.Format("%{0}%", name), groupType, startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Group> SearchGroupsByName(string name, string groupType)
        {
            return GroupManager.SearchGroupsByName(name, groupType, 0, GroupManager.SearchGroupsByNameCount(name, groupType));
        }

        static public int SearchGroupsByNameCount(string name, string groupType)
        {
            if (string.IsNullOrEmpty(name)) { return 0; }
            if (groupType == null) { groupType = string.Empty; }

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return (int)groupTableAdapter.GetGroupsByNameAndTypeCount(string.Format("%{0}%", name), groupType);
            }
        }
        #endregion

        #region SearchGroupsByName Without Type
        static public ReadOnlyCollection<Group> SearchGroupsByName(string name, int startRowIndex, int maximumRows)
        {
            return GroupManager.SearchGroupsByName(name, string.Empty, startRowIndex, maximumRows);
        }

        static public ReadOnlyCollection<Group> SearchGroupsByName(string name)
        {
            return GroupManager.SearchGroupsByName(name, string.Empty);
        }

        static public int SearchGroupsByNameCount(string name)
        {
            return GroupManager.SearchGroupsByNameCount(name, string.Empty);
        }
        #endregion

        #region GetGroupsForUser Without Type

        static public ReadOnlyCollection<Group> GetGroupsForUser(string userName, UserGroupStatus status, int startRowIndex, int maximumRows)
        {
            return GroupManager.GetGroupsForUser(userName, string.Empty, status, startRowIndex, maximumRows);
        }

        static public ReadOnlyCollection<Group> GetGroupsForUser(string userName, UserGroupStatus status)
        {
            return GroupManager.GetGroupsForUser(userName, status, 0, GroupManager.GetGroupsForUserCount(userName, status));
        }

        static public int GetGroupsForUserCount(string userName, UserGroupStatus status)
        {
            return (int)GroupManager.GetGroupsForUserCount(userName, string.Empty, status);
        }

        #endregion

        #region GetGroupsForUser With Type

        static public ReadOnlyCollection<Group> GetGroupsForUser(string userName, string groupType, UserGroupStatus status, int startRowIndex, int maximumRows)
        {
            if (string.IsNullOrEmpty(userName))
            {
                UserManager.AssertThatAUserIsLoggedIn();
                userName = UserManager.LoggedInUser.UserName;
            }
            if (groupType == null) { groupType = string.Empty; }

            bool isInviteAccepted;
            bool isApprovedByOwner;
            GroupManager.PersistUserGroupStatus(status, out isInviteAccepted, out isApprovedByOwner);

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return GroupManager.GetGroupsFromTable(groupTableAdapter.GetGroupsForUserByType(
                    UserManager.GetUserByUserName(userName).UserID, groupType, isInviteAccepted, isApprovedByOwner, startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Group> GetGroupsForUser(string userName, string groupType, UserGroupStatus status)
        {
            return GroupManager.GetGroupsForUser(userName, groupType, status, 0, GroupManager.GetGroupsForUserCount(userName, groupType, status));
        }

        static public int GetGroupsForUserCount(string userName, string groupType, UserGroupStatus status)
        {
            if (string.IsNullOrEmpty(userName))
            {
                UserManager.AssertThatAUserIsLoggedIn();
                userName = UserManager.LoggedInUser.UserName;
            }
            if (groupType == null) { groupType = string.Empty; }

            bool isInviteAccepted;
            bool isApprovedByOwner;
            GroupManager.PersistUserGroupStatus(status, out isInviteAccepted, out isApprovedByOwner);

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return Convert.ToInt32(groupTableAdapter.GetGroupsForUserByTypeCount(UserManager.GetUserByUserName(userName).UserID,
                    groupType, isInviteAccepted, isApprovedByOwner));
            }
        }

        #endregion

        #region GetGroupsForUser With Status
        static public ReadOnlyCollection<Group> GetGroupsForUser(UserGroupStatus status, int startRowIndex, int maximumRows)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return GroupManager.GetGroupsForUser(UserManager.LoggedInUser.UserName, status, startRowIndex, maximumRows);
        }

        static public ReadOnlyCollection<Group> GetGroupsForUser(UserGroupStatus status)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return GroupManager.GetGroupsForUser(UserManager.LoggedInUser.UserName, status, 0, GroupManager.GetGroupsForUserCount(status));
        }

        static public int GetGroupsForUserCount(UserGroupStatus status)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return GroupManager.GetGroupsForUserCount(UserManager.LoggedInUser.UserName, status);
        }
        #endregion

        #region GetGroupsForUser With Status And Type
        static public ReadOnlyCollection<Group> GetGroupsForUser(UserGroupStatus status, string groupType, int startRowIndex, int maximumRows)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return GroupManager.GetGroupsForUser(UserManager.LoggedInUser.UserName, groupType, status, startRowIndex, maximumRows);
        }

        static public ReadOnlyCollection<Group> GetGroupsForUser(UserGroupStatus status, string groupType)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return GroupManager.GetGroupsForUser(UserManager.LoggedInUser.UserName, groupType, status, 0, GroupManager.GetGroupsForUserCount(status, groupType));
        }

        static public int GetGroupsForUserCount(UserGroupStatus status, string groupType)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return GroupManager.GetGroupsForUserCount(UserManager.LoggedInUser.UserName, groupType, status);
        }
        #endregion

        #region GetGroupsByGroupType
        static public ReadOnlyCollection<Group> GetGroupsByGroupType(string groupType, int startRowIndex, int maximumRows)
        {
            if (groupType == null) { groupType = string.Empty; }

            return GroupManager.GetGroupsByNameExact("%", groupType, startRowIndex, maximumRows);
        }

        static public ReadOnlyCollection<Group> GetGroupsByGroupType(string groupType)
        {
            return GroupManager.GetGroupsByGroupType(groupType, 0, GroupManager.GetGroupsByGroupTypeCount(groupType));
        }

        static public int GetGroupsByGroupTypeCount(string groupType)
        {
            if (groupType == null) { groupType = string.Empty; }

            return (int)GroupManager.GetGroupsByNameExactCount("%", groupType);
        }
        #endregion

        internal static void PersistUserGroupStatus(UserGroupStatus status, out bool isInviteAccepted, out bool isApprovedByOwner)
        {
            switch (status)
            {
                case UserGroupStatus.Invited:
                    isInviteAccepted = false;
                    isApprovedByOwner = true;
                    break;

                case UserGroupStatus.Joined:
                    isInviteAccepted = true;
                    isApprovedByOwner = true;
                    break;

                case UserGroupStatus.WaitingForApproval:
                    isInviteAccepted = true;
                    isApprovedByOwner = false;
                    break;

                default:
                    isInviteAccepted = false;
                    isApprovedByOwner = false;
                    return;
            }
        }

        static internal void DeleteGroup(Group group)
        {
            GroupManager.VerifyOwnerActionOnGroup(group);

            BaseItemManager.DeleteBaseItem(group);
        }

        static public void UpdateGroup(Group group)
        {
            if (group == null) { throw new ArgumentNullException("group"); }

            GroupManager.VerifyOwnerActionOnGroup(group);

            using (TransactionScope transaction = new TransactionScope())
            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                if (group.PrivacyLevel == PrivacyLevel.Public)
                {
                    groupTableAdapter.ApproveAllUsers(group.BaseItemID);
                }

                BaseItemManager.UpdateBaseItem(group);
                transaction.Complete();
            }
        }

        static public void JoinGroup(Group group)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            if (!group.CanJoin) { throw new SecurityException("The current user does not have permission to join this group"); }
            GroupManager.AddUserToGroup(UserManager.LoggedInUser, group);
        }

        static public void AddUserToGroup(User user, Group group)
        {
            UserManager.AssertThatAUserIsLoggedIn();

            bool isApprovedByOwner = false;
            bool isInviteAccepted = false;

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                if (GroupManager.CanModifyGroup(group) || (group.PrivacyLevel == PrivacyLevel.Public) ||
                    GroupManager.IsUserInvitedToGroup(user, group))
                {
                    isApprovedByOwner = true;
                }

                if ((user == UserManager.LoggedInUser) || (GroupManager.HasUserRequestedToJoinGroup(user, group)))
                {
                    isInviteAccepted = true;
                }

                if (!(isApprovedByOwner) && !(isInviteAccepted))
                {
                    throw new SecurityException("Logged-in user cannot add specified user to this group.");
                }

                groupTableAdapter.AddUserToGroup(group.BaseItemID, user.UserID, isInviteAccepted, isApprovedByOwner);
            }
        }

        static public void LeaveGroup(Group group)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            GroupManager.RemoveUserFromGroup(UserManager.LoggedInUser, group);
        }

        static public void RemoveUserFromGroup(User user, Group group)
        {
            UserManager.AssertThatAUserIsLoggedIn();

            if (user != UserManager.LoggedInUser)
            {
                GroupManager.VerifyOwnerActionOnGroup(group);
            }

            if ((group.Owner == user) && (group.Users.Count > 1))
            {
                throw new InvalidOperationException("Owner cannot leave group until everyone else has left.");
            }

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                if ((group.Users.Count == 1) && (group.Users[0] == user))
                {
                    GroupManager.DeleteGroup(group);
                }
                else
                {
                    groupTableAdapter.RemoveUserFromGroup(group.BaseItemID, user.UserID);
                }
            }
        }

        static internal void AssociateBaseItemWithGroup(BaseItem baseItem, Group group)
        {
            BaseItemManager.VerifyOwnerActionOnBaseItem(baseItem);
            GroupManager.VerifyCanAssociateWithGroup(group);

            if (group.Albums.Contains(baseItem as Album)) { return; }
            if (group.Collections.Contains(baseItem as Collection)) { return; }

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                baseItemTableAdapter.AssociateBaseItemWithBaseItem(baseItem.BaseItemID, group.BaseItemID);
            }
        }

        static public void MigratePendingInvitesForUser(User user)
        {
            using (PendingEmailInvitesTableAdapter pendingEmailInvitesTableAdapter = new PendingEmailInvitesTableAdapter())
            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                foreach (PendingEmailInvitesDataSet.PendingEmailInvitesRow row in pendingEmailInvitesTableAdapter.GetPendingInvitesByEmail(user.Email))
                {
                    groupTableAdapter.AddUserToGroup(row.BaseItemID, user.UserID, false, true);
                }

                pendingEmailInvitesTableAdapter.DeletePendingInvitesByEmail(user.Email);
            }
        }

        static public List<string> GetPendingEmailInvitesForGroup(int baseItemID)
        {
            using (PendingEmailInvitesTableAdapter pendingEmailInvitesTableAdapter = new PendingEmailInvitesTableAdapter())
            {
                List<string> emails = new List<string>();
                foreach (PendingEmailInvitesDataSet.PendingEmailInvitesRow row in pendingEmailInvitesTableAdapter.GetPendingInvitesByBaseItemID(baseItemID))
                {
                    emails.Add(row.Email);
                }
                return emails;
            }
        }

        static public void CancelPendingEmailInvite(int baseItemID, string email)
        {
            using (PendingEmailInvitesTableAdapter pendingEmailInvitesTableAdapter = new PendingEmailInvitesTableAdapter())
            {
                pendingEmailInvitesTableAdapter.CancelPendingInvite(baseItemID, email);
            }
        }

        static public void CreatePendingEmailInvite(int baseItemID, string email)
        {
            using (PendingEmailInvitesTableAdapter pendingEmailInvitesTableAdapter = new PendingEmailInvitesTableAdapter())
            {
                if (((int)pendingEmailInvitesTableAdapter.PendingInviteExists(baseItemID, email)) > 0) { return; }

                pendingEmailInvitesTableAdapter.CreatePendingInvite(baseItemID, email);
            }
        }

        static public bool HasUserJoinedGroup(User user, Group group)
        {
            if (user == null) { throw new ArgumentNullException("user"); }
            if (group == null) { throw new ArgumentNullException("group"); }

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return (bool)groupTableAdapter.HasUserJoinedGroup(user.UserID, group.BaseItemID);
            }
        }

        static public bool HasUserRequestedToJoinGroup(User user, Group group)
        {
            if (user == null) { throw new ArgumentNullException("user"); }
            if (group == null) { throw new ArgumentNullException("group"); }

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return (bool)groupTableAdapter.HasUserRequestedToJoinGroup(user.UserID, group.BaseItemID);
            }
        }

        static public bool IsUserInvitedToGroup(User user, Group group)
        {
            if (user == null) { throw new ArgumentNullException("user"); }
            if (group == null) { throw new ArgumentNullException("group"); }

            using (GroupTableAdapter groupTableAdapter = new GroupTableAdapter())
            {
                return (bool)groupTableAdapter.IsUserInvitedToGroup(user.UserID, group.BaseItemID);
            }
        }

        static internal void VerifyOwnerActionOnGroup(Group group)
        {
            if (!GroupManager.CanModifyGroup(group.BaseItemID))
            {
                throw new SecurityException("Group cannot be modified because it does not belong to the logged in user");
            }
        }

        static internal void VerifyCanAssociateWithGroup(Group group)
        {
            if (!GroupManager.CanAssociateWithGroup(group))
            {
                throw new SecurityException("Group cannot be associated with by the the logged in user");
            }
        }

        static public bool CanCancelJoinGroup(Group group)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            return GroupManager.HasUserRequestedToJoinGroup(UserManager.LoggedInUser, group);
        }

        static public bool CanJoinGroup(Group group)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            return (!group.HasMember(UserManager.LoggedInUser) && !group.CanCancel);
        }

        static public bool CanLeaveGroup(Group group)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            return ((UserManager.LoggedInUser != group.Owner) && group.HasMember(UserManager.LoggedInUser));
        }

        static public bool CanAssociateWithGroup(Group group)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            return (UserManager.LoggedInUser.IsAdmin || group.HasMember(UserManager.LoggedInUser));
        }

        static public bool CanContributeToGroup(Group group)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }

            switch (group.PrivacyLevel)
            {
                case PrivacyLevel.Public: return true;
                case PrivacyLevel.Private:
                case PrivacyLevel.Invisible:
                    return (UserManager.LoggedInUser.IsAdmin || group.HasMember(UserManager.LoggedInUser));
                default: throw new Exception("Unexpected privacy level");
            }
        }

        static public bool CanViewGroup(Group group)
        {
            switch (group.PrivacyLevel)
            {
                case PrivacyLevel.Public:
                case PrivacyLevel.Private:
                    return true;

                case PrivacyLevel.Invisible:
                    if (!UserManager.IsUserLoggedIn()) { return false; }
                    return (group.Owner == UserManager.LoggedInUser) || (UserManager.LoggedInUser.IsAdmin) ||
                        GroupManager.HasUserJoinedGroup(UserManager.LoggedInUser, group) || GroupManager.IsUserInvitedToGroup(UserManager.LoggedInUser, group);

                default: throw new Exception("Unexpected privacy level");
            }
        }

        static internal void VerifyGroupVisible(Group group)
        {
            if (!group.CanView)
            {
                throw new GroupPrivacyException(group.BaseItemID, group.Title);
            }
        }

        static public bool CanModifyGroup(int baseItemID)
        {
            Group group = GroupManager.GetGroup(baseItemID);
            return GroupManager.CanModifyGroup(group);
        }

        static internal bool CanModifyGroup(Group group)
        {
            return ((UserManager.LoggedInUser != null) &&
                ((group.Owner == UserManager.LoggedInUser) || UserManager.LoggedInUser.IsAdmin));
        }
    }
}
