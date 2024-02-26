using WLQuickApps.ContosoBank.Common;

namespace WLQuickApps.ContosoBank.Entity
{
    /// <summary>
    /// A clustered pin is the basic object required to plot on the VE map.
    /// It has a location, a type and a bounds that it represents
    /// </summary>
    public class ClusteredPin
    {
        #region Properties

        public LatLong Loc { get; set; }
        public Bounds ClusterArea { get; set; }
        public bool IsClustered { get; set; }
        public int RecordCount { get; set; }

        #endregion

        private int pixelX = -1;
        private int pixelY = -1;

        public ClusteredPin()
        {
            ClusterArea = new Bounds();
        }

        /// <summary>
        /// Adds a pin to the cluster
        /// </summary>
        /// <param name="newPin">the pin to add</param>
        public void AddPin(ClusteredPin newPin)
        {
            if (Loc == null)
            {
                Loc = newPin.Loc;
            }
            ClusterArea.IncludeInBounds(newPin.ClusterArea);
            RecordCount += newPin.RecordCount;
        }

        /// <summary>
        /// Gets the x pixel location of the pin for the given zoomlevel
        /// location is stored. Assumption is made the zoomlevel does not change for the pin.
        /// </summary>
        /// <param name="zoomLevel">the current zoomlevel of the map</param>
        /// <returns>the x pixel location of the pin</returns>
        public int GetPixelX(int zoomLevel)
        {
            if (pixelX < 0)
            {
                pixelX = Utilities.LongitudeToXAtZoom(Loc.Lon, zoomLevel);
            }
            return pixelX;
        }

        /// <summary>
        /// Gets the y pixel location of the pin for the given zoomlevel
        /// location is stored. Assumption is made the zoomlevel does not change for the pin.
        /// </summary>
        /// <param name="zoomLevel">the current zoomlevel of the map</param>
        /// <returns>the y pixel location of the pin</returns>
        public int GetPixelY(int zoomLevel)
        {
            if (pixelY < 0)
            {
                pixelY = Utilities.LongitudeToXAtZoom(Loc.Lat, zoomLevel);
            }
            return pixelY;
        }
    }
}