// web application root
var WEBROOT = null;

// The Virtual Earth Map
var TABSTATE = null;

// The Virtual Earth Map
var MAP = null;

// The currently selected Spaces Photo Album
var ALBUM = null;

// The currently selected maps.live.com collection id
var CID = null;

// The currently selected Silverlight Streaming video location
var VIDEO = null;

// Whether the video is Paused
var PAUSED = null;

// The layer used for the maps.live.com collection
var COLLECTION_LAYER = new VEShapeLayer();

// The shape representing the Cyclist's current position              
var CYCLIST_SHAPE = null;

// Pushpin counter
var PINID = 0;




// initialize map on page load
if (typeof(Sys) !== 'undefined') Sys.Application.add_init(InitMap);





// Called when the HTML body loads   
function InitMap()
{
    getMap();
		loadCollection(CID);
		InitTabs();
}



// check to see if VIDEO, ALBUM and CID vars have been populated
// and display the appropriate tabs and content.
function InitTabs()
{
	var slideshowTab = $get("SlideShowTab");
	var videoTab = $get("VideoTab");
	var directionsTab = $get("DirectionsTab");
	
	if (ALBUM && ALBUM != "") {
		$addHandler(slideshowTab, "click", viewPhotos);
		slideshowTab.style.visibility = "visible";
		viewPhotos();
	} else {
		slideshowTab.style.display = "none";
	}

	if (VIDEO && VIDEO != "") {
		$addHandler(videoTab, "click", showVideo);
		videoTab.style.visibility = "visible";
		if ((!ALBUM || ALBUM == "")) showVideo();
	} else {
		videoTab.style.display = "none";
	}

	if (CID && CID != "") {
		$addHandler(directionsTab, "click", viewDirections);
		directionsTab.style.visibility = "visible";
		if ((!VIDEO || VIDEO == "") && (!ALBUM || ALBUM == "")) viewDirections();
	} else {
		directionsTab.style.display = "none";
	}
}










// Initialises the Virtual Earth Map
function getMap()
{
    MAP = new VEMap('MapPanel'); 
    MAP.LoadMap();
    MAP.SetScaleBarDistanceUnit(VEDistanceUnit.Kilometers);
    
    MAP.AttachEvent("oninitmode", onInitMode);

       
    // Set tile overfetching
    MAP.SetTileBuffer(1);

}   

// Loads a maps.live.com collection
function loadCollection(cid)          
{         
    // Delete any pre existing shapes from the layer
    COLLECTION_LAYER.DeleteAllShapes();

    // Create the new layer with the Collection
    var veLayerSpec = new VEShapeSourceSpecification(VEDataType.VECollection, cid, COLLECTION_LAYER);            
    
    // Import the layer
    MAP.ImportShapeLayerData(veLayerSpec, onFeedLoad, true);  
          
}

// Zooms and pans the map to the specified latitude and longitude
function zoom (lat, lon) {
    var ZOOM = 14;

		MAP.HideInfoBox();

	  if (MAP.GetZoomLevel != ZOOM)
        MAP.SetZoomLevel(ZOOM);

    MAP.PanToLatLong(new VELatLong(lat, lon));
}


function showShapeInfoBox (id) {
	var shape = COLLECTION_LAYER.GetShapeByID(id);
	MAP.HideInfoBox();
//	MAP.SetCenter(new VELatLong(shape.Latitude, shape.Longitude));
	shape.SetZIndex(1500);
	MAP.ShowInfoBox(shape);
}



