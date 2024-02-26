// Global variables ..
// Timer for detecting single\double click on video
var CLICKTIMER=null;

// Last Time left click on Video
var LASTCLICKTIME=null;

function StartPlayer(parentId) {

        Silverlight.createHostedObjectEx({
        source: "player.xaml",          // Source property value.
        parentElement:$get(parentId),    // DOM reference to hosting DIV tag.
        id:'VideoControl',                 // Unique control id value.
        properties:{                    // Control properties.
            width:'100%',               // Width of rectangular region of control in pixels.
            height:'100%',               // Height of rectangular region of control in pixels.
            inplaceInstallPrompt:true, // Determines whether to display in-place install prompt if invalid version detected.
            background:'#000000',         // Background color of control.
            isWindowless:'false',       // Determines whether to display control in Windowless mode.
            framerate:'24',             // MaxFrameRate property value.
            version:'1.0'},             // Control version.
        events:{
            onLoad:onVideoControlLoaded}, 
        initParams: ["streaming:/4650/BikeCam/NeilRun2.wmv"]});
}
    
function onVideoControlLoaded()
{
        // Whenever a marker is reached in the video, call onMarkerReached                          
        var control = document.getElementById("VideoControl");
        var media = control.content.findName("VideoWindow");
        media.addEventListener("markerReached", onMarkerReached);
        media.addEventListener("MouseLeftButtonDown", onLeftClickDownVideo);
        media.addEventListener("MouseLeftButtonUp", onLeftClickUpVideo);
        control.content.OnFullScreenChange=onFullScreen;
        media.source=GetVideoURL(0);
}	
function onLeftClickDownVideo(sender, mouseEventArgs)
{
    if (!CLICKTIMER)
    {
        CLICKTIMER=window.setTimeout(Click, 300);
    } 
}	
function onLeftClickUpVideo(sender, mouseEventArgs)
{
    var clicktime=(new Date).getTime();
    if (LASTCLICKTIME)
    {
        if (clicktime-LASTCLICKTIME < 300)
        {
            if (CLICKTIMER)
            {
                window.clearTimeout(CLICKTIMER); 
                CLICKTIMER=0;
                var control = $get("VideoControl");
                control.content.fullScreen=true;
            }
        } 
        LASTCLICKTIME=0;
    }
    else
    {
        LASTCLICKTIME=clicktime;
    } 
}	
function Click()
{
    CLICKTIMER=0;
    var control = $get("VideoControl");
    var media = control.content.findName("VideoWindow");
    
    if (PAUSED)
    {
        PAUSED=false;
        media.Play();
    }
    else
    {
        PAUSED=true;
        media.Pause();
    }
}
function onFullScreen()
{
    var control = $get("VideoControl");
    var media = control.content.findName("VideoWindow");
    media.width=control.content.actualWidth;
    media.height=control.content.actualHeight;
}
function GetVideoURL(mediaIndex)
{
     var slControl=document.getElementById('VideoControl');
     var params = slControl.initParams.split(",");
     return params[mediaIndex];
}
	
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded(); 