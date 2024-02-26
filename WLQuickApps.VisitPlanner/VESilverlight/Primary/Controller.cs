/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: Controller.cs
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Browser;
using System.Threading;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;
using System.Text;

namespace VESilverlight.Primary
{
    /// <summary>
    /// Itinerary delegate
    /// </summary>
    /// <param name="attraction">Attraction object</param>
    public delegate void ItineraryListener(Attraction attraction);

    /// <summary>
    /// This class is responsible for the core functionality of the Visit planner.  
    /// It allows for the interaction of front end code with server side code such 
    /// as data retrieval and updates from/to the database.  
    /// </summary>
    [ScriptableType]
    public class Controller
    {
        
        #region Public events
        /// <summary>
        /// Add attraction call-back
        /// </summary>
        public event ItineraryListener AddAttractionEvent;

        /// <summary>
        /// Add concierge item call-back
        /// </summary>
        public event ItineraryListener AddConciergeEvent;
        
        /// <summary>
        /// Remove attraction call-back
        /// </summary>
        public event ItineraryListener RemoveAttractionEvent;

        /// <summary>
        /// Show tour item call-back
        /// </summary>
        public event ItineraryListener ShowTourItemEvent;

        /// <summary>
        /// Search results changed call-back
        /// </summary>
        public event EventHandler SearchResultsChanged;

        /// <summary>
        /// Show itinerary call-back
        /// </summary>
        public event EventHandler ShowItineraryEvent;

        /// <summary>
        /// Hide itinerary call-back
        /// </summary>
        public event EventHandler HideItineraryEvent;

        /// <summary>
        /// Destination Change Call-back
        /// </summary>
        public event EventHandler DestinationChangedEvent;
        
        /// <summary>
        /// Filter changed event handler
        /// </summary>
        public event EventHandler<FilterChangedArgs> FilterChanged;

        /// <summary>
        /// Delegate to this method is found in SideMenu.xaml.cs and TourShareControl.xaml.cs
        /// </summary>
        public event EventHandler LoginEvent;

        /// <summary>
        /// Shared data delegate
        /// </summary>
        public event EventHandler SharedDataEvent;

        /// <summary>
        /// Select tab delegate
        /// </summary>
        public event EventHandler<SideMenu.TabSelectEventArgs> SelectTabEvent;

        #endregion

        #region Public Scriptable events
        /// <summary>
        /// Calls method in JavaScript to handle search results
        /// </summary>
        [ScriptableMember]
        public event EventHandler<SearchEventArgs> DoSearch;

        /// <summary>
        /// Drop box state changed
        /// </summary>
        [ScriptableMember]
        public event EventHandler<DropBoxStateEventArgs> AttractionDropBoxChanged;

        /// <summary>
        /// Show tour item delegate
        /// </summary>
        [ScriptableMember]
        public event EventHandler<AttractionEventArgs> ShowTourItem;

        /// <summary>
        /// Hide tour item delegate
        /// </summary>
        [ScriptableMember]
        public event EventHandler HideTourItem;

        /// <summary>
        /// Stop tour event delegate
        /// </summary>
        [ScriptableMember]
        public event EventHandler StopTourEvent;

        /// <summary>
        /// Move list hover delegate
        /// </summary>
        [ScriptableMember]
        public event EventHandler<PlaceListPositionEventArgs> MovePlaceListHover;

        /// <summary>
        /// Initialize tour delegate
        /// </summary>
        [ScriptableMember]
        public event EventHandler<AttractionCountEventArgs> TourInitializer;
        #endregion

        #region Private properties

        /// <summary>
        /// Instance of this class
        /// </summary>
        private static Controller instance = null;
        
        /// <summary>
        /// Temporary ID
        /// </summary>
        private string tempID = null;

        /// <summary>
        /// Wait flag
        /// </summary>
        private bool waitCondition = false;

        /// <summary>
        /// Tour in progress flag
        /// </summary>
        private bool tourInProgress = false;

        /// <summary>
        /// Tour paused state
        /// </summary>
        private bool tourPaused = false;

        /// <summary>
        /// Tour thread
        /// </summary>
        private Thread tourThread;

        /// <summary>
        /// Tracking filter state
        /// </summary>
        private IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterMatrix = new Dictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>>();

        /// <summary>
        /// List of search results
        /// </summary>
        private List<Attraction> mapSearchList = new List<Attraction>();

        /// <summary>
        /// Keyword search helper
        /// </summary>
        private MapSearchHelper searchHelper = new MapSearchHelper();

        /// <summary>
        /// Work dispatcher
        /// </summary>
        private UIWorkDispatcher workDispatcher = new UIWorkDispatcher();

        /// <summary>
        /// Default destinaion city (Las Vegas)
        /// </summary>
        private int currentDestinationId = 1;

        /// <summary>
        /// Default Destination Name
        /// </summary>
        private string currentDestinationName = "Las Vegas";

        #endregion

        #region Public Properties
        /// <summary>
        /// Tracking filter state
        /// </summary>
        public IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> FilterMatrix
        {
            get { return filterMatrix; }
        }

