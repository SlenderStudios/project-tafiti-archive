using System;
using System.Collections.Generic;
using System.Text;
using WLQuickApps.SocialNetwork.Data.CollectionItemDataSetTableAdapters;
using WLQuickApps.SocialNetwork.Data;
using System.Transactions;
using System.Security;
using WLQuickApps.SocialNetwork.Business.Properties;

namespace WLQuickApps.SocialNetwork.Business
{
    public class CollectionItemManager
    {
        static public CollectionItem CreateCollectionItem(Collection collection, Location location, string title, string description, byte[] thumbnailBits)
        {
            if (collection == null) { throw new ArgumentNullException("collection"); }
            if (location == null) { throw new ArgumentNullException("location"); }
            if (string.IsNullOrEmpty(title)) { throw new ArgumentNullException("title"); }

            CollectionManager.VerifyOwnerActionOnCollection(collection);

            int baseItemID;
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (CollectionItemTableAdapter collectionItemTableAdapter = new CollectionItemTableAdapter())
            {
                if (collection.HasLocation(location))
                {
                    throw new InvalidOperationException("Collection already has an item with this location.");
                }

                baseItemID = BaseItemManager.CreateBaseItem("CollectionItem", location, title, description, collection.Owner,
                    string.Empty, PrivacyLevel.Public, true, string.Empty);
                collectionItemTableAdapter.CreateCollectionItem(baseItemID, collection.BaseItemID);

                transaction.Complete();
            }

            CollectionItem collectionItem = CollectionItemManager.GetCollectionItem(baseItemID);
            collectionItem.Update();

            return collectionItem;
        }

        static public CollectionItem GetCollectionItem(int baseItemID)
        {
            using (CollectionItemTableAdapter collectionItemTableAdapter = new CollectionItemTableAdapter())
            {
                CollectionItemDataSet.CollectionItemDataTable items = collectionItemTableAdapter.GetCollectionItem(baseItemID);

                if (items.Count == 0)
                {
                    throw new ArgumentException("There is no collection item with that ID.", "baseItemID");
                }

                CollectionItem collectionItem = new CollectionItem(items[0]);
                if (!collectionItem.CanView) { throw new SecurityException("The current user does not have permission to access to this item"); }
                return collectionItem;
            }
        }

        static public List<CollectionItem> GetCollectionItemsForCollection(Collection collection)
        {
            if (collection == null) { throw new ArgumentNullException("collection"); }

            List<CollectionItem> list = new List<CollectionItem>();

            using (CollectionItemTableAdapter collectionItemTableAdapter = new CollectionItemTableAdapter())
            {
                CollectionItemDataSet.CollectionItemDataTable items = collectionItemTableAdapter.GetCollectionItemsForCollection(
                    collection.BaseItemID);

                foreach (CollectionItemDataSet.CollectionItemRow row in items)
                {
                    list.Add(new CollectionItem(row));
                }
            }

            return list;
        }

        static public void DeleteCollectionItem(CollectionItem collectionItem)
        {
            CollectionItemManager.VerifyOwnerActionOnCollectionItem(collectionItem);

            BaseItemManager.DeleteBaseItem(collectionItem);
        }

        static public void UpdateCollectionItem(CollectionItem collectionItem)
        {
            if (collectionItem == null) { throw new ArgumentNullException("collectionItem"); }

            CollectionItemManager.VerifyOwnerActionOnCollectionItem(collectionItem);

            BaseItemManager.UpdateBaseItem(collectionItem);
        }

        static internal void VerifyOwnerActionOnCollectionItem(CollectionItem collectionItem)
        {
            if (!CollectionItemManager.CanModifyCollectionItem(collectionItem.BaseItemID))
            {
                throw new SecurityException("CollectionItem cannot be modified because it does not belong to the logged in user");
            }
        }

        static public bool CanModifyCollectionItem(int baseItemID)
        {
            CollectionItem collectionItem = CollectionItemManager.GetCollectionItem(baseItemID);
            return CollectionItemManager.CanModifyCollectionItem(collectionItem);
        }

        static internal bool CanModifyCollectionItem(CollectionItem collectionItem)
        {
            return (UserManager.IsUserLoggedIn() &&
                ((collectionItem.Owner == UserManager.LoggedInUser) || UserManager.LoggedInUser.IsAdmin));
        }
    }
}
