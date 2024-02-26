using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Transactions;
using System.Web.Security;
using System.Web;

using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.AlbumDataSetTableAdapters;


namespace WLQuickApps.SocialNetwork.Business
{
    /// <summary>
    /// Provides data access methods for an album. 
    /// </summary>
    public static class AlbumManager
    {

        static private List<Album> GetAlbumsFromTable(AlbumDataSet.AlbumDataTable albumDataTable)
        {
            List<Album> list = new List<Album>();
            foreach (AlbumDataSet.AlbumRow row in albumDataTable)
            {
                Album album = new Album(row);
                if (!album.CanView) { continue; }
                list.Add(album);
            }
            return list;
        }

        /// <summary>
        /// Creates an album in the database.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Album CreateAlbum(string name)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("Albums must have a name"); }

            using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
            {
                int baseItemID = BaseItemManager.CreateBaseItem(Constants.BaseItemTypes.Album, Location.Empty, name, string.Empty, UserManager.LoggedInUser,
                    string.Empty, PrivacyLevel.Public, true, string.Empty);
                albumTableAdapter.CreateAlbum(baseItemID);
                return AlbumManager.GetAlbum(baseItemID);
            }
        }

        static public bool AlbumExists(int baseItemID)
        {
            using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
            {
                return (albumTableAdapter.GetAlbum(baseItemID).Rows.Count != 0);
            }
        }

        /// <summary>
        /// Retrieves an album from the database.
        /// </summary>
        /// <param name="albumID"></param>
        /// <returns></returns>
        public static Album GetAlbum(int baseItemID)
        {
            using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
            {
                AlbumDataSet.AlbumDataTable albumTable = albumTableAdapter.GetAlbum(baseItemID);

                if (albumTable.Rows.Count != 1)
                {
                    throw new ArgumentException("The album with the specified ID does not exist");
                }

                Album album = new Album(albumTable[0]);
                if (!album.CanView) { throw new SecurityException("The current user does not have permission to access this item"); }
                return album;
            }
        }

        /// <summary>
        /// Deletes an album along with the videos and photos belonging to it.
        /// </summary>
        /// <param name="albumID"></param>
        internal static void DeleteAlbum(Album album)
        {
            AlbumManager.VerifyOwnerActionOnAlbum(album);

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            {
                BaseItemManager.DeleteBaseItem(album);

                using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
                {
                    albumTableAdapter.DeleteAlbum(album.BaseItemID);
                }

                transactionScope.Complete();
            }
        }

        /// <summary>
        /// Updates an album.
        /// </summary>
        /// <param name="album"></param>
        public static void UpdateAlbum(Album album)
        {
            if (album == null) { throw new ArgumentNullException("album"); }

            AlbumManager.VerifyOwnerActionOnAlbum(album);

            BaseItemManager.UpdateBaseItem(album);
        }

        /// <summary>
        /// Gets the number of albums belonging to a user.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        static public int GetAlbumsByUserIDCount(Guid userID)
        {
            using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
            {
                return (int)albumTableAdapter.GetAlbumsByUserIDCount(userID);
            }
        }

        static public List<Album> GetAlbumsByUserID(Guid userID)
        {
            return AlbumManager.GetAlbumsByUserID(userID, 0, AlbumManager.GetAlbumsByUserIDCount(userID));
        }

        static public List<Album> GetAlbumsByUserID(Guid userID, int startRowIndex, int maximumRows)
        {
            using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
            {
                return AlbumManager.GetAlbumsFromTable(albumTableAdapter.GetAlbumsByUserID(userID, startRowIndex, maximumRows));
            }
        }

        static public List<Album> GetAllAlbums()
        {
            return AlbumManager.GetAllAlbums(0, AlbumManager.GetAllAlbumsCount());
        }

