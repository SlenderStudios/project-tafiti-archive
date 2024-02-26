using System;
using System.Web.UI;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class Gadgets : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GadgetsRepeater.DataSource = GadgetLogic.GetGadgets();
                GadgetsRepeater.DataBind();
            }
        }
    }
}