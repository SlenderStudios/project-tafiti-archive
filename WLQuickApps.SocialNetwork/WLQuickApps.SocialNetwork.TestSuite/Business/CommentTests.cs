using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for CommentTests
    /// </summary>
    [TestClass]
    public class CommentTests
    {
 
        public CommentTests()
        {
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        // 
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) {}
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

       
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            List<Album> albums = new List<Album>();

            Utilities.SwitchToOwnerUser();
            foreach (Album album in AlbumManager.GetAllAlbums())
            {
                album.Delete();
            }

            Utilities.SwitchToNonOwnerUser();
            foreach (Album album in AlbumManager.GetAllAlbums())
            {
                album.Delete();
            }
        }

        [TestMethod]
        public void CommentOnMedia()
        {
            Utilities.SwitchToNonOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Utilities.SwitchToOwnerUser();
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual("My comment", comment.Text);
            Assert.AreEqual(media.BaseItemID, comment.BaseItemID);
            CommentManager.DeleteComment(comment.CommentID);
        }

        [TestMethod]
        public void CommentOnEvent()
        {
            Utilities.SwitchToNonOwnerUser();
            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            Utilities.SwitchToOwnerUser();
            Comment comment = CommentManager.CreateComment(eventItem.BaseItemID, "My comment");
            Assert.AreEqual("My comment", comment.Text);
            Assert.AreEqual(eventItem.BaseItemID, comment.BaseItemID);
            CommentManager.DeleteComment(comment.CommentID);
            Utilities.SwitchToNonOwnerUser();
            eventItem.Delete();
        }

        [TestMethod]
        public void CommentOnProfile()
        {
            Utilities.SwitchToNonOwnerUser();
            User user = UserManager.LoggedInUser;
            Utilities.SwitchToOwnerUser();
            Comment comment = CommentManager.CreateComment(user.BaseItemID, "My comment");
            Assert.AreEqual("My comment", comment.Text);
            Assert.AreEqual(user.BaseItemID, comment.BaseItemID);
            CommentManager.DeleteComment(comment.CommentID);
        }

        [TestMethod]
        public void CommentOnGroup()
        {
            Utilities.SwitchToNonOwnerUser();
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            Utilities.SwitchToOwnerUser();
            Comment comment = CommentManager.CreateComment(group.BaseItemID, "My comment");
            Assert.AreEqual("My comment", comment.Text);
            Assert.AreEqual(group.BaseItemID, comment.BaseItemID);
            CommentManager.DeleteComment(comment.CommentID);
            Utilities.SwitchToNonOwnerUser();
            group.Delete();
        }

        [TestMethod]
        public void CommentRetrievalAnyOrder()
        {
            Utilities.SwitchToNonOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");

            Utilities.SwitchToOwnerUser();
            Comment comment2 = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual(true, CommentManager.CommentExists(comment.CommentID));
            Assert.AreEqual(comment2.CommentID, CommentManager.GetComment(comment2.CommentID).CommentID);
            Assert.AreEqual(2, CommentManager.GetCommentCount(media.BaseItemID));

            List<Comment> comments = CommentManager.GetComments(media.BaseItemID);
            Assert.AreEqual(2, comments.Count);
            Assert.IsTrue(comments.Contains(comment));
            Assert.IsTrue(comments.Contains(comment2));

            Utilities.SwitchToNonOwnerUser();
            album.Delete();
        }

        [TestMethod]
        public void CommentRetrievalOldestFirst()
        {
            Utilities.SwitchToNonOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");  
            
            Utilities.SwitchToOwnerUser();
            Comment comment2 = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual(true, CommentManager.CommentExists(comment.CommentID));
            Assert.AreEqual(comment2.CommentID, CommentManager.GetComment(comment2.CommentID).CommentID);
            Assert.AreEqual(2, CommentManager.GetCommentCount(media.BaseItemID));
            
            List<Comment> comments = CommentManager.GetComments(media.BaseItemID, 0, 2, true);
            Assert.AreEqual(2, comments.Count);
            Assert.AreEqual(comment.CommentID, comments[0].CommentID);
            Assert.AreEqual(comment2.CommentID, comments[1].CommentID);
            
            Utilities.SwitchToNonOwnerUser();
            album.Delete();
        }

        [TestMethod]
        public void CommentRetrievalNewestFirst()
        {
            Utilities.SwitchToNonOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");

            Utilities.SwitchToOwnerUser();
            Comment comment2 = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual(true, CommentManager.CommentExists(comment.CommentID));
            Assert.AreEqual(comment2.CommentID, CommentManager.GetComment(comment2.CommentID).CommentID);
            Assert.AreEqual(2, CommentManager.GetCommentCount(media.BaseItemID));

            List<Comment> comments = CommentManager.GetComments(media.BaseItemID, 0, 2, false);
            Assert.AreEqual(2, comments.Count);
            Assert.AreEqual(comment.CommentID, comments[1].CommentID);
            Assert.AreEqual(comment2.CommentID, comments[0].CommentID);

            Utilities.SwitchToNonOwnerUser();
            album.Delete();
        }

        [TestMethod]
        public void CommentRetrievalPaging()
        {
            Utilities.SwitchToNonOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");

            Utilities.SwitchToOwnerUser();
            Comment comment2 = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual(true, CommentManager.CommentExists(comment.CommentID));
            Assert.AreEqual(comment2.CommentID, CommentManager.GetComment(comment2.CommentID).CommentID);
            Assert.AreEqual(2, CommentManager.GetCommentCount(media.BaseItemID));

            List<Comment> comments;
            
            comments = CommentManager.GetComments(media.BaseItemID, 0,1, true);
            Assert.AreEqual(1, comments.Count);
            Assert.AreEqual(comments[0], comment);
            
            comments = CommentManager.GetComments(media.BaseItemID, 1,1, true);
            Assert.AreEqual(1, comments.Count);
            Assert.AreEqual(comments[0], comment2);

            comments = CommentManager.GetComments(media.BaseItemID, 0, 1, false);
            Assert.AreEqual(1, comments.Count);
            Assert.AreEqual(comments[0], comment2);

            comments = CommentManager.GetComments(media.BaseItemID, 1, 1, false);
            Assert.AreEqual(1, comments.Count);
            Assert.AreEqual(comments[0], comment);

            Utilities.SwitchToNonOwnerUser();
            album.Delete();
        }

        [TestMethod]
        public void OwnerDeleteComment()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual("My comment", comment.Text);
            Assert.AreEqual(media.BaseItemID, comment.BaseItemID);
            CommentManager.DeleteComment(comment.CommentID);
            Assert.IsFalse(CommentManager.CommentExists(comment.CommentID));
            album.Delete();
        }

        [TestMethod]
        public void AdminDeleteComment()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual("My comment", comment.Text);
            Assert.AreEqual(media.BaseItemID, comment.BaseItemID);
            Utilities.SwitchToAdminUser();
            CommentManager.DeleteComment(comment.CommentID);
            Assert.IsFalse(CommentManager.CommentExists(comment.CommentID));
            album.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void NonOwnerDeleteComment()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual("My comment", comment.Text);
            Assert.AreEqual(media.BaseItemID, comment.BaseItemID);
            Utilities.SwitchToNonOwnerUser();
            CommentManager.DeleteComment(comment.CommentID);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AnonymousDeleteComment()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual("My comment", comment.Text);
            Assert.AreEqual(media.BaseItemID, comment.BaseItemID);
            Utilities.SwitchToAnonymousUser();
            CommentManager.DeleteComment(comment.CommentID);
        }

        [TestMethod]
        public void BaseItemOwnerDeleteAdminComment()
        {
            Utilities.SwitchToOwnerUser();
            Album album = AlbumManager.CreateAlbum(Constants.Strings.AlbumName);
            Media media = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption", "My Description");
            Utilities.SwitchToNonOwnerUser();
            Comment comment = CommentManager.CreateComment(media.BaseItemID, "My comment");
            Assert.AreEqual("My comment", comment.Text);
            Assert.AreEqual(media.BaseItemID, comment.BaseItemID);
            Utilities.SwitchToOwnerUser();
            CommentManager.DeleteComment(comment.CommentID);
            Assert.IsFalse(CommentManager.CommentExists(comment.CommentID));
        }

        #region UpdateComment tests (currently not supported)
        //[TestMethod]
        //public void OwnerUpdateComment()
        //{
        //    Utilities.SwitchToOwnerUser();
        //    Album album = AlbumManager.CreateAlbum("My Album");
        //    Picture picture = PictureManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption");
        //    Comment comment = CommentManager.CreateComment(picture.ThreadID, "My comment");
        //    Assert.AreEqual("My comment", comment.Text);
        //    Assert.AreEqual(picture.ThreadID, comment.ThreadID);
        //    comment.Text = "I changed my mind";
        //    CommentManager.UpdateComment(comment);
        //    Assert.AreEqual("I changed my mind", CommentManager.GetComment(comment.CommentID).Text);
        //}

        //[TestMethod]
        //public void AdminUpdateComment()
        //{
        //    Utilities.SwitchToOwnerUser();
        //    Album album = AlbumManager.CreateAlbum("My Album");
        //    Picture picture = PictureManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption");
        //    Comment comment = CommentManager.CreateComment(picture.ThreadID, "My comment");
        //    Assert.AreEqual("My comment", comment.Text);
        //    Assert.AreEqual(picture.ThreadID, comment.ThreadID);
        //    comment.Text = "I changed my mind";
        //    Utilities.SwitchToAdminUser();
        //    CommentManager.UpdateComment(comment);
        //    Assert.AreEqual("I changed my mind", CommentManager.GetComment(comment.CommentID).Text);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(SecurityException))]
        //public void NonOwnerUpdateComment()
        //{
        //    Utilities.SwitchToOwnerUser();
        //    Album album = AlbumManager.CreateAlbum("My Album");
        //    Picture picture = PictureManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption");
        //    Comment comment = CommentManager.CreateComment(picture.ThreadID, "My comment");
        //    Assert.AreEqual("My comment", comment.Text);
        //    Assert.AreEqual(picture.ThreadID, comment.ThreadID);
        //    comment.Text = "I changed my mind";
        //    Utilities.SwitchToNonOwnerUser();
        //    CommentManager.UpdateComment(comment);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(SecurityException))]
        //public void AnonymousUpdateComment()
        //{
        //    Utilities.SwitchToOwnerUser();
        //    Album album = AlbumManager.CreateAlbum("My Album");
        //    Picture picture = PictureManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "My Caption");
        //    Comment comment = CommentManager.CreateComment(picture.ThreadID, "My comment");
        //    Assert.AreEqual("My comment", comment.Text);
        //    Assert.AreEqual(picture.ThreadID, comment.ThreadID);
        //    comment.Text = "I changed my mind";
        //    Utilities.SwitchToAnonymousUser();
        //    CommentManager.UpdateComment(comment);
        //}
        #endregion

    }
}
