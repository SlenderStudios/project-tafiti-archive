/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: Attraction.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;
using System.Text;
using System.Runtime.Serialization;


namespace VESilverlight
{
    /// <summary>
    /// Implements an attraction object used through out the visit planner 
    /// </summary> 
    [DataContract]
    public class Attraction
    {
        #region Public enumerations
        /// <summary>
        /// Type of attraction list enum
        /// </summary>
        public enum ListType { Concierge, User, Shared, PushpinSearch };
        
        /// <summary>
        /// Attraction category enum
        /// </summary>
        public enum Categories { Food, Music, Movie, Search, Accomodation, Art, Custom, Misc };

        /// <summary>
        /// Attraction recurrence enum
        /// </summary>
        public enum Recurrences { Daily, Weekly, Monthly, Yearly };

        /// <summary>
        /// Recurrence day enum
        /// </summary>
        private enum RecurrenceDay { Monday = -1, Tuesday = -2, Wednesday = -3, Thursday = -4, Friday = -5, Saturday = -6, Sunday = -7 }

        #endregion
        
        #region Constants
        /// <summary>
        /// Default pushpin
        /// </summary>
        private const string VP_DEFAULT_PUSHPIN = "/images/Btn_PushPin.png";
        #endregion

        #region Fields
        /// <summary>
        /// ID count
        /// </summary>
        private static int idcount = 0;

        /// <summary>
        /// ID count
        /// </summary>
        static public string NextID
        {
            get { idcount++; return idcount.ToString(); }
        }  

        /// <summary>
        /// Category of at attraction
        /// </summary>
        private Categories category = Categories.Search;

        /// <summary>
        /// Longitude mapping coordinate
        /// </summary>
        private double longitude = 0;

        /// <summary>
        /// Latitude mapping coordinate
        /// </summary>
        private double latitude = 0;

        /// <summary>
        /// Image url of attraction
        /// </summary>
        private string imageURL = Attraction.VP_DEFAULT_PUSHPIN;

        /// <summary>
        /// Image url of pushpin
        /// </summary>
        private string pushpinURL = Attraction.VP_DEFAULT_PUSHPIN;

        /// <summary>
        /// Video url of attraction
        /// </summary>
        private string videoURL = string.Empty;

        /// <summary>
        /// Brief description of attraction
        /// </summary>
        private string shortDescription = string.Empty;

        /// <summary>
        /// Title of attraction
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// Long description of attraction
        /// </summary>
        private string longDescription = string.Empty;

        /// <summary>
        /// Address line 1 of attraction
        /// </summary>
        private string addressLine1 = string.Empty;

        /// <summary>
        /// Address line 2 of attraction
        /// </summary>
        private string addressLine2 = string.Empty;

        /// <summary>
        /// Date of attraction
        /// </summary>
        private DateTime date;

        /// <summary>
        /// Recurrence of attraction
        /// </summary>
        private Recurrences recurrence = Recurrences.Daily;

        /// <summary>
        /// Day of attraction
        /// </summary>
        private int recurrenceDay = 0;

        /// <summary>
        /// Attraction ID
        /// </summary>
        private string id = string.Empty;

        /// <summary>
        /// Length of attraction
        /// </summary>
        private int duration = 60;

        /// <summary>
        /// Keywords of attraction
        /// </summary>
        private string keywords = string.Empty;

        /// <summary>
        /// Type of attraction list (concierge, user, shared, search results)
        /// </summary>
        private ListType list = ListType.Concierge;

        #endregion

        #region Public Properties
        /// <summary>
        /// Type of attraction list (concierge, user, shared, search results)
        /// </summary>
        [DataMember]
        public ListType List
        {
            get { return list; }
            set { list = value; }
        }

        /// <summary>
        /// Length of attraction
        /// </summary>
        [DataMember]
        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        /// <summary>
        /// Title of attraction
        /// </summary>
        [DataMember]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Category of at attraction
        /// </summary>
        [DataMember]
        public Categories Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// Image url of attraction
        /// </summary>
        [DataMember]
        public string ImageURL
        {
            get { return imageURL; }
            set { imageURL = value; }
        }

        /// <summary>
        /// Image url of pushpin
        /// </summary>
        [DataMember]
        public string PushpinURL
        {
            get { return pushpinURL; }
            set { pushpinURL = value; }
        }

        /// <summary>
        /// Video url of attraction
        /// </summary>
        [DataMember]
        public string VideoURL
        {
            get { return videoURL; }
            set { videoURL = value; }
        }

