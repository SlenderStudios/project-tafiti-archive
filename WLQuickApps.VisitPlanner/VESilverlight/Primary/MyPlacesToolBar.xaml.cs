/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: MyPlacesToolBar.xaml.cs
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

namespace VESilverlight.Primary
{
    /// <summary>
    /// Implements My Places tab area
    /// </summary>
    [ScriptableType]
    public partial class MyPlacesToolBar : AbstractToolBar
    {
        public MyPlacesToolBar()
        {
            this.InitializeComponent();

            this.Loaded += new RoutedEventHandler(MyPlacesToolBar_Loaded);
        }

        void MyPlacesToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            this.OnLoaded(sender, e);
        }

        #region Non-Scriptable Methods

        /// <summary>
        /// OnLoaded override
        /// </summary>
        /// <param name="e">event args</param>
        protected void OnLoaded(object sender, RoutedEventArgs args)
        {
            HtmlPage.RegisterScriptableObject("MyPlacesToolBar", this);

            Controller.GetInstance().FilterChanged += new EventHandler<Controller.FilterChangedArgs>(MyPlacesToolBar_FilterChanged);
            Controller.GetInstance().SelectTabEvent += new EventHandler<SideMenu.TabSelectEventArgs>(MyPlacesToolBar_SelectTabEvent);

            Controller.GetInstance().DestinationChangedEvent += new EventHandler(MyPlacesToolBar_DestinationChangedEvent);
            Controller.GetInstance().AddAttractionEvent += new ItineraryListener(MyPlacesToolBar_AddAttractionEvent);
            Controller.GetInstance().RemoveAttractionEvent += new ItineraryListener(MyPlacesToolBar_RemoveAttractionEvent);
        }


        /// <summary>
        /// An attraction is deleted from My Places
        /// </summary>
        /// <param name="attraction"></param>
        void MyPlacesToolBar_RemoveAttractionEvent(Attraction attraction)
        {
            if (this.Visibility != Visibility.Visible) return;

            setupList();
        }

        /// <summary>
        /// An Attraction is added to My Places
        /// </summary>
        /// <param name="attraction"></param>
        void MyPlacesToolBar_AddAttractionEvent(Attraction attraction)
        {
            if (this.Visibility != Visibility.Visible) return;

            setupList();
            setupDestinationList();
        }

        /// <summary>
        /// Event that the destination city has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyPlacesToolBar_DestinationChangedEvent(object sender, EventArgs e)
        {
            if (this.Visibility != Visibility.Visible) return;
            setupList();
            setupDestinationList();
        }

        /// <summary>
        /// Occurs when My Places tab is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyPlacesToolBar_SelectTabEvent(object sender, SideMenu.TabSelectEventArgs e)
        {
            if (e.Tab != tab) return;

            Controller.GetInstance().SetAttractionDropBoxState(true,false);

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

            //hide shared items
            y = 0;

            filterValues[Attraction.ListType.Shared] = new Dictionary<Attraction.Categories, bool>();

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.Shared][(Attraction.Categories)y] = false;

                y++;
            }

            //hide pushpin search
            y = 0;

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.PushpinSearch][(Attraction.Categories)y] = false;

                y++;
            }

            //show user items
            y = 0;

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.User][(Attraction.Categories)y] = true;

                y++;
            }            

            Controller.GetInstance().SetAttractionFilter(filterValues);

            setupList();

            setupDestinationList();
        }

        /// <summary>
        /// Displays destinations list
        /// </summary>
        void setupDestinationList()
        {
            destList.Items.Clear();

            Controller.GetInstance().GetMyDestinations(new DestinationListDelegate(
                delegate(List<Destination> destinations)
                {
                    foreach (Destination dest in destinations)
                    {
                        destList.Items.Add(new DestinationListItem(dest));

                    }
                    destList.UpdateItems();
                }));
        }

        /// <summary>
        /// Occurs when check box filters are changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyPlacesToolBar_FilterChanged(object sender, Controller.FilterChangedArgs e)
        {
            if (this.Visibility == Visibility.Visible)
                setupList();
        }

        /// <summary>
        /// Displays list of attraction in tab area
        /// </summary>
        void setupList()
        {
            repeater.Items.Clear();

            Controller.GetInstance().GetItinerary(new AttractionListDelegate(
                delegate(List<Attraction> itin)
                {
                    foreach (Attraction attraction in itin)
                    {
                        repeater.Items.Add(new PrimaryPlaceListItem(attraction));
                    }

                    repeater.UpdateItems();
                }));
        }

        #endregion

        #region Scriptable Methods

        /// <summary>
        /// Add a custom attraction
        /// </summary>
        /// <param name="serialText"></param>
        [ScriptableMember]
        public void AddCustomPlace(string serialText)
        {
            if (this.Visibility != Visibility.Visible) return;

            Controller.GetInstance().AddToItinerary(serialText);
        }

        /// <summary>
        /// Returns true if My Places tab is currently open
        /// </summary>
        /// <returns></returns>
        [ScriptableMember]
        public int IsOnMyPlaces()
        {
            if (this.Visibility == Visibility.Visible)
                return 1;
            else
                return 0;
        }

        #endregion
    }
}