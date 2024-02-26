using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Security;

using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.CommentDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class CommentManager
    {
        static public bool CommentExists(int commentID)
        {
            using (CommentTableAdapter commentTableAdapter = new CommentTableAdapter())
            {
                return (commentTableAdapter.GetComment(commentID).Rows.Count != 0);
            }
        }

        static public Comment GetComment(int commentID)
        {
            using (CommentTableAdapter commentTableAdapter = new CommentTableAdapter())
            {
                CommentDataSet.CommentDataTable table = commentTableAdapter.GetComment(commentID);

                if (table.Count < 1)
                {
                    throw new ArgumentException("Comment ID does not exist");
                }

                return new Comment(table[0]);
            }
        }

        static public Comment CreateComment(int baseItemID, string text)
        {
            if (string.IsNullOrEmpty(text)) { throw new ArgumentException("Comment text cannot be null or empty"); }
            UserManager.AssertThatAUserIsLoggedIn();

            BaseItemManager.VerifyUserCanContributeToBaseItem(BaseItemManager.GetBaseItem(baseItemID));

            using (CommentTableAdapter commentTableAdapter = new CommentTableAdapter())
            {
                int commentID = Convert.ToInt32(commentTableAdapter.CreateComment(baseItemID, UserManager.LoggedInUser.UserID, text, DateTime.Now));
                if (commentID == -1) { throw new Exception("Unable to create comment"); }
                return CommentManager.GetComment(commentID);
            }
        }

        static public void DeleteComment(int commentID)
        {
            Comment comment = CommentManager.GetComment(commentID);
            CommentManager.VerifyOwnerActionOnComment(comment);

            using (CommentTableAdapter commentTableAdapter = new CommentTableAdapter())
            {
                commentTableAdapter.DeleteComment(commentID);
            }            
        }

        static public List<Comment> GetComments(int baseItemID)
        {
            return CommentManager.GetComments(baseItemID, 0, CommentManager.GetCommentCount(baseItemID));
        }

        static public int GetCommentCount(int baseItemID)
        {
            using (CommentTableAdapter commentTableAdapter = new CommentTableAdapter())
            {
                return Convert.ToInt32(commentTableAdapter.GetCommentsCount(baseItemID));
            }
        }

        static public List<Comment> GetComments(int baseItemID, int startRowIndex, int maximumRows)
        {
            return CommentManager.GetComments(baseItemID, startRowIndex, maximumRows, false);
        }

        static public List<Comment> GetComments(int baseItemID, int startRowIndex, int maximumRows, bool oldestFirst)
        {
            List<Comment> list = new List<Comment>();

            using (CommentTableAdapter commentTableAdapter = new CommentTableAdapter())
            {
                CommentDataSet.CommentDataTable commentTable;

                if (oldestFirst)
                {
                    commentTable = commentTableAdapter.GetCommentsOldestFirst(baseItemID, startRowIndex, maximumRows);
                }
                else
                {
                    commentTable = commentTableAdapter.GetCommentsNewestFirst(baseItemID, startRowIndex, maximumRows);
                }

                foreach (CommentDataSet.CommentRow row in commentTable)
                {
                    list.Add(new Comment(row));
                }
            }

            return list; 
        }

        static internal void VerifyOwnerActionOnComment(Comment comment)
        {
            if (!CommentManager.CanModifyComment(comment.CommentID))
            {
                throw new SecurityException("Comment cannot be modified because it does not belong to the logged in user");
            }
        }

        static public bool CanModifyComment(int commentID)
        {
            Comment commentItem = CommentManager.GetComment(commentID);
            User baseItemOwner = BaseItemManager.GetBaseItem(commentItem.BaseItemID).Owner;
            return ((UserManager.LoggedInUser != null) && 
                     ((commentItem.UserItem == UserManager.LoggedInUser) || UserManager.LoggedInUser.IsAdmin || 
                     (baseItemOwner != null && baseItemOwner == UserManager.LoggedInUser)));
        }

    }
}
