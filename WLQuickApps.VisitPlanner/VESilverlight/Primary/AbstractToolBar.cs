/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: AbstractToolBar.cs
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

namespace VESilverlight.Primary
{
    /// <summary>
    /// Represents the panel for the Concierge, My Places, and Search tabs
    /// </summary>
    public abstract class AbstractToolBar : UserControl
    {
        protected SideMenu.Tabs tab;

        public void Initialize(SideMenu.Tabs tab)
        {
            this.tab = tab;
            Controller.GetInstance().SelectTabEvent += new EventHandler<SideMenu.TabSelectEventArgs>(AbstractToolBar_SelectTabEvent);
        }

        /// <summary>
        /// Opens the panel when tab is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AbstractToolBar_SelectTabEvent(object sender, SideMenu.TabSelectEventArgs e)
        {
            if (e.Tab == tab)
            {
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
        }

    }
}
