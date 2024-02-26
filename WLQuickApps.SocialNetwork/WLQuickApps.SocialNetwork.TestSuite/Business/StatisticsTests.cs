using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for StatisticsTests
    /// </summary>
    [TestClass]
    public class StatisticsTests
    {
        public StatisticsTests()
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
        public void GetStatistics()
        {
            Stats statsTotal = StatisticsManager.GetStatistics();
            Assert.IsTrue(statsTotal.ActiveUserCount + statsTotal.AlbumCount + statsTotal.AudioCount +
                statsTotal.CollectionCount + statsTotal.CommentCount + statsTotal.EventCount + statsTotal.FileCount + 
                statsTotal.FriendConfirmedCount + statsTotal.FriendRequestedCount + statsTotal.GroupCount + 
                statsTotal.PictureCount + statsTotal.RatingCount + statsTotal.TagCount + statsTotal.UserCount + 
                statsTotal.VideoCount >= 0);

            StatisticsManager.GetBaseItemSubTypeCount(WLQuickApps.SocialNetwork.Business.MediaType.Audio.ToString()); 
        }

    }
}