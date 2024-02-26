/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: MapPanel.xaml.cs
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
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Media.Imaging;

namespace VESilverlight.Primary
{
    /// <summary>
    /// Implements Silverlight Map Container
    /// </summary>
    [ScriptableType]
    public partial class MapPanel : UserControl
    {
        #region Properties

        Attraction tmpAttraction;
        private bool centering = false;

        private Attraction[] searchList = new Attraction[0];
        private Attraction[] pinSearchList = new Attraction[0];
        private Attraction[] conciergeList = new Attraction[0];
        private Attraction[] userList = new Attraction[0];
        private Attraction[] shareList = new Attraction[0];

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor. Get a handle to page controls.  Set 
        /// events
        /// </summary>
		public MapPanel()
		{
            this.InitializeComponent();

            HtmlPage.RegisterScriptableObject("MapPanel", this);

            
            Controller.GetInstance().ShowTourItemEvent += new ItineraryListener(MapPanel_ShowTourItemEvent);
            
            Controller.GetInstance().DestinationChangedEvent += new EventHandler(MapPanel_DestinationChangedEvent);
            Controller.GetInstance().SearchResultsChanged += new EventHandler(MapPanel_SearchResultsChanged);

            Controller.GetInstance().AddAttractionEvent += new ItineraryListener(MapPanel_AddAttractionEvent);
            Controller.GetInstance().RemoveAttractionEvent += new ItineraryListener(MapPanel_RemoveAttractionEvent);

            Controller.GetInstance().AddConciergeEvent += new ItineraryListener(MapPanel_AddConciergeEvent);
            
            Controller.GetInstance().FilterChanged += new EventHandler<Controller.FilterChangedArgs>(MapPanel_FilterChanged);

            this.Loaded += new RoutedEventHandler(MapPanel_Loaded);
        }

        void MapPanel_Loaded(object sender, RoutedEventArgs e)
        {
            this.panelCorner.Source = new BitmapImage(new Uri("/images/panelCorner.png", UriKind.Relative));
        }

        #endregion

        #region Non-Scriptable Methods

        /// <summary>
        /// Attraction filters changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MapPanel_FilterChanged(object sender, Controller.FilterChangedArgs e)
        {
            ChangeMapFilter(sender, e);
        }

        /// <summary>
        /// Destination city changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MapPanel_DestinationChangedEvent(object sender, EventArgs e)
        {
            ResetMap();               
        }

        /// <summary>
        /// Resets items on map
        /// </summary>
        [ScriptableMember]
        public void ResetMap()
        {
            foreach (Attraction attr in searchList)
            {
                RemovePushpin(this, new PushpinEventArgs(attr));
            }

            foreach (Attraction attr in shareList)
            {
                RemovePushpin(this, new PushpinEventArgs(attr));
            }

            foreach (Attraction attr in conciergeList)
            {
                RemovePushpin(this, new PushpinEventArgs(attr));
            }

            foreach (Attraction attr in userList)
            {
                RemovePushpin(this, new PushpinEventArgs(attr));
            }

            foreach (Attraction attr in pinSearchList)
            {
                RemovePushpin(this, new PushpinEventArgs(attr));
            }

            InitializeMapData();
        }

        /// <summary>
        /// Attraction is deleted
        /// </summary>
        /// <param name="attraction"></param>
        void MapPanel_RemoveAttractionEvent(Attraction attraction)
        {
            ResetMap(); 
        }

        /// <summary>
        /// An Attraction is added
        /// </summary>
        /// <param name="attraction"></param>
        void MapPanel_AddAttractionEvent(Attraction attraction)
        {
            ResetMap(); 
        }

        /// <summary>
        /// A concierge POI is added
        /// </summary>
        /// <param name="attraction"></param>
        void MapPanel_AddConciergeEvent(Attraction attraction)
        {
            ResetMap();
        }

