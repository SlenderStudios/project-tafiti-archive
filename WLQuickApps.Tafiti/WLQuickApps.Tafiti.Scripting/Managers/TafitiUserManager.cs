using System;
using System.DHTML;
using ScriptFX;
using System.XML;

namespace WLQuickApps.Tafiti.Scripting
{
    public class TafitiUserManager
    {
        static private Dictionary UsersCache
        {
            get
            {
                if (TafitiUserManager._usersCache == null)
                {
                    TafitiUserManager._usersCache = new Dictionary();
                }
                return TafitiUserManager._usersCache;
            }
        }
        static private Dictionary _usersCache;

        static private Dictionary EmailHashCache
        {
            get
            {
                if (TafitiUserManager._emailHashCache == null)
                {
                    TafitiUserManager._emailHashCache = new Dictionary();
                }
                return TafitiUserManager._emailHashCache;
            }
        }
        static private Dictionary _emailHashCache;

        static private ArrayList OutstandingUserRequests
        {
            get
            {
                if (TafitiUserManager._outstandingUserRequests == null)
                {
                    TafitiUserManager._outstandingUserRequests = new ArrayList();
                }
                return TafitiUserManager._outstandingUserRequests;
            }
        }
        static private ArrayList _outstandingUserRequests;

        static private ArrayList OutstandingUserListRequests
        {
            get
            {
                if (TafitiUserManager._outstandingUserListRequests == null)
                {
                    TafitiUserManager._outstandingUserListRequests = new ArrayList();
                }
                return TafitiUserManager._outstandingUserListRequests;
            }
        }
        static private ArrayList _outstandingUserListRequests;

        static public bool AlwaysSendMessages
        {
            get
            {
                if (TafitiUserManager._alwaysSendMessages) { return true; }
                string body = "";
                XMLHttpRequest request = new XMLHttpRequest();
                request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetAlwaysSendMessages", false);
                request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                request.SetRequestHeader("Content-Length", body.Length.ToString());
                request.Send(body);
                return bool.Parse(request.ResponseXML.LastChild.Text);
            }
            set
            {
                TafitiUserManager._alwaysSendMessages = value;
                string body = "alwaysSendMessages=" + value.ToString();
                XMLHttpRequest request = new XMLHttpRequest();
                request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/SetAlwaysSendMessages", false);
                request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                request.SetRequestHeader("Content-Length", body.Length.ToString());
                request.Send(body);
            }
        }
        static private bool _alwaysSendMessages;

        static public string LoggedInUserID
        {
            get { return TafitiUserManager._loggedInUserID; }
            set { TafitiUserManager._loggedInUserID = value; }
        }
        static private string _loggedInUserID;

        static public TafitiUser LoggedInUser
        {
            get
            {                
                if (TafitiUserManager._loggedInUserID == null) { return null; }
                return TafitiUserManager.GetUser(TafitiUserManager._loggedInUserID);
            }
        }


        static public void BeginUserRequestByUserID(string userID)
        {
            if (TafitiUserManager.UsersCache.ContainsKey(userID))
            {
                return;
            }

            string body = "userID=" + userID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetUser", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Onreadystatechange = new Callback(TafitiUserManager.EndUserRequest);

            TafitiUserManager.OutstandingUserRequests.Add(request);
            request.Send(body);
        }

        static public void BeginUserRequestByEmailHash(string emailHash)
        {
            if (TafitiUserManager.EmailHashCache.ContainsKey(emailHash))
            {
                return;
            }

            string body = "emailHash=" + emailHash;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetUserByEmailHash", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Onreadystatechange = new Callback(TafitiUserManager.EndUserRequest);

            TafitiUserManager.OutstandingUserRequests.Add(request);
            request.Send(body);
        }

        static private void EndUserRequest()
        {
            for (int lcv = 0; lcv < TafitiUserManager.OutstandingUserRequests.Length; lcv++)
            {
                XMLHttpRequest request = (XMLHttpRequest)TafitiUserManager.OutstandingUserRequests[lcv];
                if (request.ReadyState == 4)
                {
                    TafitiUserManager.OutstandingUserRequests.RemoveAt(lcv);
                    lcv--;

                    TafitiUser user = TafitiUser.CreateFromXmlNode(request.ResponseXML.LastChild);
                    TafitiUserManager.CacheUser(user); 
                    return;
                }
            }
        }

