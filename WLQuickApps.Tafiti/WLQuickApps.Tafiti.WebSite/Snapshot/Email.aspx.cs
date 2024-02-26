using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

using WLQuickApps.Tafiti.Business;
using WLQuickApps.Tafiti.WebSite;

public partial class Email : System.Web.UI.Page
{
    private readonly int MAX_REQUEST_BODY_STRING_LENGTH = 256 * 1024;

    void Page_Load(object sender, EventArgs args)
    {
        Utility.VerifyIsSelfRequest();

        Response.Cache.SetCacheability(HttpCacheability.NoCache);  // prevent caching of this page

        if (!UserManager.IsUserLoggedIn)
        {
            throw new HttpException((int)HttpStatusCode.Forbidden, "Forbidden");
        }

        EmailSnapshotRequest request;
        request = ParseRequest();

        string shelfStackID = request.ShelfStackID;

        Uri uri = SnapshotUtility.GetSnapshotUrl(shelfStackID);
        SnapshotUtility.SendEmail(UserManager.LoggedInUser.DisplayName, request.To, request.Subject, request.Message, uri.ToString());

        Response.StatusCode = (int)HttpStatusCode.OK;
        Response.Output.WriteLine(shelfStackID);

        SnapshotUtility.UpdateEmailQuota();
    }

    private EmailSnapshotRequest ParseRequest()
    {
        EmailSnapshotRequest request;
        try
        {
            string contentBody;
            contentBody = ReadRequestContentBody();
            request = Deserialize(contentBody);
            Validate(request);
            return request;
        }
        catch (Exception e)
        {
            throw new HttpException((int)HttpStatusCode.BadRequest, "Bad Request", e);
        }
    }

    private string ReadRequestContentBody()
    {
        char[] buffer = new char[MAX_REQUEST_BODY_STRING_LENGTH];
        StreamReader reader = new StreamReader(Request.InputStream, System.Text.Encoding.UTF8);
        int count = reader.Read(buffer, 0, buffer.Length);
        if (count == MAX_REQUEST_BODY_STRING_LENGTH)
            throw new ApplicationException("Maximum request content body length exceeded.");

        string content;
        content = new string(buffer, 0, count);
        return content;
    }

    private EmailSnapshotRequest Deserialize(string contentBody)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        serializer.MaxJsonLength = MAX_REQUEST_BODY_STRING_LENGTH;
        serializer.RecursionLimit = 10;
        return serializer.Deserialize<EmailSnapshotRequest>(contentBody);
    }


    private bool Validate(EmailSnapshotRequest request)
    {
        if (request.To.Length > SnapshotUtility.MAX_EMAIL_RECIPIENTS)
            throw new ApplicationException("Too many email recipients");

        // verify email addresses
        foreach (string s in request.To)
        {
            System.Net.Mail.MailAddress address = new System.Net.Mail.MailAddress(s);
        }

        // verify subject
        if (request.Subject.Length > SnapshotUtility.MAX_EMAIL_SUBJECT_LENGTH)
            throw new ApplicationException("Email subject too long");

        // verify message body
        if (request.Message.Length > SnapshotUtility.MAX_EMAIL_MESSAGE_BODY_LENGTH)
            throw new ApplicationException("Email message body too long");

        // Verify shelf contents
        if (request.ShelfStackID.Length > SnapshotUtility.MAX_SHELF_SLOT_LENGTH)
            throw new ApplicationException("Email shelf stack ID too long");

        return true;
    }
}
