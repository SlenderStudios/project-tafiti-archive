/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: BusinessUserManager.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VisitPlanner.Data;
using VisitPlanner.BusinessObjects;
using System.Xml;

namespace VisitPlanner.Business
{
    /// <summary>
    /// Handles Visit Planner user 
    /// </summary>
    public class VPUserManager
    {
        #region Public Methods

        /// <summary>
        /// Get a user by the Windows live token
        /// </summary>
        /// <param name="liveID">Windows Live ID (token)</param>
        /// <returns>Visit Planner User object</returns>
        public static VisitPlannerUser GetUser(string liveID)
        {
            VisitPlannerUser user = new VisitPlannerUser();
            DataAccess connection = new DataAccess();
            if (!connection.UserExists(liveID))
            {
                user.LiveID = liveID;
                user = connection.AddNewUser(user);                
            }
            else
            {
                user = connection.GetUserByLiveID(liveID);
            }
            return user;
        }

        /// <summary>
        /// Get user by user ID
        /// </summary>
        /// <param name="userID">Visit Planner User ID</param>
        /// <returns>Visit Planner User object</returns>
        public static VisitPlannerUser GetUser(int userID)
        {
            DataAccess connection = new DataAccess();
            return connection.GetUser(userID);
        }

        /// <summary>
        /// Get user collections from DB
        /// </summary>
        /// <param name="user">Visit Planner User object</param>
        /// <returns>List of Visit Planner collection objects</returns>
        public static List<VPCollection> GetUserCollections(VisitPlannerUser user)
        {
            DataAccess connection = new DataAccess();
            return connection.GetCollections(user);
        }

        /// <summary>
        /// Get user destinations from DB
        /// </summary>
        /// <param name="user">Visit Planner User object</param>
        /// <returns>List of Destination objects</returns>
        public static List<VESilverlight.Destination> GetUserDestinations(VisitPlannerUser user)
        {
            DataAccess connection = new DataAccess();
            return connection.GetDestinationsByUser(user);
        }

        #endregion
    }
}
