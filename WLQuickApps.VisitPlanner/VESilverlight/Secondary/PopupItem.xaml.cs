/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: PopupItem.xaml.cs
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
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace VESilverlight.Secondary
{
    /// <summary>
    /// Implements a pushpin popup 
    /// </summary>
    [ScriptableType]
    public partial class PopupItem : Canvas
    {
       
        Attraction attraction;
        //Indicates if in 3D mode
        bool is3D = false;

        /// <summary>
        /// Constructor - Registers object as scriptable
        /// </summary>
		public PopupItem()
		{
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("PopupItem", this);
        }

        #region Non-Scriptable Methods

        /// <summary>
        /// Page load -  Get a handle to page controls.  Set events
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void Page_Loaded(object o, EventArgs e)
        {
            // Required to initialize variables
            InitializeComponent();
  
            Close.MouseLeftButtonDown += new MouseButtonEventHandler(Close_MouseLeftButtonDown);
            Canvas PopupCanvas = this.PopupCanvas;// FindName("PopupCanvas") as Canvas;
            PopupCanvas.MouseEnter += new MouseEventHandler(PopupItem_MouseEnter);
            PopupCanvas.MouseLeave += new MouseEventHandler(PopupItem_MouseLeave);
            PopupHide.Completed += new EventHandler(PopupHide_Completed);
            PopupHide3D.Completed +=new EventHandler(PopupHide_Completed);
            CloseFast.Completed += new EventHandler(CloseFast_Completed);
            CloseFast3D.Completed += new EventHandler(CloseFast_Completed);
        }

        /// <summary>
        /// Animates closing of popup box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Close_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (is3D)
                CloseFast3D.Begin();
            else
                CloseFast.Begin();
        }

        /// <summary>
        /// Keeps the popup box open if mouse cursor is in the box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PopupItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (is3D)
                PopupHide3D.Stop();
            else
                PopupHide.Stop();
        }

        /// <summary>
        /// Animates popup hide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PopupItem_MouseLeave(object sender, EventArgs e)
        {
            if (is3D)
                PopupHide3D.Begin();
            else
                PopupHide.Begin();
        }

        /// <summary>
        /// Hides popup div container on the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PopupHide_Completed(object sender, EventArgs e)
        {
            HidePopupItem(this, new EventArgs());
        }

        /// <summary>
        /// Hides popup div container on the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CloseFast_Completed(object sender, EventArgs e)
        {
            HidePopupItem(this, new EventArgs());
        }

        /// <summary>
        /// Opens tour popup for more info on the attraction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Open_MouseLeftButtonDown(object sender, EventArgs e)
        {
            if (is3D)
                CloseFast3D.Begin();
            else
                CloseFast.Begin();
            ShowMoreInfo(this, new AttractionEventArgs(attraction.Serialize()));
        }

        #endregion

        #region Scriptable Methods

        /// <summary>
        /// Initializes components in the popup
        /// </summary>
        /// <param name="serialText"></param>
        [ScriptableMember]
        public void Initialize(string serialText)
        {
            //Controller.GetInstance().GetSerializedAttraction(serialText,
            //    delegate(string attractionText)
            //    {
            //        serialText = attractionText;
                    attraction = Attraction.Deserialize(serialText);

                    TypeText.Text = attraction.Category.ToString("g");
                    if (attraction.Category == Attraction.Categories.Misc)
                        TypeText.Text = "Miscellaneous";
                    TitleText.Text = attraction.Title.ToUpper();
                    DescriptionText.Text = attraction.ShortDescription;
                    Picture.Source = new BitmapImage(new Uri(Utilities.GetAbsolutePath(attraction.ImageURL)));
                    Open.MouseLeftButtonDown += new MouseButtonEventHandler(Open_MouseLeftButtonDown);
                //});
        }

        /// <summary>
        /// Animates popup showing
        /// </summary>
        [ScriptableMember]
        public void FadeInPopup()
        {
            if (is3D)             
                PopupHide3D.Stop();
            else
                PopupHide.Stop();
            //this.Opacity = 0;
            //PopupShow.Begin();
        }

        /// <summary>
        /// Animates popup hiding
        /// </summary>
        [ScriptableMember]
        public void FadeOutPopup()
        {
            if (is3D)
                PopupHide3D.Begin();
            else
                PopupHide.Begin();
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
        public event EventHandler HidePopupItem;

        [ScriptableMember]
        public event EventHandler<AttractionEventArgs> ShowMoreInfo;

        #endregion
    }
}