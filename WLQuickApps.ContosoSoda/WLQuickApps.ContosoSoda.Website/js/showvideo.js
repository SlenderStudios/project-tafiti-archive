var obj;
var winModalWindow;
var AppID = "99995658";
var emailID = "";
//In the code variable declarations above, the AppID variable is assigned the value of the game Hexic (1003) for users//within the US (1033). You should substitute the correct value assigned to your application (by the Support team)//for your desired market.
SaveCookie('CONTENTID', "testing", 10);
function TryItNow(AppID)
{
// Test #1: Launch with no email variable value causing the user to have to select another user to interact with.
CheckMessenger();
LaunchApp(AppID, "");
}

function LaunchApp(AppID, emailID)
{
if (obj != null)
{

obj.LaunchApp(AppID, emailID);

}
}
function CheckMessenger()
{
eval ('try {obj = new ActiveXObject("MSNMessenger.P4QuickLaunch"); } catch (e) {obj = null;}');
var strErrorPage = "http://www.dq.hk:8080/activity/Error.aspx"

 if (obj == null)
{

ShowWindow(strErrorPage, 410, 225);
}

}


 
function IgnoreEvents(e)
{
  return false
}
 
//Display error message if the MSN Messenger client 6.2 is not installed or the browser is not Internet Explorer
function ShowWindow(strError, width, height)
{
  if (window.showModalDialog)
  {
   
    window.showModalDialog(strError,null,
    "dialogWidth="+width+"px;dialogHeight="+height+"px;help=no;dialogLeft=160")
  }
  else
  {

    var ah = screen.availHeight;

    var y = (ah - height) / 2;

    window.top.captureEvents (Event.CLICK|Event.FOCUS)
    window.top.onfocus=HandleFocus 
    winModalWindow = window.open (strError,"ModalChild",
       "dependent=yes,width="+width+",height="+height+",top="+y+",left=160,screenX=160,screenY="+y)
    winModalWindow.focus()
  }
}

 
function HandleFocus()
{
  if (winModalWindow)
  {
    if (!winModalWindow.closed)
    {
      winModalWindow.focus()
    }
    else
    {
      window.top.releaseEvents (Event.CLICK|Event.FOCUS)
    }
  }
  return false
}

function SaveCookie(name, value, expirationTime)
           {
               var expirationDate=new Date();
               expirationDate.setDate(expirationDate.getDate()+expirationTime);
               document.cookie = name + "=" + escape(value) + ((expirationTime==null) ? "" : "; expires="+expirationDate.toGMTString());
           }