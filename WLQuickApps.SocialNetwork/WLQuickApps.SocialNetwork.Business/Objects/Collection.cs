using System;
using System.Collections.Generic;
using System.Text;
using WLQuickApps.SocialNetwork.Data;
using System.Collections.ObjectModel;

namespace WLQuickApps.SocialNetwork.Business
{
    public class Collection : BaseItem
    {
        public ReadOnlyCollection<CollectionItem> Items
        {
            get
            {
                return CollectionItemManager.GetCollectionItemsForCollection(this).AsReadOnly();
            }
        }

        internal Collection(int baseItemID)
            : base(baseItemID)
        {
        }

        internal Collection(CollectionDataSet.CollectionRow row)
            : base(row.BaseItemID)
        {
        }

        public override void Delete() { CollectionManager.DeleteCollection(this); }

        protected override string GetSearchTerms()
        {
            return string.Empty;
        }

        public bool HasLocation(Location location)
        {
            return CollectionManager.IsLocationInCollection(location, this);
        }
    }
}
