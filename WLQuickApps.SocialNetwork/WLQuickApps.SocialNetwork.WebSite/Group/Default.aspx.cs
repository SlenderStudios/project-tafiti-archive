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

using WLQuickApps.SocialNetwork.WebSite;
using WLQuickApps.SocialNetwork.Business;

public partial class Group_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnPreRender(EventArgs e)
    {
        this._specialGroups.DataSource = SettingsWrapper.SpecialGroups;
        this._specialGroups.DataBind();
        base.OnPreRender(e);
    }
}
