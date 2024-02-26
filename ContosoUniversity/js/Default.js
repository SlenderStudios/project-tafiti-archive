// default.js
var player;

function OnLoad() 
{
    // Set the today text
    document.getElementById('Today').innerHTML = today();
    
    // Start Silverlight streaming
    //player=new StartPlayer_0("TVWrapper");
}

// Returns today's date
function today() {
    var monthName = new Array(12);	
        monthName[1] = "January";		
        monthName[2] = "February";
        monthName[3] = "March";
        monthName[4] = "April";
        monthName[5] = "May";
        monthName[6] = "June";
        monthName[7] = "July";
        monthName[8] = "August";
        monthName[9] = "September";
        monthName[10] = "October";
        monthName[11] = "November";
        monthName[12] = "December";
        
    var dayName = new Array(7);
        dayName[1] = "Sunday";
        dayName[2] = "Monday";
        dayName[3] = "Tuesday";
        dayName[4] = "Wednesday";
        dayName[5] = "Thursday";
        dayName[6] = "Friday";
        dayName[7] = "Saturday";

    var now = new Date();				

    return (dayName[now.getDay()+1] + " " + now.getDate() + " " + monthName[now.getMonth() + 1] + " " + now.getFullYear());
    
}

function DeleteCookies()
{
    DeleteCookie("ContosoUserId");
    DeleteCookie("DomainAuthToken");
    document.location.reload();
}

function SwapImage(on)
{
    if (on)
        document.getElementById("watn").src = "App_Themes/Default/images/watn_button_on.png";
    else
        document.getElementById("watn").src = "App_Themes/Default/images/watn_button_off.png";
}
    
function playVideo(videoNum)
{
     var videoFrame=$('VideoFrame');
     videoFrame.src=String.format("http://silverlight.services.live.com/invoke/4650/{0}/iframe.html",videoNum);
     document.getElementById("channel0").style.backgroundColor = "#1E1C21";
     document.getElementById("channel1").style.backgroundColor = "#1E1C21";
     document.getElementById("channel2").style.backgroundColor = "#1E1C21";
     document.getElementById("channel" + videoNum).style.backgroundColor = "#750A00";
}
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();


