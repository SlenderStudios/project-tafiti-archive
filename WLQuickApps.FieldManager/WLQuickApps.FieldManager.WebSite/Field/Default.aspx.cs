using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using WLQuickApps.FieldManager.Business;
using WLQuickApps.FieldManager.Data;

namespace WLQuickApps.FieldManager.WebSite
{
    public partial class Field_Default : System.Web.UI.Page
    {
        public string MyFields 
        {
            get
            {
                if (!UserManager.UserIsLoggedIn()) { return string.Empty; }

                StringBuilder stringBuilder = new StringBuilder();
                foreach (Field field in FieldsManager.GetFieldsForUser(UserManager.LoggedInUser.UserID, 0, FieldsManager.GetFieldsForUserCount(UserManager.LoggedInUser.UserID)))
                {
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Append(", ");
                    }

                    stringBuilder.AppendFormat("{0}: true", field.FieldID);
                }

                return stringBuilder.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

    }
}