using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;

using WLQuickApps.Tafiti.Data;
using WLQuickApps.Tafiti.Data.ShelfStackItemDataSetTableAdapters;

namespace WLQuickApps.Tafiti.Business
{
    public class ShelfStackItemManager
    {
        static public ShelfStackItem CreateShelfStackItem(ShelfStack shelfStack, User user, string title, string description, string url, string imageUrl, int width, int height,
            string source, string domain)
        {
            using (ShelfStackItemTableAdapter adapter = new ShelfStackItemTableAdapter())
            {
                int id = Convert.ToInt32(adapter.CreateShelfStackItem(shelfStack.ShelfStackID, user.UserID, title, description, url, imageUrl, width, height, source, domain));
                ShelfStackManager.UpdateShelfStack(shelfStack); 
                return ShelfStackItemManager.GetShelfStackItemByID(id);
            }
        }

        static public ReadOnlyCollection<ShelfStackItem> GetShelfStackItemsForShelf(ShelfStack shelfStack)
        {
            using (ShelfStackItemTableAdapter adapter = new ShelfStackItemTableAdapter())
            {
                List<ShelfStackItem> list = new List<ShelfStackItem>();
                foreach (ShelfStackItemDataSet.ShelfStackItemRow row in adapter.GetShelfStackItemsByShelfStackID(shelfStack.ShelfStackID))
                {
                    list.Add(new ShelfStackItem(row));
                }
                return list.AsReadOnly();
            }
        }

        static public ShelfStackItem GetShelfStackItemByID(int shelfStackItemID)
        {
            using (ShelfStackItemTableAdapter adapter = new ShelfStackItemTableAdapter())
            {
                ShelfStackItemDataSet.ShelfStackItemDataTable table = adapter.GetShelfStackItemByID(shelfStackItemID);
                if (table.Rows.Count == 0) { return null; }
                return new ShelfStackItem(table[0]);
            }
        }

        static public void DeleteShelfStackItem(ShelfStackItem shelfStackItem)
        {
            using (ShelfStackItemTableAdapter adapter = new ShelfStackItemTableAdapter())
            {
                ShelfStackManager.UpdateShelfStack(shelfStackItem.ParentShelf);
                adapter.DeleteShelfStackItem(shelfStackItem.ShelfStackItemID);
            }
        }


    }
}
