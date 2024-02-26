using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for UserTests
    /// </summary>
    [TestClass]
    public class UserTests
    {
        public UserTests()
        {
        }
        
        [TestInitialize()]
        public void MyTestInitialize()
        {
            Utilities.SwitchToAdminUser();
            User existingUser;
            if (UserManager.TryGetUserByUserName(Constants.Strings.UserName, out existingUser))
            {
                existingUser.Delete();
            }
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            Utilities.SwitchToAdminUser();
            User existingUser;
            if (UserManager.TryGetUserByUserName(Constants.Strings.UserName, out existingUser))
            {
                existingUser.Delete();
            }
        }

        [TestMethod]
        public void CreateAndDeleteUser()
        {
            string userName = Constants.Strings.UserName;
            string email = "email@WLQuickApps.com";
            string firstName = "First Name";
            string lastName = "Last Name";
            string liveID = "Live ID" + Guid.NewGuid();
            string description = "About Me";
            string rssFeed = "http://www.WLQuickApps.com/blogs/news/SyndicationService.asmx/GetRss";
            string messengerPresenceID = "22BC3EEF0F511CA";
            Gender gender = Gender.Unspecified;
            DateTime dob = DateTime.Now.AddYears(-14);

            User user = UserManager.CreateUser(userName, email, firstName, lastName, gender, dob, liveID, description, 
                Utilities.TestLocation, rssFeed, Utilities.TestPictureBits, messengerPresenceID, string.Empty, string.Empty);

            Assert.AreEqual(userName, user.UserName);
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual(firstName, user.FirstName);
            Assert.AreEqual(lastName, user.LastName);
            Assert.AreEqual(liveID, user.WindowsLiveUUID);
            Assert.AreEqual(description, user.Description);
            Assert.AreEqual(rssFeed, user.RssFeedUrl);
            Assert.AreEqual(gender, user.Gender);
            Assert.AreEqual(dob.ToString(), user.DateOfBirth.ToString());
            Assert.AreEqual(Utilities.TestLocation, user.Location);

            Assert.AreEqual(user, UserManager.GetUser(user.UserID));
            Assert.AreEqual(user, UserManager.GetUserByBaseItemID(user.BaseItemID));
            Assert.AreEqual(user, UserManager.GetUserByUserName(user.UserName));

            User tryUser;
            Assert.IsTrue(UserManager.TryGetUserByEmail(email, out tryUser));
            Assert.AreEqual(user, tryUser);
            Assert.IsTrue(UserManager.TryGetUserByUserName(userName, out tryUser));
            Assert.AreEqual(user, tryUser);
            Assert.IsTrue(UserManager.TryGetUserByWindowsLiveUUID(liveID, out tryUser));
            Assert.AreEqual(user, tryUser);

            dob = DateTime.Now.AddYears(-15);
            user.Description = "New About Me";
            user.DateOfBirth = dob;
            user.FirstName = "New First Name";
            user.Gender = Gender.Male;
            user.LastName = "New Last Name";
            user.Location = Location.Empty;
            user.PrivacyLevel = PrivacyLevel.Private;
            user.RssFeedUrl = "http://new";
            user.Title = "New Title";
            
            user.Update();
            user = UserManager.GetUser(user.UserID);

            Assert.AreEqual("New About Me", user.Description);
            Assert.AreEqual(dob.ToString(), user.DateOfBirth.ToString());
            Assert.AreEqual("New First Name", user.FirstName);
            Assert.AreEqual(Gender.Male, user.Gender);
            Assert.AreEqual("New Last Name", user.LastName);
            Assert.AreEqual(Location.Empty, user.Location);
            Assert.AreEqual(PrivacyLevel.Private, user.PrivacyLevel);
            Assert.AreEqual("http://new", user.RssFeedUrl);
            Assert.AreEqual("New Title", user.Title);

            user.Delete();
        }

        [TestMethod]
        public void GetUserFromBaseItemManager()
        {
            Utilities.SwitchToAnonymousUser();

            string userName = Constants.Strings.UserName;
            string email = "email@WLQuickApps.com";
            string firstName = "First Name";
            string lastName = "Last Name";
            string liveID = "Live ID" + Guid.NewGuid();
            string description = "About Me";
            string rssFeed = "http://www.WLQuickApps.com/blogs/news/SyndicationService.asmx/GetRss";
            Gender gender = Gender.Unspecified;
            DateTime dob = DateTime.Now.AddYears(-14);
            string messengerPresenceID = "22BC3EEF0F511CA";
           
            User expected = UserManager.CreateUser(userName, email, firstName, lastName, gender, dob, liveID, description,
                Utilities.TestLocation, rssFeed, Utilities.TestPictureBits, messengerPresenceID, string.Empty, string.Empty);
            BaseItem actual = BaseItemManager.GetBaseItem(expected.BaseItemID);

            Assert.IsInstanceOfType(actual, typeof(User));
            Assert.AreEqual(expected.BaseItemID, actual.BaseItemID);
            Assert.AreEqual(expected.Title, actual.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(SecurityException))]
        public void AssertUserIsLoggedInWithAnon()
        {
            Utilities.SwitchToAnonymousUser();
            UserManager.AssertThatAUserIsLoggedIn();
        }

        [TestMethod]
        public void AssertUserIsLoggedInWithValidUsers()
        {
            Utilities.SwitchToOwnerUser();
            UserManager.AssertThatAUserIsLoggedIn();
            Utilities.SwitchToNonOwnerUser();
            UserManager.AssertThatAUserIsLoggedIn();
            Utilities.SwitchToAdminUser();
            UserManager.AssertThatAUserIsLoggedIn();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_EmptyName()
        {
            User user = UserManager.CreateUser(string.Empty, string.Format("{0}@WLQuickApps.com", Guid.NewGuid()), "WLQuickApps", "TestAccount", Gender.Unspecified, DateTime.Now.AddYears(-14), Guid.NewGuid().ToString(), "About me", Location.Empty, null, null, "22BC3EEF0F511CA", string.Empty, string.Empty);
            user.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_NullName()
        {
            User user = UserManager.CreateUser(null, string.Format("{0}@WLQuickApps.com", Guid.NewGuid()), "WLQuickApps", "TestAccount", Gender.Unspecified, DateTime.Now.AddYears(-14), Guid.NewGuid().ToString(), "About me", Location.Empty, null, null, "22BC3EEF0F511CA", string.Empty, string.Empty);
            user.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_EmptyEmail()
        {
            User user = UserManager.CreateUser(Constants.Strings.UserName, string.Empty, "WLQuickApps", "TestAccount", Gender.Unspecified, DateTime.Now.AddYears(-14), Guid.NewGuid().ToString(), "About me", Location.Empty, null, null, "22BC3EEF0F511CA", string.Empty, string.Empty);
            user.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_NullEmail()
        {
            User user = UserManager.CreateUser(Constants.Strings.UserName, null, "WLQuickApps", "TestAccount", Gender.Unspecified, DateTime.Now.AddYears(-14), Guid.NewGuid().ToString(), "About me", Location.Empty, null, null, "22BC3EEF0F511CA", string.Empty, string.Empty);
            user.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_EmptyFirstName()
        {
            User user = UserManager.CreateUser(Constants.Strings.UserName, string.Format("{0}@WLQuickApps.com", Guid.NewGuid()), string.Empty, "TestAccount", Gender.Unspecified, DateTime.Now.AddYears(-14), Guid.NewGuid().ToString(), "About me", Location.Empty, null, null, "22BC3EEF0F511CA", string.Empty, string.Empty);
            user.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_NullFirstName()
        {
            User user = UserManager.CreateUser(Constants.Strings.UserName, string.Format("{0}@WLQuickApps.com", Guid.NewGuid()), null, "TestAccount", Gender.Unspecified, DateTime.Now.AddYears(-14), Guid.NewGuid().ToString(), "About me", Location.Empty, null, null, "22BC3EEF0F511CA", string.Empty, string.Empty);
            user.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_EmptyLastName()
        {
            User user = UserManager.CreateUser(Constants.Strings.UserName, string.Format("{0}@WLQuickApps.com", Guid.NewGuid()), "WLQuickApps", string.Empty, Gender.Unspecified, DateTime.Now.AddYears(-14), Guid.NewGuid().ToString(), "About me", Location.Empty, null, null, "22BC3EEF0F511CA", string.Empty, string.Empty);
            user.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_NullLastName()
        {
            User user = UserManager.CreateUser(Constants.Strings.UserName, string.Format("{0}@WLQuickApps.com", Guid.NewGuid()), "WLQuickApps", null, Gender.Unspecified, DateTime.Now.AddYears(-14), Guid.NewGuid().ToString(), "About me", Location.Empty, null, null, "22BC3EEF0F511CA", string.Empty, string.Empty);
            user.Delete();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateUser_TooYoung()
        {
            User user = UserManager.CreateUser(Constants.Strings.UserName, string.Format("{0}@WLQuickApps.com", Guid.NewGuid()), "WLQuickApps", "TestAccount", Gender.Unspecified, DateTime.Now.AddYears(-12), Guid.NewGuid().ToString(), "About me", Location.Empty, null, null, "22BC3EEF0F511CA", string.Empty, string.Empty);
            user.Delete();
        }

    }
}