        /// <summary>
        /// Returns string name of current destination city
        /// </summary>
        public string DestinationName
        {
            get { return currentDestinationName; }
        }
        #endregion

        #region Singleton instance
        /// <summary>
        /// Get single instance
        /// </summary>
        /// <returns>Controller instance</returns>
        public static Controller GetInstance()
        {
            if (instance == null)
            {
                instance = new Controller();
            }
            return instance;

        }

        /// <summary>
        /// Private constructor.  Registers the controller as a 
        /// scriptable object.
        /// </summary>
        private Controller()
        {
            HtmlPage.RegisterScriptableObject("Controller", this);

            DestinationChangedEvent += new EventHandler(Controller_DestinationChangedEvent);

            filterMatrix[Attraction.ListType.Concierge] = new Dictionary<Attraction.Categories, bool>();
            filterMatrix[Attraction.ListType.Shared] = new Dictionary<Attraction.Categories, bool>();
            filterMatrix[Attraction.ListType.User] = new Dictionary<Attraction.Categories, bool>();
            filterMatrix[Attraction.ListType.PushpinSearch] = new Dictionary<Attraction.Categories, bool>();

            int y = 0;

            while (((Attraction.ListType)y).ToString("g") != y.ToString()){
                int x = 0;

                while (((Attraction.Categories)x).ToString("g") != x.ToString())
                {
                    filterMatrix[(Attraction.ListType)y][(Attraction.Categories)x] = (((Attraction.ListType)y == Attraction.ListType.Concierge) || ((Attraction.ListType)y == Attraction.ListType.Shared));
                    x++;
                }

                y++;
            }
        }
        #endregion
        
        #region Private Helper Methods
        /// <summary>
        /// Event fired when a different city is selected from drop down menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Controller_DestinationChangedEvent(object sender, EventArgs e)
        {
            if (TourInitializer != null)
            {
                this.GetConciergeList(true,
                    delegate(List<Attraction> attractions)
                    {
                        GetAttractionsCount(
                            delegate(int count)
                            {
                                TourInitializer(this, new AttractionCountEventArgs(count));

                                (HtmlPage.Window.GetProperty("ResetMap") as ScriptObject).InvokeSelf();
                            });
                    });
            }
            StopTour();
        }

        /// <summary>
        /// Checks an attraction is the same as the current attraction object
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private bool MatchesGuid(Attraction o)
        {
            return (o.ID == this.tempID);
        }
        #endregion

        #region Scriptable Event Argument Classes

        /// <summary>
        /// Class representing arguments for the event that the filter checkbox are changed
        /// </summary>
        [ScriptableType]
        public class FilterChangedArgs : EventArgs
        {
            #region Private Properties
            /// <summary>
            /// Filter values
            /// </summary>
            private IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues;
            #endregion

            #region Public Methods
            /// <summary>
            /// Updates the filter values
            /// </summary>
            /// <param name="filterValues">Dictionary of categories and the state for each</param>
            public FilterChangedArgs(IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues)
            {
                this.filterValues = filterValues;
            }
            #endregion

            #region Public Scriptable properties
            /// <summary>
            /// JSON formatted filter
            /// </summary>
            [ScriptableMember]
            public string FilterJSON
            {
               get {
                    string retVal = "{ 'filters': {";
                    
                    retVal += " 'Types': [";

                    foreach (Attraction.ListType type in filterValues.Keys)
                    {
                        retVal += " { 'idx': " + ((int)type).ToString() + ", 'Categories': [ ";
                    
                        foreach (Attraction.Categories cat in filterValues[type].Keys)
                        {
                            retVal += " { 'idx': " + ((int)cat).ToString() + ", 'value': " + filterValues[type][cat].ToString().ToLower() + " }, ";
                        }
                    
                        retVal = retVal.Substring(0,retVal.LastIndexOf(','));

                        retVal += " ] }, ";    
                    }

                    retVal = retVal.Substring(0,retVal.LastIndexOf(','));

                    retVal += "] } }";

                    retVal.Trim();
                    return retVal;
               }
            }
            #endregion
        }

        /// <summary>
        /// Class representing search event argumens
        /// </summary>
        [ScriptableType]
        public class SearchEventArgs : EventArgs
        {
            #region Private Properties
            /// <summary>
            /// Search term
            /// </summary>
            private string term;

            /// <summary>
            /// Destination
            /// </summary>
            private string dest;
            #endregion

            #region Constructor
            /// <summary>
            /// Search argument constructor
            /// </summary>
            /// <param name="searchTerm">Search term</param>
            /// <param name="destinationName">Destination name</param>
            public SearchEventArgs(string searchTerm, string destinationName)
            {
                term = searchTerm;
                dest = destinationName;
            }
            #endregion

            #region Scriptable Properties
            /// <summary>
            /// The keywords that a user searchs for
            /// </summary>
            [ScriptableMember]
            public string SearchText
            {
                get { return term; }
            }

            /// <summary>
            /// The destination city to perform search
            /// </summary>
            [ScriptableMember]
            public string DestinationName
            {
                get { return dest; }
            }
            #endregion
        }
        
