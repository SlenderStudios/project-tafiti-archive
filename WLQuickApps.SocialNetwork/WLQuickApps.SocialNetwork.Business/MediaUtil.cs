using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using LiveNet.Authentication;
using LiveNet.Photos;
using System.Web;
using System.Web.Configuration;

namespace WLQuickApps.SocialNetwork.Business
{


    public static class MediaUtil
    {
        static string cons = WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        public static string UploadPictureToSpaces(User _user, Media _picture, string album, byte[] bits, string filename)
        {
            string path = string.Empty;

            if (!filename.Contains("//"))
            {
                PhotoToken tok = _user.PhotoPermissionToken;

                if (tok == null) return string.Empty;

                string Error = string.Empty;

                LivePhotos phAlbum = new LivePhotos(tok.OwnerHandle, tok.DomainAuthenticationToken, AuthenticationToken.DomainAuthenticationToken);

                LiveNet.Photos.Album al = null;

                try
                {
                    al = phAlbum.GetAlbum(album);
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Write("Error getting album | " + ex.Message);
                }

                if (al == null) al = phAlbum.CreateAlbum(album);

                if (al == null) return string.Empty; // Failure

                LiveNet.Photos.Photo ph = null;

                try
                {
                    ph = phAlbum.GetPhoto(new Uri(al.AlbumUrl + "/" + filename));
                }
                catch (Exception ex)
                {
                    // trace and sink
                    HttpContext.Current.Trace.Write("Error getting photo from Spaces . " + ex.Message);
                }

                if (ph == null) ph = phAlbum.AddPhoto(new MemoryStream(bits), al.AlbumUrl, filename);

                if (ph == null) return string.Empty;

                path = ph.PhotoUrl.ToString();
            }
            else
            {
                path = filename;
            }

            return (path);
        }

        /// <summary>
        /// checks if the photo is in Spaces and if the user has granted permission to modify the space
        /// then makes the call to delete against the space.
        /// </summary>
        /// <param name="_baseItem"></param>
        public static void DeletePhotoFromSpace(BaseItem _baseItem)
        {
            try
            {
                // Is the photo in spaces?
                if (_baseItem.ImageURL.Length > 3)
                {
                    // Yes
                    // Do we have permission to action against the user's store?
                    if (_baseItem.Owner.PhotoPermissionToken != null)
                    {
                        // Yes
                        LivePhotos phLivePhotos = new LivePhotos(_baseItem.Owner.PhotoPermissionToken.OwnerHandle, _baseItem.Owner.PhotoPermissionToken.DomainAuthenticationToken, AuthenticationToken.DomainAuthenticationToken);

                        // Delete the photo
                        try
                        {
                            phLivePhotos.DeletePhoto(new Uri(_baseItem.ImageURL));
                        }
                        catch (LiveNet.LiveNetRequestException ex)
                        {
                            // trace and sink
                            HttpContext.Current.Trace.Write("Error deleting Photo. " + ex.Message);

                            return;
                        }
                        catch (Exception ex)
                        {
                            // trace and sink
                            HttpContext.Current.Trace.Write("Error deleting Photo. " + ex.Message);
                            return;
                        }
                    }
                }
            }
            catch
            {
                // sink 
            }
        }

        /// <summary>
        /// Check if we have permission from the user to action their space;
        /// and delete the album
        /// </summary>
        /// <param name="ItemId"></param>
        public static void DeleteAlbumFromSpace(BaseItem _baseItem)
        {
            // Has the owner granted permissions to modify the space?
            if (_baseItem.Owner.PhotoPermissionToken != null)
            {
                // Check if the album exists in spaces
                LivePhotos phLivePhotos = new LivePhotos(_baseItem.Owner.PhotoPermissionToken.OwnerHandle, _baseItem.Owner.PhotoPermissionToken.DomainAuthenticationToken, AuthenticationToken.DomainAuthenticationToken);

                // get the album from Spaces
                LiveNet.Photos.Album _album = phLivePhotos.GetAlbum(_baseItem.Title);

                // was the album returned?
                if (_album != null)
                {
                    // delete it
                    _album.Delete();



                }
            }
        }

