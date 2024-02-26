/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: VPCollection.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace VisitPlanner.BusinessObjects
{
    /// <summary>
    /// Represents a collection
    /// </summary>
    public class VPCollection
    {
        #region Private Properties
        /// <summary>
        /// Collection XML feed
        /// </summary>
        private string collection;

        /// <summary>
        /// Collection ID
        /// </summary>
        private int collectionID;
        #endregion

        #region Public Properties
        /// <summary>
        /// Collection XML feed
        /// </summary>
        public string Collection
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
            }
        }

        /// <summary>
        /// Collection ID
        /// </summary>
        public int CollectionID
        {
            get
            {
                return collectionID;
            }
            set
            {
                collectionID = value;
            }
        }
        #endregion

    }
}
