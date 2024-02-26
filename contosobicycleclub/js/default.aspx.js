// The Virtual Earth Map
var MAP = null;

// The currently selected Spaces Photo Album
var ALBUM = null;

// The currently selected maps.live.com collection id
var CID = null;

// Whether the video is Paused
var PAUSED = false;

// The layer used for the maps.live.com collection
var COLLECTION_LAYER = new VEShapeLayer();

// The shape representing the Cyclist's current position              
var CYCLIST_SHAPE = null;

// Pushpin counter
var PINID = 0;

// The icon representing the Cyclist
var CYCLIST_ICON = new VECustomIconSpecification();
CYCLIST_ICON.Image = "images/bike_icon.png";

// Menu hover states
var IS_SHOWING_RIDE_MENU = false;
var IS_SHOWING_EVENTS_MENU = false;
 
// Called when the HTML body loads   
function onBodyLoad()
{
    getMap();
    showPage("home");
}
    
// Initailises the Virtual Earth Map
function getMap()
{
    MAP = new VEMap('MapPanel'); 
    MAP.LoadMap();
       
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
    if (MAP.GetZoomLevel != ZOOM)
        MAP.SetZoomLevel(ZOOM);

    MAP.PanToLatLong(new VELatLong(lat, lon));
}

// Derives a set of directions from the pushpins in the maps.live.com collection
function getDirections() 
{
    // Get the directional panel
    var directionsPanel = $('DirectionsPanel');
    
    // define the HTML string
    var directionsHtml = '';
    
    // Setup the distance
    var totalDistance = 0.0;
    
    // Fetch the shape cound.
    var count = COLLECTION_LAYER.GetShapeCount();
   
    var MAP_ITEM_TITLE = 'Click to see point on map.';
   
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



        // build the directions HTML (is persisted throughout the enumeration)
        directionsHtml +=  '<div class="direction">' +
                           '    <div class="distance">' + distance.toFixed(2) + ' miles</div>' + 
                           '    <a title="' + MAP_ITEM_TITLE + '" href="JavaScript:zoom(' + shape.Latitude + ',' + shape.Longitude + ')">' + shape.GetTitle() + '</a>' + 
                           '</div>';
    }
    
    // Add the totals
    directionsHtml +=   '<div class="DirectionsTotal direction heavy">' +
                        '    <div class="distance heavy">' + totalDistance.toFixed(2) + ' miles</div>' + 
                        '    Total Distance' + 
                        '</div>';
    
    // Add the header and add the distance to the header
    directionsHtml = '<h2>Directions (' + totalDistance.toFixed(2) + ' miles)</h2>' + directionsHtml;
    
    // Output
    directionsPanel.innerHTML = directionsHtml;
}


function showRideMenu()
{
    showDiv($('ridesMenuDiv'), 99999);
    IS_SHOWING_RIDE_MENU = true;
    hideEventsMenu();
}

function hideRideMenu() 
{
    hideDiv($('ridesMenuDiv'));
    IS_SHOWING_RIDE_MENU = false;
}

function toggleRideMenu()
{
    if(IS_SHOWING_RIDE_MENU)
    {
        hideRideMenu();
    }
    else
    {
        showRideMenu();
    }
}

function showEventsMenu() 
{
    showDiv($('eventsMenuDiv'), 99999);
    IS_SHOWING_EVENTS_MENU = true;
    hideRideMenu();
}

function hideEventsMenu() 
{
    hideDiv($('eventsMenuDiv'));
    IS_SHOWING_EVENTS_MENU = false;
}

function toggleEventsMenu()
{
    if(IS_SHOWING_EVENTS_MENU)
    {
        hideEventsMenu();
    }
    else
    {
        showEventsMenu();
    }
}

// Called once the collection has loaded
function onFeedLoad(feed)         
{  
    // Swap out standard pushpins for better icons
    var count = COLLECTION_LAYER.GetShapeCount();
    for(var i=0; i < count; ++i)
    {
      var shape = COLLECTION_LAYER.GetShapeByIndex(i);
      shape.SetCustomIcon("<img src='images/directions_pin.png'/>");
    }
          
    getDirections();
}

