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
using System.Web.UI.MobileControls;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Reservations : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string resortName = Request.Params[WebConstants.QueryVariables.Resort];
        if (!String.IsNullOrEmpty(resortName))
        {
            this._reservationForm.DefaultValue = resortName;
        }
    }
}
