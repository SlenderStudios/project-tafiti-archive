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

public partial class Friend_SearchProfiles : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void _searchButton_Click(object sender, EventArgs e)
    {
        this._searchResultsUserGroup.DataSource = null;

        if (this._emailRadioButton.Checked)
        {
            User user;
            if (UserManager.TryGetUserByEmail(this._searchTextBox.Text, out user))
            {
                Response.Redirect(WebUtilities.GetViewItemUrl(user));
            }
        }
        else if (this._userNameRadioButton.Checked)
        {
            User user;
            if (UserManager.TryGetUserByUserName(this._searchTextBox.Text, out user))
            {
                Response.Redirect(WebUtilities.GetViewItemUrl(user));
            }
        }
        else if (this._fullNameRadioButton.Checked)
        {
            string fullName = this._searchTextBox.Text.Trim();
            int spacePos = fullName.IndexOf(" ");
            if (spacePos > 0)
            {
                string firstName = fullName.Substring(0, spacePos);
                string lastName = fullName.Substring(spacePos + 1);

                this._searchResultsDataSource.SelectParameters.Clear();
                this._searchResultsDataSource.SelectParameters.Add(
                        new Parameter("firstName", TypeCode.String, firstName));
                this._searchResultsDataSource.SelectParameters.Add(
                        new Parameter("lastName", TypeCode.String, lastName));

                this._searchResultsUserGroup.DataBind();
            }
        }

        this._searchResultsPanel.Visible = true;
    }

}
