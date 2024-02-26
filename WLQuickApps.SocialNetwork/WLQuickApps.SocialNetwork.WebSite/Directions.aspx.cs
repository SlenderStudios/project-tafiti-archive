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

public partial class Directions : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this._startAddress.Text = this.Request.QueryString[WebConstants.QueryVariables.StartAddress];
        this._endAddress.Text = this.Request.QueryString[WebConstants.QueryVariables.EndAddress];
    }
}
