/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: DataAccess.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using VisitPlanner.BusinessObjects;
using System.Xml;
using System.IO;
using VESilverlight;

namespace VisitPlanner.Data
{
    /// <summary>
    /// Interface between the business objects and the database
    /// </summary>
    public class DataAccess
    {
        #region Private properties
        /// <summary>
        /// Data connection
        /// </summary>
        private SqlConnection sqlConn = null;

        /// <summary>
        /// Connection string
        /// </summary>
        private string connString = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize object
        /// </summary>
        public DataAccess()
        {
            connString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            sqlConn = new SqlConnection(connString);
        }

        #endregion

        #region User functions
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public VisitPlannerUser AddNewUser(VisitPlannerUser user)
        {
            if (!OpenConnection())
            {            
                return user;
            }

            try
            {
                string userType = "user";  //can be user or admin
                SqlCommand com = new SqlCommand("sp_InsertUser", sqlConn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@LiveID", user.LiveID));
                com.Parameters.Add(new SqlParameter("@UserType", userType));
                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(com);
                ad.Fill(ds);
                user = ParseUser(ds);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return user;
        }
            
        /// <summary>
        /// Retreive user from database by user ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public VisitPlannerUser GetUser(int userID)
        {
            VisitPlannerUser user = null;

            if (!OpenConnection())
            {
     
                return user;
            }

            try
            {
                SqlCommand com = new SqlCommand("sp_GetUser", sqlConn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@UserID", userID));
                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(com);
                ad.Fill(ds);
                user = ParseUser(ds);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return user;
        }

        /// <summary>
        /// Get the user by live ID
        /// </summary>
        /// <param name="liveID"></param>
        /// <returns></returns>
        public VisitPlannerUser GetUserByLiveID(string liveID)
        {
            VisitPlannerUser user = null;

            if (!OpenConnection())
            {
                return user;
            }

            try
            {
                SqlCommand com = new SqlCommand("sp_GetUserByLiveID", sqlConn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@LiveID", liveID));
                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(com);
                ad.Fill(ds);
                user = ParseUser(ds);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return user;
        }
        
        /// <summary>
        /// Parse user from a dataset and return the user object
        /// </summary>
        /// <param name="ds">Data set containing a user</param>
        /// <returns>The retrieved visit planner user object</returns>
        private VisitPlannerUser ParseUser(DataSet ds)
        {
            VisitPlannerUser user = null;
            if (ds != null && ds.Tables != null)
            {

                user = new VisitPlannerUser();
                DataRow userRow = ds.Tables[0].Rows[0];
                user.LiveID = ds.Tables[0].Rows[0]["WindowsLiveID"].ToString();
                user.UserType = ds.Tables[0].Rows[0]["UserType"].ToString();
                user.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                user.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();

                int uid = -1;
                if (int.TryParse(ds.Tables[0].Rows[0]["UserID"].ToString(), out uid))
                {
                    user.UserNumber = uid;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["DestinationID"] != null && ds.Tables[0].Rows[i]["CollectionID"] != null)
                        {
                            int destID = -1;
                            int collID = -1;
                            if (int.TryParse(ds.Tables[0].Rows[i]["DestinationID"].ToString(), out destID) && int.TryParse(ds.Tables[0].Rows[i]["CollectionID"].ToString(), out collID))
                            {
                                if (!user.DestinationCollectionList.ContainsKey(destID))
                                {
                                    user.DestinationCollectionList[destID] = new List<int>();
                                }

                                user.DestinationCollectionList[destID].Add(collID);

                            }
                        }
                    }
                }
            }
            return user;

        }


        /// <summary>
        /// Adds personal info about new registered user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public void SavePersonal(int userID, string firstName, string lastName)
        {
            if (!OpenConnection())
            {
                return;
            }
            try
            {
                SqlCommand savePersonal = new SqlCommand("sp_SavePersonal", sqlConn);
                savePersonal.CommandType = CommandType.StoredProcedure;
                savePersonal.Parameters.Add(new SqlParameter("@userID", userID));
                savePersonal.Parameters.Add(new SqlParameter("@firstName", firstName));
                savePersonal.Parameters.Add(new SqlParameter("@lastName", lastName));
                savePersonal.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
        }

        #endregion

        #region Collection methods
        /// <summary>
        /// Saves a collection with the collectionID.
        /// </summary>
        /// <param name="collectionID">ID of the collection</param>
        /// <param name="collection">xml string representing the collection</param>
        public void SaveCollection(int collectionID, string collection)
        {
            if (!OpenConnection())
            {
                return;
            }
            try
            {
                SqlCommand save = new SqlCommand("sp_SaveCollection", sqlConn);
                save.CommandType = CommandType.StoredProcedure;
                save.Parameters.Add(new SqlParameter("@collectionID", collectionID));
                save.Parameters.Add(new SqlParameter("@collectionXML", collection));
                save.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
        }
        /// <summary>
        /// Adds a new collection connected to the user by the given userID and destinationID
        /// </summary>
        /// <param name="userID">userID of the Visit Planner user</param>
        /// <param name="destinationID">destinationID of the collection being created</param>
        /// <param name="collection">xml string representing the collection to add</param>
        public void AddCollection(int userID, int destinationID, string collection)
        {
            if (!OpenConnection())
            {
                return;
            }
            try
            {
                SqlCommand add = new SqlCommand("sp_InsertCollection", sqlConn);
                add.CommandType = CommandType.StoredProcedure;
                add.Parameters.Add(new SqlParameter("@collectionXML", collection));
                add.Parameters.Add(new SqlParameter("@UserID", userID));
                add.Parameters.Add(new SqlParameter("@DestinationID", destinationID));
                add.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Get a user's collections
        /// </summary>
        /// <param name="user">Visit planner user object to get collections for</param>
        /// <returns>List of collections</returns>
        public List<VPCollection> GetCollections(VisitPlannerUser user)
        {
            List<VPCollection> savedCollections = new List<VPCollection>();
            if (!OpenConnection())
            {
                return savedCollections;
            }
            try
            {
                SqlCommand getColl = new SqlCommand("sp_GetCollections", sqlConn);
                getColl.CommandType = CommandType.StoredProcedure;
                getColl.Parameters.Add(new SqlParameter("@userID", user.UserNumber));

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(getColl);
                ad.Fill(ds);
                if (ds == null || ds.Tables == null)
                {
                    return savedCollections;
                }

         
                //must do a loop here and add all the collections that exist 
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    savedCollections.Add(ParseCollection(dr));
                }

            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return savedCollections;
        }

        /// <summary>
        /// Get the destinations
        /// </summary>
        /// <param name="user">Visit planner user object</param>
        /// <returns>List of destinations</returns>
        public List<VESilverlight.Destination> GetDestinationsByUser(VisitPlannerUser user)
        {
            List<VESilverlight.Destination> destinations = new List<VESilverlight.Destination>();
            if (!OpenConnection())
            {
                return destinations;
            }
            try
            {
                SqlCommand getColl = new SqlCommand("sp_GetDestinationsByUser", sqlConn);
                getColl.CommandType = CommandType.StoredProcedure;
                getColl.Parameters.Add(new SqlParameter("@userID", user.UserNumber));

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(getColl);
                ad.Fill(ds);
                if (ds == null || ds.Tables == null)
                {
                    return destinations;
                }


                //must do a loop here and add all the collections that exist 
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    destinations.Add(new VESilverlight.Destination((int)dr["DestinationID"], (string)dr["DestinationName"]));
                }

            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return destinations;
        }

        /// <summary>
        /// Get a single collection based on the given collectionID
        /// </summary>
        /// <param name="collectionID">ID of the collection</param>
        /// <returns>A single collection</returns>
        public VPCollection GetCollection(int collectionID)
        {
            VPCollection collection = new VPCollection();
            if (!OpenConnection())
            {
                return collection;
            }
            try
            {
                SqlCommand getColl = new SqlCommand("sp_GetCollection", sqlConn);
                getColl.CommandType = CommandType.StoredProcedure;
                getColl.Parameters.Add(new SqlParameter("@collectionID", collectionID));

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(getColl);
                ad.Fill(ds);
                if (ds == null || ds.Tables == null)
                {
                    return collection;
                }
                collection = ParseCollection(ds.Tables[0].Rows[0]);



            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return collection;
        }

        /// <summary>
        /// Parse collection from DB
        /// </summary>
        /// <param name="dr">Data row containing the collection</param>
        /// <returns>Parsed collection object</returns>
        private VPCollection ParseCollection(DataRow dr)
        {
            VPCollection newCollection = new VPCollection();
            newCollection.Collection = (string)dr["Collection"];
            newCollection.CollectionID = (int)dr["CollectionID"];
            return newCollection;
        
        }
        #endregion

        #region UserExists
        /// <summary>
        /// Check if the user exists in the system
        /// </summary>
        /// <param name="liveID">Windows live ID</param>
        /// <returns>true if the user exists</returns>
        public bool UserExists(string liveID)
        {
            bool exists = false;
            if (!OpenConnection())
            {
                return exists;
            }
            try
            {
                SqlCommand com = new SqlCommand("sp_UserExists", sqlConn);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add(new SqlParameter("@LiveID", liveID));

                SqlParameter parameterReturnValue = new SqlParameter("ReturnValue", SqlDbType.Int);
                parameterReturnValue.Direction = ParameterDirection.ReturnValue;
                com.Parameters.Add(parameterReturnValue);
                com.ExecuteNonQuery();
                
                int result = (int)com.Parameters["ReturnValue"].Value;
                if (result == 1)
                {
                    exists = true;
                }
                else
                {
                    exists = false;
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return exists;
        }

        #endregion

        #region Get and Save Concierge
        /// <summary>
        /// Get locations
        /// </summary>
        /// <returns>List of destinations</returns>
        public IList<Destination> GetLocations()
        {
            IList<Destination> retVal = new List<Destination>();
            if (!OpenConnection())
            {
                return retVal;
            }
            try
            {
                SqlCommand getLoc = new SqlCommand("sp_GetDestinations", sqlConn);
                getLoc.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(getLoc);
                ad.Fill(ds);
                if (ds == null || ds.Tables == null)
                {
                    return retVal;
                }
                foreach (DataRow collectionRow in ds.Tables[0].Rows)
                {
                    retVal.Add(new Destination((int)collectionRow["DestinationID"], (string)collectionRow["DestinationName"]));
                }
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return retVal;
        }

        public string GetConciergeByDestination(int destinationID)
        {
            string concierge = null;
            if (!OpenConnection())
            {
                return concierge;
            }
            try
            {
                SqlCommand getCon = new SqlCommand("sp_GetConciergeByDestination", sqlConn);
                getCon.CommandType = CommandType.StoredProcedure;
                getCon.Parameters.Add(new SqlParameter("@destinationID", destinationID));

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(getCon);
                ad.Fill(ds);
                if (ds == null || ds.Tables == null)
                {
                    return concierge;
                }
                DataRow collectionRow = ds.Tables[0].Rows[0];
                concierge = (string)collectionRow["ConciergeCollection"];
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return concierge;
        }

        /// <summary>
        /// Saves a concierge with destination ID
        /// </summary>
        /// <param name="collectionID">ID of the collection</param>
        /// <param name="collection">xml string representing the collection</param>
        public void SaveConcierge(int destinationID, string collection)
        {
            if (!OpenConnection())
            {
                return;
            }
            try
            {
                SqlCommand save = new SqlCommand("sp_SaveConcierge", sqlConn);
                save.CommandType = CommandType.StoredProcedure;
                save.Parameters.Add(new SqlParameter("@destinationID", destinationID));
                save.Parameters.Add(new SqlParameter("@collectionXML", collection));
                save.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
            finally
            {
                CloseConnection();
            }
        }
        #endregion

        #region OpenConnection
        /// <summary>
        /// Open connection to the DB
        /// </summary>
        /// <returns>True on success</returns>
        protected bool OpenConnection()
        {
            try
            {
                sqlConn.Open();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                return false;
            }
            return true;
        }
        #endregion

        #region CloseConnection
        /// <summary>
        /// Close the DB connection
        /// </summary>
        protected void CloseConnection()
        {
            try
            {
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
            }
        }
        #endregion

       
    }

}
