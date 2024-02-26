using System;
using System.Collections.Generic;
using System.Text;
using WLQuickApps.SocialNetwork.Data;
using System.Collections.ObjectModel;

namespace WLQuickApps.SocialNetwork.Business
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User : BaseItem
    {
        /// <summary>
        /// Gets the user's unique ID.
        /// </summary>
        public Guid UserID
        {
            get { return this._userID; }
        }
        private Guid _userID;

        /// <summary>
        /// Gets the user's Windows Live ClientID (CID).
        /// </summary>
        public string WindowsLiveUUID
        {
            get { return this._windowsLiveUUID; }
        }
        private string _windowsLiveUUID;

        public string DomainAuthenticationToken
        {
            get { return this._domainAuthenticationToken; }
        }
        private string _domainAuthenticationToken;

        public string OwnerHandle
        {
            get { return _ownerHandle; }
        }
        private string _ownerHandle;

        /// <summary>
        /// Gets the user's username.
        /// </summary>
        public string UserName
        {
            get { return this._userName; }
        }
        private string _userName;

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public string Email
        {
            get { return this._email; }
            set 
            {
                if (string.IsNullOrEmpty(value)) { throw new ArgumentException("Email cannot be null or empty"); }

                this._email = value; 
            }
        }
        private string _email;

        /// <summary>
        /// Gets or sets user's first name.
        /// </summary>
        public string FirstName
        {
            get { return this._firstName; }
            set { this._firstName = value; }
        }
        private string _firstName;

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        public string LastName
        {
            get { return this._lastName; }
            set { this._lastName = value; }
        }
        private string _lastName;

        /// <summary>
        /// Gets or sets the user's gender.
        /// </summary>
        public Gender Gender
        {
            get { return this._gender; }
            set { this._gender = value; }
        }
        private Gender _gender;

        /// <summary>
        /// Gets or sets the user's date of birth.
        /// </summary>
        public DateTime DateOfBirth
        {
            get { return this._dateOfBirth; }
            set { this._dateOfBirth = value; }
        }
        private DateTime _dateOfBirth;

        /// <summary>
        /// Gets the date the user last logged in at.
        /// </summary>
        public DateTime LastLoginDate
        {
            get { return this._lastLoginDate; }
        }
        private DateTime _lastLoginDate;

        /// <summary>
        /// Gets/Sets the Messenger Presence ID (makes the Presence API and the IM Control work)
        /// see http://settings.messenger.live.com/applications/CreateHtml.aspx
        /// </summary>
        public string MessengerPresenceID
        {
            get { return _messengerPresenceID; }
            set { _messengerPresenceID = value; }
        }
        private string _messengerPresenceID;

        /// <summary>
        /// Gets a value indicating whether this user is an administrator.
        /// </summary>
        public bool IsAdmin
        {
            get { return this._isAdmin; }
        }
        private bool _isAdmin;

        /// <summary>
        /// Gets or sets the URL of this user's RSS feed (blog).
        /// </summary>
        public string RssFeedUrl
        {
            get { return this._rssFeedUrl; }
            set { this._rssFeedUrl = value; }
        }
        private string _rssFeedUrl;

        public new ReadOnlyCollection<Album> Albums
        {
            get
            {
                return AlbumManager.GetAlbumsByUserID(this.UserID).AsReadOnly();
            }
        }

        private PhotoToken _photoPermissionToken = null;
        public PhotoToken PhotoPermissionToken
        {
            get
            {
                if (_photoPermissionToken == null)
                {
                    if (_ownerHandle == null || _domainAuthenticationToken == null) return _photoPermissionToken;
                    if (_ownerHandle.Length < 4 || _domainAuthenticationToken.Length < 4) return _photoPermissionToken;

                    _photoPermissionToken = new PhotoToken(this.OwnerHandle, this.DomainAuthenticationToken);
                }
                return _photoPermissionToken;
            }
            set
            {
                _photoPermissionToken = value;

                if (_photoPermissionToken == null)
                {
                    _domainAuthenticationToken = string.Empty;
                    _ownerHandle = string.Empty;
                }
                else
                {
                    _domainAuthenticationToken = _photoPermissionToken.DomainAuthenticationToken;
                    _ownerHandle = _photoPermissionToken.OwnerHandle;
                }
            }
        }

        public override bool CanAccept 
        {
            get
            {
                if (!UserManager.IsUserLoggedIn()) { return false; }
                return FriendManager.LookupFriendRequest(this, UserManager.LoggedInUser); 
            }
        }
        public override bool CanCancel 
        {
            get
            {
                if (!UserManager.IsUserLoggedIn()) { return false; }
                return FriendManager.LookupFriendRequest(UserManager.LoggedInUser, this); 
            }
        }
        public override bool CanView { get { return UserManager.CanViewUser(this); } }

        public override bool CanJoin
        {
            get
            {
                if (!UserManager.IsUserLoggedIn()) { return false; }
                return FriendManager.CanAddFriend(this);
            }
        }

        public override bool CanLeave
        {
            get
            {
                return FriendManager.CanRemoveFriend(this);
            }
        }

        /// <summary>
        /// Initializes a User object based on a UserRow from the UserDataTable.
        /// </summary>
        /// <param name="row"></param>
        internal User(UserDataSet.UserRow row, bool isAdmin) : base(row.BaseItemID)
        {
            this.Initialize(row.UserID, row.WindowsLiveUUID, row.UserName, row.Email, row.FirstName,
                row.LastName, (Gender)row.Gender, row.DOB, row.BaseItemID, 
                row.CreateDate, row.LastLoginDate, row.RSSFeedURL, isAdmin, row.MessengerPresenceID, row.DomainToken, row.OwnerHandle);
        }

        /// <summary>
        /// User object based on user parameters.
        /// </summary>
        internal User(Guid userID, string windowsLiveUUID, string userName, string email, string firstName,
            string lastName, Gender gender, DateTime dateOfBirth, int baseItemID,
            DateTime createDate, DateTime lastLoginDate, string rssFeedURL, bool isAdmin, string messengerPresenceID, string domainToken, string ownerHandle)
            : base(baseItemID)
        {
            this.Initialize(userID, windowsLiveUUID, userName, email, firstName, lastName, gender, dateOfBirth,
                baseItemID, createDate, lastLoginDate, rssFeedURL, isAdmin, messengerPresenceID, domainToken, ownerHandle);
        }

        private void Initialize(Guid userID, string windowsLiveUUID, string userName, string email, string firstName,
            string lastName, Gender gender, DateTime dateOfBirth, int baseItemID,
            DateTime createDate, DateTime lastLoginDate, string rssFeedURL, bool isAdmin, string messengerPresenceID, string domainToken, string ownerHandle)
        {
            this._userID = userID;
            this._windowsLiveUUID = windowsLiveUUID;
            this._userName = userName;
            this._email = email;
            this._firstName = firstName;
            this._lastName = lastName;
            this._gender = gender;
            this._dateOfBirth = dateOfBirth;
            this._lastLoginDate = lastLoginDate;
            this._rssFeedUrl = rssFeedURL;
            this._isAdmin = isAdmin;
            this._messengerPresenceID = messengerPresenceID;
            this._domainAuthenticationToken = domainToken;
            _ownerHandle = ownerHandle;
        }

        public override void Delete() { UserManager.DeleteUser(this); }
        public override void Accept() { FriendManager.AddFriend(this); }
        public override void Decline() { FriendManager.RemoveFriendship(this); }
        public override void Cancel() { FriendManager.RemoveFriendship(this); }
        public override void Join() { FriendManager.AddFriend(this); }
        public override void Leave() { FriendManager.RemoveFriendship(this); }
        public override void Update() { UserManager.UpdateUser(this); }
        
        public void JoinEvent(Event eventToJoin)
        {
            GroupManager.AddUserToGroup(this, eventToJoin);
        }

        protected override string GetSearchTerms()
        {
            return string.Format("{0} {1} {2}", this.UserName, this.FirstName, this.LastName);
        }
    }
}
