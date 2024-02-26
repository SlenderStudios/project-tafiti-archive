using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Security;

using WLQuickApps.SocialNetwork.Business.Properties;
using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.BaseItemDataSetTableAdapters;
using WLQuickApps.SocialNetwork.Data.CollectionDataSetTableAdapters;
using System.Transactions;

namespace WLQuickApps.SocialNetwork.Business
{
    public class CollectionManager
    {
        public CollectionManager() { }

        static public Collection CreateCollection(string name, string description, string collectionType)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("Name cannot be null or empty"); }

            int baseItemID;
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                baseItemID = Convert.ToInt32(BaseItemManager.CreateBaseItem("Collection", Location.Empty, name, description,
                    UserManager.LoggedInUser, collectionType, PrivacyLevel.Public, SettingsWrapper.AutomaticallyApproveNewCollections, string.Empty));
                collectionTableAdapter.CreateCollection(baseItemID);

                transaction.Complete();
            }

            Collection collection = CollectionManager.GetCollection(baseItemID);
            collection.Update();

            return collection;
        }

        static public Collection GetCollection(int baseItemID)
        {
            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                object o = collectionTableAdapter.GetCollection(baseItemID);

                if (o == null) { throw new ArgumentException("Collection does not exist"); }

                Collection collection = new Collection(Convert.ToInt32(o));
                if (!collection.CanView) { throw new SecurityException("The current user does not have permission to access to this item"); }
                return collection;
            }
        }

        static public bool CollectionExists(int baseItemID)
        {
            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return (collectionTableAdapter.GetCollection(baseItemID) != null);
            }
        }

        static public ReadOnlyCollection<Collection> GetCollectionsByNameExact(string name, string collectionType)
        {
            return CollectionManager.GetCollectionsByNameExact(name, collectionType, 0, CollectionManager.GetCollectionsByNameExactCount(name, collectionType));
        }

        static public int GetCollectionsByNameExactCount(string name, string collectionType)
        {
            if (collectionType == null) { collectionType = string.Empty; }
            
            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return (int)collectionTableAdapter.GetCollectionsByNameCount(name, collectionType);
            }
        }


        static public ReadOnlyCollection<Collection> GetCollectionsByNameExact(string name, string collectionType, int startRowIndex, int maximumRows)
        {
            if (string.IsNullOrEmpty(name)) { return new List<Collection>().AsReadOnly(); }

            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return CollectionManager.GetCollectionsFromTable(collectionTableAdapter.GetCollectionsByName(name,
                    collectionType, startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Collection> GetCollectionsByNameExact(string name)
        {
            return CollectionManager.GetCollectionsByNameExact(name, string.Empty);
        }

        static public bool IsLocationInCollection(Location location, Collection collection)
        {
            if (location == null) { throw new ArgumentNullException("location"); }
            if (collection == null) { throw new ArgumentNullException("collection"); }

            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return (bool)collectionTableAdapter.IsLocationInCollection(location.LocationID, collection.BaseItemID);
            }
        }

        static public ReadOnlyCollection<Collection> SearchCollectionsByName(string name, string collectionType)
        {
            return CollectionManager.SearchCollectionsByName(name, collectionType, 0, CollectionManager.SearchCollectionsByNameCount(name, collectionType));
        }

        static public ReadOnlyCollection<Collection> SearchCollectionsByName(string name, string collectionType, int startRowIndex, int maximumRows)
        {
            if (string.IsNullOrEmpty(name)) { return new List<Collection>().AsReadOnly(); }

            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return CollectionManager.GetCollectionsFromTable(collectionTableAdapter.GetCollectionsByName(
                    string.Format("%{0}%", name), collectionType, startRowIndex, maximumRows));
            }
        }

        static public int SearchCollectionsByNameCount(string name, string collectionType)
        {
            if (string.IsNullOrEmpty(name)) { return 0; }

            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return (int) collectionTableAdapter.GetCollectionsByNameCount(name, collectionType);
            }
        }

        static public ReadOnlyCollection<Collection> SearchCollectionsByName(string name)
        {
            return CollectionManager.SearchCollectionsByName(name, string.Empty);
        }

        static public ReadOnlyCollection<Collection> GetAllCollections()
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return CollectionManager.GetCollectionsByUser(UserManager.LoggedInUser.UserName);
        }

        static public ReadOnlyCollection<Collection> GetCollectionsByUser(string userName, string collectionType)
        {
            return CollectionManager.GetCollectionsByUser(userName, collectionType, 0, CollectionManager.GetCollectionsByUserCount(userName, collectionType));
        }

        static public int GetCollectionsByUserCount(string userName, string collectionType)
        {
            if (string.IsNullOrEmpty(userName)) 
            {
                UserManager.AssertThatAUserIsLoggedIn();
                userName = UserManager.LoggedInUser.UserName;
            }
            if (collectionType == null) { collectionType = string.Empty; }

            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return (int) collectionTableAdapter.GetCollectionsByUserCount(UserManager.GetUserByUserName(userName).UserID, collectionType);
            }
        }

        static public ReadOnlyCollection<Collection> GetCollectionsByUser(string userName, string collectionType, int startRowIndex, int maximumRows)
        {
            if (string.IsNullOrEmpty(userName))
            {
                UserManager.AssertThatAUserIsLoggedIn();
                userName = UserManager.LoggedInUser.UserName;
            }
            if (collectionType == null) { collectionType = string.Empty; }

            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return CollectionManager.GetCollectionsFromTable(collectionTableAdapter.GetCollectionsByUser(
                    UserManager.GetUserByUserName(userName).UserID, collectionType, startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Collection> GetCollectionsByUser(string userName)
        {
            return CollectionManager.GetCollectionsByUser(userName, string.Empty);
        }

        static public ReadOnlyCollection<Collection> GetCollectionsByCollectionType(string collectionType)
        {
            return CollectionManager.GetCollectionsByCollectionType(collectionType, 0, CollectionManager.GetCollectionsByCollectionTypeCount(collectionType));
        }

        static public int GetCollectionsByCollectionTypeCount(string collectionType)
        {
            if (collectionType == null) { collectionType = string.Empty; }

            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return (int) collectionTableAdapter.GetCollectionsByCollectionTypeCount(collectionType);
            }
        }


        static public ReadOnlyCollection<Collection> GetCollectionsByCollectionType(string collectionType, int startRowIndex, int maximumRows)
        {
            if (collectionType == null) { collectionType = string.Empty; }

            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return CollectionManager.GetCollectionsFromTable(collectionTableAdapter.GetCollectionsByCollectionType(
                    collectionType, startRowIndex, maximumRows));
            }
        }

        static public ReadOnlyCollection<Collection> GetMostRecentCollections(string collectionType)
        {
            // TODO: Determine how many collection to return as "most recent".
            return CollectionManager.GetMostRecentCollections(collectionType, 0, 16);
        }

        static public ReadOnlyCollection<Collection> GetMostRecentCollections(string collectionType, int startRowIndex, int maximumRows)
        {
            if (collectionType == null) { collectionType = string.Empty; }

            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return CollectionManager.GetCollectionsFromTable(collectionTableAdapter.GetMostRecentCollections(
                    collectionType, startRowIndex, maximumRows));
            }
        }

        static public int GetMostRecentCollectionsCount(string collectionType)
        {
            //TODO: Add actual count return code
            return CollectionManager.GetMostRecentCollections(collectionType, 0, 24).Count;
        }

        static internal void DeleteCollection(Collection collection)
        {
            CollectionManager.VerifyOwnerActionOnCollection(collection);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            {
                foreach (CollectionItem collectionItem in collection.Items)
                {
                    collectionItem.Delete();
                }
                BaseItemManager.DeleteBaseItem(collection);
                transactionScope.Complete();
            }
        }

        static public void UpdateCollection(Collection collection)
        {
            if (collection == null) { throw new ArgumentNullException("collection"); }

            CollectionManager.VerifyOwnerActionOnCollection(collection);

            BaseItemManager.UpdateBaseItem(collection);
        }

        static public Collection CopyCollectionToCurrentUserAccount(Collection collection)
        {
            UserManager.AssertThatAUserIsLoggedIn();

            Collection newCollection = CollectionManager.CreateCollection(collection.Title, collection.Description, collection.SubType);
            if (collection.HasThumbnail)
            {
                newCollection.SetThumbnail(collection.GetThumbnailBits(2048, 2048));
            }

            foreach (CollectionItem collectionItem in collection.Items)
            {
                CollectionItem newCollectionItem = CollectionItemManager.CreateCollectionItem(newCollection, collectionItem.Location, collectionItem.Title, 
                    collectionItem.Description, collectionItem.GetThumbnailBits(2048, 2048));
            }

            return newCollection;
        }


        static internal void VerifyOwnerActionOnCollection(Collection collection)
        {
            if (!CollectionManager.CanModifyCollection(collection.BaseItemID))
            {
                throw new SecurityException("Collection cannot be modified because it does not belong to the logged in user");
            }
        }

        static public bool CanModifyCollection(int baseItemID)
        {
            Collection collection = CollectionManager.GetCollection(baseItemID);
            return CollectionManager.CanModifyCollection(collection);
        }

        static internal bool CanModifyCollection(Collection collection)
        {
            return (UserManager.IsUserLoggedIn() &&
                ((collection.Owner == UserManager.LoggedInUser) || UserManager.LoggedInUser.IsAdmin));
        }

        static private ReadOnlyCollection<Collection> GetCollectionsFromTable(CollectionDataSet.CollectionDataTable collectionDataTable)
        {
            List<Collection> list = new List<Collection>();
            foreach (CollectionDataSet.CollectionRow row in collectionDataTable)
            {
                Collection collection = new Collection(row);
                if (!collection.CanView) { continue; }
                list.Add(collection);
            }
            return list.AsReadOnly();
        }

        static public ReadOnlyCollection<Collection> GetCollectionsByBaseItemID(int baseItemID)
        {
            return CollectionManager.GetCollectionsByBaseItemID(baseItemID, 0, GetCollectionsByBaseItemIDCount(baseItemID));
        }

        static public int GetCollectionsByBaseItemIDCount(int baseItemID)
        {
            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return (int)collectionTableAdapter.GetCollectionsByBaseItemIDCount(baseItemID);
            }
        }

        static public ReadOnlyCollection<Collection> GetCollectionsByBaseItemID(int baseItemID, int startRowIndex, int maximumRows)
        {
            using (CollectionTableAdapter collectionTableAdapter = new CollectionTableAdapter())
            {
                return CollectionManager.GetCollectionsFromTable(collectionTableAdapter.GetCollectionsByBaseItemID(baseItemID, startRowIndex, maximumRows));
            }
        }

    }
}
