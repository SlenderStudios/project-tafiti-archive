using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Feedback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (Request.UrlReferrer != null)
            {
                this._referrerUrl.Text = HttpUtility.HtmlEncode(Request.UrlReferrer.ToString());
            }
            else
            {
                this._referrerUrl.Text = "(None)";
            }
        }
    }

    protected void _submitFeedback_Click(object sender, EventArgs e)
    {
        string userName = UserManager.IsUserLoggedIn() ? UserManager.LoggedInUser.UserName : "(anonymous)";
        string fromEmail = UserManager.IsUserLoggedIn() ? UserManager.LoggedInUser.Email : SettingsWrapper.FeedbackEmail;

        CommunicationManager.SendEmail(SettingsWrapper.FeedbackEmail, fromEmail, userName,
            string.Format("Feedback report: {0}", this._feedbackType.Text),
            string.Format("<html><body><p>Feedback from {0}</p><p>Regarding <a href=\"{1}\">{1}</a></p><p>Message: {2}</p></body></html>",
                userName, this._referrerUrl.Text, this._feedbackDescription.Text));

        this._feedbackPanel.Visible = false;
        this._sentPanel.Visible = true;
    }
}
