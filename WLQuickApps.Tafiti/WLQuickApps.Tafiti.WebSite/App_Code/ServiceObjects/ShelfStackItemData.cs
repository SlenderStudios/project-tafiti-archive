using System;
using System.Data;
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
    public class ShelfStackItemData
    {
        public string Domain;
        public string Title;
        public string Description;
        public string Url;
        public string ShelfStackItemID;
        public string ShelfStackID;
        public string UserID;
        public string Timestamp;
        public string ImageUrl;
        public string Source;
        public int Width;
        public int Height;

        public ShelfStackItemData() { }
        public ShelfStackItemData(ShelfStackItem shelfStackItem)
        {
            this.ShelfStackItemID = shelfStackItem.ShelfStackItemID.ToString();
            this.ShelfStackID = shelfStackItem.ParentShelf.ShelfStackID.ToString();
            this.Domain = shelfStackItem.Domain;
            this.Title = shelfStackItem.Title;
            this.Timestamp = shelfStackItem.Timestamp.ToUniversalTime().ToString("R");
            this.Description = shelfStackItem.Description;
            this.Url = shelfStackItem.Url;
            this.UserID = shelfStackItem.Owner.UserID;
            this.Source = shelfStackItem.Source;
            this.ImageUrl = shelfStackItem.ImageUrl;
            this.Height = shelfStackItem.Height;
            this.Width = shelfStackItem.Width;
        }
    }
}