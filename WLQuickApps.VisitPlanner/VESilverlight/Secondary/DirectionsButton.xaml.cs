/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: DirectionsButton.xaml.cs
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
using System.Windows.Browser;
using VESilverlight.Primary;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace VESilverlight.Secondary
{
    /// <summary>
    /// Implements the driving directions button
    /// </summary>
    [ScriptableType]
    public partial class DirectionsButton : Canvas
    {
        private bool active = false;

        /// <summary>
        /// Constructor - registers object as scriptable
        /// </summary>
        public DirectionsButton()
		{
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("DirectionsButton", this);
    	}
        
        /// <summary>
        /// Animation for button mouse out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DirectionsButton_MouseLeave(object sender, EventArgs e)
        {
            ButtonUp.Visibility = Visibility.Visible;
            ButtonDown.Visibility = Visibility.Collapsed;
        }
        
        /// <summary>
        /// When direction button is pressed, show the directions dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DirectionsButton_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (ButtonDown.Visibility == Visibility.Visible)
            {
                ButtonUp.Visibility = Visibility.Visible;
                ButtonDown.Visibility = Visibility.Collapsed;
                ShowDirectionsDialog(this, new EventArgs());
            }
        }
        
        /// <summary>
        /// When down arrows are pressed, show the directions dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DownArrow_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            ShowDirectionsDialog(this, new EventArgs());
        }
        
        /// <summary>
        /// Animate the button when it is pressed down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DirectionsButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (ButtonUp.Visibility == Visibility.Visible)
            {
                ButtonDown.Visibility = Visibility.Visible;
                ButtonUp.Visibility = Visibility.Collapsed;
            }
        }
        
        /// <summary>
        /// Page load - Get a handle to page controls.  Set events
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void Page_Loaded(object o, EventArgs e)
        {
            // Required to initialize variables
            InitializeComponent();

            directionsButton.MouseLeftButtonDown += new MouseButtonEventHandler(DirectionsButton_MouseLeftButtonDown);
            directionsButton.MouseLeftButtonUp += new MouseButtonEventHandler(DirectionsButton_MouseLeftButtonUp);
            directionsButton.MouseLeave += new MouseEventHandler(DirectionsButton_MouseLeave);
            DownArrow1.MouseLeftButtonDown += new MouseButtonEventHandler(DownArrow_MouseLeftButtonDown);
            DownArrow2.MouseLeftButtonDown += new MouseButtonEventHandler(DownArrow_MouseLeftButtonDown);
            DownArrow1.Cursor = Cursors.Hand;
            DownArrow2.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Sets the button state to active
        /// </summary>
        /// <param name="active"></param>
        [ScriptableMember]
        public void SetActive(bool active)
        {
            this.active = active;
        }

        [ScriptableMember]
        public event EventHandler ShowDirectionsDialog;
    }
}