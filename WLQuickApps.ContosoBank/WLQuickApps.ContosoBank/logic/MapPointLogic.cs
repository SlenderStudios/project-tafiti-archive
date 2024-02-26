using System;
using System.Configuration;
using System.Net;

using WLQuickApps.ContosoBank.MapPointWebService;

namespace WLQuickApps.ContosoBank.Logic
{
    public class MapPointLogic
    {
        #region Fields
        private FindServiceSoap findService;

        #endregion

        public void GeocodePostCode(string postcode, out double latitude, out double longitude)
        {
            try
            {
                initMapPoint();

                // Set the Address to search for.  Geocode at postcode level within Australia
                // using the AsiaPacific MapPoint datasource.  
                FindAddressSpecification findAddressSpec =
                    new FindAddressSpecification
                        {
                            InputAddress = new Address {PostalCode = postcode, CountryRegion = "AUS"},
                            Options = new FindOptions(),
                            DataSourceName = "MapPoint.AP"
                        };

                //Set the ResultMask to include the rooftop geocoding values
                findAddressSpec.Options.ResultMask =
                    FindResultMask.RooftopFlag |
                    FindResultMask.LatLongFlag |
                    FindResultMask.EntityFlag |
                    FindResultMask.MatchDetailsFlag |
                    FindResultMask.AddressFlag;

                //Call the MWS FindAddress method
                FindResults myFindResults = findService.FindAddress(findAddressSpec);

                //Parse the Results
                FindResult bestResult = myFindResults.Results[0];

                latitude = bestResult.BestViewableLocation.LatLong.Latitude;
                longitude = bestResult.BestViewableLocation.LatLong.Longitude;
            }
            catch (Exception)
            {
                // swallow any exceptions thrown by mappoint web service or results
                // set the lat/long to default Australia position
                longitude = Convert.ToDouble(ConfigurationManager.AppSettings["default_user_long"]);
                latitude = Convert.ToDouble(ConfigurationManager.AppSettings["default_user_lat"]);
            }
        }

        private void initMapPoint()
        {
            // initialises the mappoint web service with your mappoint username and password
            findService = new FindServiceSoap
                              {
                                  Credentials = new NetworkCredential(ConfigurationManager.AppSettings["mp_username"],
                                                                      ConfigurationManager.AppSettings["mp_password"]),
                                  PreAuthenticate = true
                              };
        }
    }
}