// Derives a set of directions from the pushpins in the maps.live.com collection
function getDirections() 
{
    // Get the directional panel
    var directionsPanel = $get('DirectionsPanel');
    
    // define the HTML string
    var directionsHtml = '';
    
    // Setup the distance
    var totalDistance = 0.0;
    
    // Fetch the shape cound.
    var count = COLLECTION_LAYER.GetShapeCount();
   
    var MAP_ITEM_TITLE = 'Click to see point on map.';
    
    
    directionsHtml += "<ul>";
    
   
    // Enumerate the shapes 
    for (var i = 0 ; i < count ; i++) {
    
        // Get the shape
        var shape = COLLECTION_LAYER.GetShapeByIndex(i);
        
        // Setup distance variable
        var distance;
        
        // If the first item
        if(i == 0)
            distance = 0.0;
        else
            distance = CalculateDistance(COLLECTION_LAYER.GetShapeByIndex(i-1).Latitude,COLLECTION_LAYER.GetShapeByIndex(i-1).Longitude,shape.Latitude, shape.Longitude);
        
        // Add to the total distance
        totalDistance = totalDistance + distance;

        // build the directions HTML if the shape is a point (is persisted throughout the enumeration)
				if (shape.GetType() == "Point") {
        directionsHtml +=  '<li class="direction">' +
//                           '    <a title="' + MAP_ITEM_TITLE + '" href="JavaScript:zoom(' + shape.Latitude + ',' + shape.Longitude + ');showShapeInfoBox(\'' + shape.GetID() + '\')">' + shape.GetTitle() + '</a>' + 
                           '    <a title="' + MAP_ITEM_TITLE + '" href="JavaScript:zoom(' + shape.Latitude + ',' + shape.Longitude + ');" onmouseover="showShapeInfoBox(\'' + shape.GetID() + '\');" onmouseout="MAP.HideInfoBox();">' + shape.GetTitle() + '</a>' + 
                           '    <span class="distance">' + distance.toFixed(2) + ' km</span>' + 
                           '</li>';
        }
    }
    
    
    directionsHtml += "</ul>";
    
    
    
    // Add the totals
    directionsHtml +=   '<div class="DirectionsTotal">' +
                        '    <span class="label">Total Distance</span>' + 
                        '    <span class="distance">' + totalDistance.toFixed(2) + ' km</span>' + 
                        '</div>';
    
    // Output
    directionsPanel.innerHTML = directionsHtml;
}




// Called once the collection has loaded
function onFeedLoad(feed)         
{  
	replaceIcons();          
  getDirections();
}


function onInitMode(mapEvent) {
	replaceIcons();
}


function replaceIcons() {
    // Swap out standard pushpins for better icons
    var count = COLLECTION_LAYER.GetShapeCount();
    for(var i=0; i < count; ++i)
    {
      var shape = COLLECTION_LAYER.GetShapeByIndex(i);
      if (MAP.GetMapMode() == VEMapMode.Mode3D) {
				shape.SetCustomIcon(WEBROOT + "/assets/images/marker_waypoint.png");
			} else {
				shape.SetCustomIcon("<img src='" + WEBROOT + "/assets/images/marker_waypoint.png' class='cbc-VeWayPoint'/>");
			}
    }
}


// Shows a DIV tag
function showDiv(div, zIndex) {
    div.style.visibility = 'visible';
    div.style.zIndex = zIndex;
}

// Hides a DIV tag
function hideDiv(div) {
    div.style.zIndex = -1;
    div.style.visibility = 'hidden';
}

function selectTab(el) {
	el.className = "selected";
}

function unselectTab(el) {
	el.className = "";
}

// Shows the route directions
function viewDirections() {
	if (TABSTATE != "directions") {
		unselectTab($get('VideoTab'));
		unselectTab($get('SlideShowTab'));
		selectTab($get('DirectionsTab'));
	
		hideVideo();
		hideDiv($get('SlideShowPanel'));
		showDiv($get('DirectionsPanel'), 500);
		TABSTATE = "directions";
	}
}

// Shows the photo slide show
function viewPhotos() {
	if (TABSTATE != "photos") {
		unselectTab($get('VideoTab'));
		unselectTab($get('DirectionsTab'));
		selectTab($get('SlideShowTab'));

    hideVideo();
    hideDiv($get('DirectionsPanel')); 
    showDiv($get('SlideShowPanel'), 500);
    
    $get('SlideShowPanel').src = '../Photos.aspx?feed=' + ALBUM;
    TABSTATE = "photos";
	}
}

// Shows the video panel
function showVideo() {
	if (TABSTATE != "video") {
		unselectTab($get('SlideShowTab'));
		unselectTab($get('DirectionsTab'));
		selectTab($get('VideoTab'));

    hideDiv($get('SlideShowPanel'));
    hideDiv($get('DirectionsPanel')); 
    showDiv($get('VideoPanel'), 500);
    
    // If the video is paused, restart it
    if (PAUSED) {
        var mediaElement = $get('VideoControl').content.findName('VideoWindow');
        mediaElement.play();
    } else {
        StartPlayer(VIDEO, "VideoPanel");
    }
    TABSTATE = "video";
	}
}

