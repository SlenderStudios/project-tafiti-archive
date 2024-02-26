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
using System.Windows.Input;

namespace VESilverlight.Secondary
{
    /// <summary>
    /// Implements the functionality of the drag and drop attraction box
    /// that appears when a user logs in
    /// </summary>
    [ScriptableType]
    public partial class AttractionDropBox : Canvas
    {
        #region Private Properties
        /// <summary>
        /// X Coordinate of the drop box
        /// </summary>
        private int xCoord = -1;

        /// <summary>
        /// Y Coordinate of the drop box
        /// </summary>
        private int yCoord = -1;

        /// <summary>
        /// Drop box active flag
        /// </summary>
        private bool active = false;

        /// <summary>
        /// Drop box insert flag
        /// </summary>
        private bool insert = true;

        /// <summary>
        /// Drop box 
        /// </summary>
        private bool hoverOver = false;
        
        /// <summary>
        /// Show outline action
        /// </summary>
        private Storyboard showOutline;

        /// <summary>
        /// Hide outline action
        /// </summary>
        private Storyboard hideOutline;

        /// <summary>
        /// Temporary serialized attraction
        /// </summary>
        private string tmpSerialText = string.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Get a handle to page controls.  
        /// Registers the object as scriptable
        /// </summary>
		public AttractionDropBox()
		{
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("AttractionDropBox", this);
        }

        #endregion

        #region Non-Sriptable Methods

        /// <summary>
        /// Initializes silverlight components and sets events
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void Page_Loaded(object o, EventArgs e)
        {
            // Required to initialize variables
            InitializeComponent();

            showOutline = this.ShowOutline;// FindName("ShowOutline") as Storyboard;
            hideOutline = this.HideOutline;// FindName("HideOutline") as Storyboard;
            SetContent();
                 
            this.MouseLeftButtonUp += new MouseButtonEventHandler(AttractionDropBox_MouseLeftButtonUp);
            this.MouseEnter += new MouseEventHandler(AttractionDropBox_MouseEnter);
            this.MouseLeave += new MouseEventHandler(AttractionDropBox_MouseLeave);
        }

        /// <summary>
        /// Tool tip hide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AttractionDropBox_MouseLeave(object sender, EventArgs e)
        {
            DropBoxToolTip.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Tool Tip show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AttractionDropBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!hoverOver)
            {
                DropBoxToolTip.Visibility = Visibility.Visible;
                DropBoxToolTipShow.Begin();
            }
        }

        /// <summary>
        /// Adds or deletes an attracion on mouse up after a pushpin has been dragged onto
        /// the box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AttractionDropBox_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {            
            if (active && hoverOver)
            {
                hoverOver = false;
                if (insert)
                {
                    AddAttraction(this, new AttractionEventArgs(tmpSerialText));
                }
                else
                {
                    RemoveAttraction(this, new AttractionEventArgs(tmpSerialText));
                }
                hideOutline.Begin();
            }
        }

        /// <summary>
        /// Depending on which tab a user is on, the box will either be an add attraction box 
        /// or a remove attraction box
        /// </summary>
        private void SetContent()
        {
            if (!insert)
            {
                Plus.Visibility = Visibility.Collapsed;
                Minus.Visibility = Visibility.Visible;
                ToolTipText.Text = "Drag items here to remove from your places";
            }
            else
            {
                Plus.Visibility = Visibility.Visible;
                Minus.Visibility = Visibility.Collapsed;
                ToolTipText.Text = "Drag items here to add to your places";
            }

        }

        #endregion

        #region Scriptable Methods

        /// <summary>
        /// Sets the x and y pixel location properties of the box on the screen
        /// </summary>
        /// <param name="x">X pixel</param>
        /// <param name="y">Y pixel</param>
        [ScriptableMember]
        public void SetPosition(int x, int y)
        {
            xCoord = x;
            yCoord = y;
        }

        /// <summary>
        /// Shows the outline animation if a pushpin is dragged ontop of the box
        /// </summary>
        /// <param name="x">X position of mouse</param>
        /// <param name="y">Y position of mouse</param>
        /// <param name="serialText">Serialized attraction</param>
        [ScriptableMember]
        public void HitDetect(int x, int y, string serialText)
        {
            if (!active) return;
            
            //The line below is always false in 3D mode except on mouse up
            if (x > xCoord && x < xCoord + Width && y > yCoord && y < yCoord + Height)
            {                
                if (!hoverOver)  //If not currently hovered
                {
                    tmpSerialText = serialText;
                    showOutline.Begin();
                    DropBoxToolTip.Visibility = Visibility.Collapsed;
                    hoverOver = true;
                }
                
            }
            else
            {
                if (hoverOver)  
                {
                    hideOutline.Begin();
                    hoverOver = false;
                }
            }
        }

        /// <summary>
        /// After a user logs in, the box becomes active
        /// </summary>
        /// <param name="active">Active flag</param>
        /// <param name="insert">Insert/Remove flag</param>
        [ScriptableMember]
        public void SetActive(bool active, bool insert)
        {
            this.active = active;
            this.insert = insert;
            SetContent();
        }

        #endregion

        #region Scriptable Events
        /// <summary>
        /// Add attraction scriptable event handler
        /// </summary>
        [ScriptableMember]
        public event EventHandler<AttractionEventArgs> AddAttraction;

        /// <summary>
        /// Remove attraction scriptable event handler
        /// </summary>
        [ScriptableMember]
        public event EventHandler<AttractionEventArgs> RemoveAttraction;

        #endregion
    }
}
