/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: DestinationListItem.xaml.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VESilverlight;
using System.Windows.Input;

namespace VESilverlight.Primary
{
    /// <summary>
    /// Implements the control that displays the various in which a user has a collection
    /// save for - My Places destination selection control
    /// </summary>
    public partial class DestinationListItem : UserControl
    {
        private Destination destination;

        /// <summary>
        /// Constructor - initializes events and controls
        /// </summary>
        /// <param name="dest"></param>
        public DestinationListItem(Destination dest)
		{
            this.InitializeComponent();

            this.MouseLeftButtonDown += new MouseButtonEventHandler(DestinationListItem_MouseLeftButtonDown);

            this.destination = dest;

            titleText.Text = destination.Name;
		}

        /// <summary>
        /// Display the collections of the clicked destination
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DestinationListItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Controller.GetInstance().SelectDestination(destination.ID, destination.Name);
        }
    }
}