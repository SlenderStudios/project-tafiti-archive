/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: VisitPlannerHandler.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Web;
using System.Collections.Specialized;
using System.Collections.Generic;
using VisitPlanner;
using VisitPlanner.BusinessObjects;
using VisitPlanner.Data;

namespace VisitPlanner.Business
{
    /// <summary>
    /// Handles communication from the client to the data layer.
    /// </summary>
    public class VisitPlannerHandler : IHttpHandler
    {
        #region Private Properties
        /// <summary>
        /// Function paramter key.  The following values can be applied:
        /// FUNC_GET_CONCIERGE, FUNC_UPDATE_CONCIERGE, FUNC_GET_ITINERARY or 
        /// FUNC_UPDATE_ITINERARY
        /// </summary>
        private const string FUNC_PARAM = "fn";

        /// <summary>
        /// Get concierge function call.  This will invoke a 
        /// call to get concierge data. 
        /// </summary>
        private const string FUNC_GET_CONCIERGE = "gc";

        /// <summary>
        /// Update concierge function call.  This will invoke a 
        /// call to update concierge data. 
        /// </summary>
        private const string FUNC_UPDATE_CONCIERGE = "uc";

        /// <summary>
        /// Get itinerary function call.  This will invoke a 
        /// call to get itinerary data. 
        /// </summary>
        private const string FUNC_GET_ITINERARY = "gi";

        /// <summary>
        /// Get itinerary function call.  This will invoke a 
        /// call to get user destination list. 
        /// </summary>
        private const string FUNC_GET_DESTINATIONS = "gd";

        /// <summary>
        /// Update itinerary function call.  This will invoke a 
        /// call to update itinerary data. 
        /// </summary>
        private const string FUNC_UPDATE_ITINERARY = "ui";

        /// <summary>
        /// User ID key used to pass the user's ID.
        /// </summary>
        private const string USER_ID_PARAM = "uid";

        /// <summary>
        /// Concierge ID key used to pass the Concierge's 
        /// ID
        /// </summary>
        private const string CONCIERGE_ID_PARAM = "cid";

        /// <summary>
        /// Destination ID key used to pass the Destination's ID
        /// </summary>
        private const string DESTINATION_ID_PARAM = "did";
        
        /// <summary>
        /// Feed parameter key used to pass the feed content.  
        /// A URL encoded feed is expected.
        /// </summary>
        private const string FEED_PARAM = "fd";

        /// <summary>
        /// Feed parameter key used to store personal info
        /// </summary>
        private const string FUNC_STORE_PERSONAL = "sp";

        /// <summary>
        /// Feed parameter key used to indicate first name
        /// </summary>
        private const string FIRST_NAME = "nf";

        /// <summary>
        /// Feed parameter key used to inicate last name
        /// </summary>
        private const string LAST_NAME = "nl";

        /// <summary>
        /// Data connection.
        /// </summary>
        private DataAccess connection;
        #endregion

        #region Public Properties
        /// <summary>
        /// IHttpHandler reusable flag
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Process the request
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/string";
            
            //get parameters
            NameValueCollection parms = context.Request.Params;
            
            //function type

            string response = " ";
            
