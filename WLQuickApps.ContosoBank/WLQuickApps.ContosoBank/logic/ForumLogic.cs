using System.Collections.Generic;
using System.Linq;
using System.Web;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class ForumLogic
    {
        public static List<ForumSubject> GetAllForumPosts()
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = from fs in ctx.ForumSubjects
                       orderby fs.PostDate descending
                       select fs;

            return temp.ToList();
        }

        public static List<ForumSubject> GetLatestForumPosts(int numRows)
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = (from fs in ctx.ForumSubjects
                        orderby fs.PostDate descending
                        select fs);

            return temp.Take(numRows).ToList();
        }

        public static List<ForumSubject> GetStickyPosts()
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = from fs in ctx.ForumSubjects
                       where fs.IsStickyPost
                       orderby fs.PostDate descending
                       select fs;

            return temp.ToList();
        }

        public static List<ForumReply> GetReply(int id)
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = from fr in ctx.ForumReplies
                       where fr.ForumSubject == id
                       orderby fr.ReplyDate ascending
                       select fr;

            return temp.ToList();
        }

        public static ForumReply GetSubjectText(int id)
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = from fr in ctx.ForumReplies
                       where fr.ForumSubject == id
                       orderby fr.ReplyDate ascending
                       select fr;

            return temp.FirstOrDefault();
        }

        public static ForumSubject GetRecord(int id)
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = from fs in ctx.ForumSubjects
                       where fs.ForumSubjectID == id
                       select fs;

            return temp.FirstOrDefault();
        }

        #region RSS Functions
        public static List<BaseForum> GetForumPostsByDate(int numRows)
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = (from fs in ctx.ForumSubjects
                        orderby fs.PostDate descending
                        select fs);

            return getBaseForumItems(temp.Take(numRows).ToList());
        }

        public static List<BaseForum> GetForumPostsByViews(int numRows)
        {
            ContosoBankDataContext ctx = new ContosoBankDataContext();
            var temp = (from fs in ctx.ForumSubjects
                        orderby fs.NumViews descending
                        select fs);

            return getBaseForumItems(temp.Take(numRows).ToList());
        } 

        private static List<BaseForum> getBaseForumItems(List<ForumSubject> items)
        {
            List<BaseForum> result = new List<BaseForum>();
            foreach (ForumSubject item in items)
            {
                string baseUrl = HttpContext.Current.Request.Url.Scheme + "://";
                baseUrl += HttpContext.Current.Request.Url.Host;
                baseUrl += HttpContext.Current.Request.Url.Port.ToString().Length > 0
                               ? ":" + HttpContext.Current.Request.Url.Port
                               : string.Empty;
                baseUrl += HttpContext.Current.Request.ApplicationPath;

                BaseForum forumItem = new BaseForum
                                          {
                                              Title = item.Subject,
                                              ForumDetail = GetSubjectText(item.ForumSubjectID).ReplyText,
                                              PostLink = baseUrl + "ForumThread.aspx?ID=" + item.ForumSubjectID,
                                              Avatar = baseUrl + item.UserProfile.Avatar,
                                              PublishDate = item.PostDate.ToLongDateString()
                                          };
                result.Add(forumItem);
            }
            return result;
        }
        #endregion


    }
}