        #endregion

        #region Event Argument Classes
        /// <summary>
        /// This class implements the arguments of the event that occurs
        /// when an attracion is dragged over the attraction drop box
        /// </summary>
        public class DropBoxStateEventArgs : EventArgs
        {
            #region Private Properties
            /// <summary>
            /// Drop box state
            /// </summary>
            private bool state;

            /// <summary>
            /// Insert/remove flag
            /// </summary>
            private bool insert;
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="state">Active or inactive</param>
            /// <param name="insert">Insert of delete</param>
            public DropBoxStateEventArgs(bool state, bool insert)
            {
                this.state = state;
                this.insert = insert;
            }
            #endregion

            #region Scriptable Public Properties
            // <summary>
            /// Drop box state
            /// </summary>
            [ScriptableMember]
            public bool State
            {
                get { return state; }
            }

            /// <summary>
            /// Insert/remove flag
            /// </summary>
            [ScriptableMember]
            public bool Insert
            {
                get { return insert; }
            }
            #endregion
        }

        /// <summary>
        /// Amount of total attractions argument
        /// </summary>
        [ScriptableType]
        public class AttractionCountEventArgs : EventArgs
        {
            #region Private Properties
            /// <summary>
            /// Number of attraction arguments
            /// </summary>
            int num = 0;
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor accepts number of arguments
            /// </summary>
            /// <param name="count">Number of arguments</param>
            public AttractionCountEventArgs(int count)
            {
                num = count;
            }
            #endregion

            #region Public Scriptable Properties
            /// <summary>
            /// Number of arguments
            /// </summary>
            [ScriptableMember]
            public int Count
            {
                get { return num; }
            }
            #endregion
        }
        #endregion

        #region Private Helper Classes
        /// <summary>
        /// Functional class that defines logics of guided tour
        /// </summary>
        private class ShowTourClass
        {
            #region Private Properties
            /// <summary>
            /// Attraction
            /// </summary>
            private Attraction attraction;
            #endregion

            #region Constructor
            /// <summary>
            /// Constructor initializes an attraction
            /// </summary>
            /// <param name="attr">Attraction</param>
            public ShowTourClass(Attraction attr)
            {
                attraction = attr;
            }
            #endregion

            #region Public Methods
            /// <summary>
            /// Show tour worker
            /// </summary>
            public void ShowTourWorker()
            {

                Controller.GetInstance().UIWorkQueue.Enqueue(new UIWorkDelegate(DoHideTour));

                Controller.GetInstance().waitCondition = true;

                while (Controller.GetInstance().waitCondition || Controller.GetInstance().tourPaused)
                {
                    if (!Controller.GetInstance().tourInProgress) return;
                    Thread.Sleep(100);
                }

                if (!Controller.GetInstance().tourInProgress) return;


                if (Controller.GetInstance().ShowTourItemEvent != null)
                    Controller.GetInstance().ShowTourItemEvent(attraction);

                while (Controller.GetInstance().tourPaused)
                {
                    if (!Controller.GetInstance().tourInProgress) return;
                    Thread.Sleep(100);
                }

                if (!Controller.GetInstance().tourInProgress) return;

                Controller.GetInstance().UIWorkQueue.Enqueue(new UIWorkDelegate(DoShowTour));

            }
            #endregion

            #region Private helper methods
            /// <summary>
            /// Hide tour
            /// </summary>
            private void DoHideTour()
            {

                if (Controller.GetInstance().tourInProgress)
                    Controller.GetInstance().HideTourItem(this, new EventArgs());
            }

            private void DoShowTour()
            {
                if (!Controller.GetInstance().tourInProgress) return;

                Controller.GetInstance().ShowTourItem(this, new AttractionEventArgs(attraction.Serialize(), Model.GetInstance().LoggedIn));
            }
            #endregion
        }

        /// <summary>
        /// Class that implements the keyword search
        /// </summary>
        private class MapSearchHelper
        {
            #region Public Methods
            /// <summary>
            /// Search map
            /// </summary>
            /// <param name="searchTerms">Search terms to lookup</param>
            /// <returns>List of attractions</returns>
            public void SearchMap(string searchTerms, AttractionListDelegate callback)
            {
                List<Attraction> retVal = new List<Attraction>();

                Model.GetInstance().GetConciergeList(Controller.GetInstance().currentDestinationId,
                    delegate(List<Attraction> attractions)
                    {
                        foreach (Attraction att in attractions)
                        {
                            if (att.MatchesSearchTerms(searchTerms))
                            {
                                Attraction copyAtt = att.Clone();
                                copyAtt.List = Attraction.ListType.PushpinSearch;
                                retVal.Add(copyAtt);
                            }
                        }

                        callback(retVal);
                    });
            }
            #endregion
        }
        #endregion

        #region Data Retrieval

        /// <summary>
        /// Get itinerary based on the day
        /// </summary>
        /// <param name="day">Day to retrieve</param>
        /// <returns>Itinerary list of attractions</returns>
        public void GetItinerary(DateTime day, AttractionListDelegate callback)
        {
            Model.GetInstance().GetItinerary(currentDestinationId, day, callback);
        }
        
