/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com

var map = null;
var office = null;
var clientlocation = null;
var PanelMode = 1; //0: Closed, 1:Cust details, 2: Diary, 3: Driving Directions
var submitted = false;
var popup = null;
    
function Page_Load()
{
    $addHandler($get("NavOffice"), "mouseover", Rollover);
    $addHandler($get("NavOffice"), "mouseout", Rollover);
    $addHandler($get("NavOffice"), "click", locateOffice);
    $addHandler($get("NavDiary"), "mouseover", Rollover);
    $addHandler($get("NavDiary"), "mouseout", Rollover);
    $addHandler($get("NavDiary"), "click", toggleCalendar);
    $addHandler($get("NavDirection"), "mouseover", Rollover);
    $addHandler($get("NavDirection"), "mouseout", Rollover);
    $addHandler($get("NavDirection"), "click", RouteTrip);
    $addHandler($get("NavSend"), "mouseover", Rollover);
    $addHandler($get("NavSend"), "mouseout", Rollover);
    $addHandler($get("NavSend"), "click", SendAppointment);
    $addHandler($get("SearchGO"), "mouseover", Rollover);
    $addHandler($get("SearchGO"), "mouseout", Rollover);
    $addHandler($get("SearchGO"), "click", HeadSearch);
    $addHandler($get("SearchText"), "keypress", HeadSearchKey);
    $addHandler($get("MainForm"), 'submit', VerifySubmit);
    $addHandler($get("CustomerSearchGO"), "mouseover", Rollover);
    $addHandler($get("CustomerSearchGO"), "mouseout", Rollover);
    $addHandler($get("CustomerSearchGO"), "click", CustSearch);
    $addHandler($get("ClientAddress"), "keypress", CustSearchKey);
    $addHandler($get("CalendarPanelClose"), "click", ClosePanel);
    $addHandler($get("CustomerDetailsPanelClose"), "click", ClosePanel);
    $addHandler($get("DrivingDirectionsPanelClose"), "click", ClosePanel);
    
    
    map = new VEMap('myMap');
    map.SetDashboardSize(VEDashboardSize.Tiny);
    map.onLoadMap = AddOffice;
    //Localize defaultLatLong in resource file, defined in Default.aspx
    map.LoadMap(new VELatLong(defaultLatLong[0],defaultLatLong[1]), 3);
    map.AttachEvent("onclick", AddClient);

    popup = new Contoso.ModalPopup();
    
    RefreshUI();
    myChannel = window.external.Channel;
} 

function AddOffice() {
    //Localize officeLatLong in resource file, defined in Default.aspx
    office = new VEShape(VEShapeType.Pushpin, new VELatLong(officeLatLong[0],officeLatLong[1]));
    office.SetCustomIcon("<div class=\"pin\"></div>");
    office.SetTitle("Office");
    map.AddShape(office);
    map.ShowMiniMap(340, -10);
}

function AddClient(e) {
    if (e.rightMouseButton) {
        var loc = map.PixelToLatLong(new VEPixel(e.mapX, e.mapY));
        if (!clientlocation) {
            clientlocation = new VEShape(VEShapeType.Pushpin, loc);
            clientlocation.SetCustomIcon("<div class=\"pinClient\"></div>");
            clientlocation.SetTitle(clientTitleText);
            map.AddShape(clientlocation);
        }else {
            clientlocation.SetPoints([loc]);
        }
    }
}

function locateOffice() {
    //centre and zoom on office
    map.SetCenterAndZoom(office.GetPoints()[0], 15);
    PanelMode=0;
    RefreshUI();    
}

function RouteTrip() {
    //remove route
    map.DeleteRoute();
    if (PanelMode!=3) {
        //Route from office to client
        if (clientlocation) {
            map.GetRoute(office.GetPoints()[0], clientlocation.GetPoints()[0], VEDistanceUnit.Kilometers, VERouteType.Quickest, onLoadRoute);
        } else {
        //Localize routeError in resource file, defined in Default.aspx
            popup.show(routeError);
        }
    }
    PanelMode=0;
    RefreshUI(); 
}

function onLoadRoute(route) {
    if (route) {
        //Localize distanceText in resource file, defined in Default.aspx    
        var routeinfo="<strong>" + distanceText;            
        routeinfo+=   route.Itinerary.Distance+" ";            
        routeinfo+=   route.Itinerary.DistanceUnit+"</strong><br />";                        
        var steps="<ol>";            
        var len = route.Itinerary.Segments.length;               
        for(var i = 0; i < len ;i++)               
        {                  
            steps+="<li>" + route.Itinerary.Segments[i].Instruction+" - <strong>(";                  
            steps+=route.Itinerary.Segments[i].Distance+" ";                  
            steps+=route.Itinerary.DistanceUnit+")</strong></li>";               
        }            
        routeinfo+=steps+"</ol>";            
        $get("DrivingDirectionsPanelContent").innerHTML = routeinfo;
        PanelMode=3;
        RefreshUI();
    }
}

