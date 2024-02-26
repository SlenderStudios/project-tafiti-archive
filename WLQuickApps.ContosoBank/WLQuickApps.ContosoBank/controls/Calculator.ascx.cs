using System;
using System.Web.UI;

using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class Calculator : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CalculatorsRepeater.DataSource = CalculatorLogic.GetCalculators();
                CalculatorsRepeater.DataBind();
            }
        }
    }
}