        /// <summary>
        /// Get user itinerary. DISABLES SCHEDULING FUNCTIONS
        /// </summary>
        /// <returns>List of attractions</returns>
        public void GetItinerary(AttractionListDelegate callback)
        {
            Model.GetInstance().GetItinerary(currentDestinationId, DateTime.MinValue, callback);
        }

        /// <summary>
        /// Get user collection
        /// </summary>
        /// <returns>Collection for save my day places</returns>
        public void GetMyDestinations(DestinationListDelegate callback)
        {
            Model.GetInstance().GetMyDestinations(
                delegate(List<Destination> destinations)
                {
                    callback(destinations);
                });
        }

        /// <summary>
        /// Get itinerary based on the day
        /// </summary>
        /// <param name="day">Day to retrieve</param>
        /// <returns>Itinerary list of attractions</returns>
        public void GetSharedItinerary(DateTime day, AttractionListDelegate callback)
        {
            Model.GetInstance().GetSharedItinerary(currentDestinationId, day, callback);
        }

        /// <summary>
        /// Get itinerary based on the day. DISABLES SCHEDULING FUNCTIONS
        /// </summary> 
        public void GetSharedItinerary(AttractionListDelegate callback)
        {
            Model.GetInstance().GetSharedItinerary(currentDestinationId, DateTime.MinValue, callback);
        }

        /// <summary>
        /// Gets shared collection 
        /// </summary>
        /// <param name="applyFilter">Flag to indicate if shared filter is applied</param>
        /// <returns>List of attractions</returns>
        public void GetSharedItinerary(bool applyFilter, AttractionListDelegate callback)
        {
            if (!applyFilter)
            {
                GetSharedItinerary(callback);
            }
            else
            {
                List<Attraction> retVal = new List<Attraction>();

                GetSharedItinerary(
                    delegate(List<Attraction> attractions)
                    {
                        foreach (Attraction att in attractions)
                        {
                            if (filterMatrix[Attraction.ListType.Shared][att.Category])
                                retVal.Add(att);
                        }

                        callback(retVal);
                    });
            }
        }

        /// <summary>
        /// Get the concierge list
        /// </summary>
        /// <returns>Concierge list of attractions</returns>
        public void GetConciergeList(AttractionListDelegate callback)
        {
            GetConciergeList(false, callback);
        }

        /// <summary>
        /// Get the concierge list
        /// </summary>
        /// <param name="applyFilter">Flag to indicate if shared filter is applied</param>
        /// <returns>Concierge list of attractions</returns>
        public void GetConciergeList(bool applyFilter, AttractionListDelegate callback)
        {
            if (!applyFilter)
            {
                Model.GetInstance().GetConciergeList(currentDestinationId, callback);
            }
            else
            {
                List<Attraction> retVal = new List<Attraction>();

                Model.GetInstance().GetConciergeList(currentDestinationId,
                    delegate(List<Attraction> attractions)
                    {
                        foreach (Attraction att in attractions)
                        {
                            if (filterMatrix[Attraction.ListType.Concierge][att.Category])
                                retVal.Add(att);
                        }

                        callback(retVal);
                    });
            }
        }

        /// <summary>
        /// Get the search results list
        /// </summary>
        /// <returns>Search results list of attractions</returns>
        public List<Attraction> GetWebSearchList()
        {
            return Model.GetInstance().GetSearchResultList();
        }

        /// <summary>
        /// Get the search results list
        /// </summary>
        /// <returns>Search results list of attractions</returns>
        public List<Attraction> GetConciergeSearchList()
        {
            return mapSearchList;
        }
        #endregion
        
        #region Data Update
        /// <summary>
        /// Save the day
        /// </summary>
        public void SaveDay()
        {
            Model vp = Model.GetInstance();
            if (vp.LoggedIn)
            {
                vp.SaveMyDay();
            }
        
        }

        #endregion
        
        #region Public Methods
        /// <summary>
        /// Called when a search is performed
        /// </summary>
        /// <param name="searchTerms">Keywords</param>
        public void Search(string searchTerms)
        {
            searchHelper.SearchMap(searchTerms,
               delegate(List<Attraction> mapSearchList)
               {
                   DoSearch(this, new SearchEventArgs(searchTerms, currentDestinationName));
               });
        }

        /// <summary>
        /// Work queue
        /// </summary>
        public Queue<UIWorkDelegate> UIWorkQueue
        {
            get { return workDispatcher.UIWorkQueue; }
        }

