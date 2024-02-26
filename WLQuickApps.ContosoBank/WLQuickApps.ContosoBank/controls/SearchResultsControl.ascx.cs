/*****************************************************************************
 * SearchResultsControl.ascx
 * Notes: Uses the Live Search API to perform a phonebook/business search
 * **************************************************************************/
using System;
using System.Configuration;
using System.Web.UI;
using WLQuickApps.ContosoBank.LiveSearchAPI;

namespace WLQuickApps.ContosoBank.controls
{
    public partial class SearchResultsControl : UserControl
    {
        private const int RESULTS_PER_PAGE = 25; // Max of 25 / page for a phone book search

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        public void BindData(string searchText, int pageNumber)
        {
            Session["SearchPage"] = pageNumber;
            BusinessSearchTextBox.Text = searchText;
            SearchResponse response = performSearch(searchText, pageNumber);
            SearchResultsRepeater.DataSource = response.Responses[0].Results;
            SearchResultsRepeater.DataBind();

            toppreviousLink.Enabled = previousLink.Enabled = pageNumber > 0;
            topnextLink.Enabled = nextLink.Enabled = ((pageNumber + 1)*RESULTS_PER_PAGE) <= response.Responses[0].Total;
        }


        private static SearchResponse performSearch(string searchText, int pageNumber)
        {
            SourceRequest[] source = new SourceRequest[1];
            source[0] = new SourceRequest();

            // NOTE: ATM Phonebook search is only in the US
            source[0].Source = SourceType.PhoneBook;
            source[0].ResultFields = ResultFieldMask.Description | 
                                     ResultFieldMask.Location | 
                                     ResultFieldMask.Title |
                                     ResultFieldMask.Url;
            source[0].Offset = (pageNumber)*RESULTS_PER_PAGE; // record to start from (for paging)
            source[0].Count = RESULTS_PER_PAGE; // number of records to retrieve
            source[0].SortBy = SortByType.Relevance;

            SearchRequest request = new SearchRequest();
            request.Query = searchText;
            // web.config values can be change to search in a different location and radius.
            request.Location = new Location
                                   {
                                       Latitude = Convert.ToDouble(ConfigurationManager.AppSettings["wlSearch_lat"]),
                                       Longitude = Convert.ToDouble(ConfigurationManager.AppSettings["wlSearch_long"]),
                                       Radius = Convert.ToDouble(ConfigurationManager.AppSettings["wlSearch_rad"])
                                   };
            request.Flags = SearchFlags.None;
            request.Requests = source;
            request.AppID = ConfigurationManager.AppSettings["wlSearch_appid"];
            request.CultureInfo = "en-AU";

            return new MSNSearchService().Search(request);
        }

        protected void previousLink_Click(object sender, EventArgs e)
        {
            int pageNumber = Convert.ToInt32(Session["SearchPage"]);
            pageNumber--;
            BindData(Session["SearchString"].ToString(), pageNumber);
        }

        protected void nextLink_Click(object sender, EventArgs e)
        {
            int pageNumber = Convert.ToInt32(Session["SearchPage"]);
            pageNumber++;
            BindData(Session["SearchString"].ToString(), pageNumber);
        }

        protected void SearchButton_Click(object sender, EventArgs e)
        {
            Session["SearchString"] = BusinessSearchTextBox.Text;
            BindData(Session["SearchString"].ToString(), 0);
        }
    }
}