using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using WLQuickApps.Tafiti.Data;

namespace WLQuickApps.Tafiti.Business
{
    public class ShelfStack
    {
        public Guid ShelfStackID { get { return this._shelfStackID; } }
        private Guid _shelfStackID;

        public string Label 
        {
            get { return this._label; } 
            set 
            {
                if (value == null) { value = string.Empty; }
                this._label = value;
            } 
        }
        private string _label;

        public DateTime LastModifiedTimestamp { get { return this._lastModifiedTimestamp; } }
        private DateTime _lastModifiedTimestamp;

        public ReadOnlyCollection<Comment> Conversation
        {
            get
            {
                if (this._conversation == null)
                {
                    this._conversation = ConversationManager.GetCommentsByShelf(this.ShelfStackID);
                }
                return this._conversation;
            }
        }
        private ReadOnlyCollection<Comment> _conversation;

        public ReadOnlyCollection<ShelfStackItem> ShelfStackItems
        {
            get
            {
                return ShelfStackItemManager.GetShelfStackItemsForShelf(this);
            }
        }

        public ReadOnlyCollection<User> Owners
        {
            get
            {
                return UserManager.GetShelfOwners(this.ShelfStackID);
            }
        }

        
        public ShelfStack(ShelfStackDataSet.ShelfStackRow row)
        {
            this._shelfStackID = row.ShelfStackID;
            this._label = row.Label;
            this._lastModifiedTimestamp = row.LastModifiedTimestamp;
        }

    }
}
