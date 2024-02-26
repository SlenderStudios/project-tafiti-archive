<%@ WebHandler Language="C#" Class="ImageHandler" %>

using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web;
using Microsoft.Security.Application;

public class ImageHandler : IHttpHandler
{
    string accountId = "";
    string accountKey = "";

    public void ProcessRequest(HttpContext context)
    {
        NameValueCollection settings = ConfigurationManager.AppSettings;
        accountId = settings["silverlight_accountid"];
        accountKey = settings["silverlight_accountkey"];
        
        // Get the image passed in the querystring
        string image = context.Request.QueryString["image"];
        if (string.IsNullOrEmpty(image))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }
        
        try
        {
            // Construct the request URI
            string uri = string.Format("https://silverlight.services.live.com/{0}", AntiXss.UrlEncode(image));

            // Authorization header
            byte[] userPass = Encoding.Default.GetBytes(accountId + ":" + accountKey);

            // Create a WebClient and add the basic authentication header
            WebClient client = new WebClient();
            client.Headers["Authorization"] = "Basic " + Convert.ToBase64String(userPass);

            // Download the image file
            byte[] bytes = client.DownloadData(uri);
            context.Response.ContentType = client.ResponseHeaders["content-type"];
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}