        /// <summary>
        /// Initialize check box filters
        /// </summary>
        /// <param name="filterValues"></param>
        public void SetAttractionFilter(IDictionary<Attraction.ListType, IDictionary<Attraction.Categories, bool>> filterValues)
        {
            try
            {

                foreach (Attraction.ListType type in filterValues.Keys)
                {
                    foreach (Attraction.Categories cat in filterValues[type].Keys)
                    {
                        filterMatrix[type][cat] = filterValues[type][cat];
                    }

                }

                if (FilterChanged != null)
                {
                    FilterChanged(this, new FilterChangedArgs(filterValues));
                }

                if (TourInitializer != null)
                {
                    GetAttractionsCount(
                        delegate(int count)
                        {
                            TourInitializer(this, new AttractionCountEventArgs(count));
                        });
                }

                StopTour();

                if (MovePlaceListHover != null)
                    MovePlaceListHover(this, new PlaceListPositionEventArgs(string.Empty, -500, -500));
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Tab selection (Concierge, My Places, Search)
        /// </summary>
        /// <param name="tab"></param>
        public void SelectTab(SideMenu.Tabs tab)
        {
            SelectTabEvent(this, new SideMenu.TabSelectEventArgs(tab));
        }

        /// <summary>
        /// User adds a POI to their collection
        /// </summary>
        /// <param name="attraction"></param>
        public void AddToItinerary(Attraction attraction)
        {
            //BYPASS SCHEDULING FUNCTIONALITY
            attraction.Date = DateTime.MinValue;

            attraction.List = Attraction.ListType.User;

            //add to model
            Model.GetInstance().AddToItinerary(currentDestinationId, currentDestinationName, attraction);

            //update listeners
            AddAttractionEvent(attraction);

            if (TourInitializer != null)
            {
                GetAttractionsCount(
                    delegate(int count)
                    {
                        TourInitializer(this, new AttractionCountEventArgs(count));

                        this.SaveDay();
                    });
            }
        }

        /// <summary>
        /// Admin adding a POI to the concierge list
        /// </summary>
        /// <param name="attraction"></param>
        public void AddToConcierge(Attraction attraction)
        {
            //add to model
            Model.GetInstance().AddToConcierge(currentDestinationId, attraction);

            //update listeners
            AddConciergeEvent(attraction);

            if (TourInitializer != null)
            {
                GetAttractionsCount(
                    delegate(int count)
                    {
                        TourInitializer(this, new AttractionCountEventArgs(count));
                    });
            }
        }

        /// <summary>
        /// An admin removing a item from concierge list
        /// </summary>
        /// <param name="attraction"></param>
        public void RemoveFromConcierge(Attraction attraction)  //similar to above
        {
            //add to model
            Model.GetInstance().RemoveFromConcierge(currentDestinationId, attraction);

            //update listeners
            AddConciergeEvent(attraction);  //this event listener just resets the map

            if (TourInitializer != null)
            {
                GetAttractionsCount(
                    delegate(int count)
                    {
                        TourInitializer(this, new AttractionCountEventArgs(count));
                    });
            }
        }

        /// <summary>
        /// Remove attraction from the itinerary
        /// </summary>
        /// <param name="attraction">Attraction to remove</param>
        public void RemoveFromItinerary(Attraction attraction)
        {
            //remove from model
            Model.GetInstance().RemoveFromItinerary(currentDestinationId, attraction);

            //update listeners
            RemoveAttractionEvent(attraction);

            if (TourInitializer != null)
            {
                GetAttractionsCount(
                    delegate(int count)
                    {
                        TourInitializer(this, new AttractionCountEventArgs(count));
                    });
            }
        }

        /// <summary>
        /// Show the itinerary.  Invokes ShowItineraryEvent.
        /// </summary>
        public void ShowItinerary()
        {
            ShowItineraryEvent(this, new EventArgs());
        }

        /// <summary>
        /// Hides the itinerary.  Invokes the HideItinerary event
        /// </summary>
        public void HideItinerary()
        {
            HideItineraryEvent(this, new EventArgs());
        }

        /// <summary>
        /// Returns the attraction ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public void GetAttractionById(string ID, AttractionDelegate callback)
        {
            tempID = ID;

            //A single attraction object
            GetConciergeList(
                delegate(List<Attraction> attractions)
                {
                    Attraction retVal = attractions.FirstOrDefault(MatchesGuid);

                    if (retVal != null)
                    {
                        callback(retVal);
                        return;
                    }

                    retVal = GetWebSearchList().FirstOrDefault(MatchesGuid);
                    if (retVal != null)
                    {
                        callback(retVal);
                        return;
                    }

                    GetItinerary(
                        delegate(List<Attraction> itinerary)
                        {
                            retVal = itinerary.FirstOrDefault(MatchesGuid);

                            if (retVal != null)
                            {
                                callback(retVal);
                                return;
                            }

                            GetSharedItinerary(
                                delegate(List<Attraction> shared)
                                {
                                    retVal = shared.FirstOrDefault(MatchesGuid);

                                    if (retVal != null)
                                    {
                                        callback(retVal);
                                        return;
                                    }

                                    callback(GetConciergeSearchList().FirstOrDefault(MatchesGuid));
                                });
                        });
                });
        }
        
        /// <summary>
        /// Enables attraction drag and drop box
        /// </summary>
        /// <param name="active"></param>
        /// <param name="insert"></param>
        public void SetAttractionDropBoxState(bool active, bool insert)
        {
            if (Model.GetInstance().LoggedIn)
                AttractionDropBoxChanged(this, new DropBoxStateEventArgs(active, insert));
        }

        /// <summary>
        /// Guided tour popup
        /// </summary>
        /// <param name="attr">Attraction to display</param>
        public void ShowTourPopup(Attraction attr)
        {
            tourInProgress = false;
            Controller.GetInstance().UIWorkQueue.Clear();
            if (tourThread != null)
            {
                if (!tourThread.Join(100))
                {
                    try
                    {
                        tourThread.Abort();
                    }
                    catch (Exception)
                    {

                    }
                }
            }


            tourInProgress = true;

            ShowTourClass tClass = new ShowTourClass(attr);

            tourThread = new Thread(new ThreadStart(tClass.ShowTourWorker));

            tourThread.Start();

        }

        #endregion
        
        #region Public Scriptable methods
        
        /// <summary>
        /// Login and store user details
        /// </summary>
        /// <param name="userId">Windows live login</param>
        [ScriptableMember]        
        public void Login(string userId, string userType, string firstName, string lastName)
        {
            int id = -1;
            try
            {
                id = int.Parse(userId);
            }
            catch
            {
                //TODO: log id error
                return;
            }
            Model vp = Model.GetInstance();
            vp.VisitorId = id;
            vp.LoggedIn = true;

            if (userType == "admin")
                vp.IsAdmin = true;
            else
                vp.IsAdmin = false;

            vp.FirstName = firstName;
            vp.LastName = lastName;

            //Makes the "Add to my places" box top appear at the bottom right
            //Gets mapped to SetAttractionDropBoxState in VisitPlanner.js
            AttractionDropBoxChanged(this, new DropBoxStateEventArgs(true,true));

            if (LoginEvent != null)
            {
                LoginEvent(this, new EventArgs());
            }
        }
                
        /// <summary>
        /// Returns true if user is logged into Windows Live
        /// </summary>
        [ScriptableMember]
        public bool LoggedIn
        {
            get { return Model.GetInstance().LoggedIn; }
        }
        
        /// <summary>
        /// Save Personal Data
        /// </summary>
        /// <param name="userId"></param>
        [ScriptableMember]
        public void SavePersonal(string firstName, string lastName)
        {
            Model vp = Model.GetInstance();

            vp.FirstName = firstName;
            vp.LastName = lastName;

            vp.SavePersonal(delegate(){});
        }

        /// <summary>
        /// Returns boolean value of whether the user is an admin or not
        /// </summary>
        [ScriptableMember]
        public bool IsAdmin
        {
            get { return Model.GetInstance().IsAdmin; }
        }

        /// <summary>
        /// Logs user out of the application
        /// </summary>
        [ScriptableMember]
        public void Logout()
        {

            Model vp = Model.GetInstance();

            if (!vp.LoggedIn) return;

            Model.GetInstance().SaveMyDay();

            vp.VisitorId = -1;
            vp.LoggedIn = false;
            vp.IsAdmin = false;
            vp.FirstName = null;
            vp.LastName = null;

            AttractionDropBoxChanged(this, new DropBoxStateEventArgs(false,false));
        }

        /// <summary>
        /// If a visitor views the page from the shared link, set the shared ID
        /// </summary>
        /// <param name="sharedId"></param>
        [ScriptableMember]
        public void SetSharedUserId(string sharedId)
        {
            Model vp = Model.GetInstance();
            int id = -1;
            try
            {
                id = int.Parse(sharedId);
            }
            catch
            {
                //TODO: log id error
                return;
            }
            vp.SharedVisitorId = id;

            if (SharedDataEvent != null)
                SharedDataEvent(this, new EventArgs());
        }
                      
        /// <summary>
        /// Sets the search results list
        /// </summary>
        /// <param name="serialTexts">Serialized search results</param>
        [ScriptableMember]
        public void SetSearchResults(string serialTexts)
        {
            SearchResults searchResults;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(SearchResults));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(serialTexts)))
            {
                searchResults = serializer.ReadObject(memoryStream) as SearchResults;
            }