        static public int GetAllAlbumsCount()
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return AlbumManager.GetAlbumsByUserIDCount(UserManager.LoggedInUser.UserID);
        }

        static public List<Album> GetAllAlbums(int startRowIndex, int maximumRows)
        {
            UserManager.AssertThatAUserIsLoggedIn();
            return AlbumManager.GetAlbumsByUserID(UserManager.LoggedInUser.UserID, startRowIndex, maximumRows);
        }

        static public List<Album> GetAlbumsByBaseItemID(int baseItemID)
        {
            return AlbumManager.GetAlbumsByBaseItemID(baseItemID, 0, GetAlbumsByBaseItemIDCount(baseItemID));
        }

        static public int GetAlbumsByBaseItemIDCount(int baseItemID)
        {
            using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
            {
                return (int)albumTableAdapter.GetAlbumsByBaseItemIDCount(baseItemID);
            }
        }

        static public List<Album> GetAlbumsByBaseItemID(int baseItemID, int startRowIndex, int maximumRows)
        {
            using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
            {
                return AlbumManager.GetAlbumsFromTable(albumTableAdapter.GetAlbumsByBaseItemID(baseItemID, startRowIndex, maximumRows));
            }
        }

        static internal void VerifyOwnerActionOnAlbum(Album album)
        {
            if (!AlbumManager.CanModifyAlbum(album.BaseItemID))
            {
                throw new SecurityException("Album cannot be modified because it does not belong to the logged in user");
            }
        }

        static public void ClearOrphanURL(int baseItemID)
        {
            using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
            {
                albumTableAdapter.ClearOrphanURL(baseItemID);
            }
        }

        static public void ClearOrphanURL()
        {
            using (AlbumTableAdapter albumTableAdapter = new AlbumTableAdapter())
            {
                albumTableAdapter.ClearOrphanURLEx();
            }
        }

        static public bool CanModifyAlbum(int baseItemID)
        {
            Album album = AlbumManager.GetAlbum(baseItemID);
            return (UserManager.IsUserLoggedIn() &&
                ((album.Owner == UserManager.LoggedInUser) || UserManager.LoggedInUser.IsAdmin));
        }

        /// <summary>
        /// Check if an album exists in the list of albums passed in;
        /// if it doesn't exist create it and return the album;
        /// if it does exist return the album.
        /// </summary>
        /// <param name="albumName"></param>
        /// <param name="_Albums"></param>
        /// <returns></returns>
        static public Album CreateAlbumIfDoesNotExist(string albumName, List<WLQuickApps.SocialNetwork.Business.Album> _Albums, bool PermitCreation)
        {
            // enumerate the albums in the database
            foreach (WLQuickApps.SocialNetwork.Business.Album _album in _Albums)
            {
                // was the album name found?
                if (_album.Title == albumName)
                {
                    // yes - return the album
                    return (_album);
                }
            }
            if (PermitCreation) return AlbumManager.CreateAlbum(albumName);// no album was found so create & return it
             return null; //only import specified albums
            
        }

        /// <summary>
        /// DELETE PICTURES LOCALLY (IF IMAGE URL EXISTS) WHICH DONT EXIST IN SPACES
        /// </summary>
        /// <param name="_Album"></param>
        /// <param name="photolst"></param>
        static private void LocalErasePhoto(WLQuickApps.SocialNetwork.Business.Album _Album, LiveNet.Photos.Photo[] photolst)
        {
            // Enumerate the pictures in the local database to check if they exist in spaces
            // if they don't exist we'll delete it from the local db.
            foreach (Media _Picture in _Album.Pictures)
            {
                // was the pic found in Spaces?
                bool LocalPhotoFoundInSpaces = false;

                // is this an attached image to Spaces?
                if (_Picture.ImageURL != string.Empty && photolst != null)
                {
                    // Yes

                    // Enumerate the Spaces photos looking for the image
                    foreach (LiveNet.Photos.Photo _photo in photolst)
                    {
                        // if we haven't already found it
                        if (!LocalPhotoFoundInSpaces)
                        {
                            // is the spaces photo URL the same as this image from the local DB?
                            if (_photo.PhotoUrl.ToString().ToLower() == _Picture.ImageURL.ToLower())
                            {
                                // Yes - mark as found
                                LocalPhotoFoundInSpaces = true;
                                break;
                            }
                        }
                    }

                    // if the photo is in the local DB but not in spaces - remove it from local.
                    if (!LocalPhotoFoundInSpaces)
                    {
                        // delete the picture
                        _Picture.Delete();
                    }
                }
            }
        }


        /// <summary>
        /// Removes orphaned (in local db but not in Spaces) photos either bulk or one at a time
        /// creates the photos locally if they exist in Spaces but on in the local DB
        /// </summary>
        /// <param name="albumid"></param>
        /// <param name="albumName"></param>
        /// <param name="photolst"></param>
        public static void SynchronizePhotosFromSpacesToLocalDatabase(WLQuickApps.SocialNetwork.Business.Album _Album, string albumName, LiveNet.Photos.Photo[] photolst)
        {
            // Are we sync'ing any photos (were any passed in)
            if (photolst == null || photolst.Length < 1)
            {
                // No - clear the orphans
                ClearOrphanURL(_Album.BaseItemID);
            }
            else
            {
                /////
                ///// IMPORT PICTURES FROM SPACES IF THEY DONT EXIST LOCALLY
                /////

                // Yes - Enumerate the photos passed in
                foreach (LiveNet.Photos.Photo _photo in photolst)
                {

                    // for this image check if it exists in the album
                    List<Media> _MediaFound = MediaManager.GetMediaByAlbumAndImageURL(_Album, _photo.PhotoUrl.ToString());

                    // Was the media found in the local database
                    if (_MediaFound.Count == 0)
                    {
                        // No - create it
                        MediaManager.CreatePicture(_Album.BaseItemID, albumName, _photo.Name, _photo.Caption, _photo.PhotoUrl.ToString());
                    }
       
                }
                LocalErasePhoto(_Album, photolst);
            }

        }

        /// <summary>
        /// create an album if it doesn't exist, and sync the photos
        /// </summary>
        /// <param name="_album"></param>
        public static void SynchronizeAlbumFromSpacesToLocalDatabase(LiveNet.Photos.Album _album, bool PermitCreation)
        {
            // Get the albums for the current user
            List<WLQuickApps.SocialNetwork.Business.Album> _Albums = AlbumManager.GetAlbumsByUserID(UserManager.LoggedInUser.UserID);

            // Make sure the album is created and if not create it.
            WLQuickApps.SocialNetwork.Business.Album _Album = AlbumManager.CreateAlbumIfDoesNotExist(_album.Name, _Albums, PermitCreation);

            if (_Album == null) return;

            // sync the photos
            AlbumManager.SynchronizePhotosFromSpacesToLocalDatabase(_Album, _album.Name, _album.ListPhotos());
        }

        /// <summary>
        /// wrapper for SynchronizeAlbumFromSpacesToLocalDatabase(LiveNet.Photos.Album _album) 
        /// which accepts a URI
        /// </summary>
        /// <param name="_AlbumURI"></param>
        public static void SynchronizeAlbumFromSpacesToLocalDatabase(Uri _SpacesAlbumURI, bool PermitCreation)
        {
            // Generate the LivePhotos object
            LiveNet.Photos.LivePhotos  _LivePhotos = new LiveNet.Photos.LivePhotos(UserManager.LoggedInUser.PhotoPermissionToken.OwnerHandle,
                                                                         UserManager.LoggedInUser.PhotoPermissionToken.DomainAuthenticationToken,
                                                                         LiveNet.Authentication.AuthenticationToken.DomainAuthenticationToken);

            // Get the album
            LiveNet.Photos.Album _album = _LivePhotos.GetAlbum(_SpacesAlbumURI);

            AlbumManager.SynchronizeAlbumFromSpacesToLocalDatabase(_album, PermitCreation);
        }

        static private void ClearDeadAlbumURL(List<WLQuickApps.SocialNetwork.Business.Album> DbAlbums, LiveNet.Photos.LivePhotos lv)
        {
            if (DbAlbums == null) return;
            foreach (WLQuickApps.SocialNetwork.Business.Album dalbum in DbAlbums)
            {
                LiveNet.Photos.Album salbum = null;
                try
                {
                    salbum = lv.GetAlbum(dalbum.Title);
                    if (salbum == null) ClearOrphanURL(dalbum.BaseItemID);
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Write("ClearDeadAlbumURL method Error. " + ex.Message);
                }                
            }
        }

        public static void SynchronizeAlbumFromSpacesToLocalDatabase()
        {
            User user = UserManager.LoggedInUser;
            if (user == null) return ;
            PhotoToken _token = user.PhotoPermissionToken;
            if (_token == null) return;

            try
            {
                LiveNet.Photos.LivePhotos lv = new LiveNet.Photos.LivePhotos(_token.OwnerHandle, _token.DomainAuthenticationToken, LiveNet.Authentication.AuthenticationToken.DomainAuthenticationToken);
                LiveNet.Photos.Album[] albums = lv.ListAlbums();
                List<WLQuickApps.SocialNetwork.Business.Album> DbAlbums = AlbumManager.GetAlbumsByUserID(user.UserID);
                ClearDeadAlbumURL(DbAlbums, lv);
                if (albums == null) return;
                if (albums.Length < 1) return;
                foreach (LiveNet.Photos.Album album in albums)
                    SynchronizeAlbumFromSpacesToLocalDatabase(album.AlbumUrl,false);
            }
            catch (Exception ex)
            {
                
                HttpContext.Current.Trace.Write("SynchronizeAlbumFromSpacesToLocalDatabase method Error. " + ex.Message);
            }

        }

        public static void SyncUserAlbumsAsynchronous()
        {
            Utilities.ThreadUtil.FireAndForget(new Utilities.ThreadUtil.AsyncNoParamsDelegate(AlbumManager.SyncUserAlbumsSynchronous), null);
        }

        public static void SyncUserAlbumsSynchronous()
        {
            AlbumManager.ClearOrphanURL(); // Remove Picture records from users who revoked their site permission 
            AlbumManager.SynchronizeAlbumFromSpacesToLocalDatabase();
        }
              

    }
}