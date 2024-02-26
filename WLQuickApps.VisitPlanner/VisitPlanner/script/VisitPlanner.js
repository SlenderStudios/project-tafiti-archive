/////////////////////////////////////////////////////////////////////////////////
//
//
//
//
// 
// Filename: VisitPlanner.js
//
// @authors Infusion Development
// @version 1.0
//
/////////////////////////////////////////////////////////////////////////////////

/////////////////////////////////////////////////////////////////////////////////
//Constants
/////////////////////////////////////////////////////////////////////////////////
var VP_HOME_PUSHPIN = "images/homeLocation.png";
var VP_SAVED_PUSHPIN = "images/Btn_PushPin_saved.png";
var VP_DEFAULT_PUSHPIN = "images/Btn_PushPin.png";

/////////////////////////////////////////////////////////////////////////////////
//Initializing various Silverlight controls and objects
/////////////////////////////////////////////////////////////////////////////////
/**
* Main Visit Planner control
*/
var MainControl = null; 

/**
* Tour Silverlight object
*/
var TourControl = null;

/**
* Tour pop-up Silverlight object
*/
var TourPopupControl = null;

/**
* Info pop-up Silverlight object
*/
var InfoPopupControl = null;

/**
* Floating pin Silverlight object
*/
var FloatingPinControl = null;

/**
* List hover Silverlight object
*/
var PlaceListHoverControl = null;

/**
* Drop-box Silverlight object
*/
var AttractionDropBoxControl = null;

/**
* Directions Button Silverlight object
*/
var DirectionsButtonControl = null;

/**
* Expanded directions Silverlight object
*/
var DirectionsDialogControl = null;

/**
* String containing serialized text for indivudual POIs
*/
var tmpAttractionSerial = null;

/////////////////////////////////////////////////////////////////////////////////
//User functions
/////////////////////////////////////////////////////////////////////////////////
/*
* User object representing a visit planner user
*/
function User(type,id,name,firstName,lastName)
{
    /**
    * User Type
    */
    this.userType = (type == null || type == "undefined") ? "user" : type;

    /**
    * User ID
    */
    this.userId = (id == null || id == "undefined") ? "" : id;

    /**
    * User Name
    */
    this.userName = (name == null || name == "undefined") ? "" : name;

    /**
    * User First Name
    */
    this.userFirstName = (firstName == null || firstName == "undefined") ? "" : firstName;

    /**
    * User Last Name
    */
    this.userLastName = (lastName == null || lastName == "undefined") ? "" : lastName;

}
/**
* Current user object
*/
var currentUser = new User("user");

/**
* When user logs out of Windows Live
*/
function Logout(){
    //Hiding all Silverlight containers because they cause rendering bug in Firefox when leaving the page
    document.getElementById("DirectionsButtonHost").style.top = "-500px";
    document.getElementById("DirectionsDialogHost").style.top = "-500px";
    document.getElementById("PlaceListHoverHost").style.top = "-500px";
    document.getElementById("TourPopupHost").style.top = "-500px";
    document.getElementById("TourControlHost").style.top = "-500px";
    document.getElementById("InfoPopup").style.top = "-500px";
    document.getElementById("floatingPin").style.top = "-500px";
    
    if (MainControl!=null)
        MainControl.Content.Controller.Logout();
}

/**
* Displays popup for a new user to register their personal info (first name, last name)
*/
function registerNewUser()
{
    var RegisterPopup = document.getElementById("Register");
    RegisterPopup.style.top = "160px";
    RegisterPopup.style.left = "300px"; 
    RegisterPopup.style.display = "block";
    DetachMouseOverEvents();  
}

/**
* When user clicks "OK" to register.  Stores personal info into database
*/
function registerClick()
{
    currentUser.userFirstName = document.getElementById("EnterFirstName").value;
    currentUser.userLastName = document.getElementById("EnterLastName").value;
    if (currentUser.userFirstName == "" || currentUser.userLastName == "")
        document.getElementById("registerError").innerHTML = "Please fill out your first and last name.";
    else {
        document.getElementById("Register").style.display = "none";
        //Call handler to store first and last name into database  
        MainControl.Content.Controller.SavePersonal(currentUser.userFirstName, currentUser.userLastName);
        var userNameArea = document.getElementById("UserName");
        var userNameIntroArea = document.getElementById("welcomeName");
        userNameArea.innerHTML = XssEncode(currentUser.userFirstName)+" "+XssEncode(currentUser.userLastName);
        userNameIntroArea.display = "block";
        AttachMouseOverEvents();   
    }
}

/**
* If the user chooses to cancel registration
*/
function registerCancel()
{
    document.getElementById("Register").style.display = "none";
    AttachMouseOverEvents();
}

/////////////////////////////////////////////////////////////////////////////////
//Initialization functions
/////////////////////////////////////////////////////////////////////////////////
/**
* Loads Page.xaml
*/
function createSilverlight()
{
    if(!Silverlight.isInstalled("2.0"))
    {
        // hide the logo
        document.getElementById("vplogo").style.display = "none";
    }
    
    Silverlight.createObjectEx({
		source: "ClientBin/VESilverlight.xap",
		parentElement: document.getElementById("SilverlightControlHost"),
		id: "SilverlightControl",
		properties: {
			width: "100%",  
			height: "100%",
			version: "2.0",
			enableHtmlAccess: "true",
			isWindowless: "true",
			background:"#00dcded0"
		},
		events: {
		    onError: OnErrorHandler,
		    onLoad: OnPageLoaded
		},
		initParams: "mode=main"
	}); 
}

/**
* Called when Silverlight Page control is loaded
*/
function OnPageLoaded(sender, args)
{   
    MainControl = sender;
   
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    
    //Partial page rendering for concierge presence icon
    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(updateConciergeStatus);
    updateConciergeStatus();
    
    window.onresize = function(){
	    var controlHost = document.getElementById("SilverlightControlHost");
	    if (parseInt(document.body.clientHeight) < 770){
	        MainControl.height = "770";
	    }
	    else {
	        MainControl.height = "100%";
	    }
	    
	    if (parseInt(document.body.clientWidth) < 850){
	        MainControl.width = "850";
	    }
	    else {
	        MainControl.width = "100%";
	    }
	}
    
    window.onresize(); 
    GetMap();
    
    //set callbacks
    MainControl.Content.MapPanel.SetMapSize = setMapSize;
    MainControl.Content.MapPanel.AddPushpin = addPushpin;
    MainControl.Content.MapPanel.RemovePushpin = removePushpin;
    MainControl.Content.MapPanel.CenterMap = setMapCenter;
    MainControl.Content.MapPanel.ChangeMapFilter = ChangeMapFilter;
    MainControl.Content.TourShareControl.ShowTourControl = showTourControl;
    MainControl.Content.Controller.DoSearch = DoSearch;
    MainControl.Content.SideMenu.CloseCustomPopup = cancelCustomClick;
    MainControl.Content.SideMenu.CloseAddConciergePopup = cancelAddConcierge;
    MainControl.Content.TourShareControl.ShowShareDialog = ShowShareDialog;
    
    //create tour control
    Silverlight.createObjectEx({
		source: "ClientBin/VESilverlight.xap",
		parentElement: document.getElementById("TourControlHost"),
		id: "TourControl",
		properties: {
			width: "100%",  
			height: "100%",
			version: "2.0",
			enableHtmlAccess: "true",
			isWindowless: "true",
			background:"#00000000"
		},
		events: {
		    onError: OnErrorHandler,
		    onLoad: OnTourControlLoaded
		},
		initParams: "mode=tour"
	}); 
}

