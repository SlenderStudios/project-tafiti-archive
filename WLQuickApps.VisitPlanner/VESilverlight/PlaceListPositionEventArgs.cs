/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: AttractionEventArgs.cs
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

namespace VESilverlight
{
    /// <summary>
    /// This class implements an attraction event argument for the event of mousing over 
    /// Items in the concierge list
    /// </summary>
    [ScriptableType]
    public class PlaceListPositionEventArgs : AttractionEventArgs
    {
        #region Private Properties
        /// <summary>
        /// X position
        /// </summary>
        private int x;

        /// <summary>
        /// Y position
        /// </summary>
        private int y;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for position 
        /// </summary>
        /// <param name="serialText">JSON string representing attraction</param>
        /// <param name="x">x pixel coord of popup box</param>
        /// <param name="y">y pixel coord of popup box</param>
        public PlaceListPositionEventArgs(string serialText, int x, int y) : base(serialText)
        {
            this.x = x;
            this.y = y;
        }

        #endregion 

        #region Public Properties

        /// <summary>
        /// X pixel coordinate of left edge of popup display
        /// </summary>
        [ScriptableMember]
        public int X
        {
            get { return x; }
        }

        /// <summary>
        /// Y pixel coordinate of top edge of popup display
        /// </summary>
        [ScriptableMember]
        public int Y
        {
            get { return y; }
        }

        #endregion
    }
}