        /// <summary>
        /// Brief description of attraction
        /// </summary>
        [DataMember]
        public string ShortDescription
        {
            get { return shortDescription; }
            set { shortDescription = value; }
        }

        /// <summary>
        /// Long description of attraction
        /// </summary>
        [DataMember]
        public string LongDescription
        {
            get { return longDescription; }
            set { longDescription = value; }
        }

        /// <summary>
        /// Address line 1 of attraction
        /// </summary>
        [DataMember]
        public string AddressLine1
        {
            get { return addressLine1; }
            set { addressLine1 = value; }
        }

        /// <summary>
        /// Address line 2 of attraction
        /// </summary>
        [DataMember]
        public string AddressLine2
        {
            get { return addressLine2; }
            set { addressLine2 = value; }
        }

        /// <summary>
        /// Date of attraction
        /// </summary>
        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// Recurrence of attraction
        /// </summary>
        [DataMember]
        public Recurrences Recurrence
        {
            get { return recurrence; }
            set { recurrence = value; }
        }

        /// <summary>
        /// Latitude mapping coordinate
        /// </summary>
        [DataMember]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        /// <summary>
        /// Longitude mapping coordinate
        /// </summary>
        [DataMember]
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        /// <summary>
        /// Attraction ID
        /// </summary>
        [DataMember]
        public string ID
        {
            get 
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = Attraction.NextID;
                }
                return id; 
            }
            set { id = value; }
        }

        /// <summary>
        /// Keywords of attraction
        /// </summary>
        [DataMember]
        public string Keywords
        {
            get { return keywords; }
            set { keywords = value; }
        }

        #endregion

        #region Javascript Serialization
       
        /// <summary>
        /// Serializes an attraction object into JSON object
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Attraction));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, this);
                byte[] array = memoryStream.ToArray();
                return Encoding.UTF8.GetString(array, 0, array.Length);
            }
        }

        /// <summary>
        /// Returns an attraction object from a JSON string
        /// </summary>
        /// <param name="serialText"></param>
        /// <returns></returns>
        public static Attraction Deserialize(string serialText)
        {
            if (_attractionCache.ContainsKey(serialText))
            {
                return _attractionCache[serialText];
            }

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Attraction));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serialText)))
            {
                Attraction attraction = serializer.ReadObject(memoryStream) as Attraction;
                _attractionCache.Add(serialText, attraction);
                return attraction;
            }
        }
        private static Dictionary<string, Attraction> _attractionCache = new Dictionary<string,Attraction>();

        /// <summary>
        /// Returns an attraction object from a JSON string
        /// </summary>
        /// <param name="serialText"></param>
        /// <returns></returns>
        public static Attraction Deserialize(IDictionary<String, Object> values)
        {
            Attraction returnVal = new Attraction();

            System.Reflection.PropertyInfo[] props = typeof(Attraction).GetProperties();

            foreach (PropertyInfo prop in props)
            {
                if (!values.ContainsKey(prop.Name)) continue;

                Type t = prop.PropertyType;

                if (t == typeof(Double))
                {
                    prop.SetValue(returnVal, Double.Parse(values[prop.Name].ToString()), null);
                }
                else
                {
                    prop.SetValue(returnVal, values[prop.Name], null);
                }
            }

            //fix GMT conversion of time
            returnVal.Date = returnVal.Date.ToLocalTime();

            return returnVal;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Default constructor
        /// </summary>
        public Attraction()
        {
        }

        /// <summary>
        /// A concierge attraction object is created out of an XML string
        /// </summary>
        /// <param name="xmlData">XML string</param>
        public Attraction(string xmlData){

            id = Attraction.NextID;

            using (XmlReader reader = XmlReader.Create(new StringReader(xmlData))) //using temp data
            {
                reader.Read();
                
                while (!reader.EOF){
                    
                        switch (reader.Name)
                        {
                            case "category":

                                string contents = reader.ReadElementContentAsString();
                                for (int z = 0; z < 8; z++)
                                {
                                    if (contents == ((Categories)z).ToString("g"))
                                    {
                                        category = (Categories)z;
                                        break;
                                    }
                                }
                                break;
                            case "title":
                                title = reader.ReadElementContentAsString();
                                break;
                            case "duration":
                                duration = reader.ReadElementContentAsInt();
                                break;
                            case "imageURL":
                                imageURL = reader.ReadElementContentAsString();
                                break;
                            case "pushpinURL":
                                pushpinURL = reader.ReadElementContentAsString();
                                break;
                            case "videoURL":
                                videoURL = reader.ReadElementContentAsString();
                                break;
                            case "shortDescription":
                                shortDescription = reader.ReadElementContentAsString();
                                break;
                            case "longDescription":
                                longDescription = reader.ReadElementContentAsString();
                                break;
                            case "addressLine1":
                                addressLine1 = reader.ReadElementContentAsString();
                                break;
                            case "addressLine2":
                                addressLine2 = reader.ReadElementContentAsString();
                                break;
                            case "keywords":
                                keywords = reader.ReadElementContentAsString();
                                break;
                            case "date":
                                try
                                {
                                    string dateString = reader.ReadElementContentAsString();
                                    date = DateTime.Parse(dateString);
                                }
                                catch (Exception)
                                {
                                    date = DateTime.MinValue;
                                }
                                break;
                            case "recurrence":
                                string recur = reader.ReadElementContentAsString();
                                for (int x = -1; x > -8; x--)
                                {
                                    if (recur == ((RecurrenceDay)x).ToString("G"))
                                    {
                                        recurrenceDay = x;
                                        recurrence = Recurrences.Weekly;
                                        break;
                                    }
                                }
                                if (recurrenceDay != 0) break;

                                try
                                {
                                    recurrenceDay = Int32.Parse(recur);
                                    recurrence = Recurrences.Monthly;
                                    break;
                                }
                                catch (Exception)
                                {

                                }

                                recurrence = Recurrences.Daily;

                                break;
                            case "geo:Point":
                                if (reader.NodeType == XmlNodeType.EndElement)
                                {
                                    reader.Read();
                                    break;
                                }
                                int count = 0;
                                while (count < 2 && ! reader.EOF)
                                {
                                    switch (reader.Name)
                                    {
                                        case "geo:lat":
                                            latitude = reader.ReadElementContentAsDouble();
                                            count++;
                                            break;
                                        case "geo:long":
                                            longitude = reader.ReadElementContentAsDouble();
                                            count++;
                                            break;
                                        default:
                                            reader.Read();
                                            break;
                                    }
                                }
                                break;

                            default:
                                reader.Read();
                                break;

                        }
                        
                }
            }
        }

        /// <summary>
        /// Copies an attraction
        /// </summary>
        /// <returns></returns>
        public Attraction Clone()
        {
            Attraction att = Attraction.Deserialize(this.Serialize());
            att.ID = Attraction.NextID;
            return att;
        }

        /// <summary>
        /// Returns the XML representation of an attraction object
        /// </summary>
        /// <returns></returns>
        public string GetXML()
        {
            System.Text.StringBuilder output = new System.Text.StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = false;
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(new StringWriter(output), settings))
            {

                writer.WriteStartElement("item");
                writer.WriteElementString("category",this.Category.ToString("g"));
                writer.WriteElementString("title", this.Title);
                writer.WriteElementString("videoURL", this.VideoURL);  
                writer.WriteElementString("imageURL", this.ImageURL);
                writer.WriteElementString("pushpinURL", this.PushpinURL);
                writer.WriteElementString("shortDescription", this.ShortDescription);
                writer.WriteElementString("longDescription", this.LongDescription);
                writer.WriteElementString("addressLine1", this.AddressLine1);
                writer.WriteElementString("addressLine2", this.AddressLine2);
                writer.WriteElementString("date", this.Date == DateTime.MinValue ? string.Empty : this.Date.ToString());
                writer.WriteElementString("recurrence", this.Recurrence.ToString("g"));
                writer.WriteElementString("duration", this.Duration.ToString());
                writer.WriteElementString("keywords", this.Keywords);
                writer.WriteStartElement("geo","Point","geo");
                writer.WriteElementString("geo","lat","geo", this.Latitude.ToString());
                writer.WriteElementString("geo", "long", "geo", this.Longitude.ToString());
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }

            return output.ToString();

        }

        /// <summary>
        /// Checks if a keyword is found anywhere in the attraction's
        /// properties ie. title, short and long description
        /// </summary>
        /// <param name="termString"></param>
        /// <returns>Boolean</returns>
        public bool MatchesSearchTerms(string termString)
        {
            string[] terms = termString.Split(' ');

            string[] kWords = keywords.Split(' ');

            string[] titleParts = Title.Split(' ');

            foreach (string term in terms)
            {
                foreach (string titlePart in titleParts)
                {
                    if (term.Equals(titlePart, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }

                if (term.Equals(Category.ToString("g"), StringComparison.CurrentCultureIgnoreCase))
                    return true;

                foreach (string kWord in kWords)
                {
                    if (term.Equals(kWord, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        #endregion    
    
    }
}
