using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using WLQuickApps.FieldManager.Silverlight.SiteService;

namespace WLQuickApps.FieldManager.Silverlight
{
    public class Field
    {
        private FieldItem _field;

        public string Address { get { return this._field.Address; } }
        public string Description { get { return this._field.Description; } }
        // We need to use FieldID as the Tag for the UI controls we databind to, but
        // it throws an exception unless we bind to a string. As a result, this
        // whole class exists just to provide the ID as a string.
        public string FieldID { get { return this._field.FieldID.ToString(); } }
        public bool IsOpen { get { return this._field.IsOpen; } }
        public double Latitude { get { return this._field.Latitude; } }
        public double Longitude { get { return this._field.Longitude; } }
        public int NumberOfFields { get { return this._field.NumberOfFields; } }
        public string ParkingLot { get { return this._field.ParkingLot; } }
        public string PhoneNumber { get { return this._field.PhoneNumber; } }
        public string Status { get { return this._field.Status; } }
        public string Title { get { return this._field.Title; } }
        public Weather[] Forecast { get { return this._field.Forecast; } }
        public Weather CurrentWeather { get { return this._field.Forecast[0]; } }

        private Field()
        {
        }

        static public Field CreateFromField(FieldItem fieldItem)
        {
            Field field = new Field();
            field._field = fieldItem;
            return field;
        }

        static public List<Field> CreateFromFieldList(IEnumerable<FieldItem> fieldItems)
        {
            List<Field> fields = new List<Field>();
            foreach (FieldItem fieldItem in fieldItems)
            {
                fields.Add(Field.CreateFromField(fieldItem));
            }
            return fields;
        }
    }
}
