//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace VESilverlight {

    // Implements a RepeatButton control. This is a button that sends Click event
    // repeatedly while the MouseButton is pressed. Uses Annimation StoryBoard for 
    // triggering repeated events
    public partial class RepeatButton : UserControl {

        // Fired when the mouse is up on this control
        public event EventHandler Click;

        #region Public Methods

        // Default RepeatButton ctor - initialize brushes and find the storyboard
        public RepeatButton()
        {
            this.InitializeComponent();

            /*((SolidColorBrush)NormalBrush).Color = Color.FromArgb(0xFF, 0xB0, 0xE0, 0xE6);
            ((SolidColorBrush)HighlightBrush).Color = Color.FromArgb(0xFF, 0xAD, 0xD8, 0xE6);*/

            //Annimation StoryBoard is used for triggering repeating events
            //storyboard = this.storyboard;// FindName("storyboard") as Storyboard;
        }

        #endregion Public Methods

        #region Protected Methods

        // Sets the ButtonPressed flag to false and starts the story board for repeat
        protected void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs args)
        {
            //start timer
            if (storyboard != null) {
                startCount = 0;
                storyboard.Begin();
            }
        }

        // Sets the ButtonPressed flag to false and stops the storyboard
        protected void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            //release timer
            if (storyboard != null) {
                storyboard.Stop();
            }
        }

        #endregion Protected Methods


        #region Private Methods

        // Storyboard callback - used to implement repeating events
        private void OnRepeat(object sender, EventArgs args)
        {
            //we have some delay in the beginning
            if (startCount++ >= maxStartCount)
            {
                EventHandler handler = this.Click;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            else
            {
                storyboard.Begin();
            }
        }

        #endregion Private Methods

        #region Data

        private int startCount = 0;     // wait some time before the first event
        private const int maxStartCount = 5; //maximum number of start loops for Click event

        #endregion Data
    }
}