            Model.GetInstance().GetSearchResultList().Clear();

            foreach (Attraction attraction in searchResults.attractions)
            {
                Model.GetInstance().GetSearchResultList().Add(attraction);
            }

            if (SearchResultsChanged != null)
                SearchResultsChanged(this, new EventArgs());

            if (TourInitializer != null)
            {
                GetAttractionsCount(
                    delegate(int count)
                    {
                        TourInitializer(this, new AttractionCountEventArgs(count));
                    });
            }
        }

        /// <summary>
        /// Changes the itinerary/POIs based on which city destination is selected
        /// </summary>
        /// <param name="destinationID"></param>
        /// <param name="destinationName"></param>
        [ScriptableMember]
        public void SelectDestination(int destinationID, string destinationName)
        {
            currentDestinationId = destinationID;

            currentDestinationName = destinationName;

            Model.GetInstance().GetSearchResultList().Clear();

            this.GetAttractionsCount(
                delegate(int count)
                {
                    DestinationChangedEvent(this, new EventArgs());
                });
        }

        /// <summary>
        /// Get serialized attraction
        /// </summary>
        /// <param name="ID">Attraction ID</param>
        /// <returns>String of serialized attraction</returns>
        [ScriptableMember]
        public void GetSerializedAttraction(string ID, ScriptObject callback)
        {
            GetAttractionById(ID,
                delegate(Attraction attraction)
                {
                    callback.InvokeSelf(null, attraction.Serialize());
                });
        }

