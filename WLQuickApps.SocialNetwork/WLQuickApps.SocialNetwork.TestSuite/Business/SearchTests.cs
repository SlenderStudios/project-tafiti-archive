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
    public class SearchTests
    {
        public SearchTests()
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

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            Utilities.SwitchToOwnerUser();

            foreach (Event eventItem in EventManager.GetAllEvents())
            {
                eventItem.Delete();
            }
            foreach (Group group in GroupManager.GetGroupsForUser(Utilities.OwnerUser.UserName, Constants.Strings.GroupType, UserGroupStatus.Joined))
            {
                group.Delete();
            }
            foreach (Album album in Utilities.OwnerUser.Albums)
            {
                album.Delete();
            }
        }

        [TestMethod]
        public void SearchForItemsWithNoParams()
        {
            Utilities.SwitchToAnonymousUser();
            SearchResults searchResults = SearchManager.SearchNetwork(null, null, null, null, null, null, null, null);
            Assert.AreEqual(0, searchResults.Audios.Count);
            Assert.AreEqual(0, searchResults.Events.Count);
            Assert.AreEqual(0, searchResults.Groups.Count);
            Assert.AreEqual(0, searchResults.Pictures.Count);
            Assert.AreEqual(0, searchResults.Users.Count);
            Assert.AreEqual(0, searchResults.Videos.Count);
        }
      
        [TestMethod]
        public void SearchForItemsWithTag()
        {
            Utilities.SwitchToOwnerUser();

            string tag = "SearchItemTag";

            Event eventItem = EventManager.CreateEvent(Constants.Strings.EventName, Constants.Strings.EventDescription, DateTime.Now, DateTime.Now.AddYears(1), Location.Empty, PrivacyLevel.Public, Constants.Strings.EventType);
            eventItem.AddTag(tag);
            eventItem.AddTag("abc");
            eventItem.AddTag("xyz");
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            group.AddTag(tag);
            group.AddTag("abc");
            group.AddTag("xyz");
            Utilities.OwnerUser.AddTag(tag);
            Utilities.OwnerUser.AddTag("abc");
            Utilities.OwnerUser.AddTag("xyz");
            Album album = AlbumManager.CreateAlbum("Name");
            Media picture = MediaManager.CreatePicture(Utilities.TestPictureBits, album.BaseItemID, "Name", "Description");
            picture.AddTag(tag);
            picture.AddTag("abc");
            picture.AddTag("xyz");
            Media audio = MediaManager.CreateAudio("file.wma", Utilities.TestAudioBits, album.BaseItemID, "Name", "Description");
            audio.AddTag(tag);
            audio.AddTag("abc");
            audio.AddTag("xyz");
            Media video = MediaManager.CreateVideo("file.wmv", Utilities.TestVideoBits, album.BaseItemID, "Name", "Description");
            video.AddTag(tag);
            video.AddTag("abc");
            video.AddTag("xyz");

            Utilities.SwitchToAnonymousUser();
            SearchResults searchResults = SearchManager.SearchNetwork(null, null, null, null, null, null, null, tag);

            Assert.IsTrue(searchResults.Audios.Contains(audio));
            Assert.IsFalse(searchResults.Events.Contains(eventItem));
            Assert.IsFalse(searchResults.Groups.Contains(group));
            Assert.IsTrue(searchResults.GetSpecialEvents(Constants.Strings.EventType).Contains(eventItem));
            Assert.IsTrue(searchResults.GetSpecialGroups(Constants.Strings.GroupType).Contains(group));
            Assert.IsTrue(searchResults.Pictures.Contains(picture));
            Assert.IsTrue(searchResults.Users.Contains(Utilities.OwnerUser));
            Assert.IsTrue(searchResults.Videos.Contains(video));

        }

    }
}
