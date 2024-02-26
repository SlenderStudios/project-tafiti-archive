var videobar_pointer = 1;

function IconZoomIn(sender, args) {
	//alert(sender.Name);
	var myStoryboard = sender.findName("ZoomIcon");
    var IconZoomIn_X = sender.findName("IconZoomIn_X");
	var IconZoomIn_Y = sender.findName("IconZoomIn_Y");
	var IconHide = sender.findName("IconHide");
	var TextShow = sender.findName("TextShow");
	var TextZoomIn_X = sender.findName("TextZoomIn_X");
	var TextZoomIn_Y = sender.findName("TextZoomIn_Y");
	
	myStoryboard.stop();
	IconZoomIn_X["Storyboard.TargetName"] = IconZoomIn_Y["Storyboard.TargetName"] = sender.Name;
	IconHide["Storyboard.TargetName"] = sender.Name + "_Icon";
	TextShow["Storyboard.TargetName"] = TextZoomIn_X["Storyboard.TargetName"] = TextZoomIn_Y["Storyboard.TargetName"] = sender.Name + "_Text";
	myStoryboard.begin();
}

function IconZoomOut(sender, args) {
	//alert(sender.Name);
	var myStoryboard = sender.findName("ZoomOutIcon");
    var IconZoomOut_X = sender.findName("IconZoomOut_X");
	var IconZoomOut_Y = sender.findName("IconZoomOut_Y");
	var IconShow = sender.findName("IconShow");
	var TextHide = sender.findName("TextHide");
	var TextZoomOut_X = sender.findName("TextZoomOut_X");
	var TextZoomOut_Y = sender.findName("TextZoomOut_Y");
	
	myStoryboard.stop();
	IconZoomOut_X["Storyboard.TargetName"] = IconZoomOut_Y["Storyboard.TargetName"] = sender.Name;
	IconShow["Storyboard.TargetName"] = sender.Name + "_Icon";
	TextHide["Storyboard.TargetName"] = TextZoomOut_X["Storyboard.TargetName"] = TextZoomOut_Y["Storyboard.TargetName"] = sender.Name + "_Text";
	myStoryboard.begin();
}

function IconClick(sender, args) {
	//alert(sender.tag);
	newWindow = window.open(sender.Tag, 'pop_win','height=500, width=700, location=0, menubar=0, resizable=0, scrollbars=0, status=0, titlebar=0, toolbar=0');
}

function getStatusIcon(sender, args) {
	alert(src="http://messenger.services.live.com/users/" + sender.Tag + "/presence/?cb=addStatusIcon");
}

function addStatusIcon(presence) {
	alert(presence.statusText);
}

function gotoSilverlight(sender, args) {
	window.open("http://www.silverlight.net");
}
//Patrick added fun. for flash layout
function IconClick_join(sender, args) {
	//alert(sender.tag);
	newWindow = window.open(sender.Tag, 'pop_win','height=550, width=750, location=0, menubar=0, resizable=0, scrollbars=1, status=0, titlebar=0, toolbar=0');
}
function IconClick_alert(sender, args) {
	//alert(sender.tag);
	newWindow = window.open(sender.Tag, 'pop_win','height=600, width=800, location=0, menubar=0, resizable=0, scrollbars=1, status=0, titlebar=0, toolbar=0');
}
function SwapOut(btn) {
document[btn].src = "images/home/"+btn+"_1.gif";
} 
function SwapBack(btn) {
document[btn].src = "images/home/"+btn+"_0.gif";

} 
