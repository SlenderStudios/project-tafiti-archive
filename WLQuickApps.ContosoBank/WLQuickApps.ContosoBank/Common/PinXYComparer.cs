using System.Collections.Generic;

using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Common
{
    /// <summary>
    /// Comparers two pins and returns the sort postion.
    /// Sorts by y then by x
    /// </summary>
    public class PinXYComparer : IComparer<ClusteredPin>
    {
        #region IComparer<ClusteredPin> Members

        int IComparer<ClusteredPin>.Compare(ClusteredPin x, ClusteredPin y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is Nothing and y is Nothing, they're
                    // equal. 
                    return 0;
                }
                // If x is Nothing and y is not Nothing, y
                // is greater. 
                return -1;
            }

            // If x is not Nothing...
            if (y == null)
            {
                // ...and y is Nothing, x is greater.
                return 1;
            }

            // ...and y is not Nothing, compare the 
            // x values
            if (x.Loc.Lon > y.Loc.Lon)
            {
                //x is greater
                return 1;
            }

            if (x.Loc.Lon == y.Loc.Lon)
            {
                //compare the y values
                if (x.Loc.Lat > y.Loc.Lat)
                {
                    //x is greater
                    return 1;
                }

                if (x.Loc.Lat == y.Loc.Lat)
                {
                    //they're equal. 
                    return 0;
                }
                //y is greater
                return -1;
            }
            //y is greater
            return -1;
        }

        #endregion
    }
}