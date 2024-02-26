using System;
using System.Web;
using System.IO;
using WindowsLive;

/// <summary>
/// This is the default aspx.cs page for the sample Web Auth site.
/// It gets the application ID and user ID for display on the main
/// page.  
/// </summary>
public partial class DefaultPage : System.Web.UI.Page
{
    const string LoginCookie = "webauthtoken";

    // Initialize the WindowsLiveLogin module.
    static WindowsLiveLogin wll = new WindowsLiveLogin(true);
    protected static string AppId = wll.AppId;
    protected string refer_link;
    protected string UserId;
    protected void Page_Load(object sender, EventArgs e)
    {
        /* If the user token has been cached in a site cookie, attempt
           to process it and extract the user ID. */
        HyperLink1.NavigateUrl = "http://login.live.com/wlogin.srf?appid="+System.Configuration.ConfigurationSettings.AppSettings["wll_appid"]+"&alg=wsignin1.0";
        HttpRequest req = HttpContext.Current.Request;
        HttpCookie loginCookie = req.Cookies[LoginCookie];

        if(loginCookie != null){
            string token = loginCookie.Value;

            if (!string.IsNullOrEmpty(token))
            {
                WindowsLiveLogin.User user = wll.ProcessToken(token);

                if (user != null) 
                {
                    UserId = user.Id;
                    Session["WlKey"] = UserId;
                    refer_link = Convert.ToString(Session["Refer_link"]);
                    Response.Redirect(refer_link, false);
                }
            }
        }
    }
}
