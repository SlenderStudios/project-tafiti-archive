using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for TagTests
    /// </summary>
    [TestClass]
    public class TagTests
    {
        public TagTests()
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
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup() 
        {
            Utilities.SwitchToOwnerUser();
            User user = UserManager.LoggedInUser;

            foreach (Event eventItem in EventManager.GetAllEvents())
            {
                eventItem.Delete();
            }

            foreach (Group group in GroupManager.GetGroupsForUser(user.UserName, Constants.Strings.GroupType, UserGroupStatus.Joined))
            {
                group.Delete();
            }

            foreach (Album album in AlbumManager.GetAllAlbums())
            {
                album.Delete();
            }
        }

        #region TagEvent tests
        [TestMethod]
        public void OwnerTagEvent()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            eventItem.AddTag("tag");
            Assert.AreEqual(1, eventItem.Tags.Count);
            Assert.AreEqual("tag", eventItem.Tags[0].Name);
            eventItem.RemoveTag("tag");
            Assert.AreEqual(0, eventItem.Tags.Count);
            eventItem.Delete();
        }
        
        [TestMethod]
        public void AdminTagEvent()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Utilities.SwitchToAdminUser();
            eventItem.AddTag("tag");
            Assert.AreEqual(1, eventItem.Tags.Count);
            Assert.AreEqual("tag", eventItem.Tags[0].Name);
            eventItem.RemoveTag("tag");
            Assert.AreEqual(0, eventItem.Tags.Count);
            eventItem.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerTagEvent()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Utilities.SwitchToNonOwnerUser();
            eventItem.AddTag("tag");
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousTagEvent()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Utilities.SwitchToAnonymousUser();
            eventItem.AddTag("tag");
        }

        #endregion

        #region TagGroup tests
        [TestMethod]
        public void OwnerTagGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            group.AddTag("tag");
            Assert.AreEqual(1, group.Tags.Count);
            Assert.AreEqual("tag", group.Tags[0].Name);
            group.RemoveTag("tag");
            Assert.AreEqual(0, group.Tags.Count);
            group.Delete();
        }

        [TestMethod]
        public void AdminTagGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToAdminUser();
            group.AddTag("tag");
            Assert.AreEqual(1, group.Tags.Count);
            Assert.AreEqual("tag", group.Tags[0].Name);
            group.RemoveTag("tag");
            Assert.AreEqual(0, group.Tags.Count);
            group.Delete();            
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerTagGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToNonOwnerUser();
            group.AddTag("tag");   
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousTagGroup()
        {
            Utilities.SwitchToOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToAnonymousUser();
            group.AddTag("tag");   
        }

        #endregion

        #region TagMedia tests
        [TestMethod]
        public void OwnerTagMedia()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            media.AddTag("tag");
            Assert.AreEqual(1, media.Tags.Count);
            Assert.AreEqual("tag", media.Tags[0].Name);
            media.RemoveTag("tag");
            Assert.AreEqual(0, media.Tags.Count);
            album.Delete();
        }

        [TestMethod]
        public void AdminTagMedia()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Utilities.SwitchToAdminUser();
            media.AddTag("tag");
            Assert.AreEqual(1, media.Tags.Count);
            Assert.AreEqual("tag", media.Tags[0].Name);
            media.RemoveTag("tag");
            Assert.AreEqual(0, media.Tags.Count);
            album.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerTagMedia()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Utilities.SwitchToNonOwnerUser();
            media.AddTag("tag");
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousTagMedia()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Utilities.SwitchToAnonymousUser();
            media.AddTag("tag");   
        }


        #endregion

        #region TagUser tests
        [TestMethod]
        public void OwnerTagUser()
        {
            Utilities.SwitchToOwnerUser();
            User user = UserManager.LoggedInUser;
            user.RemoveTag("tag");
            user.AddTag("tag");
            Assert.IsTrue(user.Tags.Contains(TagManager.GetTagByTagName("tag")));
            user.RemoveTag("tag");
            Assert.IsFalse(user.Tags.Contains(TagManager.GetTagByTagName("tag")));
        }

        [TestMethod]
        public void AdminTagUser()
        {
            Utilities.SwitchToOwnerUser();
            User user = UserManager.LoggedInUser;
            user.RemoveTag("tag");
            Utilities.SwitchToAdminUser();
            user.AddTag("tag");
            Assert.IsTrue(user.Tags.Contains(TagManager.GetTagByTagName("tag")));
            user.RemoveTag("tag");
            Assert.IsFalse(user.Tags.Contains(TagManager.GetTagByTagName("tag")));
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerTagUser()
        {
            Utilities.SwitchToOwnerUser();
            User user = UserManager.LoggedInUser;
            Utilities.SwitchToNonOwnerUser();
            user.AddTag("tag");   
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousTagUser()
        {
            Utilities.SwitchToOwnerUser();
            User user = UserManager.LoggedInUser;
            Utilities.SwitchToAnonymousUser();
            user.AddTag("tag");     
        }

        #endregion


        [TestMethod]
        public void AddDuplicateTag()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Utilities.SwitchToAdminUser();
            eventItem.AddTag("tag");
            eventItem.AddTag("tag");
            Assert.AreEqual(1, eventItem.Tags.Count);
            eventItem.Delete();
        }

    }
}
