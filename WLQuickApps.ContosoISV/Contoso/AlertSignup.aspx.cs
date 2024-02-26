/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com

using System;
using System.Web.UI;
using Contoso.Alerts;

namespace Contoso
{
    public partial class AlertSignup : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        protected void btnSignup_Click(object sender, EventArgs e)
        {
            checkSignup();
        }

        private void checkSignup()
        {
            ErrorMessage.Text = string.Empty;
            try
            {
                string result = Alert.CheckUserSignup(txtEmail.Text, "http://alerts.msn.com");
                if (result != string.Empty)
                {
                    Response.Redirect(result);
                }
            }
            catch (Exception e)
            {
                ErrorMessage.Text = e.Message;
            }
        }

        protected void btnGroup_Click(object sender, EventArgs e)
        {
            ErrorMessage.Text = string.Empty;
            try
            {
                if (txtEmail.Text.Length > 0 && txtGroup.Text.Length > 0)
                {
                    checkSignup();
                    Alert.AddUserToGroup(txtEmail.Text, txtGroup.Text);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
            }
        }
    }
}