using System;
using System.Web.UI;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class QuickLinksControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                QuickLinksGridView.DataSource = QuickLinkLogic.GetQuickLinks();
                QuickLinksGridView.DataBind();
            }
        }
    }
}