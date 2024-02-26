using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.WebSite
{
    public struct FriendSummary
    {
        public string DisplayName
        {
            get { return this._displayName; }
            set { return; }
        }
        private string _displayName;

        public string UserName
        {
            get { return this._userName; }
            set { return; }
        }
        private string _userName;

        public FriendSummary(User user)
        {
            this._userName = user.UserName;
            this._displayName = String.Format("{0} {1} ({2})", user.FirstName, user.LastName, user.UserName);
        }
    }

    /// <summary>
    /// Returns a version of the User object that a checkboxlist can consume visually
    /// </summary>
    public class FriendHelper
    {
        public FriendHelper() { }

        static public List<FriendSummary> GetFriendSummary()
        {
            return GetFriendSummary(FriendManager.GetFriends());
        }

        static public List<FriendSummary> GetFriendSummary(List<User> users)
        {
            List<FriendSummary> result = new List<FriendSummary>();
            foreach (User user in users)
            {
                result.Add(new FriendSummary(user));
            }

            return result;
        }

        static public List<FriendSummary> GetFriendSummary(Group group)
        {
            List<FriendSummary> result = new List<FriendSummary>();
            foreach (User user in FriendManager.GetFriends())
            {
                if (!GroupManager.HasUserJoinedGroup(user, group) && !GroupManager.IsUserInvitedToGroup(user, group))
                {
                    result.Add(new FriendSummary(user));
                }
            }

            return result;
        }
    }
}
