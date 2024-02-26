/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: TourShareControl.xaml.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Browser;
using System.Windows.Shapes;
using System.Windows.Input;

namespace VESilverlight.Primary
{
    /// <summary>
    /// Implements visit planner sharing (email and IM) panel
    /// </summary>
    [ScriptableType]
    public partial class TourShareControl : UserControl
    {
        #region Private Properties

        private bool tourControlShown = false;

        private static TourShareControl scriptableInstance = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Get a handle to page controls.  Set 
        /// events
        /// </summary>
        public TourShareControl()
		{
            this.InitializeComponent();

            if (scriptableInstance == null)
            {
                scriptableInstance = this;
                HtmlPage.RegisterScriptableObject("TourShareControl", this);
            }

            Controller.GetInstance().StopTourEvent += new EventHandler(TourShareControl_StopTourEvent);

            TourCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(TourCanvas_MouseLeftButtonDown);
            TourCanvas.MouseEnter += new MouseEventHandler(TourCanvas_MouseEnter);
            TourCanvas.MouseLeave += new MouseEventHandler(TourCanvas_MouseLeave);

            Controller.GetInstance().LoginEvent += new EventHandler(TourShareControl_LoginEvent);

            IMButton.MouseLeftButtonDown += new MouseButtonEventHandler(IMButton_MouseLeftButtonDown);
            EmailButton.MouseLeftButtonDown += new MouseButtonEventHandler(EmailButton_MouseLeftButtonDown);

            ShareCanvas.MouseEnter += new MouseEventHandler(ShareCanvas_MouseEnter);
            ShareCanvas.MouseLeave += new MouseEventHandler(ShareCanvas_MouseLeave);
        }

        #endregion

        #region Methods

        void ShareCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            ShareToolTip.Visibility = Visibility.Visible;
            ShareToolTipShow.Begin();
        }

        void ShareCanvas_MouseLeave(object sender, EventArgs e)
        {
            ShareToolTip.Visibility = Visibility.Collapsed;
        }

        void TourCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            TourToolTip.Visibility = Visibility.Visible;
            TourToolTipShow.Begin();
        }

        void TourCanvas_MouseLeave(object sender, EventArgs e)
        {
            TourToolTip.Visibility = Visibility.Collapsed;
        }

        void IMButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            scriptableInstance.ShowShareDialog(this, new EventArgs());
        }


        void EmailButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("mailto:?subject=Check out the Visit Planner at " + HtmlPage.Document.DocumentUri.ToString()));
        }

        void TourShareControl_LoginEvent(object sender, EventArgs e)
        {
            IMButton.Visibility = Visibility.Visible;
        }

        void TourShareControl_StopTourEvent(object sender, EventArgs e)
        {
            if (tourControlShown)
            {
                TourText.Foreground = new SolidColorBrush(Color.FromArgb(0, 0xEA, 0xE6, 0xB6));
                tourControlShown = false;
            }
        }

        void TourCanvas_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (tourControlShown)
            {
                Controller.GetInstance().StopTour();
            }
            else
            {
                scriptableInstance.ShowTourControl(this, new EventArgs());
                TourText.Foreground = new SolidColorBrush(Color.FromArgb(0, 0x8e, 0xab, 0xbd));
                tourControlShown = true;
            }
        }

        #endregion

        #region Scriptable Events

        [ScriptableMember]
        public event EventHandler ShowTourControl;

        [ScriptableMember]
        public event EventHandler ShowShareDialog;

        #endregion

    }
}