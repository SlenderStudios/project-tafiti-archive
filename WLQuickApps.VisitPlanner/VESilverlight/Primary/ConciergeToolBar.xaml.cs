/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: ConciergeToolBar.xaml.cs
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
using System.Net;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace VESilverlight.Primary
{
    [ScriptableType]
    public partial class ConciergeToolBar : AbstractToolBar
    {
        #region Private properties

        /// <summary>
        /// Select all flag to disable unneeded checkbox events
        /// </summary>
        private bool _selectAllActive = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Get a handle to page controls.  Set 
        /// events
        /// </summary>
        public ConciergeToolBar()
		{
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("ConciergeToolBar", this);

            //Event handlers
            Controller.GetInstance().FilterChanged += new EventHandler<Controller.FilterChangedArgs>(ConciergeToolBar_FilterChanged);
            Controller.GetInstance().SelectTabEvent += new EventHandler<SideMenu.TabSelectEventArgs>(ConciergeToolBar_SelectTabEvent);
            Controller.GetInstance().SharedDataEvent += new EventHandler(ConciergeToolBar_SharedDataEvent);
            Controller.GetInstance().AddConciergeEvent += new ItineraryListener(ConciergeToolBar_AddConciergeEvent);

            this.Loaded += new RoutedEventHandler(ConciergeToolBar_Loaded);
        }

        void ConciergeToolBar_Loaded(object sender, RoutedEventArgs e)
        {
            this.OnLoaded(sender, e);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Shared view 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ConciergeToolBar_SharedDataEvent(object sender, EventArgs e)
        {
            SharedCanvas.Visibility = Visibility.Visible;
            SharedBox.Value = true;
        }

        /// <summary>
        /// Select Concierge tab event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ConciergeToolBar_SelectTabEvent(object sender, SideMenu.TabSelectEventArgs e)
        {
            if (e.Tab != tab)
            {
                return;
            }

            Controller.GetInstance().SetAttractionDropBoxState(true,true);

            //show concierge items, as appropriate
            IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues = new Dictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>>();
            filterValues[Attraction.ListType.Concierge] = new Dictionary<Attraction.Categories, bool>();
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Accomodation] = HotelBox.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Art] = ArtBox.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Movie] = MovieBox.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Food] = FoodBox.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Music] = MusicBox.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Misc] = MiscBox.Value;

            //hide search items
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Search] = false;

            if (SharedBox.Value)
            {
                //show shared items, as appropriate
                filterValues[Attraction.ListType.Shared] = new Dictionary<Attraction.Categories, bool>();
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Accomodation] = HotelBox.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Art] = ArtBox.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Movie] = MovieBox.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Food] = FoodBox.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Music] = MusicBox.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Misc] = MiscBox.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Custom] = true;
            }

            //hide my places items
            int y = 0;

            filterValues[Attraction.ListType.User] = new Dictionary<Attraction.Categories, bool>();

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.User][(Attraction.Categories)y] = false;
                y++;
            }

            //hide local search items
            y = 0;

            filterValues[Attraction.ListType.PushpinSearch] = new Dictionary<Attraction.Categories, bool>();

            while (((Attraction.Categories)y).ToString("g") != y.ToString())
            {
                filterValues[Attraction.ListType.PushpinSearch][(Attraction.Categories)y] = false;
                y++;
            }

            //TODO: conditionally hide or show shared items

            Controller.GetInstance().SetAttractionFilter(filterValues);

            setupList();
        }

        /// <summary>
        /// Filter changed.  Setup the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ConciergeToolBar_FilterChanged(object sender, Controller.FilterChangedArgs e)
        {
            if (this.Visibility == Visibility.Visible)
            {
                setupList();
            }
        }
                
        /// <summary>
        /// OnLoaded override
        /// </summary>
        /// <param name="e">event args</param>
        protected void OnLoaded(object sender, RoutedEventArgs args)
        {            
            statusImage.Source = new BitmapImage(new Uri("/images/Status_Offline.png", UriKind.Relative));

            conciergeLinkCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(conciergeLinkCanvas_MouseLeftButtonDown);

            //Check boxes
            MusicBox.Value = true;
            ArtBox.Value = true;
            MovieBox.Value = true;
            FoodBox.Value = true;
            HotelBox.Value = true;
            MiscBox.Value = true;
            SelectAll.Value = true;

            //attach events
            MusicBox.ValueChanged += new EventHandler<Checkbox.ValueChangedEventArgs>(MusicBox_ValueChanged);
            ArtBox.ValueChanged += new EventHandler<Checkbox.ValueChangedEventArgs>(ArtBox_ValueChanged);
            MovieBox.ValueChanged += new EventHandler<Checkbox.ValueChangedEventArgs>(MovieBox_ValueChanged);
            FoodBox.ValueChanged += new EventHandler<Checkbox.ValueChangedEventArgs>(FoodBox_ValueChanged);
            HotelBox.ValueChanged += new EventHandler<Checkbox.ValueChangedEventArgs>(HotelBox_ValueChanged);
            SharedBox.ValueChanged += new EventHandler<Checkbox.ValueChangedEventArgs>(SharedBox_ValueChanged);
            MiscBox.ValueChanged += new EventHandler<Checkbox.ValueChangedEventArgs>(MiscBox_ValueChanged);
            SelectAll.ValueChanged += new EventHandler<Checkbox.ValueChangedEventArgs>(SelectAll_ValueChanged);

            Controller.GetInstance().DestinationChangedEvent += new EventHandler(ConciergeToolBar_DestinationChangedEvent);

            conciergeLinkCanvas.MouseEnter += new MouseEventHandler(conciergeLinkCanvas_MouseEnter);
            conciergeLinkCanvas.MouseLeave += new MouseEventHandler(conciergeLinkCanvas_MouseLeave);

            //set up the list
            setupList();

        }

        /// <summary>
        /// Display tool tip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void conciergeLinkCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            MessengerToolTip.Visibility = Visibility.Visible;
            MessengerToolTipShow.Begin();
        }

        /// <summary>
        /// Hides tool tip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void conciergeLinkCanvas_MouseLeave(object sender, EventArgs e)
        {
            MessengerToolTip.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Select all callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SelectAll_ValueChanged(object sender, Checkbox.ValueChangedEventArgs e)
        {
            _selectAllActive = true;
            
            //set filter
            IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues = new Dictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>>();
            filterValues[Attraction.ListType.Concierge] = new Dictionary<Attraction.Categories, bool>();
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Art] = e.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Food] = e.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Movie] = e.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Music] = e.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Misc] = e.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Custom] = e.Value;
            filterValues[Attraction.ListType.Concierge][Attraction.Categories.Accomodation] = e.Value;
            if (SharedBox.Value)
            {
                filterValues[Attraction.ListType.Shared] = new Dictionary<Attraction.Categories, bool>();
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Art] = e.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Food] = e.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Movie] = e.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Music] = e.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Misc] = e.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Custom] = e.Value;
                filterValues[Attraction.ListType.Shared][Attraction.Categories.Accomodation] = e.Value;
            }
            
            //apply filter
            Controller.GetInstance().SetAttractionFilter(filterValues);

            //update checkbox values
            MusicBox.Value = e.Value;
            ArtBox.Value = e.Value;
            MovieBox.Value = e.Value;
            FoodBox.Value = e.Value;
            HotelBox.Value = e.Value;
            MiscBox.Value = e.Value;
            if (SharedBox.Value)
            {
                SharedBox.Value = e.Value;
            }
            _selectAllActive = false;
        }

        /// <summary>
        /// Check box value changed helper function
        /// </summary>
        /// <param name="category">Category to change</param>
        /// <param name="value">Value to set</param>
        private void ChangeValue(Attraction.Categories category, bool value)
        {
            //only execute if not select all callback
            if (!_selectAllActive)
            {
                IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues = new Dictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>>();
                filterValues[Attraction.ListType.Concierge] = new Dictionary<Attraction.Categories, bool>();
                filterValues[Attraction.ListType.Concierge][category] = value;

                //set shared value if set
                if (SharedBox.Value)
                {
                    filterValues[Attraction.ListType.Shared] = new Dictionary<Attraction.Categories, bool>();
                    filterValues[Attraction.ListType.Shared][category] = value;
                }
                //apply filter
                Controller.GetInstance().SetAttractionFilter(filterValues);
            }
        }


        /// <summary>
        /// Shared checkbox callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SharedBox_ValueChanged(object sender, Checkbox.ValueChangedEventArgs e)
        {
            if (!_selectAllActive)
            {
                IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues = new Dictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>>();
                filterValues[Attraction.ListType.Shared] = new Dictionary<Attraction.Categories, bool>();

                if (e.Value)
                {
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Accomodation] = HotelBox.Value;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Art] = ArtBox.Value;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Food] = FoodBox.Value;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Movie] = MovieBox.Value;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Music] = MusicBox.Value;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Misc] = MiscBox.Value;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Custom] = true;
                }
                else
                {
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Accomodation] = false;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Art] = false;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Food] = false;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Movie] = false;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Music] = false;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Misc] = false;
                    filterValues[Attraction.ListType.Shared][Attraction.Categories.Custom] = false;

                }
                Controller.GetInstance().SetAttractionFilter(filterValues);
            }
        }

        /// <summary>
        /// Destination changed callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ConciergeToolBar_DestinationChangedEvent(object sender, EventArgs e)
        {
            destTitleText.Text = Controller.GetInstance().DestinationName;
            
            if (this.Visibility != Visibility.Visible) return;
            
            MusicBox.Value = true;
            ArtBox.Value = true;
            MovieBox.Value = true;
            FoodBox.Value = true;
            HotelBox.Value = true;
            MiscBox.Value = true;
            
            setupList();
        }
        
        /// <summary>
        /// Add concierge item
        /// </summary>
        /// <param name="attraction"></param>
        protected void ConciergeToolBar_AddConciergeEvent(Attraction attraction)
        {
            if (this.Visibility == Visibility.Visible)
            {
                setupList();
            }
        }

        /// <summary>
        /// Set up the concierge list
        /// </summary>
        private void setupList()
        {
            //Concierge list on right
            repeater.Items.Clear();

            //GetConciergeList returns a List of Attractions objects
            Controller.GetInstance().GetConciergeList(true,
                delegate(List<Attraction> attractions)
                {
                    foreach (Attraction attraction in attractions)
                    {
                        repeater.Items.Add(new PrimaryPlaceListItem(attraction));
                    }

                    Controller.GetInstance().GetSharedItinerary(true,
                        delegate(List<Attraction> attractions2)
                        {
                            foreach (Attraction attraction2 in attractions2)
                            {
                                repeater.Items.Add(new PrimaryPlaceListItem(attraction2));
                            }

                            repeater.UpdateItems();
                        });
                });
        }

        /// <summary>
        /// Accomodation filter callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HotelBox_ValueChanged(object sender, Checkbox.ValueChangedEventArgs e)
        {
            ChangeValue(Attraction.Categories.Accomodation, e.Value);
        }

        /// <summary>
        /// Restaurant filter callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void FoodBox_ValueChanged(object sender, Checkbox.ValueChangedEventArgs e)
        {
            ChangeValue(Attraction.Categories.Food, e.Value);
        }

        /// <summary>
        /// Movie filter callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MovieBox_ValueChanged(object sender, Checkbox.ValueChangedEventArgs e)
        {
            ChangeValue(Attraction.Categories.Movie, e.Value);
        }

        /// <summary>
        /// Art filter callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ArtBox_ValueChanged(object sender, Checkbox.ValueChangedEventArgs e)
        {
            ChangeValue(Attraction.Categories.Art, e.Value);
        }

        /// <summary>
        /// Music filter callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MusicBox_ValueChanged(object sender, Checkbox.ValueChangedEventArgs e)
        {
            ChangeValue(Attraction.Categories.Music, e.Value);
        }

        /// <summary>
        /// Miscellaneous filter callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MiscBox_ValueChanged(object sender, Checkbox.ValueChangedEventArgs e)
        {
            ChangeValue(Attraction.Categories.Misc, e.Value);

        }

        /// <summary>
        /// Concierge conversation click callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void conciergeLinkCanvas_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            //Concierge"s Live Account
            HtmlPage.Window.Navigate(new Uri("http://settings.messenger.live.com/Conversation/IMMe.aspx?invitee=e8460016b736f97f%40apps.messenger.live.com&mkt=en-US"), "_blank", "height=450, width=320");
    
        }

        #endregion

        #region Overrides
        /// <summary>
        /// The resource name used to initialize the actual object
        /// </summary>
        protected string ResourceName
        {
            get { return "ConciergeToolBar.xaml"; }
        }

        /// <summary>
        /// Update layout.  Not implemented.
        /// </summary>
        protected void UpdateLayout() {

        }
        #endregion

        #region Public Scriptable Methods

        /// <summary>
        /// Set the concierge online status image. 
        /// </summary>
        /// <param name="online">"true" if online, "false" if not</param>
        [ScriptableMember]
        public void SetStatusImage(string online)
        {
            if (Boolean.Parse(online))
                statusImage.Source = new BitmapImage(new Uri("/images/Status_Online.png", UriKind.Relative));
            else
                statusImage.Source = new BitmapImage(new Uri("/images/Status_Offline.png", UriKind.Relative));
        }

        /// <summary>
        /// Is on the concierge flag
        /// </summary>
        /// <returns>1 if on concierge, 0 if not</returns>
        [ScriptableMember]
        public int IsOnConcierge()
        {
            if (this.Visibility == Visibility.Visible)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        
        /// <summary>
        /// Add concierge item
        /// </summary>
        /// <param name="serialText">Serialized attraction item</param>
        [ScriptableMember]
        public void AddConciergeItem(string serialText)
        {
            if (this.Visibility == Visibility.Visible)
            {
                Controller.GetInstance().AddToConcierge(serialText);
            }

        }

        /// <summary>
        /// Removes concierge item
        /// </summary>
        /// <param name="serialText">Serialized attraction item</param>
        [ScriptableMember]
        public void RemoveConciergeItem(Attraction attraction)
        {
            if (this.Visibility == Visibility.Visible)
            {
                string serialText = attraction.Serialize();
                Controller.GetInstance().RemoveFromConcierge(serialText);
            }

        }

        #endregion
    }
}
