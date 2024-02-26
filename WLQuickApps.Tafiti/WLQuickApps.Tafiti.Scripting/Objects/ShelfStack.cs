using System;
using ScriptFX;
using System.XML;

namespace WLQuickApps.Tafiti.Scripting
{
    public class ShelfStack
    {
        public string ShelfStackID;
        public string Label;
        public ShelfStackItem[] ShelfStackItems;
        public TafitiUser[] Owners
        {
            get
            {
                if (this._owners == null)
                {
                    this._owners = new TafitiUser[this._ownerEmailHashes.Length];
                    for (int lcv = 0; lcv < this._ownerEmailHashes.Length; lcv++)
                    {
                        this._owners[lcv] = TafitiUserManager.GetUserByEmailHash(this._ownerEmailHashes[lcv]);                        
                    }
                }
                return this._owners;
            }
        }
        private TafitiUser[] _owners;
        private string[] _ownerEmailHashes;

        public DateTime LastModifiedTimestamp;

        public ShelfStack()
        {
        }
        
        static public ShelfStack CreateFromXmlNode(XMLNode shelfStackNode)
        {
            ShelfStack shelfStack = new ShelfStack();

            for (int lcv = 0; lcv < shelfStackNode.ChildNodes.Length; lcv++)
            {
                XMLNode childNode = shelfStackNode.ChildNodes[lcv];
                if (string.IsNullOrEmpty(childNode.BaseName)) { continue; }

                ArrayList list;

                switch (childNode.BaseName.ToLowerCase())
                {
                    case "label": shelfStack.Label = childNode.Text; break;
                    case "shelfstackid": shelfStack.ShelfStackID = childNode.Text; break;
                    case "lastmodifiedtimestamp": shelfStack.LastModifiedTimestamp = new DateTime(childNode.Text); break;
                    case "shelfstackitemids":
                        list = new ArrayList();
                        for (int id = 0; id < childNode.ChildNodes.Length; id++)
                        {
                            if (string.IsNullOrEmpty(childNode.ChildNodes[id].BaseName)) { continue; }
                            list.Add(ShelfStackManager.GetShelfStackItem(childNode.ChildNodes[id].Text));
                        }
                        shelfStack.ShelfStackItems = (ShelfStackItem[]) list.Extract(0, list.Length);
                        break;
                    case "owneremailhashes":
                        list = new ArrayList();
                        for (int id = 0; id < childNode.ChildNodes.Length; id++)
                        {
                            if (string.IsNullOrEmpty(childNode.ChildNodes[id].BaseName)) { continue; }
                            list.Add(childNode.ChildNodes[id].Text);
                            TafitiUserManager.BeginUserRequestByEmailHash(childNode.ChildNodes[id].Text);
                        }
                        shelfStack._ownerEmailHashes = (string[]) list.Extract(0, list.Length);
                        break;

                    default:
                        break;
                }
            }

            return shelfStack;
        }

        public void AddOwner(TafitiUser user)
        {
            this._owners = (TafitiUser[]) this.Owners.Concat(new TafitiUser[] { user });
        }

    }
}
