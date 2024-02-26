using System;
using ScriptFX;
using System.XML;

namespace WLQuickApps.Tafiti.Scripting
{
    public class ShelfStackItem
    {
        public string ShelfStackItemID;
        public string ShelfStackID;
        public string Domain;
        public string Title;
        public string Description;
        public string Source;
        public string ImageUrl;
        public string Height;
        public string Width;
        public string Url;
        public TafitiUser Owner;
        public DateTime LastModifiedTimestamp;

        public ShelfStackItem()
        {
        }

        static public ShelfStackItem CreateFromXmlNode(XMLNode shelfStackItemNode)
        {
            ShelfStackItem shelfStackItem = new ShelfStackItem();

            for (int lcv = 0; lcv < shelfStackItemNode.ChildNodes.Length; lcv++)
            {
                XMLNode childNode = shelfStackItemNode.ChildNodes[lcv];
                if (string.IsNullOrEmpty(childNode.BaseName)) { continue; }

                switch (childNode.BaseName.ToLowerCase())
                {
                    case "shelfstackitemid": shelfStackItem.ShelfStackItemID = childNode.Text; break;
                    case "shelfstackid": shelfStackItem.ShelfStackID = childNode.Text; break;
                    case "domain": shelfStackItem.Domain = childNode.Text; break;
                    case "title": shelfStackItem.Title = childNode.Text; break;
                    case "description": shelfStackItem.Description = childNode.Text; break;
                    case "url": shelfStackItem.Url = childNode.Text; break;
                    case "userid": shelfStackItem.Owner = TafitiUserManager.GetUser(childNode.Text); break;
                    case "timestamp": shelfStackItem.LastModifiedTimestamp = new DateTime(childNode.Text); break;
                    case "imageurl": shelfStackItem.ImageUrl = childNode.Text; break;
                    case "source": shelfStackItem.Source = childNode.Text; break;
                    case "height": shelfStackItem.Height = childNode.Text; break;
                    case "width": shelfStackItem.Width = childNode.Text; break;
                    default: break;
                }
            }
            return shelfStackItem;
        }
    }
}
