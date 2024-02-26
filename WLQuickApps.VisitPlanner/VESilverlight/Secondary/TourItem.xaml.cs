/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: TourItem.xaml.cs
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
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using VESilverlight.Primary;

namespace VESilverlight.Secondary
{
    /// <summary>
    /// Implements tour popup item
    /// </summary>
    [ScriptableType]
    public partial class TourItem : Canvas
    {

        #region Properties

        Attraction attraction;
        Attraction homeAttraction;

        private bool displayed = false;

        private bool loggedIn = false;
        private bool is3D = false;

        #endregion


        /// <summary>
        /// Constructor - Registers object as scriptable
        /// </summary>
		public TourItem()
		{
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("TourItem", this);
        }

        #region Non-Scriptable Methods

        /// <summary>
        /// Sets the data in the popup box according to attraction details
        /// </summary>
        private void UpdateData()
        {
            DescriptionText.Text = attraction.LongDescription;
            TitleText.Text = attraction.Title.ToUpper();
            TypeText.Text = attraction.Category.ToString("g");
            if (attraction.Category == Attraction.Categories.Misc)
                TypeText.Text = "Miscellaneous";


            switch (attraction.Category)
            {
                case Attraction.Categories.Food:
                    Category.Source = new BitmapImage(new Uri("/images/food.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Movie:
                    Category.Source = new BitmapImage(new Uri("/images/movie.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Music:
                    Category.Source = new BitmapImage(new Uri("/images/music.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Art:
                    Category.Source = new BitmapImage(new Uri("/images/other.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Search:
                    Category.Source = new BitmapImage(new Uri("/images/Btn_PushPin_searchSaved.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Accomodation:
                    Category.Source = new BitmapImage(new Uri("/images/hotel_sym_yellow.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Custom:
                    Category.Source = new BitmapImage(new Uri("/images/Btn_PushPin_searchSaved.png", UriKind.Relative));
                    break;
                case Attraction.Categories.Misc:
                    Category.Source = new BitmapImage(new Uri("/images/Btn_PushPin_search.png", UriKind.Relative));
                    break;
            }

            AddressLine1.Text = attraction.AddressLine1;
            AddressLine2.Text = attraction.AddressLine2;
            image.Source = new BitmapImage(new Uri(Utilities.GetAbsolutePath(attraction.ImageURL)));

            if (!string.IsNullOrEmpty(attraction.VideoURL))
            {
                video.Source = new Uri(Utilities.GetAbsolutePath(attraction.VideoURL));
                video.Stop();
            }
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

            Hide.Completed += new EventHandler(Hide_Completed);
            Hide3D.Completed +=new EventHandler(Hide_Completed);
           

            Show.Completed += new EventHandler(Show_Completed);
            Show3D.Completed += new EventHandler(Show_Completed);

            Mail.Source = new BitmapImage(new Uri("/images/Mail.png", UriKind.Relative));

            Print.Source = new BitmapImage(new Uri("/images/Print.png", UriKind.Relative));
            Print.MouseLeftButtonDown += new MouseButtonEventHandler(Print_MouseLeftButtonDown);

            AddToMyDay.Source = new BitmapImage(new Uri("/images/AddToMyDay.png", UriKind.Relative));
            Directions.Source = new BitmapImage(new Uri("/images/Directions.png", UriKind.Relative));
            VideoIcon.Source = new BitmapImage(new Uri("/images/Video.png", UriKind.Relative));

            VideoIcon.MouseLeftButtonDown += new MouseButtonEventHandler(playButton_MouseLeftButtonDown);
            Exit.MouseLeftButtonDown += new MouseButtonEventHandler(exitButton_MouseLeftButtonDown);

            VideoPlayButton.MouseLeftButtonDown += new MouseButtonEventHandler(VideoPlayButton_MouseLeftButtonDown);
            VideoPauseButton.MouseLeftButtonDown += new MouseButtonEventHandler(VideoPauseButton_MouseLeftButtonDown);
            VideoStopButton.MouseLeftButtonDown += new MouseButtonEventHandler(VideoStopButton_MouseLeftButtonDown);
            VideoMuteButton.MouseLeftButtonDown += new MouseButtonEventHandler(VideoMuteButton_MouseLeftButtonDown);

            Mail.MouseLeftButtonDown += new MouseButtonEventHandler(Mail_MouseLeftButtonDown);

            AddToMyDay.MouseLeftButtonDown += new MouseButtonEventHandler(AddToMyDay_MouseLeftButtonDown);

            Directions.MouseLeftButtonDown += new MouseButtonEventHandler(Directions_MouseLeftButtonDown);

        }

        /// <summary>
        /// Get Directions button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Directions_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            RetrieveDirections(this, new DirectionsEventArgs(homeAttraction,attraction));
        }

        /// <summary>
        /// Add to my day button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddToMyDay_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (loggedIn)
            {
                AddToItinerary(this, new AttractionEventArgs(attraction.Serialize(), loggedIn));
                ExitTour(this, new EventArgs());
            }
        }

        /// <summary>
        /// Email button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Mail_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (!loggedIn)
            {

                HtmlPage.Window.Navigate(new Uri("mailto:?subject=Check out the Visit Planner at " + System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString()));
            }
            else
            {
                ShowShareDialog(this, new EventArgs());
            }
        }

        /// <summary>
        /// Print page button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Print_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            //Manual URL encode since there is no reference to system.web 
            string urlString = attraction.Serialize().Replace("&", "%26");
            HtmlPage.Window.Navigate(new Uri(Utilities.GetAbsolutePath("PrintAttraction.aspx?ast=" + urlString)), "_blank");
        }

        /// <summary>
        /// Video Play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void playButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            VideoPanel.Visibility = Visibility.Visible;
            VideoPanel.Opacity = 1;
            if (video.Position.Milliseconds == 0)
                VideoPopup.Begin();
            VideoPopup.Completed +=new EventHandler(VideoPopup_Completed);
            PauseTour(this, new EventArgs());
            video.MediaEnded += new RoutedEventHandler(video_MediaEnded);
        }

        /// <summary>
        /// Popup close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void exitButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            ExitTour(this, new EventArgs());
        }

        /// <summary>
        /// When guided tour item has popped up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Show_Completed(object sender, EventArgs e)
        {   
            ThreadCompletionCallback(this, new EventArgs());
        }

        /// <summary>
        /// When guided tour item has closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Hide_Completed(object sender, EventArgs e)
        {
            video.Stop();
            
            MoveTourItemControl(this, new MoveEventArgs(-400, -400));
            ThreadCompletionCallback(this, new EventArgs());
        }

        /// <summary>
        /// When video has ended
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void video_MediaEnded(object sender, RoutedEventArgs e)
        {
            video.Stop();
            VideoPopdown.Begin();
            ImagePanel.Visibility = Visibility.Visible;
            VideoPopdown.Completed += new EventHandler(VideoPopdown_Completed);
        }

        /// <summary>
        /// When video media player show animation has completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VideoPopup_Completed(object sender, EventArgs e)
        {
            ImagePanel.Visibility = Visibility.Collapsed;
            video.Play();
        }

        /// <summary>
        /// When video media player hide animation has complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VideoPopdown_Completed(object sender, EventArgs e)
        {
            VideoPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Video Play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VideoPlayButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            video.Play();
        }


        /// <summary>
        /// Video stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VideoStopButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            video.Stop();
            VideoPopdown.Begin();
            ImagePanel.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Video pause
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VideoPauseButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            video.Pause();
        }

        /// <summary>
        /// Video mute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void VideoMuteButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (video.Volume == 0)
            {
                video.Volume = 1;
                VideoMuteButton.Opacity = 1;
            }
            else
            {
                video.Volume = 0;
                VideoMuteButton.Opacity = 0.5;
            }
        }

        #endregion

        #region Scriptable Methods

        /// <summary>
        /// Close popup
        /// </summary>
        [ScriptableMember]
        public void HideItem()
        {
            if (displayed)
            {
                if (is3D)
                    Hide3D.Begin();
                else
                    Hide.Begin();
                displayed = false;
            }
            else
            {
                ThreadCompletionCallback(this, new EventArgs());
            }
        }

        /// <summary>
        /// Show popup
        /// </summary>
        /// <param name="serialText"></param>
        /// <param name="homeSerialText"></param>
        /// <param name="loggedIn"></param>
        [ScriptableMember]
        public void ShowItem(string serialText, bool loggedIn)
        {
            Controller.GetInstance().GetHomeSerial(
                delegate(string homeSerialText)
                {
                    MoveTourItemControl(this, new MoveEventArgs());
                    this.loggedIn = loggedIn;
                    this.attraction = Attraction.Deserialize(serialText);
                    this.homeAttraction = Attraction.Deserialize(homeSerialText);

                    if (attraction.ID == homeAttraction.ID)
                    {
                        Directions.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        Directions.Visibility = Visibility.Visible;
                    }

                    if (loggedIn)
                    {
                        AddToMyDay.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        AddToMyDay.Visibility = Visibility.Collapsed;
                    }
                    if (attraction.VideoURL == string.Empty)
                    {
                        VideoIcon.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        VideoIcon.Visibility = Visibility.Visible;
                    }
                    UpdateData();

                    ImagePanel.Visibility = Visibility.Visible;
                    VideoPanel.Visibility = Visibility.Collapsed;

                    displayed = true;
                    if (is3D)
                        Show3D.Begin();
                    else
                        Show.Begin();
                    DisableMouseOvers(this, new EventArgs());
                });
        }

        /// <summary>
        /// Tour Move event arguments class
        /// </summary>
        [ScriptableType]
        public class MoveEventArgs : EventArgs
        {
            private bool center = true;

            public MoveEventArgs()
            {

            }
            
            public MoveEventArgs(int left, int top)
            {
                center = false;
                this.left = left;
                this.top = top;
            }

            [ScriptableMember]
            public int Left
            {
                get { return this.left; }
            }

            private int left;

            [ScriptableMember]
            public int Top
            {
                get { return this.top; }
            }
            private int top;

            [ScriptableMember]
            public bool Center
            {
                get { return this.center; }
            }

        }

        /// <summary>
        /// Sets is3d property
        /// </summary>
        [ScriptableMember]
        public void Set3D(bool value)
        {
            is3D = value;
        }

        #endregion

        #region Scriptable Events

        [ScriptableMember]
        public event EventHandler<MoveEventArgs> MoveTourItemControl;

        [ScriptableMember]
        public event EventHandler ThreadCompletionCallback;

        [ScriptableMember]
        public event EventHandler PauseTour;

        [ScriptableMember]
        public event EventHandler ExitTour;

        [ScriptableMember]
        public event EventHandler DisableMouseOvers;

        [ScriptableMember]
        public event EventHandler ShowShareDialog;

        [ScriptableMember]
        public event EventHandler<AttractionEventArgs> AddToItinerary;

        [ScriptableMember]
        public event EventHandler<DirectionsEventArgs> RetrieveDirections;

        #endregion

    }
}