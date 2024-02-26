using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;
using System.Collections.ObjectModel;

namespace WLQuickApps.SocialNetwork.Business
{

    public class Forum : BaseItem, IComparable<Forum>
    {
        public override bool CanContribute { get { return true; } }
        public override bool CanView { get { return true; } }

        internal Forum(int baseItemID)
            : base(baseItemID)
        {
        }

        internal Forum(ForumDataSet.ForumRow row)
            : base(row.BaseItemID)
        {
        }

        protected override string GetSearchTerms()
        {
            return string.Empty;
        }


        #region IComparable<Forum> Members

        public int CompareTo(Forum other)
        {
            if (other == null) { return 1; }

            return this.BaseItemID.CompareTo(other.BaseItemID);
        }

        #endregion
    }
}
