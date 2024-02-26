using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Xml;

using WLQuickApps.Tafiti.WebSite;

public partial class PostToSpace : System.Web.UI.Page
{
    /// <summary>
    /// Proxy MetaWebLog requests to spaces.live.com.
    ///  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Page_Load(object sender, EventArgs e)
    {
        Utility.VerifyIsSelfRequest();

        string uri = "https://storage.msn.com/storageservice/MetaWeblog.rpc";

        HttpWebRequest storageRequest = (HttpWebRequest) WebRequest.Create(uri);
        storageRequest.Method = "POST";
        storageRequest.ContentType = "text/xml";
        Stream storageRequestStream = storageRequest.GetRequestStream();
        Byte[] buffer = new Byte[4096];
        int count;
        while ((count = Request.InputStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            storageRequestStream.Write(buffer, 0, count);
        }
        storageRequestStream.Close();

        HttpWebResponse storageResponse = (HttpWebResponse) storageRequest.GetResponse();

        // respond
        Response.StatusCode = (int) storageResponse.StatusCode;
        Response.StatusDescription = storageResponse.StatusDescription;
        Stream storageResponseStream = storageResponse.GetResponseStream();
        Stream outputStream = Response.OutputStream;
        while ((count = storageResponseStream.Read(buffer, 0, buffer.Length)) > 0)
        {
            outputStream.Write(buffer, 0, count);
        }
        Response.Flush();
    }

}
