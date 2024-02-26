using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;
using System.Collections.ObjectModel;

namespace WLQuickApps.SocialNetwork.Business
{
    public class CollectionItem : BaseItem
    {
        public Collection Collection
        {
            get
            {
                if (this._collection == null)
                {
                    this._collection = CollectionManager.GetCollection(this._collectionBaseItemID);
                }

                return this._collection;
            }
        }
        private int _collectionBaseItemID;
        private Collection _collection;

        internal CollectionItem(int baseItemID, int collectionBaseItemID)
            : base(baseItemID)
        {
            this._collectionBaseItemID = collectionBaseItemID;
        }

        internal CollectionItem(CollectionItemDataSet.CollectionItemRow row)
            : this(row.BaseItemID, row.CollectionBaseItemID)
        {
        }

        protected override string GetSearchTerms()
        {
            return string.Empty;
        }
    }
}
