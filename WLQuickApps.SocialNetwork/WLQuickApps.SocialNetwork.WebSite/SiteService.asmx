<%@ WebService Language="C#" Class="WLQuickApps.SocialNetwork.WebSite.SiteService" %>

using System;
using System.Collections.Generic;
using System.Web.Services;

using WLQuickApps.SocialNetwork.Business;

namespace WLQuickApps.SocialNetwork.WebSite
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class SiteService : WebService
    {
        public SiteService()
        {
        }

        [WebMethod]
        public string[] GetLocationAutoCompleteList(string prefixText, int count)
        {
            if (count == 0)
            {
                count = 10;
            }
            
            List<string> locationNames = new List<string>(count);

            foreach (Location location in LocationManager.SearchLocations(prefixText, null, null, null, null, null, null, 0, count))
            {
                if (!locationNames.Contains(location.Name))
                {
                    locationNames.Add(location.Name);
                }
            }

            return locationNames.ToArray();
        }

        [WebMethod]
        public SerializableLocation GetNamedLocation(string name)
        {
            SerializableLocation location = new SerializableLocation();
            
            if (string.IsNullOrEmpty(name))
            {
                return location;
            }

            List<Location> matchingLocations = LocationManager.GetNamedLocations(name, 0, 1);

            if (matchingLocations.Count > 0)
            {
                location.Address1 = matchingLocations[0].Address1;
                location.Address2 = matchingLocations[0].Address2;
                location.City = matchingLocations[0].City;
                location.Country = matchingLocations[0].Country;
                location.Latitude = matchingLocations[0].Latitude;
                location.LocationID = matchingLocations[0].LocationID;
                location.Longitude = matchingLocations[0].Longitude;
                location.Name = matchingLocations[0].Name;
                location.PostalCode = matchingLocations[0].PostalCode;
                location.Region = matchingLocations[0].Region;
            }

            return location;
        }
    }
}