        static public void BeginUserListRequestByEmailHash(string[] emailHashes)
        {
            string body = "emailHashes=" + emailHashes.Join(",");

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetUserListByEmailHash", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Onreadystatechange = new Callback(TafitiUserManager.EndUserListRequest);

            TafitiUserManager.OutstandingUserRequests.Add(request);
            request.Send(body);
        }

        static private void EndUserListRequest()
        {
            for (int lcv = 0; lcv < TafitiUserManager.OutstandingUserRequests.Length; lcv++)
            {
                XMLHttpRequest request = (XMLHttpRequest)TafitiUserManager.OutstandingUserRequests[lcv];
                if (request.ReadyState == 4)
                {
                    TafitiUserManager.OutstandingUserRequests.RemoveAt(lcv);
                    lcv--;

                    XMLNodeList shelfStackNodeList = request.ResponseXML.GetElementsByTagName("UserData");
                    for (int index = 0; index < shelfStackNodeList.Length; index++)
                    {
                        TafitiUser tafitiUser = TafitiUser.CreateFromXmlNode(shelfStackNodeList[index]);
                        TafitiUserManager.CacheUser(tafitiUser);
                    }
                }
            }
        }

        static public void GetUserListRequestByEmailHash(string[] emailHashes)
        {
            ArrayList missingUsers = new ArrayList();
            foreach (string emailHash in emailHashes)
            {
                if (!TafitiUserManager.EmailHashCache.ContainsKey(emailHash))
                {
                    missingUsers.Add(emailHash);
                }
            }
            if (missingUsers.Length == 0) { return; }

            string body = "emailHashes=" + missingUsers.Join(",");

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetUserListByEmailHash", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            XMLNodeList shelfStackNodeList = request.ResponseXML.GetElementsByTagName("UserData");
            for (int index = 0; index < shelfStackNodeList.Length; index++)
            {
                TafitiUser tafitiUser = TafitiUser.CreateFromXmlNode(shelfStackNodeList[index]);
                TafitiUserManager.CacheUser(tafitiUser);
            }
        }

        static public void UpdateUserDetails(string displayName, string email)
        {
            string body = "displayName=" + displayName.Escape() + "&emailHash=" + Utilities.Hash(email).Escape();

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/UpdateUserDetails", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);
        }

        static public TafitiUser GetUserByEmailHash(string emailHash)
        {
            if (TafitiUserManager.EmailHashCache.ContainsKey(emailHash))
            {
                return (TafitiUser)TafitiUserManager.EmailHashCache[emailHash];
            }

            string body = "emailHash=" + emailHash;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetUserByEmailHash", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            TafitiUser user;

            if (request.ResponseXML.LastChild.Text != "")
            {
                user = TafitiUser.CreateFromXmlNode(request.ResponseXML.LastChild);
            }
            else
            {
                user = new TafitiUser();
                user.EmailHash = emailHash;
                user.UserID = emailHash;
            }

            TafitiUserManager.CacheUser(user);
            return user;
        }

        static public TafitiUser GetUser(string userID)
        {
            if (TafitiUserManager.UsersCache.ContainsKey(userID))
            {
                return (TafitiUser)TafitiUserManager.UsersCache[userID];
            }

            string body = "userID=" + userID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetUser", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            TafitiUser user = TafitiUser.CreateFromXmlNode(request.ResponseXML.LastChild);
            TafitiUserManager.CacheUser(user);
            return user;
        }

        static public void CacheUser(TafitiUser tafitiUser)
        {
            if (!TafitiUserManager.UsersCache.ContainsKey(tafitiUser.UserID))
            {
                TafitiUserManager.UsersCache[tafitiUser.UserID] = tafitiUser;
                TafitiUserManager.EmailHashCache[tafitiUser.EmailHash] = tafitiUser;
            }
        }

    }
}