// Shows a DIV tag
function showDiv(div, zIndex) {
    div.style.visibility = 'visible';
    div.style.zIndex = zIndex;
    
    // Work around for Bugzilla Bug 187435 for Firefox on Mac
    if(div.id.toLowerCase() == "directionspanel" || div.id.toLowerCase() == "textpanel" || div.id.toLowerCase() == "mainpanel")
    {
    		div.style.overflow = "auto";
    }
}

// Hides a DIV tag
function hideDiv(div) {
    div.style.zIndex = -1;
    div.style.visibility = 'hidden';
    
    // Work around for Bugzilla Bug 187435 for Firefox on Mac
    if(div.id.toLowerCase() == "directionspanel" || div.id.toLowerCase() == "textpanel" || div.id.toLowerCase() == "mainpanel")
	{
		div.style.overflow = "hidden";
	}
}

// Shows the Main Panel. Loads content from the given URL
function showMainPanel(url) {
    var mainPanel = $('MainPanel');
    showDiv(mainPanel, 500);
    
    mainPanel.src = url;    
    loadHTML(url, mainPanel)
}

// Updates the text area with ride or event information
function updateText(feed, item)
{
    var textPanel = $('TextPanel');
    loadHTML( 'Item.aspx?feed=' +  feed + '&item=' + item, textPanel)
}

// Shows the route directions
function viewDirections() { 
    hideDiv($('TextPanel'));
    hideDiv($('SlideShowPanel'));
    showDiv($('DirectionsPanel'), 500); 
    hideVideo();
}

// Resets the route view on the map
function viewRoute() {
    loadCollection(CID);
}

// Shows the photo slide show
function viewPhotos() {
    hideDiv($('TextPanel'));
    showDiv($('SlideShowPanel'), 500);
    hideDiv($('DirectionsPanel')); 
    hideVideo();
    
    $('SlideShowPanel').src = 'Photos.aspx?feed=' + ALBUM ;
}

// Shows the video panel
function showVideo() {
    showDiv($('VideoWrapper'), 500);
    
    // If the video is paused, restart it
    if (PAUSED) {
        var mediaElement = $('VideoControl').content.findName('VideoWindow');
        mediaElement.play();
    } else {
        StartPlayer("VideoWrapper");
    }

}

// Hides the video panel
function hideVideo() {
    var div = $('VideoWrapper');
    
    if (div.style.visibility == 'visible') {
        document.getElementById('VideoControl').content.findName('VideoWindow').pause();
        PAUSED = true;
        hideDiv(div);
        deleteCyclist();
    }
}

// Shows the Bike Cam video
function viewBikeCam() {
    hideDiv($('TextPanel'));
    hideDiv($('SlideShowPanel'));
    hideDiv($('DirectionsPanel'));
    showVideo();   
}

// Shows the ride report
function viewRideReport() {
    showDiv($('TextPanel'), 500);
    hideDiv($('SlideShowPanel'));
    hideDiv($('DirectionsPanel'));
    hideVideo();   
}

// Opens the map using maps.live.com in a new window
function viewMap() {
    window.open('http://maps.live.com/?cid=' + CID); 
}

// Hides the entire ride view
function hideRideView() {
    hideRideMenu();
    hideEventsMenu();
    hideDiv($('TextPanel'));
    hideDiv($('SlideShowPanel'));
    hideDiv($('DirectionsPanel'));
    hideDiv($('MenuPanel'));
    hideDiv($('MapPanel'));
    hideVideo();
}

// Updates page styling according to the currently selected view
function showPage(pageName)
{
    switch(pageName.toLowerCase())
    {
        case "home":
            home();
            resetAllMenuStyle(pageName);
            $('HomeLink').style.color = "purple";
            $('HomeLink').style.fontWeight = "bold";
            hideDiv($('contentPanel'));
            break;
        case "ridereports":
            rideReports();
            resetAllMenuStyle(pageName);
            $('RidesLink').style.color = "purple";
            $('RidesLink').style.fontWeight = "bold";
            break;
        case "events":
            events();
            resetAllMenuStyle(pageName);
            $('EventsLink').style.color = "purple";
            $('EventsLink').style.fontWeight = "bold";
            break;
        case "forum":
            forum();
            resetAllMenuStyle(pageName);
            $('ForumLink').style.color = "purple";
            $('ForumLink').style.fontWeight = "bold";
            hideDiv($('contentPanel'));
            break;
        case "classifieds":
            classifieds();
            resetAllMenuStyle(pageName);
            $('ClassifiedsLink').style.color = "purple";
            $('ClassifiedsLink').style.fontWeight = "bold";
            hideDiv($('contentPanel'));
            break;
        default:
            home();
            resetAllMenuStyle(pageName);
            $('HomeLink').style.color = "purple";
            $('HomeLink').style.fontWeight = "bold";
            hideDiv($('contentPanel'));
            break;
    }
}

