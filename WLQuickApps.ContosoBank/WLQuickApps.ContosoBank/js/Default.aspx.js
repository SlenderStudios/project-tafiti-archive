/// <reference path="VEJS/VeJavaScriptIntellisenseHelper.js" />
/// <reference path="WLQuickApps.ContosoBank.Map.js" />
/// <reference path="WLQuickApps.ContosoBank.DashBoard.js" />

//the map object
var map = null;

//the dashboard
var dash = null;

//setup filter, here it is just a simple boolean for users / branches, you could extend this into an object including date ranges, categories etc
var filter = true;

//As we will modify the DOM we must wait until the page is fully loaded before we create our map
Sys.Application.add_init(function() {
    //create map   
    map = $create(WLQuickApps.ContosoBank.Map, {
        "Service": WLQuickApps.ContosoBank.Services.ContosoBankService,
        "GetClusteredData": true, 
        "Center": new VELatLong(cUserLatitude, cUserLongitude), 
        "MapStyle" : VEMapStyle.Hybrid, 
        "Dashboard" : false,
        "MiniMapXoffset" : 528,
        "MiniMapYoffset" : 0,         
        "MouseWheelZoomToCenter" : false, 
        "ScaleBarDistanceUnit" : VEDistanceUnit.Kilometers, 
        "ZoomLevel" : cUserZoomLevel
        }, null, null, $get("mymap"));
    
    //create custom dashboard
    dash = $create(WLQuickApps.ContosoBank.DashBoard, {
        "Map": map,
        "SelectedCSSClass" : "MapActionEnabled",
        "DisabledCSSClass" : "MapActionDisbaled",
        "Shaded" : $get("MapActionShaded"),
        "Hybrid" : $get("MapActionHybrid"),
        "BirdseyeHybrid" : $get("MapActionBirdseyeHybrid"),
        "Mode2D" : $get("MapActionMode2D"),
        "Mode3D" : $get("MapActionMode3D"),
        "ZoomIn" : $get("MapActionZoomIn"),
        "ZoomOut" : $get("MapActionZoomOut"),
        "Zoombar" : "MapZoomSlider"
        }, null, null, $get("mymapdashboard"));  
});

//Members
function MapActionFilterOn_onclick() {
    filter = true;
    if(map) map.onFilterChange();
    //set this on
    Sys.UI.DomElement.addCssClass($get("MapActionFilterOn"),"MapActionEnabled");
    //turn others off
    Sys.UI.DomElement.removeCssClass($get("MapActionFilterOff"),"MapActionEnabled");     
}

//Branches
function MapActionFilterOff_onclick() {
    filter = false;
    if(map) map.onFilterChange();
    //set this on
    Sys.UI.DomElement.addCssClass($get("MapActionFilterOff"),"MapActionEnabled");
    //turn others off
    Sys.UI.DomElement.removeCssClass($get("MapActionFilterOn"),"MapActionEnabled");        
}


//Toggle MiniMap
function MapActionToggleMiniMap_onclick() {
    if(map) {
        if (!map.get_MiniMap()) {
            map.set_MiniMap(true);
            Sys.UI.DomElement.addCssClass($get("mymapminimaptoggle"),"mymapminimaptoggleEnabled");
        }else {
            map.set_MiniMap(false);
            Sys.UI.DomElement.removeCssClass($get("mymapminimaptoggle"),"mymapminimaptoggleEnabled");
        }
    }
}

//ToggleDashboardSize
function MapActionToggleDashboard_onclick() {
    Sys.UI.DomElement.toggleCssClass($get("mymapdashboard"),"mymapdashboardCollapse");
    if(map) map.RefreshShims();
}

if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
