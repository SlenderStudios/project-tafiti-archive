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
using WLQuickApps.SocialNetwork.WebSite;
using WLQuickApps.SocialNetwork.Business;

public partial class Admin_Statistics : System.Web.UI.Page
{
    private DateTime _startDateTime = DateTime.Now.AddHours(-24);

    protected void Page_Load(object sender, EventArgs e)
    {
        this._loggedInUsers.Text = Membership.GetNumberOfUsersOnline().ToString();

        this._specialActivityTable.Rows.Clear();
        this._specialTotalsTable.Rows.Clear();

        foreach (string groupType in SettingsWrapper.SpecialGroups)
        {
            this._specialActivityTable.Rows.Add(this.CreateSpecialTableRow(groupType, false));
            this._specialTotalsTable.Rows.Add(this.CreateSpecialTableRow(groupType, true));
        }
        foreach (string eventType in SettingsWrapper.SpecialEvents)
        {
            this._specialActivityTable.Rows.Add(this.CreateSpecialTableRow(eventType, false));
            this._specialTotalsTable.Rows.Add(this.CreateSpecialTableRow(eventType, true));
        }

        if (this._specialTotalsTable.Rows.Count == 0)
        {
            this._specialTotalsTable.Visible = false;
            this._specialActivityTable.Visible = false;
        }
    }
    protected void _getActivityButton_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid)
        {
            DateTime lowRangeDate = DateTime.Now.AddHours(-Double.Parse(_hoursTextBox.Text));

            this._startDateTime = lowRangeDate; 
            this._statsActivityDetailsView.DataBind();
            
            foreach (TableRow row in this._specialActivityTable.Rows)
            {
                row.Cells[1].Text = StatisticsManager.GetBaseItemSubTypeCount(row.Cells[2].Text, lowRangeDate, DateTime.Now).ToString();
            }
        }
    }
    protected void _getTotalsButton_Click(object sender, EventArgs e)
    {
        this._statsTotalsDetailsView.DataBind();

        foreach (TableRow row in this._specialTotalsTable.Rows)
        {
            row.Cells[1].Text = StatisticsManager.GetBaseItemSubTypeCount(row.Cells[2].Text).ToString();
        }
    }

    protected void _hoursTextBox_ServerValidate(object source, ServerValidateEventArgs args)
    {
        double result;
        args.IsValid = Double.TryParse(args.Value, out result);
    }

    protected void _statsActivityDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["startDateTime"] = this._startDateTime;
        e.InputParameters["endDateTime"] = DateTime.Now;
    }

    protected TableRow CreateSpecialTableRow(string subType, bool isTotal)
    {
        TableRow row = new TableRow();

        TableCell cell0 = new TableCell();
        if (isTotal)
        {
            cell0.Text = string.Format("{0}s", subType);
        }
        else 
        {
            cell0.Text = string.Format("{0}s Created", subType);
        }
        row.Cells.Add(cell0);

        TableCell cell1 = new TableCell();
        cell1.HorizontalAlign = HorizontalAlign.Right;
        if (isTotal)
        {
            cell1.Text = StatisticsManager.GetBaseItemSubTypeCount(subType).ToString();   
        }
        else
        {
            cell1.Text = StatisticsManager.GetBaseItemSubTypeCount(subType, this._startDateTime, DateTime.Now).ToString();
        }
        row.Cells.Add(cell1);

        TableCell cell2 = new TableCell();
        cell2.Text = subType;
        cell2.Visible = false;
        row.Cells.Add(cell2);

        return row;
    }
}