        /// <summary>
        /// Make the call to spaces
        /// if there are errors return the correct placeholder image.
        /// </summary>
        /// <param name="spacesImageURL"></param>
        /// <param name="domainAuthenticationToken"></param>
        /// <returns></returns>
        public static byte[] GetImagefromSpacesURL(string spacesImageURL, string domainAuthenticationToken, bool isThumbnailed)
        {
            try
            {
                // has the user granted permissions
                if (domainAuthenticationToken == string.Empty)
                {
                    // no - return the permissions revoked image
                    return (PhotoUtil.GetMissingImage(ImageMissingReason.PermissionsRevoked, string.Empty));
                }
                else
                {
                    // Make the call to spaces
                    return MediaUtil.GetImageFromSpacesURLNoHandling(spacesImageURL, domainAuthenticationToken, isThumbnailed);
                }
            }
            catch (FileNotFoundException ex)
            {
                HttpContext.Current.Trace.Write("error loading from spaces : " + ex.Message);

                // get the failure
                return (PhotoUtil.GetMissingImage(ImageMissingReason.NotFound, string.Empty));
            }
            catch (UnauthorizedAccessException ex)
            {
                HttpContext.Current.Trace.Write("error loading from spaces : " + ex.Message);

                // get the invalid permissions
                return (PhotoUtil.GetMissingImage(ImageMissingReason.PermissionsInvalid, string.Empty));
            }
            catch (System.Net.WebException ex)
            {
                HttpContext.Current.Trace.Write("error loading from spaces : " + ex.Message);

                switch (((System.Net.HttpWebResponse)(ex.Response)).StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return (PhotoUtil.GetMissingImage(ImageMissingReason.NotFound, string.Empty));

                    case HttpStatusCode.Unauthorized:
                        return (PhotoUtil.GetMissingImage(ImageMissingReason.PermissionsInvalid, string.Empty));
                    default:
                        return (PhotoUtil.GetMissingImage(ImageMissingReason.Other, string.Empty));
                }

            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Write("error loading from spaces : " + ex.Message);

                // get the failure
                return (PhotoUtil.GetMissingImage(ImageMissingReason.Other, string.Empty));
            }
        }

        /// <summary>
        /// Make a call to spaces but do not handle exceptions (bubble them up)
        /// Use this method to verify a image can successfully be fetched
        /// </summary>
        /// <param name="spacesImageURL"></param>
        /// <param name="domainAuthenticationToken"></param>
        /// <returns></returns>
        private static byte[] GetImageFromSpacesURLNoHandling(string spacesImageURL, string domainAuthenticationToken, bool thumbnailed)
        {
            // is it a thumbnail request
            if (thumbnailed)
            {
                // yes - append Thumbnail=true
                spacesImageURL = spacesImageURL + "?thumbnail=true";
            }

            // build the request
            HttpWebRequest serviceRequest = (HttpWebRequest)WebRequest.Create(spacesImageURL);

            // setup the user agent
            serviceRequest.UserAgent = "WLQuickApps.SocialNetwork";

            // set the GET method
            serviceRequest.Method = WebRequestMethods.Http.Get;

            // put the DomainAuthenticationToken in the header
            serviceRequest.Headers.Add("Authorization", "DomainAuthToken at=\"" + domainAuthenticationToken + "\"");

            // get the response
            HttpWebResponse serviceResponse = (HttpWebResponse)serviceRequest.GetResponse();

            // read the binary response & return
            return MediaUtil.ReadFully(serviceResponse.GetResponseStream(), 0);
        }

        /// <summary>
        /// Read a stream into a byte array
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="initialLength"></param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream stream, int initialLength)
        {
            try
            {
                // If we've been passed an unhelpful initial length, just
                // use 32K.
                if (initialLength < 1)
                {
                    initialLength = 32768;
                }

                // create the buffer
                byte[] buffer = new byte[initialLength];

                // initialize enumerator
                int read = 0;

                int chunk;

                // loop through the chunk
                while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
                {
                    read += chunk;

                    // If we've reached the end of our buffer, check to see if there's
                    // any more information
                    if (read == buffer.Length)
                    {
                        int nextByte = stream.ReadByte();

                        // End of stream? If so, we're done
                        if (nextByte == -1)
                        {
                            return buffer;
                        }

                        // Nope. Resize the buffer, put in the byte we've just
                        // read, and continue
                        byte[] newBuffer = new byte[buffer.Length * 2];
                        Array.Copy(buffer, newBuffer, buffer.Length);
                        newBuffer[read] = (byte)nextByte;
                        buffer = newBuffer;
                        read++;
                    }
                }

                // Buffer is now too big. Shrink it.
                byte[] ret = new byte[read];

                Array.Copy(buffer, ret, read);

                return ret;

            }
            catch (Exception ex)
            {
                // trace and sink
                HttpContext.Current.Trace.Write("Error Readingfully | " + ex.Message);
            }

            return null;
        }
    }
}
