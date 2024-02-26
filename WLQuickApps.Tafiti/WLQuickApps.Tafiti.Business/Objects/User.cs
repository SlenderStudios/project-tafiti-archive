using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.Tafiti.Data;

namespace WLQuickApps.Tafiti.Business
{
    public class User
    {
        
        public string UserID { get { return this._userID; } }
        private string _userID;

        public string EmailHash 
        {
            get { return this._emailHash; } 
            set { this._emailHash = value; } 
        }
        private string _emailHash;

        public string DisplayName 
        {
            get { return this._displayName; } 
            set { this._displayName = value; } 
        }
        private string _displayName;

        public string MessengerPresenceID 
        {
            get { return this._messengerPresenceID; } 
            set { this._messengerPresenceID = value; } 
        }
        private string _messengerPresenceID;

        public int EmailCount 
        {
            get { return this._emailCount; }
            set { this._emailCount = value; }
        }
        private int _emailCount;

        public DateTime EmailCountTimestamp 
        {
            get { return this._emailCountTimestamp; }
            set { this._emailCountTimestamp = value; }
        }
        private DateTime _emailCountTimestamp;

        public DateTime LastLoginTimestamp 
        {
            get { return this._lastLoginTimestamp; } 
            set { this._lastLoginTimestamp = value; } 
        }
        private DateTime _lastLoginTimestamp;

        public bool AlwaysSendMessages
        {
            get { return this._alwaysSendMessages; }
            set { this._alwaysSendMessages = value; }
        }
        private bool _alwaysSendMessages;

        public User(UserDataSet.UsersRow row)
        {
            this._alwaysSendMessages = row.AlwaysSendMessages;
            this._displayName = row.DisplayName;
            this._emailCount = row.EmailCount;
            this._emailCountTimestamp = row.EmailCountTimestamp;
            this._emailHash = row.EmailHash;
            this._lastLoginTimestamp = row.LastLogin;
            this._messengerPresenceID = row.MessengerPresenceID;
            this._userID = row.UserID;
        }

        public override bool Equals(object obj)
        {
            User otherUser = obj as User;
            if (((object)otherUser) == null) { return false; }

            return (this.UserID == otherUser.UserID);
        }

        public override int GetHashCode()
        {
            return this.UserID.GetHashCode();
        }

        public static bool operator ==(User first, User second)
        {
            if (((object)first) == null)
            {
                return (((object)second) == null);
            }

            return first.Equals(second);
        }

        public static bool operator !=(User first, User second)
        {
            return !(first == second);
        }

    }
}
