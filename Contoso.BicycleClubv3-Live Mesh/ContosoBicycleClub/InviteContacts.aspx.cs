using System;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.LiveFX.Client;
using Microsoft.LiveFX.ResourceModel;
using Mail = System.Net.Mail;
using WLQuickApps.ContosoBicycleClub.UI;

namespace WLQuickApps.ContosoBicycleClub
{
    public partial class InviteContacts : System.Web.UI.Page
    {
        private LiveOperatingEnvironment loe = new LiveOperatingEnvironment();

        protected void Page_Load(object sender, EventArgs e)
        {
            LiveItemAccessOptions liao = new LiveItemAccessOptions(true);
            HttpRequest req = HttpContext.Current.Request;
            loe.Connect(WebProfile.Current.ContactsDelToken, AuthenticationTokenType.DelegatedAuthToken, new Uri(Constants.ServiceEndPoint), liao);
            if (!loe.Contacts.IsLoaded) loe.Contacts.Load();
            foreach (Contact contact in loe.Contacts.Entries)
            {
                foreach (ContactEmail email in contact.Resource.Emails)
                {
                    ListItem item = new ListItem(contact.Resource.Title + " (" + email.Value + ")", email.Value);
                    ContactList.Items.Add(item);
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            // Retrieve the mail sttings from the web.config.
            WindowsLiveLogin wll = new WindowsLiveLogin(true);
            WebProfile userProfile = WebProfile.Current;
            // This should be set to your SMTP server.  
            string host = wll.MailHost;
            string from = wll.MailFrom;
            string subject = wll.MailSubject;
            string body = "Come check out the Contoso Bicycle Club for the latest on biking events." + Environment.NewLine + userProfile.DisplayName;

            ResultList.Items.Clear();
            foreach (ListItem item in ContactList.Items)
            {
                if (item.Selected)
                {
                    // Email the invitation.
                    bool result = SendMail(host, item.Value, from, subject, body, false);
                    ResultList.Items.Add(item.Text + " " + (result ? "succeeded." : "failed."));
                }
            }
            ResultList.Visible = true;
            ContactList.Visible = false;
            SaveButton.Visible = false;
            CancelButton.Visible = false;
            DoneButton.Visible = true;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.LoginPage);
        }

        protected void DoneButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Constants.LoginPage);
        }

        private bool SendMail(string host, string to, string from, string subject, string body, bool IsHTML)
        {
            try
            {
                Mail.SmtpClient smtpClient = new Mail.SmtpClient(host);
                Mail.MailMessage mailMessage = new Mail.MailMessage(from, to);
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = IsHTML;
                mailMessage.Subject = subject;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception err)
            {
                return false;
            }
        }
    }
}
