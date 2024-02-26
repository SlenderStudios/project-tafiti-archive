using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

/// <summary>
/// Provides access to the Windows Live Expo Service
/// </summary>
public class ExpoService
{
    private string appKey;
    private string siteId;

    public ExpoService(string appKey, string siteId)
    {
        this.appKey = appKey;
        this.siteId = siteId;
    }

    /// <summary>
    /// Get the listings from the Expo API.
    /// </summary>
    /// <param name="cat">Category</param>
    /// <param name="transactionType">Transaction Type</param>
    /// <param name="keyword">Keyword</param>
    /// <returns></returns>
    public XmlDocument GetListings(string cat, string transactionType, string keyword)
    {
        try
        {
            int page = 1;
            int pageSize = 10;
            string orderBy = "";
            bool orderAscending = true;
            
            //Get the Configuration Search vaues from web.config file
            string city = ConfigurationManager.AppSettings["ExpoCity"];
            string state = ConfigurationManager.AppSettings["ExpoState"];
            string postalCode = ConfigurationManager.AppSettings["ExpoPostalCode"];
            string country = ConfigurationManager.AppSettings["ExpoCountry"];
            string maxDist = ConfigurationManager.AppSettings["ExpoMaxDistance"]; // in meters - this is 50 miles

            // Define the URI for the Expo Service.
            string request = string.Format("http://expo.live.com/API/Classifieds_ListingsByCategoryKeywordLocation_V2.ashx?appKey={0}&page={1}&pagesize={2}&orderBy={3}&orderAscending={4}&keyword={5}&cat={6}&city={7}&state={8}&postalCode={9}&country={10}&maxDist={11}&siteId={12}&transactionType={13}", appKey, page, pageSize, orderBy, orderAscending, keyword, cat, city, state, postalCode, country, maxDist, siteId, transactionType);

            // Create the XML Document to store the output.
            XmlDocument xmlDocument = new XmlDocument();

            // Populate teh XML Reader from the URI (direct HTTP request)
            XmlReader xmlReader = XmlReader.Create(request);

            // Load the XML Document from the Reader
            xmlDocument.Load(xmlReader);
            
            return xmlDocument;
        }
        catch(Exception ex)
        {
            
            XmlDocument xmlDocument = new XmlDocument();
            
            xmlDocument.LoadXml("<error>" + ex.Message + "|" + ex.StackTrace + "</error>");
            
            return(xmlDocument);
        }
    }
}
