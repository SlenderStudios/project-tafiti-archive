using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for PendingInviteTests
    /// </summary>
    [TestClass]
    public class PendingInviteTests
    {
        public PendingInviteTests()
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
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void CreatePendingInviteAndConfirm()
        {
            string userName = "UserName";
            string email = "pendinginvite@WLQuickApps.com";
            Utilities.SwitchToAdminUser();
            User existingUser;
            if (UserManager.TryGetUserByUserName(userName, out existingUser))
            {
                existingUser.Delete();
            }

            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);

            List<string> emails = GroupManager.GetPendingEmailInvitesForGroup(eventItem.BaseItemID);
            Assert.AreEqual(0, emails.Count);
            List<User> invitedUsers = UserManager.GetAllUsersForEvent(eventItem.BaseItemID, UserGroupStatus.Invited);
            Assert.AreEqual(0, invitedUsers.Count);

            GroupManager.CreatePendingEmailInvite(eventItem.BaseItemID, email);

            emails = GroupManager.GetPendingEmailInvitesForGroup(eventItem.BaseItemID);
            Assert.AreEqual(1, emails.Count);
            Assert.AreEqual(email, emails[0]);
            invitedUsers = UserManager.GetAllUsersForEvent(eventItem.BaseItemID, UserGroupStatus.Invited);
            Assert.AreEqual(0, invitedUsers.Count);

            GroupManager.CreatePendingEmailInvite(eventItem.BaseItemID, email);

            Utilities.SwitchToAdminUser();
            User user = UserManager.CreateUser(userName, email, "First", "Last", Gender.Unspecified, DateTime.Now.AddYears(-14), Guid.NewGuid().ToString(), string.Empty, Location.Empty, string.Empty, null, string.Empty, string.Empty, string.Empty);

            emails = GroupManager.GetPendingEmailInvitesForGroup(eventItem.BaseItemID);
            Assert.AreEqual(0, emails.Count);
            invitedUsers = UserManager.GetAllUsersForEvent(eventItem.BaseItemID, UserGroupStatus.Invited);
            Assert.AreEqual(1, invitedUsers.Count);
            Assert.AreEqual(user, invitedUsers[0]);

            eventItem.Delete();
        }

        [TestMethod]
        public void CancelPendingInvite()
        {
            string email = "pendinginvite@WLQuickApps.com";
            Utilities.SwitchToAdminUser();
            User existingUser;
            if (UserManager.TryGetUserByEmail(email, out existingUser))
            {
                existingUser.Delete();
            }

            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);

            List<string> emails = GroupManager.GetPendingEmailInvitesForGroup(eventItem.BaseItemID);
            Assert.AreEqual(0, emails.Count);
            List<User> invitedUsers = UserManager.GetAllUsersForEvent(eventItem.BaseItemID, UserGroupStatus.Invited);
            Assert.AreEqual(0, invitedUsers.Count);

            GroupManager.CreatePendingEmailInvite(eventItem.BaseItemID, email);

            emails = GroupManager.GetPendingEmailInvitesForGroup(eventItem.BaseItemID);
            Assert.AreEqual(1, emails.Count);
            Assert.AreEqual(email, emails[0]);

            GroupManager.CancelPendingEmailInvite(eventItem.BaseItemID, email);

            emails = GroupManager.GetPendingEmailInvitesForGroup(eventItem.BaseItemID);
            Assert.AreEqual(0, emails.Count);

            eventItem.Delete();
        }

    }
}
