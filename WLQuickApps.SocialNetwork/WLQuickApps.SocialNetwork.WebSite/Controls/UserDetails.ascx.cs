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
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class UserDetails : System.Web.UI.UserControl
{
    [Bindable(true)]
    [ToolboxItem(true)]
    public User UserItem
    {
        get { return this._userItem; }
        set { this._userItem = value; }
    }
    private User _userItem;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (this._userItem == null)
            return;
        this._userPicture.NavigateUrl = WebUtilities.GetViewItemUrl(this._userItem);
        this._userPicture.Item = this._userItem;

        this.UserNameLabel.Text = this._userItem.UserName;

        this.FirstNameLabel.Text = this._userItem.FirstName;
        this.LastNameLabel.Text = this._userItem.LastName;

        this.DateOfBirthLabel.Text = this._userItem.DateOfBirth.ToString("M");
        this.LastLoginDateLabel.Text = this.FormatLastSeen(this._userItem.LastLoginDate);

        this.CreateDateLabel.Text = this._userItem.CreateDate.ToString("D");

        this.GenderPanel.Visible = (this._userItem.Gender != Gender.Unspecified);
        this.GenderLabel.Text = this._userItem.Gender.ToString();

        // Use the UserPresence control and pass in the MessengerPresenceID
        this.UserPresence1.LoadPresence(this.UserItem.MessengerPresenceID);
        
        this._onlineLabel.Visible = UserManager.IsUserOnline(this._userItem);

        base.OnPreRender(e);
    }

    private string FormatLastSeen(DateTime timestamp)
    {
        string day;
        string time;

        TimeSpan difference = DateTime.Now.Date - timestamp.Date;

        if (difference.TotalSeconds < 0)
            return string.Empty;

        if (DateTime.Now.Date == timestamp.Date)
            day = "Today";
        else if (difference.Days == 1)
            day = "Yesterday";
        else if (difference.Days < 7)
            day = String.Format("{0} days ago", difference.Days);
        else
            day = timestamp.Date.ToShortDateString();

        if (((DateTime.Now.Hour - timestamp.Hour) < 2) && (day == "Today"))
            time = "Just Now";
        else
            time = timestamp.ToShortTimeString();

        return String.Format("{0}, {1}", day, time);
    }
}
