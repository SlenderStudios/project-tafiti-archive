using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using WLQuickApps.Tafiti.Business;

namespace WLQuickApps.Tafiti.WebSite
{
    /// <summary>
    /// Summary description for AnonymousUserManager
    /// </summary>
    public class AnonymousUserManager
    {
        public AnonymousUserManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// This method merges shelves a user created prior to signing in with the shelves associated
        /// with their account after they sign in.
        /// </summary>
        static public void MergeShelfStacks()
        {
            List<ShelfStackData> shelfStacks = AnonymousUserManager.ShelfStacks;
            Dictionary<string, ShelfStackItemData> shelfStackItems = AnonymousUserManager.ShelfStackItems;

            foreach (ShelfStackData shelfData in shelfStacks)
            {
                ShelfStack shelfStack = ShelfStackManager.CreateShelfStack(shelfData.Label);
                foreach (string shelfStackItemID in shelfData.ShelfStackItemIDs)
                {
                    ShelfStackItemData shelfStackItemData = shelfStackItems[shelfStackItemID];
                    ShelfStackItemManager.CreateShelfStackItem(shelfStack, UserManager.LoggedInUser, shelfStackItemData.Title, shelfStackItemData.Description, shelfStackItemData.Url, 
                        shelfStackItemData.ImageUrl, shelfStackItemData.Width, shelfStackItemData.Height, shelfStackItemData.Source, shelfStackItemData.Domain);
                }
            }

            AnonymousUserManager.ShelfStacks = null;
            AnonymousUserManager.ShelfStackItems = null;
        }

        static private List<ShelfStackData> ShelfStacks
        {
            get
            {
                HttpSessionState session = HttpContext.Current.Session;

                List<ShelfStackData> shelfStacks = session["ShelfStacks"] as List<ShelfStackData>;
                if (shelfStacks == null)
                {
                    return new List<ShelfStackData>();
                }

                return shelfStacks;
            }
            set
            {
                HttpSessionState session = HttpContext.Current.Session;
                session["ShelfStacks"] = value;
            }
        }

        static private Dictionary<string, ShelfStackItemData> ShelfStackItems
        {
            get
            {
                HttpSessionState session = HttpContext.Current.Session;

                Dictionary<string, ShelfStackItemData> shelfStackItems = session["ShelfStackItems"] as Dictionary<string, ShelfStackItemData>;
                if (shelfStackItems == null)
                {
                    return new Dictionary<string, ShelfStackItemData>();
                }

                return shelfStackItems;
            }
            set
            {
                HttpSessionState session = HttpContext.Current.Session;
                session["ShelfStackItems"] = value;
            }
        }    

        static public ShelfStackData[] GetShelfStacks()
        {
            return AnonymousUserManager.ShelfStacks.ToArray();
        }

        static public void LeaveShelfStack(string shelfStackID)
        {
            List<ShelfStackData> shelfStacks = AnonymousUserManager.ShelfStacks;
            foreach (ShelfStackData shelfStackData in shelfStacks)
            {
                if (shelfStackData.ShelfStackID == shelfStackID)
                {
                    shelfStacks.Remove(shelfStackData);
                    break;
                }
            }
            AnonymousUserManager.ShelfStacks = shelfStacks;
        }

        static public ShelfStackData AddShelfStack(string label)
        {
            ShelfStackData shelfStackData = new ShelfStackData();
            shelfStackData.Label = label;
            shelfStackData.LastModifiedTimestamp = DateTime.UtcNow.ToString("R");
            shelfStackData.OwnerEmailHashes = new string[0];
            shelfStackData.ShelfStackID = Guid.NewGuid().ToString();
            shelfStackData.ShelfStackItemIDs = new string[0];

            List<ShelfStackData> shelfStacks = AnonymousUserManager.ShelfStacks;
            shelfStacks.Add(shelfStackData);
            AnonymousUserManager.ShelfStacks = shelfStacks;

            return shelfStackData;
        }

