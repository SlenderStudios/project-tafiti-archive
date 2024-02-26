using System;
using System.DHTML;
using ScriptFX;
using System.XML;

using Microsoft.Live.Messenger;

namespace WLQuickApps.Tafiti.Scripting
{
    static public class MessengerManager
    {
        static public MessengerController MyMessengerController { get { return MessengerManager._myMessengerController; } }
        static private MessengerController _myMessengerController;

        static public bool IsSignedIn
        {
            get
            {
                return ((MessengerManager._myMessengerController != null) && MessengerManager._myMessengerController.IsSignedIn);
            }
        }

        static public void StartMessenger()
        {
            if (MessengerManager._myMessengerController == null)
            {
                MessengerManager._myMessengerController = new MessengerController();
            }
        }

        static public Contact GetMessengerContactByEmailHash(string emailHash)
        {
            MessengerManager.VerifyUserIsLoggedInToMessenger();

            foreach (Contact contact in MessengerManager.MyMessengerController.LoggedInUser.Contacts)
            {
                if (Utilities.Hash(contact.CurrentAddress.Address) == emailHash)
                {
                    return contact;
                }
            }

            return null;
        }

        static public bool SupportsTafitiMessages(string emailHash)
        {
            Contact contact = MessengerManager.GetMessengerContactByEmailHash(emailHash);
            if ((contact == null) || !contact.CurrentAddress.IsOnline) { return false; }

            if (!contact.CurrentAddress.Capabilities.SupportsMessageType(MessageType.ApplicationMessage))
            {
                return false;
            }

            // TODO: Figure out if/when this should be disposed.
            Conversation conversation = MessengerManager.MyMessengerController.LoggedInUser.Conversations.Create(contact.CurrentAddress);
            return conversation.SupportsApplicationMessageType(Constants.TafitiUpdateMessageID);
        }
        
        static public void SendUpdateMessage(string emailHash, ShelfStack shelfStack)
        {
            MessengerManager.VerifyUserIsLoggedInToMessenger();

            if (MessengerManager.SupportsTafitiMessages(emailHash))
            {
                Contact contact = MessengerManager.GetMessengerContactByEmailHash(emailHash);
                // TODO: Figure out if/when this should be disposed.
                Conversation conversation = MessengerManager.MyMessengerController.LoggedInUser.Conversations.Create(contact.CurrentAddress);
                conversation.SendMessage(new TafitiUpdateMessage(shelfStack.ShelfStackID), null);
            }
            else
            {
                string message = "I have updated the shelf stack \"" + shelfStack.Label + "\" at " + Utilities.GetSiteUrlRoot();

                MessengerManager.SendTextMessage(emailHash, message);
            }
        }

        static public void SendTextMessage(string emailHash, string message)
        {            
            MessengerManager.VerifyUserIsLoggedInToMessenger();

            Contact contact = MessengerManager.GetMessengerContactByEmailHash(emailHash);
            if ((contact == null) || !contact.CurrentAddress.IsOnline) { return; }

            // TODO: Figure out when this should be disposed.
            Conversation conversation = MessengerManager.MyMessengerController.LoggedInUser.Conversations.Create(contact.CurrentAddress);
            conversation.SendMessage(new TextMessage(message, TextMessageFormat.DefaultFormat), null);
        }


        static public void SendShelfStackInvite(string emailHash, ShelfStack shelfStack)
        {
            MessengerManager.VerifyUserIsLoggedInToMessenger();

            string message = "I have invited you to join the shelf stack \"" + shelfStack.Label + "\" at " + Utilities.GetSiteUrlRoot();

            bool found = false;
            foreach (TafitiUser tafitiUser in shelfStack.Owners)
            {
                if (!tafitiUser.IsOnline || tafitiUser.IsLoggedInUser) { continue; }

                MessengerManager.SendUpdateMessage(emailHash, shelfStack);
            }

            if (MessengerManager.SupportsTafitiMessages(emailHash))
            {
                MessengerManager.SendUpdateMessage(emailHash, shelfStack);
            }
            else
            {
                MessengerManager.SendTextMessage(emailHash, message);
            }
        }

        static public bool IsContact(string emailHash)
        {
            foreach (Contact contact in MessengerManager.MyMessengerController.LoggedInUser.Contacts)
            {
                if (Utilities.Hash(contact.CurrentAddress.Address) == emailHash)
                {
                    return true;
                }
            }

            return false;
        }

        static public string GetDisplayName(string emailHash)
        {
            TafitiUser user = TafitiUserManager.GetUserByEmailHash(emailHash);
            if ((user != null) && (user.MessengerAddress != null))
            {
                return user.DisplayName;
            }

            return "Unknown";
        }

        static public void AddContact(string emailHash)
        {
            // Check first to see if this is a response to a pending request.
            foreach (PendingContact pendingContact in MessengerManager.MyMessengerController.LoggedInUser.PendingContacts)
            {
                if (Utilities.Hash(pendingContact.IMAddress.Address) == emailHash)
                {
                    MessengerManager.MyMessengerController.AddContact(pendingContact.IMAddress.Address);
                    return;
                }
            }

            // Check next to see if we're initiating the request in our IM dialog.
            foreach (Conversation conversation in MessengerManager.MyMessengerController.LoggedInUser.Conversations)
            {
                foreach (IMAddress imAddress in conversation.Roster)
                    if (Utilities.Hash(imAddress.Address) == emailHash)
                    {
                        MessengerManager.MyMessengerController.AddContact(imAddress.Address);
                        return;
                    }
            }

            // Finally, check to see if this person is already known to us, but not a contact somehow.
            TafitiUser user = TafitiUserManager.GetUserByEmailHash(emailHash);
            if ((user != null) && (user.MessengerAddress != null))
            {
                MessengerManager.MyMessengerController.AddContact(user.MessengerAddress.Address);
            }

            // TODO: If we got here it's because we couldn't figure out who the other user is.
        }

        static private void VerifyUserIsLoggedInToMessenger()
        {
            if (!MessengerManager.MyMessengerController.IsSignedIn) { throw new Exception("You must be logged in to messenger to do this"); }
        }
    }
}
