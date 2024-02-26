// Profile.js

function SaveEmail(email)
{
	Sys.Services.ProfileService.properties.Email = email;
	Sys.Services.ProfileService.save(null, OnSaveCompleted, OnProfileFailed, null);
}

// The OnClickLogin function is called when 
// the user clicks the Login button. 
// It calls the AuthenticationService.login to
// authenticates the user.
function OnClickLogin()
{
	Sys.Services.AuthenticationService.login(
	    document.form1.userId.value,
	    document.form1.userPwd.value,false,null,null,
	    OnLoginComplete, OnAuthenticationFailed,
	    "User context information.");
}

// The OnClickLogout function is called when 
// the user clicks the Logout button. 
// It logs out the current authenticated user.
function OnClickLogout()
{
	Sys.Services.AuthenticationService.logout(
	    null, OnLogoutComplete, OnAuthenticationFailed,null);
}


function OnLogoutComplete(result, 
    userContext, methodName)
{
	// Code that performs logout 
	// housekeeping goes here.			
}		

// This function is called after the user is
// authenticated. It loads the user's profile.
// This is the callback function called 
// if the authentication completed successfully.
function OnLoginComplete(validCredentials, 
    userContext, methodName)
{
	if(validCredentials == true)
	{
		DisplayInformation("Welcome " + document.form1.userId.value); //BugID: 170706. 
			
		LoadProfile();
	}
	else
	{
		DisplayInformation("Could not login");
	}
}

// This is the callback function called 
// if the authentication failed.
function OnAuthenticationFailed(error_object, 
    userContext, methodName)
{	
    DisplayInformation("Authentication failed with this error: " +
	    error_object.get_message());
}


// Loads the profile of the current
// authenticated user.
function LoadProfile()
{
	Sys.Services.ProfileService.load(null, 
	    OnLoadCompleted, OnProfileFailed, null);
}

// Saves the new profile
// information entered by the user.
function SaveProfile()
{
	Sys.Services.ProfileService.properties.Email = $get("emailTextbox").value;
	Sys.Services.ProfileService.save(null, 
	    OnSaveCompleted, OnProfileFailed, null);
}

// Reads the profile information and displays it.
function OnLoadCompleted(numProperties, userContext, methodName)
{
    //alert('OnLoadCompleted:' + Sys.Services.ProfileService.properties.Email);
    //llau: removed
    //	document.bgColor = 
    //	    Sys.Services.ProfileService.properties.Backgroundcolor;

    //    document.fgColor =   
    //	    Sys.Services.ProfileService.properties.Foregroundcolor;			
}

// This is the callback function called 
// if the profile was saved successfully.
function OnSaveCompleted(numProperties, userContext, methodName)
{
	LoadProfile();
	// Hide the area that contains 
	// the controls to set the profile properties.
  SetProfileControlsVisibility("hidden");
}

// This is the callback function called 
// if the profile load or save operations failed.
function OnProfileFailed(error_object, userContext, methodName)
{
	alert("Profile service failed with message: " + 
	        error_object.get_message());
}


// Utility functions.

// This function sets the visibilty for the
// area containing the page elements for settings
// profiles.
function SetProfileControlsVisibility(currentVisibility)
{
}

// Utility function to display user's information.
function DisplayInformation(text)
{
	$get('placeHolder').innerHTML += 
	"<br/>" + SecureHtml(text); //BugID: 170706. 
}



function SecureHtml (s) {
	var secured = s;
	secured = secured.replace(/</g, "&lt;").replace(/>/g, "&gt;");
	secured = secured.replace(/script(.*)/g, "");    
	secured = secured.replace(/eval\((.*)\)/g, "");
	return (secured);
}


    
if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();