/**
* Called when Silverlight Tour Control is loaded
*/
function OnTourControlLoaded(sender, args)
{
    TourControl = sender;

    //Attach event to disable/enable mouseover events when guided tour is playing/stopped
    TourControl.Content.TourControl.DisableMouseOvers = DetachMouseOverEvents;
    TourControl.Content.TourControl.EnableMouseOvers = AttachMouseOverEvents;

    TourControl.Content.TourControl.ShowTour = function (sender, args) { 
        MainControl.Content.Controller.ShowTourPopup(args.Index);
    }
    
    TourControl.Content.TourControl.PauseTour = function (sender, args) { 
        MainControl.Content.Controller.PauseTour();
    }
    
    TourControl.Content.TourControl.UnpauseTour = function (sender, args) { 
        MainControl.Content.Controller.ResumeTour();
    }
    
     TourControl.Content.TourControl.StopTour = function (sender, args) { 
        MainControl.Content.Controller.StopTour();
    }
    
    MainControl.Content.Controller.StopTourEvent = function (sender, args){
        hideTourControl();
        TourControl.Content.TourControl.ExitTourLogic();
    }
   
    TourControl.Content.TourControl.InitializeWithoutCount();
        
    MainControl.Content.Controller.TourInitializer = function (sender,args){
        TourControl.Content.TourControl.Initialize( args.Count );
    }

        //create tour item    
        Silverlight.createObjectEx({
	        source: "ClientBin/VESilverlight.xap",
	        parentElement: document.getElementById("TourPopupHost"),
	        id: "tourPopupControl",
	        properties: {
	            width: "568px",
	            height: "281px",
		        version: "2.0",
		        enableHtmlAccess: "true",
		        isWindowless: "true",
		        background:"#00000000"
	        },
	        events: {
	            onError: OnErrorHandler,
	            onLoad: OnTourPopupControlLoaded
	        },
	        initParams: "mode=touritem"
        });
    	
        
        //create pop-up item
        Silverlight.createObjectEx({
	        source: "ClientBin/VESilverlight.xap",
	        parentElement: document.getElementById("InfoPopup"),
	        id: "InfoPopupControl",
	        properties: {
		            width: "224px",
		            height: "136px",
		        version: "2.0",
		        enableHtmlAccess: "true",
		        isWindowless: "true",
		        background:"#00000000"
	        },
	        events: {
	            onError: OnErrorHandler,
	            onLoad: OnPopupLoaded
	        },
	        initParams: "mode=popup"
        });
    	
    	
        //create floating pin control    
        Silverlight.createObjectEx({
	        source: "ClientBin/VESilverlight.xap",
	        parentElement: document.getElementById("floatingPin"),
	        id: "floatingPinControl",
	        properties: {
	            width: "62px",
	            height: "62px",
		        version: "2.0",
		        enableHtmlAccess: "true",
		        isWindowless: "true",
		        background:"#00000000"
	        },
	        events: {
	            onError: OnErrorHandler,
	            onLoad: OnFloatPinLoaded
	        },
	        initParams: "mode=floatingpin"
        });

        //create list hover control
        Silverlight.createObjectEx({
	        source: "ClientBin/VESilverlight.xap",
	        parentElement: document.getElementById("PlaceListHoverHost"),
	        id: "PlaceListHoverControl",
	        properties: {
	            width: "284",
		        height: "50",
		        version: "2.0",
		        enableHtmlAccess: "true",
		        isWindowless: "true",
		        background:"#00000000"
	        },
	        events: {
	            onError: OnErrorHandler,
	            onLoad: OnPlaceListHoverLoaded
	        },
	        initParams: "mode=placelist"
        });

       
        //create drop box
        Silverlight.createObjectEx({
	        source: "ClientBin/VESilverlight.xap",
	        parentElement: document.getElementById("AttractionDropBoxHost"),
	        id: "AttractionDropBoxControl",
	        properties: {
	            width: "87",
		        height: "75",
		        version: "2.0",
		        enableHtmlAccess: "true",
		        isWindowless: "true",
		        background:"#00000000"
	        },
	        events: {
	            onError: OnErrorHandler,
	            onLoad: OnAttractionDropBoxLoaded
	        },
	        initParams: "mode=dropbox"
        });

       
       //create directions button 
       Silverlight.createObjectEx({
	        source: "ClientBin/VESilverlight.xap",
	        parentElement: document.getElementById("DirectionsButtonHost"),
	        id: "DirectionsButtonControl",
	        properties: {
	            width: "190",
		        height: "36",
		        version: "2.0",
		        enableHtmlAccess: "true",
		        isWindowless: "true",
		        background:"#00FFFFFF"
	        },
	        events: {
	            onError: OnErrorHandler,
	            onLoad: OnDirectionsButtonLoaded
	        },
	        initParams: "mode=directionsbutton"
        });
}

/**
* Called when Sliverlight direction button is loaded
*/
function OnDirectionsButtonLoaded(sender, args)
{
    DirectionsButtonControl = sender;

    SetDirectionsButtonState(true);
   
   Silverlight.createObjectEx({
		source: "ClientBin/VESilverlight.xap",
		parentElement: document.getElementById("DirectionsDialogHost"),
		id: "DirectionsDialogControl",
		properties: {
		    width: "190",
			height: "330",
			version: "2.0",
			enableHtmlAccess: "true",
			isWindowless: "true",
			background:"#00FFFFFF"
		},
		events: {
		    onError: OnErrorHandler,
		    onLoad: OnDirectionsDialogLoaded
		},
		initParams: "mode=directionsdialog"
	});
	}

/**
* Called when Sliverlight direction dialog box is loaded
*/
function OnDirectionsDialogLoaded(sender, args)
{
    DirectionsDialogControl = sender;
    
    DirectionsButtonControl.Content.DirectionsButton.ShowDirectionsDialog = function(){
        SetDirectionsButtonState(false);
        SetDirectionsDialogState(true);
    }
    
    DirectionsDialogControl.Content.DirectionsDialog.Close = function (){
        SetDirectionsDialogState(false);
        SetDirectionsButtonState(true);
    }
    
    DirectionsDialogControl.Content.DirectionsDialog.RetrieveDirections = retrieveDirections; 
}

/**
* Called when Sliverlight Attraction drag and drop box is loaded
*/
function OnAttractionDropBoxLoaded(sender, args)
{
    AttractionDropBoxControl = sender;
    
    AttractionDropBoxControl.Content.AttractionDropBox.AddAttraction = function (sender, args) 
    {
        var text = args.get_SerialText();
        MainControl.Content.Controller.AddToItinerary(args.get_SerialText());
    };
    AttractionDropBoxControl.Content.AttractionDropBox.RemoveAttraction = function (sender, args) 
    {
        var text = args.get_SerialText();
        MainControl.Content.Controller.RemoveFromItinerary(args.get_SerialText());
    };
    MainControl.Content.Controller.AttractionDropBoxChanged = SetAttractionDropBoxState;
    
    //Hidden fields populated on the page from user credentials in the database
    var visitorIdField = document.getElementById("UserId");             
    var visitorTypeField = document.getElementById("UserType");        
    var visitorFirstName = document.getElementById("UserFirstName");   
    var visitorLastName = document.getElementById("UserLastName");      
    
    //Check user is logged into Windows Live
    if(visitorIdField != null)  
    {   
        currentUser.userId = visitorIdField.getAttribute("value"); 
        currentUser.userType = visitorTypeField.getAttribute("value");
        
        if (visitorFirstName.getAttribute("value") != "")
            currentUser.userFirstName = visitorFirstName.getAttribute("value");
        if (visitorLastName.getAttribute("value") != "")
            currentUser.userLastName = visitorLastName.getAttribute("value");
        
        //If they are a new user, prompt them to register their name    
        if (currentUser.userFirstName == "" && currentUser.userLastName == ""){
            registerNewUser();
        }
        if(currentUser.userId.length > 0){
            //If user is logged in, set various login properties in client side C# objects
            MainControl.Content.Controller.Login(currentUser.userId, currentUser.userType, currentUser.userFirstName, currentUser.userLastName);        
        }
    }
    
    //If someone visits the link that was shared to them
    var sharedIdField = document.getElementById("SharedUserId");
    if(sharedIdField != null){   
        var sharedId = null;
        sharedId = sharedIdField.getAttribute("value");
        
        if(sharedId != null){
            MainControl.Content.Controller.SetSharedUserId(sharedId);      
        }
    }
}


