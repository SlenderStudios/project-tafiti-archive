/// Authors:   Dr. Neil - neil@aptovita.com

//array of videos shown in the horizontal strip when in place 
// and vertical strip when full screen
var vidArray = null;

// the array vid that is currently in the center 
var centerVid = 2;

var canvasWidth;
var canvasHeight;	

//video control globals
var mainMute = "true";
var mainPause = false;
var vidTimerToggle = null;
var vidTimerToggleShadow = null;

// inti the vid array with the vids in the XAML file
function initVidArray(sender)
{
	vidArray = new Array();
	vidArray[0] = sender.findName("vid1"); 
	vidArray[1] = sender.findName("vid2"); 
	vidArray[2] = sender.findName("vid3"); 
	vidArray[3] = sender.findName("vid4"); 
	vidArray[4] = sender.findName("vid5"); 
}

function onLoaded(sender, args)
{
	if (vidArray == null)
	{
		initVidArray(sender);
	}
	
	for(i=0;i<5;i++)
	{
		//this is so we can loop the videos
		vidArray[i].addEventListener("mediaEnded", "videoEnded");
	}
			
	// Retrieve a reference to the control.
	control = sender.getHost();

	setCenterVideo(control, sender);
	
	// Set the event handler function for the OnFullScreenChange event.
	control.content.onFullScreenChange = fullScreenChangedEvent;

	// Do initial layout of the app based on initial size.
	updateLayout(sender, control.content.fullScreen,
		control.content.actualWidth, 
		control.content.actualHeight);
	
	setVidControls(sender);
	
	hideRollovers(sender);
	
	setProgressControl(sender);
}

function setVidControls(sender)
{
	playIcon = sender.findName("play"); 
	pauseIcon = sender.findName("pause"); 

	//remember collapsed == hidden in Silverlight 1.0
	if (vidArray[centerVid].CurrentState == "Playing" )
	{
		playIcon.visibility = "Collapsed";
		pauseIcon.visibility = "Visible";
	}
	else
	{
		pauseIcon.visibility = "Collapsed";
		playIcon.visibility = "Visible";
	}
	
	unmuteIcon = sender.findName("unmuteVid"); 
	muteIcon = sender.findName("muteVid"); 
	
	if (vidArray[centerVid]["IsMuted"])
	{
		muteIcon.visibility = "Collapsed";
		unmuteIcon.visibility = "Visible";
	}
	else
	{
		unmuteIcon.visibility = "Collapsed";
		muteIcon.visibility = "Visible";
	}
}


function setProgressControl(sender)
{
	if (sender != null)
	{
		vidTimerToggle = sender.findName("vidTimerToggle");
		vidTimerToggleShadow = sender.findName("vidTimerToggleShadow");
	}
	if (vidTimerToggle != null)
	{
		// the timer goes from 120 to 209 px along the video window
		if (vidArray[centerVid].naturalDuration != null)
		{
		    vidTimerToggle["Canvas.Left"] = 120;
		    if ( vidArray[centerVid].position.seconds > 0 
		        && vidArray[centerVid].naturalDuration.seconds > 0)
		    {
			    vidTimerToggle["Canvas.Left"] 
			        = 120 + ((vidArray[centerVid].position.seconds / vidArray[centerVid].naturalDuration.seconds) * 209);
			}
			vidTimerToggleShadow["Canvas.Left"] = vidTimerToggle["Canvas.Left"]+1;
		}
	}
	setTimeout("setProgressControl()", 100);
}

function hideRollovers(sender)
{
	playRollover = sender.findName("playRollover"); 
	playRollover.visibility = "Collapsed";	
	
	muteRollover = sender.findName("muteRollover"); 
	muteRollover.visibility = "Collapsed";	
}
		
function fullScreenChangedEvent(sender, eventArgs)
{
	control = sender.getHost();
	
	var isFullScreen = control.content.fullScreen;
	
	// Do layout resizing of the app whenever the FullScreen property changes.
	updateLayout(sender, isFullScreen,
		control.content.actualWidth, 
		control.content.actualHeight);
}
		
function returnToSite(sender, args)
{
	control.content.fullScreen = false;
}

