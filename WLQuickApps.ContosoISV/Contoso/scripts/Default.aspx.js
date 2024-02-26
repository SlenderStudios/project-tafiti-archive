/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com

var DragController;
    
function Page_Load() {  
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

    // Call the create silverlight - this is in SilverlihtContent.js
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
}

function Rollover(e) {
    Sys.UI.DomElement.toggleCssClass(e.target, "mouseover");
}

function showpresence(presence) 
{ 
	$get("userstatus").innerhtml=presence[0]["statustext"]; 
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