/**
* Called when Silverlight control for concierge mouseover hover box is loaded 
*/
function OnPlaceListHoverLoaded(sender, args)
{
    PlaceListHoverControl = sender;
    PlaceListHoverControl.Content.PlaceListHover.MovePlaceListHover = MovePlaceListHover;
    MainControl.Content.Controller.MovePlaceListHover = MovePlaceListHover;
    PlaceListHoverControl.Content.PlaceListHover.StopTour = function (sender, args) {
        MainControl.Content.Controller.StopTour();
    };
    PlaceListHoverControl.Content.PlaceListHover.ShowTourPopupBySerial = function (sender, args) {
        MainControl.Content.Controller.ShowTourPopupBySerial(args.get_SerialText());
    };
}

/**
* Called when Silverlight control for tour item popup is loaded 
*/ 
function OnTourPopupControlLoaded(sender, args){
    TourPopupControl = sender;
    
    TourPopupControl.Content.TourItem.RetrieveDirections = retrieveDirections;
    
    TourPopupControl.Content.TourItem.MoveTourItemControl = MoveTourItemControl;
    
    TourPopupControl.Content.TourItem.ThreadCompletionCallback = ThreadCompletionCallback;

    TourPopupControl.Content.TourItem.PauseTour = function (sender, args) {
        TourControl.Content.TourControl.PauseTourLogic();
    };
    
    TourPopupControl.Content.TourItem.ExitTour = function (sender, args) {
        MainControl.Content.Controller.StopTour();
    };
    
    TourPopupControl.Content.TourItem.DisableMouseOvers = DetachMouseOverEvents;
    
    TourPopupControl.Content.TourItem.ShowShareDialog = ShowShareDialog;
    
    TourPopupControl.Content.TourItem.AddToItinerary = function (sender, args) {
        MainControl.Content.Controller.AddToItinerary(args.get_SerialText());
    };

    MainControl.Content.Controller.ShowTourItem = function (sender, args) { 
        TourPopupControl.Content.TourItem.ShowItem(args.get_SerialText(), args.LoggedIn); 
    };
   

    MainControl.Content.Controller.HideTourItem = function () { 
        TourPopupControl.Content.TourItem.HideItem(); 
        AttachMouseOverEvents();
    }; 
}

/**
* Called when Silverlight control for mouseover popup is loaded
*/
function OnPopupLoaded(sender, args)
{
    InfoPopupControl = sender;
    InfoPopupControl.Content.PopupItem.HidePopupItem = HidePopup;
    
    //When user clicks on the pushpin popup"s maximize (more info) button
    InfoPopupControl.Content.PopupItem.ShowMoreInfo = function (sender, args) {
        TourPopupControl.Content.TourItem.ShowItem(args.get_SerialText(), currentUser.userId.length > 0); 
        }
}

/**
* Called when Silverlight control for the drag and drop floating pin is loaded
*/
function OnFloatPinLoaded(sender, args){
    FloatingPinControl = sender;
}

/**
* Sets size of map and position of Silverlight elements based on window size
*/
function setMapSize(sender, args){
    var myMap = document.getElementById("myMap");
    if (myMap != null && map!=null){
        myMap.style.height = args.Height;
        myMap.style.width = args.Width;
        map.Resize(args.Width, args.Height);
    }
    
    //set tour control position
    var tourControlHost = document.getElementById("TourControlHost");
    tourControlTop = args.Height - 2*parseInt(tourControlHost.style.height.split("px")[0]) + parseInt(myMap.style.top.split("px")[0]) + "px";
    tourControlHost.style.width = args.Width + "px";
        
    if (tourControlVisible)
    {
        tourControlHost.style.top = tourControlTop;
    }
    
    try
    {
        TourControl.Content.TourControl.Resize(args.Width);
    } catch (err) {
        
    }

    //set attraction drop box position
    if (AttractionDropBoxControl != null){
        var y = parseInt(myMap.style.top.split("px")[0]) + args.Height - AttractionDropBoxControl.height - 5;
        
        var x = parseInt(myMap.style.left.split("px")[0]) + args.Width - AttractionDropBoxControl.width - 5;
        
        AttractionDropBoxControl.Content.AttractionDropBox.SetPosition(x, y);
        
        var attractionDropHost = document.getElementById("AttractionDropBoxHost");
        attractionDropHost.style.left = x + "px";
        attractionDropHost.style.top = y + "px";
    }
    
    //set directions button position
    if (DirectionsButtonControl != null){
        var y2 = parseInt(myMap.style.top.split("px")[0]);
        var x2 = parseInt(myMap.style.left.split("px")[0]) + args.Width - parseInt(DirectionsButtonControl.width);
        
        var directionsButtonHost = document.getElementById("DirectionsButtonHost");
        directionsButtonHost.style.left = x2 + "px";
        directionsButtonHost.style.top = y2 + "px";
    } 
    
    //set direction dialog postition
    if (DirectionsDialogControl != null){
        var y3 = parseInt(myMap.style.top.split("px")[0]);
        var x3 = parseInt(myMap.style.left.split("px")[0]) + args.Width - parseInt(DirectionsDialogControl.width);
        
        var directionsDialogHost = document.getElementById("DirectionsDialogHost");    
        directionsDialogHost.style.left = x3 + "px";
        directionsDialogHost.style.top = y3 + "px";
    }
}

/////////////////////////////////////////////////////////////////////////////////
//Concierge functions
/////////////////////////////////////////////////////////////////////////////////
/**
* Updates the Concierge Windows Live presence icon
*/
function updateConciergeStatus()
{
    var statusImgSrc = document.getElementById("ConciergeStatusImageSrc");
    if (statusImgSrc != null){
        MainControl.Content.ConciergeToolBar.SetStatusImage(statusImgSrc.value);
    }
}

/**
* Called when destination city is changed in drop down menu
*/
function SelectDestination(destinationID, destinationName){
    if (MainControl){
        //HomeID = -1;
        MainControl.Content.Controller.SelectDestination(parseInt(destinationID),destinationName);
        SetBestMapView();
    }
}

/**
* Dialog box when admin chooses to add a new POI from ConciergeAddRemovePopup popup
*/
function ConciergeAddLinkClicked()
{
    document.getElementById("ConciergeAddRemove").style.display = "none";
    AddConciergePopup = document.getElementById("AddConciergeItem");          

    document.getElementById("AddConciergeTitle").value = "";
    document.getElementById("ConciergeShortDesc").value = "";
    document.getElementById("ConciergeLongDesc").value = "";
    document.getElementById("AddConciergeImageURL").value = "";
    document.getElementById("AddConciergeVideoURL").value = "";
    document.getElementById("ConciergeStreet").value = "";
    document.getElementById("ConciergeCityState").value = "";
    document.getElementById("AddConciergeCategory").selectedIndex=0;
    document.getElementById("AddConciergeKeywords").value = "";
    document.getElementById("AddConciergeRecurrence").selectedIndex=0;      
 
    //Adds temporary pin to show where the new icon will be placed
    customTempPin = new VEShape(VEShapeType.Pushpin, customLatLong);
       
    var Shape3d = new VECustomIconSpecification();
    Shape3d.Image = "images/Btn_PushPin_Faded.png";
    customTempPin.SetCustomIcon(Shape3d);
    map.AddShape(customTempPin);
 
    AddConciergePopup.style.display = "block";
    AddConciergePopup.style.top = "110px";
    
    var offset = map.GetLeft();  //The offset of the map due to centering and resizing
    
    if (xClicked > 380 + offset)  //greater than half of the map
        AddConciergePopup.style.left = xClicked - offset - parseInt(AddConciergePopup.style.width) + "px";
    else
        AddConciergePopup.style.left = xClicked - offset + 60 + "px";
        
    if(is3D)
        Show3DFrame(AddConciergePopup.style.left, AddConciergePopup.style.top, AddConciergePopup);
        
    document.getElementById("AddConciergeTitle").focus();
}

