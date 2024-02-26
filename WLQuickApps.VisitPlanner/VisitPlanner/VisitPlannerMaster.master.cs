/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: VisitPlannerMaster.master.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
namespace VisitPlanner
{
    /// <summary>
    /// Master page layout
    /// </summary>
    public partial class VisitPlannerMaster : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Update logo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                VisitPlannerLogo.ImageUrl = ConfigurationManager.AppSettings["VisitPlannerLogo"];
            }
        }
    }
}
