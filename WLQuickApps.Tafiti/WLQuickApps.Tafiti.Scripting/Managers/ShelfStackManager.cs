using System;
using System.DHTML;
using ScriptFX;
using System.XML;

namespace WLQuickApps.Tafiti.Scripting
{
    static public class ShelfStackManager
    {
        static public ArrayList MyShelfStacks
        {
            get
            {
                if (ShelfStackManager._myShelfStacks == null)
                {
                    ShelfStackManager._myShelfStacks = new ArrayList();
                }

                return ShelfStackManager._myShelfStacks;
            }
        }
        static private ArrayList _myShelfStacks;

        static private Dictionary ShelfStackItemCache
        {
            get
            {
                if (ShelfStackManager._shelfStackItemCache == null)
                {
                    ShelfStackManager._shelfStackItemCache = new Dictionary();
                }
                return ShelfStackManager._shelfStackItemCache;
            }
        }
        static private Dictionary _shelfStackItemCache;

        static private Dictionary ShelfStackCache
        {
            get
            {
                if (ShelfStackManager._shelfStackCache == null)
                {
                    ShelfStackManager._shelfStackCache = new Dictionary();
                }
                return ShelfStackManager._shelfStackCache;
            }
        }
        static private Dictionary _shelfStackCache;

        static private ArrayList OutstandingMasterDataRequests
        {
            get
            {
                if (ShelfStackManager._outstandingMasterDataRequests == null)
                {
                    ShelfStackManager._outstandingMasterDataRequests = new ArrayList();
                }
                return ShelfStackManager._outstandingMasterDataRequests;
            }
        }
        static private ArrayList _outstandingMasterDataRequests;

        static private ArrayList OutstandingShelfStackRequests
        {
            get
            {
                if (ShelfStackManager._outstandingShelfStackRequests == null)
                {
                    ShelfStackManager._outstandingShelfStackRequests = new ArrayList();
                }
                return ShelfStackManager._outstandingShelfStackRequests;
            }
        }
        static private ArrayList _outstandingShelfStackRequests;

        static private ArrayList OutstandingShelfStackItemRequests
        {
            get
            {
                if (ShelfStackManager._outstandingShelfStackItemRequests == null)
                {
                    ShelfStackManager._outstandingShelfStackItemRequests = new ArrayList();
                }
                return ShelfStackManager._outstandingShelfStackItemRequests;
            }
        }
        static private ArrayList _outstandingShelfStackItemRequests;

        static public void BeginGetMasterData()
        {
            string body = "";

            XMLHttpRequest request = new XMLHttpRequest();
            // TODO: Revert this back to async if thge UI fits
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetMasterData", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Onreadystatechange = new Callback(ShelfStackManager.EndMasterDataRequest);
            ShelfStackManager.OutstandingMasterDataRequests.Add(request);
            request.Send(body);
        }

