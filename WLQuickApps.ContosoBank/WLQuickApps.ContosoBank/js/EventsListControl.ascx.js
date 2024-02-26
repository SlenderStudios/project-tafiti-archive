/// <reference path="VEJS/VeJavaScriptIntellisenseHelper.js" />
/// <reference path="WLQuickApps.ContosoBank.Map.js" />
/// <reference path="WLQuickApps.ContosoBank.DashBoard.js" />

//the map object
var map = null;

//Toggle MiniMap
function MapActionToggleMiniMap_onclick() {
    if (map) {
        if (!map.get_MiniMap()) {
            map.set_MiniMap(true);
            Sys.UI.DomElement.addCssClass($get("mymapminimaptoggle"),"mymapminimaptoggleEnabled");
        }else {
            map.set_MiniMap(false);
            Sys.UI.DomElement.removeCssClass($get("mymapminimaptoggle"),"mymapminimaptoggleEnabled");
        }
    }
}

//launch map
function MapActionLaunchMap_onclick(id, e) {
    //clean up an old map
    if (map) {
        map.dispose();
        map = null;    
    }
    
    var mapFrame = $get("EventMapFrame");
    var scrollTop = window.pageYOffset || document.documentElement.scrollTop || 0; 
    var scrollLeft = window.pageXOffset || document.documentElement.scrollLeft || 0;
    mapFrame.style.left = e.clientX + scrollLeft - 640 + "px";
    mapFrame.style.top = e.clientY + scrollTop - 10  + "px";
    mapFrame.style.display = "block";
    map = $create(WLQuickApps.ContosoBank.Map, {
        "Center": new VELatLong(-25, 130),
        "MapStyle" : VEMapStyle.Hybrid, 
        "Dashboard" : false,
        "MiniMapXoffset" : 441,
        "MiniMapYoffset" : 0,         
        "MouseWheelZoomToCenter" : false, 
        "ScaleBarDistanceUnit" : VEDistanceUnit.Kilometers, 
        "MiniMap" : true,
        "ClearInfoBoxStyles" : true
        }, null, null, $get("mymap"));
        
    //get the data for this id
    WLQuickApps.ContosoBank.Services.ContosoBankService.GetEventByID(id,onReceivePin,Utility.OnFailed);   
}

function onReceivePin(localEvent) {
    map.AddPinOffCentreAndPopup(
        new VELatLong(localEvent.Latitude, localEvent.Longitude), 
        localEvent.EventName, 
        "Location: " + localEvent.Location + "<br /><br />" +
        "Date: " + localEvent.EventDate.toLocaleDateString() + "<br /><br />" +
        "Time: " + localEvent.EventDuration + "<br /><br />" +
        "<div class='CommandButton'><a href='#'>Add to Outlook Calendar</a></div>", 
        "EventPin");
}

//close map
function MapActionCloseMap_onclick() {
    $get("EventMapFrame").style.display = "none";
    map.dispose();
    map = null;
}

if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