        /// <summary>
        /// Adds serialized itinerary.
        /// </summary>
        /// <param name="serialText">Add serialized itinerary</param>
        [ScriptableMember]
        public void AddToItinerary(string serialText)
        {
            if (Model.GetInstance().LoggedIn)
                AddToItinerary(Attraction.Deserialize(serialText));
        }

        /// <summary>
        /// Removes serialized itinerary.
        /// </summary>
        /// <param name="serialText">Remove serialized itinerary</param>
        [ScriptableMember]
        public void RemoveFromItinerary(string serialText)
        {
            if (Model.GetInstance().LoggedIn)
                RemoveFromItinerary(Attraction.Deserialize(serialText));
        }

        /// <summary>
        /// Adds serialized congierge item.
        /// </summary>
        /// <param name="serialText">Add serialized item to concierge</param>
        [ScriptableMember]
        public void AddToConcierge(string serialText)
        {
            if (Model.GetInstance().IsAdmin)
                AddToConcierge(Attraction.Deserialize(serialText));
        }

        /// <summary>
        /// Removes serialized congierge item.
        /// </summary>
        /// <param name="serialText">Removes serialized item to concierge</param>
        [ScriptableMember]
        public void RemoveFromConcierge(string serialText)
        {
            if (Model.GetInstance().IsAdmin)
                RemoveFromConcierge(Attraction.Deserialize(serialText));
        }

        

        /// <summary>
        /// Method is called when mouseover concierge list item occurs
        /// </summary>
        /// <param name="attr">Attracion object</param>
        /// <param name="x">x pixel of popup</param>
        /// <param name="y">y pixel of popup</param>
        public void ShowPlaceListHover(Attraction attr, int x, int y)
        {
            PlaceListPositionEventArgs e = new PlaceListPositionEventArgs(attr.Serialize(), x - 90, y - 12);
            MovePlaceListHover(this, new PlaceListPositionEventArgs(attr.Serialize(),x-90,y-12));
        }

        /// <summary>
        /// Stop tour 
        /// </summary>
        [ScriptableMember]
        public void StopTour()
        {
            if (HideTourItem == null || StopTourEvent == null) return;
            
            if (!tourInProgress)
            {
                HideTourItem(this, new EventArgs());
                return;
            }

            tourInProgress = false;
            tourPaused = false;
            Controller.GetInstance().UIWorkQueue.Clear();
            if (tourThread != null && !tourThread.Join(100))
            {
                try
                {
                    tourThread.Abort();
                }
                catch (Exception)
                {

                }
            }
            HideTourItem(this, new EventArgs());

            StopTourEvent(this, new EventArgs());
        }

        /// <summary>
        /// Show tour pop-up
        /// </summary>
        /// <param name="index">Index of the tour item</param>
        [ScriptableMember]
        public void ShowTourPopup(int index)
        {           
           int idx = 0;
           int count = 0;

           Controller.GetInstance().GetConciergeList(
               delegate(List<Attraction> conciergeList)
               {
                   count += conciergeList.Count;

                   List<Attraction> webList = Controller.GetInstance().GetWebSearchList();
                   count += webList.Count;

                   Controller.GetInstance().GetItinerary(
                       delegate(List<Attraction> itineraryList)
                       {
                           count += itineraryList.Count;

                           GetSharedItinerary(
                               delegate(List<Attraction> sharedList)
                               {
                                   count += sharedList.Count;

                                   while (idx < count)
                                   {
                                       Attraction tmpAtt;

                                       if (idx < conciergeList.Count)
                                       {
                                           tmpAtt = conciergeList[idx];

                                           if (filterMatrix[Attraction.ListType.Concierge][tmpAtt.Category])
                                           {
                                               if (index == 0)
                                               {
                                                   ShowTourPopup(tmpAtt);
                                                   return;
                                               }
                                               else
                                               {
                                                   index--;
                                               }
                                           }
                                       }
                                       else if (idx < conciergeList.Count + webList.Count)
                                       {
                                           tmpAtt = webList[idx - conciergeList.Count];

                                           if (filterMatrix[Attraction.ListType.Concierge][tmpAtt.Category])
                                           {
                                               if (index == 0)
                                               {
                                                   ShowTourPopup(tmpAtt);
                                                   return;
                                               }
                                               else
                                               {
                                                   index--;
                                               }
                                           }
                                       }
                                       else if (idx < conciergeList.Count + webList.Count + itineraryList.Count)
                                       {
                                           tmpAtt = itineraryList[idx - conciergeList.Count - webList.Count];

                                           if (filterMatrix[Attraction.ListType.User][tmpAtt.Category])
                                           {
                                               if (index == 0)
                                               {
                                                   ShowTourPopup(tmpAtt);
                                                   return;
                                               }
                                               else
                                               {
                                                   index--;
                                               }
                                           }
                                       }
                                       else
                                       {
                                           tmpAtt = sharedList[idx - conciergeList.Count - webList.Count - itineraryList.Count];

                                           if (filterMatrix[Attraction.ListType.Shared][tmpAtt.Category])
                                           {
                                               if (index == 0)
                                               {
                                                   ShowTourPopup(tmpAtt);
                                                   return;
                                               }
                                               else
                                               {
                                                   index--;
                                               }
                                           }
                                       }

                                       idx++;
                                   }
                               });
                       });
               });
        }