// Resize and reposition application elements.
function updateLayout(sender, isFullScreen, width, height)
{
	control = sender.getHost();
	//reusing the same controls this repositions and resizes them
	// based on a fullscreen layout or an inplace layout
	
	if (isFullScreen)
	{
		//set the canvas width and height for the whole control
		videogoround_canvas = sender.findName("videogoround_canvas");
		videogoround_canvas.width = width;
		videogoround_canvas.height = height;
		
		//use the center video as a videobrush to paint the background
		mainVideoBrush = sender.findName("mainVideoBrush");
		mainVideoBrush.sourceName = vidArray[centerVid].name;
		
		//panel vid loactions
		//center
		vidArray[centerVid]["Canvas.Top"] = 322; 
		vidArray[centerVid]["Canvas.Left"] = 22; 
		vidArray[centerVid].Height = 80;
		vidArray[centerVid].Width = 160;
		vidArray[centerVid]["Canvas.ZIndex"] = 500;
		
		//left of center
		leftVid = centerVid-1;
		if(leftVid < 0)
		{
			leftVid = 4;
		}
		vidArray[leftVid]["Canvas.Top"] = 230; 
		vidArray[leftVid]["Canvas.Left"] = 22; 
		vidArray[leftVid].Height = 80;
		vidArray[leftVid].Width = 160;
		vidArray[leftVid]["Canvas.ZIndex"] = 500;
		
		//left of left
		leftVid = leftVid-1;
		if(leftVid < 0)
		{
			leftVid = 4;
		}
		vidArray[leftVid]["Canvas.Top"] = 140; 
		vidArray[leftVid]["Canvas.Left"] = 22; 
		vidArray[leftVid].Height = 80;
		vidArray[leftVid].Width = 160;
		vidArray[leftVid]["Canvas.ZIndex"] = 500;
		
		//right of center
		rightVid = centerVid+1;
		if(rightVid > 4)
		{
			rightVid = 0;
		}
		vidArray[rightVid]["Canvas.Top"] = 415; 
		vidArray[rightVid]["Canvas.Left"] = 22; 
		vidArray[rightVid].Height = 80;
		vidArray[rightVid].Width = 160;
		vidArray[rightVid]["Canvas.ZIndex"] = 500;
		
		//right of right
		rightVid = rightVid+1;
		if(rightVid > 4)
		{
			rightVid = 0;
		}
		vidArray[rightVid]["Canvas.Top"] = 510; 
		vidArray[rightVid]["Canvas.Left"] = 22; 
		vidArray[rightVid].Height = 80;
		vidArray[rightVid].Width = 160;
		vidArray[rightVid]["Canvas.ZIndex"] = 500;
		
		//show the return to inplace layout button
		returnBtn = sender.findName("return_to_site");
		returnBtn.visibility = "Visible";
		returnBtn["Canvas.ZIndex"] = 500;
		
		//hide the inplace layout elements not shown in full screen
		background = sender.findName("background");
		background.visibility = "Collapsed";
		
		video_005 = sender.findName("video_005");
		video_005.visibility = "Collapsed";
		video_004 = sender.findName("video_004");
		video_004.visibility = "Collapsed";
		video_003 = sender.findName("video_003");
		video_003.visibility = "Collapsed";
		video_002 = sender.findName("video_002");
		video_002.visibility = "Collapsed";
		video_001 = sender.findName("video_001");
		video_001.visibility = "Collapsed";
		
		arrows = sender.findName("arrows");
		arrows.visibility = "Collapsed";
		
		//show the large video panel
		video_panel = sender.findName("video_panel");
		video_panel.visibility = "Visible";
		
		//show the large full screen video control panel
		playIcon = sender.findName("play"); 
		pauseIcon = sender.findName("pause"); 
		unmuteIcon = sender.findName("unmuteVid"); 
		muteIcon = sender.findName("muteVid"); 

		pauseIcon.visibility = "Visible";
		playIcon.visibility = "Visible";
		unmuteIcon.visibility = "Visible";
		muteIcon.visibility = "Visible";
	
		playerControls = sender.findName("playerControls");
		//center the controls
		playerControls["Canvas.Left"] = 
			(control.content.actualWidth/2) - (playerControls.width/2);
		playerControls["Canvas.Top"] = control.content.actualHeight - 100;
				
		//apply a transform to the controls
		var xamlFragment = '<TransformGroup>';
		xamlFragment += '<ScaleTransform ScaleX="1" ScaleY="1"/>';
		xamlFragment += '<SkewTransform AngleX="0" AngleY="0"/>'
		xamlFragment += '<RotateTransform Angle="0"/>'
		xamlFragment += '<TranslateTransform X="0" Y="0"/>';
		xamlFragment += '</TransformGroup>';
		transform = control.content.createFromXaml(xamlFragment);
		playerControls.RenderTransform = transform;
		
		setVidControls(sender);
	}
	else
	{
		//hide the 'exit full screen' button
		returnBtn = sender.findName("return_to_site");
		returnBtn.visibility = "Collapsed";
		// show the swosh in the back ground
		background = sender.findName("background");
		background.visibility = "Visible";
		// show the in place video frames
		video_005 = sender.findName("video_005");
		video_005.visibility = "Visible";
		video_004 = sender.findName("video_004");
		video_004.visibility = "Visible";
		video_003 = sender.findName("video_003");
		video_003.visibility = "Visible";
		video_002 = sender.findName("video_002");
		video_002.visibility = "Visible";
		video_001 = sender.findName("video_001");
		video_001.visibility = "Visible";
		//show the scroll arrows
		arrows = sender.findName("arrows");
		arrows.visibility = "Visible";
		
		// hide the vertical video panel
		video_panel = sender.findName("video_panel");
		video_panel.visibility = "Collapsed";
		
		//vid positions
		//center - fixed for no transform
		vidArray[centerVid]["Canvas.Top"] = 35; 
		vidArray[centerVid]["Canvas.Left"] = 125; 
		vidArray[centerVid].Height = 160;
		vidArray[centerVid].Width = 300;
		
		//left of center
		leftVid = centerVid-1;
		if(leftVid < 0)
		{
			leftVid = 4;
		}
		vidArray[leftVid]["Canvas.Top"] = 61; 
		vidArray[leftVid]["Canvas.Left"] = 65; 
		vidArray[leftVid].Height = 100;
		vidArray[leftVid].Width = 180;
		vidArray[leftVid]["Canvas.ZIndex"] = 202;
		
		//left of left
		leftVid = leftVid-1;
		if(leftVid < 0)
		{
			leftVid = 4;
		}
		vidArray[leftVid]["Canvas.Top"] = 82; 
		vidArray[leftVid]["Canvas.Left"] = 35; 
		vidArray[leftVid].Height = 70;
		vidArray[leftVid].Width = 110;
		vidArray[leftVid]["Canvas.ZIndex"] = 101;
		
		//right of center
		rightVid = centerVid+1;
		if(rightVid > 4)
		{
			rightVid = 0;
		}
		vidArray[rightVid]["Canvas.Top"] = 61; 
		vidArray[rightVid]["Canvas.Left"] = 310; 
		vidArray[rightVid].Height = 100;
		vidArray[rightVid].Width = 180;
		vidArray[rightVid]["Canvas.ZIndex"] = 202;
		
		//right of right
		rightVid = rightVid+1;
		if(rightVid > 4)
		{
			rightVid = 0;
		}
		vidArray[rightVid]["Canvas.Top"] = 82; 
		vidArray[rightVid]["Canvas.Left"] = 405; 
		vidArray[rightVid].Height = 70;
		vidArray[rightVid].Width = 110;
		vidArray[rightVid]["Canvas.ZIndex"] = 101;
		
		//reset the video control panel
		playIcon = sender.findName("play"); 
		pauseIcon = sender.findName("pause"); 
		unmuteIcon = sender.findName("unmuteVid"); 
		muteIcon = sender.findName("muteVid"); 

		pauseIcon.visibility = "Visible";
		playIcon.visibility = "Visible";
		unmuteIcon.visibility = "Visible";
		muteIcon.visibility = "Visible";
	
		playerControls = sender.findName("playerControls");
		playerControls["Canvas.Left"] = 180;
		playerControls["Canvas.Top"] = 200;
		playerControls["Canvas.ZIndex"] = 600;
		// scale the video control panel down to half size
		var xamlFragment = '<TransformGroup>';
		xamlFragment += '<ScaleTransform ScaleX="0.5" ScaleY="0.5"/>';
		xamlFragment += '<SkewTransform AngleX="0" AngleY="0"/>'
		xamlFragment += '<RotateTransform Angle="0"/>'
		xamlFragment += '<TranslateTransform X="0" Y="0"/>';
		xamlFragment += '</TransformGroup>';
		transform = control.content.createFromXaml(xamlFragment);
		playerControls.RenderTransform = transform;
		
		setVidControls(sender);
		
	}
			
}
		