            if(parms[FUNC_PARAM] != null)
            {
                switch (parms[FUNC_PARAM])
                {
                    case FUNC_GET_CONCIERGE:
                        response = GetConcierge(parms[DESTINATION_ID_PARAM]);

                        break;

                    case FUNC_GET_DESTINATIONS:
                        {
                            VisitPlannerUser vp = GetUser(parms[USER_ID_PARAM]);

                            List<VESilverlight.Destination> destList = VPUserManager.GetUserDestinations(vp);

                            if (destList.Count > 0)
                            {
                                response = string.Empty;
                                foreach (VESilverlight.Destination dest in destList)
                                {
                                    response += dest.Serialize() + "|";
                                }
                                response = response.Substring(0, response.Length - 1);
                            }

                            break;
                        }
                    case FUNC_GET_ITINERARY:
                        {
                            int destId = -1;

                            if (!int.TryParse(parms[DESTINATION_ID_PARAM], out destId))
                            {
                                //todo: indicate error to client
                                break;
                            }

                            VisitPlannerUser vp = GetUser(parms[USER_ID_PARAM]);

                            if (vp != null && vp.DestinationCollectionList != null && vp.DestinationCollectionList.ContainsKey(destId) && vp.DestinationCollectionList[destId].Count > 0)
                            {
                                response = GetCollection(vp.DestinationCollectionList[destId][0]);
                            }

                            break;
                        }
                    case FUNC_UPDATE_CONCIERGE:
                        {
                            //retrieve concierge list xml from POST data                        
                            byte[] buffer = new byte[context.Request.InputStream.Length];
                            context.Request.InputStream.Read(buffer, 0, (int)context.Request.InputStream.Length);
                            string itinString = System.Text.Encoding.UTF8.GetString(buffer);
                            response = UpdateConcierge(parms[DESTINATION_ID_PARAM], itinString);
                            break;
                        }
                    case FUNC_UPDATE_ITINERARY:
                        {
                            VisitPlannerUser vp = GetUser(parms[USER_ID_PARAM]);

                            int destId = -1;

                            if (!int.TryParse(parms[DESTINATION_ID_PARAM], out destId))
                            {
                                //todo: indicate error to client
                                break;
                            }

                            //retrieve itinerary list xml from POST data
                            byte[] buffer = new byte[context.Request.InputStream.Length];
                            context.Request.InputStream.Read(buffer, 0, (int)context.Request.InputStream.Length);
                            string itinString = System.Text.Encoding.UTF8.GetString(buffer);  //itinString is the xml string
                            if (vp != null && vp.DestinationCollectionList != null && vp.DestinationCollectionList.ContainsKey(destId) && vp.DestinationCollectionList[destId].Count > 0)
                            {
                                response = UpdateCollection(vp.DestinationCollectionList[destId][0], itinString);
                            }
                            else
                            {
                                response = AddCollection(vp.UserNumber, destId, itinString);
                            }
                            break;
                        }
                    case FUNC_STORE_PERSONAL:
                        {
                            //retrieve concierge list xml from POST data   
                            string firstNameParam = parms[FIRST_NAME];
                            string lastNameParam = parms[LAST_NAME];
                            string paramID = parms[USER_ID_PARAM];
                            response = SavePersonal(paramID, firstNameParam, lastNameParam);
                            break;
                        }
                }
            
            }
            //the XML concierge string stored in the DB
            context.Response.Write(response);
        }
        
        #endregion

        #region Private methods
        /// <summary>
        /// Get the user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private VisitPlannerUser GetUser(string userID)
        {
            int uid = -1;
            int.TryParse(userID, out uid);
            return VPUserManager.GetUser(uid);
        }

        /// <summary>
        /// Get concierge feed
        /// </summary>
        /// <param name="destinationID"></param>
        /// <returns></returns>
        private string GetConcierge(string destinationID)
        {
            int did = -1;
            int.TryParse(destinationID, out did);
            connection = new DataAccess();
            return connection.GetConciergeByDestination(did);
        }

        /// <summary>
        /// Update concierge feed
        /// </summary>
        /// <param name="conciergeID"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        private string UpdateConcierge(string destinationID, string feed)
        {
            int did = -1;
            int.TryParse(destinationID, out did);
            connection = new DataAccess();
            connection.SaveConcierge(did, feed);
            return "updated";
        }

        /// <summary>
        /// Get itinerary for a user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private string GetCollection(int collectionID)
        {
            connection = new DataAccess();
            VPCollection vp = connection.GetCollection(collectionID);
            if (vp != null)
            {
                return vp.Collection;
            }

            return null;
        }
        
        /// <summary>
        /// Update a collection
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        private string UpdateCollection(int collectionID, string feed)
        {

            connection = new DataAccess();
            connection.SaveCollection(collectionID, feed);
            
            return "updated";
        
        }
        /// <summary>
        /// Add a new collection
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="feed"></param>
        /// <returns></returns>
        private string AddCollection(int userID, int destinationID, string feed)
        {

            connection = new DataAccess();
            connection.AddCollection(userID, destinationID, feed);

            return "added";

        }


        /// <summary>
        /// Save personal info
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private string SavePersonal(string userID, string firstName, string lastName)
        {
            int uid = -1;
            int.TryParse(userID, out uid);
            connection = new DataAccess();
            connection.SavePersonal(uid, firstName, lastName);
            return "saved";
        }


        #endregion
    }
        
}
