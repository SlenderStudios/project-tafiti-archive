
//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Resources;
using System.Windows.Media.Animation;

namespace VESilverlight
{

    // Implements a Button control. The button gets highlighted when the mouse is 
    // over and changes its outline color when the left mouse button is down. It also sends
    // Click event when the left mouse button is up. The Height, Widht and Text of the 
    // Button can be set by the Application
    public class Button : ButtonBase
    {

        #region Public Methods

        // Default Button ctor - does nothing
        public Button()
        {
            text = (TextBlock)FindName("text");
            dimlight = FindName("dimlight") as Rectangle;
            outline = FindName("outline") as Rectangle;
            highlight = FindName("highlight") as Rectangle;
        }

        #endregion Public Methods

        #region Public Properties

        // Sets the text of the button
        public string Text
        {
            set
            {
                if (text != null) {
                    text.Text = value;
                    UpdateLayout();
                }
            }
        }

        #endregion Public Properties

        #region Public Events

        // Fired when the mouse is up on this control
        public event EventHandler Click;

        #endregion Public Events

        #region Protected Methods

        // Sets the ButtonPressed flag to false
        protected override void OnMouseLeftButtonUp(object sender, MouseEventArgs args)
        {
            base.OnMouseLeftButtonUp(sender, args);
            RaiseClick();
        }

        // Fires Click event if there are registered listeners
        protected void RaiseClick()
        {
            if (MouseOver && (Click != null)) {
                Click(this, null);
            }
        }

        // Executed when button sizes or Text string changes. Repositions the text
        protected override void UpdateLayout()
        {
            base.UpdateLayout();
            if (outline != null) {
                outline.Width = Width;
                outline.Height = Height;
                outline.RadiusX = outline.RadiusY = Height / 2;
            }
            //set highligh and dimlight dimensions
            if (highlight != null) {
                highlight.Height = heightScale * Height;
                highlight.Width = Width - 2 * sideScale * Height;
                highlight.RadiusX = highlight.RadiusY = highlight.Height / 2;
                highlight.SetValue(Canvas.TopProperty, (0.5 - heightScale / 2) * Height);
                highlight.SetValue(Canvas.LeftProperty, sideScale * Height);
            }

            if (dimlight != null) {
                dimlight.Height = heightScale * Height;
                dimlight.Width = Width - 2 * sideScale * Height;
                dimlight.RadiusX = dimlight.RadiusY = dimlight.Height / 2;
                dimlight.SetValue(Canvas.TopProperty, (0.5 - heightScale / 2) * Height);
                dimlight.SetValue(Canvas.LeftProperty, sideScale * Height);
            }

            text.SetValue(Canvas.LeftProperty, (Width - text.ActualWidth) / 2);
            text.SetValue(Canvas.TopProperty, (Height - text.ActualHeight) / 2);

        }

        #endregion Protected Methods

        #region Protected Properties

        // The resource name used to initialize the actual object
        protected override string ResourceName
        {
            get { return "Button.xaml"; }
        }

        #endregion Protected Properties

        #region Private Methods

        private void PulseStoryboardCompleted(object sender, EventArgs e)
        {
            ((Storyboard)sender).Begin();
        }

        #endregion Private Methods

        #region Data

        private TextBlock text = null; //button text
        private Rectangle dimlight = null;
        private Rectangle outline = null;
        private Rectangle highlight = null;
        private const double heightScale = 0.8;
        private const double sideScale = 0.3;

        #endregion Data
    }
}
