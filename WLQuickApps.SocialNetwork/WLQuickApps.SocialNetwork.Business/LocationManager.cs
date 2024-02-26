using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Security;

using WLQuickApps.SocialNetwork.Business.MapPointService;
using WLQuickApps.SocialNetwork.Data;
using WLQuickApps.SocialNetwork.Data.LocationDataSetTableAdapters;

namespace WLQuickApps.SocialNetwork.Business
{
    public static class LocationManager
    {
        static LocationManager()
        {
        }

        static public Location CreateLocation(string name, string address1, string address2, string city, string region, string country, string postalCode)
        {
            return LocationManager.CreateLocation(name, address1, address2, city, region, country, postalCode, string.Empty);
        }

        static public Location CreateLocation(string name, string address1, string address2, string city, string region, string country, string postalCode, string type)
        {
            if (name == null) { name = string.Empty; }
            if (address1 == null) { address1 = string.Empty; }
            if (address2 == null) { address2 = string.Empty; }
            if (city == null) { city = string.Empty; }
            if (region == null) { region = string.Empty; }
            if (country == null) { country = string.Empty; }
            if (postalCode == null) { postalCode = string.Empty; }

            if ((name + address1 + address2 + city + region + country + postalCode).Length == 0)
            {
                return Location.Empty;
            }

            if (type == null) { type = string.Empty; }

            if ((type.Length > 0) && (!UserManager.LoggedInUser.IsAdmin))
            {
                throw new SecurityException("Only administrators may create typed locations.");
            }

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, Utilities.ReadUncommittedTransaction))
            using (LocationTableAdapter locationTableAdapter = new LocationTableAdapter())
            {
                LocationDataSet.LocationDataTable locationDataTable = locationTableAdapter.LookupLocation(name, address1, address2, city, region, country, postalCode, type);

                if (locationDataTable.Rows.Count > 0)
                {
                    return LocationManager.GetLocation(locationDataTable[0].LocationID);
                }

                LatLong latLong = MapPointWrapper.GetLatLong(Location.GetAddressText(name, address1, address2, city, region, country, postalCode));

                Guid locationID = new Guid(locationTableAdapter.CreateLocation(name, address1, address2, city, region, country,
                    postalCode, latLong.Latitude, latLong.Longitude, type).ToString());

                Location location = LocationManager.GetLocation(locationID);
                locationTableAdapter.SetSearchHint(location.GetSearchString(), locationID);

                transaction.Complete();
                return location;
            }
        }

        static public Location GetLocation(Guid locationID)
        {
            if (locationID == Guid.Empty)
            {
                return Location.Empty;
            }

            using (LocationTableAdapter locationTableAdapter = new LocationTableAdapter())
            {
                LocationDataSet.LocationDataTable dataTable = locationTableAdapter.GetLocationByLocationID(locationID);

                if (dataTable.Rows.Count < 1) { throw new ArgumentException("Location does not exist"); }

                return new Location(dataTable[0]);
            }
        }

        static public List<Location> GetNamedLocations(string name)
        {
            return LocationManager.GetNamedLocations(name, 0, LocationManager.GetNamedLocationsCount(name));
        }

        static public List<Location> GetNamedLocations(string name, int startRowIndex, int maximumRows)
        {
            List<Location> list = new List<Location>();

            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            // Escape wildcard characters for LIKE statement.
            name = name.Replace("%", "!%");
            name = name.Replace("_", "!_");
            name = name.Replace("[", "![");

            using (LocationTableAdapter locationTableAdapter = new LocationTableAdapter())
            {
                foreach (LocationDataSet.LocationRow row in locationTableAdapter.GetNamedLocations(name, startRowIndex, maximumRows))
                {
                    list.Add(new Location(row));
                }

                return list;
            }
        }

        static public int GetNamedLocationsCount(string name)
        {
            List<Location> list = new List<Location>();

            if (string.IsNullOrEmpty(name)) { throw new ArgumentNullException("name"); }

            // Escape wildcard characters for LIKE statement.
            name = name.Replace("%", "!%");
            name = name.Replace("_", "!_");
            name = name.Replace("[", "![");

            using (LocationTableAdapter locationTableAdapter = new LocationTableAdapter())
            {
                return (int)locationTableAdapter.GetNamedLocationsCount(name);
            }
        }

        static public List<Location> SearchLocations(string name, string address1, string address2, string city, string region, string country, string postalCode)
        {
            return LocationManager.SearchLocations(name, address1, address2, city, region, country, postalCode, 0,
                LocationManager.SearchLocationsCount(name, address1, address2, city, region, country, postalCode));
        }

        static public List<Location> SearchLocations(string name, string address1, string address2, string city, string region, string country, string postalCode,
            int startRowIndex, int maximumRows)
        {
            List<Location> list = new List<Location>();

            if (name == null) { name = string.Empty; }
            if (address1 == null) { address1 = string.Empty; }
            if (address2 == null) { address2 = string.Empty; }
            if (city == null) { city = string.Empty; }
            if (region == null) { region = string.Empty; }
            if (country == null) { country = string.Empty; }
            if (postalCode == null) { postalCode = string.Empty; }

            if ((name + address1 + address2 + city + region + country + postalCode).Length == 0)
            {
                return list;
            }

            using (LocationTableAdapter locationTableAdapter = new LocationTableAdapter())
            {
                foreach (LocationDataSet.LocationRow row in locationTableAdapter.SearchLocations(
                    Location.BuildSearchString(name, address1, address2, city, region, country, postalCode), startRowIndex, maximumRows))
                {
                    list.Add(new Location(row));
                }

                return list;
            }
        }

        static public int SearchLocationsCount(string name, string address1, string address2, string city, string region, string country, string postalCode)
        {
            List<Location> list = new List<Location>();

            if (name == null) { name = string.Empty; }
            if (address1 == null) { address1 = string.Empty; }
            if (address2 == null) { address2 = string.Empty; }
            if (city == null) { city = string.Empty; }
            if (region == null) { region = string.Empty; }
            if (country == null) { country = string.Empty; }
            if (postalCode == null) { postalCode = string.Empty; }

            if ((name + address1 + address2 + city + region + country + postalCode).Length == 0)
            {
                return 0;
            }

            using (LocationTableAdapter locationTableAdapter = new LocationTableAdapter())
            {
                return (int)locationTableAdapter.SearchLocationsCount(Location.BuildSearchString(name, address1, address2, city, region, country, postalCode));
            }
        }
    }
}
