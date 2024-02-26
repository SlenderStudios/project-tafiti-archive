using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;

using WLQuickApps.Tafiti.Data;
using WLQuickApps.Tafiti.Data.ConversationDataSetTableAdapters;

namespace WLQuickApps.Tafiti.Business
{
    public class ConversationManager
    {
        static public Comment AddComment(ShelfStack shelfStack, User user, string text)
        {
            return ConversationManager.AddComment(shelfStack.ShelfStackID, user.UserID, text);
        }

        static public Comment AddComment(Guid shelfStackID, string userID, string text)
        {
            using (ConversationsTableAdapter adapter = new ConversationsTableAdapter())
            {
                int id = Convert.ToInt32(adapter.CreateComment(shelfStackID, userID, DateTime.Now, text));
                return ConversationManager.GetCommentByID(id);
            }
        }

        static public Comment GetCommentByID(int commentID)
        {
            using (ConversationsTableAdapter adapter = new ConversationsTableAdapter())
            {
                ConversationDataSet.ConversationsDataTable table = adapter.GetCommentByID(commentID);
                if (table.Rows.Count == 0) { return null; }
                return new Comment(table[0]);
            }
        }

        static public ReadOnlyCollection<Comment> GetCommentsByShelf(Guid shelfStackID)
        {
            using (ConversationsTableAdapter adapter = new ConversationsTableAdapter())
            {
                List<Comment> list = new List<Comment>();
                foreach (ConversationDataSet.ConversationsRow row in adapter.GetConversationByShelfStackID(shelfStackID))
                {
                    list.Add(new Comment(row));
                }
                return list.AsReadOnly();
            }
        }

        static public void DeleteComment(Comment comment)
        {
            using (ConversationsTableAdapter adapter = new ConversationsTableAdapter())
            {
                adapter.DeleteComment(comment.CommentID);
            }
        }


    }
}
