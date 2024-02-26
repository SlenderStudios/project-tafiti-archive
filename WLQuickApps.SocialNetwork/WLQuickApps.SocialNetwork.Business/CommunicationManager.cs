using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Security;

using WLQuickApps.SocialNetwork.Business.Properties;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class CommunicationManager
    {
        static public void SendMessage(string email, string message, string url)
        {
            CommunicationManager.SendMessage(new string[] { email }, message, url);
        }

        static public void SendMessage(IEnumerable<string> recipientEmails, string message, string url)
        {
            UserManager.AssertThatAUserIsLoggedIn();

            Dictionary<string, string> emailsToUserNames = new Dictionary<string, string>();
            Dictionary<string, string> emailsToEmails = new Dictionary<string, string>();
            foreach (string email in recipientEmails)
            {
                User user;
                if (UserManager.TryGetUserByEmail(email, out user) && SettingsWrapper.EnableLiveAlerts)
                {
                    if (!emailsToUserNames.ContainsKey(email.ToLower()))
                    {
                        emailsToUserNames.Add(email.ToLower(), user.UserName);
                    }
                    continue;
                }
                else if (!emailsToEmails.ContainsKey(email.ToLower()))
                {
                    emailsToEmails.Add(email.ToLower(), email);
                }
            }

            if (SettingsWrapper.EnableLiveAlerts)
            {
                LiveAlertsWrapper.SendAlert(emailsToUserNames.Values, message, url, UserManager.LoggedInUser.Title);
            }

            CommunicationManager.SendEmail(emailsToEmails.Values, UserManager.LoggedInUser.Email, UserManager.LoggedInUser.Title, message,
                string.Format("<html><body><p>{0}</p><p>Learn more at <a href=\"{1}\">{1}</a>.</p></body></html>", message, url));
        }

        static public void SendEmail(IEnumerable<string> toEmails, string fromEmail, string fromName, string subject, string body)
        {
            foreach (string email in toEmails)
            {
                CommunicationManager.SendEmail(email, fromEmail, fromName, subject, body);
            }
        }

        static public void SendEmail(string toEmail, string fromEmail, string fromName, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient();
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = subject;

            mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");

            AlternateView plainView = AlternateView.CreateAlternateViewFromString(Regex.Replace(body, @"<(.|\n)*?>", string.Empty), null, "text/plain");
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");

            mailMessage.AlternateViews.Add(plainView);
            mailMessage.AlternateViews.Add(htmlView);

            try
            {
                mailMessage.To.Add(new MailAddress(toEmail));

                mailMessage.From = new MailAddress(SettingsWrapper.SiteFromEmailAddress, fromName);
                mailMessage.ReplyTo = new MailAddress(fromEmail, fromName);

                smtpClient.Send(mailMessage);
            }
            catch (Exception e)
            {
                HealthMonitoringManager.LogWarning(e, "Unable to send email");
            }
        }
    }
}

