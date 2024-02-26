
/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: Model.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
//using System.Windows.Browser.Net;
using System.Windows.Browser;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Net;
using System.Text;
using VESilverlight.Primary;
using System.Threading;
namespace VESilverlight
{
    public delegate void EmptyDelegate();
    public delegate void AttractionListDelegate(List<Attraction> args);
    public delegate void AttractionDelegate(Attraction args);
    public delegate void DestinationListDelegate(List<Destination> args);
    public delegate void StringDelegate(string args);
    public delegate void IntDelegate(int args);    
    
    //This class represents an instance of a user logged into Windows Live.  
    public class Model
    {
        #region Private properties
        /// <summary>
        /// Instance
        /// </summary>
        private static Model instance = null;

        /// <summary>
        /// Shared Itinerary list
        /// </summary>
        private Dictionary<int, Dictionary<DateTime, List<Attraction>>> sharedItineraries = new Dictionary<int, Dictionary<DateTime, List<Attraction>>>();
        
        /// <summary>
        /// Itinerary list
        /// </summary>
        private Dictionary<int, Dictionary<DateTime, List<Attraction>>> itineraries = new Dictionary<int, Dictionary<DateTime,List<Attraction>>>();

        /// <summary>
        /// Attraction list (Concierge List)
        /// </summary>
        private Dictionary<int, List<Attraction>> allAttractions = new Dictionary<int, List<Attraction>>();

        /// <summary>
        /// Search results list
        /// </summary>
        private List<Attraction> searchResults = new List<Attraction>();

        /// <summary>
        /// User city destination list
        /// </summary>
        private List<Destination> myDestinations;
        
        /// <summary>
        /// Visitor ID
        /// </summary>
        private int visitorId = -1;

        /// <summary>
        /// Flag to indicate if an admin is logged in
        /// </summary>
        private bool isAdmin = false;

        /// <summary>
        /// Shared ID
        /// </summary>
        private int sharedId = -1;
        
        /// <summary>
        /// Flag to indicate if the user is logged in
        /// </summary>
        private bool loggedIn = false;

        /// <summary>
        /// The user's first name
        /// </summary>
        private string firstName = null;

        /// <summary>
        /// The user's last name
        /// </summary>
        private string lastName = null;


        #endregion

