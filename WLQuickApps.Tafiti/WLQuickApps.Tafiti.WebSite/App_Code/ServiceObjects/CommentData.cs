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
    public class CommentData
    {
        public string CommentID;
        public string ShelfStackID;
        public string UserID;
        public string Timestamp;
        public string Text;

        public CommentData() { }
        public CommentData(Comment comment)
        {
            this.CommentID = comment.CommentID.ToString();
            this.ShelfStackID = comment.ParentShelf.ShelfStackID.ToString();
            this.Text = comment.Text;
            this.Timestamp = comment.Timestamp.ToUniversalTime().ToString("R"); // Seems to be friendly to javascript.
            this.UserID = comment.Owner.UserID;
        }
    }
}