/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: View.aspx.cs
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
    /// Redirect users to Default page for shared view
    /// </summary>
    public partial class View : System.Web.UI.Page
    {
        #region Public Properties
        /// <summary>
        /// User ID query param
        /// </summary>
        public const string USER_ID = "uid";

        /// <summary>
        /// Shared user ID  query param
        /// </summary>
        public const string SESSION_SHARED_ID = "SharedUserID";

        #endregion

        #region Page Load
        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection parms = Request.Params;

            if (parms[USER_ID] != null)
            {
                Session.Add(SESSION_SHARED_ID, parms[USER_ID]);
            }

            Response.Redirect("Default.aspx");
        }

        #endregion

    }
}
