using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for AlbumTests
    /// </summary>
    [TestClass]
    public class AlbumTests
    {
        public AlbumTests()
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

        [TestCleanup()]
        public void MyTestCleanup()
        {
            Utilities.DeleteAllAlbumsForTestUsers();
        }


        [TestInitialize()]
        public void MyTestInitialize()
        {
            Utilities.DeleteAllAlbumsForTestUsers();
        }

        #region AlbumExists
        [TestMethod]
        public void AlbumExists_True()
        {
            Utilities.SwitchToOwnerUser();

            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Assert.IsTrue(AlbumManager.AlbumExists(newAlbum.BaseItemID));
        }

        [TestMethod]
        public void AlbumExists_False()
        {
            Utilities.SwitchToOwnerUser();

            Assert.IsFalse(AlbumManager.AlbumExists(0));
        }

        #endregion

        #region CanModifyAlbum
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CanModifyAlbum_InvalidAlbumId()
        {
            Utilities.SwitchToOwnerUser(); ;

            AlbumManager.CanModifyAlbum(0);
        }

        [TestMethod]
        public void CanModifyAlbum_UserForUser()
        {
            Utilities.SwitchToOwnerUser();

            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            Assert.IsTrue(AlbumManager.CanModifyAlbum(newAlbum.BaseItemID));
        }

        [TestMethod]
        public void CanModifyAlbum_NonOwnerUserForUser()
        {
            Utilities.SwitchToOwnerUser();

            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            Utilities.SwitchToNonOwnerUser();

            Assert.IsFalse(AlbumManager.CanModifyAlbum(newAlbum.BaseItemID));
        }

        [TestMethod]
        public void CanModifyAlbum_AdminForUser()
        {
            Utilities.SwitchToOwnerUser();

            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            Utilities.SwitchToAdminUser();

            Assert.IsTrue(AlbumManager.CanModifyAlbum(newAlbum.BaseItemID));
        }

        [TestMethod]
        public void CanModifyAlbum_AnonForUser()
        {
            Utilities.SwitchToOwnerUser();

            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            Utilities.SwitchToAnonymousUser();

            Assert.IsFalse(AlbumManager.CanModifyAlbum(newAlbum.BaseItemID));
        }

        #endregion

        #region CreateAlbum
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateAlbum_NullName()
        {
            Utilities.SwitchToOwnerUser();

            AlbumManager.CreateAlbum(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateAlbum_EmptyName()
        {
            Utilities.SwitchToOwnerUser();

            AlbumManager.CreateAlbum(string.Empty);
        }

        [TestMethod]
        public void CreateAlbum_ForUser()
        {
            Utilities.SwitchToOwnerUser();

            int albumCntBefore = AlbumManager.GetAlbumsByUserIDCount(Utilities.OwnerUser.UserID);
            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            int albumCntAfter = AlbumManager.GetAlbumsByUserIDCount(Utilities.OwnerUser.UserID);

            Assert.IsTrue(albumCntAfter == albumCntBefore + 1);

            Assert.AreEqual(Constants.Strings.AlbumName, newAlbum.Title);
            Assert.AreEqual(Utilities.OwnerUser.UserID, newAlbum.Owner.UserID);

            Album createdAlbum = AlbumManager.GetAlbum(newAlbum.BaseItemID);
            Assert.AreEqual(Constants.Strings.AlbumName, createdAlbum.Title);
            Assert.AreEqual(Utilities.OwnerUser.UserID, createdAlbum.Owner.UserID);
        }
        #endregion

        #region DeleteAlbum

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void DeleteAlbum_NonOwnerUser()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Utilities.SwitchToNonOwnerUser();
            album.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void DeleteAlbum_Anon()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Utilities.SwitchToAnonymousUser();
            album.Delete();
        }

        [TestMethod]
        public void DeleteAlbum_User()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            album.Delete();
            Assert.IsFalse(AlbumManager.AlbumExists(album.BaseItemID));
        }

        [TestMethod]
        public void DeleteAlbum_WithMultipleMedia()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            album.Delete();
            Assert.IsFalse(AlbumManager.AlbumExists(album.BaseItemID));
            Assert.IsFalse(MediaManager.MediaExists(video.BaseItemID));
            Assert.IsFalse(MediaManager.MediaExists(file.BaseItemID));
        }

        [TestMethod]
        public void DeleteAlbum_Admin()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            
            Utilities.SwitchToAdminUser();
            album.Delete();
            Assert.IsFalse(AlbumManager.AlbumExists(album.BaseItemID));
        }
        #endregion

        #region GetAlbum
        [TestMethod]
        public void GetAlbum()
        {
            Utilities.SwitchToOwnerUser();

            Album expected = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Album actual = AlbumManager.GetAlbum(expected.BaseItemID);

            Assert.AreEqual(expected.BaseItemID, actual.BaseItemID);
            Assert.AreEqual(expected.Title, actual.Title);
        }

        [TestMethod]
        public void GetAlbumFromBaseItemManager()
        {
            Utilities.SwitchToOwnerUser();

            Album expected = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            BaseItem actual = BaseItemManager.GetBaseItem(expected.BaseItemID);

            Assert.IsInstanceOfType(actual, typeof(Album));
            Assert.AreEqual(expected.BaseItemID, actual.BaseItemID);
            Assert.AreEqual(expected.Title, actual.Title);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAlbum_InvalidAlbumId()
        {
            Utilities.SwitchToOwnerUser();

            Album actual = AlbumManager.GetAlbum(0);
        }
        #endregion

        #region GetAlbumsByUserIDCount
        [TestMethod]
        public void GetAlbumsByUserIDCount()
        {
            Utilities.SwitchToOwnerUser();

            int beforeCnt = AlbumManager.GetAlbumsByUserIDCount(Utilities.OwnerUser.UserID);
            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            int afterCnt = AlbumManager.GetAlbumsByUserIDCount(Utilities.OwnerUser.UserID);

            Assert.IsTrue(afterCnt == beforeCnt + 1);

            beforeCnt = afterCnt;
            newAlbum.Delete();
            afterCnt = AlbumManager.GetAlbumsByUserIDCount(Utilities.OwnerUser.UserID);

            Assert.IsTrue(afterCnt == (beforeCnt - 1));
        }

        [TestMethod]
        public void GetAlbumsByUserIDCount_InvalidUserId()
        {
            int cnt = AlbumManager.GetAlbumsByUserIDCount(new Guid("11111111111111111111111111111111"));
            Assert.AreEqual(0, cnt);
        }
        #endregion

        #region GetAlbumsByBaseItemID
        [TestMethod]
        public void GetAlbumsByBaseItemID()
        {
            Utilities.SwitchToOwnerUser();

            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Album newAlbum2 = AlbumManager.CreateAlbum("New Album Name");
            Media newPic = MediaManager.CreatePicture(Utilities.TestPictureBits, newAlbum.BaseItemID, Constants.Strings.AlbumName, string.Empty);

            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            
            List<Album> albums = AlbumManager.GetAlbumsByBaseItemID(group.BaseItemID);
            Assert.AreEqual(0, albums.Count);

            group.Associate(newAlbum);
            albums = AlbumManager.GetAlbumsByBaseItemID(group.BaseItemID);
            Assert.AreEqual(1, albums.Count);
            Assert.AreEqual(newAlbum.BaseItemID, albums[0].BaseItemID);

            group.Associate(newAlbum2);
            albums = AlbumManager.GetAlbumsByBaseItemID(group.BaseItemID);
            Assert.AreEqual(2, albums.Count);
            Assert.AreEqual(2, group.Albums.Count);
            Assert.AreEqual(newAlbum, albums[0]);
            Assert.AreEqual(newAlbum2, albums[1]);
        }

        [TestMethod]
        public void GetAlbumsByBaseItemID_InvalidBaseItemId()
        {
            Utilities.SwitchToOwnerUser();

            List<Album> albums = AlbumManager.GetAlbumsByBaseItemID(0);
            Assert.AreEqual(0, albums.Count);
        }
        #endregion

        #region GetAllAlbums
        [TestMethod]
        public void GetAllAlbums()
        {
            Utilities.SwitchToOwnerUser();

            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Album newAlbum2 = AlbumManager.CreateAlbum("New Album Name");
            Media newPic = MediaManager.CreatePicture(Utilities.TestPictureBits, newAlbum.BaseItemID, Constants.Strings.AlbumName, string.Empty);

            List<Album> albums = AlbumManager.GetAllAlbums();

            Assert.AreEqual(2, albums.Count);
            Assert.AreEqual(newAlbum, albums[0]);
            Assert.AreEqual(newAlbum2, albums[1]);
        }

        [TestMethod]
        public void GetAllAlbums_EmptySet()
        {
            Utilities.SwitchToOwnerUser();

            List<Album> albums = AlbumManager.GetAllAlbums();

            Assert.AreEqual(0, albums.Count);
        }
        #endregion

        #region GetAlbumsByUser
        [TestMethod]
        public void GetAlbumsByUser()
        {
            Utilities.SwitchToOwnerUser();
            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Album newAlbum2 = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            Utilities.SwitchToNonOwnerUser();
            Album newAlbum3 = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            List<Album> albums = AlbumManager.GetAlbumsByUserID(Utilities.OwnerUser.UserID);
            Assert.AreEqual(2, albums.Count);
            Assert.AreEqual(newAlbum, albums[0]);
            Assert.AreEqual(newAlbum2, albums[1]);

            albums = AlbumManager.GetAlbumsByUserID(Utilities.NonOwnerUser.UserID);
            Assert.AreEqual(1, albums.Count);
            Assert.AreEqual(newAlbum3, albums[0]);

            Utilities.SwitchToAdminUser();
            newAlbum.Delete();
            newAlbum2.Delete();
            newAlbum3.Delete();
        }

        [TestMethod]
        public void GetAlbumsByUser_Subset()
        {
            Utilities.SwitchToOwnerUser();

            Album newAlbum = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Album newAlbum2 = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media newPic = MediaManager.CreatePicture(Utilities.TestPictureBits, newAlbum.BaseItemID, Constants.Strings.AlbumName, string.Empty);

            List<Album> albums = AlbumManager.GetAlbumsByUserID(Utilities.OwnerUser.UserID, 1, 1);
            Assert.AreEqual(1, albums.Count);
        }

        [TestMethod]
        public void GetAlbumsByUser_InvalidUserId()
        {
            List<Album> albums = AlbumManager.GetAlbumsByUserID(new Guid("11111111111111111111111111111111"));

            Assert.AreEqual(0, albums.Count);
        }
        #endregion

        #region UpdateAlbum
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateAlbum_NullAlbum()
        {
            Utilities.SwitchToOwnerUser();

            AlbumManager.UpdateAlbum(null);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void UpdateAlbum_NonOwnerUser()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            album.Title = "New Album Name";
            Utilities.SwitchToNonOwnerUser();
            AlbumManager.UpdateAlbum(album);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void UpdateAlbum_Anon()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            album.Title = "New Album Name";
            Utilities.SwitchToAnonymousUser();
            AlbumManager.UpdateAlbum(album);
        }

        [TestMethod]
        public void UpdateAlbum_User()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            
            album.Title = "New Album Name";
            AlbumManager.UpdateAlbum(album);

            Assert.AreEqual(album.BaseItemID, AlbumManager.GetAlbum(album.BaseItemID).BaseItemID);
            Assert.AreEqual("New Album Name", AlbumManager.GetAlbum(album.BaseItemID).Title);
        }

        [TestMethod]
        public void UpdateAlbum_Admin()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            
            album.Title = "New Album Name";
            Utilities.SwitchToAdminUser();
            AlbumManager.UpdateAlbum(album);

            Assert.AreEqual(album.BaseItemID, AlbumManager.GetAlbum(album.BaseItemID).BaseItemID);
            Assert.AreEqual("New Album Name", AlbumManager.GetAlbum(album.BaseItemID).Title);
        }
        #endregion
    }
}
