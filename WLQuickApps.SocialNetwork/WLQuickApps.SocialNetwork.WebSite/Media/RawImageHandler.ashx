<%@ WebHandler Language="C#" Class="RawImageHandler" %>

using System;
using System.Web;
using WLQuickApps.SocialNetwork.Business;
using WLQuickApps.SocialNetwork.WebSite;
using System.Web.SessionState;
using System.Threading;


public class RawImageHandler : IHttpAsyncHandler, IReadOnlySessionState
{

    public void ProcessRequest(HttpContext context)
    {
        throw new InvalidOperationException();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, Object extraData)
    {
        AsynchOperation asynch = new AsynchOperation(cb, context, extraData);
        asynch.StartAsyncWork();
        return asynch;
    }

    public void EndProcessRequest(IAsyncResult result)
    {
    }

}

class AsynchOperation : IAsyncResult
{
    private bool _completed;
    private Object _state;
    private AsyncCallback _callback;
    private HttpContext _context;

    bool IAsyncResult.IsCompleted { get { return _completed; } }
    WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
    Object IAsyncResult.AsyncState { get { return _state; } }
    bool IAsyncResult.CompletedSynchronously { get { return false; } }

    public AsynchOperation(AsyncCallback callback, HttpContext context, Object state)
    {
        _callback = callback;
        _context = context;
        _state = state;
        _completed = false;
    }

    public void StartAsyncWork()
    {
        ThreadPool.QueueUserWorkItem(new WaitCallback(StartAsyncTask), null);
    }

    private void StartAsyncTask(Object workItemState)
    {
        object ob = _context.Request.QueryString["IMAGEURL"];
        object ob2 = _context.Session["DomainAuthToken"];
        if (ob == null || ob2 == null) return;
        string domainToken = ob2.ToString();
        string imageURL = ob.ToString();
        if (imageURL.Length < 4 || domainToken.Length < 4) return;
        byte[] pictureBytes = MediaUtil.GetImagefromSpacesURL(imageURL, domainToken, true);
        if (pictureBytes == null) return;
        pictureBytes = PhotoUtil.GetThumbImage(pictureBytes, 256, 256);
        _context.Response.ContentType = "image/jpeg";
        _context.Response.Cache.VaryByParams["IMAGEURL"] = true;
        _context.Response.Cache.SetExpires(DateTime.Now.AddSeconds(60));
        _context.Response.Cache.SetCacheability(HttpCacheability.Private);

        if (pictureBytes != null) _context.Response.BinaryWrite(pictureBytes);
        _completed = true;
        _callback(this);

    }

}
