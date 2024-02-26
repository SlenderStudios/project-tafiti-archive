using System;
using System.Collections.Generic;
using System.Text;

using WLQuickApps.ContosoBank.Entity;

namespace WLQuickApps.ContosoBank.Common
{
    public static class Utilities
    {
        private const int binaryChunkSize = 5;
        private const double earthCircum = earthRadius * 2.0 * Math.PI; //calulated circumference of the earth
        private const double earthHalfCirc = earthCircum / 2; //calulated half circumference of the earth
        private const double earthRadius = 6378137; //The radius of the earth - should never change!
        private const int minASCII = 63;
        private const int pixelsPerTile = 256;

        public static int LatitudeToYAtZoom(double lat, int zoom)
        {
            double arc = earthCircum / ((1 << zoom) * pixelsPerTile);
            double sinLat = Math.Sin(DegToRad(lat));
            double metersY = earthRadius / 2 * Math.Log((1 + sinLat) / (1 - sinLat));
            return (int)Math.Round((earthHalfCirc - metersY) / arc);
        }

        public static int LongitudeToXAtZoom(double lon, int zoom)
        {
            double arc = earthCircum / ((1 << zoom) * pixelsPerTile);
            double metersX = earthRadius * DegToRad(lon);
            return (int)Math.Round((earthHalfCirc + metersX) / arc);
        }

        private static double DegToRad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static Bounds DecodeBounds(string encoded)
        {
            List<LatLong> locs = DecodeLatLong(encoded);
            //OverSize the bounds to allow for rounding errors in the encoding process.
            locs[0].Lat += 0.00001;
            locs[0].Lon -= 0.00001;
            locs[1].Lat -= 0.00001;
            locs[1].Lon += 0.00001;
            return new Bounds {NW = locs[0], SE = locs[1]};
        }

        public static List<LatLong> DecodeLatLong(string encoded)
        {
            List<LatLong> locs = new List<LatLong>();

            int index = 0;
            int lat = 0;
            int lng = 0;

            int len = encoded.Length;
            while (index < len)
            {
                lat += decodePoint(encoded, index, out index);
                lng += decodePoint(encoded, index, out index);

                locs.Add(new LatLong {Lat = (lat * 1e-5), Lon = (lng * 1e-5)});
            }

            return locs;
        }

        private static int decodePoint(string encoded, int startindex, out int finishindex)
        {
            int b;
            int shift = 0;
            int result = 0;
            do
            {
                //get binary encoding
                b = Convert.ToInt32(encoded[startindex++]) - minASCII;
                //binary shift
                result |= (b & 0x1f) << shift;
                //move to next chunk
                shift += binaryChunkSize;
            }
            while (b >= 0x20); //see if another binary value
            //if negivite flip
            int dlat = (((result & 1) > 0) ? ~(result >> 1) : (result >> 1));
            //set output index
            finishindex = startindex;
            return dlat;
        }

        public static string EncodeCluster(List<ClusteredPin> pins)
        {
            var encoded = new StringBuilder();
            //encode the locations
            var points = new List<LatLong>();
            foreach (ClusteredPin pin in pins)
            {
                points.Add(pin.Loc);
            }

            encoded.Append(EncodeLatLong(points));


            //encode the bounds per cluster
            foreach (ClusteredPin pin in pins)
            {
                //comma seperated
                encoded.Append(',');
                points = new List<LatLong> {pin.ClusterArea.NW, pin.ClusterArea.SE};
                encoded.Append(EncodeLatLong(points));
            }
            //add the recordout
            foreach (ClusteredPin pin in pins)
            {
                //comma seperated
                encoded.Append(',');
                encoded.Append(pin.RecordCount);
            }


            return encoded.ToString();
        }

        public static string EncodeLatLong(List<LatLong> points)
        {
            int plat = 0;
            int plng = 0;
            int len = points.Count;

            var encoded_points = new StringBuilder();

            for (int i = 0; i < len; ++i)
            {
                //Round to 5 decimal places and drop the decimal
                var late5 = (int)(points[i].Lat * 1e5);
                var lnge5 = (int)(points[i].Lon * 1e5);

                //encode the differences between the points
                encoded_points.Append(encodeSignedNumber(late5 - plat));
                encoded_points.Append(encodeSignedNumber(lnge5 - plng));

                //store the current point
                plat = late5;
                plng = lnge5;
            }
            return encoded_points.ToString();
        }

        private static string encodeSignedNumber(int num)
        {
            int sgn_num = num << 1; //shift the binary value
            if (num < 0) //if negative invert
            {
                sgn_num = ~(sgn_num);
            }
            return (encodeNumber(sgn_num));
        }

        private static string encodeNumber(int num)
        {
            var encodeString = new StringBuilder();
            while (num >= 0x20)
            {
                //while another chunk follows
                encodeString.Append((char)((0x20 | (num & 0x1f)) + minASCII));
                    //OR value with 0x20, convert to decimal and add 63
                num >>= binaryChunkSize; //shift to next chunk
            }
            encodeString.Append((char)(num + minASCII));
            return encodeString.ToString();
        }
    }
}