/**
* When admin chooses to remove an existing POI from ConciergeAddRemovePopup popup
*/
function ConciergeRemoveLinkClicked()
{        
    document.getElementById("ConciergeAddRemove").style.display = "none";
    try
    {
        MainControl.Content.Controller.GetSerializedAttraction(deleteItem.Attraction,
            function(sender, text)
            {
                MainControl.Content.ConciergeToolBar.RemoveConciergeItem(text);
            });
    }
    catch (e)
    {
    }
}

/**
* When admin clicks "OK" to adding a new custom POI to concierge list
* New concierge collection is saved into database
*/
function AddConciergeItemClick(){
    var AddConciergePopup = document.getElementById("AddConciergeItem");     
    var ItemTitle = document.getElementById("AddConciergeTitle").value;
    var ItemShortDescription = document.getElementById("ConciergeShortDesc").value;
    var ItemLongDescription = document.getElementById("ConciergeLongDesc").value;
    var ItemImageURL = document.getElementById("AddConciergeImageURL").value;
    var ItemVideoURL = document.getElementById("AddConciergeVideoURL").value;
    var ItemAddressLine1 = document.getElementById("ConciergeStreet").value;
    var ItemAddressLine2 = document.getElementById("ConciergeCityState").value;
    var ItemCategory = document.getElementById("AddConciergeCategory").options[document.getElementById("AddConciergeCategory").selectedIndex].text;
    var ItemKeywords = document.getElementById("AddConciergeKeywords").value;
    var ItemRecurrence = document.getElementById("AddConciergeRecurrence").options[document.getElementById("AddConciergeRecurrence").selectedIndex].text;
         
    if (ItemImageURL == "")
        ItemImageURL = "images/Btn_PushPin.png";
        
    var category = 7; //misc
    switch(ItemCategory)
    {
        case "Food":
            category = 0;
            break;
        case "Music":
            category = 1;
            break;
        case "Movie":
            category = 2;
            break;
        case "Art":
            category = 5;
        default:
            category = 7;
    }
    
    var recurrence = 0;
    switch(ItemRecurrence)
    {
        case "Daily":
            recurrence = 0;
            break;
        case "Weekly":
            recurrence = 1;
            break;
        case "Monthly":
            recurrence = 2;
            break;
        default:
            recurrence = 3;
    }

    var resultString = " {";
    
    resultString += "\"Title\" : \""+filterText(ItemTitle)+"\",";
    resultString += "\"Category\" : "+category+",";     
    resultString += "\"ImageURL\" : \""+filterText(ItemImageURL)+"\",";
    resultString += "\"VideoURL\" : \""+filterText(ItemVideoURL)+"\",";

    resultString += "\"ShortDescription\" : \""+filterText(ItemShortDescription)+"\",";
    resultString += "\"LongDescription\" : \""+filterText(ItemLongDescription)+"\",";        
    resultString += "\"AddressLine1\" : \""+filterText(ItemAddressLine1)+"\",";
    resultString += "\"AddressLine2\" : \""+filterText(ItemAddressLine2)+"\",";
    resultString += "\"Recurrence\" : "+recurrence+",";   
    
    resultString += "\"Latitude\" : \"" + customLatLong.Latitude + "\",";
    resultString += "\"Longitude\" : \"" + customLatLong.Longitude + "\",";
    resultString += "\"Keywords\" : \""+filterText(ItemKeywords)+"\" ";
   
    resultString += "}";
    
    MainControl.Content.ConciergeToolBar.AddConciergeItem(resultString);
    AddConciergePopup.style.display = "none";
    if(customTempPin != null)
        map.DeleteShape(customTempPin);

}

/**
* When admin cancels request to add new POI
*/
function cancelAddConcierge(){
    var AddConciergePopup = document.getElementById("AddConciergeItem");
    document.getElementById("ConciergeAddRemove").style.display = "none";
    AddConciergePopup.style.display = "none";
    if(customTempPin != null)
        map.DeleteShape(customTempPin);
}

/////////////////////////////////////////////////////////////////////////////////
//Directions functions
/////////////////////////////////////////////////////////////////////////////////
/**
* Displays or hides Driving directions button
*/
function SetDirectionsButtonState(state)
{
    if (DirectionsButtonControl == null) return;
    
    var directionsButtonHost = document.getElementById("DirectionsButtonHost");
    
    if (state){
        directionsButtonHost.style.zIndex = "34";
        if (is3D){
            Show3DFrame(parseInt (directionsButtonHost.style.top.split("px")[0]),parseInt (directionsButtonHost.style.left.split("px")[0]),DirectionsButtonControl);
        }
    } else {
        directionsButtonHost.style.zIndex = "-1";
        
        if (is3D){
            Hide3DFrame(DirectionsButtonControl);
        }
    }
    DirectionsButtonControl.Content.DirectionsButton.SetActive(state);
}

/**
* Displays or hides Driving direction dialog box
*/
function SetDirectionsDialogState(state)
{
    if (DirectionsDialogControl == null) return;
    
    var directionsDialogHost = document.getElementById("DirectionsDialogHost");
  
    if (state){
        directionsDialogHost.style.zIndex = "34";
        
        if (is3D){
            Show3DFrame(parseInt (directionsDialogHost.style.top.split("px")[0]),parseInt (directionsDialogHost.style.left.split("px")[0]),DirectionsDialogControl);
        }

        DirectionsDialogControl.Content.DirectionsDialog.SetActive(true);
    } else {
        directionsDialogHost.style.zIndex = "-1";
        
        if (is3D){
            Hide3DFrame(DirectionsDialogControl);
        }
        DirectionsDialogControl.Content.DirectionsDialog.SetActive(false,
            "",
            "" ); 
    }
}

/**
* Opens new window when user clicks to find driving directions
*/
function retrieveDirections(sender, args)
{
    var url = "DrivingDirections.aspx?";
    url += "st=" + args.StartTitle + "&"; 
    url += "sa=" + args.StartAddress + "&";
    url += "slat=" + args.StartLatitude + "&"; 
    url += "slon=" + args.StartLongitude + "&";
    
    url += "et=" + args.EndTitle + "&";
    url += "ea=" + args.EndAddress + "&";
    url += "elat=" + args.EndLatitude + "&";
    url += "elon=" + args.EndLongitude; 
    window.open(url,"mywin","toolbar=1,resizable=0,scrollbars=1,height=600, width=600");
}

/////////////////////////////////////////////////////////////////////////////////
//Drop Box functions
/////////////////////////////////////////////////////////////////////////////////
/**
* Display or Hide the attraction drag and drop box
*/
function SetAttractionDropBoxState(sender, args){
    var attractionDropHost = document.getElementById("AttractionDropBoxHost");
    
    if (hideBoxForTour && attractionDropHost.style.zIndex == "-1"){
        hideBoxForTour = false;
    }

    if (args.State){
        attractionDropHost.style.zIndex = "34";              
       
    } else {
        attractionDropHost.style.zIndex = "-1";
        if (is3D){
            Hide3DFrame(AttractionDropBoxControl);
        }
    }
    
    //Makes the drag n drop box active or inactive (true or false)
    AttractionDropBoxControl.Content.AttractionDropBox.SetActive(args.State,args.Insert);
}

