using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Microsoft.Live.ServerControls;

using WLQuickApps.ContosoBank.Entity;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class AdvisorListControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                AdvisorGridView.DataSource = AdvisorLogic.GetActiveAdvisors();
                AdvisorGridView.DataBind();
            }
        }

        protected void AdvisorGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var currentAdvisor = (Advisor)e.Row.DataItem;
                var chat = (MessengerChat)e.Row.FindControl("MessengerChat2");
                if (chat != null)
                {
                    chat.CID = currentAdvisor.CID;
                    chat.Visible = true;
                }

                if (currentAdvisor.AvailableAppointment)
                {
                    var separator = (Label)e.Row.FindControl("appointmentLabel");
                    if (separator != null)
                    {
                        separator.Visible = true;
                        var advisorLink = (HyperLink)e.Row.FindControl("appointmentHyperlink");
                        advisorLink.Visible = true;
                        advisorLink.NavigateUrl = "../Appointments.aspx?ID=" + currentAdvisor.ID;
                    }
                }
            }
        }
    }
}