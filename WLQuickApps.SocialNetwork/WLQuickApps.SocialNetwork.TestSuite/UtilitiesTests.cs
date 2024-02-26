using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for UtilitiesTests
    /// </summary>
    [TestClass]
    public class UtilitiesTests
    {
        public UtilitiesTests()
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
        public void TestUtilitiesUsers()
        {
            Assert.AreEqual("TestAdministrator", Utilities.AdminUser.UserName);
            Assert.IsTrue(Utilities.AdminUser.IsAdmin);
            Assert.IsNull(Utilities.AnonymousUser);

            string ownerUserName = Utilities.OwnerUser.UserName;
            string nonOwnerUserName = Utilities.NonOwnerUser.UserName;
            
            Utilities.SwitchToAdminUser();
            Assert.AreEqual("TestAdministrator", UserManager.LoggedInUser.UserName);
            
            Utilities.SwitchToOwnerUser();
            Assert.AreEqual(ownerUserName, UserManager.LoggedInUser.UserName);
            
            Utilities.SwitchToNonOwnerUser();
            Assert.AreEqual(nonOwnerUserName, UserManager.LoggedInUser.UserName);

            Utilities.SwitchToAnonymousUser();
            Assert.IsNull(UserManager.LoggedInUser);
        }
    }
}
