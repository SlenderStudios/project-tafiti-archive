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

public partial class Friend_ViewProfile : System.Web.UI.Page
{
    protected void Page_Init(object sender, EventArgs e)
    {
        string userName = this.Request.QueryString[WebConstants.QueryVariables.UserName];
        User user;
        if (!string.IsNullOrEmpty(userName) && !UserManager.TryGetUserByUserName(userName, out user))
        {
            this.FormView1.Visible = false;
            this._doesNotExistPanel.Visible = true;
        }
        else
        {
            if (this.IsPostBack)
            {
                this.FormView1.DataBind();
            }
        }
    }

    protected void DataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        string userName = this.Request.QueryString[WebConstants.QueryVariables.UserName];

        if (!string.IsNullOrEmpty(userName))
        {
            this.Title = userName;
        }
        else if (UserManager.IsUserLoggedIn())
        {
            userName = this.User.Identity.Name;
        }
        else
        {
            FormsAuthentication.RedirectToLoginPage();
            e.Cancel = true;
        }

        e.InputParameters[WebConstants.DataBindingParameters.UserName] = userName;
    }

    protected void DataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        if ((e.Exception != null) && (e.Exception.InnerException != null) && (e.Exception is ArgumentException))
        {
            this.Response.Redirect("~/Friend/ViewProfile.aspx");
            e.ExceptionHandled = true;
        }
    }

    protected void _approveUser_Command(object sender, CommandEventArgs e)
    {
        User user = UserManager.GetUserByBaseItemID(Int32.Parse(e.CommandArgument.ToString()));
        user.Approve();
        Response.Redirect(WebUtilities.GetViewItemUrl(user));
    }

    protected void _specialGroups_Load(object sender, EventArgs e)
    {
        DataList specialGroups = (DataList)sender;

        specialGroups.DataSource = SettingsWrapper.SpecialGroups;
        specialGroups.DataBind();
    }
}
