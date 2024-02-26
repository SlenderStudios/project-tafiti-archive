using System;
using System.Data;
using System.Configuration;
using System.Net;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

using LiveSearch;

public class SearchProviderSharePoint : SearchProvider
{
    public string QueryText { get; set; }
    private string _queryPacket = string.Empty;

    private WindowsImpersonationContext _impersonationContext = null;

    public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
    {
        // left out 
        if (config == null)
            throw new ArgumentNullException("config");

        if (String.IsNullOrEmpty(name))
            name = "SearchProviderSharePoint";

        if (string.IsNullOrEmpty(config["description"]))
        {
            config.Remove("description");
            config.Add("description", "Tafiti SharePoint Search Provider");
        }

        base.Initialize(name, config);
    }

    public SearchProviderSharePoint() {}

    public SearchProviderSharePoint(string queryText)
    {
        QueryText = queryText;

        _queryPacket = String.Concat(
        "<QueryPacket xmlns='urn:Microsoft.Search.Query'>",
            "<Query>",
                "<SupportedFormats>",
                    "<Format revision='1'>urn:Microsoft.Search.Response.Document:Document</Format>",
                "</SupportedFormats>",
                "<Context>",
                    "<QueryText language='en-US' type='STRING'>",
                        QueryText,
                    "</QueryText>",
                "</Context>",
                "<Range>",
                    "<StartAt>1</StartAt>",
                    "<Count>100</Count>",
                "</Range>",
            "</Query>",
        "</QueryPacket>");

        if (IsSearchServiceAvailable() == false)
            throw new Exception("SharePoint Search Service is unavailable");
    }

    public override LiveXmlSearchResults ExecuteSearch()
    {
        _impersonationContext = ((WindowsIdentity)HttpContext.Current.User.Identity).Impersonate();
        SPSearch.QueryService queryService = new SPSearch.QueryService();
        queryService.Credentials = CredentialCache.DefaultNetworkCredentials;
        DataSet queryResults = queryService.QueryEx(_queryPacket);
        _impersonationContext.Undo();

        return CreateLiveXmlSearchResults(queryResults);
    }

    private LiveXmlSearchResults CreateLiveXmlSearchResults(DataSet queryResults)
    {
        DataTable relevantResults = 
            queryResults.Tables["RelevantResults"];

        LiveXmlSearchResults returnResults = new LiveXmlSearchResults();
        returnResults.searchresult.documentset._source = "FEDERATOR_MONARCH";
        returnResults.searchresult.documentset._count = relevantResults.Rows.Count.ToString();
        returnResults.searchresult.documentset._start = "0";
        returnResults.searchresult.documentset._total = relevantResults.Rows.Count.ToString();

        LiveXmlResult[] results = new LiveXmlResult[relevantResults.Rows.Count];
        for (int i = 0; i < relevantResults.Rows.Count; i++)
        {
            LiveXmlResult result = new LiveXmlResult();
            result.domain = "web";
            result.title = relevantResults.Rows[i]["Title"].ToString();
            result.description =
                GetDescription(relevantResults.Rows[i]["Description"].ToString(),
                relevantResults.Rows[i]["HitHighlightedSummary"].ToString());
            result.url = relevantResults.Rows[i]["Path"].ToString();
            results[i] = result;
        }

        if (results != null)
            returnResults.searchresult.documentset.document =
                (results.Length > 1) ? (object)results : (object)results[0];
        
        return returnResults;  
    }

    private static string GetDescription(string description, string hitHighlightedSummary)
    {
        string strippedText = string.Empty;

        if (string.IsNullOrEmpty(description))
        {
            Regex regEx = new Regex("<[^>]*>", RegexOptions.IgnoreCase);
            strippedText = regEx.Replace(hitHighlightedSummary, "...");
            
            return strippedText;
        }
        else
            return description;
    }

    private bool IsSearchServiceAvailable()
    {
        try
        {
            _impersonationContext = ((WindowsIdentity)HttpContext.Current.User.Identity).Impersonate();
            SPSearch.QueryService queryService = new SPSearch.QueryService();
            queryService.Credentials = CredentialCache.DefaultNetworkCredentials;
            string serviceStatus = queryService.Status();
            _impersonationContext.Undo();

            return serviceStatus.ToUpper() == "ONLINE" ? true : false;
        }
        catch (Exception e)
        {
            throw new Exception("SearchProviderSharePoint.IsSearchServiceAvailable --> " + e.Message);
        }
    }
}