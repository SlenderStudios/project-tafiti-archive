// Map.js 


// Microsoft is hosting the Mapcrunch but you could change this to put it anywhere.
var MAPCRUNCH = "http://contosouniversitymapcrunch.mslivelabs-int.com/Layer_Campus/";
var map = null; 
var myLocationPin = null;
var contactsLayer;               
var tileSourceSpec;

// Add an event handler for the page load
Sys.Application.add_load(ApplicationLoadHandler);

// Page load event handler. Page load handler gets fired on async panel updates 
function ApplicationLoadHandler(sender, args)
{
    // Add an event handler for the panel updates
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(PanelUpdated);
}

// Page load event handler
function OnPageLoad()      
{         
    map = new VEMap('myMap');         
    map.LoadMap(new VELatLong(47.6409, -122.1323), 15, "r", false, VEMapMode.Mode2D, false);    
    map.AttachEvent("onclick", MouseClickHandler); 
    contactsLayer = new VEShapeLayer();
    map.AddShapeLayer(contactsLayer);
    AddCampusOverlay();
    PanelUpdated(null, null);
}         

// Scans the DOM for table cells and displays the show all link if any locations exist
function PanelUpdated(sender, args) 
{
    var td = document.getElementsByTagName("td");
    var noLocations = true;
    
    for (var i=0; i<td.length; i++)
    {
        var t = td[i].innerHTML;
        if (t.match("location_icon.gif")) 
        {
            noLocations = false;
            break;
        }
    }        
    if (noLocations)
        document.getElementById("ContactsControl1_SeeAllLink").style.display = "none";
    else
        document.getElementById("ContactsControl1_SeeAllLink").style.display = "inline";
}

// Add the campus map crunch layer 
function AddCampusOverlay()
{
    tileSourceSpec = new VETileSourceSpecification("contoso_campus", MAPCRUNCH + "/%4.png");
    tileSourceSpec.NumServers = 1;
    tileSourceSpec.MinZoomLevel = 10;
    tileSourceSpec.MaxZoomLevel = 19;
    tileSourceSpec.Opacity = 1.0;
    tileSourceSpec.ZIndex = 100;
    map.AddTileLayer(tileSourceSpec, true);
}

// Show/hides the map overlay
function ShowOverlay(checked)
{
    if (!checked) 
        map.DeleteTileLayer("contoso_campus");
    else 
        map.AddTileLayer(tileSourceSpec, true);
}

// Adds a pushpin on right click
function AddPushpin(latLong)      
{   
    if (myLocationPin != null) map.DeleteShape(myLocationPin);       
    myLocationPin = new VEShape(VEShapeType.Pushpin, latLong);          
    myLocationPin.SetTitle('Set My Location');         
    var html = "Add a comment and click save.<br /><br />";
    html += "<b>Comment:</b><br /><input id='WhereAmI' type='text' />";
    html += "<input type=button onclick='javascript:SaveLocation(" + latLong.Latitude + "," + latLong.Longitude + ");' value='Save' />";
    html += "<br /><br />" + FormatLatLong(latLong);
    myLocationPin.SetDescription(html);          
    map.AddShape(myLocationPin);      
}

// Adds a pushpin at the map center
function AddCenterPushpin()
{
    AddPushpin(map.GetCenter());
}

// Converts a decimal lat long to canonical format
function FormatLatLong(latLong)
{
    var latdir = "N";
	var latdeg = Math.floor(parseFloat(latLong.Latitude));
	var latmin = (parseFloat(latLong.Latitude) - Math.floor(parseFloat(latLong.Latitude))) * 60;
	if (latdeg <= 0.0) latdir = "S";
	latdeg = Math.abs(latdeg);
	var lats = latdeg + "°" + Math.floor(latmin) + "\"" + RoundDown((latmin - Math.floor(latmin)) * 60, 2) + "\'" + latdir;

    var londir = "E";	    
	var londeg = Math.floor(parseFloat(latLong.Longitude));
	var lonmin = (parseFloat(latLong.Longitude) - Math.floor(parseFloat(latLong.Longitude))) * 60;
	if (londeg <= 0.0) londir = "W";
	londeg = Math.abs(londeg);
	var lons = londeg + "°" + Math.floor(lonmin) + "\"" + RoundDown((lonmin - Math.floor(lonmin)) * 60, 2) + "\'" + londir;
	    
    return lats + " " + lons;
}

function RoundDown(number, factor)
{
    number = parseFloat(number);
    factor = parseFloat(factor);
    number = Math.round(number * Math.pow(10, factor)) / Math.pow(10, factor);
    return number;
}

// Mouse button event handler that adds a users location
function MouseClickHandler(e)
{
    if (e.rightMouseButton)
    {   
        var pixel = new VEPixel(e.mapX, e.mapY);
        var latLong = map.PixelToLatLong(pixel);
        AddPushpin(latLong);
    }
}

// Scans the DOM for table cells and extracts and eval's the plot functions
function PlotAllLocations()
{
    var td = document.getElementsByTagName("td");
    
    for (var i=0; i<td.length; i++)
    {
        var t = td[i].innerHTML;
        if (t.match("location_icon.gif"))
        {
            var p1 = t.indexOf("javascript:") + 11;
            var p2 = t.indexOf("><")-2;
            eval(t.substring(p1, p2));
        }
    }
    map.SetMapView(contactsLayer.GetBoundingRectangle());
}

// Centers the map on a given lat long from the drop down
function LocatePoi(location)
{
    var lat = location.split(",")[0];
    var lng = location.split(",")[1];
    map.SetCenterAndZoom(new VELatLong(lat, lng),17);
}

// Calls the live contacts web service to save a user's location info
function SaveLocation(lat, lng)
{
    var comment = document.getElementById('WhereAmI').value + "<br /><br />" + Date();
    LiveContactsService.SaveMyLocation(lat, lng, escape(comment), OnSaveCompleted, OnFailed);    
}

function OnSaveCompleted(x)
{
    alert(x);
}

function OnFailed(e)
{
    alert(e.get_message());
}

// Plots a contact on the map   
function PlotContactOnMap(contactName, latitude, longitude, text)
{
    var latLong = new VELatLong(latitude, longitude);
    var pin = new VEShape(VEShapeType.Pushpin, latLong);
    pin.SetTitle(contactName);
    text = unescape(text);
    text = text.replace(/'/g, "\'");
    pin.SetDescription(text);
    pin.SetCustomIcon("<table cellpadding='0' cellspacing='0' id='pushpin'><tr><td><img src='App_Themes/Default/images/location_map_icon.gif' /></td><td>" + contactName + "&nbsp;&nbsp;</td></tr></table>");
    contactsLayer.AddShape(pin);
    map.SetCenterAndZoom(latLong, 16);
}

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();