/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: UIWorkDispatcher.cs
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
using System.Collections.Generic;
using System.Windows.Browser;
using System.Windows.Threading;

namespace VESilverlight
{
    /// <summary>
    /// UIWork delegate
    /// </summary>
    public delegate void UIWorkDelegate();

    /// <summary>
    /// This Class implements a work dispatcher which is used to organize calls to methods 
    /// and helps ensure that they occure in proper sequence
    /// </summary>
    public class UIWorkDispatcher
    {
        #region Public Properties
        /// <summary>
        /// Work queue
        /// </summary>
        public Queue<UIWorkDelegate> UIWorkQueue = new Queue<UIWorkDelegate>();

        /// <summary>
        /// Timer
        /// </summary>
        private DispatcherTimer t = new DispatcherTimer();

        #endregion

        #region Public Methods

        /// <summary>
        /// Constructor which creates a delay of 500 ms
        /// </summary>
        public UIWorkDispatcher()
        {
            t.Interval = TimeSpan.FromMilliseconds(500);
            t.Tick += new EventHandler(UIWorkCallback);
            t.Start();
        }

        /// <summary>
        /// Removes a task from the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UIWorkCallback(object sender, EventArgs e)
        {

            while (UIWorkQueue.Count > 0)
            {
                UIWorkDelegate work = UIWorkQueue.Dequeue();
                work();
            }
        }

        #endregion
    }
}
