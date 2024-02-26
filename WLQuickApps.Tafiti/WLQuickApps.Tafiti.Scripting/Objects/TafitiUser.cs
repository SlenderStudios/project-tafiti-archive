using System;
using ScriptFX;
using System.XML;
using Microsoft.Live.Messenger;

namespace WLQuickApps.Tafiti.Scripting
{
    public class TafitiUser
    {
        public string UserID;
        public string EmailHash;
        public string MessengerPresenceID;
        public string DisplayName
        {
            get
            {
                if ((this.MessengerAddress != null) && !string.IsNullOrEmpty(this.MessengerAddress.Presence.DisplayName))
                {
                    if (!string.IsNullOrEmpty(this.MessengerAddress.Presence.DisplayName))
                    {
                        return this.MessengerAddress.Presence.DisplayName;
                    }

                    return this.MessengerAddress.Presence.IMAddress.Address;
                }

                return this._displayName;
            }
        }
        private string _displayName;

        public IMAddress MessengerAddress
        {
            get
            {
                if (!MessengerManager.MyMessengerController.IsSignedIn)
                {
                    this._messengerAddress = null;
                }

                if ((this._messengerAddress == null) && (MessengerManager.MyMessengerController.IsSignedIn))
                {
                    if (string.Compare(this.EmailHash,
                        Utilities.Hash(MessengerManager.MyMessengerController.LoggedInUser.Address.Address),
                        true) == 0)
                    {
                        this._messengerAddress = MessengerManager.MyMessengerController.LoggedInUser.Address;                        
                    }
                    else
                    {
                        foreach (Contact contact in MessengerManager.MyMessengerController.LoggedInUser.Contacts)
                        {
                            if (string.Compare(this.EmailHash, Utilities.Hash(contact.CurrentAddress.Address), true) == 0)
                            {
                                this._messengerAddress = contact.CurrentAddress;
                                break;
                            }
                        }
                    }
                }
                
                return this._messengerAddress;
            }
        }
        private IMAddress _messengerAddress;

        public bool IsLoggedInUser
        {
            get
            {
                return ((TafitiUserManager.LoggedInUser != null) && (this.EmailHash == TafitiUserManager.LoggedInUser.EmailHash));
            }
        }

        public bool IsOnline
        {
            get
            {
                return ((this.MessengerAddress != null) && this.MessengerAddress.IsOnline);
            }
        }

        public TafitiUser()
        {
        }

        static public TafitiUser CreateFromXmlNode(XMLNode node)
        {
            TafitiUser user = new TafitiUser();

            for (int lcv = 0; lcv < node.ChildNodes.Length; lcv++)
            {
                XMLNode childNode = node.ChildNodes[lcv];
                if (string.IsNullOrEmpty(childNode.BaseName)) { continue; }

                switch (childNode.BaseName.ToLowerCase())
                {
                    case "userid": user.UserID = childNode.Text; break;
                    case "displayname": user._displayName = childNode.Text; break;
                    case "emailhash": user.EmailHash = childNode.Text; break;
                    case "messengerpresenceid": user.MessengerPresenceID = childNode.Text; break;
                    default: break;
                }
            }
            
            return user;
        }
    }
}
