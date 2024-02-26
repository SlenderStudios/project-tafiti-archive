using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

using WLQuickApps.Tafiti.Business;

namespace WLQuickApps.Tafiti.WebSite
{
    /// <summary>
    /// Summary description for SiteService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SiteService : System.Web.Services.WebService
    {

        public SiteService()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }
        
        [WebMethod(true)]
        public ShelfStackData[] GetShelfStacks()
        {
            if (!UserManager.IsUserLoggedIn) 
            {
                return AnonymousUserManager.GetShelfStacks();
            }

            ReadOnlyCollection<ShelfStack> shelfStacks = ShelfStackManager.GetShelfStacksForUser(UserManager.LoggedInUser);
            ShelfStackData[] shelfStackDatas = new ShelfStackData[shelfStacks.Count];
            for (int lcv = 0; lcv < shelfStacks.Count; lcv++)
            {
                shelfStackDatas[lcv] = new ShelfStackData(shelfStacks[lcv]);
            }

            return shelfStackDatas;
        }

        [WebMethod(true)]
        public string[] GetConversation(string shelfStackID)
        {
            ReadOnlyCollection<Comment> comments = ConversationManager.GetCommentsByShelf(new Guid(shelfStackID));

            string[] commentIDs = new string[comments.Count];
            for (int lcv = 0; lcv < comments.Count; lcv++)
            {
                commentIDs[lcv] = comments[lcv].CommentID.ToString();
            }

            return commentIDs;
        }

        [WebMethod(true)]
        public void AddComment(string shelfStackID, string text)
        {
            ConversationManager.AddComment(new Guid(shelfStackID), UserManager.LoggedInUser.UserID, text);
        }

        [WebMethod(true)]
        public void UpdateUserDetails(string displayName, string emailHash)
        {
            User user = UserManager.LoggedInUser;
            user.DisplayName = displayName;
            user.EmailHash = emailHash;
            UserManager.UpdateUser(user);
        }