        /// <summary>
        /// Called when an attraction popup box is requested
        /// </summary>
        /// <param name="serialText">Serialized attraction</param>
        [ScriptableMember]
        public void ShowTourPopupBySerial(string serialText)
        {
            ShowTourPopup(Attraction.Deserialize(serialText));
        }

        /// <summary>
        /// Called when a single tour item has completed is showing
        /// </summary>
        [ScriptableMember]
        public void ThreadCompletionCallback()
        {
            waitCondition = false;
        }

        /// <summary>
        /// Pause guided tour
        /// </summary>
        [ScriptableMember]
        public void PauseTour()
        {
            tourPaused = true;
        }

        /// <summary>
        /// Resume guided tour
        /// </summary>
        [ScriptableMember]
        public void ResumeTour()
        {
            tourPaused = false;
        }
        
        /// <summary>
        /// Returns total number of attractions
        /// </summary>
        [ScriptableMember]
        public void GetAttractionsCount(IntDelegate callback)
        {
            int count = 0;
            GetConciergeList(
                delegate(List<Attraction> conciergeList)
                {
                    count += conciergeList.Count;

                    List<Attraction> webList = GetWebSearchList();
                    count += webList.Count;

                    GetItinerary(
                        delegate(List<Attraction> itineraryList)
                        {
                            count += itineraryList.Count;

                            GetSharedItinerary(
                                delegate(List<Attraction> sharedList)
                                {
                                    count += sharedList.Count;

                                    Attraction[] all = new Attraction[count];

                                    conciergeList.CopyTo(all, 0);
                                    webList.CopyTo(all, conciergeList.Count);
                                    itineraryList.CopyTo(all, conciergeList.Count + webList.Count);
                                    sharedList.CopyTo(all, conciergeList.Count + webList.Count + itineraryList.Count);

                                    int retVal = 0;

                                    foreach (Attraction att in all)
                                    {
                                        if (filterMatrix.ContainsKey(att.List) && filterMatrix[att.List].ContainsKey(att.Category) && filterMatrix[att.List][att.Category])
                                        {
                                            retVal++;
                                        }
                                    }

                                    callback(retVal);
                                });
                        });
                });
        }

        /// <summary>
        /// Returns a serialized string represting all of the viewable attractions on the map
        /// </summary>
        /// <returns></returns>
        [ScriptableMember]
        public void GetDisplayedAttractionsSerial(StringDelegate callback)
        {
            int count = 0;

            GetConciergeList(
                delegate(List<Attraction> conciergeList)
                {
                    count += conciergeList.Count;

                    List<Attraction> webList = GetWebSearchList();
                    count += webList.Count;

                    GetItinerary(
                        delegate(List<Attraction> itineraryList)
                        {
                            count += itineraryList.Count;

                            GetSharedItinerary(
                                delegate(List<Attraction> sharedList)
                                {
                                    count += sharedList.Count;

                                    Attraction[] all = new Attraction[count];

                                    conciergeList.CopyTo(all, 0);
                                    webList.CopyTo(all, conciergeList.Count);
                                    itineraryList.CopyTo(all, conciergeList.Count + webList.Count);
                                    sharedList.CopyTo(all, conciergeList.Count + webList.Count + itineraryList.Count);

                                    string retVal = string.Empty;

                                    foreach (Attraction att in all)
                                    {
                                        if (filterMatrix.ContainsKey(att.List) && filterMatrix[att.List].ContainsKey(att.Category) && filterMatrix[att.List][att.Category])
                                        {
                                            retVal += att.Serialize() + "|";
                                        }
                                    }

                                    if (retVal != String.Empty)
                                    {
                                        retVal = retVal.Substring(0, retVal.Length - 1);
                                    }

                                    callback(retVal);
                                });
                        });
                });
        }

        /// <summary>
        /// Gets the home (hotel) attraction 
        /// </summary>
        /// <returns></returns>
        [ScriptableMember]
        public void GetHomeSerial(StringDelegate callback)
        {
            GetConciergeList(
                delegate(List<Attraction> attractions)
                {
                    if (attractions.Count > 0)
                    {
                        callback(attractions[0].Serialize());
                    }
                    else
                    {
                        // TODO: What happens if we don't have a hotel to reference?
                    }
                });
        }

        #endregion

    }
}