function resetAllMenuStyle(pageName)
{
    for(var i = 0; i < $('topMenuBar').childNodes.length; i++)
    {
        if($('topMenuBar').childNodes[i].nodeName == "A")
        {
            $('topMenuBar').childNodes[i].style.color = "black";
            $('topMenuBar').childNodes[i].style.fontWeight = "normal";
        }
    }  
}

function rideReports() {
    toggleRideMenu();
}

function home() {
    hideRideView()
    showMainPanel('HomePage.aspx');
}

function forum() {
    hideRideView()
    showMainPanel('Forum.aspx');
}

// Classified Advertisements, courtesy of Expo
function classifieds() {
    hideRideView()
    // Expo can be slow, so show an intermediate page
    showMainPanel('PleaseWait.htm');
    showMainPanel('Classifieds.aspx');
}

function events() {
    toggleEventsMenu();
}

// Updates the page with new ride information
function updatePage(title, feed, item, cid, album) {
    hideRideMenu();
    hideEventsMenu();
    hideDiv($('SlideShowPanel'));
    hideDiv($('DirectionsPanel'));
    hideDiv( $('MainPanel'));
    hideVideo();
    showDiv($('TextPanel'), 500);
    showDiv($('MenuPanel'), 500);
    showDiv($('MapPanel'), 99999);
    
    CID = cid;
    
    var titleDiv = $('Title');
    titleDiv.innerHTML = title;
            
    ALBUM = album;
    
    updateText(feed, item);
    
    // Load the collection
    loadCollection(cid);
    
    if(album == "")
        disableAnchor($('PhotosFromRideLink'), true); 
    else
    {
        if($('PhotosFromRideLink').disabled)
            disableAnchor($('PhotosFromRideLink'), false); 
    }
        
    if(cid == "")
    {
        disableAnchor($('MapLink'), true);   
        disableAnchor($('DirectionsLink'), true);   
        disableAnchor($('ViewRouteLink'), true);   
        disableAnchor($('BikeCamLink'), true);   
    }
    else
    {
        if($('MapLink').disabled)
            disableAnchor($('MapLink'), false);  
        if($('DirectionsLink').disabled) 
            disableAnchor($('DirectionsLink'), false); 
        if($('ViewRouteLink').disabled)              
            disableAnchor($('ViewRouteLink'), false);   
        if($('BikeCamLink').disabled)
            disableAnchor($('BikeCamLink'), false);  
    }
    
    // Hard coded link for London Bike Cam demo
    switch(title.toLowerCase())
    {
        case "london river thames":
            if($('BikeCamLink').disabled)
                disableAnchor($('BikeCamLink'), false); 
            break;
        default:
            disableAnchor($('BikeCamLink'), true);   
            break;
    }
}

// Workaround to support disabling hyperlinks in Firefox
function disableAnchor(obj, disable)
{
    if(disable)
    {
        obj.disabled = true;
        var href = obj.getAttribute("href");
        if(href && href != "" && href != null)
        {
            obj.setAttribute('href_bak', href);
        }
        obj.removeAttribute('href');
        obj.style.color="gray";
    }
    else
    {
        obj.disabled = false;
        obj.setAttribute('href', obj.attributes['href_bak'].nodeValue);
        obj.style.color="#4b2269";
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
    CYCLIST_SHAPE.SetCustomIcon(CYCLIST_ICON);        
    CYCLIST_SHAPE.SetTitle('Cyclist');          
    CYCLIST_SHAPE.SetDescription('Map position ' + formatLatitude(point.Latitude) + ' ' + formatLongitude(point.Longitude));          
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
    var R = 3959; // miles
    
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