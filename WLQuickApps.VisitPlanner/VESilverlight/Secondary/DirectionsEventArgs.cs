/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: DirectionEventArgs.cs
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
using System.Windows.Browser;

namespace VESilverlight.Secondary
{
    /// <summary>
    /// This class implements an argument parameter for an attraction when directions
    /// are requested
    /// </summary>
    [ScriptableType]
    public class DirectionsEventArgs : EventArgs
    {
        #region Private Properties

        private Attraction start;
        private Attraction end;

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="home">Starting Location</param>
        /// <param name="destination">Destination location</param>
        public DirectionsEventArgs(Attraction home, Attraction destination)
        {
            start = home;
            end = destination;
        }

        /// <summary>
        /// Title of the starting point
        /// </summary>
        [ScriptableMember]
        public string StartTitle
        {
            get
            {
                return start.Title;
            }
        }

        /// <summary>
        /// Address of the starting point
        /// </summary>
        [ScriptableMember]
        public string StartAddress
        {
            get
            {
                string retVal = start.AddressLine1;
                if (!retVal.EndsWith(",")) retVal += ",";
                retVal += " " + start.AddressLine2;
                return retVal;
            }
        }

        /// <summary>
        /// Latitude coordinate of the starting point
        /// </summary>
        [ScriptableMember]
        public double StartLatitude
        {
            get
            {
                return start.Latitude;
            }
        }

        /// <summary>
        /// Longitude coordinate of the starting point
        /// </summary>
        [ScriptableMember]
        public double StartLongitude
        {
            get
            {
                return start.Longitude;
            }
        }

        /// <summary>
        /// Title of the destination 
        /// </summary>
        [ScriptableMember]
        public string EndTitle
        {
            get
            {
                return end.Title;
            }
        }

        /// <summary>
        /// Address of the destination
        /// </summary>
        [ScriptableMember]
        public string EndAddress
        {
            get
            {
                string retVal = start.AddressLine1;
                if (!retVal.EndsWith(",")) retVal += ",";
                retVal += " " + start.AddressLine2;
                return retVal;
            }
        }

        /// <summary>
        /// Latitude coordinate of the destination
        /// </summary>
        [ScriptableMember]
        public double EndLatitude
        {
            get
            {
                return end.Latitude;
            }
        }

        /// <summary>
        /// Longitude coordinate of the destination
        /// </summary>
        [ScriptableMember]
        public double EndLongitude
        {
            get
            {
                return end.Longitude;
            }
        }

        #endregion
    }
}
