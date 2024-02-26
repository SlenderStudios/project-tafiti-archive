using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class PresenceControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        MembershipUserCollection users = Membership.GetAllUsers();

        var data = from user in users.Cast<MembershipUser>()
                   let profile = ProfileBase.Create(user.UserName, true)
                   where !string.IsNullOrEmpty((string)profile.GetPropertyValue("Presence.ID"))
                   select new
                   {
                       ID = profile.GetPropertyValue("Presence.ID"),
                       DisplayName = profile.GetPropertyValue("Presence.DisplayName")
                   };

        ListView.DataSource = data;
        ListView.DataBind();
    }
}
