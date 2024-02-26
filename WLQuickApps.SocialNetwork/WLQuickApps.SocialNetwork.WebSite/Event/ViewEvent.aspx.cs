using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Security;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Event_ViewEvent : System.Web.UI.Page
{
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        this._signInHyperlink.NavigateUrl = WindowsLiveLogin.GetLoginUrl();
        this._registerHyperlink.NavigateUrl = WindowsLiveLogin.GetLoginUrl();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this._viewEventForm.GroupItem = EventManager.GetEvent(
                Int32.Parse(this.Request.QueryString[WebConstants.QueryVariables.BaseItemID]));
            this._viewEventForm.Visible = true;
        }
        catch (SecurityException)
        {
            this._noPermissionPanel.Visible = true;

            if (!UserManager.IsUserLoggedIn())
            {
                this._anonymousPanel.Visible = true;
            }
        }
        catch (Exception)
        {
            this._doesNotExistPanel.Visible = true;
        }
    }
}
