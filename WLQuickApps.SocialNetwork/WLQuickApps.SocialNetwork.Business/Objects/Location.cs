using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.SocialNetwork.Data;
using System.Collections.ObjectModel;

namespace WLQuickApps.SocialNetwork.Business
{
    [Serializable]
    public class Location
    {
        static public Location Empty
        {
            get
            {
                if (Location._emptyLocation == null)
                {
                    Location._emptyLocation = new Location(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, 0, 0);
                }
                return Location._emptyLocation;
            }
        }
        static private Location _emptyLocation;

        public string Name
        {
            get { return this._name; }
        }
        private string _name;

        public string Address1
        {
            get { return this._address1; }
        }
        private string _address1;

        public string Address2
        {
            get { return this._address2; }
        }
        private string _address2;

        public string City
        {
            get { return this._city; }
        }
        private string _city;

        public string Region
        {
            get { return this._region; }
        }
        private string _region;

        public string Country
        {
            get { return this._country; }
        }
        private string _country;

        public string PostalCode
        {
            get { return this._postalCode; }
        }
        private string _postalCode;

        public Guid LocationID
        {
            get { return this._locationID; }
        }
        private Guid _locationID;

        public double Latitude
        {
            get { return this._latitude; }
        }
        private double _latitude;

        public double Longitude
        {
            get { return this._longitude; }
        }
        private double _longitude;

        public Location(Guid locationID, string name, string address1, string address2, string city, string region, string country,
            string postalCode, double latitude, double longitude)
        {
            this._locationID = locationID;
            this._name = name;
            this._address1 = address1;
            this._address2 = address2;
            this._city = city;
            this._region = region;
            this._country = country;
            this._postalCode = postalCode;
            this._latitude = latitude;
            this._longitude = longitude;
        }

        public Location(LocationDataSet.LocationRow row)
            : this(row.LocationID, row.Name, row.Address1, row.Address2, row.City, row.Region, row.Country, row.PostalCode,
            row.Latitude, row.Longitude)
        {
        }

        static public string GetAddressText(string name, string address1, string address2, string city, string region, string country, string postalCode)
        {
            // Seems to work reliably for the Virtual Earth control.
            StringBuilder stringBuilder = new StringBuilder();
            if (address1.Length > 0) { stringBuilder.AppendFormat("{0}, ", address1); }
            if (address2.Length > 0) { stringBuilder.AppendFormat("{0}, ", address2); }
            if (city.Length > 0) { stringBuilder.AppendFormat("{0}, ", city); }
            if (region.Length > 0) { stringBuilder.AppendFormat("{0}, ", region); }
            if (postalCode.Length > 0) { stringBuilder.AppendFormat("{0}, ", postalCode); }
            if (country.Length > 0) { stringBuilder.AppendFormat("{0}", country); }

            if (stringBuilder.Length == 0) { return name; }

            return stringBuilder.ToString().TrimEnd(',', ' ');
        }

        public string GetAddressText()
        {
            return Location.GetAddressText(this.Name, this.Address1, this.Address2, this.City, this.Region, this.Country, this.PostalCode);
        }

        internal string GetSearchString()
        {
            return string.Format("{0}!{1}!{2}!{3}!{4}!{5}!{6}",
                this.Name.ToLower(), this.Address1.ToLower(), this.Address2.ToLower(), this.City.ToLower(),
                this.Region.ToLower(), this.Country.ToLower(), this.PostalCode.ToLower());
        }

        static internal string BuildSearchString(string name, string address1, string address2, string city, string region, string country, string postalCode)
        {
            return string.Format("%{0}%!%{1}%!%{2}%!%{3}%!%{4}%!%{5}%!%{6}%",
                name.ToLower(), address1.ToLower(), address2.ToLower(), city.ToLower(),
                region.ToLower(), country.ToLower(), postalCode.ToLower());
        }

        internal string GetSearchTerms()
        {
            StringBuilder termsBuilder = new StringBuilder(this.Name).AppendLine(this.Address1).AppendLine(this.Address2)
                .AppendLine(this.City).AppendLine(this.Region).AppendLine(this.Country).AppendLine(this.PostalCode);
            return termsBuilder.ToString();
        }

        public override bool Equals(object obj)
        {
            Location otherLocation = obj as Location;
            if (((object)otherLocation) == null) { return false; }

            return (this.LocationID == otherLocation.LocationID);
        }

        public override int GetHashCode()
        {
            return this.LocationID.GetHashCode();
        }

        public static bool operator ==(Location first, Location second)
        {
            if (((object)first) == null)
            {
                return (((object)second) == null);
            }

            return first.Equals(second);
        }

        public static bool operator !=(Location first, Location second)
        {
            return !(first == second);
        }
    }
}
