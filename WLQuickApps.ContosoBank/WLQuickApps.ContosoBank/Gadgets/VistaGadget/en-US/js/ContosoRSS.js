//varibles - common to Gadget and Settings
var cFeedNotAvailable = "Feed is not available";
var maxDockedWidth      = 132;
var maxDockedHeight     = 116;
var maxUndockedWidth    = 178;
var maxUndockedHeight   = 180;

var maxDockedWidthContent      = 110;
var maxDockedHeightContent     = 75;
var maxUndockedWidthContent    = 155;
var maxUndockedHeightContent   = 130;

var gSecondsToShow      = 3600;
var gfeedUrl            = "http://contosobnk.com/LatestForumRSS.ashx";
var timer;
var gSettingsLoaded = false;
var feedItems = [];
var bGetFeed = true;

//////////////MAIN GADGET////////////////

//Fires when the Setting screen closes, very important as this is where we sync any setting changes.
function onSettingsReturn() {
    GetSettings();
    //refresh data and timer
    bGetFeed = true;
    loadData();
}

//called on gadget load
function onLoadMain() {
    //setup the UI
    updateUI();
    //setup setting screen
    System.Gadget.settingsUI = "settings.html";
    //attach gadget events
    System.Gadget.onUndock = updateUI;
    System.Gadget.onDock = updateUI;
    System.Gadget.visibilityChanged = onCheckVisibility;
    System.Gadget.onSettingsClosed = onSettingsReturn;

    // get settings
    GetSettings();
    
    //delay start slightly
    startTimer(.25, "loadData()");
}

//start showing Location data after load
function loadData() {
    if (bGetFeed) {
        // get the current feeds
        getFeed();    
        bGetFeed = false;
        //set timeout
        startTimer(gSecondsToShow, "onFeedTimerPing()");      
    }
}

//Timer ping event, if the gadget is visible we will refresh the feed
function onFeedTimerPing() {
    bGetFeed = true;
    onCheckVisibility();
}

//stop the gadget if not visible to save bandwidth / cpu, start again on visible
function onCheckVisibility() {
    isVisible = System.Gadget.visible;
    if(isVisible) {
        loadData();
    }
}

// load RSS feed
function getFeed() {
	var xdoc = new ActiveXObject("Microsoft.XMLDOM");
	xdoc.onreadystatechange = function () {
		if ( xdoc.readyState == 4 ) {
			onXmlReturnHandler(xdoc);
		}
	};
	xdoc.load(gfeedUrl);
}

// called after the RSS file is loaded
function onXmlReturnHandler(xdoc) {

	var n, node, nodes, item;
	feedItems = [];
	try {
		nodes = xdoc.selectNodes("/rss/channel/item");
		for (n=0; n < nodes.length; n++){
			node = nodes[n];
			item = {};
			item.title = node.selectSingleNode('title').text;
			item.link = node.selectSingleNode('link').text;
			item.description = node.selectSingleNode('description').text;
			feedItems.push(item);
		}
	} catch(ex) {}
	nodes = item = xdoc = null;
	//Show items
	showFeedItems();
}

//Show the correct number of feed items based on the gadget mode
function showFeedItems() {
    var feedtext = cFeedNotAvailable;
    if (feedItems) {
        if (feedItems[0]) {
            feedtext = CreateFeedItemString(feedItems[0]);
	        if(!System.Gadget.docked) {
    	        feedtext += "<hr />" + CreateFeedItemString(feedItems[1]);
	        }
	    }
	}
	Feeds.innerHTML = feedtext;
}

//Create a item as HTML
function CreateFeedItemString(item) {
    var returnstring = "";
    returnstring += "<a href='" + item.link + "'>" + LimitChar(item.title, 35) + "</a>";
    returnstring += "<br />";
    returnstring += LimitChar(item.description, 80);
    return returnstring;
}

//Truncate a string if too long and add '...'
function LimitChar(string, length) {
    if (string.length > length) {
        return string.substring(0, length) + "...";
    }
    return string;
}

//setup the UI
function updateUI() {
    if(!System.Gadget.docked) {
        undockedState();
    } 
    else if (System.Gadget.docked) {
        dockedState(); 
    }
    showFeedItems();
}

//Setup the UI when undocked
function undockedState() {
    with(document.body.style)
        width=maxUndockedWidth, 
        height=maxUndockedHeight;

    with(Bg.style)
        width=maxUndockedWidth, 
        height=maxUndockedHeight;
    Bg.src="url(images/on_desktop/GadgetVistaUnDocked.png)";

    with(Feeds.style)
        width=maxUndockedWidthContent, height=maxUndockedHeightContent;
}

//setup the UI when Docked
function dockedState() {   
    with(document.body.style)
        width=maxDockedWidth,
        height=maxDockedHeight;

    with(Bg.style)
        width=maxDockedWidth,
        height=maxDockedHeight;
    Bg.src="url(images/in_sidebar/GadgetVistaDocked.png)";

    with(Feeds.style)
        width=maxDockedWidthContent, height=maxDockedHeightContent;           
}

//Helper to keep single timer
function startTimer(time, timeFunction) {
    clearInterval(timer);
    timer = null;
    if(time >= 0) {
        timer = setInterval(timeFunction, parseInt(time + 1) * 1000);
    }
}

/////////////SETTINGS SCREEN//////////////////
//Note: completly different context to main - no varibles are shared! No access to Main Gadget form.
//We use the same js file so as to share common varibles and keep things simple
//If this is confusing move into seperate files and duplicate main varibles defaults.

//called on setting load
function onLoadSettingsScreen() {
    if (!gSettingsLoaded) {
        GetSettings();
        FeedURL.value = gfeedUrl;
        System.Gadget.onSettingsClosing = onSettingsClosing;
        gSettingsLoaded = true;
    }
                     
}

//called when settings is closed - save if action commit
function onSettingsClosing(event) {
    if (event.closeAction == event.Action.commit) {
        gfeedUrl = FeedURL.value;
        SetSettings();

    }
    event.cancel = false;
}


//////////////COMMON FUNCTIONS///////////////
//We communicate settings between the Main Gadget and the settings form through common System.Gadget.Settings object.

//retreive settings
function GetSettings() {
    if (System.Gadget.Settings.read("FeedUrl")!="") {
        gfeedUrl = System.Gadget.Settings.read("FeedUrl");
    }    
}

//save current settings
function SetSettings() {
    System.Gadget.Settings.write("FeedUrl", gfeedUrl);
}
