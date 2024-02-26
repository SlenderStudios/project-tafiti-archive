/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: PrintAttraction.aspx.cs
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
using Microsoft.Security.Application;
namespace VisitPlanner
{
    /// <summary>
    /// Print attraction page.  
    /// </summary>
    public partial class PrintAttraction : System.Web.UI.Page
    {

        #region Private Properties

        /// <summary>
        /// A serialized Attraction object
        /// </summary>
        private const string ATTRACTION_SERIAL_TEXT = "ast";

        /// <summary>
        /// The name of the Destination associated with the Attraction
        /// </summary>
        private const string DESTINATION_NAME = "dn";

        #endregion

        #region Page Load
        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            NameValueCollection parms = Page.Request.Params;

            if (parms[ATTRACTION_SERIAL_TEXT] != null)
            {
                VESilverlight.Attraction attraction = VESilverlight.Attraction.Deserialize(parms[ATTRACTION_SERIAL_TEXT]);
                Page.Title = AntiXss.HtmlEncode(attraction.Title);
                TitleLabel.Text = AntiXss.HtmlEncode(attraction.Title);
                Address1.Text = AntiXss.HtmlEncode(attraction.AddressLine1);
                Address2.Text = AntiXss.HtmlEncode(attraction.AddressLine2);
                DescriptionLabel.Text = AntiXss.HtmlEncode(attraction.LongDescription);
                AttractionImage.ImageUrl = AntiXss.UrlEncode(attraction.ImageURL);
                VisitPlannerLogo.ImageUrl = AntiXss.UrlEncode(ConfigurationManager.AppSettings["VisitPlannerLogo"]);
            }
        }

        #endregion
    }
}