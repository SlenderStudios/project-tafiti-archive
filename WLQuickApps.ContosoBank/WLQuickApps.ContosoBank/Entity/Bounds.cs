namespace WLQuickApps.ContosoBank.Entity
{
    /// <summary>
    /// Represents a bounded map area using a rectange based on a topleft (NW and bottom right (SE) latitude/longitude
    /// </summary>
    public class Bounds
    {
        #region Properties

        public LatLong NW { get; set; }
        public LatLong SE { get; set; }

        #endregion

        public Bounds()
        {
            //Set values to opposite to allow any new values to override
            NW = new LatLong {Lat = -90, Lon = 180};
            SE = new LatLong {Lat = 90, Lon = -180};
        }

        /// <summary>
        /// Expands the current bounds to include the supplied bounds
        /// </summary>
        /// <param name="bounds">the latitude/longitude to be included</param>
        public void IncludeInBounds(Bounds bounds)
        {
            if (bounds.SE.Lat < SE.Lat)
            {
                SE.Lat = bounds.SE.Lat;
            }
            if (bounds.NW.Lat > NW.Lat)
            {
                NW.Lat = bounds.NW.Lat;
            }
            if (bounds.SE.Lon > SE.Lon)
            {
                SE.Lon = bounds.SE.Lon;
            }
            if (bounds.NW.Lon < NW.Lon)
            {
                NW.Lon = bounds.NW.Lon;
            }
        }
    }
}