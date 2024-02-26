/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: SideMenu.xaml.cs
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
using System.Collections.Generic;
using System.Windows.Input;

namespace VESilverlight.Primary
{
    /// <summary>
    /// Implements tab area
    /// </summary>
    [ScriptableType]
    public partial class SideMenu : UserControl
    {

        public enum Tabs { Concierge, MyPlaces, Search }

        /// <summary>
        /// Tab selection event arguments
        /// </summary>
        public class TabSelectEventArgs : EventArgs
        {
            private Tabs tab;

            public TabSelectEventArgs(Tabs tab)
            {
                this.tab = tab;
            }

            public Tabs Tab
            {
                get { return tab; }
            }
        }

        #region Constructor

        /// <summary>
        /// Constructor. Get a handle to page controls.  Set 
        /// events
        /// </summary>
        public SideMenu()
        {
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("SideMenu", this);

            Controller.GetInstance().SelectTabEvent += new EventHandler<TabSelectEventArgs>(SideMenu_SelectTabEvent);

            Controller.GetInstance().LoginEvent += new EventHandler(SideMenu_LoginEvent);

            this.Loaded += new RoutedEventHandler(SideMenu_Loaded);
        }

        void SideMenu_Loaded(object sender, RoutedEventArgs e)
        {
            this.OnLoaded(sender, e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tab selection display logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SideMenu_SelectTabEvent(object sender, SideMenu.TabSelectEventArgs e)
        {
            cNotSelected.Visibility = Visibility.Visible;
            tNotSelected.Visibility = Visibility.Visible;
            sNotSelected.Visibility = Visibility.Visible;

            cSelected.Visibility = Visibility.Collapsed;
            tSelected.Visibility = Visibility.Collapsed;
            sSelected.Visibility = Visibility.Collapsed;

            switch (e.Tab)
            {
                case Tabs.Concierge:
                    cNotSelected.Visibility = Visibility.Collapsed;
                    cSelected.Visibility = Visibility.Visible;
                    break;
                case Tabs.MyPlaces:
                    tNotSelected.Visibility = Visibility.Collapsed;
                    tSelected.Visibility = Visibility.Visible;
                    break;
                case Tabs.Search:
                    sNotSelected.Visibility = Visibility.Collapsed;
                    sSelected.Visibility = Visibility.Visible;
                    break;
            }
        }

        /// <summary>
        /// User login event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SideMenu_LoginEvent(object sender, EventArgs e)
        {
            MyTripsButton.Cursor = Cursors.Hand;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// OnLoaded override
        /// </summary>
        /// <param name="e">event args</param>
        protected void OnLoaded(object sender, RoutedEventArgs args)
        {
            conciergeToolBar.Initialize(Tabs.Concierge);
            myPlacesToolBar.Initialize(Tabs.MyPlaces);
            searchToolBar.Initialize(Tabs.Search);

            ConciergeButton.MouseLeftButtonDown += new MouseButtonEventHandler(ConciergeButton_MouseLeftButtonDown);
            MyTripsButton.MouseLeftButtonDown += new MouseButtonEventHandler(MyTripsButton_MouseLeftButtonDown);
            SearchButton.MouseLeftButtonDown += new MouseButtonEventHandler(SearchButton_MouseLeftButtonDown);

            ConciergeButton.MouseEnter += new MouseEventHandler(ConciergeButton_MouseEnter);
            ConciergeButton.MouseLeave += new MouseEventHandler(ConciergeButton_MouseLeave);

            MyTripsButton.MouseEnter += new MouseEventHandler(MyTripsButton_MouseEnter);
            MyTripsButton.MouseLeave += new MouseEventHandler(MyTripsButton_MouseLeave);

            SearchButton.MouseEnter += new MouseEventHandler(SearchButton_MouseEnter);
            SearchButton.MouseLeave += new MouseEventHandler(SearchButton_MouseLeave);
            
        }

        #endregion

        #region Mouse Events

        /// <summary>
        /// Search tab is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (sSelected.Visibility == Visibility.Collapsed)
            {
                Controller.GetInstance().SelectTab(Tabs.Search);
                CloseCustomPopup(this, new EventArgs());
                CloseAddConciergePopup(this, new EventArgs());
            }
        }

        /// <summary>
        /// My Places tab is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyTripsButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (tSelected.Visibility == Visibility.Collapsed && Controller.GetInstance().LoggedIn)
            {
                Controller.GetInstance().SelectTab(Tabs.MyPlaces);
                CloseAddConciergePopup(this, new EventArgs());
            }
        }
        
        /// <summary>
        /// Concierge tab is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConciergeButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (cSelected.Visibility == Visibility.Collapsed)
            {
                Controller.GetInstance().SelectTab(Tabs.Concierge);
                CloseCustomPopup(this, new EventArgs());
            }
        }

        /// <summary>
        /// Concierge tool tip show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConciergeButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ConciergeToolTip.Visibility = Visibility.Visible;
            ConciergeToolTipShow.Begin();
        }

        /// <summary>
        /// Concierge tool tip hide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConciergeButton_MouseLeave(object sender, EventArgs e)
        {
            ConciergeToolTip.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// My Places tool tip show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyTripsButton_MouseEnter(object sender, MouseEventArgs e)
        {
            MyPlaceToolTip.Visibility = Visibility.Visible;
            if (Controller.GetInstance().LoggedIn)
                MyPlaceToolTipText.Text = "Create your personalized trip plan";
            else
                MyPlaceToolTipText.Text = "Sign in to Windows Live to create your personalized trip plan";
            MyPlaceToolTipShow.Begin();
        }

        /// <summary>
        /// My Places tool tip hide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyTripsButton_MouseLeave(object sender, EventArgs e)
        {
            MyPlaceToolTip.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Search tool tip show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchButton_MouseEnter(object sender, MouseEventArgs e)
        {
            SearchToolTip.Visibility = Visibility.Visible;
            SearchToolTipShow.Begin();
        }

        /// <summary>
        /// Search Tool Tip hide
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchButton_MouseLeave(object sender, EventArgs e)
        {
            SearchToolTip.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Scriptable Events

        [ScriptableMember]
        public event EventHandler CloseCustomPopup;

        [ScriptableMember]
        public event EventHandler CloseAddConciergePopup;

        #endregion
    }



}