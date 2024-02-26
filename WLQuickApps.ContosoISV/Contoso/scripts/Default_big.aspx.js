/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com

var bChat=false;
var DragController;
var myLiveContacts;
    
function Page_Load() {  
    //addFavPic is defined in Default_BIG.aspx
    $get("NewsItems_ctl00_NewsItem").style.backgroundImage = addFavPic;
    $get("NewsItems_ctl02_NewsItem").style.backgroundImage = addFavPic;
    $get("NewsItems_ctl04_NewsItem").style.backgroundImage = addFavPic;

    //setup Rollovers
    $addHandler($get("NavWelcome"), "mouseover", Rollover);
    $addHandler($get("NavWelcome"), "mouseout", Rollover);
    $addHandler($get("NavDevelopment"), "mouseover", Rollover);
    $addHandler($get("NavDevelopment"), "mouseout", Rollover);
    $addHandler($get("NavSystems"), "mouseover", Rollover);
    $addHandler($get("NavSystems"), "mouseout", Rollover);
    $addHandler($get("NavCaseStudies"), "mouseover", Rollover);
    $addHandler($get("NavCaseStudies"), "mouseout", Rollover);
    $addHandler($get("NavPartnerShips"), "mouseover", Rollover);
    $addHandler($get("NavPartnerShips"), "mouseout", Rollover);
    $addHandler($get("NavContact"), "mouseover", Rollover);
    $addHandler($get("NavContact"), "mouseout", Rollover);
    $addHandler($get("ContactTabChat"), "mouseover", Rollover);
    $addHandler($get("ContactTabChat"), "mouseout", Rollover);
    $addHandler($get("ContactTabChat"), "click", ShowChat);
    $addHandler($get("ContactTabContacts"), "mouseover", Rollover);
    $addHandler($get("ContactTabContacts"), "mouseout", Rollover);
    $addHandler($get("ContactTabContacts"), "click", ShowContacts);
    
    ShowChat();
    myLiveContacts = new Contoso.LiveContacts();
    SetupDragandDrop();
    
    createSilverlightControl();
    
}  

function Page_Unload() {
    //unload Rollovers
    $removeHandler($get("NavWelcome"), "mouseover", Rollover);
    $removeHandler($get("NavWelcome"), "mouseout", Rollover);
    $removeHandler($get("NavDevelopment"), "mouseover", Rollover);
    $removeHandler($get("NavDevelopment"), "mouseout", Rollover);
    $removeHandler($get("NavSystems"), "mouseover", Rollover);
    $removeHandler($get("NavSystems"), "mouseout", Rollover);
    $removeHandler($get("NavCaseStudies"), "mouseover", Rollover);
    $removeHandler($get("NavCaseStudies"), "mouseout", Rollover);
    $removeHandler($get("NavPartnerShips"), "mouseover", Rollover);
    $removeHandler($get("NavPartnerShips"), "mouseout", Rollover);
    $removeHandler($get("NavContact"), "mouseover", Rollover);
    $removeHandler($get("NavContact"), "mouseout", Rollover);
    $removeHandler($get("ContactTabChat"), "mouseover", Rollover);
    $removeHandler($get("ContactTabChat"), "mouseout", Rollover);
    $removeHandler($get("ContactTabChat"), "click", ShowChat);
    $removeHandler($get("ContactTabContacts"), "mouseover", Rollover);
    $removeHandler($get("ContactTabContacts"), "mouseout", Rollover);
    $removeHandler($get("ContactTabContacts"), "click", ShowContacts);
}

function Rollover(e) {
    Sys.UI.DomElement.toggleCssClass(e.target, "mouseover");
}

function ShowChat() {
    if(!bChat) {
        $get("ContactsPanel").style.display="none";
        $get("ChatPanel").style.display="block";
        bChat=true;
    }
}

function ShowContacts() {
    if(bChat) {
        $get("ContactsPanel").style.display="block";
        $get("ChatPanel").style.display="none";
        bChat=false;
    }
}

function showpresence(presence) 
{ 
	$get("userstatus").innerhtml=presence[0]["statustext"]; 
} 

//drag and drop to favorites
function SetupDragandDrop()
{
    //drop zone
    $create(Contoso.DropZoneBehavior, {'ServiceMethod':Contoso.services.ContosoService.SaveFavorite}, null, null, $get('FavoriteContent'));
    
    //setup dragable elements
    if (navigator.appName == "Microsoft Internet Explorer")
    {
        $create(Contoso.DragSourceBehavior, {'DataType':"TypeNews", 'ID':$get("NewsItems_ctl00_NewsItem").parentNode.childNodes[0].innerText}, null, null, $get("NewsItems_ctl00_NewsItem"));
        $create(Contoso.DragSourceBehavior, {'DataType':"TypeNews", 'ID':$get("NewsItems_ctl02_NewsItem").parentNode.childNodes[0].innerText}, null, null, $get("NewsItems_ctl02_NewsItem"));
        $create(Contoso.DragSourceBehavior, {'DataType':"TypeNews", 'ID':$get("NewsItems_ctl04_NewsItem").parentNode.childNodes[0].innerText}, null, null, $get("NewsItems_ctl04_NewsItem"));
    }
    else
    {
        $create(Contoso.DragSourceBehavior, {'DataType':"TypeNews", 'ID':$get("NewsItems_ctl00_NewsItem").parentNode.childNodes[1].innerText}, null, null, $get("NewsItems_ctl00_NewsItem"));
        $create(Contoso.DragSourceBehavior, {'DataType':"TypeNews", 'ID':$get("NewsItems_ctl02_NewsItem").parentNode.childNodes[1].innerText}, null, null, $get("NewsItems_ctl02_NewsItem"));
        $create(Contoso.DragSourceBehavior, {'DataType':"TypeNews", 'ID':$get("NewsItems_ctl04_NewsItem").parentNode.childNodes[1].innerText}, null, null, $get("NewsItems_ctl04_NewsItem"));
    }
    
}

function onSignin()
{}

function onSignout()
{}

function onError()
{
}

function receiveData(p_contacts)
{
    myLiveContacts.receiveData(p_contacts);
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
