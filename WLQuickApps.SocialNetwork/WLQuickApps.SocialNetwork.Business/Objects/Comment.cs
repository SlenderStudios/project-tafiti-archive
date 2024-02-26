using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;

namespace WLQuickApps.SocialNetwork.Business
{
    public class Comment : IComparable
    {
        public int CommentID
        {
            get { return this._commentID; }
        }
        private int _commentID;

        public int BaseItemID
        {
            get { return this._baseItemID; }
        }
        private int _baseItemID;

        public User UserItem
        {
            get 
            {
                if (this._userItem == null)
                {
                    this._userItem = UserManager.GetUser(this._userID);
                }
                return this._userItem;
            }
        }
        private Guid _userID;
        private User _userItem;

        public string Text
        {
            get { return this._text; }
        }
        private string _text;

        public DateTime PostDateTime
        {
            get { return this._postDateTime; }
        }
        private DateTime _postDateTime;

        internal Comment(int commentID, int baseItemID, Guid userID, string text, DateTime postDateTime)
        {
            this._commentID = commentID;
            this._baseItemID = baseItemID;
            this._userID = userID;
            this._text = text;
            this._postDateTime = postDateTime;
        }

        internal Comment(CommentDataSet.CommentRow row)
            : this(row.CommentID, row.BaseItemID, row.UserID, row.Text, row.PostDateTime)
        { 
        }

        public override bool Equals(object obj)
        {
            Comment otherComment = obj as Comment;
            if (((object)otherComment) == null) { return false; }

            return (this.CommentID == otherComment.CommentID);
        }

        public override int GetHashCode()
        {
            return this.CommentID.GetHashCode();
        }

        public static bool operator ==(Comment first, Comment second)
        {
            if (((object)first) == null)
            {
                return (((object)second) == null);
            }

            return first.Equals(second);
        }

        public static bool operator !=(Comment first, Comment second)
        {
            return !(first == second);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            Comment other = obj as Comment;
            if (other == null)
            {
                throw new ArgumentException("Other object is not a Comment.");
            }
            return this.PostDateTime.CompareTo(other.PostDateTime);
        }

        #endregion
    }
}
