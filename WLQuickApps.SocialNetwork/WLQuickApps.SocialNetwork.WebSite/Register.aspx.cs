using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;

public partial class Register : System.Web.UI.Page
{
    private string _windowsLiveUUID;

    protected void Page_Init(object sender, EventArgs e)
    {
        this.RegisterRequiresControlState(this);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserManager.IsUserLoggedIn())
        {
            
            WindowsLiveLogin.AuthenticateUser(out this._windowsLiveUUID);

            this.Response.Redirect(FormsAuthentication.DefaultUrl);

        }

        if (this.IsPostBack)
        {
            this._windowsLiveUUID = WindowsLiveLogin.GetWindowsLiveUUID();
        }
        else
        {
            for (int year = DateTime.Now.Year - 13; year >= 1900; year--)
            {
                this._birthYear.Items.Add(year.ToString());
            }

            foreach (string gender in Enum.GetNames(typeof(Gender)))
            {
                this._gender.Items.Add(gender);
            }

            WindowsLiveLogin.AuthenticateUser(out this._windowsLiveUUID);
        }

        if (string.IsNullOrEmpty(this._windowsLiveUUID))
        {
            this.Response.Redirect(WindowsLiveLogin.GetLoginUrl());
        }

        User user;
        if (UserManager.TryGetUserByWindowsLiveUUID(this._windowsLiveUUID, out user))
        {
            WindowsLiveLogin.LoginUser(user.UserName);
        }

        this._alertsPanel.Visible = SettingsWrapper.EnableLiveAlerts;
        
    }

    protected override void LoadControlState(object savedState)
    {
        Dictionary<string, object> stateData = (Dictionary<string, object>)savedState;

        base.LoadControlState(stateData[WebConstants.ControlStateVariables.BaseState]);
        this._windowsLiveUUID = stateData[WebConstants.ControlStateVariables.WindowsLiveUUID] as string;
    }

    protected override object SaveControlState()
    {
        Dictionary<string, object> stateData = new Dictionary<string, object>(2);

        stateData[WebConstants.ControlStateVariables.BaseState] = base.SaveControlState();
        stateData[WebConstants.ControlStateVariables.WindowsLiveUUID] = this._windowsLiveUUID;

        return stateData;
    }

    protected void _birthDateValid_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string inputDate = String.Format("{0} {1}, {2}", this._birthMonth.Text, this._birthDay.Text, this._birthYear.Text);

        DateTime birthDate;
        args.IsValid = ((DateTime.TryParse(inputDate, out birthDate)) && (birthDate.AddYears(13) < DateTime.Now));
    }

    protected void _userNameAvailable_ServerValidate(object source, ServerValidateEventArgs args)
    {
        User user;
        args.IsValid = !UserManager.TryGetUserByUserName(args.Value, out user);
    }

    protected void _emailAvailable_ServerValidate(object source, ServerValidateEventArgs args)
    {
        User user;
        args.IsValid = !UserManager.TryGetUserByEmail(args.Value, out user);
    }

    protected void _pictureValid_ServerValidate(object source, ServerValidateEventArgs args)
    {
        try
        {
            System.Drawing.Bitmap.FromStream(this._picture.FileContent);
            args.IsValid = true;
        }
        catch (ArgumentException)
        {
            args.IsValid = false;
        }
    }

    protected void _signUp_Click(object sender, EventArgs e)
    {
        // Validate the page
        this.Page.Validate();

        // Is the page valid?
        if (!this.Page.IsValid)
        {
            // Return it
            return;
        }

        // Parse the gender enum
        Gender gender = (Gender)Enum.Parse(typeof(Gender), this._gender.Text);

        // Parse the date.
        DateTime birthDate = DateTime.Parse(
            String.Format("{0} {1}, {2}", this._birthMonth.Text, this._birthDay.Text, this._birthYear.Text));

        // Spin up a new user object
        User newUser = UserManager.CreateUser(
                                                this._userName.Text,
                                                this._email.Text,
                                                this._firstName.Text,
                                                this._lastName.Text,
                                                gender,
                                                birthDate,
                                                this._windowsLiveUUID,
                                                this._aboutMe.Text,
                                                this._locationControl.LocationItem,
                                                this._rssFeedUrl.Text,
                                                (this._picture.FileBytes.Length > 0) ? this._picture.FileBytes : null,
                                                this._MessengerIdentifier.Text, _DomainAuthenticationToken.Text, _OwnerHandle.Text
                                            );

        

        // Log the user in.
        WindowsLiveLogin.LoginUser(newUser.UserName);
    }

   


}