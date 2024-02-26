using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Xml;
using System.Web.Script.Serialization;
using System.Web.Services.Protocols;

using WLQuickApps.Tafiti.WebSite;
using LiveSearch;

public partial class Search : System.Web.UI.Page
{
    /// <summary>
    /// Proxy search requests to search.live.com.
    /// 
    /// We pass thru the query string. We append the PathInfo (if any) to http://search.live.com to 
    /// support searching different scopes (e.g., images, news). Here are a few examples:
    ///   /Search.aspx?q=seattle          mimics http://search.live.com/results.aspx?q=seattle
    ///   /Search.aspx/images?q=seattle   mimics http://search.live.com/images/results.aspx?q=seattle
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Page_Load(object sender, EventArgs e)
    {
        Utility.VerifyIsSelfRequest();

        string scope = Request.PathInfo.Remove(0, 1); // remove beginning '/'
        if (UseSoapService(scope))
        {
            string query = Request.QueryString["q"];
            if (scope.ToLower() == "feeds")
            {
                query += " feed:";
            }

            SourceType sourceType = GetSourceType(scope);
            int offset = int.Parse(Request.QueryString["first"]);
            int count = int.Parse(Request.QueryString["count"]);
            SoapSearch(sourceType, query, offset, count, scope);
        }
        else
        {
            throw new HttpException((int)HttpStatusCode.BadRequest, "Bad Request");
        }
    }
    
    private bool UseSoapService(string scope)
    {
        switch (scope.ToLower())
        {
            case "web":
            case "images":
            case "news":
            case "phonebook":
            case "feeds":
                return true;

            default: return false;
        }
    }

    private SourceType GetSourceType(string scope)
    {
        switch (scope.ToLower())
        {
            case "web": return SourceType.Web;
            case "images": return SourceType.Image;
            case "news": return SourceType.News;
            case "phonebook": return SourceType.PhoneBook;
            case "feeds": return SourceType.Web;
            default: throw new ArgumentException("unsupported search scope");
        }
    }

    // For compatibility with LiveSearch's XML-format responses
    private string GetDocumentSetSource(SourceType sourceType)
    {
        switch (sourceType)
        {
            case SourceType.Web:
            case SourceType.Image:
            case SourceType.PhoneBook:
            case SourceType.InlineAnswers:
                return "FEDERATOR_MONARCH";
            case SourceType.News:
                return "FEDERATOR_BACKFILL_NEWS";
            default:
                throw new ArgumentException("unsupported source type");
        }
    }

    private ResultFieldMask GetResultFieldMask(SourceType sourceType)
    {
        switch (sourceType)
        {
            case SourceType.Web:
                return ResultFieldMask.Title | ResultFieldMask.Description | ResultFieldMask.Url;
            case SourceType.Image:
                return ResultFieldMask.Image | ResultFieldMask.Url;
            case SourceType.News:
                return ResultFieldMask.Title | ResultFieldMask.Description | ResultFieldMask.Url | ResultFieldMask.Source;
            case SourceType.PhoneBook:
                return ResultFieldMask.Title | ResultFieldMask.Description | ResultFieldMask.Url | ResultFieldMask.Location;
            default:
                throw new ArgumentException("unsupported source type");
        }
    }

    private void SoapSearch(SourceType sourceType, string query, int first, int count, string domain)
    {
        try
        {
            SourceRequest[] sr = new SourceRequest[1];
            sr[0] = new SourceRequest();
            sr[0].Source = sourceType;
            sr[0].Offset = first;
            sr[0].Count = count;
            sr[0].ResultFields = GetResultFieldMask(sourceType);

            SearchRequest request = new SearchRequest();
            request.Query = query;
            request.Requests = sr;
            request.SafeSearch = SafeSearchOptions.Strict;
            request.AppID = SettingsWrapper.LiveSearchAppID;
            request.CultureInfo = "en-US";

            if (sourceType == SourceType.PhoneBook)
            {
                // Using Redmond as the search location.
                request.Location = new Location();
                request.Location.Longitude = Double.Parse("-122.33482360839846");
                request.Location.Latitude = Double.Parse("47.6082462871061");
                request.Location.Radius = Double.Parse("5");
            }

            MSNSearchService searchService = new MSNSearchService();
            SearchResponse searchResponse;
            searchResponse = searchService.Search(request);

            LiveXmlSearchResults result = CreateXmlSearchResults(sourceType, searchResponse.Responses[0], domain);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(result);
            byte[] jsonUtf8 = System.Text.Encoding.UTF8.GetBytes(json);

            Response.StatusCode = 200;
            Response.ContentType = "text/javascript";
            Response.OutputStream.Write(jsonUtf8, 0, jsonUtf8.Length);
        }
        catch (SoapException e)
        {
            throw new HttpException((int)HttpStatusCode.InternalServerError, "Internal Server Error", e);
        }
        catch (WebException e)
        {
            throw new HttpException((int)HttpStatusCode.InternalServerError, "Internal Server Error", e);
        }
    }

