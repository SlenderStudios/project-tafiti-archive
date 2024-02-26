/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: TourControl.xaml.cs
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
using System.Collections.Generic;
using System.Windows.Threading;
using System.Windows.Input;
using VESilverlight.Primary;

namespace VESilverlight.Secondary
{
    /// <summary>
    /// Implements guided tour control
    /// </summary>
    [ScriptableType]
    public partial class TourControl : Canvas
    {
        #region Properties 

        DispatcherTimer itemDisplayTimer = new DispatcherTimer();
        DispatcherTimer progressUpdateTimer = new DispatcherTimer();
        int itemsCount = 0;
        int tourIndex = 0;
        int secondsElapsed = 0;
        int tourInterval = 10000;
        double originalWidth = 0;
        bool tourInProgress = false;
        bool paused = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor - Registers object as scriptable, initializes properties
        /// </summary>
        public TourControl()
		{
            this.InitializeComponent();

            itemDisplayTimer.Interval = TimeSpan.FromMilliseconds(tourInterval);
            itemDisplayTimer.Tick += new EventHandler(TourCallback);

            progressUpdateTimer.Interval = TimeSpan.FromMilliseconds(1000);
            progressUpdateTimer.Tick += new EventHandler(progressUpdateTimer_Tick);

            HtmlPage.RegisterScriptableObject("TourControl", this);
        }

        #endregion

        #region Non-Scriptable Methods

        /// <summary>
        /// Animates tour status bar scrubber
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void progressUpdateTimer_Tick(object sender, EventArgs e)
        {          
            secondsElapsed++;

            int minutes = secondsElapsed / 60;
            int seconds = secondsElapsed % 60;

            ElapsedTime.Text = minutes.ToString() + ":";
            if (seconds < 10)
            {
                ElapsedTime.Text += "0";
            }
            ElapsedTime.Text += seconds.ToString();         
            ProgressBar.Width += (originalWidth / ((tourInterval / 1000) * itemsCount));
        }

        /// <summary>
        /// Play button - start tour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Play_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (!tourInProgress)
            {
                tourInProgress = true;
                TourCallback(this, new TourEventArgs(0));
                DisableMouseOvers(this, new EventArgs());
            }
            else if (paused)
            {
                UnpauseTour(this, new EventArgs());
                itemDisplayTimer.Interval = TimeSpan.FromMilliseconds(tourInterval - (secondsElapsed % (tourInterval / 1000)) * 1000);
                progressUpdateTimer.Start();
                paused = false;
                itemDisplayTimer.Start();
            }
        }

