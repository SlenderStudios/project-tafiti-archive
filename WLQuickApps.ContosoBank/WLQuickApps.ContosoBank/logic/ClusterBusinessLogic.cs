using System;
using System.Collections.Generic;
using WLQuickApps.ContosoBank.Common;
using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Logic
{
    public static class ClusterBusinessLogic
    {
        private const int clusterheight = 20; //Cluster region height, all pin within this area are clustered
        private const int clusterwidth = 20; //Cluster region width, all pin within this area are clustered

        public static string GetClusteredMapData(string encodedBounds, int zoomLevel, bool users)
        {
            //decode the bounds into bounds object
            Bounds bounds = Utilities.DecodeBounds(encodedBounds);
            List<ClusteredPin> pins = users ? UserLogic.GetUserLocationsByBounds(bounds) : BranchLogic.GetBranchLocationsByBounds(bounds);

            //cluster the points based on the zoomlevel
            List<ClusteredPin> clusteredpins = cluster(pins, zoomLevel);

            //return the encoded data for the clusters
            return Utilities.EncodeCluster(clusteredpins);
        }

        private static List<ClusteredPin> cluster(List<ClusteredPin> pins, int zoomLevel)
        {
            //sort pins - must be ordered correctly.
            PinXYComparer pinComparer = new PinXYComparer();
            pins.Sort(pinComparer);

            List<ClusteredPin> clusteredPins = new List<ClusteredPin>();

            for (int index = 0; index < pins.Count; index++)
            {
                if (!pins[index].IsClustered) //skip already clusted pins
                {
                    ClusteredPin currentClusterPin = new ClusteredPin();
                    //create our cluster object and add the first pin
                    currentClusterPin.AddPin(pins[index]);
                    pins[index].IsClustered = true;

                    //look backwards in the list for any points within the range that are not already grouped, as the points are in order we exit as soon as it exceeds the range.  
                    addPinsWithinRange(pins, index, -1, currentClusterPin, zoomLevel);

                    //look forwards in the list for any points within the range, again we short out.  
                    addPinsWithinRange(pins, index, 1, currentClusterPin, zoomLevel);

                    clusteredPins.Add(currentClusterPin);
                }
            }
            return clusteredPins;
        }


        private static void addPinsWithinRange(List<ClusteredPin> pins, int index, int direction,
                                               ClusteredPin currentClusterPin, int zoomLevel)
        {
            bool finished = false;
            int searchindex = index + direction;
            while (!finished)
            {
                if (searchindex >= pins.Count || searchindex < 0)
                {
                    finished = true;
                }
                else
                {
                    if (!pins[searchindex].IsClustered)
                    {
                        if (Math.Abs(pins[searchindex].GetPixelX(zoomLevel) - pins[index].GetPixelX(zoomLevel)) <
                            clusterwidth) //within the same x range
                        {
                            if (Math.Abs(pins[searchindex].GetPixelY(zoomLevel) - pins[index].GetPixelY(zoomLevel)) <
                                clusterheight) //within the same y range = cluster needed
                            {
                                //add to cluster
                                currentClusterPin.AddPin(pins[searchindex]);

                                //stop any further clustering
                                pins[searchindex].IsClustered = true;
                            }
                        }
                        else
                        {
                            finished = true;
                        }
                    }
                    searchindex += direction;
                }
            }
        }
    }
}