using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Transactions;
using System.Security;

using WLQuickApps.SocialNetwork.Business.Properties;
using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.MediaDataSetTableAdapters;
using WLQuickApps.SocialNetwork.Data.BaseItemDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class MediaManager
    {
        static private List<Media> GetMediaFromTable(MediaDataSet.MediaDataTable mediaDataTable)
        {
            List<Media> list = new List<Media>();
            foreach (MediaDataSet.MediaRow row in mediaDataTable)
            {
                Media media = new Media(row);
                if (!media.CanView) { continue; }
                list.Add(media);
            }
            return list;
        }

        static public Media CreateAudio(string fileName, byte[] bits, int albumBaseItemID, string title, string description)
        {
            return MediaManager.CreateMedia(bits, albumBaseItemID, MediaType.Audio, title, description, Location.Empty, fileName, string.Empty);
        }

        static public Media CreateAudio(string fileName, byte[] bits, int albumBaseItemID, string title, string description, Location location)
        {
            return MediaManager.CreateMedia(bits, albumBaseItemID, MediaType.Audio, title, description, location, fileName, string.Empty);
        }

        static public Media CreateVideo(string fileName, byte[] bits, int albumBaseItemID, string title, string description)
        {
            return MediaManager.CreateMedia(bits, albumBaseItemID, MediaType.Video, title, description, Location.Empty, fileName, string.Empty);
        }

        static public Media CreateVideo(string fileName, byte[] bits, int albumBaseItemID, string title, string description, Location location)
        {
            return MediaManager.CreateMedia(bits, albumBaseItemID, MediaType.Video, title, description, location, fileName, string.Empty);
        }

        static public Media CreatePicture(byte[] bits, int albumBaseItemID, string title, string description)
        {
            return MediaManager.CreateMedia(bits, albumBaseItemID, MediaType.Picture, title, description, Location.Empty, string.Empty, string.Empty);
        }

        static public Media CreatePicture(byte[] bits, int albumBaseItemID, string title, string description, Location location)
        {
            return MediaManager.CreateMedia(bits, albumBaseItemID, MediaType.Picture, title, description, location, string.Empty, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="albumBaseItemID"></param>
        /// <param name="AlbumName"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        static public Media CreatePicture(byte[] bits, int albumBaseItemID, string AlbumName, string title, string description, Location location, string filename, string imageURL)
        {
            return MediaManager.CreatePictureInner(bits, albumBaseItemID, AlbumName, title, description, location, filename, imageURL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bits"></param>
        /// <param name="albumBaseItemID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        static public Media CreateFile(string fileName, byte[] bits, int albumBaseItemID, string title, string description)
        {
            return MediaManager.CreateMedia(bits, albumBaseItemID, MediaType.File, title, description, Location.Empty, fileName, string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="bits"></param>
        /// <param name="albumBaseItemID"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        static public Media CreateFile(string fileName, byte[] bits, int albumBaseItemID, string title, string description, Location location, string imageURL)
        {
            if (!fileName.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException("Only zip files may be uploaded to Silverlight Streaming");
            }

            return MediaManager.CreateMedia(bits, albumBaseItemID, MediaType.File, title, description, location, fileName, imageURL);
        }

        /// <summary>
        /// Creates media
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="albumBaseItemID"></param>
        /// <param name="mediaType"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="location"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static public Media CreateMedia(byte[] bits, int albumBaseItemID, MediaType mediaType, string title,
            string description, Location location, string fileName, string imageURL)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            if (bits == null) { throw new ArgumentNullException("bits"); }
            if (string.IsNullOrEmpty(title)) { throw new ArgumentException("Caption cannot be null or empty"); }
            if (description == null) { description = string.Empty; }

            Album album = AlbumManager.GetAlbum(albumBaseItemID);
            AlbumManager.VerifyOwnerActionOnAlbum(album);

            Media media;
            // Get the user reference we need before starting the transaction
            // to avoid deadlocks.
            User user = UserManager.LoggedInUser;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                int baseItemID = BaseItemManager.CreateBaseItem(Constants.BaseItemTypes.Media, location, title, description, user,
                    MediaManager.ConvertMediaTypeToString(mediaType), PrivacyLevel.Public, SettingsWrapper.AutomaticallyApproveNewMedia, imageURL);
                mediaTableAdapter.CreateMedia(albumBaseItemID, baseItemID);

                byte[] thumbnailBits = new byte[0];
                switch (mediaType)
                {
                    case MediaType.Video: Utilities.SendMediaToProcessorQueue(Utilities.GenerateSilverlightID(baseItemID), fileName, mediaType, bits, ref thumbnailBits); break;
                    case MediaType.Audio: Utilities.SendMediaToProcessorQueue(Utilities.GenerateSilverlightID(baseItemID), fileName, mediaType, bits, ref thumbnailBits); break;
                    case MediaType.File: Utilities.UploadPackageToSilverlightStreaming(Utilities.GenerateSilverlightID(baseItemID), bits); break;
                }

                media = MediaManager.GetMedia(baseItemID);
                media.Update();

                if (media.MediaType == MediaType.Picture)
                {
                    thumbnailBits = bits;
                }

                if (thumbnailBits.Length > 0)
                {
                    media.SetThumbnail(thumbnailBits);

                }

                transactionScope.Complete();
            }

            return media;
        }


        static private Media CreatePictureInner(byte[] bits, int albumBaseItemID, string AlbumName, string title,
            string description, Location location, string filename, string imageURL)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            if (bits == null) { throw new ArgumentNullException("bits"); }
            if (string.IsNullOrEmpty(title)) { throw new ArgumentException("Caption cannot be null or empty"); }
            if (description == null) { description = string.Empty; }

            Album album = AlbumManager.GetAlbum(albumBaseItemID);
            AlbumManager.VerifyOwnerActionOnAlbum(album);

            Media media;
            // Get the user reference we need before starting the transaction
            // to avoid deadlocks.
            User user = UserManager.LoggedInUser;
            int baseItemID = 0;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                baseItemID = BaseItemManager.CreateBaseItem(Constants.BaseItemTypes.Media, location, title, description, user,
                    MediaManager.ConvertMediaTypeToString(MediaType.Picture), PrivacyLevel.Public, SettingsWrapper.AutomaticallyApproveNewMedia, imageURL);
                mediaTableAdapter.CreateMedia(albumBaseItemID, baseItemID);

                byte[] thumbnailBits = new byte[0];

                media = MediaManager.GetMedia(baseItemID);
                
                media.ImageURL = MediaUtil.UploadPictureToSpaces(UserManager.LoggedInUser, media, AlbumName, bits, filename);
                
                media.Update();

                transactionScope.Complete();
            }

            return media;
        }

        static public Media CreatePicture(int albumBaseItemID, string AlbumName, string title, string description, string imageURL)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            if (string.IsNullOrEmpty(title)) { throw new ArgumentException("Caption cannot be null or empty"); }
            if (description == null) { description = string.Empty; }

            Album album = AlbumManager.GetAlbum(albumBaseItemID);
            AlbumManager.VerifyOwnerActionOnAlbum(album);

            Media media;
            // Get the user reference we need before starting the transaction
            // to avoid deadlocks.
            User user = UserManager.LoggedInUser;
            int baseItemID = 0;
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                baseItemID = BaseItemManager.CreateBaseItem(Constants.BaseItemTypes.Media, Location.Empty, title, description, user,
                    MediaManager.ConvertMediaTypeToString(MediaType.Picture), PrivacyLevel.Public, SettingsWrapper.AutomaticallyApproveNewMedia, imageURL);
                mediaTableAdapter.CreateMedia(albumBaseItemID, baseItemID);
                media = MediaManager.GetMedia(baseItemID);
                media.Update();
                transactionScope.Complete();
            }

            return media;
        }



        static public bool MediaExists(int baseItemID)
        {
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return (mediaTableAdapter.GetMedia(baseItemID).Rows.Count != 0);
            }
        }

        /// <summary>
        /// Retrieves media from the database (without picture image and thumbnail)
        /// </summary>
        /// <param name="baseItemID"></param>
        /// <returns></returns>
        static public Media GetMedia(int baseItemID)
        {
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                MediaDataSet.MediaDataTable mediaTable = mediaTableAdapter.GetMedia(baseItemID);

                if (mediaTable.Rows.Count != 1)
                {
                    throw new ArgumentException("The media with the specified ID does not exist");
                }

                Media media = new Media(mediaTable[0]);
                if (!media.CanView) { throw new SecurityException("The current user does not have permission to access this item"); }
                return media;
            }
        }

        /// <summary>
        /// Gets the number of media belonging to an album.
        /// </summary>
        /// <param name="albumBaseItemID"></param>
        /// <returns></returns>
        static public int GetMediaCountByAlbum(int albumBaseItemID)
        {
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return (int)mediaTableAdapter.GetMediaByAlbumBaseItemIDCount(albumBaseItemID);
            }
        }

        /// <summary>
        /// Gets the number of media of a certain type belonging to an album.
        /// </summary>
        /// <param name="albumBaseItemID"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        static public int GetMediaCountByAlbum(int albumBaseItemID, MediaType mediaType)
        {
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return (int)mediaTableAdapter.GetMediaOfTypeByAlbumBaseItemIDCount(albumBaseItemID, MediaManager.ConvertMediaTypeToString(mediaType));
            }
        }

        /// <summary>
        /// Updates media.
        /// </summary>
        /// <param name="media"></param>
        static public void UpdateMedia(Media media)
        {
            if (media == null) { throw new ArgumentNullException("media"); }

            MediaManager.VerifyOwnerActionOnMedia(media);

            BaseItemManager.UpdateBaseItem(media);

            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                mediaTableAdapter.UpdateMedia(media.ParentAlbum.BaseItemID, media.BaseItemID);
                media.Update();
            }
        }

        /// <summary>
        /// Deletes media.
        /// </summary>
        /// <param name="baseItemID"></param>
        static internal void DeleteMedia(Media media)
        {
            MediaManager.VerifyOwnerActionOnMedia(media);

            BaseItemManager.DeleteBaseItem(media);
        }

        static public List<Media> GetMediaByAlbumBaseItemID(int albumBaseItemID)
        {
            return MediaManager.GetMediaByAlbumBaseItemID(albumBaseItemID, 0, MediaManager.GetMediaByAlbumBaseItemIDCount(albumBaseItemID));
        }

        static public int GetMediaByAlbumBaseItemIDCount(int albumBaseItemID)
        {
            // This is just here to verify that the album exists.
            Album album = AlbumManager.GetAlbum(albumBaseItemID);

            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return (int)mediaTableAdapter.GetMediaByAlbumBaseItemIDCount(albumBaseItemID);
            }
        }

        static public List<Media> GetMediaByAlbumBaseItemID(int albumBaseItemID, int startRowIndex, int maximumRows)
        {
            // This is just here to verify that the album exists.
            Album album = AlbumManager.GetAlbum(albumBaseItemID);

            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return MediaManager.GetMediaFromTable(mediaTableAdapter.GetMediaByAlbumBaseItemID(albumBaseItemID, startRowIndex, maximumRows));
            }
        }

        static public List<Media> GetMediaOfTypeByAlbumBaseItemID(int albumBaseItemID, MediaType mediaType)
        {
            return MediaManager.GetMediaOfTypeByAlbumBaseItemID(albumBaseItemID, mediaType, 0, MediaManager.GetMediaOfTypeByAlbumBaseItemIDCount(albumBaseItemID, mediaType));
        }

        static public int GetMediaOfTypeByAlbumBaseItemIDCount(int albumBaseItemID, MediaType mediaType)
        {
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return (int)mediaTableAdapter.GetMediaOfTypeByAlbumBaseItemIDCount(albumBaseItemID, MediaManager.ConvertMediaTypeToString(mediaType));
            }
        }

        static public List<Media> GetMediaOfTypeByAlbumBaseItemID(int albumBaseItemID, MediaType mediaType, int startRowIndex, int maximumRows)
        {
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return MediaManager.GetMediaFromTable(mediaTableAdapter.GetMediaOfTypeByAlbumBaseItemID(albumBaseItemID, MediaManager.ConvertMediaTypeToString(mediaType), startRowIndex, maximumRows));
            }
        }

        static public List<Media> GetMediaForLocation(Location location)
        {
            return MediaManager.GetMediaForLocation(location, 0, MediaManager.GetMediaForLocationCount(location));
        }

        static public List<Media> GetMediaForLocation(Location location, int startRowIndex, int maximumRows)
        {
            if (location == null) { throw new ArgumentNullException("location"); }

            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return MediaManager.GetMediaFromTable(mediaTableAdapter.GetMediaForLocation(location.LocationID,
                   startRowIndex, maximumRows));
            }
        }

        static public List<Media> GetMediaByAlbumAndImageURL(Album _Album, string _ImageURL)
        {
            if (_Album == null) { throw new ArgumentNullException("_Album"); }

            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return MediaManager.GetMediaFromTable(mediaTableAdapter.GetMediaByAlbumBaseItemIDAndImageURL(_ImageURL, _Album.BaseItemID));
            }
        }

        static public int GetMediaForLocationCount(Location location)
        {
            if (location == null) { throw new ArgumentNullException("location"); }

            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return (int)mediaTableAdapter.GetMediaForLocationCount(location.LocationID);
            }
        }

        static public int GetTotalMediaCount(MediaType mediaType)
        {
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return (int)mediaTableAdapter.GetTotalMediaCount(MediaManager.ConvertMediaTypeToString(mediaType));
            }
        }

        static public List<Media> GetMostRecentMedia(MediaType mediaType)
        {
            // TODO: Determine how many media to return as "most recent".
            return MediaManager.GetMostRecentMedia(mediaType, 0, 16);
        }

        static public List<Media> GetMostRecentMedia(MediaType mediaType, int startRowIndex, int maximumRows)
        {
            using (MediaTableAdapter mediaTableAdapter = new MediaTableAdapter())
            {
                return MediaManager.GetMediaFromTable(mediaTableAdapter.GetMostRecentMedia(MediaManager.ConvertMediaTypeToString(mediaType), startRowIndex, maximumRows));
            }
        }

        static public int GetMostRecentMediaCount(MediaType mediaType)
        {
            //TODO: Add actual count return code
            return GetMostRecentMedia(mediaType, 0, 24).Count;
        }

        static internal void VerifyOwnerActionOnMedia(Media media)
        {
            if (!MediaManager.CanModifyMedia(media.BaseItemID))
            {
                throw new SecurityException("Media cannot be modified because it does not belong to the logged in user");
            }
        }

        static public bool CanModifyMedia(int baseItemID)
        {
            Media media = MediaManager.GetMedia(baseItemID);
            return AlbumManager.CanModifyAlbum(media.ParentAlbum.BaseItemID);
        }

        static internal bool CanModifyMedia(Media media)
        {
            return AlbumManager.CanModifyAlbum(media.ParentAlbum.BaseItemID);
        }

        static internal MediaType GetMediaTypeFromString(string mediaType)
        {
            if (mediaType == null) { return MediaType.File; }

            switch (mediaType)
            {
                case Constants.MediaTypes.Picture: return MediaType.Picture;
                case Constants.MediaTypes.Audio: return MediaType.Audio;
                case Constants.MediaTypes.Video: return MediaType.Video;
                default: return MediaType.File;
            }
        }

        static internal string ConvertMediaTypeToString(MediaType mediaType)
        {
            switch (mediaType)
            {
                case MediaType.Picture: return Constants.MediaTypes.Picture;
                case MediaType.Audio: return Constants.MediaTypes.Audio;
                case MediaType.Video: return Constants.MediaTypes.Video;
                default: return Constants.MediaTypes.File;
            }
        }

    }
}