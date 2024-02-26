<%@ WebHandler Language="C#" Class="ImageHandler"  %>

using System;
using System.Web;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;
using System.Web.SessionState;



public class ImageHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext _context)
    {
        try
        {
            // set the response type.
            _context.Response.ContentType = "image/jpeg";

            // set the max height/width
            int maxWidth = 2048;
            int maxHeight = 2048;
            
            Object oheight = _context.Request.QueryString[WebConstants.QueryVariables.MaxThumbnailHeight];
            Object owidth = _context.Request.QueryString[WebConstants.QueryVariables.MaxThumbnailWidth];
            
            // get the base item
            int baseItemID = int.Parse(_context.Request.QueryString[WebConstants.QueryVariables.BaseItemID]);

            if (oheight != null && owidth != null)
            {
                int.TryParse(oheight.ToString(), out maxHeight);
                int.TryParse(owidth.ToString(), out maxWidth);
            }
            else
            {
                maxWidth = 2048;
                maxHeight = 2048;
            }

            // declare the picture bytes
            byte[] pictureBytes = null;

            // get the media picture
            BaseItem _baseItem = BaseItemManager.GetBaseItem(baseItemID);

            // Is the ImageURL set?
            if (_baseItem.ImageURL != string.Empty)
            {
                bool _isThumbnail = true;

                if ((maxWidth <= 200) && (maxHeight <= 200))
                {
                    _isThumbnail = true;
                }
                else
                {
                    _isThumbnail = false;
                }

                // Yes - this is from spaces
                pictureBytes = MediaUtil.GetImagefromSpacesURL(_baseItem.ImageURL, _baseItem.Owner.DomainAuthenticationToken, _isThumbnail);
                // thumbnail the bits
                pictureBytes = PhotoUtil.GetThumbImage(pictureBytes, maxWidth, maxHeight);
            }
            else
            {
                // No - use a local image (thumbnailed)
                pictureBytes = _baseItem.GetThumbnailBits(maxWidth, maxHeight);

                // were the bytes returned?
                if (pictureBytes == null)
                {

                    string theType = _baseItem.GetType().ToString(); // WLQuickApps.SocialNetwork.Business.Album

                    theType = theType.Substring(theType.LastIndexOf(".") + 1); // Album

                    pictureBytes = PhotoUtil.GetMissingImage(ImageMissingReason.None, theType, maxWidth);
                }
            }

            // Do we need to thumbnail the image?
            if (pictureBytes.Length > (maxWidth * maxHeight))
            {
                // Yes
                pictureBytes = PhotoUtil.GetThumbImage(pictureBytes, maxWidth, maxHeight);
            }
           
            // Write the binary stream
            _context.Response.BinaryWrite(pictureBytes);
        }
        catch { }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

    