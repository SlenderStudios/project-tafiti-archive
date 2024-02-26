using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using WLQuickApps.Tafiti.Business;

namespace WLQuickApps.Tafiti.WebSite
{
    public class ShelfStackData
    {
        public string ShelfStackID;
        public string Label;
        public string LastModifiedTimestamp;
        public string[] ShelfStackItemIDs;
        public string[] OwnerEmailHashes;

        public ShelfStackData() { }
        public ShelfStackData(ShelfStack shelfStack)
        {
            this.ShelfStackID = shelfStack.ShelfStackID.ToString();
            this.Label = shelfStack.Label;

            ReadOnlyCollection<ShelfStackItem> shelfStackItems = ShelfStackItemManager.GetShelfStackItemsForShelf(shelfStack);
            this.ShelfStackItemIDs = new string[shelfStackItems.Count];
            for (int lcv = 0; lcv < shelfStackItems.Count; lcv++)
            {
                this.ShelfStackItemIDs[lcv] = shelfStackItems[lcv].ShelfStackItemID.ToString();
            }

            ReadOnlyCollection<User> owners = UserManager.GetShelfOwners(shelfStack.ShelfStackID);
            ReadOnlyCollection<string> pendingInvites = ShelfStackManager.GetPendingInvitesForShelfStack(shelfStack);
            this.OwnerEmailHashes = new string[owners.Count + pendingInvites.Count];
            for (int lcv = 0; lcv < owners.Count; lcv++)
            {
                this.OwnerEmailHashes[lcv] = owners[lcv].EmailHash;
            }
            for (int lcv = 0; lcv < pendingInvites.Count; lcv++)
            {
                this.OwnerEmailHashes[lcv + owners.Count] = pendingInvites[lcv];
            }

            this.LastModifiedTimestamp = shelfStack.LastModifiedTimestamp.ToUniversalTime().ToString("R");
        }
    }
}
