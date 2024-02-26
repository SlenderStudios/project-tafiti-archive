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
using WLQuickApps.SocialNetwork.WebSite;

public partial class User_EditProfile : System.Web.UI.Page
{
    private string _editUserName;

    private void UpdateLiveAlertsPanel()
    {
        if (!SettingsWrapper.EnableLiveAlerts)
        {
            this._liveAlertsPanel.Visible = false;
            return;
        }

        this._liveAlertsChangeHyperlink.NavigateUrl = SettingsWrapper.LiveAlertsChangeUrl;
        this._liveAlertsSignUpHyperlink.NavigateUrl = LiveAlertsWrapper.GetAlertsSignupUrl(this.Request.Url.AbsoluteUri);
        this._liveAlertsSignUpHyperlink.Visible = (this._liveAlertsSignUpHyperlink.NavigateUrl.Length > 0);
        this._liveAlertsChangeHyperlink.Visible = (this._liveAlertsSignUpHyperlink.NavigateUrl.Length == 0);
        this._liveAlertsUnsubscribeHyperlink.Visible = (this._liveAlertsSignUpHyperlink.NavigateUrl.Length == 0);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this._editUserName = this.Request.QueryString[WebConstants.QueryVariables.UserName];

        this._invalidPictureError.Visible = false;

        if (!this.IsPostBack)
        {
            for (int year = DateTime.Now.Year - 13; year >= 1900; year--)
            {
                this._birthYear.Items.Add(year.ToString());
            }

            foreach (string gender in Enum.GetNames(typeof(Gender)))
            {
                this._gender.Items.Add(gender);
            }

            User user = UserManager.GetUserByUserName(this._editUserName);
            this._title.Text = user.Title;
            this._firstName.Text = user.FirstName;
            this._lastName.Text = user.LastName;
            this._rssFeedUrl.Text = user.RssFeedUrl;
            this._aboutMe.Text = user.Description;

            this._locationControl.LocationItem = user.Location;

            this._birthDay.Text = user.DateOfBirth.Day.ToString();
            this._birthMonth.Text = user.DateOfBirth.ToString("MMMM");
            this._birthYear.Text = user.DateOfBirth.Year.ToString();
            this._gender.Text = user.Gender.ToString();

            this._pictureOldImage.Item = user;

            this._MessengerIdentifier.Text = user.MessengerPresenceID;

            // Is the token null?
            if (UserManager.LoggedInUser.PhotoPermissionToken != null)
            {
                // no - display the values
                this._DomainAuthenticationToken.Text = UserManager.LoggedInUser.PhotoPermissionToken.DomainAuthenticationToken;
                this._OwnerHandle.Text = UserManager.LoggedInUser.PhotoPermissionToken.OwnerHandle;
            }
        }

        this.UpdateLiveAlertsPanel();
    }

    protected void _birthDateValid_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string inputDate = String.Format("{0} {1}, {2}", this._birthMonth.Text, this._birthDay.Text, this._birthYear.Text);

        DateTime birthDate;
        args.IsValid = (DateTime.TryParse(inputDate, out birthDate));
    }

    private void RedirectToProfile()
    {
        if (String.IsNullOrEmpty(this._editUserName))
        {
            this.Response.Redirect("~/Friend/ViewProfile.aspx");
        }
        else
        {
            this.Response.Redirect(WebUtilities.GetViewItemUrl(UserManager.GetUserByUserName(this._editUserName)));
        }
    }

    protected void _saveButton_Click(object sender, EventArgs e)
    {
        if (this.Page.IsValid)
        {
            Gender gender = (Gender)Enum.Parse(typeof(Gender), this._gender.Text);
            DateTime birthDate = DateTime.Parse(
                String.Format("{0} {1}, {2}", this._birthMonth.Text, this._birthDay.Text, this._birthYear.Text));

            User user = UserManager.GetUserByUserName(this._editUserName);

            user.Title = _title.Text;
            user.FirstName = _firstName.Text;
            user.LastName = _lastName.Text;
            user.DateOfBirth = birthDate;
            user.Gender = gender;
            user.RssFeedUrl = _rssFeedUrl.Text;
            user.Description = _aboutMe.Text;
            user.Location = this._locationControl.LocationItem;
            user.MessengerPresenceID = _MessengerIdentifier.Text.Trim();
            user.PhotoPermissionToken = new PhotoToken(this._OwnerHandle.Text, this._DomainAuthenticationToken.Text);

            if (this._pictureUpload.HasFile)
            {
                try
                {
                    user.SetThumbnail(this._pictureUpload.FileBytes);
                }
                catch (ArgumentException)
                {
                    this._invalidPictureError.Visible = true;
                    return;
                }
            }

            UserManager.UpdateUser(user);

            RedirectToProfile();
        }
    }

    protected void _cancelButton_Click(object sender, EventArgs e)
    {
        RedirectToProfile();
    }

    protected void _liveAlertsUnsubscribe_Command(object sender, CommandEventArgs e)
    {
        LiveAlertsWrapper.UnsubscribeAll();

        this.UpdateLiveAlertsPanel();
    }
    

}
