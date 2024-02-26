using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.Tafiti.Data;

namespace WLQuickApps.Tafiti.Business
{
    public class Comment
    {
        public int CommentID { get { return this._commentID; } }
        private int _commentID;

        public string Text { get { return this._text; } }
        private string _text;

        public User Owner 
        {
            get
            {
                if (this._owner == null)
                {
                    this._owner = UserManager.GetUserByID(this._userID);
                }
                return this._owner;
            }
        }
        private User _owner;
        private string _userID;

        public ShelfStack ParentShelf
        {
            get
            {
                if (this._parentShelf == null)
                {
                    this._parentShelf = ShelfStackManager.GetShelfStackByID(this._parentShelfID);
                }
                return this._parentShelf;
            }
        }
        private ShelfStack _parentShelf;
        private Guid _parentShelfID;

        public DateTime Timestamp { get { return this._timestamp; } }
        private DateTime _timestamp;

        public Comment(ConversationDataSet.ConversationsRow row)
        {
            this._commentID = row.CommentID;
            this._parentShelfID = row.ShelfStackID;
            this._text = row.Text;
            this._timestamp = row.Timestamp;
            this._userID = row.UserID;
        }

    }
}
