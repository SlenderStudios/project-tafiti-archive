using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    ///// <summary>
    ///// Summary description for MediaTests
    ///// </summary>
    [TestClass]
    public class MediaTests
    {

        public MediaTests()
        {
        }

        #region Additional test attributes
        ////
        //// You can use the following additional attributes as you write your tests:
        ////
        //// Use ClassInitialize to run code before running the first test in the class
        //// [ClassInitialize()]
        //// public static void MyClassInitialize(TestContext testContext) { }
        ////
        //// Use TestInitialize to run code before running each test 
        //// [TestInitialize()]
        //// public void MyTestInitialize() { }
        ////
        //// Use TestCleanup to run code after each test has run
        //// [TestCleanup()]
        //// public void MyTestCleanup() { }
        ////
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


        #region CreateVideo tests
        [TestMethod]
        public void CreateVideo_UserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            Media actual = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            Assert.AreEqual("Test Video", actual.Title);
            Assert.AreEqual("Test Description", actual.Description);
            Assert.AreEqual(MediaType.Video, actual.MediaType);
            Assert.AreEqual(0.0, actual.AverageRating);
            Assert.AreEqual(0, actual.TotalRatingCount);
            Assert.AreEqual(0.0, actual.TotalRatingScore);
            Assert.AreEqual(0, actual.TotalViews);
            //Assert.IsNotNull(actual.GetImage());

            Assert.IsNotNull(actual.Owner);
            Assert.AreEqual(album.Owner, actual.Owner);

            Assert.IsNotNull(actual.ParentAlbum);
            Assert.AreEqual(album, actual.ParentAlbum);

            Assert.IsNotNull(actual.Comments);
            Assert.IsNotNull(actual.CreateDate);
            Assert.IsNotNull(actual.Tags);
            Assert.IsNotNull(actual.Location);

            Assert.IsTrue(actual.BaseItemID > 0, "Invalid media ID.");
            Assert.IsTrue(actual.BaseItemID > 0, "Invalid thread ID.");
        }

        [TestMethod]
        public void CreateVideo_UserInUserAlbum2()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media actual = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description", location);


            Assert.AreEqual("Test Video", actual.Title);
            Assert.AreEqual("Test Description", actual.Description);
            Assert.AreEqual(MediaType.Video, actual.MediaType);
            Assert.AreEqual(0.0, actual.AverageRating);
            Assert.AreEqual(0, actual.TotalRatingCount);
            Assert.AreEqual(0.0, actual.TotalRatingScore);
            Assert.AreEqual(0, actual.TotalViews);
            //Assert.IsNotNull(actual.GetImage());

            Assert.IsNotNull(actual.Owner);
            Assert.AreEqual(album.Owner, actual.Owner);

            Assert.IsNotNull(actual.ParentAlbum);
            Assert.AreEqual(album, actual.ParentAlbum);

            Assert.IsNotNull(actual.Comments);
            Assert.IsNotNull(actual.CreateDate);
            Assert.IsNotNull(actual.Tags);

            Assert.IsNotNull(actual.Location);
            Assert.AreEqual(location.Address1, actual.Location.Address1);
            Assert.AreEqual(location.Address2, actual.Location.Address2);
            Assert.AreEqual(location.City, actual.Location.City);
            Assert.AreEqual(location.Country, actual.Location.Country);
            Assert.AreEqual(location, actual.Location);
            Assert.AreEqual(location.Name, actual.Location.Name);
            Assert.AreEqual(location.PostalCode, actual.Location.PostalCode);
            Assert.AreEqual(location.Region, actual.Location.Region);

            Assert.IsTrue(actual.BaseItemID > 0, "Invalid media ID.");
            Assert.IsTrue(actual.BaseItemID > 0, "Invalid thread ID.");
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void CreateVideo_NonOwnerUserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Utilities.SwitchToNonOwnerUser();

            Media actual =
                MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description", location);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void CreateVideo_AnonInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Utilities.SwitchToAnonymousUser();

            //CreateVideo call is expected to fail.
            Media actual = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description", location);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateVideo_NullFileName()
        {
            MediaManager.CreateVideo(null, Utilities.TestVideoBits, 1, "My Video", "My video description");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateVideo_EmptyFileName()
        {
            MediaManager.CreateVideo(string.Empty, Utilities.TestVideoBits, 1, "My Video", "My video description");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateVideo_NullBits()
        {
            MediaManager.CreateVideo("file.wmv", null, 1, "My Video", "My video description");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateVideo_NullName()
        {
            MediaManager.CreateVideo("file.wmv", Utilities.TestVideoBits, 1, null, "My video description");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateVideo_EmptyName()
        {
            MediaManager.CreateVideo("file.wmv", Utilities.TestVideoBits, 1, string.Empty, "My video description");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateVideo_InvalidAlbum()
        {
            MediaManager.CreateVideo("file.wmv", Utilities.TestVideoBits, 1, "My Video", "My video description");
        }
        #endregion

        #region CreatePicture tests
        ////Note: Parameter tests are considered to be covered by the CreateVideo param
        ////tests since Create* calls all go through CreateMedia.
        ////Note: Test variations that take into consideration logged in user types
        ////are also considered to be covered by the CreateVideo tests.
        [TestMethod]
        public void CreatePicture_UserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media actual = MediaManager.CreatePicture(
                Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            Assert.AreEqual("Test Picture", actual.Title);
            Assert.AreEqual("", actual.Description);
            Assert.AreEqual(MediaType.Picture, actual.MediaType);
            Assert.AreEqual(0.0, actual.AverageRating);
            Assert.AreEqual(0, actual.TotalRatingCount);
            Assert.AreEqual(0.0, actual.TotalRatingScore);
            Assert.AreEqual(0, actual.TotalViews);
            Assert.IsNotNull(actual.GetImage());

            Assert.IsNotNull(actual.Owner);
            Assert.AreEqual(album.Owner, actual.Owner);

            Assert.IsNotNull(actual.ParentAlbum);
            Assert.AreEqual(album, actual.ParentAlbum);

            Assert.IsNotNull(actual.Comments);
            Assert.IsNotNull(actual.CreateDate);
            Assert.IsNotNull(actual.Tags);

            Assert.IsNotNull(actual.Location);
            Assert.AreEqual(location.Address1, actual.Location.Address1);
            Assert.AreEqual(location.Address2, actual.Location.Address2);
            Assert.AreEqual(location.City, actual.Location.City);
            Assert.AreEqual(location.Country, actual.Location.Country);
            Assert.AreEqual(location, actual.Location);
            Assert.AreEqual(location.Name, actual.Location.Name);
            Assert.AreEqual(location.PostalCode, actual.Location.PostalCode);
            Assert.AreEqual(location.Region, actual.Location.Region);

            Assert.IsTrue(actual.BaseItemID > 0, "Invalid media ID.");
            Assert.IsTrue(actual.BaseItemID > 0, "Invalid thread ID.");
        }

        [TestMethod]
        public void CreatePicture_UserInUserAlbum_Photo()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;
            Media actual = MediaManager.CreatePicture(
                Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "Title","Desc", location, string.Empty,string.Empty);

            Assert.AreEqual("Test Picture", actual.Title);
            Assert.AreEqual("", actual.Description);
            Assert.AreEqual(MediaType.Picture, actual.MediaType);
            Assert.AreEqual(0.0, actual.AverageRating);
            Assert.AreEqual(0, actual.TotalRatingCount);
            Assert.AreEqual(0.0, actual.TotalRatingScore);
            Assert.AreEqual(0, actual.TotalViews);
            Assert.IsNotNull(actual.GetImage());

            Assert.IsNotNull(actual.Owner);
            Assert.AreEqual(album.Owner, actual.Owner);

            Assert.IsNotNull(actual.ParentAlbum);
            Assert.AreEqual(album, actual.ParentAlbum);

            Assert.IsNotNull(actual.Comments);
            Assert.IsNotNull(actual.CreateDate);
            Assert.IsNotNull(actual.Tags);

            Assert.IsNotNull(actual.Location);
            Assert.AreEqual(location.Address1, actual.Location.Address1);
            Assert.AreEqual(location.Address2, actual.Location.Address2);
            Assert.AreEqual(location.City, actual.Location.City);
            Assert.AreEqual(location.Country, actual.Location.Country);
            Assert.AreEqual(location, actual.Location);
            Assert.AreEqual(location.Name, actual.Location.Name);
            Assert.AreEqual(location.PostalCode, actual.Location.PostalCode);
            Assert.AreEqual(location.Region, actual.Location.Region);

            Assert.IsTrue(actual.BaseItemID > 0, "Invalid media ID.");
            Assert.IsTrue(actual.BaseItemID > 0, "Invalid thread ID.");
        }

        ////Commented Issue#2977
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePicture_InvalidPicture()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media media = MediaManager.CreatePicture(Utilities.TestPictureBitsBad, album.BaseItemID, "Test Picture", "", location);
        }
        //#endregion
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreatePicture_InvalidPicture_Photo()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;
            Media media = MediaManager.CreatePicture( Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "Title", "Desc", location, string.Empty, string.Empty);
        }
        #region CreateAudio tests
        ////Note: Parameter tests are considered to be covered by the CreateVideo
        ////tests since Create* calls all go through CreateMedia.
        [TestMethod]
        public void CreateAudio_UserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media actual = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);

            actual.SetThumbnail(Utilities.TestPictureBits);

            Assert.AreEqual("Test Audio", actual.Title);
            Assert.AreEqual("Test Description", actual.Description);
            Assert.AreEqual(MediaType.Audio, actual.MediaType);
            Assert.AreEqual(0.0, actual.AverageRating);
            Assert.AreEqual(0, actual.TotalRatingCount);
            Assert.AreEqual(0.0, actual.TotalRatingScore);
            Assert.AreEqual(0, actual.TotalViews);
            //Assert.IsNotNull(actual.GetImage());

            Assert.IsNotNull(actual.Owner);
            Assert.AreEqual(album.Owner, actual.Owner);

            Assert.IsNotNull(actual.ParentAlbum);
            Assert.AreEqual(album, actual.ParentAlbum);

            Assert.IsNotNull(actual.Comments);
            Assert.IsNotNull(actual.CreateDate);
            Assert.IsNotNull(actual.Tags);

            Assert.IsNotNull(actual.Location);
            Assert.AreEqual(location.Address1, actual.Location.Address1);
            Assert.AreEqual(location.Address2, actual.Location.Address2);
            Assert.AreEqual(location.City, actual.Location.City);
            Assert.AreEqual(location.Country, actual.Location.Country);
            Assert.AreEqual(location, actual.Location);
            Assert.AreEqual(location.Name, actual.Location.Name);
            Assert.AreEqual(location.PostalCode, actual.Location.PostalCode);
            Assert.AreEqual(location.Region, actual.Location.Region);

            Assert.IsTrue(actual.BaseItemID > 0, "Invalid media ID.");
            Assert.IsTrue(actual.BaseItemID > 0, "Invalid thread ID.");
        }
        #endregion

        #region CreateFile tests
        [TestMethod]
        public void CreateFile_UserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            Media actual = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");

            actual.SetThumbnail(Utilities.TestPictureBits);

            Assert.AreEqual("Test File", actual.Title);
            Assert.AreEqual("Test Description", actual.Description);
            Assert.AreEqual(MediaType.File, actual.MediaType);
            Assert.AreEqual(0.0, actual.AverageRating);
            Assert.AreEqual(0, actual.TotalRatingCount);
            Assert.AreEqual(0.0, actual.TotalRatingScore);
            Assert.AreEqual(0, actual.TotalViews);
            //Assert.IsNotNull(actual.GetImage());

            Assert.IsNotNull(actual.Owner);
            Assert.AreEqual(album.Owner, actual.Owner);

            Assert.IsNotNull(actual.ParentAlbum);
            Assert.AreEqual(album, actual.ParentAlbum);

            Assert.IsNotNull(actual.Comments);
            Assert.IsNotNull(actual.CreateDate);
            Assert.IsNotNull(actual.Tags);

            Assert.IsNotNull(actual.Location);

            Assert.IsTrue(actual.BaseItemID > 0, "Invalid media ID.");
            Assert.IsTrue(actual.BaseItemID > 0, "Invalid thread ID.");
        }
        #endregion

        #region DeleteMedia tests

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void DeleteMedia_NonOwnerUserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "");
            Utilities.SwitchToNonOwnerUser();
            media.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void DeleteMedia_AnonInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "");
            Utilities.SwitchToAnonymousUser();
            media.Delete();
        }

        [TestMethod]
        public void DeleteMedia_UserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "");

            media.Delete();

            Assert.IsFalse(MediaManager.MediaExists(media.BaseItemID));

            album.Delete();
        }

        [TestMethod]
        public void DeleteMedia_AdminInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "");

            Utilities.SwitchToAdminUser();
            media.Delete();

            Assert.IsFalse(MediaManager.MediaExists(media.BaseItemID));

            album.Delete();
        }

        [TestMethod]
        public void DeleteMedia_AdminInUserAlbum_Photo()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "Title", "Desc", Location.Empty, string.Empty, string.Empty);

            Utilities.SwitchToAdminUser();
            media.Delete();

            Assert.IsFalse(MediaManager.MediaExists(media.BaseItemID));

            album.Delete();
        }
        #endregion

        #region GetMediaByAlbumID
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMediaByAlbumID_InvalidAlbumId()
        {
            Utilities.SwitchToOwnerUser();
            List<Media> media = MediaManager.GetMediaByAlbumBaseItemID(0);
        }

        [TestMethod]
        public void GetMediaByAlbumID_EmptyAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            List<Media> media = MediaManager.GetMediaByAlbumBaseItemID(album.BaseItemID);

            Assert.IsNotNull(media);
            Assert.AreEqual(0, media.Count);
        }

        // Commented Issue#2977
        [TestMethod]
        public void GetMediaByAlbumID_UserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);
            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            List<Media> media = MediaManager.GetMediaByAlbumBaseItemID(album.BaseItemID);
            Assert.IsNotNull(media);
            Assert.AreEqual(4, media.Count);

            media = MediaManager.GetMediaOfTypeByAlbumBaseItemID(album.BaseItemID, MediaType.Audio);
            Assert.IsNotNull(media);
            Assert.AreEqual(1, media.Count);
            Assert.AreEqual(MediaType.Audio, media[0].MediaType);

            media = MediaManager.GetMediaOfTypeByAlbumBaseItemID(album.BaseItemID, MediaType.File);
            Assert.IsNotNull(media);
            Assert.AreEqual(1, media.Count);
            Assert.AreEqual(MediaType.File, media[0].MediaType);

            media = MediaManager.GetMediaOfTypeByAlbumBaseItemID(album.BaseItemID, MediaType.Picture);
            Assert.IsNotNull(media);
            Assert.AreEqual(1, media.Count);
            Assert.AreEqual(MediaType.Picture, media[0].MediaType);

            media = MediaManager.GetMediaOfTypeByAlbumBaseItemID(album.BaseItemID, MediaType.Video);
            Assert.IsNotNull(media);
            Assert.AreEqual(1, media.Count);
            Assert.AreEqual(MediaType.Video, media[0].MediaType);
        }

        #endregion

        #region GetMedia
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMedia_InvalidMediaId()
        {
            MediaManager.GetMedia(0);
        }

        ////Commented Issue#2977
        [TestMethod]
        public void GetMedia_UserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media expected = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            Media actual = MediaManager.GetMedia(expected.BaseItemID);

            Assert.AreEqual("Test Picture", actual.Title);
            Assert.AreEqual("", actual.Description);
            Assert.AreEqual(MediaType.Picture, actual.MediaType);
            Assert.AreEqual(0.0, actual.AverageRating);
            Assert.AreEqual(0, actual.TotalRatingCount);
            Assert.AreEqual(0.0, actual.TotalRatingScore);
            Assert.AreEqual(0, actual.TotalViews);

            Assert.IsNotNull(actual.GetImage());

            Assert.IsNotNull(actual.Owner);
            Assert.AreEqual(album.Owner, actual.Owner);

            Assert.IsNotNull(actual.ParentAlbum);
            Assert.AreEqual(album, actual.ParentAlbum);

            Assert.IsNotNull(actual.Comments);
            Assert.IsNotNull(actual.CreateDate);
            Assert.IsNotNull(actual.Tags);

            Assert.IsNotNull(actual.Location);
            Assert.AreEqual(location.Address1, actual.Location.Address1);
            Assert.AreEqual(location.Address2, actual.Location.Address2);
            Assert.AreEqual(location.City, actual.Location.City);
            Assert.AreEqual(location.Country, actual.Location.Country);
            Assert.AreEqual(location, actual.Location);
            Assert.AreEqual(location.Name, actual.Location.Name);
            Assert.AreEqual(location.PostalCode, actual.Location.PostalCode);
            Assert.AreEqual(location.Region, actual.Location.Region);

            Assert.IsTrue(actual.BaseItemID > 0, "Invalid media ID.");
            Assert.IsTrue(actual.BaseItemID > 0, "Invalid thread ID.");
        }
        #endregion

        #region GetMediaBits
        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaBits()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media expected = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            Assert.IsNotNull(expected.GetImage());
        }
        #endregion

        #region GetMediaByAlbum

        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaByAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);
            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            List<Media> media = MediaManager.GetMediaByAlbumBaseItemID(album.BaseItemID, 0, 4);

            Assert.AreEqual(4, media.Count);
        }

        //Commented Issue#2977
        [TestMethod]
        public void GetMediaByAlbum_Subset()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);
            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            List<Media> media = MediaManager.GetMediaByAlbumBaseItemID(album.BaseItemID, 2, 2);

            Assert.AreEqual(2, media.Count);
        }

        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaByAlbum_EmptySet()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            List<Media> media = MediaManager.GetMediaByAlbumBaseItemID(album.BaseItemID, 0, 0);

            Assert.AreEqual(0, media.Count);
        }
        //Commented Issue#2977
        [TestMethod]
        public void GetMediaByAlbum2()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);
            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            List<Media> media = MediaManager.GetMediaOfTypeByAlbumBaseItemID(album.BaseItemID, MediaType.Audio, 0, 1);
            Assert.AreEqual(1, media.Count);

            media = MediaManager.GetMediaOfTypeByAlbumBaseItemID(album.BaseItemID, MediaType.File, 0, 1);
            Assert.AreEqual(MediaType.File, media[0].MediaType);

            media = MediaManager.GetMediaOfTypeByAlbumBaseItemID(album.BaseItemID, MediaType.Picture, 0, 1);
            Assert.AreEqual(MediaType.Picture, media[0].MediaType);

            media = MediaManager.GetMediaOfTypeByAlbumBaseItemID(album.BaseItemID, MediaType.Video, 0, 1);
            Assert.AreEqual(MediaType.Video, media[0].MediaType);
        }
        #endregion

        #region GetMediaByBaseItemID
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMediaByBaseItemID_InvalidId()
        {
            Media actual = MediaManager.GetMedia(-1);
        }

        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaByBaseItemID()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            Media actual = MediaManager.GetMedia(picture.BaseItemID);

            Assert.AreEqual(picture.BaseItemID, actual.BaseItemID);
        }

        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaFromBaseItemManager()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media expected = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            BaseItem actual = BaseItemManager.GetBaseItem(expected.BaseItemID);

            Assert.IsInstanceOfType(actual, typeof(Media));
            Assert.AreEqual(expected.BaseItemID, actual.BaseItemID);
            Assert.AreEqual(expected.Title, actual.Title);
        }
        //#endregion

        #region GetMediaCountByAlbum
        [TestMethod]
        public void GetMediaCountByAlbum_Empty()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            int mediaCount = MediaManager.GetMediaCountByAlbum(album.BaseItemID);

            Assert.AreEqual(0, mediaCount);
        }

        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaCountByAlbum_FourElements()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);
            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            int mediaCount = MediaManager.GetMediaCountByAlbum(album.BaseItemID);

            Assert.AreEqual(4, mediaCount);
        }

        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaCountByAlbum2()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            int mediaCount = MediaManager.GetMediaCountByAlbum(album.BaseItemID, MediaType.Picture);

            Assert.AreEqual(1, mediaCount);
        }

        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaCountByAlbum2_MediaTypeNotInAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            int mediaCount = MediaManager.GetMediaCountByAlbum(album.BaseItemID, MediaType.Audio);

            Assert.AreEqual(0, mediaCount);
        }
        #endregion

        #region GetMediaImage

        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaImage_Picture()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            picture.SetThumbnail(Utilities.TestPictureBits);

            Assert.IsNotNull(picture.GetImage());
        }

        [TestMethod]
        public void GetMediaImage_Audio()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);
            audio.SetThumbnail(Utilities.TestPictureBits);

            Assert.IsNotNull(audio.GetThumbnail(64, 64));
        }

        [TestMethod]
        public void GetMediaImage_File()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            file.SetThumbnail(Utilities.TestPictureBits);

            Assert.IsNotNull(file.GetThumbnail(64, 64));
        }

        [TestMethod]
        public void GetMediaImage_Video()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");
            video.SetThumbnail(Utilities.TestPictureBits);

            Assert.IsNotNull(video.GetThumbnail(64, 64));
        }

        [TestMethod]
        public void GetMediaImage_AudioWithNoThumbnail()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);

            Assert.IsNull(audio.GetThumbnail(64, 64));
        }

        [TestMethod]
        public void GetMediaImage_FileWithNoThumbnail()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");

            Assert.IsNull(file.GetThumbnail(64, 64));
        }

        [TestMethod]
        public void GetMediaImage_VideoWithNoThumbnail()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            Assert.IsNull(video.GetThumbnail(64, 64));
        }

        #endregion

        #region GetMediaThumbnail
        ////Commented Issue#2977
        [TestMethod]
        public void GetMediaThumbnail()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);
            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            picture.SetThumbnail(Utilities.TestPictureBits);
            Image img = picture.GetThumbnail(64, 64);
            Assert.IsNotNull(img);

            audio.SetThumbnail(Utilities.TestPictureBits);
            img = audio.GetThumbnail(64, 64);
            Assert.IsNotNull(img);

            file.SetThumbnail(Utilities.TestPictureBits);
            img = file.GetThumbnail(64, 64);
            Assert.IsNotNull(img);

            video.SetThumbnail(Utilities.TestPictureBits);
            img = video.GetThumbnail(64, 64);
            Assert.IsNotNull(img);
        }

        ////Commented Issue#2977
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetMediaThumbnail_InvalidWidth()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            picture.SetThumbnail(Utilities.TestPictureBits);

            Image img = picture.GetThumbnail(0, 64);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMediaThumbnail_InvalidWidth2()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            picture.SetThumbnail(Utilities.TestPictureBits);

            Image img = picture.GetThumbnail(1, 64);
        }

        [TestMethod]
        public void GetMediaThumbnail_LargeWidth()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            picture.SetThumbnail(Utilities.TestPictureBits);

            Image img = picture.GetThumbnail(9999, 64);
        }
        #endregion

        #region GetMostRecentMedia
        [TestMethod]
        public void GetMostRecentMedia()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "");
            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description");
            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            List<Media> recent = MediaManager.GetMostRecentMedia(MediaType.Picture);
            Assert.IsTrue(recent.Count >= 1);
            Assert.IsTrue(recent.Contains(picture));

            recent = MediaManager.GetMostRecentMedia(MediaType.File);
            Assert.IsTrue(recent.Count >= 1);
            Assert.IsTrue(recent.Contains(file));

            recent = MediaManager.GetMostRecentMedia(MediaType.Audio);
            Assert.IsTrue(recent.Count >= 1);
            Assert.IsTrue(recent.Contains(audio));

            recent = MediaManager.GetMostRecentMedia(MediaType.Video);
            Assert.IsTrue(recent.Count >= 1);
            Assert.IsTrue(recent.Contains(video));
        }
        #endregion

        #region HasThumbnail
        [TestMethod]
        public void HasThumbnail()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);
            Media audio = MediaManager.CreateAudio("TestAudio.wma", Utilities.TestAudioBits, album.BaseItemID, "Test Audio", "Test Description", location);
            Media file = MediaManager.CreateFile("myapp.zip", Utilities.TestPictureBitsBad, album.BaseItemID, "Test File", "Test Description");
            Media video = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "Test Video", "Test Description");

            //Assert.IsFalse(picture.HasThumbnail);
            picture.SetThumbnail(Utilities.TestPictureBits);
            Assert.IsTrue(picture.HasThumbnail);

            Assert.IsFalse(audio.HasThumbnail);
            audio.SetThumbnail(Utilities.TestPictureBits);
            Assert.IsTrue(audio.HasThumbnail);

            Assert.IsFalse(file.HasThumbnail);
            file.SetThumbnail(Utilities.TestPictureBits);

            Assert.IsFalse(video.HasThumbnail);
            video.SetThumbnail(Utilities.TestPictureBits);
            Assert.IsTrue(video.HasThumbnail);
        }
        #endregion

        #region MediaExists
        [TestMethod]
        public void MediaExists()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;

            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            Assert.IsTrue(MediaManager.MediaExists(picture.BaseItemID));

            picture.Delete();

            Assert.IsFalse(MediaManager.MediaExists(picture.BaseItemID));
        }
        #endregion

        #region SetMediaThumbnail
        [TestMethod]
        public void SetMediaThumbnail_UserSetsUserMedia()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;
            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            picture.SetThumbnail(Utilities.TestPictureBits2);

            Assert.IsTrue(picture.HasThumbnail);
            Image beforeImg = Business.Utilities.ConvertBytesToImage(Utilities.TestPictureBits2);
            Image afterImg = picture.GetThumbnail(1024, 1024);

            Assert.IsNotNull(afterImg);
            Assert.IsTrue(Utilities.ImagesEqual(beforeImg, afterImg));
        }

        [TestMethod]
        public void SetMediaThumbnail_AdminSetsUserMedia()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;
            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            Utilities.SwitchToAdminUser();

            picture.SetThumbnail(Utilities.TestPictureBits2);

            Assert.IsTrue(picture.HasThumbnail);
            Image beforeImg = Business.Utilities.ConvertBytesToImage(Utilities.TestPictureBits2);
            Image afterImg = picture.GetThumbnail(1024, 1024);

            Assert.IsNotNull(afterImg);
            Assert.IsTrue(Utilities.ImagesEqual(beforeImg, afterImg));
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void SetMediaThumbnail_NonOwnerUserSetsUserMedia()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;
            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            Utilities.SwitchToNonOwnerUser();

            picture.SetThumbnail(Utilities.TestPictureBits2);
        }


        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void SetMediaThumbnail_AnonSetsUserMedia()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;
            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            Utilities.SwitchToAnonymousUser();

            picture.SetThumbnail(Utilities.TestPictureBits2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SetMediaThumbnail_InvalidImageBits()
        {
            Utilities.SwitchToOwnerUser();

            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Location location = Utilities.TestLocation;
            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Test Picture", "", location);

            picture.SetThumbnail(Utilities.TestPictureBitsBad);
        }
        #endregion

        #region UpdateMedia tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateMedia_NullParam()
        {
            MediaManager.UpdateMedia(null);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void UpdateMedia_NonOwnerUserInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "");

            media.Title = "New Picture Caption";
            media.Location = Utilities.TestLocation;

            Utilities.SwitchToNonOwnerUser();
            MediaManager.UpdateMedia(media);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void UpdateMedia_AnonInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "");

            media.Title = "New Picture Caption";
            media.Location = Utilities.TestLocation;

            Utilities.SwitchToAnonymousUser();
            MediaManager.UpdateMedia(media);
        }

        [TestMethod]
        public void UpdateMedia_AdminInUserAlbum()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "");

            media.Title = "New Picture Caption";
            media.Location = Utilities.TestLocation;

            Utilities.SwitchToAdminUser();
            MediaManager.UpdateMedia(media);

            Media updated = MediaManager.GetMedia(media.BaseItemID);

            Assert.AreEqual("New Picture Caption", updated.Title);

            Assert.IsNotNull(updated.Location);
            Assert.AreEqual(Utilities.TestLocation, updated.Location);
            Assert.AreEqual(Utilities.TestLocation.Address1, updated.Location.Address1);
            Assert.AreEqual(Utilities.TestLocation.Address2, updated.Location.Address2);
            Assert.AreEqual(Utilities.TestLocation.City, updated.Location.City);
            Assert.AreEqual(Utilities.TestLocation.Country, updated.Location.Country);
            Assert.AreEqual(Utilities.TestLocation, updated.Location);
            Assert.AreEqual(Utilities.TestLocation.Name, updated.Location.Name);
            Assert.AreEqual(Utilities.TestLocation.PostalCode, updated.Location.PostalCode);
            Assert.AreEqual(Utilities.TestLocation.Region, updated.Location.Region);
        }

        [TestMethod]
        public void UpdateMedia_UserInUserAlbum_Picture()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "");

            media.Title = "New Picture Caption";
            media.Location = Utilities.TestLocation;

            MediaManager.UpdateMedia(media);

            Media updated = MediaManager.GetMedia(media.BaseItemID);

            Assert.AreEqual("New Picture Caption", updated.Title);

            Assert.IsNotNull(updated.Location);
            Assert.AreEqual(Utilities.TestLocation, updated.Location);
            Assert.AreEqual(Utilities.TestLocation.Address1, updated.Location.Address1);
            Assert.AreEqual(Utilities.TestLocation.Address2, updated.Location.Address2);
            Assert.AreEqual(Utilities.TestLocation.City, updated.Location.City);
            Assert.AreEqual(Utilities.TestLocation.Country, updated.Location.Country);
            Assert.AreEqual(Utilities.TestLocation, updated.Location);
            Assert.AreEqual(Utilities.TestLocation.Name, updated.Location.Name);
            Assert.AreEqual(Utilities.TestLocation.PostalCode, updated.Location.PostalCode);
            Assert.AreEqual(Utilities.TestLocation.Region, updated.Location.Region);
        }

        [TestMethod]
        public void UpdateMedia_UserInUserAlbum_Video()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreateVideo("TestVideo.wmv", Utilities.TestVideoBits, album.BaseItemID, "My Video", "My video description");

            media.Title = "New Video Name";
            media.Description = "New video description";
            media.Location = Utilities.TestLocation;

            MediaManager.UpdateMedia(media);


            Media updated = MediaManager.GetMedia(media.BaseItemID);

            Assert.AreEqual("New Video Name", updated.Title);
            Assert.AreEqual("New video description", updated.Description);

            Assert.IsNotNull(updated.Location);
            Assert.AreEqual(Utilities.TestLocation, updated.Location);
            Assert.AreEqual(Utilities.TestLocation.Address1, updated.Location.Address1);
            Assert.AreEqual(Utilities.TestLocation.Address2, updated.Location.Address2);
            Assert.AreEqual(Utilities.TestLocation.City, updated.Location.City);
            Assert.AreEqual(Utilities.TestLocation.Country, updated.Location.Country);
            Assert.AreEqual(Utilities.TestLocation, updated.Location);
            Assert.AreEqual(Utilities.TestLocation.Name, updated.Location.Name);
            Assert.AreEqual(Utilities.TestLocation.PostalCode, updated.Location.PostalCode);
            Assert.AreEqual(Utilities.TestLocation.Region, updated.Location.Region);
        }
        #endregion
    }
}
        #endregion
        #endregion