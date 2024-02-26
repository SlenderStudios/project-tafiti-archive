using System;
using System.DHTML;
using ScriptFX;
using ScriptFX.UI;
using Microsoft.Live.Core;
using Microsoft.Live.Messenger;
using Microsoft.Live.Messenger.UI;

namespace WLQuickApps.Tafiti.Scripting
{
    public class MessengerController
    {
        private User _localUser;
        private SignInControl _signInControl;
        private ArrayList _contacts;
        private Dictionary _contactLookup;

        public bool IsSignedIn
        {
            get { return ((this._localUser != null) && (this._localUser.Address.Presence.Status == PresenceStatus.Online)); }
        }

        public User LoggedInUser
        {
            get
            {
                return this._localUser;
            }
        }

        /// <summary>
        ///     Creates a Sign In control, calls GetControlReferences
        /// </summary>
        /// <param name="args">List of arguments taken in via Script# tags</param>
        public MessengerController()
        {
            if (Utilities.UserIsLoggedIn())
            {
                this._contacts = new ArrayList();
                this._contactLookup = new Dictionary();

                Window.AttachEvent("onbeforeunload", this.Document_Unload);

                this._signInControl = new SignInControl(Constants.SignInFrameID, Utilities.GetSiteUrlRoot() + "/privacy.html",
                                                        Utilities.GetSiteUrlRoot() + "/Channel.html", "");
                this._signInControl.AuthenticationCompleted += this.AuthenticationCompleted;
            }
        }

        public void AddContact(string address)
        {
            this._localUser.AddContact(address, "I have added you as a contact through Tafiti.", null);
        }

        /// <summary>
        ///     Event handler called when the Sign In control finishes authenticating,
        ///     after the user initiates the sign-in process
        /// </summary>
        /// <param name="sender">Reference to the sender object (Sign In control)</param>
        /// <param name="e">AuthenticationCompletedEventArgs event arguments</param>
        public void AuthenticationCompleted(Object sender, AuthenticationCompletedEventArgs e)
        {
            this._localUser = new User(e.Identity);
            this._localUser.SignInCompleted += this.SignInCompleted;
            this._localUser.SignIn(null);
        }

        /// <summary>
        ///     Event handler called after the complete sign-in process is finished;
        ///     e.ResultCode will indicate whether the process succeeded
        /// </summary>
        /// <param name="sender">Reference to the sender object (Sign In control)</param>
        /// <param name="e">SignInCompletedEventArgs event arguments</param>
        public void SignInCompleted(Object sender, SignInCompletedEventArgs e)
        {
            if (e.ResultCode == SignInResultCode.Success)
            {
                // Set status to online and display appropriate content
                this._localUser.Address.Presence.Status = PresenceStatus.Online;

                // Wire up the logged in user's Presence changed to catch incomind data
                this._localUser.Presence.PropertyChanged += this.PresencePropertyChanged;

                ArrayList emailHashes = new ArrayList();
                foreach (Contact contact in this._localUser.Contacts)
                {
                    emailHashes.Add(Utilities.Hash(contact.CurrentAddress.Address));
                }
                TafitiUserManager.GetUserListRequestByEmailHash((string[]) emailHashes.Extract(0));

                // Generate list of contacts
                foreach (Contact contact in this._localUser.Contacts)
                {
                    string contactHash = Utilities.Hash(contact.CurrentAddress.Address);
                    if (!this._contactLookup.ContainsKey(contactHash))
                    {
                        ContactElement contactElement = new ContactElement(contact);
                        this._contacts.Add(contactElement);
                        this._contactLookup[contactHash] = contactElement;
                    }
                }

                if (this._localUser.MessageFactory == null)
                {
                    this._localUser.MessageFactory = new TafitiUpdateMessageFactory();
                    if (!this._localUser.MessageFactory.IsRegistered(Constants.TafitiUpdateMessageID))
                    {
                        this._localUser.MessageFactory.Register(Constants.TafitiUpdateMessageID);
                    }
                }

                this._localUser.Contacts.CollectionChanged += new NotifyCollectionChangedEventHandler(Contacts_CollectionChanged);
                this._localUser.SignOutCompleted += new SignOutCompletedEventHandler(this._localUser_SignOutCompleted);
                this._localUser.PendingContacts.CollectionChanged += new NotifyCollectionChangedEventHandler(PendingContacts_CollectionChanged);

                this._localUser.Conversations.CollectionChanged += new NotifyCollectionChangedEventHandler(Conversations_CollectionChanged);

                InteropManager.MessengerStatusChanged(true);
            }
            else
            {
                // Failed unexpectedly; handle somehow.
            }
        }

