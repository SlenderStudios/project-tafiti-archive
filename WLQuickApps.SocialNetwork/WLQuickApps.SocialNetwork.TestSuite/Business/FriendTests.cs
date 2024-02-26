using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for FriendTests
    /// </summary>
    [TestClass]
    public class FriendTests
    {
        public FriendTests()
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
        [TestInitialize]
        [TestCleanup]
        public void ClearFriends()
        {
            Utilities.DeleteAllFriendsForTestUsers();
        }

        #endregion

        [TestMethod]
        public void ConfirmFriendship()
        {
            User ownerUser = Utilities.OwnerUser;
            User adminUser = Utilities.AdminUser;

            // If it exists, remove it. This will fail queitly if it didn't exist.
            FriendManager.RemoveFriendship(ownerUser, adminUser);

            Assert.IsFalse(FriendManager.ConfirmRelationship(ownerUser, adminUser));
            Assert.IsFalse(FriendManager.LookupFriendRequest(ownerUser, adminUser));
            Assert.IsFalse(FriendManager.LookupFriendRequest(adminUser, ownerUser));
            Assert.IsFalse(FriendManager.ConfirmFriendship(adminUser, ownerUser));

            Utilities.SwitchToOwnerUser();
            FriendManager.AddFriend(adminUser);
            // Add again just to make sure that multiple AddFriend calls won't
            // incorrectly confirm a friendship (should fail silently).
            FriendManager.AddFriend(adminUser);

            Assert.IsTrue(FriendManager.ConfirmRelationship(ownerUser, adminUser));
            Assert.IsTrue(FriendManager.LookupFriendRequest(ownerUser, adminUser));
            Assert.IsFalse(FriendManager.LookupFriendRequest(adminUser, ownerUser));
            Assert.IsFalse(FriendManager.ConfirmFriendship(ownerUser, adminUser));

            Assert.IsTrue(FriendManager.GetPendingFriendRequests().Count == 1);
            Assert.AreEqual(FriendManager.GetPendingFriendRequests()[0], adminUser);
            
            Utilities.SwitchToAdminUser();
            Assert.IsTrue(FriendManager.GetPendingFriendInvitations().Count == 1);
            Assert.AreEqual(FriendManager.GetPendingFriendInvitations()[0], ownerUser);

            FriendManager.AddFriend(ownerUser);
            // Add again just to make sure that multiple AddFriend calls won't
            // incorrectly add multiple friendships (should fail silently).
            FriendManager.AddFriend(ownerUser);

            Assert.IsTrue(FriendManager.ConfirmRelationship(ownerUser, adminUser));
            Assert.IsFalse(FriendManager.LookupFriendRequest(ownerUser, adminUser));
            Assert.IsFalse(FriendManager.LookupFriendRequest(adminUser, ownerUser));
            Assert.IsTrue(FriendManager.ConfirmFriendship(ownerUser, adminUser));

            Assert.IsTrue(FriendManager.GetFriends().Count == 1);
            Assert.AreEqual(FriendManager.GetFriends()[0], ownerUser);

            Utilities.SwitchToOwnerUser();
            Assert.IsTrue(FriendManager.GetFriends().Count == 1);
            Assert.AreEqual(FriendManager.GetFriends()[0], adminUser);
        }


        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerDeleteFriendship()
        {
            User ownerUser = Utilities.OwnerUser;
            User adminUser = Utilities.AdminUser;

            Utilities.SwitchToOwnerUser();
            FriendManager.AddFriend(adminUser);
            Utilities.SwitchToAdminUser();
            FriendManager.AddFriend(ownerUser);
            Utilities.SwitchToNonOwnerUser();
            FriendManager.RemoveFriendship(ownerUser, adminUser);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousDeleteFriendship()
        {
            User ownerUser = Utilities.OwnerUser;
            User adminUser = Utilities.AdminUser;

            Utilities.SwitchToOwnerUser();
            FriendManager.AddFriend(adminUser);
            Utilities.SwitchToAdminUser();
            FriendManager.AddFriend(ownerUser);
            Utilities.SwitchToAnonymousUser();
            FriendManager.RemoveFriendship(ownerUser, adminUser);
        }

        [TestMethod]
        public void OwnerDeleteFriendship()
        {
            User ownerUser = Utilities.OwnerUser;
            User adminUser = Utilities.AdminUser;

            Utilities.SwitchToOwnerUser();
            FriendManager.AddFriend(adminUser);
            Utilities.SwitchToAdminUser();
            FriendManager.AddFriend(ownerUser);
            Utilities.SwitchToOwnerUser();
            FriendManager.RemoveFriendship(ownerUser, adminUser);
        }

        [TestMethod]
        public void AdminDeleteFriendship()
        {
            User ownerUser = Utilities.OwnerUser;
            User adminUser = Utilities.AdminUser;

            Utilities.SwitchToOwnerUser();
            FriendManager.AddFriend(adminUser);
            Utilities.SwitchToAdminUser();
            FriendManager.AddFriend(ownerUser);
            FriendManager.RemoveFriendship(ownerUser, adminUser);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OwnerAddSelfAsFriend()
        {
            Utilities.SwitchToOwnerUser();

            Assert.IsFalse(FriendManager.CanAddFriend(Utilities.OwnerUser));
            FriendManager.AddFriend(Utilities.OwnerUser);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AdminAddSelfAsFriend()
        {
            Utilities.SwitchToAdminUser();

            Assert.IsFalse(FriendManager.CanAddFriend(Utilities.AdminUser));
            FriendManager.AddFriend(Utilities.OwnerUser);
        }
    }
}
