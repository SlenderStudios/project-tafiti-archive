using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for ForumTests
    /// </summary>
    [TestClass]
    public class ForumTests
    {
        public ForumTests()
        {
        }

        [TestMethod]
        public void CreateForum()
        {
            Utilities.SwitchToOwnerUser();
            Forum forum = ForumManager.CreateForum(Constants.Strings.ForumName, Constants.Strings.ForumDescription);
            forum.AddComment(Constants.Strings.ForumComment);
            Assert.AreEqual(1, forum.Comments.Count);
            Assert.AreEqual(Constants.Strings.ForumComment, forum.Comments[0].Text);
            forum.Delete();
        }

    }
}
