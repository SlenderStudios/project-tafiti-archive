using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WLQuickApps.SocialNetwork.WebSite
{
    public struct SerializableLocation
    {
        public string Address1;
        public string Address2;
        public string City;
        public string Country;
        public double Latitude;
        public Guid LocationID;
        public double Longitude;
        public string Name;
        public string PostalCode;
        public string Region;
    }
}