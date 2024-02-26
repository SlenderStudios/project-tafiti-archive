using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;

using WLQuickApps.FieldManager.Business;
using WLQuickApps.FieldManager.Data;

namespace WLQuickApps.FieldManager.WebSite
{
    /// <summary>
    /// Summary description for FieldItem
    /// </summary>
    [DataContract]
    public class FieldItem
    {
        [DataMember] public string Address;
        [DataMember] public string Description;
        [DataMember] public int FieldID;
        [DataMember] public bool IsOpen;
        [DataMember] public double Latitude;
        [DataMember] public double Longitude;
        [DataMember] public int NumberOfFields;
        [DataMember] public string ParkingLot;
        [DataMember] public string PhoneNumber;
        [DataMember] public string Status;
        [DataMember] public string Title;
        [DataMember] public Weather[] Forecast;

        public FieldItem(){}

        static public FieldItem CreateFromField(Field item)
        {
            FieldItem fieldItem = new FieldItem();

            fieldItem.Address = item.Address;
            fieldItem.Description = item.Description;
            fieldItem.FieldID = item.FieldID;
            fieldItem.IsOpen = item.IsOpen;
            fieldItem.Latitude = item.Latitude;
            fieldItem.Longitude = item.Longitude;
            fieldItem.NumberOfFields = item.NumberOfFields;
            fieldItem.ParkingLot = item.ParkingLot;
            fieldItem.PhoneNumber = item.PhoneNumber;
            fieldItem.Status = item.Status;
            fieldItem.Title = item.Title;
            fieldItem.Forecast = WeatherManager.GetWeather(item.Latitude, item.Longitude);

            return fieldItem;
        }

        static public FieldItem[] CreateFromFieldList(IEnumerable<Field> list)
        {
            List<FieldItem> fieldItems = new List<FieldItem>();
            foreach (Field field in list)
            {
                fieldItems.Add(FieldItem.CreateFromField(field));
            }
            return fieldItems.ToArray();
        }

    }
}