        /// <summary>
        /// Stop button - stop tour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Stop_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            ExitTourLogic();
        }

        /// <summary>
        /// Pause tour button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Pause_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            PauseTourLogic();
        }

        /// <summary>
        /// Called after each tour item has been displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TourCallback(object sender, EventArgs e)
        {
            if (tourIndex >= itemsCount)
            {
                itemDisplayTimer.Stop();
                progressUpdateTimer.Stop();
                tourIndex = 0;
                tourInProgress = false;
                StopTour(this, new EventArgs());
                ElapsedTime.Text = TotalTime.Text;
                EnableMouseOvers(this, new EventArgs());  //Tour Over
                return;
            }
            else if (!itemDisplayTimer.IsEnabled)
            {
                secondsElapsed = 0;

                ProgressBar.Width = 0;
                
                itemDisplayTimer.Start();
                progressUpdateTimer.Start();

                int totalDuration = itemsCount * tourInterval / 1000;

                int minutes = totalDuration / 60;
                int seconds = totalDuration % 60;

                TotalTime.Text = minutes.ToString() + ":";
                if (seconds < 10)
                {
                    TotalTime.Text += "0";
                }
                TotalTime.Text += seconds.ToString();
            }

            itemDisplayTimer.Stop();
            itemDisplayTimer.Interval = TimeSpan.FromMilliseconds(tourInterval);
            itemDisplayTimer.Start();

            ShowTour(this, new TourEventArgs(tourIndex));

            tourIndex++;
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

            Play.MouseLeftButtonDown += new MouseButtonEventHandler(Play_MouseLeftButtonDown);
            Pause.MouseLeftButtonDown += new MouseButtonEventHandler(Pause_MouseLeftButtonDown);
            Stop.MouseLeftButtonDown += new MouseButtonEventHandler(Stop_MouseLeftButtonDown);
            Forward.MouseLeftButtonDown += new MouseButtonEventHandler(Forward_MouseLeftButtonDown);
            Backward.MouseLeftButtonDown += new MouseButtonEventHandler(Backward_MouseLeftButtonDown);
            originalWidth = ProgressBar.Width;
            ProgressBar.Width = 0;
        }

        /// <summary>
        /// Rewind tour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Backward_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tourInProgress){              
                if (paused)
                {
                    paused = false;
                    progressUpdateTimer.Start();
                }

                //StopTour(this, new EventArgs());
                tourIndex-=2;
                if (tourIndex < 0)
                {
                    tourIndex = 0;
                }

                secondsElapsed = tourIndex * tourInterval / 1000;
                ProgressBar.Width = (originalWidth / itemsCount) * tourIndex;

                itemDisplayTimer.Stop();
                itemDisplayTimer.Interval = TimeSpan.FromMilliseconds(10);
                itemDisplayTimer.Start();
            }
        }

        /// <summary>
        /// Fast forward tour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Forward_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (tourInProgress)
            {
                if (tourIndex >= itemsCount)
                {
                    return;
                }
                
                //StopTour(this, new EventArgs());
                if (paused)
                {
                    paused = false;
                    progressUpdateTimer.Start();
                }

                secondsElapsed = tourIndex * tourInterval / 1000;
                ProgressBar.Width = (originalWidth / itemsCount) * tourIndex;

                itemDisplayTimer.Stop();
                itemDisplayTimer.Interval = TimeSpan.FromMilliseconds(10);
                itemDisplayTimer.Start();
            }
        }

        #endregion

        #region Scriptable Methods

        /// <summary>
        /// Stops guided tour
        /// </summary>
        [ScriptableMember]
        public void ExitTourLogic()
        {
            if (tourInProgress)
            {
                itemDisplayTimer.Stop();
                progressUpdateTimer.Stop();
                ElapsedTime.Text = "0:00";
                ProgressBar.Width = 0;
                tourIndex = 0;
                tourInProgress = false;
                paused = false;

                StopTour(this, new EventArgs());
            }
            EnableMouseOvers(this, new EventArgs());
            StopTour(this, new EventArgs());
        }

        /// <summary>
        /// Pauses guided tour
        /// </summary>
        [ScriptableMember]
        public void PauseTourLogic()
        {
            if (tourInProgress && !paused)
            {
                itemDisplayTimer.Stop();
                progressUpdateTimer.Stop();
                paused = true;
            }
        }

        /// <summary>
        /// Implements tour event arguments
        /// </summary>
        [ScriptableType]
        public class TourEventArgs : EventArgs
        {
            private int index;

            public TourEventArgs(int idx)
            {
                index = idx;
            }

            [ScriptableMember]
            public int Index
            {
                get { return index; }
            }
        }

        /// <summary>
        /// Resize tour control on a browswer resize
        /// </summary>
        /// <param name="width"></param>
        [ScriptableMember]
        public void Resize(int width)
        {
            this.Width = width;
            Background.Width = width;
        }

        [ScriptableMember]
        public void InitializeWithoutCount()
        {
            Controller.GetInstance().GetAttractionsCount(
                delegate(int conciergeCount)
                {
                    this.Initialize(conciergeCount);
                });
        }

        /// <summary>
        /// Initialize tour control
        /// </summary>
        /// <param name="conciergeCount"></param>
        [ScriptableMember]
        public void Initialize(int conciergeCount)
        {
            if (itemsCount != 0) ExitTourLogic();
            //TODO: update itemsCount dynamically whenever an item is added/removed
            //Also, when destination is changed
            itemsCount = conciergeCount;
        }

        /// <summary>
        /// Start tour
        /// </summary>
        [ScriptableMember]
        public void StartTour()
        {
            RoutedEventArgs args = new RoutedEventArgs();
            Play_MouseLeftButtonDown(this, (MouseEventArgs)args);
        }

        #endregion

        #region Scriptable Events

        [ScriptableMember]
        public event EventHandler<TourEventArgs> ShowTour;

        [ScriptableMember]
        public event EventHandler StopTour;

        [ScriptableMember]
        public event EventHandler PauseTour;

        [ScriptableMember]
        public event EventHandler DisableMouseOvers;

        [ScriptableMember]
        public event EventHandler EnableMouseOvers;

        [ScriptableMember]
        public event EventHandler UnpauseTour;

        #endregion

    }
}