function videoClicked(sender, args)
{
	var control = sender.getHost();
	control.content.fullScreen = true;
}
		
function videoEnded(sender, args)
{
	sender.Position = "00:00:00";
	sender.Play();
}

var vidScrollEngaged = false;
var vidScrollLastY = 0;

function vidScrollMouseUp(sender, args) 
{
	vidScrollEngaged = false;
	vidScrollLastY = 0;
}

function vidScrollMouseDown(sender, args) 
{
	vidScrollEngaged = true;
	vidScrollLastY = args.getPosition(null).y;
}

function vidScrollMouseMove(sender, args) 
{
	if(vidScrollEngaged)
	{
		if (vidScrollLastY == 0)
		{
			vidScrollLastY = args.getPosition(null).y;
		}
		else
		{
			if( vidScrollLastY < (args.getPosition(null).y-15) )
			{
				leftBtnDown(sender, args);
				vidScrollLastY = args.getPosition(null).y;
			}
			else if (vidScrollLastY > (args.getPosition(null).y+15) )
			{
				rightBtnDown(sender, args);
				vidScrollLastY = args.getPosition(null).y;
			}
		}
	}
}


// function for the left arrow button down
// this scrolls the videos to the left
// this function is reused in full screen to scroll the videos up and down
function leftBtnDown(sender, args) 
{
	if (vidArray == null)
	{
		initVidArray(sender);
	}
			
	var tmpTop = vidArray[4]["Canvas.Top"];
	var tmpLeft = vidArray[4]["Canvas.Left"];
	var tmpHeight = vidArray[4].Height;
	var tmpWidth = vidArray[4].Width;
			
	vidArray[4]["Canvas.Top"]= vidArray[3]["Canvas.Top"];
	vidArray[4]["Canvas.Left"] = vidArray[3]["Canvas.Left"];
	vidArray[4].Height = vidArray[3].Height;
	vidArray[4].Width = vidArray[3].Width;
			
	vidArray[3]["Canvas.Top"] = vidArray[2]["Canvas.Top"];
	vidArray[3]["Canvas.Left"] = vidArray[2]["Canvas.Left"];
	vidArray[3].Height = vidArray[2].Height;
	vidArray[3].Width = vidArray[2].Width;
			
	vidArray[2]["Canvas.Top"] = vidArray[1]["Canvas.Top"];
	vidArray[2]["Canvas.Left"] = vidArray[1]["Canvas.Left"];
	vidArray[2].Height = vidArray[1].Height;
	vidArray[2].Width = vidArray[1].Width;
			
	vidArray[1]["Canvas.Top"] = vidArray[0]["Canvas.Top"];
	vidArray[1]["Canvas.Left"]= vidArray[0]["Canvas.Left"];
	vidArray[1].Height = vidArray[0].Height;
	vidArray[1].Width = vidArray[0].Width;
			
	vidArray[0]["Canvas.Top"] = tmpTop;
	vidArray[0]["Canvas.Left"] = tmpLeft;
	vidArray[0].Height = tmpHeight;
	vidArray[0].Width = tmpWidth;
					
	centerVid ++;
	if (centerVid > 4)
	{
		centerVid = 0;
	}
	
	// Retrieve a reference to the control.
	control = sender.getHost();
			
	setCenterVideo(control, sender);
}

