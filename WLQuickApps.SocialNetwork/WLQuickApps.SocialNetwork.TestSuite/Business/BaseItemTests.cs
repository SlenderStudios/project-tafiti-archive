using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for BaseItemTests
    /// </summary>
    [TestClass]
    public class BaseItemTests
    {
        public BaseItemTests()
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
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            Utilities.DeleteAllAlbumsForTestUsers();

            Utilities.DeleteAllEventsForTestUsers();
        }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            Utilities.DeleteAllAlbumsForTestUsers();

            Utilities.DeleteAllEventsForTestUsers();
        }


        #region AssociateWithBaseItem
        [TestMethod]
        public void AssociateWithBaseItem()
        {
            Utilities.SwitchToOwnerUser();
            Album albumItem = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Event eventItemPublic = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Group groupItemPublic = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);

            eventItemPublic.Associate(albumItem);
            Assert.IsTrue(eventItemPublic.Albums.Contains(albumItem));
            eventItemPublic.Associate(albumItem);
            Assert.IsTrue(eventItemPublic.Albums.Contains(albumItem));
            eventItemPublic.Associate(albumItem);
            Assert.IsTrue(eventItemPublic.Albums.Contains(albumItem));

            groupItemPublic.Associate(albumItem);            
            Assert.IsTrue(groupItemPublic.Albums.Contains(albumItem));
            groupItemPublic.Associate(albumItem);
            Assert.IsTrue(groupItemPublic.Albums.Contains(albumItem));
            groupItemPublic.Associate(albumItem);
            Assert.IsTrue(groupItemPublic.Albums.Contains(albumItem));

            eventItemPublic.Delete();
            Assert.IsFalse(eventItemPublic.Albums.Contains(albumItem));

            groupItemPublic.Delete();
            Assert.IsFalse(groupItemPublic.Albums.Contains(albumItem));
        }

        [TestMethod]
        public void RemoveAssociationFromBaseItem()
        {
            Utilities.SwitchToOwnerUser();
            Album albumItem = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Event eventItemPublic = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Group groupItemPublic = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);

            eventItemPublic.Associate(albumItem);
            Assert.IsTrue(eventItemPublic.Albums.Contains(albumItem));

            groupItemPublic.Associate(albumItem);
            Assert.IsTrue(groupItemPublic.Albums.Contains(albumItem));

            eventItemPublic.RemoveBaseItemAssociation(albumItem, true);
            Assert.IsFalse(eventItemPublic.Albums.Contains(albumItem));

            groupItemPublic.RemoveBaseItemAssociation(albumItem, true);
            Assert.IsFalse(groupItemPublic.Albums.Contains(albumItem));


        }
        #endregion

        #region CanContributeToBaseItem
        [TestMethod]
        public void CanContributeToBaseItem()
        {
            Utilities.SwitchToOwnerUser();
            Album albumItem = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Event eventItemInvisible = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Invisible, Constants.Strings.EventType);
            Event eventItemPrivate = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Private, Constants.Strings.EventType);
            Event eventItemPublic = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Group groupItemInvisible = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Invisible);
            Group groupItemPrivate = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Private);
            Group groupItemPublic = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Media mediaItem = MediaManager.CreatePicture(Utilities.TestPictureBits, albumItem.BaseItemID, "Test Picture", "");

            Assert.IsTrue(eventItemInvisible.CanContribute);
            Assert.IsTrue(eventItemPrivate.CanContribute);
            Assert.IsTrue(eventItemPublic.CanContribute);
            Assert.IsTrue(groupItemInvisible.CanContribute);
            Assert.IsTrue(groupItemPrivate.CanContribute);
            Assert.IsTrue(groupItemPublic.CanContribute);
            Assert.IsTrue(mediaItem.CanContribute);

            Utilities.SwitchToNonOwnerUser();
            Assert.IsFalse(eventItemInvisible.CanContribute);
            Assert.IsFalse(eventItemPrivate.CanContribute);
            Assert.IsTrue(eventItemPublic.CanContribute);
            Assert.IsFalse(groupItemInvisible.CanContribute);
            Assert.IsFalse(groupItemPrivate.CanContribute);
            Assert.IsTrue(groupItemPublic.CanContribute);
            Assert.IsTrue(mediaItem.CanContribute);
            User nonOwner = Utilities.NonOwnerUser;

            Utilities.SwitchToOwnerUser();
            GroupManager.AddUserToGroup(nonOwner, eventItemInvisible);
            GroupManager.AddUserToGroup(nonOwner, eventItemPrivate);
            GroupManager.AddUserToGroup(nonOwner, eventItemPublic);
            GroupManager.AddUserToGroup(nonOwner, groupItemInvisible);
            GroupManager.AddUserToGroup(nonOwner, groupItemPrivate);
            GroupManager.AddUserToGroup(nonOwner, groupItemPublic);

            Utilities.SwitchToNonOwnerUser();
            Assert.IsFalse(eventItemInvisible.CanContribute);
            Assert.IsFalse(eventItemPrivate.CanContribute);
            Assert.IsTrue(eventItemPublic.CanContribute);
            Assert.IsFalse(groupItemInvisible.CanContribute);
            Assert.IsFalse(groupItemPrivate.CanContribute);
            Assert.IsTrue(groupItemPublic.CanContribute);
            Assert.IsTrue(mediaItem.CanContribute);

            eventItemInvisible.Join();
            eventItemPrivate.Join();
            eventItemPublic.Join();
            groupItemInvisible.Join();
            groupItemPrivate.Join();
            groupItemPublic.Join();
            Assert.IsTrue(eventItemInvisible.CanContribute);
            Assert.IsTrue(eventItemPrivate.CanContribute);
            Assert.IsTrue(eventItemPublic.CanContribute);
            Assert.IsTrue(groupItemInvisible.CanContribute);
            Assert.IsTrue(groupItemPrivate.CanContribute);
            Assert.IsTrue(groupItemPublic.CanContribute);
            Assert.IsTrue(mediaItem.CanContribute);

            Utilities.SwitchToAdminUser();
            Assert.IsTrue(eventItemInvisible.CanContribute);
            Assert.IsTrue(eventItemPrivate.CanContribute);
            Assert.IsTrue(eventItemPublic.CanContribute);
            Assert.IsTrue(groupItemInvisible.CanContribute);
            Assert.IsTrue(groupItemPrivate.CanContribute);
            Assert.IsTrue(groupItemPublic.CanContribute);
            Assert.IsTrue(mediaItem.CanContribute);
        }

        #endregion
        
        #region CanModifyBaseItem
        [TestMethod]
        public void CanModifyBaseItem()
        {
            Utilities.SwitchToOwnerUser();
            Album albumItem = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Event eventItemInvisible = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Invisible, Constants.Strings.EventType);
            Event eventItemPrivate = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Private, Constants.Strings.EventType);
            Event eventItemPublic = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Group groupItemInvisible = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Invisible);
            Group groupItemPrivate = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Private);
            Group groupItemPublic = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Media mediaItem = MediaManager.CreatePicture(Utilities.TestPictureBits, albumItem.BaseItemID, "Test Picture", "");

            Assert.IsTrue(eventItemInvisible.CanEdit);
            Assert.IsTrue(eventItemPrivate.CanEdit);
            Assert.IsTrue(eventItemPublic.CanEdit);
            Assert.IsTrue(groupItemInvisible.CanEdit);
            Assert.IsTrue(groupItemPrivate.CanEdit);
            Assert.IsTrue(groupItemPublic.CanEdit);
            Assert.IsTrue(mediaItem.CanEdit);

            Utilities.SwitchToNonOwnerUser();
            Assert.IsFalse(eventItemInvisible.CanEdit);
            Assert.IsFalse(eventItemPrivate.CanEdit);
            Assert.IsFalse(eventItemPublic.CanEdit);
            Assert.IsFalse(groupItemInvisible.CanEdit);
            Assert.IsFalse(groupItemPrivate.CanEdit);
            Assert.IsFalse(groupItemPublic.CanEdit);
            Assert.IsFalse(mediaItem.CanEdit);

            Utilities.SwitchToAdminUser();
            Assert.IsTrue(eventItemInvisible.CanEdit);
            Assert.IsTrue(eventItemPrivate.CanEdit);
            Assert.IsTrue(eventItemPublic.CanEdit);
            Assert.IsTrue(groupItemInvisible.CanEdit);
            Assert.IsTrue(groupItemPrivate.CanEdit);
            Assert.IsTrue(groupItemPublic.CanEdit);
            Assert.IsTrue(mediaItem.CanEdit);
        }

        #endregion

        #region GetThumbnail
        [TestMethod]
        public void GetThumbnail()
        {
            Utilities.SwitchToOwnerUser();
            Album albumItem = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Group groupItem = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Media mediaItem = MediaManager.CreatePicture(Utilities.TestPictureBits, albumItem.BaseItemID, "Test Picture", "");

            eventItem.SetThumbnail(Utilities.TestPictureBits2);
            groupItem.SetThumbnail(Utilities.TestPictureBits2);
            mediaItem.SetThumbnail(Utilities.TestPictureBits2);

            Assert.IsNotNull(eventItem.GetThumbnail(1024, 1024));
            Assert.IsNotNull(groupItem.GetThumbnail(1024, 1024));
            Assert.IsNotNull(mediaItem.GetThumbnail(1024, 1024));
        }
        #endregion


        #region HasThumbnail()
        [TestMethod]
        public void HasThumbnail()
        {
            Utilities.SwitchToOwnerUser();
            Album albumItem = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Group groupItem = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Media mediaItem = MediaManager.CreatePicture(Utilities.TestPictureBits, albumItem.BaseItemID, "Test Picture", "");

            Assert.IsFalse(eventItem.HasThumbnail);
            Assert.IsFalse(groupItem.HasThumbnail);
            //Assert.IsFalse(mediaItem.HasThumbnail);

            eventItem.SetThumbnail(Utilities.TestPictureBits2);
            groupItem.SetThumbnail(Utilities.TestPictureBits2);
            mediaItem.SetThumbnail(Utilities.TestPictureBits2);

            Assert.IsTrue(eventItem.HasThumbnail);
            Assert.IsTrue(groupItem.HasThumbnail);
            Assert.IsTrue(mediaItem.HasThumbnail);
        }
        #endregion

        #region RateBaseItem
        [TestMethod]
        public void RateBaseItem()
        {
            Utilities.SwitchToOwnerUser();

            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);

            Assert.AreEqual(0, eventItem.TotalRatingCount);
            Assert.AreEqual(0.0, eventItem.TotalRatingScore);
            Assert.AreEqual(0.0, eventItem.AverageRating);
            eventItem.Rate(0);
            Assert.AreEqual(1, eventItem.TotalRatingCount);
            Assert.AreEqual(0.0, eventItem.TotalRatingScore);
            Assert.AreEqual(0.0, eventItem.AverageRating);
            eventItem.Rate(5);
            Assert.AreEqual(1, eventItem.TotalRatingCount);
            Assert.AreEqual(5.0, eventItem.TotalRatingScore);
            Assert.AreEqual(5.0, eventItem.AverageRating);

            Utilities.SwitchToNonOwnerUser();

            eventItem.Rate(0);
            Assert.AreEqual(2, eventItem.TotalRatingCount);
            Assert.AreEqual(5.0, eventItem.TotalRatingScore);
            Assert.AreEqual(2.5, eventItem.AverageRating);
            eventItem.Rate(5);
            Assert.AreEqual(2, eventItem.TotalRatingCount);
            Assert.AreEqual(10.0, eventItem.TotalRatingScore);
            Assert.AreEqual(5.0, eventItem.AverageRating);

            Utilities.SwitchToAdminUser();

            eventItem.Rate(4);
            Assert.AreEqual(3, eventItem.TotalRatingCount);
            Assert.AreEqual(14.0, eventItem.TotalRatingScore);
            Assert.AreEqual(4.7, Math.Round(eventItem.AverageRating, 1));
        }
        #endregion

        #region BaseItem.CanXXX

        [TestMethod]
        public void BaseItemCanView()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItemPublic = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Event eventItemPrivate = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Private, Constants.Strings.EventType);
            Event eventItemInvisible = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Invisible, Constants.Strings.EventType);

            Assert.IsTrue(eventItemPublic.CanView);
            Assert.IsTrue(eventItemPrivate.CanView);
            Assert.IsTrue(eventItemInvisible.CanView);

            Utilities.SwitchToAdminUser();
            Assert.IsTrue(eventItemPublic.CanView);
            Assert.IsTrue(eventItemPrivate.CanView);
            Assert.IsTrue(eventItemInvisible.CanView);

            Utilities.SwitchToNonOwnerUser();
            Assert.IsTrue(eventItemPublic.CanView);
            Assert.IsTrue(eventItemPrivate.CanView);
            Assert.IsFalse(eventItemInvisible.CanView);

            Utilities.SwitchToAnonymousUser();
            Assert.IsTrue(eventItemPublic.CanView);
            Assert.IsTrue(eventItemPrivate.CanView);
            Assert.IsFalse(eventItemInvisible.CanView);

            Utilities.SwitchToOwnerUser();
            eventItemPublic.Delete();
            eventItemPrivate.Delete();
            eventItemInvisible.Delete();
        }

        [TestMethod]
        public void BaseItemCanXXX()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);

            Assert.IsFalse(eventItem.CanAccept);
            Assert.IsFalse(eventItem.CanApprove);
            Assert.IsTrue(eventItem.CanAssociate);
            Assert.IsFalse(eventItem.CanCancel);
            Assert.IsTrue(eventItem.CanContribute);
            Assert.IsTrue(eventItem.CanDelete);
            Assert.IsTrue(eventItem.CanEdit);
            Assert.IsFalse(eventItem.CanJoin);
            Assert.IsFalse(eventItem.CanLeave);
            Assert.IsTrue(eventItem.CanView);

            Utilities.SwitchToAdminUser();
            Assert.IsFalse(eventItem.CanAccept);
            Assert.IsFalse(eventItem.CanApprove);
            Assert.IsTrue(eventItem.CanAssociate);
            Assert.IsFalse(eventItem.CanCancel);
            Assert.IsTrue(eventItem.CanContribute);
            Assert.IsTrue(eventItem.CanDelete);
            Assert.IsTrue(eventItem.CanEdit);
            Assert.IsTrue(eventItem.CanJoin);
            Assert.IsFalse(eventItem.CanLeave);
            Assert.IsTrue(eventItem.CanView);

            Utilities.SwitchToNonOwnerUser();
            Assert.IsFalse(eventItem.CanAccept);
            Assert.IsFalse(eventItem.CanApprove);
            Assert.IsFalse(eventItem.CanAssociate);
            Assert.IsFalse(eventItem.CanCancel);
            Assert.IsTrue(eventItem.CanContribute);
            Assert.IsFalse(eventItem.CanDelete);
            Assert.IsFalse(eventItem.CanEdit);
            Assert.IsTrue(eventItem.CanJoin);
            Assert.IsFalse(eventItem.CanLeave);
            Assert.IsTrue(eventItem.CanView);

            Utilities.SwitchToAnonymousUser();
            Assert.IsFalse(eventItem.CanAccept);
            Assert.IsFalse(eventItem.CanApprove);
            Assert.IsFalse(eventItem.CanAssociate);
            Assert.IsFalse(eventItem.CanCancel);
            Assert.IsFalse(eventItem.CanContribute);
            Assert.IsFalse(eventItem.CanDelete);
            Assert.IsFalse(eventItem.CanEdit);
            Assert.IsFalse(eventItem.CanJoin);
            Assert.IsFalse(eventItem.CanLeave);
            Assert.IsTrue(eventItem.CanView);

            Utilities.SwitchToOwnerUser();
            eventItem.Delete();
        }

        #endregion

        [TestMethod]
        public void BaseItemOwnerAssociate()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(0, eventItem.Albums.Count);
            Album album = AlbumManager.CreateAlbum("Test Album");
            eventItem.Associate(album);
            Assert.AreEqual(1, eventItem.Albums.Count);
            eventItem.RemoveBaseItemAssociation(album, true);
            Assert.AreEqual(0, eventItem.Albums.Count);
            album.Delete();
            eventItem.Delete();
        }

        [TestMethod]
        public void BaseItemAdminAssociate()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(0, eventItem.Albums.Count);
            Utilities.SwitchToNonOwnerUser();
            Album album = AlbumManager.CreateAlbum("Test Album");
            Utilities.SwitchToAdminUser();
            eventItem.Associate(album);
            Assert.AreEqual(1, eventItem.Albums.Count);
            album.Delete();
            Assert.AreEqual(0, eventItem.Albums.Count);
            eventItem.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void BaseItemNonMemberAssociate()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(0, eventItem.Albums.Count);
            Utilities.SwitchToNonOwnerUser();
            Album album = AlbumManager.CreateAlbum("Test Album");
            eventItem.Associate(album);
        }


        [TestMethod]
        public void BaseItemAssociateDuplicateAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(0, eventItem.Albums.Count);
            Album album = AlbumManager.CreateAlbum("Test Album");
            eventItem.Associate(album);
            Assert.AreEqual(1, eventItem.Albums.Count);
            eventItem.Associate(album);
            Assert.AreEqual(1, eventItem.Albums.Count);
            album.Delete();
            Assert.AreEqual(0, eventItem.Albums.Count);
            eventItem.Delete();
        }

        [TestMethod]
        public void BaseItemOwnerDeleteAssociatedAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(0, eventItem.Albums.Count);
            Album album = AlbumManager.CreateAlbum("Test Album");
            eventItem.Associate(album);
            Assert.AreEqual(1, eventItem.Albums.Count);
            album.Delete();
            Assert.AreEqual(0, eventItem.Albums.Count);
            eventItem.Delete();
        }

        [TestMethod]
        public void BaseItemOwnerRemoveNonOwnerAssociatedBaseItem()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(0, eventItem.Albums.Count);
            Utilities.SwitchToNonOwnerUser();
            Album album = AlbumManager.CreateAlbum("Test Album");
            eventItem.Join();
            eventItem.Associate(album);
            Assert.AreEqual(1, eventItem.Albums.Count);
            Utilities.SwitchToOwnerUser();
            eventItem.RemoveBaseItemAssociation(album, false);
            Assert.AreEqual(0, eventItem.Albums.Count);
            Utilities.SwitchToAdminUser();
            album.Delete();
            eventItem.Delete();
        }
        
        [TestMethod]
        public void BaseItemNonOwnerRemoveOwnedAssociatedBaseItem()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);  
            Assert.AreEqual(0, eventItem.Albums.Count);
            Utilities.SwitchToNonOwnerUser();
            Album album = AlbumManager.CreateAlbum("Test Album");
            eventItem.Join();
            eventItem.Associate(album);
            Assert.AreEqual(1, eventItem.Albums.Count);
            eventItem.RemoveBaseItemAssociation(album, true);
            Assert.AreEqual(0, eventItem.Albums.Count);
            Utilities.SwitchToAdminUser();
            album.Delete();
            eventItem.Delete();
        }

        [TestMethod]
        public void GetAssociatedBaseItems()
        {
            Utilities.SwitchToOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Assert.AreEqual(0, eventItem.Albums.Count);
            Album album = AlbumManager.CreateAlbum("Test Album");
            eventItem.Associate(album);
            
            List<BaseItem> associatedBaseItems = BaseItemManager.GetAssociatedBaseItemsForBaseItem(eventItem);
            Assert.AreEqual(album,(Album)associatedBaseItems[0]);

            List<BaseItem> baseItems = BaseItemManager.GetBaseItemsAssociatedWithBaseItem(album);
            Assert.AreEqual(eventItem, (Event)baseItems[0]);
            
            album.Delete();
            eventItem.Delete();    
        }

        [TestMethod]
        public void BaseItemGetUnapprovedItems()
        {
            Utilities.SwitchToAdminUser();
            int count = BaseItemManager.GetUnapprovedBaseItemsCount();
            SearchResults searchResults = BaseItemManager.GetUnapprovedBaseItems();
        }
    }
}
