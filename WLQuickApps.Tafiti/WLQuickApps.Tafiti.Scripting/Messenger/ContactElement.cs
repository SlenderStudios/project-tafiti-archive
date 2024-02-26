using System;
using ScriptFX;
using Microsoft.Live.Core;
using Microsoft.Live.Messenger;
using Microsoft.Live.Messenger.UI;

namespace WLQuickApps.Tafiti.Scripting
{
    public class ContactElement
    {
        private Contact _contact;
        private SJ.ContactVisual _visual;

        public string EmailHash
        {
            get { return Utilities.Hash(this._contact.CurrentAddress.Address); }
        }

        /// <summary>
        ///     Creates visual object (implemented in native js) with details populated;
        ///     sets up event handler for presence property changes
        /// </summary>
        /// <param name="contact"></param>
        public ContactElement(Contact contact)
        {
            this._contact = contact;
            TafitiUser tafitiUser = TafitiUserManager.GetUserByEmailHash(Utilities.Hash(contact.CurrentAddress.Address));

            this._visual = new SJ.ContactVisual(tafitiUser);
            this._contact.CurrentAddress.Presence.PropertyChanged += this.PropertyChanged;
        }

        public void Remove()
        {
            this._visual.Remove();
        }

        /// <summary>
        ///     Refreshes the contents of the ListItem object upon receiving
        ///     fresh data
        /// </summary>
        /// <param name="sender">Reference to the sender object (self)</param>
        /// <param name="e">PropertyChangedEventArgs event arguments</param>
        private void PropertyChanged(Object sender, Microsoft.Live.Core.PropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this._contact.CurrentAddress.Presence.DisplayName))
            {
                this._visual.DisplayName = this._contact.CurrentAddress.Presence.DisplayName;
            }
            else
            {
                this._visual.DisplayName = this._contact.CurrentAddress.Presence.IMAddress.Address;
            }

            this._visual.IsOnline = (this._contact.CurrentAddress.Presence.Status != PresenceStatus.Offline);

            this._visual.UpdateUI();
        }
    }
}
