using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Management;
using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.WebSite
{
    /// <summary>
    /// Provides static methods for functionality commonly required on the UI tier
    /// </summary>
    public class WebUtilities
    {
        private WebUtilities()
        { }

        public static string TrimLongTitles(string s)
        {
            return WebUtilities.TrimLongTitles(s, 14);
        }

        public static string TrimLongTitles(string s, int maxLength)
        {
            string result = string.Empty;
            foreach (string segment in s.Split(' '))
            {
                result += string.Format(" {0}", WebUtilities.TrimSection(segment, maxLength));
            }
            return result.Trim();
        }

        public static string GetViewImageUrl(int baseItemID, int maxWidth, int maxHeight)
        {
            return string.Format("~/Media/ImageHandler.ashx?baseItemID={0}&maxWidth={1}&maxHeight={2}", baseItemID, maxWidth, maxHeight);
        }

        public static string GetViewItemUrl(BaseItem baseItem)
        {
            if (baseItem is Media)
            {
                return string.Format("~/Media/ViewMedia.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is Event)
            {
                return string.Format("~/Event/ViewEvent.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is Group)
            {
                return string.Format("~/Group/ViewGroup.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is User)
            {
                return string.Format("~/Friend/ViewProfile.aspx?{0}={1}", WebConstants.QueryVariables.UserName, (baseItem as User).UserName);
            }
            else if (baseItem is Collection)
            {
                return string.Format("~/Collection/ViewCollection.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is Forum)
            {
                return string.Format("~/Forum/ViewForum.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is Album)
            {
                return string.Format("~/Media/ViewAlbum.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }

            return "~/";
        }

        public static string GetEditItemUrl(BaseItem baseItem)
        {
            if (baseItem is Media)
            {
                return string.Format("~/Media/EditMedia.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is Event)
            {
                return string.Format("~/Event/EditEvent.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is Group)
            {
                return string.Format("~/Group/EditGroup.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is User)
            {
                return "~/User/EditProfile.aspx";
            }
            else if (baseItem is Collection)
            {
                return string.Format("~/Collection/EditCollection.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is Forum)
            {
                return string.Format("~/Forum/ViewForum.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }
            else if (baseItem is Album)
            {
                return string.Format("~/Media/EditAlbum.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, baseItem.BaseItemID);
            }

            return "~/";
        }

        private static string TrimSection(string s, int maxLength)
        {
            if (s.Length > maxLength)
            {
                s = string.Format("{0}<wbr />{1}", s.Substring(0, maxLength), WebUtilities.TrimSection(s.Substring(maxLength), maxLength));
            }
            return s;
        }
    }
}
