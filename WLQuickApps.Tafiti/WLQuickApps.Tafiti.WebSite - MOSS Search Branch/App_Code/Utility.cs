using System;
using System.Configuration;
using System.Net.Mail;
using System.Web;
using System.Web.Security;

namespace WLQuickApps.Tafiti.WebSite
{
    public static class Utility
    {
        public static void VerifyIsSelfRequest()
        {
            // Check for cross-site request forgery by verifying that the referer matches our domain.
            // Although users can configure their browsers to not send the referer header, it disables
            // so many sites that it's not practical.
            // See http://en.wikipedia.org/wiki/Cross-site_request_forgery
            if (HttpContext.Current.Request.UrlReferrer == null ||
                HttpContext.Current.Request.UrlReferrer.Host != HttpContext.Current.Request.Url.Host)
                throw new HttpException((int)System.Net.HttpStatusCode.Forbidden, "Forbidden");
        }

        public static bool IsValidEmailAddress(string s)
        {
            try
            {
                System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(s);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static void SendEmail(string from, string[] addresses, string subject, string message)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from);
            Array.ForEach<string>(addresses, delegate(string address)
            {
                msg.To.Add(address);
            });
            msg.Subject = subject;
            msg.IsBodyHtml = false;
            msg.Body = message;

            if (SettingsWrapper.SendEmail)
            {
                SmtpClient client = new SmtpClient();
                client.Send(msg);
            }
        }
    }
}