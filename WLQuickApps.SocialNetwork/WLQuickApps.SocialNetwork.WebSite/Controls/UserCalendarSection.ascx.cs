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

public partial class UserCalendarSection : System.Web.UI.UserControl
{
    #region Public fields
    [ToolboxItem(true)]
    public string HeaderText
    {
        get
        {
            string s = this.ViewState[WebConstants.ViewStateVariables.HeaderText] as string;
            if (s == null)
            {
                return string.Empty;
            }
            else
            {
                return s;
            }
        }
        set
        {
            this.ViewState[WebConstants.ViewStateVariables.HeaderText] = value;
        }
    }
    /// <summary>
    ///  #see Issue #3197
    /// </summary>
    //private string _headerText;

    public DateTime StartDate
    {
        get
        {
            object startDate = this.ViewState[WebConstants.ViewStateVariables.StartDate];
            if (startDate is DateTime)
            {
                return (DateTime)startDate;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
        set
        {
            this.ViewState[WebConstants.ViewStateVariables.StartDate] = value;
        }
    }

    public DateTime EndDate
    {
        get
        {
            object endDate = this.ViewState[WebConstants.ViewStateVariables.EndDate];
            if (endDate is DateTime)
            {
                return (DateTime)endDate;
            }
            else
            {
                return DateTime.MaxValue;
            }
        }
        set
        {
            this.ViewState[WebConstants.ViewStateVariables.EndDate] = value;
        }
    }

    public String UserName
    {
        get { return this._userName; }
        set { this._userName = value; }
    }
    private String _userName;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserManager.IsUserLoggedIn())
        {
            this._userName = UserManager.LoggedInUser.UserName;
        }
    }

    protected override void DataBind(bool raiseOnDataBinding)
    {
        if ((this.StartDate != DateTime.MinValue) && (this.EndDate != DateTime.MinValue))
        {
            this._dataList.DataSourceID = "_pagedEventsDataSource";
        }

        base.DataBind(raiseOnDataBinding);
    }

    protected void _pagedEventsDataSource_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters["userName"] = this._userName;
        e.InputParameters["searchRangeStart"] = this.StartDate;
        e.InputParameters["searchRangeEnd"] = this.EndDate;
    }

    public override void DataBind()
    {
        base.DataBind();

        this._dataList.DataBind();
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (EventManager.GetEventsForUserCount(UserManager.LoggedInUser.UserName, this.StartDate, this.EndDate, UserGroupStatus.Joined) == 0)
        {
            this.Visible = false;
        }
        base.OnPreRender(e);
    }
}
