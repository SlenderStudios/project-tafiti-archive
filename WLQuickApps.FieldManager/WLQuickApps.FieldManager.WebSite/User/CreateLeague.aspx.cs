using System;
using System.Configuration;
using System.Data;
using System.Linq;
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
    public partial class User_CreateLeague : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void _createButton_Click(object sender, EventArgs e)
        {
            League league = LeagueManager.CreateLeague(
                this._titleTextBox.Text,
                this._descriptionTextBox.Text,
                this._typeTextBox.Text);

            this.Response.Redirect(string.Format("~/League/ViewLeague.aspx?leagueID={0}", league.LeagueID));
        }

    }
}