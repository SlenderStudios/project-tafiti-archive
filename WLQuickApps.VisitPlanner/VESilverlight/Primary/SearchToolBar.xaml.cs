/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: SearchToolBar.xaml.cs
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
    /// Implements the search control tab area
    /// </summary>
    public partial class SearchToolBar : AbstractToolBar
    {
        #region Constructor

        /// <summary>
        /// Constructor. Get a handle to page controls.  Set 
        /// events
        /// </summary>
        public SearchToolBar()
        {
            this.InitializeComponent();
            Controller.GetInstance().FilterChanged += new EventHandler<Controller.FilterChangedArgs>(SearchToolBar_FilterChanged);
            Controller.GetInstance().SelectTabEvent += new EventHandler<SideMenu.TabSelectEventArgs>(SearchToolBar_SelectTabEvent);
            Controller.GetInstance().SearchResultsChanged += new EventHandler(SearchToolBar_SearchResultsChanged);

            FindButton.MouseLeftButtonDown += new MouseButtonEventHandler(FindButton_MouseLeftButtonDown);
            FindButton.MouseLeftButtonUp += new MouseButtonEventHandler(FindButton_MouseLeftButtonUp);
            FindButton.MouseLeave += new MouseEventHandler(FindButton_MouseLeave);
            
            Controller.GetInstance().DestinationChangedEvent += new EventHandler(SearchToolBar_DestinationChangedEvent);

            this.Loaded += new RoutedEventHandler(SearchToolBar_Loaded);
        }

        void SearchToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            this.OnLoaded(sender, e);
        }

        #endregion

        #region Methods & Events

        /// <summary>
        /// Animate find button mouseout 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FindButton_MouseLeave(object sender, EventArgs e)
        {
            FindUp.Visibility = Visibility.Visible;
            FindDown.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Find Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FindButton_MouseLeftButtonUp(object sender, MouseEventArgs e)
        {
            if (FindDown.Visibility == Visibility.Visible)
            {
                FindUp.Visibility = Visibility.Visible;
                FindDown.Visibility = Visibility.Collapsed;

                if (textBox.Text != String.Empty)
                {
                    Controller.GetInstance().Search(textBox.Text);
                }

                //textBox.ClearText();
                textBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// Animate find button pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FindButton_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            if (FindUp.Visibility == Visibility.Visible)
            {
                FindDown.Visibility = Visibility.Visible;
                FindUp.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Destination City changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchToolBar_DestinationChangedEvent(object sender, EventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                textBox.Text = string.Empty;

                IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues = new Dictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>>();
                filterValues[Attraction.ListType.Concierge] = new Dictionary<Attraction.Categories, bool>();
                filterValues[Attraction.ListType.PushpinSearch] = new Dictionary<Attraction.Categories, bool>();

                filterValues[Attraction.ListType.Concierge][Attraction.Categories.Search] = false;

                int y = 0;

                while (((Attraction.Categories)y).ToString("g") != y.ToString())
                {
                    filterValues[Attraction.ListType.PushpinSearch][(Attraction.Categories)y] = false;

                    y++;
                }

                Controller.GetInstance().SetAttractionFilter(filterValues);
            }
        }

        void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (textBox.Text != String.Empty)
                {
                    Controller.GetInstance().Search(textBox.Text);
                }

                textBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// Display search results
        /// </summary>
        void setupList()
        {
            repeater.Items.Clear();

            List<Attraction> searchList = Controller.GetInstance().GetConciergeSearchList();

            foreach (Attraction attraction in searchList)
            {
                repeater.Items.Add(new PrimaryPlaceListItem(attraction));
            }

            searchList = Controller.GetInstance().GetWebSearchList();

            foreach (Attraction attraction in searchList)
            {
                repeater.Items.Add(new PrimaryPlaceListItem(attraction));
            }

            repeater.UpdateItems();
        }

        /// <summary>
        /// New search results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchToolBar_SearchResultsChanged(object sender, EventArgs e)
        {
            IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues = new Dictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>>();
            filterValues[Attraction.ListType.Concierge] = new Dictionary<Attraction.Categories, bool>();
            filterValues[Attraction.ListType.PushpinSearch] = new Dictionary<Attraction.Categories, bool>();

            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Search] = true;

            int y = 0;

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.PushpinSearch][(Attraction.Categories)y] = true;

                y++;
            }

            Controller.GetInstance().SetAttractionFilter(filterValues);

            setupList();
        }

        /// <summary>
        /// When search tab is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchToolBar_SelectTabEvent(object sender, SideMenu.TabSelectEventArgs e)
        {
            if (e.Tab != tab) return;

            Controller.GetInstance().SetAttractionDropBoxState(true,true);

            textBox.Text = string.Empty;
            
            //Controller.GetInstance().GetWebSearchList().Clear();

            IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues = new Dictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>>();
            filterValues[Attraction.ListType.Concierge] = new Dictionary<Attraction.Categories, bool>();
            filterValues[Attraction.ListType.User] = new Dictionary<Attraction.Categories, bool>();
            filterValues[Attraction.ListType.Shared] = new Dictionary<Attraction.Categories, bool>();
            filterValues[Attraction.ListType.PushpinSearch] = new Dictionary<Attraction.Categories, bool>();


            //hide concierge items
            int y = 0;

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.Concierge][(Attraction.Categories)y] = false;

                y++;
            }

            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Search] = true;

            //hide shared items
            y = 0;

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.Shared][(Attraction.Categories)y] = false;

                y++;
            }

            //show pushpin search
            y = 0;

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.PushpinSearch][(Attraction.Categories)y] = true;

                y++;
            }

            //hide user items
            y = 0;

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.User][(Attraction.Categories)y] = false;

                y++;
            }

            Controller.GetInstance().SetAttractionFilter(filterValues);

            //setupList();
        }

        /// <summary>
        /// Check box filters change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SearchToolBar_FilterChanged(object sender, Controller.FilterChangedArgs e)
        {
            if (this.Visibility == Visibility.Visible)
                setupList();
        }


        /// <summary>
        /// OnLoaded override
        /// </summary>
        /// <param name="e">event args</param>
        protected void OnLoaded(object sender, RoutedEventArgs args)
        {
            textBox.Focus();
            //textBox.BorderColor = Color.FromArgb(0, 0, 0, 0);
            //textBox.FocusColor = Color.FromArgb(0, 0, 0, 0);
            //textBox.FontColor = Color.FromArgb(0, 0x8f, 0xaa, 0xbc);
            //textBox.FontFamily = "Verdana";
            //textBox.FontSize = 12;

            //textBox.Initialize(Page.RootCanvas);

            //textBox.Resize(textBox.Width, textBox.Height);   
        }

        #endregion
    }
}