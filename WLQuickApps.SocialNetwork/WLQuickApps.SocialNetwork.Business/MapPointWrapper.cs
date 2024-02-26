using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

using WLQuickApps.SocialNetwork.Business.MapPointService;

namespace WLQuickApps.SocialNetwork.Business
{
    static public class MapPointWrapper
    {
        #region Properties

        
        static public string UserName
        {
            get { return SettingsWrapper.MapPointUserName; }
        }
        static private string _userName = String.Empty;

        static public string Password
        {
            get { return SettingsWrapper.MapPointPassword; }
        }
        static private string _password = String.Empty;

        /*
        static public string DataSourceName
        {
            get { return SettingsWrapper.MapPointDataSource; }
        }*/

        static public string DataSourceName
        {
            get
            {
                string[] APCountries = { "New Zealand", "Australia", "Taiwan", "Singapore", "Hong Kong", "Malaysia" };
                string[] NACountries = { "USA", "Canada", "Mexico", "Puerto Rico" };
                string[] BRCountries = { "Brazil" };
                string[] EUCountries = { "Austria", "Belgium", "Denmark", "Finland", "France", "Germany", "Italy", "Luxembourg", "The Netherlands", "Norway", "Portugal", "Spain", "Sweden", "Switzerland", "Greece", "United Kingdom", "Andorra", "Czech Republic", "Ireland", "Liechtenstein", "Monaco", "San Marino", "Slovakia", "Vatican City" };

                System.Collections.Generic.List<string> APCountriesList = new List<string>();
                APCountriesList.AddRange(APCountries);
                System.Collections.Generic.List<string> NACountriesList = new List<string>();
                NACountriesList.AddRange(NACountries);
                System.Collections.Generic.List<string> BRCountriesList = new List<string>();
                BRCountriesList.AddRange(BRCountries);
                System.Collections.Generic.List<string> EUCountriesList = new List<string>();
                EUCountriesList.AddRange(EUCountries);

                string MapPoint;
                if(APCountriesList.Contains(MapPointWrapper._DataSourceName))
                    MapPoint = "MapPoint.AP";
                else if (NACountriesList.Contains(MapPointWrapper._DataSourceName))
                    MapPoint = "MapPoint.NA";
                else if (BRCountriesList.Contains(MapPointWrapper._DataSourceName))
                    MapPoint = "MapPoint.BR";
                else if (EUCountriesList.Contains(MapPointWrapper._DataSourceName))
                    MapPoint = "MapPoint.EU";
                else
                    MapPoint = "MapPoint.World";

                return MapPoint;
            }
            set { MapPointWrapper._DataSourceName = value; }
        }
        static public string _DataSourceName = string.Empty;


        static public string IconDataSourceName
        {
            get { return MapPointWrapper._iconDataSourceName; }
            set { MapPointWrapper._iconDataSourceName = value; }
        }
        static private string _iconDataSourceName = "MapPoint.Icons";


        #endregion

        static public Address CreateAddress(string addressLine, string city, string state, string postalCode, string country)
        {
            Address address = new Address();
            address.AddressLine = addressLine;
            address.CountryRegion = country;
            address.PostalCode = postalCode;
            address.PrimaryCity = city;
            address.Subdivision = state;
            return address;
        }

        static public Address CreateAddress(string formattedAddress)
        {
            Address address = new Address();
            address.FormattedAddress = formattedAddress;
            return address;
        }

        static public LatLong CreateLatLong(double latitude, double longitude)
        {
            LatLong latLong = new LatLong();
            latLong.Latitude = latitude;
            latLong.Longitude = longitude;
            return latLong;
        }

        static public FindSpecification CreateFindSpecificationWrapper(string dataSourceName, string inputPlace)
        {
            FindSpecification findSpecification = new FindSpecification();
            findSpecification.DataSourceName = dataSourceName;
            findSpecification.InputPlace = inputPlace;
            return findSpecification;
        }

        static public FindSpecification CreateFindSpecificationWrapper(string inputPlace)
        {
            return MapPointWrapper.CreateFindSpecificationWrapper(MapPointWrapper.DataSourceName, inputPlace);
        }

        static public FindAddressSpecification CreateFindAddressSpecification(string dataSourceName, Address inputAddress, FindOptions options)
        {

            FindAddressSpecification findAddressSpecification = new FindAddressSpecification();

            findAddressSpecification.DataSourceName = dataSourceName;

            findAddressSpecification.InputAddress = inputAddress;

            findAddressSpecification.Options = options;

            return findAddressSpecification;
        }

        static public FindAddressSpecification CreateFindAddressSpecification(string dataSourceName, Address inputAddress)
        {
            return MapPointWrapper.CreateFindAddressSpecification(dataSourceName, inputAddress, null);
        }

        static public FindAddressSpecification CreateFindAddressSpecification(Address inputAddress)
        {
            return MapPointWrapper.CreateFindAddressSpecification(MapPointWrapper.DataSourceName, inputAddress);
        }

        #region Web Service objects
        static public FindServiceSoap FindService
        {
            get
            {
                if (MapPointWrapper._findServiceSoap == null)
                {
                    MapPointWrapper._findServiceSoap = new FindServiceSoap();
                    MapPointWrapper._findServiceSoap.Credentials = new NetworkCredential(MapPointWrapper.UserName, MapPointWrapper.Password);
                }

                return MapPointWrapper._findServiceSoap;
            }
        }
        static private FindServiceSoap _findServiceSoap;

        #endregion

        static public LatLong GetLatLongByPlace(string place)
        {
            FindResults findResults = MapPointWrapper.FindService.Find(MapPointWrapper.CreateFindSpecificationWrapper(place));

            if (findResults.Results.Length == 0)
            {
                return MapPointWrapper.CreateLatLong(0, 0);
            }

            return findResults.Results[0].FoundLocation.LatLong;
        }

        static public LatLong GetLatLong(string formattedAddress)
        {
            try
            {
                LatLong latLong = MapPointWrapper.GetLatLongByPlace(formattedAddress);
                if ((latLong.Latitude == 0) && (latLong.Longitude == 0))
                {
                    latLong = MapPointWrapper.GetLatLongByAddress(formattedAddress);
                }
                return latLong;
            }
            catch (Exception e)
            {
                HealthMonitoringManager.LogWarning(e, "Unable to find address: \"{0}\"", formattedAddress);
                return MapPointWrapper.CreateLatLong(0, 0);
            }
        }

        static public LatLong GetLatLongByAddress(string formattedAddress)
        {
            return MapPointWrapper.GetLatLongByAddress(MapPointWrapper.CreateAddress(formattedAddress));
        }

        /// <summary>
        /// Get the LatLong from an Address (parameterized)
        /// </summary>
        /// <param name="addressLine"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        static public LatLong GetLatLongByAddress(string addressLine, string city, string state, string postalCode, string country)
        {
            return MapPointWrapper.GetLatLongByAddress(MapPointWrapper.CreateAddress(addressLine, city, state, postalCode, country));
        }

        /// <summary>
        /// Executes the web service call after packaging a request.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        static public LatLong GetLatLongByAddress(Address address)
        {
            FindAddressSpecification findAddressSpecification = MapPointWrapper.CreateFindAddressSpecification(address);

            // Execute the webservice call.
            FindResults findResults = MapPointWrapper.FindService.FindAddress(findAddressSpecification);

            if (findResults.Results.Length == 0)
            {
                return MapPointWrapper.CreateLatLong(0, 0);
            }

            // Return the LatLong
            return findResults.Results[0].FoundLocation.LatLong;
        }
    }
}
