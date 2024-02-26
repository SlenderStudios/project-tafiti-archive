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
    public partial class User_Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this._fieldDataSource.SelectParameters["userID"].DefaultValue = UserManager.LoggedInUser.UserID.ToString();
            this._leaguesDataSource.SelectParameters["userID"].DefaultValue = UserManager.LoggedInUser.UserID.ToString();

            if (!this.IsPostBack)
            {
                this._addressTextBox.Text = UserManager.LoggedInUser.Address;
                this._displayEmailTextBox.Text = UserManager.LoggedInUser.DisplayName;
                this._messengerIdentifier.Text = string.IsNullOrEmpty(UserManager.LoggedInUser.MessengerPresenceID) ? "Disabled" : "Enabled";
            }

            this.UpdateLiveAlertsPanel();
        }

        protected void _deleteFieldClick(object sender, EventArgs e)
        {
            int fieldID = Convert.ToInt32(((LinkButton)sender).CommandArgument.ToString());
            FieldsManager.DeleteField(fieldID);

            this.DataBind();
        }

        protected void _deleteLeagueClick(object sender, EventArgs e)
        {
            int leagueID = Convert.ToInt32(((LinkButton)sender).CommandArgument.ToString());
            LeagueManager.DeleteLeague(leagueID);

            this.DataBind();
        }


        protected void _removeFieldClick(object sender, EventArgs e)
        {
            int fieldID = Convert.ToInt32(((LinkButton)sender).CommandArgument.ToString());
            FieldsManager.RemoveField(fieldID);

            this.DataBind();
        }

        protected void _removeLeagueClick(object sender, EventArgs e)
        {
            int leagueID = Convert.ToInt32(((LinkButton)sender).CommandArgument.ToString());
            LeagueManager.RemoveLeague(leagueID);

            this.DataBind();
        }

        protected void _updateClick(object sender, EventArgs e)
        {
            UserManager.UpdateUser(this._displayEmailTextBox.Text, this._addressTextBox.Text.Replace("\n", ", "), UserManager.LoggedInUser.MessengerPresenceID);
        }

        private void UpdateLiveAlertsPanel()
        {
            if (!SettingsWrapper.EnableLiveAlerts)
            {
                this._liveAlertsPanel.Visible = false;
                return;
            }

            this._liveAlertsChangeHyperlink.NavigateUrl = SettingsWrapper.LiveAlertsChangeUrl;
            this._liveAlertsSignUpHyperlink.NavigateUrl = LiveAlertsWrapper.GetAlertsSignupUrl(this.Request.Url.AbsoluteUri);
            this._liveAlertsSignUpHyperlink.Visible = (this._liveAlertsSignUpHyperlink.NavigateUrl.Length > 0);
            this._liveAlertsChangeHyperlink.Visible = (this._liveAlertsSignUpHyperlink.NavigateUrl.Length == 0);
            this._liveAlertsUnsubscribeHyperlink.Visible = (this._liveAlertsSignUpHyperlink.NavigateUrl.Length == 0);
        }

        protected void _liveAlertsUnsubscribe_Command(object sender, CommandEventArgs e)
        {
            LiveAlertsWrapper.UnsubscribeAll();

            this.UpdateLiveAlertsPanel();
        }

        
    }
}