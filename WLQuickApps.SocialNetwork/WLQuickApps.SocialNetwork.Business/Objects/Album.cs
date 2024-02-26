using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using WLQuickApps.SocialNetwork.Data;

namespace WLQuickApps.SocialNetwork.Business
{
    /// <summary>
    /// Represents a user album.
    /// </summary>
    public class Album : BaseItem
    {
        public ReadOnlyCollection<Media> Pictures
        {
            get
            {
                if (this._pictures == null)
                {
                    this._pictures = MediaManager.GetMediaOfTypeByAlbumBaseItemID(this.BaseItemID, MediaType.Picture).AsReadOnly();
                }

                return this._pictures;
            }
        }
        private ReadOnlyCollection<Media> _pictures;

        public ReadOnlyCollection<Media> Audios
        {
            get
            {
                if (this._audios == null)
                {
                    this._audios = MediaManager.GetMediaOfTypeByAlbumBaseItemID(this.BaseItemID, MediaType.Audio).AsReadOnly();
                }

                return this._audios;
            }
        }
        private ReadOnlyCollection<Media> _audios;

        public ReadOnlyCollection<Media> Videos
        {
            get
            {
                if (this._videos == null)
                {
                    this._videos = MediaManager.GetMediaOfTypeByAlbumBaseItemID(this.BaseItemID, MediaType.Video).AsReadOnly();
                }

                return this._videos;
            }
        }
        private ReadOnlyCollection<Media> _videos;

        public override void Delete() { AlbumManager.DeleteAlbum(this); }

        /// <summary>
        /// Constructor for Album object based on an AlbumRow from the AlbumDataTable.
        /// </summary>
        /// <param name="row"></param>
        internal Album(AlbumDataSet.AlbumRow row) : base(row.BaseItemID)
        {
        }

        protected override string GetSearchTerms()
        {
            return string.Empty;
        }
    }
}
