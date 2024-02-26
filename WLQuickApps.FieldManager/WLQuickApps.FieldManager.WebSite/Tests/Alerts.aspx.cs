using System;
using System.Collections;
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

public partial class Tests_Alerts : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.lblAlertURI.Text = WLQuickApps.FieldManager.Business.LiveAlertsWrapper.TestAlertConnectivity();
    }
}
