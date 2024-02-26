/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: Destination.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Runtime.Serialization;


namespace VESilverlight
{
    //Implements a destination object ie. Las Vegas, Seattle, New York
    [DataContract]
    public class Destination
    {
        #region Fields

        /// <summary>
        /// Destination ID number
        /// </summary>
        private int id;

        /// <summary>
        /// Destination city name
        /// </summary>
        private string name;

        #endregion

        #region Public Methods
        /// <summary>
        /// Default constructor
        /// </summary>
        public Destination()
        {

        }

        /// <summary>
        /// Destination constructor
        /// </summary>
        /// <param name="id">Destination ID</param>
        /// <param name="name">Destination Name</param>
        public Destination(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        #endregion 

        #region Public Properties

        /// <summary>
        /// Destination city name
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Destination ID number
        /// </summary>
        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        #endregion

        #region Javascript Serialization
        /// <summary>
        /// Serialize this object
        /// </summary>
        /// <returns>Serialized Destination</returns>
        public string Serialize()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Destination));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, this);
                byte[] array = memoryStream.ToArray();
                return Encoding.UTF8.GetString(array, 0, array.Length);
            }
        }

        /// <summary>
        /// Returns a destination object from a JSON string
        /// </summary>
        /// <param name="serialText">Serialized Destination</param>
        /// <returns>Deserialized Destination</returns>
        public static Destination Deserialize(string serialText)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Destination));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serialText)))
            {
                return serializer.ReadObject(memoryStream) as Destination;
            }
        }

        /// <summary>
        /// Returns a destination object from a JSON string
        /// </summary>
        /// <param name="serialText">JSON object</param>
        /// <returns>Deserialized Destination</returns>
        public static Destination Deserialize(IDictionary<String, Object> values)
        {
            Destination returnVal = new Destination();

            PropertyInfo[] props = typeof(Destination).GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (!values.ContainsKey(prop.Name)) continue;

                Type t = prop.PropertyType;

                prop.SetValue(returnVal, values[prop.Name], null);
            }

            return returnVal;
        }

        #endregion
    }
}
