using System.Collections.Generic;
using System.Linq;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class BranchLogic
    {
        public static List<ClusteredPin> GetBranchLocationsByBounds(Bounds bounds)
        {
            ContosoBankDataContext db = new ContosoBankDataContext();
            var temp = from branch in db.Branches
                       where
                           branch.Latitude > bounds.SE.Lat && branch.Latitude < bounds.NW.Lat &&
                           branch.Longitude > bounds.NW.Lon && branch.Longitude < bounds.SE.Lon
                       select
                           new ClusteredPin
                               {Loc = new LatLong {Lat = branch.Latitude, Lon = branch.Longitude}, RecordCount = 1};

            return temp.ToList();
        }
    }
}