/////////////////////////////////////////////////////////////////////////////////
//Item list functions
/////////////////////////////////////////////////////////////////////////////////
/**
* Displays or Hides image popup when hovering over concierge items
*/
function MovePlaceListHover(sender, args)
{
    var placeListHost = document.getElementById("PlaceListHoverHost");
    
    placeListHost.style.top = args.Y + "px";
    placeListHost.style.left = args.X + "px";
    
    if (args.X > 0){
       // args.SerialText is always null here, but we can get the correct value by
       // calling get_SerialText(). This is probably a SL2B1 bug.
       //PlaceListHoverControl.Content.PlaceListHover.Initialize( args.SerialText );
       PlaceListHoverControl.Content.PlaceListHover.Initialize(args.get_SerialText());
    
        if (is3D){
            Show3DFrame(args.X,args.Y,PlaceListHoverControl);
        }
    } else if (is3D){
        Hide3DFrame(PlaceListHoverControl);
    } 
}

/////////////////////////////////////////////////////////////////////////////////
// Mapping functions
/////////////////////////////////////////////////////////////////////////////////
/**
* Latlong coordinates of the user or admin click when they add a new item
*/
var customLatLong;

/**
* Temporary indicator pin of where the user or admin clicks to add a new item
*/
var customTempPin = null;

/**
* X pixel coordinates of the user or admin click
*/
var xClicked = null;

/**
* Y pixel coordinates of the user or admin click
*/
var yClicked = null;

/**
* The pin that is to be deleted if selected
*/
var deleteItem = null;

/**
* Div dialog box for user to add a custom POI
*/
var CustomPinPopup = null;

/**
* Div popup for admin to add or remove a POI
*/
var ConciergeAddRemovePopup = null;

/**
* Div dialog box for admin to add new POI
*/
var AddConciergePopup = null;

/**
* Boolean variable to indicate if VE map is in 3D mode
*/
var is3D = false;

/**
* Map initialized flag
*/
var initialized = false;

/**
* Disables map mouse hovering
*/
var disableHover = false;

/**
* Centering flag
*/
var centering = false;

/**
* Center lat/long
*/
var centerGoal = null;

/**
* Initializes map
*/
function InitializeMap(){
    if (!initialized){
        MainControl.Content.MapPanel.InitializeMapData();
        initialized = true;
        //Sets appopriate center position and zoom based on POIs
        SetBestMapView();
    }
}

/**
* Loads Virtual Earth map
*/
function GetMap(){
    map = new VEMap("myMap");
    map.LoadMap();
    map.AttachEvent("onmouseover", HoverHandler);
    map.AttachEvent("onstartpan", HidePopup);
    map.AttachEvent("onmouseout", FadePopup);
    map.AttachEvent("onstartzoom", HidePopup);
    map.AttachEvent("onmousedown", MouseDownHandler);
    map.AttachEvent("onmouseup", MouseUpHandler);
    map.AttachEvent("onchangeview", CenterComplete);
    map.AttachEvent("onresize",InitializeMap);
    map.AttachEvent("oninitmode", ModeChanged);
    map.AttachEvent("onclick", MapClickHandler); 
}

/**
* VE map onclick handler for adding new POIs
*/
function MapClickHandler(e){
    if (e.rightMouseButton){
        if(customTempPin != null)
            map.DeleteShape(customTempPin)    

        xClicked = e.clientX;
        yClicked = e.clientY;
        var offset = map.GetLeft();  //The offset of the map due to centering and resizing
           
        //e.mapX and e.mapY not supportted in 3D
        if (is3D){
            customLatLong = e.latLong;  //e.latLong only supported in 3D
            var lltmp = map.LatLongToPixel(e.latLong);
            xClicked = lltmp.x + offset;
            yClicked = lltmp.y + 100;
        }
        else
            customLatLong = map.PixelToLatLong(new VEPixel(e.mapX , e.mapY)); 
    
        var isOnMyPlaces = MainControl.Content.MyPlacesToolBar.IsOnMyPlaces();
        if (isOnMyPlaces == 1){       
            //Popup form to enter custom place details
            CustomPinPopup = document.getElementById("AddCustomPin"); 
            document.getElementById("CustomTitle").value = "Custom Place";
            document.getElementById("CustomDescription").value = "<Enter a description for your item here.>"; 
            document.getElementById("CustomNotes").value = "<Enter your notes here.>";
            document.getElementById("CustomImageURL").value = "";
            document.getElementById("CustomVideoURL").value = "";        
        
            //Adds temporary pin to show where the new icon will be placed
            customTempPin = new VEShape(VEShapeType.Pushpin, customLatLong);
            
            var Shape3d = new VECustomIconSpecification();
            Shape3d.Image = "images/Btn_PushPin_Faded.png";
            
            customTempPin.SetCustomIcon(Shape3d);
            map.AddShape(customTempPin);
                  
            CustomPinPopup.style.display = "block";
            CustomPinPopup.style.top = "160px";
            if (xClicked > 380 + offset)  //greater than half of the map
                CustomPinPopup.style.left = xClicked - offset - parseInt(CustomPinPopup.style.width) + "px";
            else
                CustomPinPopup.style.left = xClicked - offset + 60 + "px"; 
            
            if(is3D)
                Show3DFrame(CustomPinPopup.style.left, CustomPinPopup.style.top, CustomPinPopup);
                
            document.getElementById("CustomTitle").focus();                  
        } 
        
        //Add Concierge Item
        var isOnConcierge = MainControl.Content.ConciergeToolBar.IsOnConcierge();
            
        if (isOnConcierge == 1 && currentUser.userType == "admin"){
            cancelAddConcierge();
            ConciergeAddRemovePopup = document.getElementById("ConciergeAddRemove");
            ConciergeAddRemovePopup.style.left = xClicked + 30 - offset + "px";
            ConciergeAddRemovePopup.style.top = yClicked + "px";
            ConciergeAddRemovePopup.style.display = "block";
    
            if (e.elementID != null){
                deleteItem = map.GetShapeByID(e.elementID);
                document.getElementById("ConciergeRemoveLink").style.display = "block";
            } else {
                document.getElementById("ConciergeRemoveLink").style.display = "none";
            }
           
            if(is3D)
                Show3DFrame(ConciergeAddRemovePopup.style.left, ConciergeAddRemovePopup.style.top, ConciergeAddRemovePopup);                      
        }
    
    }
}

/**
* VE map onmouseover handler
*/
function HoverHandler(e){
    if (e.elementID != null && !disableHover){
        var shape = map.GetShapeByID(e.elementID);
        
        if (shape != customTempPin)
        {
            var point = map.LatLongToPixel(shape.GetPoints()[0]);        
            var myMap = document.getElementById("myMap");     
            //Show Silverlight popup    
            showPopup(parseFloat(point.x)+parseFloat(myMap.style.left.split("px")[0])+40,parseFloat(point.y)+parseFloat(myMap.style.top.split("px")[0])-120,shape.Attraction);
        }

    }
}

/**
* Detach mouse events during certain times (ie. guided tour in play)
*/
function DetachMouseOverEvents()
{
    map.DetachEvent("onmouseover", HoverHandler);
    map.DetachEvent("onmouseout", FadePopup);
    map.DetachEvent("onmousedown", MouseDownHandler);
    map.DetachEvent("onmouseup", MouseUpHandler);
    map.DetachEvent("onclick", MapClickHandler);
}

