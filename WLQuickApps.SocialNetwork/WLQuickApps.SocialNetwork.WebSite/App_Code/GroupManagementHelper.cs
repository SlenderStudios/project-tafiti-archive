using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.SocialNetwork.Business;
using System.Collections.Generic;

/// <summary>
/// Summary description for GroupManagementHelper
/// </summary>

namespace WLQuickApps.SocialNetwork.WebSite
{
    public class GroupManagementHelper
    {
        static public List<User> GetAllUsersForGroup(int BaseItemID, int startRowIndex, int maximumRows)
        {
            return UserManager.GetAllUsersForGroup(BaseItemID, UserGroupStatus.Joined, startRowIndex, maximumRows);
        }

        static public int GetAllUsersForGroupCount(int baseItemID)
        {
            return UserManager.GetAllUsersForGroupCount(baseItemID, UserGroupStatus.Joined);
        }

        static public void RemoveUserFromGroup(int BaseItemID, Group group)
        {
            GroupManager.RemoveUserFromGroup(BaseItemManager.GetBaseItem(BaseItemID) as User, group);
        }
    }
}