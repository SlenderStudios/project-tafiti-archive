using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using WLQuickApps.SocialNetwork.Data;
using System.Text.RegularExpressions;

namespace WLQuickApps.SocialNetwork.Business
{
    /// <summary>
    /// Represents a user album.
    /// </summary>
    public abstract class BaseItem
    {
        #region Public Properties
        /// <summary>
        /// The ID of the album owner.
        /// </summary>
        public User Owner
        {
            get
            {
                if (this._user == null)
                {
                    this._user = UserManager.GetUser(this._ownerUserID);
                }

                return this._user;
            }
            set
            {
                this._ownerUserID = value.UserID;
                this._user = value;
            }
        }
        private User _user;

        protected Guid OwnerUserID
        {
            get { return this._ownerUserID; }
            set { this._ownerUserID = value; }
        }
        protected Guid _ownerUserID;


        private string _imageURL;

        public string ImageURL
        {
            get { return this._imageURL; }
            set { this._imageURL = value; }
        }

        /// <summary>
        /// The item's location.
        /// </summary>
        public Location Location
        {
            get
            {
                if (this._location == null)
                {
                    this._location = LocationManager.GetLocation(this._locationID);
                }

                return this._location;
            }
            set
            {
                this._location = value;
                this.Update();
            }
        }
        private Location _location;
        private Guid _locationID = Location.Empty.LocationID;

        public ReadOnlyCollection<Album> Albums
        {
            get { return AlbumManager.GetAlbumsByBaseItemID(this._baseItemID).AsReadOnly(); }
        }

        public ReadOnlyCollection<Collection> Collections
        {
            get { return CollectionManager.GetCollectionsByBaseItemID(this._baseItemID); }
        }

        public ReadOnlyCollection<Event> Events
        {
            get { return EventManager.GetEventsByBaseItemID(this._baseItemID); }
        }

        public ReadOnlyCollection<Tag> Tags
        {
            get { return TagManager.GetTags(this).AsReadOnly(); }
        }

        public ReadOnlyCollection<Comment> Comments
        {
            get { return CommentManager.GetComments(this._baseItemID).AsReadOnly(); }
        }

        public int TotalViews
        {
            get { return this._totalViews; }
        }
        private int _totalViews;

        public double TotalRatingScore
        {
            get { return this._totalRatingScore; }
        }
        private double _totalRatingScore;

        public int TotalRatingCount
        {
            get { return this._totalRatingCount; }
        }
        private int _totalRatingCount;

        public double AverageRating
        {
            get { return this._averageRating; }
        }
        private double _averageRating;

        /// <summary>
        /// The ID of this BaseItem.
        /// </summary>
        public int BaseItemID
        {
            get { return this._baseItemID; }
        }
        private int _baseItemID;

        public string SubType
        {
            get { return this._subType; }
        }
        private string _subType;

        public PrivacyLevel PrivacyLevel
        {
            get { return this._privacyLevel; }
            set { this._privacyLevel = value; }
        }
        private PrivacyLevel _privacyLevel;

        public bool HasThumbnail
        {
            get { return BaseItemManager.HasThumbnail(this); }
        }

        public string Title
        {
            get { return this._title; }
            set
            {
                if (string.IsNullOrEmpty(value)) { throw new ArgumentException("Title cannot be null or empty"); }
                this._title = value;
            }
        }
        private string _title;

        public string Description
        {
            get { return this._description; }
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this._description = value;
            }
        }
        private string _description;

        public DateTime CreateDate
        {
            get { return this._createDate; }
        }
        private DateTime _createDate;

        public bool IsApproved
        {
            get { return this._isApproved; }
        }
        private bool _isApproved;

        public int ThumbnailBaseItemID { get { return this.BaseItemID; } }

        #endregion

        #region Public Virtual Properties
        public virtual bool CanAccept { get { return false; } }
        public virtual bool CanApprove { get { return BaseItemManager.CanApproveBaseItem(this); } }
        public virtual bool CanAssociate { get { return false; } }
        public virtual bool CanCancel { get { return false; } }
        public virtual bool CanContribute { get { return BaseItemManager.CanContributeToBaseItem(this); } }
        public virtual bool CanDelete { get { return BaseItemManager.CanModifyBaseItem(this); } }
        public virtual bool CanEdit { get { return BaseItemManager.CanModifyBaseItem(this); } }
        public virtual bool CanJoin { get { return false; } }
        public virtual bool CanLeave { get { return false; } }
        public virtual bool CanView { get { return BaseItemManager.CanViewBaseItem(this); } }
        #endregion

