//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Windows;

namespace VESilverlight {

    // Defines custom exception for sample controls
    public class ControlException : Exception {
        #region Public Methods

        // The only ctor for the moment calls the base class
        public ControlException(string message)
            : base(message)
        {
        }

        #endregion Public Methods
    }
}