        void PendingContacts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (int lcv = e.NewStartingIndex; lcv < e.NewItems.Length; lcv++)
            {
                PendingContact pendingContact = (PendingContact)e.NewItems[lcv];

                InteropManager.PopAcceptContactDialog(pendingContact.IMAddress.Presence.DisplayName, pendingContact.IMAddress.Address, pendingContact.InviteMessage);
            }
        }

        void Document_Unload()
        {
            if (this.IsSignedIn)
            {
                this._localUser.SignOut(SignOutLocations.Local, false);
            }
        }

        void Conversations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (int lcv = 0; lcv < e.NewItems.Length; lcv++)
            {
                Conversation conversation = (Conversation) e.NewItems[lcv];
                conversation.MessageReceived += new MessageReceivedEventHandler(conversation_MessageReceived);
            }            
        }

        void conversation_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Message.Type)
            {
                case MessageType.ApplicationMessage:
                    TafitiUpdateMessage updateMessage = (TafitiUpdateMessage)e.Message;
                    if (updateMessage != null)
                    {
                        ShelfStackManager.UpdateShelfStack(updateMessage.ShelfStackID);
                    }
                    
                    break;
                    
                case MessageType.NudgeMessage: /* Ignore nudges */ break;

                case MessageType.TextMessage:
                    TextMessage textMessage = (TextMessage)e.Message;

                    string displayName = textMessage.Sender.Presence.DisplayName;
                    if (string.IsNullOrEmpty(displayName))
                    {
                        displayName = textMessage.Sender.Presence.IMAddress.Address;
                    }
                    InteropManager.OnIncomingTextMessage(Utilities.Hash(textMessage.Sender.Address), displayName, textMessage.Text);
                    break;

                default: throw new Exception("Unknown message type");
            }
        }

        /// <summary>
        ///     Event Handler in case the user adds a new contact offsite while the app is running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Contacts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            for (int lcv = 0; lcv < e.NewItems.Length; lcv++)
            {
                Contact contact = (Contact) e.NewItems[lcv];
                string contactHash = Utilities.Hash(contact.CurrentAddress.Address);
                ContactElement contactElement = (ContactElement) this._contactLookup[contactHash];
                if (contactElement == null)
                {
                    contactElement = new ContactElement(contact);
                    this._contacts.Add(contactElement);
                    this._contactLookup[contactHash] = contactElement;
                }
            }
        }

        /// <summary>
        ///     Sign out completed, refresh page to return to start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _localUser_SignOutCompleted(object sender, SignOutCompletedEventArgs e)
        {
            if (this._localUser != null)
            {
                this._localUser.Dispose();
                this._localUser = null;
            }

            InteropManager.MessengerStatusChanged(false);

            Window.Navigate(Window.Location.Href);
        }

        /// <summary>
        ///     Event handler called whenever the Presence of the logged in user changes
        ///     and an update has been recieved.
        /// </summary>
        /// <param name="sender">Reference to the sender object (User)</param>
        /// <param name="e">PropertyChangedEventArgs event arguments</param>
        public void PresencePropertyChanged(object sender, Microsoft.Live.Core.PropertyChangedEventArgs e)
        {
            IMAddressPresence presence = (IMAddressPresence)sender;

            if ((e.PropertyName == "Status") && (presence.IMAddress.Address == this._localUser.Address.Address))
            {
                // This seems to be the only way we can be updated when the local user signs out.
                InteropManager.MessengerStatusChanged(false);
            }
            else
            {
                TafitiUserManager.UpdateUserDetails(presence.DisplayName, presence.IMAddress.Address);

                // Kick off a master get just in case this is the first time we learn of the
                // user's email address and need to retrieve shelf stacks they were invited to.                
                ShelfStackManager.BeginGetMasterData();
            }
        }
    }
}
