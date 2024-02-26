using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.TestSuite
{
    /// <summary>
    /// Summary description for AlbumTests
    /// </summary>
    [TestClass]
    public class CollectionTests
    {
        public CollectionTests()
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

        [TestInitialize]
        [TestCleanup]
        public void MyTestCleanup()
        {
            Utilities.DeleteAllCollectionsForTestUsers();
        }

        #endregion


        #region CreateCollection
        
        [TestMethod]
        public void CreateAndDeleteCollection()
        {
            Utilities.SwitchToOwnerUser();

            Collection collection = CollectionManager.CreateCollection(Constants.Strings.CollectionName, Constants.Strings.CollectionDescription, Constants.Strings.CollectionType);

            Assert.AreEqual(Constants.Strings.CollectionName, collection.Title);
            Assert.AreEqual(Constants.Strings.CollectionDescription, collection.Description);
            Assert.AreEqual(Constants.Strings.CollectionType, collection.SubType);
            Assert.AreEqual(0, collection.Items.Count);

            collection.Title = "New Title";
            collection.Description = "New Description";
            collection.Update();
            collection = CollectionManager.GetCollection(collection.BaseItemID);
            Assert.AreEqual("New Title", collection.Title);
            Assert.AreEqual("New Description", collection.Description);

            CollectionItem collectionItem = CollectionItemManager.CreateCollectionItem(collection, Utilities.TestLocation, Constants.Strings.CollectionItemName, Constants.Strings.CollectionItemDescription, Utilities.TestPictureBits);
            Assert.AreEqual(Constants.Strings.CollectionItemName, collectionItem.Title);
            Assert.AreEqual(Constants.Strings.CollectionItemDescription, collectionItem.Description);
            Assert.AreEqual(1, collection.Items.Count);
            Assert.AreEqual(collectionItem, collection.Items[0]);
            Assert.IsTrue(collection.HasLocation(Utilities.TestLocation));

            collectionItem.Title = "New Title";
            collectionItem.Description = "New Description";
            collectionItem.Update();
            collectionItem = CollectionItemManager.GetCollectionItem(collectionItem.BaseItemID);
            Assert.AreEqual("New Title", collectionItem.Title);
            Assert.AreEqual("New Description", collectionItem.Description);

            collectionItem.Delete();
            Assert.AreEqual(0, collection.Items.Count);

            collection.Delete();
        }

        #endregion

        #region Get Collections

        [TestMethod]
        public void GetCollectionFromBaseItemManager()
        {
            Utilities.SwitchToOwnerUser();

            Collection expected = CollectionManager.CreateCollection(Constants.Strings.CollectionName, Constants.Strings.CollectionDescription, Constants.Strings.CollectionType);
            BaseItem actual = BaseItemManager.GetBaseItem(expected.BaseItemID);

            Assert.IsInstanceOfType(actual, typeof(Collection));
            Assert.AreEqual(expected.BaseItemID, actual.BaseItemID);
            Assert.AreEqual(expected.Title, actual.Title);
        }

        [TestMethod]
        public void GetMostRecentCollections()
        {
            Utilities.SwitchToOwnerUser();

            Collection newCollection = CollectionManager.CreateCollection(Constants.Strings.CollectionName, Constants.Strings.CollectionDescription, Constants.Strings.CollectionType);
            Collection newerCollection = CollectionManager.CreateCollection(Constants.Strings.CollectionName, Constants.Strings.CollectionDescription, Constants.Strings.CollectionType);
            Collection newestCollection = CollectionManager.CreateCollection(Constants.Strings.CollectionName, Constants.Strings.CollectionDescription, Constants.Strings.CollectionType);

            ReadOnlyCollection<Collection> mostRecentCollections = CollectionManager.GetMostRecentCollections(Constants.Strings.CollectionType);

            Assert.IsNotNull(mostRecentCollections);
            Assert.IsTrue(mostRecentCollections.Count >= 3);
            Assert.AreEqual(newestCollection.BaseItemID, mostRecentCollections[0].BaseItemID);
            Assert.AreEqual(newerCollection.BaseItemID, mostRecentCollections[1].BaseItemID);
            Assert.AreEqual(newCollection.BaseItemID, mostRecentCollections[2].BaseItemID);
        }

        #endregion

        #region DeleteCollection

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteCollection()
        {
            Utilities.SwitchToOwnerUser();

            Collection collection = CollectionManager.CreateCollection(Constants.Strings.CollectionName, Constants.Strings.CollectionDescription, Constants.Strings.CollectionType);
            CollectionItem collectionItem = CollectionItemManager.CreateCollectionItem(collection, Utilities.TestLocation, Constants.Strings.CollectionItemName, Constants.Strings.CollectionItemDescription, Utilities.TestPictureBits);
            collection.Delete();
            collectionItem = CollectionItemManager.GetCollectionItem(collectionItem.BaseItemID);
        }

        #endregion

        #region Search Collections

        [TestMethod]
        public void SearchCollections()
        {
            Utilities.SwitchToOwnerUser();

            Collection collection = CollectionManager.CreateCollection(Constants.Strings.CollectionName, Constants.Strings.CollectionDescription, Constants.Strings.CollectionType);
            CollectionItem collectionItem = CollectionItemManager.CreateCollectionItem(collection, Utilities.TestLocation, Constants.Strings.CollectionItemName, Constants.Strings.CollectionItemDescription, Utilities.TestPictureBits);

            ReadOnlyCollection<Collection> collections = CollectionManager.GetCollectionsByCollectionType(Constants.Strings.CollectionType);
            Assert.IsTrue(collections.Contains(collection));

            collections = CollectionManager.GetCollectionsByNameExact(Constants.Strings.CollectionName, Constants.Strings.CollectionType);
            Assert.IsTrue(collections.Contains(collection));

            collections = CollectionManager.GetCollectionsByUser(Utilities.OwnerUser.UserName, Constants.Strings.CollectionType);
            Assert.IsTrue(collections.Contains(collection));

            collection.Delete();
        }

        [TestMethod]
        public void GetCollectionsPaged()
        {
            Utilities.SwitchToOwnerUser();

            int collectionIndex;
            List<Collection> myCollections = new List<Collection>(10);

            for (collectionIndex = 0; collectionIndex < 10; collectionIndex++)
            {
                Collection collection = CollectionManager.CreateCollection(Constants.Strings.CollectionName,
                    Constants.Strings.CollectionDescription, Constants.Strings.CollectionType);
                CollectionItem collectionItem = CollectionItemManager.CreateCollectionItem(collection,
                    Utilities.TestLocation, Constants.Strings.CollectionItemName, Constants.Strings.CollectionItemDescription, Utilities.TestPictureBits);

                myCollections.Add(collection);
            }
                        
            ReadOnlyCollection<Collection> collections;
            Assert.AreEqual(10, CollectionManager.GetCollectionsByUserCount(Utilities.OwnerUser.UserName, Constants.Strings.CollectionType));

            collections = CollectionManager.GetCollectionsByUser(Utilities.OwnerUser.UserName, Constants.Strings.CollectionType, 0, 5);
            for (collectionIndex = 0; collectionIndex < 5; collectionIndex++)
            {
                Assert.AreEqual(myCollections[collectionIndex], collections[collectionIndex]);
            }
            
            collections = CollectionManager.GetCollectionsByUser(Utilities.OwnerUser.UserName, Constants.Strings.CollectionType, 5, 5);
            for (collectionIndex = 0; collectionIndex < 5; collectionIndex++)
            {
                Assert.AreEqual(myCollections[collectionIndex + 5], collections[collectionIndex]);
            }
        }

        #endregion

        [TestMethod]
        public void AssociateCollection()
        {
            Utilities.SwitchToOwnerUser();
            Collection collection = CollectionManager.CreateCollection(Constants.Strings.CollectionName, Constants.Strings.CollectionDescription, Constants.Strings.CollectionType);
            Group group = GroupManager.CreateGroup(Constants.Strings.GroupName, Constants.Strings.GroupDescription, Constants.Strings.GroupType, PrivacyLevel.Public);
            group.Associate(collection);

            Assert.AreEqual(1, group.Collections.Count);
            Assert.AreEqual(collection, group.Collections[0]);

            group.RemoveBaseItemAssociation(collection, true);

            Assert.AreEqual(0, group.Collections.Count);

            group.Associate(collection);

            Assert.AreEqual(1, group.Collections.Count);
            Assert.AreEqual(collection, group.Collections[0]);

            collection.Delete();

            Assert.AreEqual(0, group.Collections.Count);

            group.Delete();
        }
    }
}
