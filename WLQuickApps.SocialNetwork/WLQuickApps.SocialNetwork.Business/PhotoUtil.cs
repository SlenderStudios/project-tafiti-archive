using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Configuration;
using System.Drawing;
using System.IO;
using System.Web;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class PhotoUtil
    {
        static string cons = WebConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString;

        public const string MaxThumbnailHeight = "maxHeight";
        public const string MaxThumbnailWidth = "maxWidth";

        public static bool SetPhoto(string ownerid, byte[] bits, string baseItemID, string title, string filename, string AlbumName)
        {
            return false;
        }

        public static byte[] GetThumbImage(byte[] bits, int width, int height)
        {
            byte[] ret;
            using (Image image = Utilities.ConvertBytesToImage(bits))
            using (Image thumbnail = Utilities.GenerateThumbnail(image, width, height, true))
            {
                ret = Utilities.ConvertImageToBytes(thumbnail);
            }
            return ret;
        }



        public static byte[] GetMissingImage(ImageMissingReason _reason, string objectType)
        {
            return (GetMissingImage(_reason, objectType, 0));
        }
        /// <summary>
        /// Get the missing image for the reason
        /// </summary>
        /// <param name="_reason"></param>
        /// <returns></returns>
        public static byte[] GetMissingImage(ImageMissingReason _reason, string objectType, int initialSize)
        {
            string imageURL = "";

            // what is the reason?
            switch (_reason)
            {
                case ImageMissingReason.NotFound:
                    imageURL = "~/images/missing-image-from-spaces-NotFound.jpg";
                    break;
                case ImageMissingReason.Other:
                    imageURL = "~/images/missing-image-from-spaces-Other.jpg";
                    break;
                case ImageMissingReason.PermissionsInvalid:
                    imageURL = "~/images/missing-image-from-spaces-PermissionsInvalid.jpg";
                    break;
                case ImageMissingReason.PermissionsRevoked:
                    imageURL = "~/images/missing-image-from-spaces-PermissionsRevoked.jpg";
                    break;
                case ImageMissingReason.None:
                    // default to 128
                    if (initialSize == 0)
                    {
                        initialSize = 128;
                    }

                    // set the URL by merging the type and the size
                    imageURL = string.Format("~/Images/missing-{0}-{1}x{2}.png", objectType, initialSize, initialSize);

                    break;
                default:
                    imageURL = "~/images/missing-image-from-spaces.jpg";
                    break;
            }

            // Load the binarystream
            FileStream _fileStream = System.IO.File.OpenRead(HttpContext.Current.Server.MapPath(imageURL));

            // read the media
            return (MediaUtil.ReadFully(_fileStream, 0));

        }

        public static byte[] GetPhotoLocal(int baseItemID, int maxWidth, int maxHeight)
        {
            BaseItem baseItem = BaseItemManager.GetBaseItem(baseItemID);
            if (baseItem == null) return null;
            return baseItem.GetThumbnailBits(maxWidth, maxHeight);
        }

    }
}
