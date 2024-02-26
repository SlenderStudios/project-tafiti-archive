using System;
using ScriptFX;
using System.DHTML;

using Microsoft.Live.Core;
using Microsoft.Live.Messenger;
using Microsoft.Live.Messenger.Messaging;
using Microsoft.Live.Messenger.UI;

namespace WLQuickApps.Tafiti.Scripting
{
    public class TafitiUpdateMessageFactory : ApplicationMessageFactory
    {
        public TafitiUpdateMessageFactory()
            : base(MessengerManager.MyMessengerController.LoggedInUser)
        {
        }

        protected override ApplicationMessage Deserialize(IMAddress sender, string id, string content)
        {
            return new TafitiUpdateMessage(content);
        }

        protected override string Serialize(ApplicationMessage message)
        {
            return ((TafitiUpdateMessage) message).ShelfStackID;
        }
    }
}
