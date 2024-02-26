using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.MediaDataSetTableAdapters;
using System.Drawing;

namespace WLQuickApps.SocialNetwork.Business
{

    /// <summary>
    /// Represents a picture without the image.
    /// </summary>
    public class Media : BaseItem, IComparable<Media>
    {
        /// <summary>
        /// The album the media belongs to.
        /// </summary>
        public Album ParentAlbum
        {
            get 
            {
                if (this._parentAlbum == null)
                {
                    this._parentAlbum = AlbumManager.GetAlbum(this._albumID);
                }
                return this._parentAlbum;
            }
        }
        private int _albumID;
        private Album _parentAlbum;

        public MediaType MediaType
        {
            get { return MediaManager.GetMediaTypeFromString(this.SubType); }
        }

        /// <summary>
        /// Constructor for Picture object based on a PicturesRow from the PicturesDataTable.
        /// </summary>
        /// <param name="row"></param>
        internal Media(MediaDataSet.MediaRow row) : base(row.BaseItemID)
        {
            this._albumID = row.AlbumBaseItemID;
        }

        protected override string GetSearchTerms()
        {
            return String.Format("{0} {1} {2}", this.Title, this.Description, this.Owner.UserName);
        }

        #region IComparable<Media> Members

        public int CompareTo(Media other)
        {
            if (other == null) { return 1; }

            return this.AverageRating.CompareTo(other.AverageRating);
        }

        #endregion
    }
}