        static public ShelfStackItemData AddShelfStackItem(string shelfStackID, string domain, string title, string description, string url, string imageUrl, int width, int height, string source)
        {
            List<ShelfStackData> shelfStacks = AnonymousUserManager.ShelfStacks;
            foreach (ShelfStackData shelfStackData in shelfStacks)
            {
                if (shelfStackData.ShelfStackID == shelfStackID)
                {
                    ShelfStackItemData shelfItemData = new ShelfStackItemData();
                    shelfItemData.Description = description;
                    shelfItemData.Domain = domain;
                    shelfItemData.ImageUrl = imageUrl;
                    shelfItemData.Height = height;
                    shelfItemData.ShelfStackID = shelfStackID;
                    shelfItemData.ShelfStackItemID = Guid.NewGuid().ToString();
                    shelfItemData.Source = source;
                    shelfItemData.Timestamp = DateTime.UtcNow.ToString("R");
                    shelfItemData.Title = title;
                    shelfItemData.Url = url;
                    shelfItemData.UserID = string.Empty;
                    shelfItemData.Width = width;

                    List<string> shelfStackIDs = new List<string>(shelfStackData.ShelfStackItemIDs);
                    shelfStackIDs.Add(shelfItemData.ShelfStackItemID);
                    shelfStackData.ShelfStackItemIDs = shelfStackIDs.ToArray();

                    Dictionary<string, ShelfStackItemData> shelfStackItems = AnonymousUserManager.ShelfStackItems;
                    shelfStackItems.Add(shelfItemData.ShelfStackItemID, shelfItemData);
                    AnonymousUserManager.ShelfStackItems = shelfStackItems;

                    return shelfItemData;
                }
            }

            throw new ArgumentException("Shelf stack not found");
        }

        static public void RemoveShelfStackItem(string shelfStackItemID)
        {
            Dictionary<string, ShelfStackItemData> shelfStackItems = AnonymousUserManager.ShelfStackItems;
            shelfStackItems.Remove(shelfStackItemID);
            AnonymousUserManager.ShelfStackItems = shelfStackItems;

            List<ShelfStackData> shelfStacks = AnonymousUserManager.ShelfStacks;
            foreach (ShelfStackData shelfStackData in shelfStacks)
            {
                foreach (string id in shelfStackData.ShelfStackItemIDs)
                {
                    if (id == shelfStackItemID)
                    {
                        List<string> ids = new List<string>(shelfStackData.ShelfStackItemIDs);
                        ids.Remove(id);
                        shelfStackData.ShelfStackItemIDs = ids.ToArray();

                        AnonymousUserManager.ShelfStacks = shelfStacks;
                        return;
                    }
                }
            }
        }

        static public ShelfStackData GetShelfStack(string shelfStackID)
        {
            List<ShelfStackData> shelfStacks = AnonymousUserManager.ShelfStacks;
            foreach (ShelfStackData existingShelfStackData in shelfStacks)
            {
                if (existingShelfStackData.ShelfStackID == shelfStackID)
                {
                    return existingShelfStackData;
                }
            }

            ShelfStack shelfStack = ShelfStackManager.GetShelfStackByID(new Guid(shelfStackID));
            if (shelfStack == null)
            {
                return null;
            }

            ShelfStackData shelfStackData = new ShelfStackData(shelfStack);
            shelfStackData.ShelfStackItemIDs = new string[0];
            shelfStacks.Add(shelfStackData);
            AnonymousUserManager.ShelfStacks = shelfStacks;

            foreach (ShelfStackItem shelfStackItem in ShelfStackItemManager.GetShelfStackItemsForShelf(shelfStack))
            {
                AnonymousUserManager.AddShelfStackItem(shelfStack.ShelfStackID.ToString(), shelfStackItem.Domain, 
                    shelfStackItem.Title, shelfStackItem.Description, shelfStackItem.Url, 
                    shelfStackItem.ImageUrl, shelfStackItem.Width, shelfStackItem.Height, shelfStackItem.Source);
            }

            return shelfStackData;
        }

        static public ShelfStackItemData GetShelfStackItem(string shelfStackItemID)
        {
            return AnonymousUserManager.ShelfStackItems[shelfStackItemID];
        }

        static public void SetShelfStackLabel(string shelfStackID, string label)
        {
            List<ShelfStackData> shelfStacks = AnonymousUserManager.ShelfStacks;
            foreach (ShelfStackData shelfStackData in shelfStacks)
            {
                if (shelfStackData.ShelfStackID == shelfStackID)
                {
                    shelfStackData.Label = label;
                }
            }
            AnonymousUserManager.ShelfStacks = shelfStacks;
        }

        static public UserData GetUser(string userID)
        {
            UserData userData = new UserData();
            userData.DisplayName = "Anonymous";
            userData.EmailHash = string.Empty;
            userData.MessengerPresenceID = string.Empty;
            userData.UserID = "Anonymous";
            return userData;
        }

        static public UserData GetUserByEmailHash(string emailHash)
        {
            UserData userData = new UserData();
            userData.DisplayName = "Anonymous";
            userData.EmailHash = emailHash;
            userData.MessengerPresenceID = string.Empty;
            userData.UserID = "Anonymous";
            return userData;
        }

    }
}