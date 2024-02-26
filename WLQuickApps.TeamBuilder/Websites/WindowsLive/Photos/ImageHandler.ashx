<%@ WebHandler Language="C#" Class="ImageHandler" %>
/////////////////////////////////////////////////////////
//       IMPORTANT NOTICE ABOUT THE PHOTO API          //
// The photo api used in this project is a CTP i.e.    //
// you should not use this API in production, it is    //
// for preview sake. Please do not implement this      //
// functionality in a production environment.          //
/////////////////////////////////////////////////////////
using System;
using System.Net;
using System.Web;
using WindowsLive;

public class ImageHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        // Get the url passed in the querystring
        string url = context.Request.QueryString["url"];
        if (string.IsNullOrEmpty(url) || !Utilities.IsValidUrl(url))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return;
        }

        if (context.Cache[url] != null)
        {
            byte[] bytes = (byte[])context.Cache[url];
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
        }
        else
        {

            // Get the lid and delegation token.
            // We have saved the consent token to session state, all we 
            // need to do is extract the values from the consent token.
            WindowsLiveLogin.ConsentToken ct = Utilities.GetAppConsentToken();

            if (ct != null && ct.IsValid())
            {
                try
                {
                    // Create a WebClient and add the delegation token to the header
                    WebClient client = new WebClient();
                    client.Headers["Authorization"] = "DelegatedToken dt=\"" + ct.DelegationToken + "\"";

                    // Download the image file
                    byte[] bytes = client.DownloadData(url);
                    context.Response.ContentType = client.ResponseHeaders["content-type"];
                    context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                    context.Cache.Insert(url, bytes);
                }
                catch (Exception)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }
            }
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