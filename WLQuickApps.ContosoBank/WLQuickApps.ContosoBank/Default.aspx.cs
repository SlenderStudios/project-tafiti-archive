/*****************************************************************************
 * Default.ashx
 * Notes: Home page.
 * **************************************************************************/

using System;
using System.Configuration;
using System.Web.UI.WebControls;

using WLQuickApps.ContosoBank.Logic;

namespace WLQuickApps.ContosoBank
{
    public partial class Default : BasePage
    {
        #region Fields

        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }
        public int UserZoomLevel { get; set; }

        private const int ZOOMLEVEL_AUTHENTICATED = 11;
        private const int ZOOMLEVEL_UNAUTHENTICATED = 4;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            // If the user is logged in - zoom map to their location,
            // otherwise show all Australian members
            UserProfile currentUser = UserLogic.GetCurrentUser();
            if (currentUser != null)
            {
                UserLatitude = currentUser.Latitude;
                UserLongitude = currentUser.Longitude;
                UserZoomLevel = ZOOMLEVEL_AUTHENTICATED;
            }
            else
            {
                UserLatitude = Convert.ToDouble(ConfigurationManager.AppSettings["default_user_lat"]);
                UserLongitude = Convert.ToDouble(ConfigurationManager.AppSettings["default_user_long"]);
                UserZoomLevel = ZOOMLEVEL_UNAUTHENTICATED;
            }
        }

        protected void LattestPostGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((HyperLink)e.Row.Cells[1].Controls[1]).NavigateUrl = "ForumThread.aspx?ID=" +
                                                                      ((ForumSubject)e.Row.DataItem).ForumSubjectID;
            }
        }
    }
}