        /// <summary>
        /// Results of a search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MapPanel_SearchResultsChanged(object sender, EventArgs e)
        {
            foreach (Attraction attr in pinSearchList)
            {
                RemovePushpin(this, new PushpinEventArgs(attr));
            }
            
            foreach (Attraction attr in searchList)
            {
                RemovePushpin(this, new PushpinEventArgs(attr));
            }

            List<Attraction> tmpList = Controller.GetInstance().GetConciergeSearchList();
            pinSearchList = new Attraction[tmpList.Count];
            tmpList.CopyTo(pinSearchList);

            foreach (Attraction attr in pinSearchList)
            {
                AddPushpin(this, new PushpinEventArgs(attr));
            }

            tmpList = Controller.GetInstance().GetWebSearchList();
            searchList = new Attraction[tmpList.Count];
            tmpList.CopyTo(searchList);

            foreach (Attraction attr in searchList)
            {
                AddPushpin(this, new PushpinEventArgs(attr));
            } 
        }

        /// <summary>
        /// Displays tour popup item
        /// </summary>
        /// <param name="attraction"></param>
        void MapPanel_ShowTourItemEvent(Attraction attraction)
        {
            tmpAttraction = attraction;

            Controller.GetInstance().UIWorkQueue.Enqueue(new UIWorkDelegate(DoCenterMap));

            centering = true;

            //In order to have the tour item popup while the map is panning to the next
            //center location in the guided tour
            while (centering)
            {
                Thread.Sleep(500);
            }   
        }

        /// <summary>
        /// Centers map in between guided tour items
        /// </summary>
        private void DoCenterMap()
        {
            CenterMap(this, new PushpinEventArgs(tmpAttraction));
        }

        #endregion

        #region Scriptable Methods

        /// <summary>
        /// Initializes map panel - displays all pins on map from collections
        /// </summary>
        [ScriptableMember]
        public void InitializeMapData()
        {
            Controller.GetInstance().GetConciergeList(new AttractionListDelegate(
                delegate(List<Attraction> tmpList)
                {
                    conciergeList = new Attraction[tmpList.Count];
                    tmpList.CopyTo(conciergeList);

                    foreach (Attraction attr in conciergeList)
                    {
                        AddPushpin(this, new PushpinEventArgs(attr));
                    }

                    Controller.GetInstance().GetSharedItinerary(new AttractionListDelegate(
                        delegate(List<Attraction> tmpList2)
                        {
                            shareList = new Attraction[tmpList2.Count];
                            tmpList2.CopyTo(shareList);

                            foreach (Attraction attr in shareList)
                            {
                                AddPushpin(this, new PushpinEventArgs(attr));
                            }

                            Controller.GetInstance().GetItinerary(new AttractionListDelegate(
                                delegate(List<Attraction> tmpList3)
                                {
                                    userList = new Attraction[tmpList3.Count];
                                    tmpList3.CopyTo(userList);

                                    foreach (Attraction attr in userList)
                                    {
                                        AddPushpin(this, new PushpinEventArgs(attr));
                                    }

                                    List<Attraction> tmpList4 = Controller.GetInstance().GetWebSearchList();
                                    searchList = new Attraction[tmpList4.Count];
                                    tmpList4.CopyTo(searchList);

                                    foreach (Attraction attr in searchList)
                                    {
                                        AddPushpin(this, new PushpinEventArgs(attr));
                                    }

                                    List<Attraction> tmpList5 = Controller.GetInstance().GetConciergeSearchList();
                                    pinSearchList = new Attraction[tmpList5.Count];
                                    tmpList5.CopyTo(pinSearchList);

                                    foreach (Attraction attr in pinSearchList)
                                    {
                                        AddPushpin(this, new PushpinEventArgs(attr));
                                    }

                                    ChangeMapFilter(this, new Controller.FilterChangedArgs(Controller.GetInstance().FilterMatrix));
                                }));
                        }));
                }));
        }

