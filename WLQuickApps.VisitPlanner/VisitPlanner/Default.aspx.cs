/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: Default.aspx.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Data.OleDb;
using System.Data.SqlClient;
using VisitPlanner;
using VisitPlanner.Business;
using VisitPlanner.BusinessObjects;
using VESilverlight;
using System.Collections.Generic;
using System.Net;
using Microsoft.Security.Application;

namespace VisitPlanner
{
    public partial class _Default : System.Web.UI.Page
    {
        #region Properties

        /// <summary>
        /// Login cookie
        /// </summary>
        protected static string LoginCookie = ConfigurationManager.AppSettings["wll_logincookie"];

        /// <summary>
        /// Windows live login module
        /// </summary>
        private WindowsLiveLogin wll = null;

        /// <summary>
        /// Application ID
        /// </summary>
        protected string AppId = null;

        /// <summary>
        /// Share link
        /// </summary>
        protected string ShareLink = "";

        /// <summary>
        /// User ID
        /// </summary>
        protected string UserId;

        /// <summary>
        /// Logged in status flag
        /// </summary>
        protected bool SignedIn = false;

        /// <summary>
        /// Concierge online status flag
        /// </summary>
        protected static bool ConciergeOnline = false;

        /// <summary>
        /// Concierge online status polling timer
        /// </summary>
        protected static System.Timers.Timer GlobalConciergeStatusCheckTimer;

        #endregion

        #region Page Load

        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ConciergeStatusTimer.Tick += new EventHandler<EventArgs>(ConciergeStatusTimer_Tick);

            if (!IsPostBack)
            {

                if (GlobalConciergeStatusCheckTimer == null)
                {
                    GlobalConciergeStatusCheckTimer = new System.Timers.Timer();
                    GlobalConciergeStatusCheckTimer.Interval = 1000 * 5;
                    GlobalConciergeStatusCheckTimer.AutoReset = true;
                    GlobalConciergeStatusCheckTimer.Elapsed += new System.Timers.ElapsedEventHandler(GlobalConciergeStatusCheckTimer_Elapsed);
                    GlobalConciergeStatusCheckTimer.Enabled = true;
                    GlobalConciergeStatusCheckTimer.Start();
                }

                ConciergeStatusTimer.Interval = 1000 * 2;

                IList<Destination> locations = VPLocationManager.GetLocations();

                foreach (Destination location in locations)
                {
                    DestinationSelect.Items.Add(new ListItem(location.Name, location.ID.ToString()));
                }

                /* If the user token has been cached in a site cookie, attempt
                  to process it and extract the user ID. */
                SignInLink.Text = "-Sign In-";
                SignInLink.NavigateUrl = string.Format("http://login.live.com/wlogin.srf?appid={0}&alg={1}", ConfigurationManager.AppSettings["wll_appid"], ConfigurationManager.AppSettings["wll_securityalgorithm"]);

                HttpRequest req = HttpContext.Current.Request;
                HttpCookie loginCookie = req.Cookies[LoginCookie];
                string userName = string.Empty;

                if (loginCookie != null)
                {

                    // Initialize the WindowsLiveLogin module.
                    wll = new WindowsLiveLogin(true);

                    AppId = wll.AppId;
                    string token = loginCookie.Value;

                    if ((token != null) && (token.Length != 0))
                    {
                        WindowsLiveLogin.User user = wll.ProcessToken(token);

                        if (user != null)
                        {

                            //Retrieve user from DB based on Windows Live Encrypted ID
                            VisitPlannerUser visitor = VPUserManager.GetUser(user.Id);

                            //sign out link    
                            SignInLink.Text = "-Sign Out-";
                            SignInLink.NavigateUrl = string.Format("http://login.live.com/logout.srf?appid={0}", ConfigurationManager.AppSettings["wll_appid"]);

                            //TODO: Should get these properties by calling a handler instead with user.Id
                            UserIdArea.Text = string.Format("<input type=\"hidden\" id=\"UserId\" value=\"{0}\" />", visitor.UserNumber);
                            UserTypeArea.Text = string.Format("<input type=\"hidden\" id=\"UserType\" value=\"{0}\" />", AntiXss.HtmlEncode(visitor.UserType));
                            UserFirstNameArea.Text = string.Format("<input type=\"hidden\" id=\"UserFirstName\" value=\"{0}\" />", AntiXss.HtmlEncode(visitor.FirstName));
                            UserLastNameArea.Text = string.Format("<input type=\"hidden\" id=\"UserLastName\" value=\"{0}\" />", AntiXss.HtmlEncode(visitor.LastName));
                            userName = string.Format("{0} {1}", AntiXss.HtmlEncode(visitor.FirstName), AntiXss.HtmlEncode(visitor.LastName));
                            SignedIn = true;
                        }
                    }
                }
                string display = (userName != string.Empty) ? "block" : "none";
                WelcomeMessage.Text = string.Format("<div id=\"welcomeName\" style=\"color:#93958a;font-size:10pt;display:{0};\">Signed in as: <span id='UserName' style='color:#eab649;'>{1}</span></div>", AntiXss.HtmlEncode(display), AntiXss.HtmlEncode(userName));

                if (Session[VisitPlanner.View.SESSION_SHARED_ID] != null)
                {
                    SharedUserIdArea.Text = string.Format("<input type=\"hidden\" id=\"SharedUserId\" value=\"{0}\" />", Session[VisitPlanner.View.SESSION_SHARED_ID]);
                }

            }
        }

        #endregion

        #region Concierge Presence Icon Methods

        /// <summary>
        /// Timer callback while polling for concierge status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GlobalConciergeStatusCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //Presence URL - set the conciergePresenceID to the correct value in the web.config
                string presenceURL = string.Format("http://messenger.services.live.com/users/{0}@apps.messenger.live.com/presenceimage?mkt=en-US", ConfigurationManager.AppSettings["conciergePresenceID"]);
                WebResponse response = WebRequest.Create(new Uri(presenceURL, UriKind.Absolute)).GetResponse();

                ConciergeOnline = !response.ResponseUri.AbsoluteUri.EndsWith("offline.gif", StringComparison.CurrentCultureIgnoreCase);
            }
            catch
            {
                //ignore error
            }
        }

        /// <summary>
        /// Poll for concierge status.  Set hidden flag field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConciergeStatusTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                ConciergeStatusLiteral.Text = string.Format("<input type=\"hidden\" id=\"ConciergeStatusImageSrc\" value=\"{0}\" />", ConciergeOnline.ToString());
                ConciergeStatusTimer.Interval = 1000 * 8;
            }
            catch
            {
                //ignore error
            }
        }

        #endregion

    }
}