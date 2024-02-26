using System;
using ScriptFX;
using System.DHTML;

using Microsoft.Live.Core;
using Microsoft.Live.Messenger;
using Microsoft.Live.Messenger.Messaging;
using Microsoft.Live.Messenger.UI;

namespace WLQuickApps.Tafiti.Scripting
{    
    public class TafitiUpdateMessage : ApplicationMessage
    {
        public string ShelfStackID;

        public TafitiUpdateMessage(string shelfStackID)
        {
            this.ShelfStackID = shelfStackID;
        }

        public override string Id
        {
            get 
            {
                return Constants.TafitiUpdateMessageID;
            }
        }
    }
}
