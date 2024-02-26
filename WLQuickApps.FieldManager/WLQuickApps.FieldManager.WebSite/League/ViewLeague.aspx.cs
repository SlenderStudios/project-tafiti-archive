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
    public partial class League_ViewLeague : System.Web.UI.Page
    {
        private League _league;

        public string IsLeagueAdmin { get { return LeagueManager.IsLeagueAdmin(this._league.LeagueID).ToString().ToLower(); } }
      
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

        public string LeagueFields
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (Field field in FieldsManager.GetFieldsForLeague(this._league.LeagueID))
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
            this._league = LeagueManager.GetLeague(Convert.ToInt32(this.Request.QueryString["leagueID"]));
            this._adminPanel.Visible = LeagueManager.IsLeagueAdmin(this._league.LeagueID);

            if (!this.IsPostBack)
            {
                this._descriptionTextBox.Text = this._league.Description;
                this._titleTextBox.Text = this._league.Title;
                this._typeTextBox.Text = this._league.Type;
            }
        }

        protected void _removeFieldClick(object sender, EventArgs e)
        {
            int fieldID = Convert.ToInt32(((LinkButton)sender).CommandArgument.ToString());
            FieldsManager.RemoveFieldFromLeague(fieldID, this._league.LeagueID);

            this.DataBind();
        }

        protected void _updateClick(object sender, EventArgs e)
        {
            LeagueManager.UpdateLeague(
                this._league.LeagueID,
                this._titleTextBox.Text,
                this._descriptionTextBox.Text,
                this._typeTextBox.Text);
        }

        protected void _deleteClick(object sender, EventArgs e)
        {
            LeagueManager.DeleteLeague(this._league.LeagueID);
            this.Response.Redirect("~/User/Default.aspx");
        }

        protected void _addAdminByEmailButtonClick(object sender, EventArgs e)
        {
            try
            {
                LeagueManager.AddLeagueAdmin(this._league.LeagueID, (Guid)Membership.GetUser(Membership.GetUserNameByEmail(this._addAdminByEmailTextBox.Text)).ProviderUserKey);
                this._addAdminByEmailTextBox.Text = string.Empty;
            }
            catch { }
        }

        
    }
}