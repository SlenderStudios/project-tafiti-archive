using System;
using System.Data;
using System.Collections.ObjectModel;
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
    public class UserData
    {
        public string UserID = string.Empty;
        public string DisplayName = string.Empty;
        public string EmailHash = string.Empty;
        public string MessengerPresenceID = string.Empty;

        public UserData() { }
        public UserData(User user) 
        {
            this.UserID = user.UserID.ToString();
            this.DisplayName = user.DisplayName;
            this.EmailHash = user.EmailHash;
            this.MessengerPresenceID = user.MessengerPresenceID;
        }

    }
}
