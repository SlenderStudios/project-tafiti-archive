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

public partial class Event_SearchEvents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void _endTimeFuture_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (String.IsNullOrEmpty(this._startHour.Text) ||
            String.IsNullOrEmpty(this._startMinute.Text) ||
            String.IsNullOrEmpty(this._startAmPm.Text) ||
            String.IsNullOrEmpty(this._endHour.Text) ||
            String.IsNullOrEmpty(this._endMinute.Text) ||
            String.IsNullOrEmpty(this._endAmPm.Text))
        {
            return;
        }
        else
        {
            // TODO: Deal with differing timezones.
            DateTime startTime = DateTime.Parse(String.Format("{0} {1}:{2} {3}",
                this._startDate.Text, this._startHour.Text, this._startMinute.Text, this._startAmPm.Text));
            DateTime endTime = DateTime.Parse(String.Format("{0} {1}:{2} {3}",
                this._endDate.Text, this._endHour.Text, this._endMinute.Text, this._endAmPm.Text));

            args.IsValid = (endTime > startTime);
        }
    }

    protected void _searchButton_Click(object sender, EventArgs e)
    {
        DateTime startDateTime = DateTime.Now;
        DateTime endDateTime = DateTime.MaxValue;

        if (!String.IsNullOrEmpty(this._startDate.Text))
        {
            if (String.IsNullOrEmpty(this._startHour.Text) ||
                String.IsNullOrEmpty(this._startMinute.Text) ||
                String.IsNullOrEmpty(this._startAmPm.Text))
            {
                this._startHour.Text = "12";
                this._startMinute.Text = "00";
                this._startAmPm.Text = "AM";
            }
            startDateTime = DateTime.Parse(String.Format("{0} {1}:{2} {3}",
                this._startDate.Text, this._startHour.Text, this._startMinute.Text, this._startAmPm.Text));
        }

        if (!String.IsNullOrEmpty(this._endDate.Text))
        {
            if (String.IsNullOrEmpty(this._endHour.Text) ||
                String.IsNullOrEmpty(this._endMinute.Text) ||
                String.IsNullOrEmpty(this._endAmPm.Text))
            {
                this._endHour.Text = "11";
                this._endMinute.Text = "55";
                this._endAmPm.Text = "PM";
            }
            endDateTime = DateTime.Parse(String.Format("{0} {1}:{2} {3}",
                this._endDate.Text, this._endHour.Text, this._endMinute.Text, this._endAmPm.Text));

            if (endDateTime < startDateTime)
            {
                startDateTime = DateTime.MinValue;
            }
        }

//        this.Page.Session[Constants.SessionVariables.ContextLocation] = location;

        this._userCalendar.EventList = EventManager.SearchEventsWithDateRange(startDateTime, endDateTime, this._locationControl.Name, this._locationControl.Address1,
            this._locationControl.Address2, this._locationControl.City, this._locationControl.Region, this._locationControl.Country, this._locationControl.PostalCode, 
            this._tagTextbox.Text);
        this._userCalendar.DataBind();
        this._searchResultsPanel.Visible = true;
    }
}