// Hides the video panel
function hideVideo() {
    var div = $get('VideoPanel');
    
    if (div.style.visibility == 'visible') {
        $get('VideoControl').content.findName('VideoWindow').pause();
        PAUSED = true;
        hideDiv(div);
        deleteCyclist();
    }
}



function deleteCyclist()      
{         
    if(CYCLIST_SHAPE != null)         
    {            
        MAP.DeleteShape(CYCLIST_SHAPE);            
        CYCLIST_SHAPE = null;         
    }      
}

function addCyclist(point)      
{          
    CYCLIST_SHAPE = new VEShape(VEShapeType.Pushpin,  point);  
    if (MAP.GetMapMode() == VEMapMode.Mode3D) {
			CYCLIST_SHAPE.SetCustomIcon(WEBROOT + "/assets/images/marker_active.png");
		} else {
	    CYCLIST_SHAPE.SetCustomIcon("<img src='" + WEBROOT + "/assets/images/marker_active.png' class='cbc-VePosition'/>");        
		}
    CYCLIST_SHAPE.SetTitle('Cyclist');          
    CYCLIST_SHAPE.SetDescription('Map position ' + formatLatitude(point.Latitude) + ' ' + formatLongitude(point.Longitude));
    CYCLIST_SHAPE.SetZIndex(2000);
    PINID++;          
    MAP.AddShape(CYCLIST_SHAPE);
}

function moveCyclist(position)
{
    deleteCyclist();
    addCyclist(position);
}

// Called whenever a key frame marker is reached in the video
// For more information see: http://www.codeplex.com/WLQuickApps/Thread/View.aspx?ThreadId=12491
function onMarkerReached(sender, markerEventArgs)
{

    // The video marker text is in the format GPS, <latitude>, <longitude>
    // so split out the comma separated fields.
    var fields = markerEventArgs.Marker.Text.split(",");

    // If this is a valid marker
    if (fields.length > 0) {
        // Extract the GPS position
        var position = new VELatLong(fields[1]-0, fields[2]-0);
        
        // Pan the map to that position
        MAP.PanToLatLong(position);

        moveCyclist(position)
    }
}

// Formats a Latitude for display   
function formatLatitude(latitude)
{
    var compass;

    // Degrees
    var degrees = Math.floor(Math.abs(latitude));

    // Minutes
    var minutes = (Math.abs(latitude) - degrees) * 60.0;

    minutes = Math.round(minutes * 100) / 100;

    // North or South
    if (latitude >= 0)
        compass = "N";
    else
        compass = "S";

    return (String.format("{0}&deg;{1}{2}", degrees, minutes, compass));
}

// Formats a Longitude for display 
function formatLongitude( longitude)
{
    var compass;

    // Degrees
    var degrees = Math.floor(Math.abs(longitude));

    // Minutes
    var minutes = (Math.abs(longitude) - degrees) * 60.0;

    minutes = Math.round(minutes * 100) / 100;

    // East or West?
    if (longitude >= 0)
        compass = "E";
    else
        compass = "W";

    return (String.format("{0}&deg;{1}{2}", degrees, minutes, compass));
}

// Calculate the great-circle distance between two points – See http://en.wikipedia.org/wiki/Great-circle_distance
function CalculateDistance(startLatitude, startLongitude, endLatitude, endLongitude)
{
    var R = 6378.1; // km
    
    // Get the Latitude differential
    var dLat = (endLatitude-startLatitude).toRad();
    
    // Get the Longitude differential
    var dLon = (endLongitude-startLongitude).toRad(); 
    
    ///TODO - add comments.
    var a = Math.sin(dLat/2) * Math.sin(dLat/2) +
            Math.cos(startLatitude.toRad()) * Math.cos(endLatitude.toRad()) * 
            Math.sin(dLon/2) * Math.sin(dLon/2); 
    
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a)); 
    
    var d = R * c;

    return d;
}


Number.prototype.toRad = function() {  // convert degrees to radians
  return this * Math.PI / 180;
}

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded(); 
