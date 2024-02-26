#region VPLocationManager
//
//
//
//
// 
// Filename: VPLocationManager.cs
//
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VESilverlight;
using VisitPlanner.Data;

namespace VisitPlanner.Business
{
    /// <summary>
    /// Manages Visit Planner locations
    /// </summary>
    public class VPLocationManager
    {
        #region Public Methods
        /// <summary>
        /// Gets locations from database
        /// </summary>
        public static IList<Destination> GetLocations()
        {
            DataAccess connection = new DataAccess();
            return connection.GetLocations();
        }
        #endregion
    }
}
