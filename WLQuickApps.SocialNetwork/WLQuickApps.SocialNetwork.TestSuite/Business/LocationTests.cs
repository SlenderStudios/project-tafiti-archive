using System;
using System.Text;
using System.Collections.Generic;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for LocationTests
    /// </summary>
    [TestClass]
    public class LocationTests
    {
        public LocationTests()
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
        public void TestLocation()
        {
            Location location = LocationManager.CreateLocation("WLQuickApps", "16625 Redmond Way", "Suite M PMB 206", "Redmond", "WA", "USA", "98052");
            Assert.AreEqual("WLQuickApps", location.Name);
            Assert.AreEqual("16625 Redmond Way", location.Address1);
            Assert.AreEqual("Suite M PMB 206", location.Address2);
            Assert.AreEqual("Redmond", location.City);
            Assert.AreEqual("WA", location.Region);
            Assert.AreEqual("USA", location.Country);
            Assert.AreEqual("98052", location.PostalCode);

            Assert.AreEqual(location.LocationID, LocationManager.CreateLocation("WLQuickApps", "16625 Redmond Way", "Suite M PMB 206", "Redmond", "WA", "USA", "98052").LocationID);

            Assert.AreEqual(Guid.Empty, Location.Empty.LocationID);
        }

        #region SearchLocations tests
        [TestMethod]
        public void TestSearchLocations_Name()
        {
            Location location = Utilities.TestSearchLocation;
            List<Location> locations = LocationManager.SearchLocations("Sharp", null, null, null, null, null, null);
            Assert.IsTrue(locations.Contains(location));
        }

        [TestMethod]
        public void TestSearchLocations_Address()
        {
            Location location = Utilities.TestSearchLocation;
            List<Location> locations = LocationManager.SearchLocations(null, "16625 Redmond Way", "", "", "", "", "");
            Assert.IsTrue(locations.Contains(location));
        }

        [TestMethod]
        public void TestSearchLocations_Zip()
        {
            Location location = Utilities.TestSearchLocation;
            List<Location> locations = LocationManager.SearchLocations("", "", "", "", "", "", "98052");
            Assert.IsTrue(locations.Contains(location));
        }

        [TestMethod]
        public void TestSearchLocations_City()
        {
            Location location = Utilities.TestSearchLocation;
            List<Location> locations = LocationManager.SearchLocations("", "", "", "Redmond", "", "", "");
            Assert.IsTrue(locations.Contains(location));
        }

        [TestMethod]
        public void TestSearchLocations_NameCity()
        {
            Location location = Utilities.TestSearchLocation;
            List<Location> locations = LocationManager.SearchLocations("Sharp", "", "", "Redmond", "", "", "");
            Assert.IsTrue(locations.Contains(location));
        }

        [TestMethod]
        public void TestSearchLocations_NameDifferentCity()
        {
            Location location = Utilities.TestSearchLocation;
            List<Location> locations = LocationManager.SearchLocations("Sharp", "", "", "Spokane", "", "", "");
            Assert.IsFalse(locations.Contains(location));
        }

        [TestMethod]
        public void TestSearchLocations_NameCityDifferentZip()
        {
            Location location = Utilities.TestSearchLocation;
            List<Location> locations = LocationManager.SearchLocations("Sharp", "", "", "Redmond", "", "", "98059");
            Assert.IsFalse(locations.Contains(location));
        }
        #endregion

    }
}