/**
* Re-attaches mouse events
*/
function AttachMouseOverEvents()
{
    map.AttachEvent("onmouseover", HoverHandler);
    map.AttachEvent("onmouseout", FadePopup);
    map.AttachEvent("onmousedown", MouseDownHandler);
    map.AttachEvent("onmouseup", MouseUpHandler);
    map.AttachEvent("onclick", MapClickHandler);
}

/**
* Mouse down handler for dragging a POI 
*/
function MouseDownHandler(e){
    var tempPinDeleted = false;
    if (e.leftMouseButton){
        if (e.elementID != null && map.GetShapeByID(e.elementID) == customTempPin){
            tempPinDeleted = true;
        }
        cancelCustomClick(); 
        cancelAddConcierge();        
    }

    //Drag an attraction icon
    if (e.elementID != null && e.leftMouseButton && !tempPinDeleted && (currentUser.userId.length > 0)){
        HidePopup();
        disableHover = true;
        document.body.onmousemove = OnMouseMoved;
        document.body.onmouseup = MouseUpHandler; 

        var shape = map.GetShapeByID(e.elementID);     
        if (shape != customTempPin){
            MainControl.Content.Controller.GetSerializedAttraction(shape.Attraction,
                function(sender, text)
                {
                    tmpAttractionSerial = text;
                    FloatingPinControl.Content.FloatingPin.Initialize(text);
                });
            return true;
        }
    }
}

/**
* Mouse up handler for dragging a POI
*/ 
function MouseUpHandler(e){
    document.body.onmousemove = null;
    document.body.onmouseup = null;
    HideFloatingPin();
    disableHover = false;
}

/**
* Set center position of map
*/
function setMapCenter(sender, args){
    centering = true;
    centerGoal = new VELatLong(args.Latitude, args.Longitude);
    map.PanToLatLong(centerGoal);
}

/**
* Adds a pin to the map
*/
function addPushpin(sender, args){

    var position = new VELatLong(args.Latitude, args.Longitude);
    var layerCount = layers[args.List].length;
    var layer;
    
    while (layerCount <= (args.Category)){
        layer = new VEShapeLayer();
        map.AddShapeLayer(layer);
        layers[args.List].push(layer);
        layerCount = layers[args.List].length;
    } 
    
    layer = layers[args.List][args.Category];
    
    var shape = new VEShape(VEShapeType.Pushpin,position);

    shape.Attraction = args.AttractionGuid;
    var url = VP_DEFAULT_PUSHPIN;

    //Setting custom icon images 
    switch (args.List){
        case 0:
            if (args.PushpinURL != null && args.PushpinURL != ""){
                url = args.PushpinURL;
            }
            if (url == VP_HOME_PUSHPIN)
                shape.SetZIndex(1001);                            
            break;
            
        case 1:
            if (args.Category == 6) //shared 
                url = VP_SAVED_PUSHPIN;
            else 
                url = VP_DEFAULT_PUSHPIN;
            break;
        case 2:
        case 3:
            url = VP_SAVED_PUSHPIN;
            break;
    }

    //If the image url is a hyperlink
    if (url.indexOf("http://",0) != 0){
        var loc = window.location.href.substr(0,window.location.href.lastIndexOf("/"));
        while (url.indexOf("../") == 0){
            loc = loc.substr(0,loc.lastIndexOf("/"));
            url = url.substr(3);
        }
        
        if (url.indexOf("./") == 0){
            url = url.substr(2);
        }
        
        url = loc + "/" + url;
    }
    
    var Shape3d = new VECustomIconSpecification();
    Shape3d.Image = url;
    
    shape.SetCustomIcon(Shape3d);
    layer.AddShape (shape);
}

/**
* Deletes a pushpin from the map
*/
function removePushpin(sender, args){
    var layer = layers[args.List][args.Category];    
    var count = layer.GetShapeCount();
    
    for (var x = 0; x<count; x++){
        var shape = layer.GetShapeByIndex(x);
        if (shape.Attraction == args.AttractionGuid){
            var position = shape.GetPoints()[0];            
            
            layer.DeleteShape(shape);
            return;
        }
    }    
}

function ResetMap(){
    MainControl.Content.MapPanel.ResetMap();
    SetBestMapView();
}

/**
* Sets appropriate centerpoint and zoom level of map
*/
function SetBestMapView(){
    var locations = new Array();
    var count = map.GetShapeLayerCount();      
    
    for (var x = 0; x < count; x++){
        var layer = map.GetShapeLayerByIndex(x);
        if (layer.IsVisible){
            var shapeCount = layer.GetShapeCount();
            for (var y = 0; y < shapeCount; y++){
                locations = locations.concat(layer.GetShapeByIndex(y).GetPoints());
            }
        }
    }
    
    if (locations.length > 0)
        map.SetMapView(locations);
}

/////////////////////////////////////////////////////////////////////////////////
// My Places functions
/////////////////////////////////////////////////////////////////////////////////

/**
* When user clicks "OK" to adding a custom POI to their collection 
* New collection is save into database for that user
*/
function saveCustomClick(){
    var CustomPinPopup = document.getElementById("AddCustomPin"); 
    var customTitle = document.getElementById("CustomTitle").value;
    var customDescription = document.getElementById("CustomDescription").value;
    var customNotes = document.getElementById("CustomNotes").value;
    var customImageURL = document.getElementById("CustomImageURL").value;
    var customVideoURL = document.getElementById("CustomVideoURL").value;
    
    if (customTitle == "")
        customTitle = "Custom Place";
    
    if (customDescription == "<Enter a description for your item here.>")
        customDescription = "";
        
    if (customNotes == "<Enter your notes here.>")
        customNotes = "";
         
    if (customImageURL == "")
        customImageURL= "images/Btn_PushPin_saved.png";
    
    var resultString = " {";
    
    resultString += "\"Title\" : \""+filterText(customTitle)+"\",";
    resultString += "\"ImageURL\" : \""+filterText(customImageURL)+"\",";
    resultString += "\"VideoURL\" : \""+filterText(customVideoURL)+"\",";
    resultString += "\"Category\" : 6,"; //6 == "Custom"
    resultString += "\"ShortDescription\" : \""+filterText(customDescription)+"\",";
    resultString += "\"LongDescription\" : \""+filterText(customNotes)+"\",";        
    resultString += "\"AddressLine1\" : \"\",";
    resultString += "\"AddressLine2\" : \"\",";
    resultString += "\"Latitude\" : \"" + customLatLong.Latitude + "\",";
    resultString += "\"Longitude\" : \"" + customLatLong.Longitude + "\" ";
    resultString += "}";

    //Stores new item in user"s collection and saves to database
    MainControl.Content.MyPlacesToolBar.AddCustomPlace(resultString);
    CustomPinPopup.style.display = "none";
    if(customTempPin != null)
        map.DeleteShape(customTempPin);
}



/**
* When user clicks cancels request to add custom POI
*/
function cancelCustomClick(){
    var CustomPinPopup = document.getElementById("AddCustomPin");
    CustomPinPopup.style.display = "none";
    if(customTempPin != null)
        map.DeleteShape(customTempPin);
}


/////////////////////////////////////////////////////////////////////////////////
// Dragable pin
/////////////////////////////////////////////////////////////////////////////////
/**
* Hides the floating pin when mouse button is released by user
*/
function HideFloatingPin(){
    var popup = document.getElementById("floatingPin");
    if (popup != null){
        popup.style.top = "-500px";
        popup.style.left = "-500px";
    }
    
    if (is3D){
        Hide3DFrame(FloatingPinControl);
    }
}