        [WebMethod(true)]
        public void RemoveShelfStackItem(string shelfStackItemID)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                AnonymousUserManager.RemoveShelfStackItem(shelfStackItemID);
                return;
            }

            ShelfStackItemManager.DeleteShelfStackItem(ShelfStackItemManager.GetShelfStackItemByID(Convert.ToInt32(shelfStackItemID)));
        }

        [WebMethod(true)]
        public void LeaveShelfStack(string shelfStackID)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                AnonymousUserManager.LeaveShelfStack(shelfStackID);
                return;
            }

            ShelfStackManager.RemoveUserFromShelfStack(UserManager.LoggedInUser, ShelfStackManager.GetShelfStackByID(new Guid(shelfStackID)));
        }

        [WebMethod(true)]
        public ShelfStackData AddShelfStack(string label)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return AnonymousUserManager.AddShelfStack(label);
            }

            ShelfStack shelfStack = ShelfStackManager.CreateShelfStack(label);
            return new ShelfStackData(shelfStack);
        }

        [WebMethod(true)]
        public ShelfStackItemData AddShelfStackItem(string shelfStackID, string domain, string title, string description, string url, string imageUrl, int width, int height, string source)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return AnonymousUserManager.AddShelfStackItem(shelfStackID, domain, title, description, url, imageUrl, width, height, source);
            }

            ShelfStackItem shelfStackItem = ShelfStackItemManager.CreateShelfStackItem(ShelfStackManager.GetShelfStackByID(new Guid(shelfStackID)),
                UserManager.LoggedInUser, title, description, url, imageUrl, width, height, source, domain);
            return new ShelfStackItemData(shelfStackItem);
        }

        [WebMethod(true)]
        public bool CheckForUpdates(DateTime lastUpdate)
        {
            return ShelfStackManager.CheckForUpdates(lastUpdate.ToLocalTime());
        }

        [WebMethod(true)]
        public UserData GetUser(string userID)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return AnonymousUserManager.GetUser(userID);
            }

            User user = UserManager.GetUserByID(userID);
            if (user != null)
            {
                return new UserData(user);
            }

            UserData userData = new UserData();
            userData.EmailHash = userID;
            userData.UserID = userID;
            return userData;            
        }

        [WebMethod(true)]
        public UserData GetUserByEmailHash(string emailHash)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return AnonymousUserManager.GetUserByEmailHash(emailHash);
            }

            User user = UserManager.GetUserByEmailHash(emailHash);
            if (user != null)
            {
                return new UserData(user);
            }

            UserData userData = new UserData();
            userData.EmailHash = emailHash;
            userData.UserID = emailHash;
            return userData;
        }

        [WebMethod(true)]
        public UserData[] GetUserListByEmailHash(string emailHashes)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return new UserData[0];
            }


            List<UserData> list = new List<UserData>();
            foreach (string emailHash in emailHashes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                User user = UserManager.GetUserByEmailHash(emailHash);
                if (user != null)
                {
                    list.Add(new UserData(user));
                }
                else
                {
                    UserData userData = new UserData();
                    userData.EmailHash = emailHash;
                    userData.UserID = emailHash;
                    list.Add(userData);
                }
            }

            return list.ToArray();
        }

        [WebMethod(true)]
        public CommentData GetComment(string commentID)
        {
            Comment comment = ConversationManager.GetCommentByID(Convert.ToInt32(commentID));
            return new CommentData(comment);
        }

        [WebMethod(true)]
        public ShelfStackData GetShelfStack(string shelfStackID)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return AnonymousUserManager.GetShelfStack(shelfStackID);
            }

            ShelfStack shelfStack = ShelfStackManager.GetShelfStackByID(new Guid(shelfStackID));
            return new ShelfStackData(shelfStack);
        }

        [WebMethod(true)]
        public ShelfStackItemData GetShelfStackItem(string shelfStackItemID)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return AnonymousUserManager.GetShelfStackItem(shelfStackItemID);
            } 
            
            ShelfStackItem shelfStackItem = ShelfStackItemManager.GetShelfStackItemByID(Convert.ToInt32(shelfStackItemID));
            return new ShelfStackItemData(shelfStackItem);
        }

        [WebMethod(true)]
        public void AddUserToShelfStack(string shelfStackID, string emailHash)
        {
            Guid shelfStackIDGuid = new Guid(shelfStackID);
            ShelfStack shelfStack = ShelfStackManager.GetShelfStackByID(shelfStackIDGuid);
            User user = UserManager.GetUserByEmailHash(emailHash);

            if (user != null)
            {
                ShelfStackManager.AddUserToShelfStack(user, shelfStack);
            }
            else
            {
                ShelfStackManager.CreatePendingInvite(shelfStackIDGuid, emailHash);
            }
        }

        [WebMethod(true)]
        public void SetShelfStackLabel(string shelfStackID, string label)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                AnonymousUserManager.SetShelfStackLabel(shelfStackID, label);
                return;
            } 
            
            ShelfStack shelfStack = ShelfStackManager.GetShelfStackByID(new Guid(shelfStackID));
            shelfStack.Label = label;
            ShelfStackManager.UpdateShelfStack(shelfStack);
        }

        [WebMethod(true)]
        public bool GetAlwaysSendMessages()
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return false;
            }

            return UserManager.LoggedInUser.AlwaysSendMessages;
        }

        [WebMethod(true)]
        public void SetAlwaysSendMessages(bool alwaysSendMessages)
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return;
            }

            User user = UserManager.LoggedInUser;
            user.AlwaysSendMessages = alwaysSendMessages;
            UserManager.UpdateUser(user);
        }

        [WebMethod(true)]
        public MasterData GetMasterData()
        {
            if (!UserManager.IsUserLoggedIn)
            {
                return new MasterData();
            }

            return new MasterData(ShelfStackManager.GetShelfStacksForUser(UserManager.LoggedInUser));
        }

    }
}
