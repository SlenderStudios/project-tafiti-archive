/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: PlaceListHover.xaml.cs
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
using System.Windows.Browser;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace VESilverlight.Secondary
{
    /// <summary>
    /// Implements hover box containing attraction image when user hovers over a 
    /// a list item in the Conciere, My Places, or Search tab
    /// </summary>
    [ScriptableType]
    public partial class PlaceListHover : Canvas
    {
        private Attraction attraction;

        /// <summary>
        /// Constructor - Registers object as scriptable
        /// </summary>
        public PlaceListHover()
		{
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("PlaceListHover", this);
            this.MouseLeave += new MouseEventHandler(PlaceListHover_MouseLeave);
            Application.Current.Host.Content.Resized += new EventHandler(BrowserHost_Resize);
        }

        #region Non-Scriptable Methods

        /// <summary>
        /// Hide popup if Window is resized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BrowserHost_Resize(object sender, EventArgs e)
        {
            if (MovePlaceListHover != null)
                MovePlaceListHover(this, new PlaceListPositionEventArgs("", -500, -500));
        }

        /// <summary>
        /// Hide popup on mouse out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PlaceListHover_MouseLeave(object sender, EventArgs e)
        {
            MovePlaceListHover(this, new PlaceListPositionEventArgs("", -500, -500));
        }

        /// <summary>
        /// Page load -  Get a handle to page controls.  Set events
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void Page_Loaded(object o, EventArgs e)
        {
            // Required to initialize variables
            InitializeComponent();
            this.MouseLeftButtonDown += new MouseButtonEventHandler(PlaceListHover_MouseLeftButtonDown);
        }

        /// <summary>
        /// Show tour popup when item is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PlaceListHover_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StopTour(this, new EventArgs());
            ShowTourPopupBySerial(this, new AttractionEventArgs(attraction.Serialize()));
        }

        #endregion

        #region Scriptable Methods

        /// <summary>
        /// Sets the image of the popup
        /// </summary>
        /// <param name="serialText"></param>
        [ScriptableMember]
        public void Initialize(string serialText)
        {
            attraction = Attraction.Deserialize(serialText);
            image.Source = new BitmapImage(new Uri(Utilities.GetAbsolutePath(attraction.ImageURL)));
        }

        #endregion

        #region Scriptable Events

        [ScriptableMember]
        public event EventHandler<PlaceListPositionEventArgs> MovePlaceListHover;

        [ScriptableMember]
        public event EventHandler StopTour;

        [ScriptableMember]
        public event EventHandler<AttractionEventArgs> ShowTourPopupBySerial;

        #endregion
    }
}