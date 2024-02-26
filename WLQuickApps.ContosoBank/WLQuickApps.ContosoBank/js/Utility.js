/// <reference path="VEJS/VeJavaScriptIntellisenseHelper.js" />
/// <reference name="MicrosoftAjax.js" />

var Utility = {
    /// <summary>
    ///   static Utility class
    /// </summary>

    OnFailed: function(error) {
        /// <summary>
        ///     This is the failed callback function for all webservices.
        /// </summary>  
        /// <param name="error">The error object from the webservice</param>          
        
        var stackTrace = error.get_stackTrace();
        var message = error.get_message();
        var statusCode = error.get_statusCode();
        var exceptionType = error.get_exceptionType();
        var timedout = error.get_timedOut();
       
        // Display the error.    
        var RsltElem = 
            "Stack Trace: " +  stackTrace + "<br/>" +
            "Service Error: " + message + "<br/>" +
            "Status Code: " + statusCode + "<br/>" +
            "Exception Type: " + exceptionType + "<br/>" +
            "Timedout: " + timedout;
            
            alert(RsltElem);
    },
       
    decodeLine: function(encoded) {
        /// <summary>
        ///     Decode an encoded string into a list of VE lat/lng.
        /// </summary>  
        /// <param name="encoded">The encoded string</param>       
        /// <returns>Array of VELatLong</returns>
       
        var len = encoded.length;
        var index = 0;
        var array = [];
        var lat = 0;
        var lng = 0;
        try
        {
            while (index < len) {
                var b;
                var shift = 0;
                var result = 0;
                do {
                      b = encoded.charCodeAt(index++) - 63;
                      result |= (b & 0x1f) << shift;
                      shift += 5;
                } while (b >= 0x20);
                var dlat = ((result & 1) ? ~(result >> 1) : (result >> 1));
                lat += dlat;

                shift = 0;
                result = 0;
                do {
                      b = encoded.charCodeAt(index++) - 63;
                      result |= (b & 0x1f) << shift;
                      shift += 5;
                } while (b >= 0x20);
                var dlng = ((result & 1) ? ~(result >> 1) : (result >> 1));
                lng += dlng;

                array.push(new VELatLong((lat * 1e-5), (lng * 1e-5)));
            }
        } catch(ex) {
            //error in encoding.
        }
        return array;
    },  

    createEncodings: function(points) {
        /// <summary>
        ///     Create the encoded bounds.
        /// </summary>  
        /// <param name="points">Array of VELatLong</param>       
        /// <returns>The encoded string</returns>    
        var i = 0;
        var plat = 0;
        var plng = 0;
        var encoded_points = "";

        for(i = 0; i < points.length; ++i) {
            var point = points[i];
            var lat = point.Latitude;
            var lng = point.Longitude;

            var late5 = Math.floor(lat * 1e5);
            var lnge5 = Math.floor(lng * 1e5);

            dlat = late5 - plat;
            dlng = lnge5 - plng;

            plat = late5;
            plng = lnge5;

            encoded_points += this._encodeSignedNumber(dlat) + this._encodeSignedNumber(dlng);
        } 
        return encoded_points;
    },
    
    _encodeSignedNumber: function(num) {
        /// <summary>
        ///     Encode a signed number in the encode format.
        /// </summary>  
        /// <param name="num">signed number</param>       
        /// <returns>encoded string</returns>       
        var sgn_num = num << 1;

        if (num < 0) {
            sgn_num = ~(sgn_num);
        }

        return(this._encodeNumber(sgn_num));
    },

    _encodeNumber: function(num) {
        /// <summary>
        ///     Encode an unsigned number in the encode format.
        /// </summary>  
        /// <param name="num">unsigned number</param>       
        /// <returns>encoded string</returns>        
        var encodeString = "";

        while (num >= 0x20) {
            encodeString += (String.fromCharCode((0x20 | (num & 0x1f)) + 63));
            num >>= 5;
        }

        encodeString += (String.fromCharCode(num + 63));
        return encodeString;
    },
    
    GetBounds: function(_map) {
        /// <summary>
        ///     Encodes the current map bounds.
        /// </summary>  
        /// <param name="_map">The VE Map object to get the map bounds from</param>       
        /// <returns>encoded string</returns>       
        var points = new Array();    
        if (_map.GetMapStyle() == VEMapStyle.Birdseye) {          
            var be = _map.GetBirdseyeScene();
            var rect = be.GetBoundingRectangle();
            points.push(rect.TopLeftLatLong);
            points.push(rect.BottomRightLatLong);
        }else {
            var view = _map.GetMapView();
            //Bounds must be NW, SE. Reorder if not
            if (view.TopLeftLatLong.Latitude < view.BottomRightLatLong.Latitude) {
                var temp = view.TopLeftLatLong.Latitude;
                view.TopLeftLatLong.Latitude = view.BottomRightLatLong.Latitude;
                view.BottomRightLatLong.Latitude = temp;
            }
            if (view.TopLeftLatLong.Longitude > view.BottomRightLatLong.Longitude) {
                var temp = view.TopLeftLatLong.Longitude;
                view.TopLeftLatLong.Longitude = view.BottomRightLatLong.Longitude;
                view.BottomRightLatLong.Longitude = temp;
            }            
            
            points.push(view.TopLeftLatLong);
            points.push(view.BottomRightLatLong);
        }
        return Utility.createEncodings(points);
    },    
    
    GetCirclePoints: function(impactPoint, radius) {
        /// <summary>
        ///     generates 360 points to approximate a circle.
        /// </summary>  
        /// <param name="impactPoint">VELatLong object marking centre</param>    
        /// <param name="radius">radius in Km</param>
        /// <returns>array of VELatLong values</returns> 
            
        var R = 3959.872469777;// earth's mean radius
        var lat = (impactPoint.Latitude * Math.PI) / 180; //rad
        var lon = (impactPoint.Longitude * Math.PI) / 180; //rad
        var d = parseFloat(radius)/R;  // d = angular distance covered on earth's surface
        var locs1 = new Array();
        var locs2 = new Array();
        var locs = new Array();
        var alreadyalerted = false;
        var absLongBoundary = 180;
        var absLatBoundary = 89.99995;
        for (var x = 0; x <= 360; x+=20) 
        { 
            var p2 = new VELatLong(0,0)            
            brng = x * Math.PI / 180; //rad
            p2.Latitude = Math.asin(Math.sin(lat)*Math.cos(d) + Math.cos(lat)*Math.sin(d)*Math.cos(brng));
            p2.Longitude = ((lon + Math.atan2(Math.sin(brng)*Math.sin(d)*Math.cos(lat), Math.cos(d)-Math.sin(lat)*Math.sin(p2.Latitude))) * 180) / Math.PI;
            p2.Latitude = (p2.Latitude * 180) / Math.PI;
            
            var absLong = Math.abs(p2.Longitude);
            var absLat = Math.abs(p2.Latitude);                
            if (absLong > absLongBoundary || absLat > absLatBoundary)
            {
                if (absLong > absLongBoundary)
                {
                    if (p2.Longitude > 0)
                        p2.Longitude = (p2.Longitude - absLongBoundary) - absLongBoundary;
                    else
                        p2.Longitude = (p2.Longitude + absLongBoundary) + absLongBoundary;
                }
                if (absLat > absLatBoundary)
                {
                    if (p2.Latitude > 0)
                        p2.Latitude = (p2.Latitude - absLatBoundary) - absLatBoundary;
                    else
                        p2.Latitude = (p2.Latitude + absLatBoundary) + absLatBoundary;
                }
                locs2.push(p2);
            }
            else
            {
            locs1.push(p2); 
            }
        }
        locs.push(locs1);
        locs.push(locs2);
        return locs; 
    }
}

if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();