// function for the right arrow button down
// this scrolls the videos to the right
// this function is reused in full screen to scroll the videos up and down
function rightBtnDown(sender, args) 
{
	if (vidArray == null)
	{
		initVidArray(sender);
	}
			
	var tmpTop = vidArray[0]["Canvas.Top"];
	var tmpLeft = vidArray[0]["Canvas.Left"];
	var tmpHeight = vidArray[0].Height;
	var tmpWidth = vidArray[0].Width;
			
	vidArray[0]["Canvas.Top"]= vidArray[1]["Canvas.Top"];
	vidArray[0]["Canvas.Left"] = vidArray[1]["Canvas.Left"];
	vidArray[0].Height = vidArray[1].Height;
	vidArray[0].Width = vidArray[1].Width;
			
	vidArray[1]["Canvas.Top"] = vidArray[2]["Canvas.Top"];
	vidArray[1]["Canvas.Left"] = vidArray[2]["Canvas.Left"];
	vidArray[1].Height = vidArray[2].Height;
	vidArray[1].Width = vidArray[2].Width;
	
	vidArray[2]["Canvas.Top"] = vidArray[3]["Canvas.Top"];
	vidArray[2]["Canvas.Left"] = vidArray[3]["Canvas.Left"];
	vidArray[2].Height = vidArray[3].Height;
	vidArray[2].Width = vidArray[3].Width;
			
	vidArray[3]["Canvas.Top"] = vidArray[4]["Canvas.Top"];
	vidArray[3]["Canvas.Left"]= vidArray[4]["Canvas.Left"];
	vidArray[3].Height = vidArray[4].Height;
	vidArray[3].Width = vidArray[4].Width;
			
	vidArray[4]["Canvas.Top"] = tmpTop;
	vidArray[4]["Canvas.Left"] = tmpLeft;
	vidArray[4].Height = tmpHeight;
	vidArray[4].Width = tmpWidth;
					
	centerVid--;
	if (centerVid < 0 )
	{
		centerVid = 4;
	}
	
	// Retrieve a reference to the control.
	control = sender.getHost();

	setCenterVideo(control, sender);
}
		
