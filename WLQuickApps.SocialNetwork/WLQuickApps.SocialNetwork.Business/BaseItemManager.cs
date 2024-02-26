using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Drawing;

using WLQuickApps.SocialNetwork.Business.Properties;
using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.BaseItemDataSetTableAdapters;
using WLQuickApps.SocialNetwork.Data.MediaDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class BaseItemManager
    {
        static internal BaseItemDataSet.BaseItemRow GetBaseItemRow(int baseItemID)
        {
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                BaseItemDataSet.BaseItemDataTable table = baseItemTableAdapter.GetBaseItemByBaseItemID(baseItemID);

                if (table.Rows.Count == 0) { throw new ArgumentException("BaseItem does not exist"); }

                return table[0];
            }
        }

        static public BaseItem GetBaseItem(int baseItemID)
        {
            switch (BaseItemManager.GetBaseItemRow(baseItemID).ItemType)
            {
                case Constants.BaseItemTypes.Album: return AlbumManager.GetAlbum(baseItemID);
                case Constants.BaseItemTypes.Media: return MediaManager.GetMedia(baseItemID);
                case Constants.BaseItemTypes.User: return UserManager.GetUserByBaseItemID(baseItemID);
                case Constants.BaseItemTypes.Group:
                    try
                    {
                        return EventManager.GetEvent(baseItemID);
                    }
                    catch
                    {
                        return GroupManager.GetGroup(baseItemID);
                    }
                case Constants.BaseItemTypes.Collection: return CollectionManager.GetCollection(baseItemID);
                case Constants.BaseItemTypes.CollectionItem: return CollectionItemManager.GetCollectionItem(baseItemID);
                case Constants.BaseItemTypes.Forum: return ForumManager.GetForum(baseItemID);
                default: throw new ArgumentException("BaseItem has unknown type");
            }
        }

        static internal int CreateBaseItem(string itemType, Location location, string title, string description, User owner, string subType, PrivacyLevel privacyLevel, bool isApproved, string imageURL)
        {
            return BaseItemManager.CreateBaseItem(itemType, location, title, description, owner.UserID, subType, privacyLevel, isApproved, imageURL);
        }

        static internal int CreateBaseItem(string itemType, Location location, string title, string description, Guid userID, string subType, PrivacyLevel privacyLevel, bool isApproved, string imageURL)
        {
            if (subType == null) { subType = string.Empty; }

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                int baseItemID = Convert.ToInt32(baseItemTableAdapter.CreateBaseItem(itemType, string.Empty, location.LocationID, 0, 0, 0, title, description, userID, DateTime.Now, subType, (int)privacyLevel, isApproved, imageURL));

                return (baseItemID);
            }
        }

        static public void DeletePhoto(int BaseItemID)
        {
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                baseItemTableAdapter.RemoveAssociatedBaseItemAssociations(BaseItemID);
                baseItemTableAdapter.DeleteBaseItem(BaseItemID);
            }
        }

        static internal void DeleteBaseItem(BaseItem baseItem)
        {
            BaseItemManager.VerifyOwnerActionOnBaseItem(baseItem);
            
            // Is the subtype a picture?
            if (baseItem.SubType == "Picture")
            {
                // Yes - remove it from Spaces
                MediaUtil.DeletePhotoFromSpace(baseItem);
            }
            else
            {
                // Is this an album?
                if (GetBaseItemRow(baseItem.BaseItemID).ItemType == Constants.BaseItemTypes.Album)
                {
                    // Yes - delete the album
                    MediaUtil.DeleteAlbumFromSpace(baseItem);
                }
            }
       
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                baseItemTableAdapter.RemoveAssociatedBaseItemAssociations(baseItem.BaseItemID);
                baseItemTableAdapter.DeleteBaseItem(baseItem.BaseItemID);
            }
        }

        static internal void UpdateBaseItem(BaseItem baseItem)
        {
            BaseItemManager.VerifyOwnerActionOnBaseItem(baseItem);
            
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                baseItemTableAdapter.UpdateBaseItem(
                    baseItem.GetSearchDataString(), 
                    baseItem.Location.LocationID, 
                    baseItem.Title, 
                    baseItem.Description, 
                    baseItem.Owner.UserID, 
                    (int)baseItem.PrivacyLevel, 
                    baseItem.ImageURL, 
                    baseItem.BaseItemID);
            }
        }

        static internal void RateBaseItem(BaseItem baseItem, int rating)
        {
            UserManager.AssertThatAUserIsLoggedIn();

            if (!baseItem.CanContribute) { return; }

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                baseItemTableAdapter.RateItem(UserManager.LoggedInUser.UserID, baseItem.BaseItemID, rating);
            }
        }

        static internal void RemoveBaseItemAssociationFromBaseItem(BaseItem associatedBaseItem, BaseItem baseItem, bool isAssociatedBaseItemInstigator)
        {
            if (isAssociatedBaseItemInstigator)
            {
                BaseItemManager.VerifyOwnerActionOnBaseItem(associatedBaseItem);
            }
            else
            {
                BaseItemManager.VerifyOwnerActionOnBaseItem(baseItem);
            }

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                baseItemTableAdapter.RemoveAssociationFromBaseItem(associatedBaseItem.BaseItemID, baseItem.BaseItemID);
            }
        }

        static public void RemoveBaseItemAssociationFromBaseItem(BaseItem baseItem, int BaseItemID, bool isAssociatedBaseItemInstigator)
        {
            BaseItemManager.RemoveBaseItemAssociationFromBaseItem(BaseItemManager.GetBaseItem(BaseItemID), baseItem, isAssociatedBaseItemInstigator);
        }

        static public void RemoveAssociatedBaseItemFromBaseItem(int BaseItemID, BaseItem baseItem, bool isAssociatedBaseItemInstigator)
        {
            BaseItemManager.RemoveBaseItemAssociationFromBaseItem(baseItem, BaseItemManager.GetBaseItem(BaseItemID), isAssociatedBaseItemInstigator);
        }

        static internal void ApproveBaseItem(BaseItem baseItem)
        {
            BaseItemManager.VerifyUserCanApproveBaseItem(baseItem);
            using (BaseItemTableAdapter adapter = new BaseItemTableAdapter())
            {
                adapter.ApproveBaseItem(baseItem.BaseItemID);
            }
        }

        static public List<BaseItem> GetAssociatedBaseItemsForBaseItem(BaseItem baseItem)
        {
            if (baseItem == null) { throw new ArgumentNullException("baseItem"); }

            List<BaseItem> list = new List<BaseItem>();

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.GetAssociatedBaseItemsByBaseItemID(baseItem.BaseItemID))
                {
                    list.Add(BaseItemManager.GetBaseItem(row.BaseItemID));
                }
            }
            return list;
        }

        static public List<BaseItem> GetBaseItemsAssociatedWithBaseItem(BaseItem baseItem)
        {
            if (baseItem == null) { throw new ArgumentNullException("baseItem"); }

            List<BaseItem> list = new List<BaseItem>();

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.GetBaseItemsAssociatedWithBaseItem(baseItem.BaseItemID))
                {
                    list.Add(BaseItemManager.GetBaseItem(row.BaseItemID));
                }
            }
            return list;
        }

        static public SearchResults GetUnapprovedBaseItems()
        {
            SearchResults searchResults = new SearchResults();

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                foreach (BaseItemDataSet.BaseItemRow row in baseItemTableAdapter.GetUnapprovedBaseItems())
                {
                    switch (row.ItemType)
                    {
                        case Constants.BaseItemTypes.Media: searchResults.AddMedia(row.BaseItemID); break;
                        case Constants.BaseItemTypes.User: searchResults.AddUser(row.BaseItemID); break;
                        case Constants.BaseItemTypes.Group: searchResults.AddGroup(row.BaseItemID); break;
                        case Constants.BaseItemTypes.Collection: searchResults.AddCollection(row.BaseItemID); break;
                        default: break;
                    }
                }
            }
            return searchResults;
        }

        static public int GetUnapprovedBaseItemsCount()
        {
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                return (int)baseItemTableAdapter.GetUnapprovedBaseItemsCount();
            }
        }

        static internal void SetTotalViews(BaseItem baseItem)
        {
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                baseItemTableAdapter.SetTotalViews(baseItem.BaseItemID, baseItem.TotalViews);
            }
        }

        #region Thumbnails
        static internal void SetThumbnail(BaseItem baseItem, byte[] bits)
        {
            BaseItemManager.VerifyOwnerActionOnBaseItem(baseItem);

            using (Image image = Utilities.ConvertBytesToImage(bits))
            using (Image thumbnail = Utilities.GenerateThumbnail(image, 640, 640, true))
            {
                bits = Utilities.ConvertImageToBytes(thumbnail);
            }

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                baseItemTableAdapter.SetThumbnail(baseItem.BaseItemID, bits);
            }
        }

        static internal bool HasThumbnail(BaseItem baseItem)
        {
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                return (Convert.ToInt32(baseItemTableAdapter.VerifyBaseItemHasBits(baseItem.BaseItemID)) > 0);
            }
        }

        static internal byte[] GetRawThumbnailBits(int baseItemID)
        {
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                byte[] bits = baseItemTableAdapter.GetRawThumbnailBits(baseItemID) as byte[];

                if (bits == null)
                {
                    throw new ArgumentException("The specified item does not have a thumbnail");
                }

                return bits;
            }
        }

        static internal byte[] GetThumbnailBits(int baseItemID, int maximumWidth, int maximumHeight)
        {
            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                byte[] bits = baseItemTableAdapter.GetThumbnail(baseItemID, maximumWidth, maximumHeight) as byte[];

                if (bits == null)
                {
                    return Utilities.ConvertImageToBytes(BaseItemManager.GetThumbnail(baseItemID, maximumWidth, maximumHeight));
                }
                    using (Image image = Utilities.ConvertBytesToImage(bits))
                    using (Image thumbnail = Utilities.GenerateThumbnail(image, maximumWidth, maximumHeight, true))
                    {
                        return Utilities.ConvertImageToBytes(thumbnail);
                    }

            }
        }

        /// <summary>
        /// Try to get the baseitem thumbnail;
        /// if it doesn't exist the baseitem may be coming from spaces
        /// therefore we should download the binary
        /// </summary>
        /// <param name="baseItemID"></param>
        /// <param name="maximumWidth"></param>
        /// <param name="maximumHeight"></param>
        /// <returns></returns>
        static internal Image GetThumbnail(int baseItemID, int maximumWidth, int maximumHeight)
        {
            maximumWidth = Math.Min(640, maximumWidth);
            maximumHeight = Math.Min(640, maximumHeight);

            using (BaseItemTableAdapter baseItemTableAdapter = new BaseItemTableAdapter())
            {
                byte[] thumbnailBits = (byte[])baseItemTableAdapter.GetThumbnail(baseItemID, maximumWidth, maximumHeight);

                Image thumbnail = null;
                if (thumbnailBits != null)
                {
                    thumbnail = Utilities.ConvertBytesToImage(thumbnailBits);
                }
                else
                {
                    byte[] bits = baseItemTableAdapter.GetRawThumbnailBits(baseItemID) as byte[];

                    // are the thumbnail bits null?
                    if (bits == null)
                    {
                        // Yes

                        // Get the base item
                        BaseItem _baseItem = BaseItemManager.GetBaseItem(baseItemID);

                        // Is the base item a picture
                        if (_baseItem.SubType == "Picture")
                        {
                            // yes - is it sync'd with Spaces?
                            if (_baseItem.ImageURL != string.Empty)
                            {
                                // yes - get the bits from Spaces
                                bits = MediaUtil.GetImagefromSpacesURL(_baseItem.ImageURL, _baseItem.Owner.DomainAuthenticationToken, true);
                            }
                        }
                    }
                    else
                    {
                        // the bits exist, get the thumbnail
                        // generate the thumbnail
                        thumbnail = Utilities.GenerateThumbnail(bits, maximumWidth, maximumHeight, true);
                    }

                    // are the bits still null?
                    //if(bits == null)
                    //{
                    //    return null;
                    //}

                    // are the bits null
                    if (bits == null)
                    {
                        // yes - but is the thumbnail set?
                        if (thumbnail != null)
                        {
                            // yes - get the bits.
                            bits = Utilities.ConvertImageToBytes(thumbnail);
                        }
                    }

                    // are the bits still null?
                    if (bits == null)
                    {
                        // yes - return null
                        return null;
                    }
                    else
                    {
                        // the bits exist

                        // Add the thumbnail to cache
                        baseItemTableAdapter.AddThumbnailToCache(baseItemID, maximumWidth, maximumHeight, bits);

                        // load the thumbnail up
                        thumbnail = Utilities.ConvertBytesToImage(bits);
                    }
                }

                return thumbnail;
            }
        }
        #endregion

        #region Can User/Verify User Permissions
        static internal bool CanModifyBaseItem(int baseItemID)
        {
            return BaseItemManager.CanModifyBaseItem(BaseItemManager.GetBaseItem(baseItemID));
        }

        static internal bool CanContributeToBaseItem(BaseItem baseItem)
        {
            return UserManager.IsUserLoggedIn();
        }

        static internal bool CanModifyBaseItem(BaseItem baseItem)
        {
            return (UserManager.IsUserLoggedIn() &&
                ((baseItem.Owner == UserManager.LoggedInUser) || UserManager.LoggedInUser.IsAdmin));
        }

        static internal bool CanApproveBaseItem(BaseItem baseItem)
        {
            if (!UserManager.IsUserLoggedIn()) { return false; }
            if (baseItem.IsApproved) { return false; }
            return UserManager.LoggedInUser.IsAdmin;
        }

        static internal bool CanViewBaseItem(BaseItem baseItem)
        {
            switch (baseItem.PrivacyLevel)
            {
                case PrivacyLevel.Public: return true;
                case PrivacyLevel.Invisible: return false;
                case PrivacyLevel.Private:
                    if (!UserManager.IsUserLoggedIn()) { return false; }
                    return (baseItem.Owner == UserManager.LoggedInUser) ||
                        UserManager.LoggedInUser.IsAdmin ||
                        FriendManager.ConfirmFriendship(UserManager.LoggedInUser, baseItem.Owner);

                default: throw new Exception("Unexpected privacy level");
            }
        }

        static internal void VerifyUserCanApproveBaseItem(BaseItem baseItem)
        {
            if (!BaseItemManager.CanApproveBaseItem(baseItem))
            {
                throw new SecurityException("The current user does not have permission to approve to this item");
            }
        }

        static internal void VerifyUserCanContributeToBaseItem(BaseItem baseItem)
        {
            if (!BaseItemManager.CanContributeToBaseItem(baseItem))
            {
                throw new SecurityException("The current user does not have permission to contribute to this item");
            }
        }

        static internal void VerifyOwnerActionOnBaseItem(BaseItem baseItem)
        {
            if (!BaseItemManager.CanModifyBaseItem(baseItem.BaseItemID))
            {
                throw new SecurityException("Item cannot be modified because it does not belong to the logged in user");
            }
        }

        #endregion

    }
}