    private LiveXmlSearchResults CreateXmlSearchResults(SourceType sourceType, SourceResponse sourceResponse, string domain)
    {
        LiveXmlSearchResults searchResults = new LiveXmlSearchResults();
        searchResults.searchresult.documentset._source = GetDocumentSetSource(sourceType);
        searchResults.searchresult.documentset._count = sourceResponse.Results.Length.ToString();
        searchResults.searchresult.documentset._start = sourceResponse.Offset.ToString();
        searchResults.searchresult.documentset._total = sourceResponse.Total.ToString();

        if (sourceResponse.Results.Length > 0)
        {
            LiveXmlResult[] results;
            switch (sourceType)
            {
                case SourceType.Web:
                    results = ConvertWebResults(sourceResponse, domain);
                    break;

                case SourceType.Image:
                    results = ConvertImageResults(sourceResponse, domain);
                    break;

                case SourceType.News:
                    results = ConvertNewsResults(sourceResponse, domain);
                    break;

                case SourceType.PhoneBook:
                    results = ConvertPhoneBookResults(sourceResponse, domain);
                    break;

                default:
                    results = null;
                    break;
            }

            if (results != null)
                searchResults.searchresult.documentset.document = (results.Length > 1) ? (object)results : (object)results[0];
        }

        return searchResults;
    }

    private static LiveXmlResult[] ConvertPhoneBookResults(SourceResponse sourceResponse, string domain)
    {
        return Search.ConvertWebResults(sourceResponse, domain);
    }

    private static LiveXmlResult[] ConvertNewsResults(SourceResponse sourceResponse, string domain)
    {
        LiveXmlResult[] results = new LiveXmlResult[sourceResponse.Results.Length];
        for (int i = 0; i < sourceResponse.Results.Length; i++)
        {
            Result sourceResult = sourceResponse.Results[i];
            LiveXmlResult result = new LiveXmlResult();
            result.domain = domain;
            result.title = sourceResult.Title;
            result.source = sourceResult.Source;
            result.description = sourceResult.Description;
            result.url = sourceResult.Url;
            results[i] = result;
        }
        return results;
    }

    private static LiveXmlResult[] ConvertImageResults(SourceResponse sourceResponse, string domain)
    {
        LiveXmlResult[] results = new LiveXmlResult[sourceResponse.Results.Length];
        for (int i = 0; i < sourceResponse.Results.Length; i++)
        {
            Result sourceResult = sourceResponse.Results[i];
            LiveXmlResult result = new LiveXmlResult();
            if (sourceResult.Image != null)
            {
                result.domain = domain;
                result.width = sourceResult.Image.ThumbnailWidthSpecified ? sourceResult.Image.ThumbnailWidth.ToString() : "100"; // make up a value!
                result.height = sourceResult.Image.ThumbnailHeightSpecified ? sourceResult.Image.ThumbnailHeight.ToString() : "100";
                result.url = sourceResult.Image.ThumbnailURL;
                result.imageUrl = sourceResult.Image.ThumbnailURL;
            }
            result.url = sourceResult.Url;
            results[i] = result;
        }
        return results;
    }

    private static LiveXmlResult[] ConvertWebResults(SourceResponse sourceResponse, string domain)
    {
        LiveXmlResult[] results = new LiveXmlResult[sourceResponse.Results.Length];
        for (int i = 0; i < sourceResponse.Results.Length; i++)
        {
            Result sourceResult = sourceResponse.Results[i];
            LiveXmlResult result = new LiveXmlResult();
            result.domain = domain;
            result.title = sourceResult.Title;
            result.description = sourceResult.Description;
            result.url = sourceResult.Url;
            results[i] = result;
        }
        return results;
    }
   
    class LiveXmlSearchResults
    {
        public LiveXmlSearchResult searchresult = new LiveXmlSearchResult();
    }

    class LiveXmlSearchResult
    {
        public LiveXmlDocumentSet documentset = new LiveXmlDocumentSet();
    }

    class LiveXmlDocumentSet
    {
        public string _source;
        public string _start;
        public string _total;
        public string _count;
        public object document; // if _count is 1, this is an instance of LiveXml...Result, otherwise it is an array
    }

    class LiveXmlResult
    {
        public string domain;
        public string title;
        public string description;
        public string source;
        public string imageUrl;
        public string height;
        public string width;
        public string url;
    }

}
