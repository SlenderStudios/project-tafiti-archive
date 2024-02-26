// Copyright 2007 Microsoft Corp. All Rights Reserved.

// Main setup and control of tree component
var stage = null;	// root object tree

function createControl()
{
	var slVersion = "1.0.20716";
	var parentElement = document.getElementById("host");

	stage = new SLTree.Stage();
	Silverlight.createObject(
		"xaml/Stage.xaml",					// Source property value.
		parentElement,						// DOM reference to hosting DIV tag.
		"SLControl",						// Unique control id value.
		{	// Control properties
			width:"100%",					// Width of rectangular region of control in pixels.
			height:"100%",					// Height of rectangular region of control in pixels.
			inplaceInstallPrompt:false,		// Determines whether to display in-place install prompt if invalid version detected.
			background:"black",				// Background color of control.
			isWindowless:"false",			// Determines whether to display control in Windowless mode.
			framerate:slc_MaxFPS,			// MaxFrameRate property value.
			version:slVersion				// Control version to use.
		},
		{
			onError:null,						// OnError property value -- event handler function name.
			onLoad:methodCaller(stage, "Init")	// OnLoad property value -- event handler function name.
		},
		null);								// Context value -- event handler function name.
}

if (!window.methodCaller) {
    window.methodCaller = function (obj, methodName) {
        return function() {
            return obj[methodName].apply(obj, arguments);
        }
    };
}

//
// API Calls
//


// External call to set the stage's fullscreen state
function Stage_setFullScreen(fFullScreen)
{
	if(stage == null)
	{
		return(false);
	}
	
	stage.control.content.fullScreen = fFullScreen;
	
	return(true);
}