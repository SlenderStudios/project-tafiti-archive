using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Security;

using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.ForumDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{

    public static class ForumManager
    {
        static private List<Forum> GetForumsFromTable(ForumDataSet.ForumDataTable forumDataTable)
        {
            List<Forum> list = new List<Forum>();
            foreach (ForumDataSet.ForumRow row in forumDataTable)
            {
                Forum forum = new Forum(row);
                if (!forum.CanView) { continue; }
                list.Add(forum);
            }
            return list;
        }

        #region Create

        static public Forum CreateForum(string title, string topic)
        {
            if (string.IsNullOrEmpty(title)) { throw new ArgumentException("Title cannot be null or empty"); }
            if (topic == null) { topic = string.Empty; }

            using (ForumTableAdapter tableAdapter = new ForumTableAdapter())
            {
                int baseItemID = Convert.ToInt32(BaseItemManager.CreateBaseItem(Constants.BaseItemTypes.Forum, Location.Empty, title, topic, UserManager.LoggedInUser, string.Empty, PrivacyLevel.Public, true, string.Empty));
                tableAdapter.CreateForum(baseItemID);

                Forum forum = ForumManager.GetForum(baseItemID);
                forum.Update();
                return forum;
            }
        }

        #endregion

        static public Forum GetForum(int baseItemID)
        {
            using (ForumTableAdapter tableAdapter = new ForumTableAdapter())
            {
                ForumDataSet.ForumDataTable dataTable = tableAdapter.GetForum(baseItemID);

                if (dataTable.Rows.Count == 0) { throw new ArgumentException("Forum does not exist"); }

                Forum forum = new Forum(dataTable[0].BaseItemID);
                if (!forum.CanView) { throw new SecurityException("The current user does not have permission to access this item"); }
                return forum;
            }
        }

        static public ReadOnlyCollection<Forum> GetForums(int startRowIndex, int maximumRows)
        {
            using (ForumTableAdapter tableAdapter = new ForumTableAdapter())
            {
                return ForumManager.GetForumsFromTable(tableAdapter.GetForums(startRowIndex, maximumRows)).AsReadOnly();
            }
        }

        static public int GetForumsCount()
        {
            using (ForumTableAdapter tableAdapter = new ForumTableAdapter())
            {
                return tableAdapter.GetForumsCount().GetValueOrDefault(0);
            }
        }

        static public ReadOnlyCollection<Forum> GetForumsByTopic(string topic, int startRowIndex, int maximumRows)
        {
            if (topic == null) { topic = string.Empty; }
            using (ForumTableAdapter tableAdapter = new ForumTableAdapter())
            {
                return ForumManager.GetForumsFromTable(tableAdapter.GetForumsByTopic(topic, startRowIndex, maximumRows)).AsReadOnly();
            }
        }

        static public int GetForumsByTopicCount(string topic)
        {
            if (topic == null) { topic = string.Empty; }
            using (ForumTableAdapter tableAdapter = new ForumTableAdapter())
            {
                return (int) tableAdapter.GetForumsByTopicCount(topic);
            }
        }

        static public bool ForumExists(int baseItemID)
        {
            using (ForumTableAdapter tableAdapter = new ForumTableAdapter())
            {
                return (tableAdapter.GetForum(baseItemID) != null);
            }
        }

        static internal void DeleteForum(Forum forum)
        {
            ForumManager.VerifyOwnerActionOnForum(forum);

            BaseItemManager.DeleteBaseItem(forum);
        }

        static public void UpdateForum(Forum forum)
        {
            if (forum == null) { throw new ArgumentNullException("forum"); }

            ForumManager.VerifyOwnerActionOnForum(forum);

            BaseItemManager.UpdateBaseItem(forum);
        }

        static internal void VerifyOwnerActionOnForum(Forum forum)
        {
            if (!ForumManager.CanModifyForum(forum.BaseItemID))
            {
                throw new SecurityException("Forum cannot be modified because it does not belong to the logged in user");
            }
        }

        static public bool CanModifyForum(int baseItemID)
        {
            Forum forum = ForumManager.GetForum(baseItemID);
            return ForumManager.CanModifyForum(forum);
        }

        static internal bool CanModifyForum(Forum forum)
        {
            return ((UserManager.LoggedInUser != null) &&
                ((forum.Owner == UserManager.LoggedInUser) || UserManager.LoggedInUser.IsAdmin));
        }
    }
}
