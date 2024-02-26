// Class1.cs
//

using System;
using System.DHTML;
using ScriptFX;
using ScriptFX.UI;
using System.XML;

namespace WLQuickApps.Tafiti.Scripting
{
    public class EntryPoint
    {
        public static void main(Dictionary arguments)
        {
            if (Utilities.UserIsLoggedIn())
            {
                ShelfStackManager.BeginGetMasterData();
            }
        }

        public static void Start()
        {
            // Only launch messenger stuff if the user is logged in.
            if (Utilities.UserIsLoggedIn())
            {
                MessengerManager.StartMessenger();
            }

        }
    }
}