        #region Constructors
        private BaseItem(BaseItemDataSet.BaseItemRow row)
        {
            this._averageRating = row.AverageRating;
            this._baseItemID = row.BaseItemID;
            this._createDate = row.CreateDate;
            this._description = row.Description;
            this._locationID = row.LocationID;
            this._ownerUserID = row.OwnerUserID;
            this._privacyLevel = (PrivacyLevel)row.PrivacyLevel;
            this._subType = (row.SubType == Constants.GroupTypes.GenericEvent) ? string.Empty : row.SubType;
            this._title = row.Title;
            this._totalRatingCount = row.TotalRatingCount;
            this._totalRatingScore = row.TotalRatingScore;
            this._totalViews = row.TotalViews;
            this._isApproved = row.IsApproved;
            this._imageURL = row.ImageURL;
        }

        internal BaseItem(int baseItemID) : this(BaseItemManager.GetBaseItemRow(baseItemID)) { }
        #endregion

        #region Public Methods
        public void IncrementViews()
        {
            this._totalViews++;
            BaseItemManager.SetTotalViews(this);
        }

        public void Rate(int rating)
        {
            BaseItemManager.RateBaseItem(this, rating);

            BaseItemDataSet.BaseItemRow row = BaseItemManager.GetBaseItemRow(this._baseItemID);
            this._averageRating = row.AverageRating;
            this._totalRatingCount = row.TotalRatingCount;
            this._totalRatingScore = row.TotalRatingScore;
        }

        public void AddComment(string comment)
        {
            CommentManager.CreateComment(this.BaseItemID, comment);
        }

        public void AddTag(string tag)
        {
            TagManager.AddTagToBaseItem(tag, this);
            this.Update();
        }

        public void RemoveTag(string tag)
        {
            TagManager.RemoveTagFromBaseItem(tag, this);
            this.Update();
        }

        public Image GetImage()
        {
            if (!this.HasThumbnail)
            {
                return null;
            }

            return Utilities.ConvertBytesToImage(BaseItemManager.GetRawThumbnailBits(this.BaseItemID));
        }

        public byte[] GetThumbnailBits(int maximumWidth, int maximumHeight)
        {
            if (!this.HasThumbnail)
            {
                return null;
            }

            return BaseItemManager.GetThumbnailBits(this.BaseItemID, maximumWidth, maximumHeight);
        }

        public Image GetThumbnail(int maximumWidth, int maximumHeight)
        {
            if (!this.HasThumbnail)
            {
                return null;
            }

            return BaseItemManager.GetThumbnail(this.BaseItemID, maximumWidth, maximumHeight);
        }

        public void SetThumbnail(byte[] bits)
        {
            BaseItemManager.SetThumbnail(this, bits);
        }
        #endregion

        public virtual void Accept() { throw new NotSupportedException(); }
        public virtual void Approve() { BaseItemManager.ApproveBaseItem(this); }
        public virtual void Associate(BaseItem baseItem) { throw new NotSupportedException(); }
        public virtual void Cancel() { throw new NotSupportedException(); }
        public virtual void Decline() { throw new NotSupportedException(); }
        public virtual void Delete() { BaseItemManager.DeleteBaseItem(this); }
        public virtual void Join() { throw new NotSupportedException(); }
        public virtual void Leave() { throw new NotSupportedException(); }
        public virtual void Update() { BaseItemManager.UpdateBaseItem(this); }

        public override bool Equals(object obj)
        {
            BaseItem otherBaseItem = obj as BaseItem;
            if (((object)otherBaseItem) == null) { return false; }

            return (this.BaseItemID == otherBaseItem.BaseItemID);
        }

        public override int GetHashCode()
        {
            return this.BaseItemID.GetHashCode();
        }

        public static bool operator ==(BaseItem first, BaseItem second)
        {
            if (((object)first) == null)
            {
                return (((object)second) == null);
            }

            return first.Equals(second);
        }

        public static bool operator !=(BaseItem first, BaseItem second)
        {
            return !(first == second);
        }

        protected abstract string GetSearchTerms();

        internal string GetSearchDataString()
        {
            List<string> searchData = new List<string>();
            StringBuilder searchTerms = new StringBuilder(this.GetSearchTerms());

            if (this.Location != Location.Empty)
            {
                searchTerms.AppendFormat(" {0}", this.Location.GetSearchTerms());
            }

            searchTerms.AppendFormat(" {0} {1}", this.Description, this.Title);

            foreach (Match match in Regex.Matches(searchTerms.ToString().ToLower(), @"\b([^\s,]+)\b", RegexOptions.Compiled))
            {
                string term = match.Groups[0].Value;

                if (!searchData.Contains(term))
                {
                    searchData.Add(term);
                }
            }

            foreach (Tag tag in this.Tags)
            {
                string tagName = tag.Name.ToLower();

                if (!searchData.Contains(tagName))
                {
                    searchData.Add(tagName);
                }
            }

            searchData.Sort();
            return String.Format(",{0},", String.Join(",,", searchData.ToArray()));
        }

    }
}
