/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: VisitPlannerUser.cs
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


namespace VisitPlanner.BusinessObjects
{
    /// <summary>
    /// Represents a Visit Planner User
    /// </summary>
    public class VisitPlannerUser
    {

        #region Private Properties

        /// <summary>
        /// Windows live ID
        /// </summary>
        private string liveID;

        /// <summary>
        /// Visit Planner ID
        /// </summary>
        private int userNumber;

        /// <summary>
        /// User collection id's by Destination
        /// </summary>
        private IDictionary<int, List<int>> destinationCollectionList;
        
        /// <summary>
        /// User type
        /// </summary>
        private string userType;

        /// <summary>
        /// User First Name
        /// </summary>
        private string firstName;

        /// <summary>
        /// User Last Name
        /// </summary>
        private string lastName;

        #endregion

        #region Public Properties
        /// <summary>
        /// Windows live ID
        /// </summary>
        public string LiveID
        {
            get
            {
                return liveID;
            }
            set
            {
                liveID = value;
            }
        }
        /// <summary>
        /// Visit Planner ID
        /// </summary>
        public int UserNumber
        {
            get
            {
                return userNumber;
            }
            set
            {
                userNumber = value;
            }
        }
        /// <summary>
        /// User collection IDs
        /// </summary>
        public IDictionary<int, List<int>> DestinationCollectionList
        {
            get
            {
                return destinationCollectionList;
            }
            set
            {
                destinationCollectionList = value;
            }
        }

        /// <summary>
        /// User type
        /// </summary>
        public string UserType
        {
            get 
            {
                return userType;
            }
            set
            {
                userType = value;
            }
        
        }

        /// <summary>
        /// User First Name
        /// </summary>
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }

        }

        /// <summary>
        /// User Last Name
        /// </summary>
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Default constructor
        /// </summary>
        public VisitPlannerUser() : this (string.Empty)
        {
        }
        /// <summary>
        /// Constructor takes live ID
        /// </summary>
        /// <param name="liveLoginID"></param>
        public VisitPlannerUser(string liveLoginID)
        {
            liveID = liveLoginID;
            userNumber = -1;
            destinationCollectionList = new Dictionary<int, List<int>>();
        }

        #endregion

    }
}
