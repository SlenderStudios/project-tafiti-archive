using System;
using System.DHTML;
using ScriptFX;
using System.XML;

namespace SJ
{
    [Imported]
    public class Interop
    {
        public void MessengerStatusChanged(bool isSignedIn) { }
        public void UpdateShelfStack(WLQuickApps.Tafiti.Scripting.ShelfStack shelfStack) { }
        public void OnIncomingTextMessage(string emailhash, string displayName, string messageText) { }
        public void PopIMConsentDialog(WLQuickApps.Tafiti.Scripting.ShelfStack shelfStack) { }
        public void PopAcceptContactDialog(string displayName, string emailAddress, string inviteMessage) { }

        public Interop()
        {
        }
    }
}

namespace WLQuickApps.Tafiti.Scripting
{
    public class InteropManager
    {
        static public SJ.Interop Interop
        {
            get
            {
                if (InteropManager._updater == null)
                {
                    InteropManager._updater = new SJ.Interop();
                }
                return InteropManager._updater;
            }
        }
        static private SJ.Interop _updater;

        static public void UpdateShelfStack(ShelfStack shelfStack)
        {
            InteropManager.Interop.UpdateShelfStack(shelfStack);
        }

        static public void PopIMConsentDialog(ShelfStack pendingShelfStack)
        {
            InteropManager.Interop.PopIMConsentDialog(pendingShelfStack);
        }

        static public void PopAcceptContactDialog(string displayName, string emailAddress, string inviteMessage)
        {
            InteropManager.Interop.PopAcceptContactDialog(displayName, emailAddress, inviteMessage);
        }

        static public void MessengerStatusChanged(bool isSignedIn)
        {
            InteropManager.Interop.MessengerStatusChanged(isSignedIn);
        }

        static public void OnIncomingTextMessage(string emailhash, string displayName, string messageText)
        {
            InteropManager.Interop.OnIncomingTextMessage(emailhash, displayName, messageText);
        }        
    }
}
