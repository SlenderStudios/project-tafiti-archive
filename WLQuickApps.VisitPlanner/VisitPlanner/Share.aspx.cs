/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: Share.aspx.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Collections.Specialized;

namespace VisitPlanner
{
    /// <summary>
    /// Page used to display shared locations
    /// </summary>
    public partial class Share : System.Web.UI.Page
    {
        #region Private Properties
        /// <summary>
        /// Share link 
        /// </summary>
        protected string ShareLink;

        #endregion

        #region Page Load
        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ShareLink = Page.Request.Url.AbsoluteUri.Replace("Share.aspx", "View.aspx");
            VisitPlannerLogo.ImageUrl = ConfigurationManager.AppSettings["VisitPlannerLogo"];
        }

        #endregion
    }
}