/**
* Returns the x and y pixel coordinates of the cursor
*/
function positionFloatingPin(e)
{
        var tempX, tempY;
        var offset = map.GetLeft();  //Accomodates for centering and resizing
        var IE = document.all?true:false
        if (IE) { // grab the x-y pos.s if browser is IE
            //If it"s a window event
            if (window.event != null){
                e = window.event;
            }
            tempX = e.clientX + document.documentElement.scrollLeft - offset;
            tempY = e.clientY + document.documentElement.scrollTop;  
        } else {  // grab the x-y pos.s if browser is NS
            tempX = e.pageX - offset;
            tempY = e.pageY;
        }  
        //moving the container that holds the silverlight floating pin object      
	    var popup = document.getElementById("floatingPin");    
	    popup.style.top = tempY + 2 + "px";    
	    popup.style.left = tempX + 2 + "px";
	    
	    //Disabling floating pin in 3D mode due to mouse event limitations 
	    
	    return new Array ( tempX, tempY );
}

/////////////////////////////////////////////////////////////////////////////////
// Location pop-up
/////////////////////////////////////////////////////////////////////////////////
/**
* Hides tour item popup
*/
function HidePopup(){
    var popup = document.getElementById("InfoPopup");
    if (popup != null){
        popup.style.top = "-500px";
        popup.style.left = "-500px";
    }
    
    try
    {
        if (is3D){
           Hide3DFrame(InfoPopupControl);
        }
    }
    catch (e)
    {
    }
}

/**
* Shows tour item popup
*/
function showPopup(x, y, attractionHash)
{
    var popup = document.getElementById("InfoPopup");
    
    popup.style.top = y + "px";
    popup.style.left = x + "px";
 
    if (is3D){
        Show3DFrame(x,y,InfoPopupControl);
    }

    try
    {
    MainControl.Content.Controller.GetSerializedAttraction(
        attractionHash, 
        function(sender, text)
        {
            InfoPopupControl.Content.PopupItem.Initialize(text);
            InfoPopupControl.Content.PopupItem.FadeInPopup();
        });
    }
    catch(e)
    {
        debugger;
    }
}

/**
* Invokes Silverlight storyboard to fade out tour popup item
*/
function FadePopup()
{
    if (InfoPopupControl != null){
        InfoPopupControl.Content.PopupItem.FadeOutPopup();
    }
}

/////////////////////////////////////////////////////////////////////////////////
// Filtering
/////////////////////////////////////////////////////////////////////////////////
/**
* Layers array
*/
var layers = new Array();
layers.push(new Array());
layers.push(new Array());
layers.push(new Array());
layers.push(new Array());