function SendAppointment() {
    //send appointment
    var appointment = new Contoso.Common.Entity.Appointment();
    appointment.AptDate = $get("CalendarSelectedDate").value;
    appointment.AptTime = "9:00am";
    appointment.ClientAddress = $get("ClientAddress").value;
    appointment.ClientCompany = $get("ClientCompany").value;
    appointment.ClientName = $get("ClientName").value;
    appointment.ClientPhone = $get("ClientPhone").value;
    Contoso.Sales.services.AlertService.SendGroupRequest(appointment, currentCulture, onSendRequest, onFailed);
}

function onSendRequest(result)
{
    //Localize appointmentSentText in resource file, defined in Default.aspx
    popup.show(appointmentSentText);
}

function onFailed(error)
{      
    alert(error.get_message());
}

function HeadSearchKey(e) {
    if (e.charCode==13)
    {
        submitted = true;
        HeadSearch();
    }
}


function CustSearchKey(e) {
    if (e.charCode==13)
    {
        submitted = true;
        CustSearch();
    }
}

function VerifySubmit(e)
{
    if (submitted)
    {
        submitted = false;
        if (e && e.preventDefault) //FF
        {
            e.preventDefault();
        }
        return false; //IE
    }
}

function CustSearch() {
    var searchtext = $get("ClientAddress").value;
    Search(searchtext);
}

function HeadSearch() {
    var searchtext = $get("SearchText").value;
    Search(searchtext);
}

function Search(searchtext) {
    //search map for text location
    if (searchtext && searchtext.length > 0) {
        map.Find(null,searchtext);
    }
}

function Rollover(e) {
    Sys.UI.DomElement.toggleCssClass(e.target, "mouseover");
}

function toggleCalendar() {
    if (PanelMode==2) PanelMode=1;
    else PanelMode=2;
    RefreshUI();
}

function ClosePanel() {
    PanelMode=0;
    RefreshUI();
}

function RefreshUI() {
    $get("CalendarPanel").style.display = "none";
    $get("CustomerDetailsPanel").style.display = "none";
    $get("DrivingDirectionsPanel").style.display = "none";

    switch (PanelMode) {
        case 1: 
            $get("CustomerDetailsPanel").style.display = "block";
            break;
        case 2: 
            $get("CalendarPanel").style.display = "block";
            break;
        case 3: 
            $get("DrivingDirectionsPanel").style.display = "block";
            break;
    }    
}

function Page_Unload() { 

    $removeHandler($get("NavOffice"), "mouseover", Rollover);
    $removeHandler($get("NavOffice"), "mouseout", Rollover);
    $removeHandler($get("NavOffice"), "click", locateOffice);
    $removeHandler($get("NavDiary"), "mouseover", Rollover);
    $removeHandler($get("NavDiary"), "mouseout", Rollover);
    $removeHandler($get("NavDiary"), "click", toggleCalendar);
    $removeHandler($get("NavDirection"), "mouseover", Rollover);
    $removeHandler($get("NavDirection"), "mouseout", Rollover);
    $removeHandler($get("NavDirection"), "click", RouteTrip);
    $removeHandler($get("NavSend"), "mouseover", Rollover);
    $removeHandler($get("NavSend"), "mouseout", Rollover);
    $removeHandler($get("NavSend"), "click", SendAppointment);
    $removeHandler($get("SearchGO"), "mouseover", Rollover);
    $removeHandler($get("SearchGO"), "mouseout", Rollover);
    $removeHandler($get("SearchGO"), "click", HeadSearch);
    $removeHandler($get("SearchText"), "keypress", HeadSearchKey);
    $removeHandler($get("MainForm"), 'submit', VerifySubmit);
    $removeHandler($get("CustomerSearchGO"), "mouseover", Rollover);
    $removeHandler($get("CustomerSearchGO"), "mouseout", Rollover);
    $removeHandler($get("CustomerSearchGO"), "click", CustSearch);    
    $removeHandler($get("ClientAddress"), "keypress", CustSearchKey);
    $removeHandler($get("CalendarPanelClose"), "click", ClosePanel);
    $removeHandler($get("CustomerDetailsPanelClose"), "click", ClosePanel);
    $removeHandler($get("DrivingDirectionsPanelClose"), "click", ClosePanel);    
    map.Dispose();
}
    
//set page event handlers
if (window.attachEvent) {
	window.attachEvent("onload", Page_Load);
	window.attachEvent("onunload", Page_Unload);	
} else {
	window.addEventListener("DOMContentLoaded", Page_Load, false);
	window.addEventListener("unload", Page_Unload, false);
}

if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();
