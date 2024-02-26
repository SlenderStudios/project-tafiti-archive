/*****************************************************************************
 * ProfilePage.apx
 * Notes: Page to display user's profile
 * **************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using WLQuickApps.ContosoBank.Entity;
using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank
{
    public partial class ProfilePage : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Don't let the user to this page unless they've logged in
            if (Session["ApplicationUserID"] == null)
            {
                FormsAuthentication.SignOut();
                Response.Redirect("Default.aspx");
            }

            if (!Page.IsPostBack)
            {
                bindData();
            }
        }

        private void bindData()
        {
            UserProfile currentUser = UserLogic.GetCurrentUser();

            // Create a avatar button list with avatar imagages
            List<Avatar> avatars = AvatarLogic.GetAvatars();
            foreach (Avatar avatar in avatars)
            {
                Page pageHolder = new Page();
                UserControl viewControl = (UserControl) pageHolder.LoadControl("~/controls/Avatar.ascx");
                ((IAvatarData) viewControl).Item = avatar;
                pageHolder.Controls.Add(viewControl);

                StringWriter output = new StringWriter();
                HttpContext.Current.Server.Execute(pageHolder, output, false);

                bool selectedAvatar;
                if (currentUser == null)
                {
                    selectedAvatar = avatar.IsDefault;
                }
                else
                {
                    selectedAvatar = currentUser.Avatar == avatar.Location;
                }
                AvatarDataList.Items.Add(new ListItem
                                             {
                                                 Text = output.ToString(),
                                                 Value = avatar.ID.ToString(),
                                                 Enabled = currentUser == null,
                                                 Selected = selectedAvatar
                                             });
            }

            PreferredBackgroundDropDownList.DataSource = BackgroundLogic.GetBackgrounds();
            PreferredBackgroundDropDownList.DataBind();

            ProfileBase profile = HttpContext.Current.Profile;
            PreferredBackgroundDropDownList.Items.FindByText(profile["PreferredBackground"].ToString()).Selected = true;

            // If the user exists populate the form with their profile information
            if (currentUser != null)
            {
                DisplayNameTextBox.Text = currentUser.DisplayName;
                DisplayNameTextBox.Enabled = false;

                PostCodeTextBox.Text = currentUser.Postcode;
                PostCodeTextBox.Enabled = false;
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            // if the user doesn't exist - remove the token from session
            if (UserLogic.GetCurrentUser() == null)
            {
                HttpContext.Current.Session["ApplicationUserID"] = null;
            }
            Response.Redirect("Default.aspx");
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                UserProfile currentUser = UserLogic.GetCurrentUser();
                if (currentUser != null)
                {
                    // Update exsiting user's preferred background
                    ProfileBase profile = HttpContext.Current.Profile;
                    profile["PreferredBackground"] = HttpUtility.HtmlEncode(PreferredBackgroundDropDownList.SelectedItem.Text);
                    Response.Redirect("Default.aspx");
                }
                else if (validUserName())
                {
                    // ensure no html and make sure input is valid length
                    string country = HttpUtility.HtmlEncode(CountryTextBox.Text);
                    country = country.Length > 50 ? country.Substring(0, 50) : country;
                    string displayname = HttpUtility.HtmlEncode(DisplayNameTextBox.Text);
                    displayname = displayname.Length > 50 ? displayname.Substring(0, 50) : displayname;
                    string postcode = HttpUtility.HtmlEncode(PostCodeTextBox.Text);
                    postcode = postcode.Length > 6 ? postcode.Substring(0, 60) : postcode;

                    // Create a new user profile
                    UserProfile user = new UserProfile
                                           {
                                               Avatar = AvatarLogic.GetAvatarByID(Convert.ToInt32(AvatarDataList.SelectedItem.Value)).Location,
                                               Country = country,
                                               DisplayName = displayname,
                                               UserProfileID = Guid.NewGuid(),
                                               Postcode = postcode
                                           };
                    UserLogic.CreateUser(user);
                    ProfileBase profile = ProfileBase.Create(user.DisplayName, true);
                    profile["PreferredBackground"] = HttpUtility.HtmlEncode(PreferredBackgroundDropDownList.SelectedItem.Text);
                    profile.Save();

                    // Log the user into forms authentication
                    FormsAuthentication.SetAuthCookie(user.DisplayName, true);

                    Response.Redirect("Default.aspx");
                }
            }
        }

        private bool validUserName()
        {
            if (UserLogic.UserNameExists(DisplayNameTextBox.Text))
            {
                DisplayNameCustomValidator.IsValid = false;
                return false;
            }
            return true;
        }
    }
}