using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for GroupTests
    /// </summary>
    [TestClass]
    public class GroupTests
    {
        public GroupTests()
        {
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize]
        [TestCleanup]
        public void DeleteTestGroups()
        {
            Utilities.SwitchToOwnerUser();
            foreach (Group group in GroupManager.GetGroupsForUser(UserGroupStatus.Joined, Constants.Strings.GroupType))
            {
                group.Delete();
            }
        }

        #endregion

        #region CreateGroup tests
        [TestMethod]
        public void CreateGroup()
        {
            Utilities.SwitchToOwnerUser();

            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);

            Assert.AreEqual(Constants.Strings.GroupName, group.Title);
            Assert.AreEqual(Constants.Strings.GroupType, group.SubType);
            Assert.AreEqual(1, group.Users.Count);
            Assert.AreEqual(Utilities.OwnerUser, group.Users[0]);
            Assert.AreEqual(0.0, group.AverageRating);
            Assert.AreEqual(0, group.TotalViews);
            Assert.AreEqual(0, group.TotalRatingCount);
            Assert.AreEqual(0.0, group.TotalRatingScore);
            Assert.AreEqual(0, group.Comments.Count);
            Assert.AreEqual(Location.Empty, group.Location);
            Assert.AreEqual(0, group.Tags.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateGroupWithEmptyName()
        {
            GroupManager.CreateGroup(string.Empty, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateGroupWithNullName()
        {
            GroupManager.CreateGroup(null, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
        }

        [TestMethod]
        public void CreateGroupWithType()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);

            Assert.AreEqual(Constants.Strings.GroupName, group.Title);
            Assert.AreEqual(Constants.Strings.GroupType, group.SubType);
            Assert.AreEqual(1, group.Users.Count);
            Assert.AreEqual(Utilities.OwnerUser, group.Users[0]);
            Assert.AreEqual(0.0, group.AverageRating);
            Assert.AreEqual(0, group.TotalViews);
            Assert.AreEqual(0, group.TotalRatingCount);
            Assert.AreEqual(0.0, group.TotalRatingScore);
            Assert.AreEqual(0, group.Comments.Count);
            Assert.AreEqual(Location.Empty, group.Location);
            Assert.AreEqual(0, group.Tags.Count);
        }

        #endregion

        #region RetrieveGroup tests
        [TestMethod]
        public void GetGroupByBaseItemID()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Assert.AreEqual(group.BaseItemID, GroupManager.GetGroup(group.BaseItemID).BaseItemID);
            group.Delete();
        }

        [TestMethod]
        public void GetGroupFromBaseItemManager()
        {
            Utilities.SwitchToOwnerUser();

            Group expected = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            BaseItem actual = BaseItemManager.GetBaseItem(expected.BaseItemID);

            Assert.IsInstanceOfType(actual, typeof(Group));
            Assert.AreEqual(expected.BaseItemID, actual.BaseItemID);
            Assert.AreEqual(expected.Title, actual.Title);

            expected.Delete();
        }

        [TestMethod]
        public void GetGroupsPaged()
        {
            Utilities.SwitchToOwnerUser();

            int groupIndex;
            List<Group> myGroups = new List<Group>(10);

            for (groupIndex = 0; groupIndex < 10; groupIndex++)
            {
                Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription,
                    Constants.Strings.GroupType, PrivacyLevel.Public);
                myGroups.Add(group);

            }

            Assert.AreEqual(10, GroupManager.GetGroupsForUserCount(Utilities.OwnerUser.UserName, Constants.Strings.GroupType,
                UserGroupStatus.Joined));

            ReadOnlyCollection<Group> groups = GroupManager.GetGroupsForUser(Utilities.OwnerUser.UserName, Constants.Strings.GroupType, UserGroupStatus.Joined, 0, 5);
            for (groupIndex = 0; groupIndex < 5; groupIndex++)
            {
                Assert.AreEqual(myGroups[groupIndex], groups[groupIndex]);
            }

            groups = GroupManager.GetGroupsForUser(Utilities.OwnerUser.UserName, Constants.Strings.GroupType, UserGroupStatus.Joined, 5, 5);
            for (groupIndex = 0; groupIndex < 5; groupIndex++)
            {
                Assert.AreEqual(myGroups[groupIndex + 5], groups[groupIndex]);
            }
        }

        [TestMethod]
        public void GetGroupsForUser_Owner()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            ReadOnlyCollection<Group> groups = GroupManager.GetGroupsForUser(group.Owner.UserName, Constants.Strings.GroupType, UserGroupStatus.Joined);
            Assert.IsTrue(groups.Contains(group));
            group.Delete();
        }

        [TestMethod]
        public void GetGroupForUser_NonOwner()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToNonOwnerUser();
            GroupManager.JoinGroup(group);
            ReadOnlyCollection<Group> groups = GroupManager.GetGroupsForUser(UserManager.LoggedInUser.UserName, Constants.Strings.GroupType, UserGroupStatus.Joined);
            Assert.IsTrue(groups.Contains(group));
            Utilities.SwitchToOwnerUser();
            group.Delete();
        }

        [TestMethod]
        public void GetGroupForUser_NonMember()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToNonOwnerUser();
            ReadOnlyCollection<Group> groups = GroupManager.GetGroupsForUser(UserManager.LoggedInUser.UserName, Constants.Strings.GroupType, UserGroupStatus.Joined);
            Assert.IsFalse(groups.Contains(group));
            Utilities.SwitchToOwnerUser();
            group.Delete();
        }

        #endregion

        #region SearchGroup tests
        [TestMethod]
        public void SearchGroupByName()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            ReadOnlyCollection<Group> groups = GroupManager.SearchGroupsByName(Constants.Strings.GroupName, Constants.Strings.GroupType);

            // NOTE: There isn't a good way to accurately verify this works when we have multiple instances of the test suite running.
            Assert.IsTrue(groups.Contains(group));
            group.Delete();
        }

        [TestMethod]
        public void SearchGroupByNamePartialBegin()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            ReadOnlyCollection<Group> groups = GroupManager.SearchGroupsByName(Constants.Strings.GroupName.Substring(0, 5), Constants.Strings.GroupType);

            // NOTE: There isn't a good way to accurately verify this works when we have multiple instances of the test suite running.
            Assert.IsTrue(groups.Contains(group));
            group.Delete();
        }

        [TestMethod]
        public void SearchGroupByNamePartialEnd()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            ReadOnlyCollection<Group> groups = GroupManager.SearchGroupsByName(Constants.Strings.GroupName.Substring(Constants.Strings.GroupName.Length - 5, 5), Constants.Strings.GroupType);

            // NOTE: There isn't a good way to accurately verify this works when we have multiple instances of the test suite running.
            Assert.IsTrue(groups.Contains(group));
            group.Delete();
        }

        [TestMethod]
        public void SearchGroupByNameNotFound()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            ReadOnlyCollection<Group> groups = GroupManager.SearchGroupsByName("notfound", Constants.Strings.GroupType);

            // NOTE: There isn't a good way to accurately verify this works when we have multiple instances of the test suite running.
            Assert.IsFalse(groups.Contains(group));
            group.Delete();
        }

        [TestMethod]
        public void SearchGroupByGroupType()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            ReadOnlyCollection<Group> groups = GroupManager.GetGroupsByGroupType(Constants.Strings.GroupType);

            // NOTE: There isn't a good way to accurately verify this works when we have multiple instances of the test suite running.
            Assert.IsTrue(groups.Contains(group));
            group.Delete();
        }

        #endregion

        #region JoinAndLeaveGroup tests
        [TestMethod]
        public void NonOwnerJoinAndLeaveGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToNonOwnerUser();
            GroupManager.JoinGroup(group);
            Assert.IsTrue(group.HasMember(UserManager.LoggedInUser));
            Assert.AreEqual(2, group.Users.Count);
            GroupManager.LeaveGroup(group);
            Assert.IsFalse(group.HasMember(UserManager.LoggedInUser));
            Assert.AreEqual(1, group.Users.Count);
            Utilities.SwitchToOwnerUser();
            group.Delete();
        }

        [TestMethod]
        public void AdminAddsAndRemovesNonOwnerFromGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            User userNonOwner = Utilities.NonOwnerUser;
            Utilities.SwitchToAdminUser();
            GroupManager.AddUserToGroup(userNonOwner, group);
            Assert.IsFalse(group.HasMember(userNonOwner));
            Utilities.SwitchToNonOwnerUser();
            group.Join();
            Assert.IsTrue(group.HasMember(userNonOwner));
            Utilities.SwitchToAdminUser();
            GroupManager.RemoveUserFromGroup(userNonOwner, group);
            Assert.IsFalse(group.HasMember(userNonOwner));
            group.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousJoinGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToAnonymousUser();
            GroupManager.JoinGroup(group);
            group.Delete();
        }

        [TestMethod]
        public void OwnerLeaveGroupWithNoMembers()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            GroupManager.LeaveGroup(group);
            Assert.IsFalse(GroupManager.GroupExists(group.BaseItemID));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OwnerLeaveGroupWithOtherMembers()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToNonOwnerUser();
            GroupManager.JoinGroup(group);
            Utilities.SwitchToOwnerUser();
            GroupManager.LeaveGroup(group);
            group.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousLeaveGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToAnonymousUser();
            GroupManager.LeaveGroup(group);
            group.Delete();
        }

        #endregion

        #region DeleteGroup tests
        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerDeleteGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToNonOwnerUser();
            group.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousDeleteGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToAnonymousUser();
            group.Delete();
        }

        [TestMethod]
        public void OwnerDeleteGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            group.Delete();
            Assert.IsFalse(GroupManager.GroupExists(group.BaseItemID));
        }

        [TestMethod]
        public void AdminDeleteGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToAdminUser();
            group.Delete();
            Assert.IsFalse(GroupManager.GroupExists(group.BaseItemID));
        }
        #endregion

        #region UpdateGroup tests
        [TestMethod]
        public void OwnerUpdateGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            group.Location = Utilities.TestLocation;
            group.PrivacyLevel = PrivacyLevel.Private;
            group.Description = "New Description.";
            group.Title = "New Title.";
            GroupManager.UpdateGroup(group);

            Group updatedGroup = GroupManager.GetGroup(group.BaseItemID);
            Assert.AreEqual(Utilities.TestLocation, updatedGroup.Location);
            Assert.AreEqual(PrivacyLevel.Private, updatedGroup.PrivacyLevel);
            Assert.AreEqual("New Description.", updatedGroup.Description);
            Assert.AreEqual("New Title.", updatedGroup.Title);
        }

        [TestMethod]
        public void AdminUpdateGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToAdminUser();
            group.Location = Utilities.TestLocation;
            GroupManager.UpdateGroup(group);
            Assert.AreEqual(Utilities.TestLocation.LocationID, GroupManager.GetGroup(group.BaseItemID).Location.LocationID);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerUpdateGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToNonOwnerUser();
            group.Location = Utilities.TestLocation;
            GroupManager.UpdateGroup(group);
            Assert.AreEqual(Utilities.TestLocation.LocationID, GroupManager.GetGroup(group.BaseItemID).Location.LocationID);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousUpdateGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToAnonymousUser();
            group.Location = Utilities.TestLocation;
            GroupManager.UpdateGroup(group);
            Assert.AreEqual(Utilities.TestLocation.LocationID, GroupManager.GetGroup(group.BaseItemID).Location.LocationID);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateGroupWithNullGroup()
        {
            GroupManager.UpdateGroup(null);
        }

        [TestMethod]
        public void OwnerSwitchGroupFromPrivateToPublic()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Private);

            Utilities.SwitchToNonOwnerUser();
            GroupManager.JoinGroup(group);
            Assert.IsTrue(GroupManager.HasUserRequestedToJoinGroup(Utilities.NonOwnerUser, group));
            Assert.IsFalse(GroupManager.HasUserJoinedGroup(Utilities.NonOwnerUser, group));

            Utilities.SwitchToOwnerUser();
            group.PrivacyLevel = PrivacyLevel.Public;
            GroupManager.UpdateGroup(group);

            Assert.IsTrue(GroupManager.HasUserJoinedGroup(Utilities.NonOwnerUser, group));
        }

        #endregion

    }
}
