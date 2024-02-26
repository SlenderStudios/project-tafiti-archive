/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: AttractionDropBox.xaml.cs
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
using System.Collections.Generic;
using System.Windows.Input;

namespace VESilverlight.Secondary
{
    /// <summary>
    /// Implements directions dialog box
    /// </summary>
    [ScriptableType]
    public partial class DirectionsDialog : Canvas
    {
        #region Private Properties

        private bool active = false;
        private Attraction home;
        private Attraction dest = null;
        private List<Attraction> allAttractions;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor - Registers object as scriptable
        /// </summary>
        public DirectionsDialog()
		{
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("DirectionsDialog", this);
        }

        #endregion

        #region Non-Scriptable Methods

        /// <summary>
        /// Mouseout button animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DirectionsButton_MouseLeave(object sender, EventArgs e)
        {
            ButtonUp.Visibility = Visibility.Visible;
            ButtonDown.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Close dialog box 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DirectionsButton_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (ButtonDown.Visibility == Visibility.Visible)
            {
                ButtonUp.Visibility = Visibility.Visible;
                ButtonDown.Visibility = Visibility.Collapsed;
                Close(this, new EventArgs());
            }
        }

        /// <summary>
        /// Button down animation
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
        /// Button mouse out animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GetDirectionsButton_MouseLeave(object sender, EventArgs e)
        {
            GetButtonUp.Visibility = Visibility.Visible;
            GetButtonDown.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Retrieves directions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GetDirectionsButton_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (GetButtonDown.Visibility == Visibility.Visible)
            {
                GetButtonUp.Visibility = Visibility.Visible;
                GetButtonDown.Visibility = Visibility.Collapsed;

                if (dest != null){

                    Close(this, new EventArgs());

                    RetrieveDirections(this, new DirectionsEventArgs(home,dest));
                }
            }
        }

        /// <summary>
        /// Close dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DownArrow_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Close(this, new EventArgs());
        }

        /// <summary>
        /// Button down animation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GetDirectionsButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (GetButtonUp.Visibility == Visibility.Visible)
            {
                GetButtonDown.Visibility = Visibility.Visible;
                GetButtonUp.Visibility = Visibility.Collapsed;
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

            getDirectionsButton.MouseLeftButtonDown += new MouseButtonEventHandler(GetDirectionsButton_MouseLeftButtonDown);
            getDirectionsButton.MouseLeftButtonUp += new MouseButtonEventHandler(GetDirectionsButton_MouseLeftButtonUp);
            getDirectionsButton.MouseLeave += new MouseEventHandler(GetDirectionsButton_MouseLeave);
        }

        /// <summary>
        /// When user clicks on a place list item in the dialog box.  This item
        /// becomes the destination for the driving directions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            Attraction attraction = ((DirectionsPlaceListItem)sender).GetAttraction();
            EndText.Text = attraction.Title;
            dest = attraction;
        }

        #endregion

        #region Scriptable Methods

        /// <summary>
        /// Activates dialog box when user clicks on directions button
        /// </summary>
        /// <param name="active"></param>
        /// <param name="homeSerial"></param>
        /// <param name="allSerial"></param>
        [ScriptableMember]
        public void SetActive(bool active)
        {
            Controller.GetInstance().GetHomeSerial(
                delegate(string homeSerial)
                {
                    Controller.GetInstance().GetDisplayedAttractionsSerial(
                        delegate(string allSerial)
                        {
                            this.active = active;

                            if (active)
                            {
                                home = Attraction.Deserialize(homeSerial);

                                dest = null;

                                StartText.Text = home.Title;

                                EndText.Text = "(Pick a Destination)";

                                string[] allParts = allSerial.Split('|');

                                Repeater.Items.Clear();

                                foreach (string part in allParts)
                                {
                                    if (part == "") break;
                                    Attraction att = Attraction.Deserialize(part);

                                    if (att.ID != home.ID)
                                    {
                                        DirectionsPlaceListItem item = new DirectionsPlaceListItem(att);
                                        item.MouseLeftButtonDown += new MouseButtonEventHandler(item_MouseLeftButtonDown);
                                        Repeater.Items.Add(item);
                                    }
                                }

                                Repeater.UpdateItems();
                            }
                        });
                });
        }

        #endregion

        #region Scriptable Events

        [ScriptableMember]
        public event EventHandler Close;        

        [ScriptableMember]
        public event EventHandler<DirectionsEventArgs> RetrieveDirections;

        #endregion
    }
}
