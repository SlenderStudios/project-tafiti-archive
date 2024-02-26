using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Net.Mail;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WLQuickApps.Tafiti.Business;

namespace WLQuickApps.Tafiti.WebSite
{
    /// <summary>
    /// Summary description for Snapshot
    /// </summary>
    public class SnapshotUtility
    {
        public static readonly int MAX_EMAIL_SUBJECT_LENGTH = 500;
        public static readonly int MAX_EMAIL_MESSAGE_BODY_LENGTH = 2000;
        public static readonly int MAX_SHELF_SLOT_LENGTH = 256 * 1024;
        public static readonly int EMAIL_QUOTA_PER_DAY = 50;
        public static readonly int MAX_EMAIL_RECIPIENTS = 50;

        public static Uri GetSnapshotUrl(string shelfStackID)
        {
            UriBuilder ub = new UriBuilder(HttpContext.Current.Request.Url);
            ub.Path = "/";
            ub.Query = string.Format("{0}={1}", Constants.QueryKeys.SnapshotID, shelfStackID);
            return ub.Uri;
        }

        public static string GetSnapshotId(HttpRequest request)
        {
            return request.QueryString[Constants.QueryKeys.SnapshotID];
        }

        public static void UpdateEmailQuota()
        {
            User user = UserManager.LoggedInUser;

            if (user.EmailCountTimestamp == DateTime.Now.Date)
            {
                user.EmailCount += 1;
            }
            else
            {
                user.EmailCount = 1;
                user.EmailCountTimestamp = DateTime.Now;
            }

            UserManager.UpdateUser(user);
        }

        public static void SendEmail(string from, string[] addresses, string subject, string message, string url)
        {
            if (string.IsNullOrEmpty(from))
            {
                from = SettingsWrapper.SiteEmail;
            }

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(from);
            Array.ForEach<string>(addresses, delegate(string address) {
                    msg.To.Add(address);
                });
            msg.Subject = subject;
            msg.IsBodyHtml = false;
            msg.Body = FormatMessage(from, Uri.EscapeUriString(url), message);

            if (SettingsWrapper.SendEmail)
            {
                SmtpClient client = new SmtpClient();
                client.Send(msg);
            }
        }

        private static string FormatMessage(string from, string snapshotUrl, string message)
        {
            // Just to be safe, we'll encode.
            from = HttpUtility.HtmlEncode(from);
            snapshotUrl = snapshotUrl.Replace("<", "&lt");
            message = HttpUtility.HtmlEncode(message);

            return string.Format(@"
{0} has shared web clippings with you at:

  {1}

{2}", from, Uri.EscapeUriString(snapshotUrl), message);
        }
    }

    public class EmailSnapshotRequest
    {
        public string[] To = null;
        public string Subject = null;
        public string Message = null;
        public string ShelfStackID = null;
    }
}