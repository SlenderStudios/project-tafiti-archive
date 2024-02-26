/*****************************************************************************
 * Appointments.aspx
 * Notes: Page used for authenticated users to make appointment with
 *        financial advisors
 * **************************************************************************/

using System;

using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank
{
    public partial class Appointments : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack && Request.QueryString["ID"] != null)
            {
                Session["AdvisorID"] = Convert.ToInt32(Request.QueryString["ID"]);
            }
        }

        public string GetAdvisorName()
        {
            if (Request.QueryString["ID"] != null)
            {
                return AdvisorLogic.GetAdvisorByID(Convert.ToInt32(Request.QueryString["ID"])).AdvisorName;
            }
            return string.Empty;
        }
    }
}