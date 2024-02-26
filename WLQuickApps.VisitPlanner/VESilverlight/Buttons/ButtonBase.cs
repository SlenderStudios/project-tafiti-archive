//------------------------------------------------------------
//  Windows Live Quick Apps http://codeplex.com/wlquickapps
//------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace VESilverlight {
    [Flags]
    public enum HighlightType { None, Outline, Highlight};

    // Implements a base for all "button-type" controls. It looks for 'outline' and 'highlight' children
    // that will change when the mouse is over the control, or MLB is Down/Up.
    // Uses HighlightBrush and NormalBrush to Fill the outline in the different
    // modes. They are exposed as properties so they can be set in XAML. Will throw
    // exception if the 'outline' is missing. If the 'highlight' is missing the control
    // will not be highlighted when mouse is over.
    public abstract class ButtonBase : ControlBase {

        #region Public Methods

        // Default Button ctor - sets the events
        public ButtonBase()
        {
            //create brushes - default values that can be changed in derived classes ot XAML template
            normalBrush = FindName("normalBrush") as Brush;
            Shape outlineHighlight = FindName("outlineHighlight") as Shape;
            if (outlineHighlight != null) {
                highlightBrush = outlineHighlight.Fill;
                //remove this brush from outlineHighlight - same brush can not be used in two elements
                outlineHighlight.Fill = new SolidColorBrush();
            }
            if (highlightBrush != null)
            {
                Debug.Assert(normalBrush != null, "normalBrush must be defined along with highlightBrush");
            }

            //attach event handlers
            ActualControl.MouseEnter += new MouseEventHandler(OnMouseEnter);
            ActualControl.MouseLeave += new EventHandler(OnMouseLeave);
            ActualControl.MouseLeftButtonDown += new MouseEventHandler(OnMouseLeftButtonDown);
            ActualControl.MouseLeftButtonUp += new MouseEventHandler(OnMouseLeftButtonUp);
            RootLeave += new EventHandler(OnRootLeave);

            //find the needed members
            outline = FindName("outline") as Shape;
            Debug.Assert (outline != null, "The outline of the control must be defined");
            highlight = FindName("highlight") as Shape;
            pulse = FindName("pulse") as Storyboard;
        }

        #endregion Public Methods

        #region Public Properties

        //makes the item highlighted whithout mouse
        public HighlightType IsHighlighted
        {
            get { return isHighlighted; }
            set { isHighlighted = value; UpdateVisuals(); }
        }

        #endregion Public Properties

        #region Protected Methods

        // Sets the MouseOver and ButtonPressed flags to true
        protected virtual void OnMouseLeftButtonDown(object sender, MouseEventArgs args)
        {
            ActualControl.CaptureMouse();
            mouseOver = true;
            buttonPressed = true;
            UpdateVisuals();
        }

        // Sets the ButtonPressed flag to false
        protected virtual void OnMouseLeftButtonUp(object sender, MouseEventArgs args)
        {
            ReleaseMouseCapture();
            buttonPressed = false;
            UpdateVisuals();
        }

        // Sets the MouseOver flag to true
        protected virtual void OnMouseEnter(object sender, MouseEventArgs args)
        {
            mouseOver = true;
            UpdateVisuals();
        }

        // Sets the MouseOver flag to false
        protected virtual void OnMouseLeave(object sender, EventArgs args)
        {
            mouseOver = false;
            UpdateVisuals();
        }

        // When the mouse leaves the root visual we lose the capture - reset the state 
        protected virtual void OnRootLeave(object sender, EventArgs args)
        {
            if (mouseOver || buttonPressed) {
                ReleaseMouseCapture();
                mouseOver = false;
                buttonPressed = false;
                UpdateVisuals();
            }
        }

        //Updates Height and Width of outline and highlight of the button
        protected override void UpdateLayout()
        {
            base.UpdateLayout();
        }

        #endregion Protected Methods

        #region Protected Properties

        // thrue if the mouse is over the control
        protected bool MouseOver
        {
            get { return mouseOver;  }
            set { mouseOver = value; }
        }

        // true if LMB is pressed
        protected bool ButtonPressed
        {
            get { return buttonPressed; }
        }

        #endregion Protected Properties

        #region Private Methods

        // Updates the visual based on the current values of 
        // MouseOver and ButtonPressed flags
        private void UpdateVisuals()
        {
            //fill it with appropriate brush
            if ((buttonPressed && mouseOver) || (isHighlighted == HighlightType.Outline)) {
                if (highlightBrush != null)
                {
                    outline.Fill = highlightBrush;
                    Debug.WriteLine("highlight");
                }
            } else {
                if (normalBrush != null)
                {
                    outline.Fill = normalBrush;
                    Debug.WriteLine("normal");
                }
            }

            // highlight
            if (highlight != null) {
                if (buttonPressed || mouseOver || (isHighlighted == HighlightType.Highlight)) {
                    highlight.Opacity = 1;
                    if (pulse != null) {
                        pulse.Begin();
                    }
                } else {
                    if (pulse != null) {
                        pulse.Stop();
                    }
                    highlight.Opacity = 0;
                }
            }
        }

        #endregion Private Methods

        #region Data

        private Shape outline = null; //main button rectangle
        private Shape highlight = null; //the highlight rectangle
        private Brush normalBrush = null; //color of the main rectangle when it is not highlighted
        private Brush highlightBrush = null; //color of the main rectangle when it is highlighted
        private bool mouseOver = false;  //true if mouse is over the control
        private bool buttonPressed = false; //true if left button is down while on this control
        private Storyboard pulse = null;    //used to pulse the highlight opacity
        //allows making the item highlighted without the mouse - used by ScrollBar
        private HighlightType isHighlighted = HighlightType.None; 
        #endregion Data
    }
}
