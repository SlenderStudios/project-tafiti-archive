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
using Microsoft.Security.Application; // NOTE: If you get a build error here you require the Anti XSS library (see the pre-requisites)

namespace VisitPlanner
{
    /// <summary>
    /// Driving Directions page.  Location info passed through query string.
    /// </summary>
    public partial class DrivingDirections : System.Web.UI.Page
    {
        #region Private Properties

        /// <summary>
        /// Start title query param
        /// </summary>
        private const string START_TITLE_PARAM = "st";
        
        /// <summary>
        /// Start address query param
        /// </summary>
        private const string START_ADDRESS_PARAM = "sa";

        /// <summary>
        /// Start latitude query param 
        /// </summary>
        private const string START_LAT_PARAM = "slat";

        /// <summary>
        /// Start longitude query param
        /// </summary>
        private const string START_LONG_PARAM = "slon";

        /// <summary>
        /// End title query param
        /// </summary>
        private const string END_TITLE_PARAM = "et";

        /// <summary>
        /// End address query param
        /// </summary>
        private const string END_ADDRESS_PARAM = "ea";

        /// <summary>
        /// End latitude query param
        /// </summary>
        private const string END_LAT_PARAM = "elat";

        /// <summary>
        /// End longitude query param
        /// </summary>
        private const string END_LONG_PARAM = "elon";
        #endregion

        #region Protected Methods
        /// <summary>
        /// Page Load method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //get parameters from query string
            string StartTitle = Request.Params[START_TITLE_PARAM];
            string StartAddress = Request.Params[START_ADDRESS_PARAM];
            string StartLatitude = Request.Params[START_LAT_PARAM];
            string StartLongitude = Request.Params[START_LONG_PARAM];
            string EndTitle = Request.Params[END_TITLE_PARAM];
            string EndAddress = Request.Params[END_ADDRESS_PARAM];
            string EndLatitude = Request.Params[END_LAT_PARAM];
            string EndLongitude = Request.Params[END_LONG_PARAM];

            //get start address
            string[] StartAddressArray = StartAddress.Split(',');
            if (StartAddressArray.Length == 3)
            {
                string startStreet = StartAddressArray[0];
                string startCity = StartAddressArray[1];
                string startZip = StartAddressArray[2];
                StartAddress = startStreet + "<br />" + startCity + "," + startZip;
            }

            //get end address
            string[] EndAddressArray = EndAddress.Split(',');
            if (EndAddressArray.Length == 3)
            {
                string endStreet = EndAddressArray[0];
                string endCity = EndAddressArray[1];
                string endZip = EndAddressArray[2];
                EndAddress = endStreet + "<br />" + endCity + "," + endZip;
            }

            //set address fields
            AddressFrom.Text = string.Format("{0}<br />{1}", AntiXss.HtmlEncode(StartTitle), AntiXss.HtmlEncode(StartAddress));
            AddressTo.Text = string.Format("{0}<br />{1}", AntiXss.HtmlEncode(EndTitle), AntiXss.HtmlEncode(EndAddress));
            
            //add JS call to get route
            string onLoadCall = string.Format("GetRouteMap(new VELatLong({0}, {1}),new VELatLong({2}, {3}));",StartLatitude,StartLongitude,EndLatitude,EndLongitude);
            body.Attributes.Add("onLoad", onLoadCall);
            VisitPlannerLogo.ImageUrl = ConfigurationManager.AppSettings["VisitPlannerLogo"];



        }
        #endregion
    }
}
