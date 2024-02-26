using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;
using System.Collections.ObjectModel;

namespace WLQuickApps.SocialNetwork.Business
{

    public class Group : BaseItem, IComparable<Group>
    {
        public List<User> Users
        {
            get
            {
                return UserManager.GetUserSetByBaseItemID(this.BaseItemID);
            }
        }

        public bool HasMember(User user)
        {
            return GroupManager.HasUserJoinedGroup(user, this);
        }

        public override bool CanAssociate { get { return GroupManager.CanAssociateWithGroup(this); } }
        public override bool CanCancel { get { return GroupManager.CanCancelJoinGroup(this); } }
        public override bool CanContribute { get { return GroupManager.CanContributeToGroup(this); } }
        public override bool CanJoin { get { return GroupManager.CanJoinGroup(this); } }
        public override bool CanLeave { get { return GroupManager.CanLeaveGroup(this); } }
        public override bool CanView { get { return GroupManager.CanViewGroup(this); } }

        internal Group(int baseItemID)
            : base(baseItemID)
        {
        }

        internal Group(GroupDataSet.GroupRow row) : base(row.BaseItemID)
        {
        }

        public override void Accept() { GroupManager.JoinGroup(this); }
        public override void Associate(BaseItem baseItem) { GroupManager.AssociateBaseItemWithGroup(baseItem, this); }
        public override void Cancel() { GroupManager.LeaveGroup(this); }
        public override void Decline() { GroupManager.LeaveGroup(this); }
        public override void Join() { GroupManager.JoinGroup(this); }
        public override void Leave() { GroupManager.LeaveGroup(this); }
        public override void Update() { GroupManager.UpdateGroup(this); }

        public void RemoveBaseItemAssociation(BaseItem baseItem, bool isAssociatedBaseItemInstigator)
        {
            BaseItemManager.RemoveBaseItemAssociationFromBaseItem(baseItem, this, isAssociatedBaseItemInstigator);
        }

        protected override string GetSearchTerms()
        {
            return string.Empty;
        }


        #region IComparable<Group> Members

        public int CompareTo(Group other)
        {
            if (other == null) { return 1; }

            return UserManager.GetUserSetByBaseItemIDCount(other.BaseItemID).CompareTo(UserManager.GetUserSetByBaseItemIDCount(this.BaseItemID));
        }

        #endregion
    }
}
