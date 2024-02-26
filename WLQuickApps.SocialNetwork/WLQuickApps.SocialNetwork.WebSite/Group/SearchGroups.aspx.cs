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
using WLQuickApps.SocialNetwork.WebSite;

public partial class SearchGroups : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.Request.QueryString[WebConstants.QueryVariables.SearchName]) && !this.Page.IsPostBack)
        {
            this._nameTextBox.Text = this.Request.QueryString[WebConstants.QueryVariables.SearchName];
        }
    }

    protected void _searchButton_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("~/Group/SearchGroups.aspx?searchName={0}", this._nameTextBox.Text));
    }


    protected void _specialGroups_Load(object sender, EventArgs e)
    {
        DataList specialGroups = (DataList)sender;

        specialGroups.DataSource = SettingsWrapper.SpecialGroups;
        specialGroups.DataBind();
    }
}
