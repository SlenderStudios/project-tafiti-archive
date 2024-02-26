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
    //This class implements an attraction event argument
    [ScriptableType]
    public class AttractionEventArgs : EventArgs
    {
        #region Private Properties
        /// <summary>
        /// Serialized attraction
        /// </summary>
        private string serialText;

        /// <summary>
        /// Flag to indicate logged in status
        /// </summary>
        private bool loggedIn;

        #endregion

        #region Public Methods
        /// <summary>
        /// Constructor takes serialized attraction
        /// </summary>
        /// <param name="serialText">Serialized attraction</param>
        public AttractionEventArgs(string serialText) : this(serialText, false)
        {
        }

        /// <summary>
        /// Attraction argument constructor takes serialized attraction and logged in
        /// status flag
        /// </summary>
        /// <param name="serialText">Serialized attraction</param>
        /// <param name="loggedIn">Logged in status flag</param>
        public AttractionEventArgs(string serialText, bool loggedIn)
        {
            this.loggedIn = loggedIn;
            this.serialText = serialText;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// JSON formatted string representing attraction object
        /// </summary>
        [ScriptableMember]
        public String SerialText
        {
            get { return serialText; }
            set { serialText = value; }
        }

        /// <summary>
        /// Indicator if user is logged in
        /// </summary>
        [ScriptableMember]
        public bool LoggedIn
        {
            get { return loggedIn; }
        }

        #endregion
    }
}