        static private void EndMasterDataRequest()
        {
            for (int lcv = 0; lcv < ShelfStackManager.OutstandingMasterDataRequests.Length; lcv++)
            {
                XMLHttpRequest request = (XMLHttpRequest)ShelfStackManager.OutstandingMasterDataRequests[lcv];
                if (request.ReadyState == 4)
                {
                    ShelfStackManager.OutstandingMasterDataRequests.RemoveAt(lcv);
                    lcv--;

                    XMLNodeList shelfStackNodeList = request.ResponseXML.GetElementsByTagName("UserData");
                    for (int index = 0; index < shelfStackNodeList.Length; index++)
                    {
                        TafitiUser tafitiUser = TafitiUser.CreateFromXmlNode(shelfStackNodeList[index]);
                        TafitiUserManager.CacheUser(tafitiUser);
                    }

                    shelfStackNodeList = request.ResponseXML.GetElementsByTagName("CommentData");
                    for (int index = 0; index < shelfStackNodeList.Length; index++)
                    {
                        Comment comment = Comment.CreateFromXmlNode(shelfStackNodeList[index]);
                        CommentManager.CacheComment(comment);
                    }

                    shelfStackNodeList = request.ResponseXML.GetElementsByTagName("ShelfStackItemData");
                    for (int index = 0; index < shelfStackNodeList.Length; index++)
                    {
                        ShelfStackItem shelfStackItem = ShelfStackItem.CreateFromXmlNode(shelfStackNodeList[index]);
                        ShelfStackManager.ShelfStackItemCache[shelfStackItem.ShelfStackItemID] = shelfStackItem;
                    }

                    shelfStackNodeList = request.ResponseXML.GetElementsByTagName("ShelfStackData");
                    for (int index = 0; index < shelfStackNodeList.Length; index++)
                    {
                        ShelfStack shelfStack = ShelfStack.CreateFromXmlNode(shelfStackNodeList[index]);

                        bool found = false;
                        for (int item = 0; item < ShelfStackManager.MyShelfStacks.Length; item++)
                        {
                            if (shelfStack.ShelfStackID == ((ShelfStack)ShelfStackManager.MyShelfStacks[item]).ShelfStackID)
                            {
                                ShelfStackManager.MyShelfStacks[item] = shelfStack;
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            ShelfStackManager.MyShelfStacks.Add(shelfStack);
                        }

                        ShelfStackManager.ShelfStackCache[shelfStack.ShelfStackID] = shelfStack;
                        InteropManager.UpdateShelfStack(shelfStack);
                    }
                }
            }
        }

        static public void BeginAddShelfStack(string label)
        {
            string body = "label=" + label.Escape();

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/AddShelfStack", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Onreadystatechange = new Callback(ShelfStackManager.EndShelfStackRequest);
            ShelfStackManager.OutstandingShelfStackRequests.Add(request);
            request.Send(body);
        }

        static private void EndShelfStackRequest()
        {
            for (int lcv = 0; lcv < ShelfStackManager.OutstandingShelfStackRequests.Length; lcv++)
            {
                XMLHttpRequest request = (XMLHttpRequest)ShelfStackManager.OutstandingShelfStackRequests[lcv];
                if (request.ReadyState == 4)
                {
                    ShelfStackManager.OutstandingShelfStackRequests.RemoveAt(lcv);
                    lcv--;

                    ShelfStack shelfStack = ShelfStack.CreateFromXmlNode(request.ResponseXML.LastChild);

                    bool found = false;
                    for (int index = 0; index < ShelfStackManager.MyShelfStacks.Length; index++)
                    {
                        if (shelfStack.ShelfStackID == ((ShelfStack)ShelfStackManager.MyShelfStacks[index]).ShelfStackID)
                        {
                            ShelfStackManager.MyShelfStacks[index] = shelfStack;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        ShelfStackManager.MyShelfStacks.Add(shelfStack);
                    }

                    ShelfStackManager.ShelfStackCache[shelfStack.ShelfStackID] = shelfStack;

                    ShelfStackManager.SendShelfStackUpdate(shelfStack);
                    InteropManager.UpdateShelfStack(shelfStack);
                }
            }
        }

        static public ShelfStack AddShelfStack(string label)
        {
            string body = "label=" + label.Escape();

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/AddShelfStack", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStack shelfStack = ShelfStack.CreateFromXmlNode(request.ResponseXML.LastChild);
            ShelfStackManager.MyShelfStacks.Add(shelfStack);
            ShelfStackManager.ShelfStackCache[shelfStack.ShelfStackID] = shelfStack;

            return shelfStack;
        }

        static public void LeaveShelfStack(string shelfStackID)
        {
            if (!ShelfStackManager.ShelfStackCache.ContainsKey(shelfStackID)) { return; }

            ShelfStack shelfStack = ShelfStackManager.GetShelfStack(shelfStackID);

            ShelfStackManager.ShelfStackCache.Remove(shelfStackID);

            string body = "shelfStackID=" + shelfStackID.Escape();

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/LeaveShelfStack", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStackManager.SendShelfStackUpdate(shelfStack);
        }

        static public void BeginAddShelfStackItem(string shelfStackID, string domain, string title, string description, string url, string imageUrl, int width, int height, string source)
        {
            string body = "shelfStackID=" + shelfStackID.Escape() +
                            "&domain=" + domain.Escape() +
                            "&title=" + title.Escape() +
                            "&description=" + description.Escape() +
                            "&url=" + url.Escape() +
                            "&imageUrl=" + imageUrl.Escape() +
                            "&width=" + width.ToString() +
                            "&height=" + height.ToString() +
                            "&source=" + source.Escape();

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/AddShelfStackItem", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Onreadystatechange = new Callback(ShelfStackManager.EndShelfStackItemRequest);
            ShelfStackManager.OutstandingShelfStackItemRequests.Add(request);
            request.Send(body);
        }

        static private void EndShelfStackItemRequest()
        {
            for (int lcv = 0; lcv < ShelfStackManager.OutstandingShelfStackItemRequests.Length; lcv++)
            {
                XMLHttpRequest request = (XMLHttpRequest)ShelfStackManager.OutstandingShelfStackItemRequests[lcv];
                if (request.ReadyState == 4)
                {
                    ShelfStackManager.OutstandingShelfStackItemRequests.RemoveAt(lcv);
                    lcv--;

                    ShelfStackItem shelfStackItem = ShelfStackItem.CreateFromXmlNode(request.ResponseXML.LastChild);
                    ShelfStack shelfStack = ShelfStackManager.GetShelfStack(shelfStackItem.ShelfStackID);

                    ShelfStackManager.ShelfStackItemCache[shelfStackItem.ShelfStackItemID] = shelfStackItem;

                    bool found = false;
                    for (int index = 0; index < shelfStack.ShelfStackItems.Length; index++)
                    {
                        if (shelfStackItem.ShelfStackItemID == shelfStack.ShelfStackItems[index].ShelfStackItemID)
                        {
                            shelfStack.ShelfStackItems[index] = shelfStackItem;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        shelfStack.ShelfStackItems = (ShelfStackItem[]) shelfStack.ShelfStackItems.Concat(new object[] { shelfStackItem });
                    }

                    InteropManager.UpdateShelfStack(shelfStack);
                    ShelfStackManager.SendShelfStackUpdate(shelfStack);
                }
            }
        }


        static public string AddShelfStackItem(string shelfStackID, string domain, string title, string description, string url, string imageUrl, int width, int height, string source)
        {
            string body = "shelfStackID=" + shelfStackID.Escape() +
                            "&domain=" + domain.Escape() +
                            "&title=" + title.Escape() +
                            "&description=" + description.Escape() +
                            "&url=" + url.Escape() +
                            "&imageUrl=" + imageUrl.Escape() +
                            "&width=" + width.ToString() +
                            "&height=" + height.ToString() +
                            "&source=" + source.Escape();

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/AddShelfStackItem", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStackManager.UpdateShelfStack(shelfStackID); 
            ShelfStackManager.SendShelfStackUpdate(ShelfStackManager.GetShelfStack(shelfStackID));

            return request.ResponseXML.LastChild.Text;
        }

        static private DateTime _lastUpdate = DateTime.Now;
        static public bool CheckForUpdates()
        {
            string body = "lastUpdate=" + ShelfStackManager._lastUpdate.ToLocaleString().Escape();

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/CheckForUpdates", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStackManager._lastUpdate = DateTime.Now;

            // TODO: This is sort of a hack. Should probably be more elegant. Need to search for ChildNodes[1] and fix everywhere.
            //
            return bool.Parse(request.ResponseXML.LastChild.Text);
        }

        static public void BeginUpdateShelfStack(string shelfStackID)
        {
            string body = "shelfStackID=" + shelfStackID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetShelfStack", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Onreadystatechange = new Callback(ShelfStackManager.EndShelfStackRequest);
            ShelfStackManager.OutstandingShelfStackRequests.Add(request);
            request.Send(body);
        }

        static public void UpdateShelfStack(string shelfStackID)
        {
            string body = "shelfStackID=" + shelfStackID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetShelfStack", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStack shelfStack = ShelfStack.CreateFromXmlNode(request.ResponseXML.LastChild);

            if (ShelfStackManager.ShelfStackCache.ContainsKey(shelfStackID))
            {
                for (int lcv = 0; lcv < ShelfStackManager.MyShelfStacks.Length; lcv++)
                {
                    ShelfStack existingShelfStack = (ShelfStack)ShelfStackManager.MyShelfStacks[lcv];
                    if (existingShelfStack.ShelfStackID == shelfStackID)
                    {
                        ShelfStackManager._myShelfStacks[lcv] = shelfStack;
                        break;
                    }
                }
            }
            else
            {
                ShelfStackManager._myShelfStacks.Add(shelfStack);
            }

            ShelfStackManager.ShelfStackCache[shelfStackID] = shelfStack;

            InteropManager.UpdateShelfStack(shelfStack);
        }


        static public ShelfStack GetShelfStack(string shelfStackID)
        {
            if (ShelfStackManager.ShelfStackCache.ContainsKey(shelfStackID))
            {
                return (ShelfStack)ShelfStackManager.ShelfStackCache[shelfStackID];
            }

            string body = "shelfStackID=" + shelfStackID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetShelfStack", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStack shelfStack = ShelfStack.CreateFromXmlNode(request.ResponseXML.LastChild);
            ShelfStackManager.MyShelfStacks.Add(shelfStack);
            ShelfStackManager.ShelfStackCache[shelfStackID] = shelfStack;

            return shelfStack;
        }

        static public ShelfStackItem GetShelfStackItem(string shelfStackItemID)
        {
            shelfStackItemID = shelfStackItemID.ToLowerCase();
            if (ShelfStackManager.ShelfStackItemCache.ContainsKey(shelfStackItemID))
            {
                return (ShelfStackItem)ShelfStackManager.ShelfStackItemCache[shelfStackItemID];
            }

            string body = "shelfStackItemID=" + shelfStackItemID;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/GetShelfStackItem", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStackItem shelfItem = ShelfStackItem.CreateFromXmlNode(request.ResponseXML.LastChild);
            ShelfStackManager.ShelfStackItemCache[shelfStackItemID] = shelfItem;
            return shelfItem;
        }

        static public void AddUserToShelfStack(string shelfStackID, string emailHash)
        {
            ShelfStack shelfStack = ShelfStackManager.GetShelfStack(shelfStackID);
            foreach (TafitiUser owner in shelfStack.Owners)
            {
                if (owner.EmailHash == emailHash) { return; }
            }
            TafitiUser user = TafitiUserManager.GetUserByEmailHash(emailHash);

            string body = "shelfStackID=" + shelfStackID + "&emailHash=" + emailHash;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/AddUserToShelfStack", false);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStackManager.BeginUpdateShelfStack(shelfStack.ShelfStackID);
            MessengerManager.SendShelfStackInvite(emailHash, shelfStack);
        }

        static public void SetShelfStackLabel(string shelfStackID, string label)
        {
            string body = "shelfStackID=" + shelfStackID + "&label=" + label;

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/SetShelfStackLabel", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStack shelfStack = ShelfStackManager.GetShelfStack(shelfStackID);
            ShelfStackManager.BeginUpdateShelfStack(shelfStackID);
            ShelfStackManager.SendShelfStackUpdate(shelfStack);
        }

        static public void RemoveShelfStackItem(string shelfStackItemID)
        {
            if (!ShelfStackManager.ShelfStackItemCache.ContainsKey(shelfStackItemID)) { return; }

            ShelfStackItem shelfStackItem = ShelfStackManager.GetShelfStackItem(shelfStackItemID);
            ShelfStack shelfStack = ShelfStackManager.GetShelfStack(shelfStackItem.ShelfStackID);

            string body = "shelfStackItemID=" + shelfStackItemID;
            if (!ShelfStackManager.ShelfStackItemCache.ContainsKey(shelfStackItemID)) { return; }

            XMLHttpRequest request = new XMLHttpRequest();
            request.Open("POST", Utilities.GetSiteUrlRoot() + "/SiteService.asmx/RemoveShelfStackItem", true);
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Content-Length", body.Length.ToString());
            request.Send(body);

            ShelfStackManager.BeginUpdateShelfStack(shelfStack.ShelfStackID);
            ShelfStackManager.SendShelfStackUpdate(shelfStack);
        }

        static public void SendShelfStackUpdate(ShelfStack shelfStack)
        {
            bool sendMessage = false;
            foreach (TafitiUser user in shelfStack.Owners)
            {
                if (user.IsOnline && !user.IsLoggedInUser)
                {
                    sendMessage = true;
                    break;
                }
            }
            if (!sendMessage) { return; }

            if (TafitiUserManager.AlwaysSendMessages)
            {
                ShelfStackManager.SendShelfStackUpdateApproved(shelfStack);
            }
            else
            {
                InteropManager.PopIMConsentDialog(shelfStack);
            }
        }

        static public void SendShelfStackUpdateApproved(ShelfStack shelfStack)
        {
            foreach (TafitiUser user in shelfStack.Owners)
            {
                if (user.IsOnline && !user.IsLoggedInUser)
                {
                    MessengerManager.SendUpdateMessage(user.EmailHash, shelfStack);
                }
            }
        }
    }
}