        /// <summary>
        /// Resizes map panel based on browser resizing
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        [ScriptableMember]
        public void Resize(double width, double height)
        {
            Canvas parentCanvas = this.ParentCanvas;

            Rectangle mapRect = this.MapRect;
            Rectangle mapRectShadow = this.MapRectShadow;


            this.Width = width;
            //this.Height = height;

            backPanel.SetValue(Canvas.LeftProperty, Width - backPanel.Width);

            mapRect.Width = width - (parentCanvas.Width - mapRect.Width);
            mapRectShadow.Width = width - (parentCanvas.Width - mapRectShadow.Width);

            panelCorner.SetValue(Canvas.LeftProperty, mapRectShadow.Width + (double)mapRectShadow.GetValue(Canvas.LeftProperty) - panelCorner.Width + 2);

            //resize the map itself
            Canvas mapContainer = this.MapContainer;
            mapContainer.Width = width - (parentCanvas.Width - mapContainer.Width);

            Rectangle mapContainerShadow = this.MapContainerShadow;
            mapContainerShadow.Width = width - (parentCanvas.Width - mapContainerShadow.Width);
            
            if (SetMapSize!=null) SetMapSize(this,new ResizeEventArgs((int)mapContainer.Width, (int)mapContainer.Height));

            sideMenu.SetValue(Canvas.LeftProperty, (double)backPanel.GetValue(Canvas.LeftProperty) + 4);

            parentCanvas.Width = width;
            //parentCanvas.Height = height;
        }

        /// <summary>
        /// Resize event arguments
        /// </summary>
        [ScriptableType]
        public class ResizeEventArgs : EventArgs
        {
            public ResizeEventArgs(int width, int height)
            {
                this.width = width;
                this.height = height;
            }

            [ScriptableMember]
            public int Width
            {
                get { return this.width; }
            }

            private int width;
            
            [ScriptableMember]
            public int Height
            {
                get { return this.height; }
            }
            private int height;

        }
  
        /// <summary>
        /// Pushpin event arguments
        /// </summary>
        [ScriptableType]
        public class PushpinEventArgs : EventArgs
        {
            public PushpinEventArgs(Attraction attraction)
            {
                this.attraction = attraction;
            }

            [ScriptableMember]
            public int List
            {
                get { return (int)this.attraction.List; }
            }

            [ScriptableMember]
            public int Category
            {
                get { return (int)this.attraction.Category; }
            }

            [ScriptableMember]
            public string AttractionGuid
            {
                get { return this.attraction.ID; }             
            }

            [ScriptableMember]
            public double Latitude
            {
                get { return this.attraction.Latitude; }
            }

            [ScriptableMember]
            public double Longitude
            {
                get { return this.attraction.Longitude; }
            }

            [ScriptableMember]
            public string ImageURL
            {
                get { return this.attraction.ImageURL; }
            }
            [ScriptableMember]
            public string PushpinURL
            {
                get { return this.attraction.PushpinURL; }
            }
            private Attraction attraction;
        }

        /// <summary>
        /// Called when centering is done in between guided tour items
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        [ScriptableMember]
        public void CenterComplete(double latitude, double longitude)
        {
            if (tmpAttraction.Latitude == latitude && tmpAttraction.Longitude == longitude)
                centering = false;
            else
            {
                Controller.GetInstance().UIWorkQueue.Enqueue(new UIWorkDelegate(DoCenterMap));
            }

            
        }

        //Various scriptable events

        [ScriptableMember]
        public event EventHandler<ResizeEventArgs> SetMapSize;

        [ScriptableMember]
        public event EventHandler<PushpinEventArgs> AddPushpin;

        [ScriptableMember]
        public event EventHandler<PushpinEventArgs> RemovePushpin;

        [ScriptableMember]
        public event EventHandler<PushpinEventArgs> CenterMap;

        [ScriptableMember]
        public event EventHandler<Controller.FilterChangedArgs> ChangeMapFilter;

        #endregion
    }


}
