using System;
using ScriptFX;

using WLQuickApps.Tafiti.Scripting;

namespace SJ
{
    [Imported]
    public class ContactVisual
    {
        public bool IsOnline;
        public string EmailHash;
        public string DisplayName;

        public void UpdateUI() { }
        public void Remove() { }

        public ContactVisual(TafitiUser user) 
        {
        }
    }
}
