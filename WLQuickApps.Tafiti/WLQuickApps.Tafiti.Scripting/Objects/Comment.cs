using System;
using ScriptFX;
using System.XML;

namespace WLQuickApps.Tafiti.Scripting
{
    public class Comment
    {
        public string CommentID;
        public string ShelfStackID;
        public DateTime Timestamp;
        public string Text;
        public TafitiUser Owner
        {
            get
            {
                return TafitiUserManager.GetUser(this._ownerID);
            }
        }
        private string _ownerID;

        public Comment()
        {
        }


        static public Comment CreateFromXmlNode(XMLNode node)
        {
            Comment comment = new Comment();

            for (int lcv = 0; lcv < node.ChildNodes.Length; lcv++)
            {
                XMLNode childNode = node.ChildNodes[lcv];
                if (string.IsNullOrEmpty(childNode.BaseName)) { continue; }

                switch (childNode.BaseName.ToLowerCase())
                {
                    case "commentid": comment.CommentID = childNode.Text; break;
                    case "shelfstackid": comment.ShelfStackID = childNode.Text; break;
                    case "timestamp": comment.Timestamp = new DateTime(childNode.Text); break;
                    case "text": comment.Text = childNode.Text; break;
                    case "userid":
                        comment._ownerID = childNode.Text;
                        TafitiUserManager.BeginUserRequestByUserID(comment._ownerID);
                        break;
                    default: break;
                }
            }
            
            return comment;
        }

    }
}