        #region Public Properties
        /// <summary>
        /// Visitor ID
        /// </summary>
        public int VisitorId
        {
            get { return visitorId; }
            set { visitorId = value; }
        }

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; }
        }

        /// <summary>
        /// Shared Visitor ID
        /// </summary>
        public int SharedVisitorId
        {
            get { return sharedId; }
            set { sharedId = value; }
        }

        /// <summary>
        /// Flag to indicate if the user is logged in
        /// </summary>
        public bool LoggedIn
        {
            get { return loggedIn; }
            set { loggedIn = value; }
        }

        /// <summary>
        /// The user's first name
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        /// <summary>
        /// The user's last name
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        #endregion

        #region Singleton constructor
        /// <summary>
        /// Get singleton instance
        /// </summary>
        /// <returns>The model</returns>
        public static Model GetInstance()
        {
            if (instance == null)
            {
                instance = new Model();
            }
            return instance;
        }
        /// <summary>
        /// Private constructor
        /// </summary>
        private Model()
        {            
            
        }
        #endregion

        #region Public methods

        /// <summary>
        /// Gets the list of user cities
        /// </summary>
        /// <returns></returns>
        public void GetMyDestinations(DestinationListDelegate callback)
        {
            if (!LoggedIn) callback(new List<Destination>());

            if (myDestinations == null)
            {
                UpdateDestinationData(visitorId, 
                    delegate()
                    {
                        callback(myDestinations);
                    }
                    );
            }
            else
            {
                callback(myDestinations);
            }
        }

        /// <summary>
        /// Gets the itinerary based on the day
        /// </summary>
        /// <param name="day">Day to retrieve</param>
        /// <returns>List of itinerary attractions</returns>
        public void GetItinerary(int destinationID, DateTime day, AttractionListDelegate callback){

            if (!LoggedIn)
            {
                callback(new List<Attraction>());
                return;
            }

            //DISABLE SCHEDULING BY USING ONLY DateTime.MinValue
            day = DateTime.MinValue;

            if (itineraries.ContainsKey(destinationID) && itineraries[destinationID].ContainsKey(day.Date))
            {
                callback(itineraries[destinationID][day.Date]);
            }
            else
            {
                UpdateItineraryData(destinationID, visitorId, Attraction.ListType.User, itineraries,
                    delegate()
                    {
                        if (itineraries[destinationID].ContainsKey(day.Date))
                        {
                            callback(itineraries[destinationID][day.Date]);
                        }
                        else
                        {
                            callback(new List<Attraction>());
                        }
                    });
            }
                
        }

        /// <summary>
        /// Gets the itinerary based on the day
        /// </summary>
        /// <param name="day">Day to retrieve</param>
        /// <returns>List of itinerary attractions</returns>
        public void GetSharedItinerary(int destinationID, DateTime day, AttractionListDelegate callback)
        {
            //DISABLE SCHEDULING BY USING ONLY DateTime.MinValue
            day = DateTime.MinValue;

            if (sharedItineraries.ContainsKey(destinationID) && sharedItineraries[destinationID].ContainsKey(day.Date))
            {
                callback(sharedItineraries[destinationID][day.Date]);
            }
            else
            {
                if (sharedId == -1)
                {
                    callback(new List<Attraction>()); //todo throw error
                }
                else
                {
                    UpdateItineraryData(destinationID, sharedId, Attraction.ListType.Shared, sharedItineraries,
                        delegate()
                        {
                            if (sharedItineraries[destinationID].ContainsKey(day.Date))
                            {
                                callback(sharedItineraries[destinationID][day.Date]);
                            }
                            else
                            {
                                callback(new List<Attraction>());
                            }
                        });
                }
            }

        }

        /// <summary>
        /// Save the itinerary to DB
        /// </summary>
        public void SaveMyDay()
        {
            SaveItineraryData();
        }

        /// <summary>
        /// Get concierge list of attractions
        /// </summary>
        /// <returns>List of concierge attractions</returns>
        public void GetConciergeList(int destinationID, AttractionListDelegate callback)
        {
            if (!allAttractions.ContainsKey(destinationID))
            {
                UpdateConciergeData(destinationID,
                    delegate()
                    {
                        callback(allAttractions[destinationID]);
                    });
            }
            else
            {
                callback(allAttractions[destinationID]);
            }
        }


        /// <summary>
        /// Add an item to concierge
        /// </summary>
        /// <param name="attraction">Attraction to add to the itinerary</param>
        public void AddToConcierge(int destinationID, Attraction attraction)
        {
            allAttractions[destinationID].Add(attraction);
            SaveConcierge();
        }


        /// <summary>
        /// Remove an item to concierge
        /// </summary>
        /// <param name="attraction">Attraction to add to the itinerary</param>
        public void RemoveFromConcierge(int destinationID, Attraction attraction)
        {
            //allAttractions[destinationID].Add(attraction);

            foreach (Attraction attr in allAttractions[destinationID])
            {
                if (attr.ID == attraction.ID)
                {
                    allAttractions[destinationID].Remove(attr);
                    break;
                }
            }
            SaveConcierge();
        }

        /// <summary>
        /// Get searched results list
        /// </summary>
        /// <returns>List of search results attractions</returns>
        public List<Attraction> GetSearchResultList()
        {
            return searchResults;
        }

        /// <summary>
        /// Add an attraction to visitors itinerary
        /// </summary>
        /// <param name="attraction">Attraction to add to the itinerary</param>
        public void AddToItinerary(int destinationID, string destinationName, Attraction attraction)
        {
            List<Attraction> dayList;

            if (!itineraries.ContainsKey(destinationID))
            {
                UpdateItineraryData(destinationID, visitorId, Attraction.ListType.User, itineraries,
                    delegate()
                    {
                        if (itineraries[destinationID].ContainsKey(attraction.Date.Date))
                        {
                            dayList = itineraries[destinationID][attraction.Date.Date];

                        }
                        else
                        {
                            dayList = new List<Attraction>();
                        }

                        //fake a deep copy
                        attraction = Attraction.Deserialize(attraction.Serialize());

                        attraction.List = Attraction.ListType.User;

                        dayList.Add(attraction);

                        itineraries[destinationID][attraction.Date.Date] = dayList;

                        bool hasDest = false;
                        GetMyDestinations(
                            delegate(List<Destination> destinations)
                            {
                                foreach (Destination dest in destinations)
                                {
                                    if (dest.ID == destinationID)
                                    {
                                        hasDest = true;
                                        break;
                                    }
                                }

                                if (!hasDest)
                                    destinations.Add(new Destination(destinationID, destinationName));
                            });
                    });
            }
            else
            {
                if (itineraries[destinationID].ContainsKey(attraction.Date.Date))
                {
                    dayList = itineraries[destinationID][attraction.Date.Date];

                }
                else
                {
                    dayList = new List<Attraction>();
                }

                //fake a deep copy
                attraction = Attraction.Deserialize(attraction.Serialize());

                attraction.List = Attraction.ListType.User;

                dayList.Add(attraction);

                itineraries[destinationID][attraction.Date.Date] = dayList;

                bool hasDest = false;
                GetMyDestinations(
                    delegate(List<Destination> destinations)
                    {
                        foreach (Destination dest in destinations)
                        {
                            if (dest.ID == destinationID)
                            {
                                hasDest = true;
                                break;
                            }
                        }

                        if (!hasDest)
                            destinations.Add(new Destination(destinationID, destinationName));
                    });
            }
        }

        /// <summary>
        /// Remove attraction from itinerary
        /// </summary>
        /// <param name="attraction">Attraction to remove</param>
        public void RemoveFromItinerary(int destinationID, Attraction attraction)
        {
            GetItinerary(destinationID, DateTime.MinValue, new AttractionListDelegate(
                delegate(List<Attraction> attractions)
                {
                    foreach (Attraction attr in attractions)
                    {
                        if (attr.ID == attraction.ID)
                        {
                            attractions.Remove(attr);
                            itineraries[destinationID][attr.Date] = attractions;
                            break;
                        }
                    }
                }));
        }
        #endregion

        #region Client-Server Data Operations

        /// <summary>
        /// Store personal info into DB for new registered user
        /// </summary>
        public void SavePersonal(EmptyDelegate callback)
        {
            //send the data back to the server, where it will magically be saved
            //uc is the param indicating UPDATE_CONCIERGE
            string handlerReq = string.Format("visitplanner.ashx?fn=sp&uid={0}&nf={1}&nl={2}", visitorId.ToString(), firstName, lastName);
            HttpWebRequest _request = (HttpWebRequest) HttpWebRequest.Create(new Uri(Utilities.GetAbsolutePath(handlerReq)));
            _request.Method = "GET";

            _request.BeginGetResponse(new AsyncCallback( 
                delegate(IAsyncResult result)
                {
                    HttpWebResponse response = (HttpWebResponse) _request.EndGetResponse(result);
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception("HttpStatusCode " +
                            response.StatusCode.ToString() + " was returned.");

                    callback();
                }), null);

        }

        /// <summary>
        /// Get the concierge data from server
        /// </summary>
        private void UpdateConciergeData(int destinationID, EmptyDelegate callback)
        {
            if (allAttractions.ContainsKey(destinationID))
            {
                allAttractions[destinationID].Clear();
            }
            else
            {
                allAttractions[destinationID] = new List<Attraction>();
            }

            SynchronizationContext context = SynchronizationContext.Current;

            //retrieve georss from webservice

            HttpWebRequest _request = (HttpWebRequest) HttpWebRequest.Create(new Uri(Utilities.GetAbsolutePath(string.Format("visitplanner.ashx?fn=gc&did={0}", destinationID))));
            //Prevents caching of response
            //_request.IfModifiedSince = "Sat, 29 Oct 1994 19:43:31 GMT";
            _request.BeginGetResponse(new AsyncCallback( 
                delegate(IAsyncResult responseResult)
                {
                    ThreadPool.QueueUserWorkItem(
                        delegate(object state)
                        {
                            context.Send(delegate(object otherState)
                            {
                                HttpWebResponse response = (HttpWebResponse) _request.EndGetResponse(responseResult);
                                if (response.StatusCode != HttpStatusCode.OK)
                                    throw new Exception("HttpStatusCode " +
                                        response.StatusCode.ToString() + " was returned.");

                                if (response.ContentLength > 1)
                                {
                                    // Read response
                                    StreamReader responseReader = new StreamReader(response.GetResponseStream());

                                    //xml response from DB
                                    string rawResponse = responseReader.ReadToEnd();

                                    if (rawResponse != null && rawResponse != string.Empty)
                                    {
                                        using (XmlReader reader = XmlReader.Create(new StringReader(rawResponse)))
                                        {
                                            if (!reader.ReadToFollowing("item")) return;
                                            while (!reader.EOF)
                                            {
                                                if (reader.Name != "item")
                                                {
                                                    reader.ReadToFollowing("item");
                                                    continue;
                                                }
                                                string itm = null;
                                                try
                                                {
                                                    itm = reader.ReadOuterXml();
                                                }
                                                catch (Exception ex)
                                                {
                                                    break;
                                                }

                                                //An attraction object is created by passing in the xml string segment
                                                //Go to Attraction.cs afor constructor
                                                Attraction attraction = new Attraction(itm);
                                                allAttractions[destinationID].Add(attraction);
                                            }
                                        }
                                    }
                                }
                                callback();
                        }, null);
                    });
                }), null);


        }

        /// <summary>
        /// Save the added concierge items 
        /// </summary>
        /// <returns></returns>
        private void SaveConcierge()
        {
            foreach (int key in allAttractions.Keys)
            {
                System.Text.StringBuilder xml = new System.Text.StringBuilder();

                xml.Append("<?xml version=\"1.0\"?><rss version=\"2.0\" xmlns:geo=\"http://www.w3.org/2003/01/geo/wgs84_pos#\"><channel>");

                foreach (Attraction attr in allAttractions[key])
                {
                    xml.Append(attr.GetXML());
                }

                xml.Append("</channel></rss>");

                //send the data back to the server, where it will magically be saved
                //uc is the param indicating UPDATE_CONCIERGE
                string handlerReq = string.Format("visitplanner.ashx?fn=uc&did={0}", key);

                HttpWebRequest _request = (HttpWebRequest)HttpWebRequest.Create(new Uri(Utilities.GetAbsolutePath(handlerReq)));
                _request.Method = "POST";
                _request.BeginGetRequestStream(new AsyncCallback( 
                    delegate(IAsyncResult requestResult)
                    {
                        UTF8Encoding encoding = new UTF8Encoding();
                        byte[] itinBytes = encoding.GetBytes(xml.ToString());
                        Stream body = _request.EndGetRequestStream(requestResult);
                        body.Write(itinBytes, 0, itinBytes.Length);  //posts the xml string to the handler

                        _request.BeginGetResponse(new AsyncCallback( 
                            delegate(IAsyncResult responseResult)
                            {
                                HttpWebResponse response = (HttpWebResponse) _request.EndGetResponse(responseResult);
                                if (response.StatusCode != HttpStatusCode.OK)
                                    throw new Exception("HttpStatusCode " +
                                        response.StatusCode.ToString() + " was returned.");
                            }), null);
                    }), null);                
            }

        }


        /// <summary>
        /// Retrieve all attractions on all days for the user
        /// </summary>
        private void UpdateDestinationData(int userID, EmptyDelegate callback)
        {
            myDestinations = new List<Destination>();

            SynchronizationContext context = SynchronizationContext.Current;
            
            //retrieve georss from webservice
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(new Uri(Utilities.GetAbsolutePath(string.Format("visitplanner.ashx?fn=gd&uid={0}", userID))));
            request.Method = "GET";

            request.BeginGetResponse(new AsyncCallback( 
                delegate(IAsyncResult responseResult)
                {
                    ThreadPool.QueueUserWorkItem(
                        delegate(object state)
                        {
                            context.Send(delegate(object otherState)
                            {
                                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(responseResult);
                                if (response.StatusCode != HttpStatusCode.OK)
                                    throw new Exception("HttpStatusCode " +
                                        response.StatusCode.ToString() + " was returned.");

                                if (response.ContentLength > 1)
                                {
                                    // Read response
                                    StreamReader responseReader = new StreamReader(response.GetResponseStream());

                                    string rawResponse = responseReader.ReadToEnd();

                                    if (rawResponse != null && rawResponse != string.Empty)
                                    {
                                        string[] destinations = rawResponse.Split('|');

                                        foreach (string destination in destinations)
                                        {
                                            myDestinations.Add(Destination.Deserialize(destination));
                                        }
                                    }
                                }

                                callback();
                            }, null);
                        });
                }), null);
        }

        /// <summary>
        /// Retrieve all attractions on all days for the user
        /// </summary>
        private void UpdateItineraryData(int destinationID, int userID, Attraction.ListType type,Dictionary<int, Dictionary<DateTime, List<Attraction>>> list, EmptyDelegate callback)
        {
            if (list.ContainsKey(destinationID)){
                list[destinationID].Clear();
            } else {
                list[destinationID] = new Dictionary<DateTime, List<Attraction>>();
            }

            SynchronizationContext context = SynchronizationContext.Current;

            //retrieve georss from webservice
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(new Uri(Utilities.GetAbsolutePath(string.Format("visitplanner.ashx?fn=gi&uid={0}&did={1}", userID, destinationID))));
            request.Method = "GET";

            request.BeginGetResponse(new AsyncCallback( 
                delegate(IAsyncResult responseResult)
                {
                    ThreadPool.QueueUserWorkItem(
                        delegate(object state)
                        {
                            context.Send(delegate(object otherState)
                            {
                                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(responseResult);
                                if (response.StatusCode != HttpStatusCode.OK)
                                    throw new Exception("HttpStatusCode " +
                                        response.StatusCode.ToString() + " was returned.");

                                if (response.ContentLength > 1)
                                {
                                    // Read response
                                    StreamReader responseReader = new StreamReader(response.GetResponseStream());

                                    string rawResponse = responseReader.ReadToEnd();

                                    if (rawResponse != null && rawResponse != string.Empty)
                                    {
                                        using (XmlReader reader = XmlReader.Create(new StringReader(rawResponse)))
                                        {
                                            if (!reader.ReadToFollowing("item")) return;

                                            while (!reader.EOF)
                                            {
                                                if (reader.Name != "item")
                                                {
                                                    reader.ReadToFollowing("item");
                                                    continue;
                                                }

                                                string itm = reader.ReadOuterXml();
                                                Attraction attraction = new Attraction(itm);

                                                //set flag that the attraction is in the user's list (as opposed to concierge)
                                                attraction.List = type;

                                                //add to itineraries collection by date (ignore time so everything on the same day goes in the same list)
                                                List<Attraction> dayList;

                                                //DISABLE SCHEDULING BY USING ONLY DateTime.MinValue
                                                attraction.Date = DateTime.MinValue;

                                                if (list[destinationID].ContainsKey(attraction.Date.Date))
                                                {
                                                    dayList = list[destinationID][attraction.Date.Date];

                                                }
                                                else
                                                {
                                                    dayList = new List<Attraction>();
                                                }

                                                dayList.Add(attraction);

                                                list[destinationID][attraction.Date.Date] = dayList;
                                            }
                                        }
                                    }
                                }
                                callback();
                            }, null);
                        });
                }), null);                
        }

       
        /// <summary>
        /// Save the visitors itinerary data 
        /// </summary>
        /// <returns></returns>
        private bool SaveItineraryData()
        {
            foreach (int key in itineraries.Keys)
            {
                System.Text.StringBuilder xml = new System.Text.StringBuilder();

                xml.Append("<?xml version=\"1.0\"?><rss version=\"2.0\" xmlns:geo=\"http://www.w3.org/2003/01/geo/wgs84_pos#\"><channel>");

                foreach (List<Attraction> itiniterary in itineraries[key].Values)
                {
                    foreach (Attraction attr in itiniterary)
                    {
                        xml.Append(attr.GetXML());
                    }
                }

                xml.Append("</channel></rss>");

                //send the data back to the server, where it will magically be saved
                //retrieve georss from webservice
                //ui is the param indicating UPDATE_ITINERARY
                string handlerReq = string.Format("visitplanner.ashx?fn=ui&did={1}&uid={0}", visitorId.ToString(), key);

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] itinBytes = encoding.GetBytes(xml.ToString());

                Uri endPoint = new Uri(Utilities.GetAbsolutePath(handlerReq));
                WebClient client = new WebClient();
                client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_SaveItineraryDataCompleted);
                client.UploadStringAsync(endPoint, xml.ToString());
            }
            return true;
        }

        void client_SaveItineraryDataCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ToString().Length <= 1)
                    return;
                if (!e.Result.ToString().Equals("updated"))
                    return;
            }
            else
            {
                throw new Exception(e.Error.Message.ToString());
            }
        }

        #endregion

    }
}
