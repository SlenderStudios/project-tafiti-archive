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
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;

using WLQuickApps.FieldManager.Business;
using WLQuickApps.FieldManager.Data;

namespace WLQuickApps.FieldManager.WebSite
{
    /// <summary>
    /// Summary description for UserItem
    /// </summary>
    [DataContract]
    public class UserItem
    {
        [DataMember] public string DisplayName;
        [DataMember] public string MessengerPresenceID;

        public UserItem() { }

        static public UserItem CreateFromUser(User item)
        {
            UserItem userItem = new UserItem();

            userItem.DisplayName = item.DisplayName;
            userItem.MessengerPresenceID = item.MessengerPresenceID;

            return userItem;
        }

        static public UserItem[] CreateFromUserList(IEnumerable<User> list)
        {
            List<UserItem> userItems = new List<UserItem>();
            foreach (User user in list)
            {
                userItems.Add(UserItem.CreateFromUser(user));
            }
            return userItems.ToArray();
        }

    }
}