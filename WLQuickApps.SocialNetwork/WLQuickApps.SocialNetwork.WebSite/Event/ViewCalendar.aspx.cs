using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WLQuickApps.SocialNetwork.Business;
using System.Globalization;
using System.Collections.ObjectModel;

public partial class Event_ViewCalendar : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void _calendar_PreRender(object sender, EventArgs e)
    {
        if (EventManager.GetEventsForUserCount(UserManager.LoggedInUser.UserName, DateTime.Today, DateTime.MaxValue, UserGroupStatus.Joined) == 0)
        {
            this._noEvents.Visible = true;
        }
    }
}