function setCenterVideo(control, sender)
{
	for(i=0;i<5;i++)
	{
		vidArray[i]["IsMuted"] = "true";
		vidArray[i]["Volume"] = 0;
		vidArray[i]["Opacity"] = 0.5;
		vidArray[i].removeEventListener("mouseLeftButtonDown", "videoClicked");
		//vidArray[i].play();
	}

	vidArray[centerVid]["IsMuted"] = mainMute;
	vidArray[centerVid]["Volume"] = 1;
	vidArray[centerVid]["Opacity"] = 1;
	
	if (mainPause)
	{
		vidArray[centerVid].pause();
	}
	
	var isFullScreen = control.content.fullScreen;
	if (isFullScreen)
	{
		vidArray[centerVid]["Canvas.ZIndex"] = 500;
		
		mainVideoBrush = sender.findName("mainVideoBrush");
		mainVideoBrush.sourceName = vidArray[centerVid].name;
	}
	else
	{
		//make it topmost
		vidArray[centerVid]["Canvas.ZIndex"] = 301;
	}
	
	//add event handler to show full screen when center video clicked
	vidArray[centerVid].addEventListener("mouseLeftButtonDown", "videoClicked")
			
	//reorder the Zindex so the videos are shown with the center on top
	var rightofCenter = centerVid+1;
	if ( rightofCenter > 4)
	{ 
		rightofCenter = 0;
	}
	vidArray[rightofCenter]["Canvas.ZIndex"] = 201;
	
	var leftofCenter = centerVid - 1;
	if (leftofCenter < 0)
	{
		leftofCenter = 4;
	}
	vidArray[leftofCenter]["Canvas.ZIndex"] = 201;
	
	
	var rightofCenter = rightofCenter+1;
	if ( rightofCenter > 4)
	{ 
		rightofCenter = 0;
	}
	vidArray[rightofCenter]["Canvas.ZIndex"] = 101;
	
	var leftofCenter = leftofCenter - 1;
	if (leftofCenter < 0)
	{
		leftofCenter = 4;
	}
	vidArray[leftofCenter]["Canvas.ZIndex"] = 101;
	
}

function vidMute(sender, args)
{
	muteIcon = sender.findName("muteVid"); 
	unmuteIcon = sender.findName("unmuteVid"); 
	
	vidArray[centerVid]["IsMuted"] = !vidArray[centerVid]["IsMuted"];
	
	if (vidArray[centerVid]["IsMuted"])
	{
		muteIcon.visibility = "Collapsed";
		unmuteIcon.visibility = "Visible";
		mainMute = "true";
	}
	else
	{
		unmuteIcon.visibility = "Collapsed";
		muteIcon.visibility = "Visible";
		mainMute = "false";
	}
}

//------------------------------------------
//video control panel functions
//------------------------------------------
function vidPause(sender, args)
{
	playIcon = sender.findName("play"); 
	playTextBlock = sender.findName("playTextBlock"); 
	pauseIcon = sender.findName("pause"); 
	
	if (vidArray[centerVid].CurrentState != "Playing" )
	{
		vidArray[centerVid].play();
		playIcon.visibility = "Collapsed";
		pauseIcon.visibility = "Visible";
		mainPause = false;
	}
	else
	{
		vidArray[centerVid].pause();
		pauseIcon.visibility = "Collapsed";
		playIcon.visibility = "Visible";
		mainPause = true;
	}
}

function playButtonMouseEnter(sender, args)
{
	playRollover = sender.findName("playRollover"); 
	playRollover.visibility = "Visible";
}

function playButtonMouseLeave(sender, args)
{
	playRollover = sender.findName("playRollover"); 
	playRollover.visibility = "Collapsed";
}

function muteButtonMouseEnter(sender, args)
{
	muteRollover = sender.findName("muteRollover"); 
	muteRollover.visibility = "Visible";
}

function muteButtonMouseLeave(sender, args)
{
	muteRollover = sender.findName("muteRollover"); 
	muteRollover.visibility = "Collapsed";
}