/**
* Hide or display POIs based on categories selected from checkboxes in Concierge panel
*/
function ChangeMapFilter(sender, args){
    SetDirectionsDialogState(false);
    SetDirectionsButtonState(true);
    
    eval("var obj = " + args.FilterJSON + ";");
    
    for (var x = 0; x<obj.filters.Types.length; x++){
        for (var y = 0; y<obj.filters.Types[x].Categories.length; y++){
            if (layers[obj.filters.Types[x].idx].length > obj.filters.Types[x].Categories[y].idx){
                if (obj.filters.Types[x].Categories[y].value){
                    layers[obj.filters.Types[x].idx][obj.filters.Types[x].Categories[y].idx].Show();
                } else {
                    layers[obj.filters.Types[x].idx][obj.filters.Types[x].Categories[y].idx].Hide();
                }
            }
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////
// Guided Tour
/////////////////////////////////////////////////////////////////////////////////
/**
* Top pixel coordinate of Guided Tour Control
*/
var tourControlTop = 0;

/**
* Width of guided tour control
*/
var tourControlWidth = 0;

/**
* Visibility of guided tour control
*/
var tourControlVisible = false;

/**
* Flag to hide drop box during the guided tour
*/
var hideBoxForTour = false;

/**
* Displays the guided tour control
*/
function showTourControl()
{
    tourControlVisible = true;
    var tourControlHost = document.getElementById("TourControlHost");
    var myMap = document.getElementById("myMap");
    tourControlHost.style.top = tourControlTop;
    
    TourControl.Content.TourControl.Resize(parseInt(myMap.style.width.split("px")[0]));
    
    TourControl.Content.TourControl.StartTour();
    
    var attractionDropHost = document.getElementById("AttractionDropBoxHost");
    
    if (attractionDropHost.style.zIndex == "35"){
        var o = new Object();
        o.State = false;
        hideBoxForTour = true;
        SetAttractionDropBoxState(null,o);
    }
    
    if (is3D){
        Show3DFrame(tourControlHost.style.left.split("px")[0],tourControlTop.split("px")[0],TourControl);
    }
    
    SetDirectionsButtonState(false);
    SetDirectionsDialogState(false);

    if (currentUser.userId.length > 0)
        document.getElementById("AttractionDropBoxHost").style.zIndex = "-1";
}

/**
* Hides the guided tour control
*/
function hideTourControl()
{
    tourControlVisible = false;
    var tourControlHost = document.getElementById("TourControlHost");
    tourControlHost.style.top = "-500px";
    
    if (hideBoxForTour){
        var o = new Object();
        o.State = true;
        hideBoxForTour = false;
        SetAttractionDropBoxState(null,o);
    }
    
    if (is3D){
        Hide3DFrame(TourControl);
    }
    
    SetDirectionsButtonState(true);

    if (currentUser.userId.length > 0)
        document.getElementById("AttractionDropBoxHost").style.zIndex = "34";
}

/**
* Displays or Hides the tour pop up item.
*/  
function MoveTourItemControl(sender, args){
    var control = document.getElementById("TourPopupHost");
    var offset = map.GetLeft();
    if (!args.Center){
        control.style.left = args.Left - offset + "px";
        control.style.top = args.Top + "px";
    } else {
        var myMap = document.getElementById("myMap");
        var pos = findPos(myMap);
        
        control.style.left = pos[0] + myMap.style.width.split("px")[0]/2 - TourPopupControl.width/2 - offset + "px";
        control.style.top = myMap.style.height.split("px")[0]/2 - TourPopupControl.height/2 + 70 + "px";
    }
    
    if (is3D){
        if (args.Center || args.Left > 0){
            Show3DFrame(control.style.left.split("px")[0] - offset,control.style.top.split("px")[0],TourPopupControl);
        } else {
            Hide3DFrame(TourPopupControl);
        }
    }
}

/////////////////////////////////////////////////////////////////////////////////
// Search functions
/////////////////////////////////////////////////////////////////////////////////
/**
* Performs Yellow Pages search based in user input
*/
function DoSearch(sender, args){
    map.Find(args.SearchText,null, VEFindType.Businesses, null, null, 20, false, false,false,false,SearchCallback);
}

/**
* Call back for Search call back
*/
function SearchCallback(shapeLayer, findResults, places, moreResults, errMsg){
   
    if (findResults == null) return;
    
    var resultString = "{ \"attractions\": [ ";
    
    for (var x = 0; x<findResults.length; x++){
        var result = findResults[x];
        
        resultString += " {";
        
        resultString += "\"Title\" : \"" + result.Name + "\",";
        resultString += "\"ImageURL\" : \"images/Btn_PushPin.png\",";
        resultString += "\"Category\" : 3,"; //3 == "Search"
        resultString += "\"ShortDescription\" : \"" + result.Phone + "\",";
        resultString += "\"LongDescription\" : \"" + result.Phone + "\",";
        
        var firstComma = result.Description.indexOf(",");
        
        resultString += "\"AddressLine1\" : \"" + result.Description.substr(0,firstComma) + "\",";
        resultString += "\"AddressLine2\" : \"" + result.Description.substr(firstComma) + "\",";
        resultString += "\"Latitude\" : \"" + result.LatLong.Latitude + "\",";
        resultString += "\"Longitude\" : \"" + result.LatLong.Longitude + "\" ";
        
        if (x<findResults.length-1)
            resultString += "}, ";
        else 
            resultString += "} ";
    }
    
    resultString += "] }";
    
    SetDirectionsDialogState(false);
    SetDirectionsButtonState(true);
    
    MainControl.Content.Controller.SetSearchResults(resultString);

}

/////////////////////////////////////////////////////////////////////////////////
// Error handling
/////////////////////////////////////////////////////////////////////////////////
/**
* Error handler
*/
function OnErrorHandler(sender, errorArgs)
{
    // The error message to display.
    var errorMsg = "Silverlight Error: \n\n";
    
    // Error information common to all errors.
    errorMsg += "Error Type:    " + errorArgs.errorType + "\n";
    errorMsg += "Error Message: " + errorArgs.errorMessage + "\n";
    errorMsg += "Error Code:    " + errorArgs.errorCode + "\n";
    
    // Determine the type of error and add specific error information.
    switch(errorArgs.errorType)
    {
        case "RuntimeError":
            // Display properties specific to RuntimeErrorEventArgs.
            if (errorArgs.lineNumber != 0)
            {
                errorMsg += "Line: " + errorArgs.lineNumber + "\n";
                errorMsg += "Position: " +  errorArgs.charPosition + "\n";
            }
            errorMsg += "MethodName: " + errorArgs.methodName + "\n";
            break;
        case "ParserError":
            // Display properties specific to ParserErrorEventArgs.
            errorMsg += "Xaml File:      " + errorArgs.xamlFile      + "\n";
            errorMsg += "Xml Element:    " + errorArgs.xmlElement    + "\n";
            errorMsg += "Xml Attribute:  " + errorArgs.xmlAttribute  + "\n";
            errorMsg += "Line:           " + errorArgs.lineNumber    + "\n";
            errorMsg += "Position:       " + errorArgs.charPosition  + "\n";
            break;
        default:
            break;
    }
    // Display the error message.
    alert(errorMsg);
}

/////////////////////////////////////////////////////////////////////////////////
//Utility functions
/////////////////////////////////////////////////////////////////////////////////
/**
* Raised after an asynchronous postback is complete. Used for error logging.
*/
function EndRequestHandler(sender, args)
{
   if (args.get_error() != undefined){
       args.set_errorHandled(true);     
   }
}

/**
* Opens Windows Live Contacts popup window
*/
function ShowShareDialog(sender, args)
{
    if (currentUser.userId.length > 0)
        window.open("Share.aspx?uid=" + currentUser.userId, "_blank", "height=450,width=320"); 
}

/**
* The page"s onmousemove event
* Attached on the VE map left button down event when on a pushpin and the user is logged in
* Detached (set to null) on the VE map left moust button up event
*/
function OnMouseMoved(e)
{
    // the mouse cursor position as the mouse moves
    var pos = positionFloatingPin(e);
    var x = pos[0]; 
    var y = pos[1];
    AttractionDropBoxControl.Content.AttractionDropBox.HitDetect(x,y,tmpAttractionSerial);
}

/**
* Finds the top and left pixel coordinates of an object
*/
function findPos(obj) {
	var curleft = curtop = 0;
	if (obj.offsetParent) {
		curleft = obj.offsetLeft;
		curtop = obj.offsetTop;
		obj = obj.offsetParent;
		while (obj) {
			curleft += obj.offsetLeft;
			curtop += obj.offsetTop;
			obj = obj.offsetParent;
		}
	}
	return [curleft,curtop];
}

/**
* Removes double quotes from input fields becuase they would 
* mess up the results string which is deserialized to an attraction object
*/
function filterText(text)
{
    var filteredText = text.replace(/"/g, ""); 
    return filteredText;
}

/**
* Called when map changes between 2D and 3D modes
*/
function ModeChanged(){
    var mode = map.GetMapMode();
    if (mode == VEMapMode.Mode3D){
        is3D = true;
        InfoPopupControl.Content.PopupItem.Set3D(true);
        TourPopupControl.Content.TourItem.Set3D(true); 
        SetDirectionsButtonState(true);   
        if (tourControlVisible)
        {
            var tourControlHost = document.getElementById("TourControlHost");
            Show3DFrame(tourControlHost.style.left.split("px")[0],tourControlTop.split("px")[0],TourControl); 
        }
    } else {
        is3D = false;
        InfoPopupControl.Content.PopupItem.Set3D(false);
        TourPopupControl.Content.TourItem.Set3D(false); 
        Hide3DFrame(FloatingPinControl);
        Hide3DFrame(InfoPopupControl);
        Hide3DFrame(TourPopupControl);
        Hide3DFrame(TourControl);
        Hide3DFrame(PlaceListHoverControl);
        Hide3DFrame(DirectionsButtonControl);
        Hide3DFrame(AttractionDropBoxControl);
        Hide3DFrame(CustomPinPopup);
        Hide3DFrame(ConciergeAddRemovePopup);
        Hide3DFrame(AddConciergePopup);
    }
}

/**
* Creates and positions a floating iFrame so that elements can be 
* displayed over top of the 3D map
*/
function Show3DFrame(x,y,control){
    var ThreeDeeFrame;
    if (control.threedeeframe == null){
        ThreeDeeFrame = document.createElement("iframe");
        ThreeDeeFrame.style.position = "absolute";
        ThreeDeeFrame.style.zIndex = "-1";
        ThreeDeeFrame.style.backgroundColor = "white";
        ThreeDeeFrame.frameBorder="0";
        ThreeDeeFrame.scrolling="no";
        
        //CustomPinPopup, ConciergeAddRemovePopup, and AddConciergePopup are at different node levels
        //than all the other silverlight container controls on the page.
        if (control == CustomPinPopup || control == ConciergeAddRemovePopup || control == AddConciergePopup){
            control.appendChild(ThreeDeeFrame);
        }
        else
        {
            //control.style.zIndex = "1";
            control.parentElement.appendChild(ThreeDeeFrame);
            //control.parentElement.insertBefore(ThreeDeeFrame, control);
        }
        control.threedeeframe = ThreeDeeFrame;
    } 
    else {
        ThreeDeeFrame = control.threedeeframe;
    }
    
    ThreeDeeFrame.style.top = "0px";
    ThreeDeeFrame.style.left = "0px";
    
    if (control == PlaceListHoverControl){
        ThreeDeeFrame.style.width = "90px";
        ThreeDeeFrame.style.height = "50px";
    } 
    else {
        ThreeDeeFrame.style.width = control.clientWidth + "px";
        ThreeDeeFrame.style.height = control.clientHeight + "px";
    }
    
}

/**
* Removes floating iFrame in 2D mode
*/
function Hide3DFrame(control){
    if (control != null && control.threedeeframe != null){
    
        //CustomPinPopup, ConciergeAddRemovePopup, and AddConciergePopup are at different node levels
        //than all the other silverlight container controls.
        if (control == CustomPinPopup || control == ConciergeAddRemovePopup || control == AddConciergePopup){
            control.removeChild(control.threedeeframe);
        }
        else
            control.parentElement.removeChild(control.threedeeframe);
        control.threedeeframe = null;
    }
}

/**
* Called each time a POI in the guided tour is centered
*/
function CenterComplete(){
    if (centering){    
        centering = false;
        MainControl.Content.MapPanel.CenterComplete(centerGoal.Latitude, centerGoal.Longitude);
    }   
}

/**
* Invoked when a tour item popup opens or closes
*/
function ThreadCompletionCallback(){
    MainControl.Content.Controller.ThreadCompletionCallback();
}

/**
 * Remove characters from a string to prevent XSS
 */
 function XssEncode(string){
     if (typeof string == "string")
     {
        return string.replace( /[^\w\s,\.-]/g, "" );
     }
     return "";
 }