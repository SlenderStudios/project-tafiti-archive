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
using System.ComponentModel;
using System.Collections.Generic;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public enum CalendarDisplay
{
    Full,
    SimpleList,
}

public partial class UserCalendar : System.Web.UI.UserControl
{
    [Bindable(true)]
    [ToolboxItem(true)]
    public string UserName
    {
        get { return this._userName; }
        set { this._userName = value; }
    }
    private string _userName;

    [Themeable(true)]
    [ToolboxItem(true)]
    public CalendarDisplay DisplayMode
    {
        get { return this._displayMode; }
        set { this._displayMode = value; }
    }
    private CalendarDisplay _displayMode;

    [Bindable(true)]
    public IEnumerable<Event> EventList
    {
        get { return this._eventList; }
        set { this._eventList = value; }
    }
    private IEnumerable<Event> _eventList;

    [Bindable(true)]
    public UserGroupStatus EventStatus
    {
        get { return this._eventStatus; }
        set { this._eventStatus = value; }
    }
    private UserGroupStatus _eventStatus = UserGroupStatus.Joined;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.DataBind();
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        this.DataBind();
        base.OnPreRender(e);
    }

    public override void DataBind()
    {
 	    base.DataBind();

        if ((this._eventList == null) && (String.IsNullOrEmpty(this._userName)) && (UserManager.IsUserLoggedIn()))
        {
            this._userName = UserManager.LoggedInUser.UserName;
        }

        //if (!String.IsNullOrEmpty(this._userName))
        //{
        //    this._eventList = EventManager.GetEventsForUser(this._userName, DateTime.Today, DateTime.MaxValue, this._eventStatus);
        //}

        switch (this._displayMode)
        {
            case CalendarDisplay.Full:
                this.ShowFullCalendar();
                break;

            case CalendarDisplay.SimpleList:
                this.ShowSimpleListCalendar();
                break;
        }
    }

    private void ShowFullCalendar()
    {
        this._calendarView.SetActiveView(this._fullView);

        DateTime today = DateTime.Today;
        DateTime tomorrow = DateTime.Today.AddDays(1);
        DateTime threeDaysFromNow = DateTime.Today.AddDays(2);
        DateTime endOfWeek = this.GetEndOfWeek(DateTime.Today);
        DateTime endOfNextWeek = endOfWeek.AddDays(7);

        this._eventsTodaySection.HeaderText = String.Format("Today ({0})", DateTime.Today.ToString("MMMM d"));
        this._eventsTomorrowSection.HeaderText = String.Format("Tomorrow ({0})", DateTime.Today.AddDays(1).ToString("MMMM d"));

        this._eventsTodaySection.StartDate = today;
        this._eventsTodaySection.EndDate = tomorrow;
        this._eventsTomorrowSection.StartDate = tomorrow;
        this._eventsTomorrowSection.EndDate = threeDaysFromNow;
        this._eventsLaterThisWeekSection.StartDate = threeDaysFromNow;
        this._eventsLaterThisWeekSection.EndDate = endOfWeek;
        this._eventsNextWeekSection.StartDate = endOfWeek;
        this._eventsNextWeekSection.EndDate = endOfNextWeek;
        this._eventsLaterSection.StartDate = endOfNextWeek;
        this._eventsLaterSection.EndDate = DateTime.MaxValue;

        if (!string.IsNullOrEmpty(this._userName))
        {
            this._eventsTodaySection.UserName =
            this._eventsTomorrowSection.UserName =
            this._eventsLaterThisWeekSection.UserName =
            this._eventsNextWeekSection.UserName =
            this._eventsLaterSection.UserName = this._userName;
        }
    }

    private bool IsEventInDateRange(Event eventItem, DateTime rangeStartDate, DateTime rangeEndDate)
    {
        if ((eventItem.StartDateTime >= rangeStartDate) && (eventItem.StartDateTime <= rangeEndDate))
        {
            return true;
        }
        else if ((eventItem.EndDateTime >= rangeStartDate) && (eventItem.EndDateTime <= rangeEndDate))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShowSimpleListCalendar()
    {
        this._calendarView.SetActiveView(this._simpleListView);

        if (this._eventList == null)
        {
            if (!string.IsNullOrEmpty(this._userName))
            {
                this._eventGrid.DataSourceID = "_pagedEventsDataSource";
                this._eventGrid.AllowPaging = true;
                this._eventGrid.PageSize = 4;
            }
        }
        else
        {
            this._eventGrid.DataSourceID = string.Empty;
            this._eventGrid.AllowPaging = false;
            this._eventGrid.DataSource = this._eventList;
            this._eventGrid.DataBind();
        }
    }

    protected void _pagedEventsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = this._userName;
    }

    private DateTime GetEndOfWeek(DateTime day)
    {
        double offset = 0;
        switch (day.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                offset = 6;
                break;
            case DayOfWeek.Monday:
                offset = 5;
                break;
            case DayOfWeek.Tuesday:
                offset = 4;
                break;
            case DayOfWeek.Wednesday:
                offset = 3;
                break;
            case DayOfWeek.Thursday:
                offset = 2;
                break;
            case DayOfWeek.Friday:
                offset = 1;
                break;
            case DayOfWeek.Saturday:
                offset = 0;
                break;
        }

        return day.Date.AddDays(offset).AddHours(23).AddMinutes(59).AddSeconds(59);
    }

    protected override void LoadControlState(object savedState)
    {
        Dictionary<string, object> stateData = (Dictionary<string, object>)savedState;

        base.LoadControlState(stateData[WebConstants.ControlStateVariables.BaseState]);
        this._displayMode = (CalendarDisplay)stateData[WebConstants.ControlStateVariables.DisplayMode];
        this._userName = (string)stateData[WebConstants.ControlStateVariables.UserName];
    }

    protected override object SaveControlState()
    {
        Dictionary<string, object> stateData = new Dictionary<string, object>(3);

        stateData[WebConstants.ControlStateVariables.BaseState] = base.SaveControlState();
        stateData[WebConstants.ControlStateVariables.DisplayMode] = this._displayMode;
        stateData[WebConstants.ControlStateVariables.UserName] = this._userName;

        return stateData;
    }
}
