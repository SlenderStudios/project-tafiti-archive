<%@ Page Language="C#" CodeFile="Default.aspx.cs" Inherits="_Default" AutoEventWireup="true" %>
<%@ Import Namespace="WLQuickApps.Tafiti.WebSite" %>
<%@ Import Namespace="WLQuickApps.Tafiti.Business" %>
<%@ Import Namespace="Microsoft.Security.Application" %>
<%@ Import Namespace="System.Configuration" %>

<html>
<head>
  <title>Tafiti Search Visualization</title>
  <script type="text/javascript" src="js/controls.js"></script>
  <script type="text/javascript" src="js/searchui.js"></script>
  <script type="text/javascript" src="js/browserDetect.js"></script>
  <script type="text/javascript" src="js/clientStorage.js"></script>
  <script type="text/javascript" src="js/search.js"></script>
  <script type="text/javascript" src="js/animationHack.js"></script>
  <script src="http://settings.messenger.live.com/api/1.0/messenger.js" type="text/javascript" language="javascript"></script>
  <style type="text/css">
    #firstExperience { position: absolute; width: 100%; text-align: center; visibility: hidden; z-index: 100;}
    #firstExperience p { color: white; font: 8pt Verdana; margin: 10; } 
    #firstExperienceControlHost { width: 210px; margin: 25px auto; }
    #signInFrame { position: absolute; left: 4px; bottom: 4px; width: 259px; height: 100px; z-index: 10000; border: 3px Solid #444; }
  </style>
</head>
<body style="margin: 0px; overflow: hidden; background: black;">

    <form id="Form1" runat="server">
            <asp:ScriptManager ID="_scriptManager" runat="server" EnablePageMethods="True" EnablePartialRendering="True" ScriptMode="Release" />
            
            <!-- This panel is only rendered if the user is logged in. -->
            <asp:Panel runat="server" ID="_loggedInUserPanel" Visible="false">
                <!-- Start messenger control contents -->
                <div id="signInFrame">
                </div>
                <!-- End messenger control contents -->

                <!-- Messenger Presence - Start -->
                <script type="text/javascript" language="javascript">
                     // Open the window.
                    function openPermissionScreen()
                    {
                        if (!messengerPresenceID)
                        {
                            var index = window.location.href.indexOf("/Default.aspx");
                            if (index < 0)
                            {
                                return;
                            }
                            
                            var returnUrl = window.location.href.substring(0, index) + "/ProcessMessengerConsent.aspx";
                            
                            var privacyUrl = "";
                            
                            // Check if there is a privacy override
                            if(privacyUrl == '')
                            {
                                // No privacy statement - go with /Privacy.aspx
                                privacyUrl = window.location.href.substring(0, index) + "/Privacy.aspx";
                            }
                            
                            var PermissionGrantingString = "";
                            
                            PermissionGrantingString = "http://settings.messenger.live.com/Applications/WebSignup.aspx?returnURL=" +
                                                       returnUrl + "&privacyURL=" + privacyUrl;

                            // If you send a # to the Consent screen it raises an error.
                            PermissionGrantingString = PermissionGrantingString.replace("#", '');
                            
                            window.open(PermissionGrantingString, "ConsentScreen", 'width=800,height=610,scrollbars=yes');
                        }
                        else
                        {
                            handleMessengerPermissionResponse(null);
                        }
                    }
                    
                    function handleMessengerPermissionResponse(theMessengerID)
                    {
                        messengerPresenceID = theMessengerID;
                        
                        if (messengerPresenceID)
                        {
                            presenceLink.textBlock.Text = "-presence";
                        }
                        else
                        {
                            presenceLink.textBlock.Text = "+presence";
                        }
                    }
                </script>
                <!-- Messenger Presence - End -->        
            </asp:Panel>
    </form>
    
    <%--<div style="behavior:url(#default#userdata)" ID="IeStorage"></div>--%> <!-- IE client-side storage -->
    
    <div id="firstExperience">
        <p>Welcome to</p>
        <img src="images/FirstUX_logo.png" alt="Tafiti" />
        <p id="firstExperience_install_silverlight" style="visibility: hidden;">You'll need to install Silverlight to experience Tafiti.com</p>
        <p id="firstExperience_safari3_not_supported" style="visibility: hidden;">We're sorry, but Tafiti doesn't support Safari 3 beta yet.<br/>Tafiti supports Internet Explorer 6/7, Firefox 1.5/2, or Safari 2.</p>
        <div id="firstExperienceControlHost"></div>
        <p id="firstExperience_restart" style="visibility: hidden;">Once installed, please restart your browser.</p>
        <p id="firstExperience_platform_not_supported" style="visibility: hidden;">Silverlight requires Windows or Mac OS X</p>
        <p><a href="faq.html">faq</a> | <a href="mailto:tafiti@microsoft.com">feedback</a> | <a href="legal.html">terms of use</a> | <a href="privacy.html">privacy</a></p>
    </div>  
    
    <script type="text/javascript" src="js/Silverlight.js"></script> <!-- load after asp:ScriptManager -->
    
    <script type="text/javascript">
    
    var ViewMode = { 
        InstallSilverlight: 0,  // link to install
        Minimal: 1,             // just the search card
        Standard: 2,            // everything
        Screensaver: 4 };       // fullscreen screensaver
        
    var wpfeVersion = '1.0.20716';
    var isWpfeInstalled = Silverlight.isInstalled(wpfeVersion);
    var wpfeHost;
    var topCanvas;
    var canvas;
    var backgroundImage;
    var viewMode;
    var searchCardStack;
    var resultsLayer; // parent to the resultsPanels and shelf
    var resultsPanels = [];
    var currentPanel = 0;
    var carousel;
    var shelf;
    var savedResultsPanel;
    var slotShowing;
    var offscreenAnimationHack;
    var clientStorage;
    var shelfFirstExperience;
    var shelfFirstExperienceCloseBtn;
    var userIsAuthenticated = <%= UserManager.IsUserLoggedIn ? "true" : "false" %>;
    var userEmail = <%= UserManager.IsUserLoggedIn ? AntiXss.JavaScriptEncode(UserManager.LoggedInUser.DisplayName) : AntiXss.JavaScriptEncode("") %>;
    var userSigninUrl = <%= AntiXss.JavaScriptEncode(string.Format("http://login.live.com/wlogin.srf?appid={0}", SettingsWrapper.LiveAuthID)) %>;
    var userSignoutUrl = <%= AntiXss.JavaScriptEncode(string.Format("http://login.live.com/logout.srf?appid={0}", SettingsWrapper.LiveAuthID)) %>;
    var messengerPresenceID <%= UserManager.IsUserLoggedIn ? "= " + AntiXss.JavaScriptEncode(UserManager.LoggedInUser.MessengerPresenceID) : "" %>;
    var signInSignOutLink;
    var viewedShelfCount = 0;
    var savedShelfCount = 0;
    var presenceLink;

    // screensaver    
    var stage;
    var screenSaverCanvas;
    var originalFps;
    var searchResultAggregator;
    
       
    function topXamlLoaded() {
        if (typeof(WLQuickApps) == "undefined")
        {
            setTimeout(topXamlLoaded, 100);
            return;
        }
        
        WLQuickApps.Tafiti.Scripting.EntryPoint.start();

        if (userIsAuthenticated)
        {
            WLQuickApps.Tafiti.Scripting.TafitiUserManager.set_loggedInUserID(<%= (UserManager.IsUserLoggedIn) ? AntiXss.JavaScriptEncode(UserManager.LoggedInUser.UserID) : "null" %>);
        }
        
        wpfeHost = document.getElementById("WpfeControl");
        SJ.initialize(wpfeHost);
        
        topCanvas = SJ.findElement("topCanvas");
        topCanvas.addEventListener("KeyDown", "onKeyDown");
        
        canvas = new SJ.Layer(0, 0);
        canvas.setParent(topCanvas);
        canvas.setAnimations([
                "<Storyboard x:Name='%fadeIn%'> \
                    <DoubleAnimation \
                         Storyboard.TargetName='%name%' \
                         Storyboard.TargetProperty='Opacity' \
                         From='0' To='1' Duration='0:0:0.5' /> \
                </Storyboard>" ]);

        backgroundImage = new SJ.Image(0, 0, "images/background.jpg", false, 'Fill');
        backgroundImage.setParent(canvas);
        
        var logo = new SJ.Image(-20, 25, "images/Tafiti_logo.png");
        logo.setParent(canvas);

        // links to sign in/out, faq, terms of use, ...
        var linkStack = new SJ.StackPanel(25, 85, false);
        linkStack.hAlign = "left";
        linkStack.setParent(canvas);
        signInSignOutLink = createHyperlink(linkStack, userIsAuthenticated ? "sign out" : "sign in", 35, userIsAuthenticated ? userSignoutUrl : userSigninUrl, true);
        if (!userIsAuthenticated) {
            signInSignOutLink.onMouseEnter = function(sender, args) { SJ_ShowToolTip(sender.visual, "Sign in with your Windows Live ID so you<LineBreak/>can access your stacks from multiple computers<LineBreak/>and connect with your Windows Live contacts.", args.getPosition(SJ.topCanvas)); }
            signInSignOutLink.onMouseLeave = function() { SJ_HideToolTip(); }
        }
        
        if (userIsAuthenticated)
        {
            var userLinkStack = new SJ.StackPanel(25, 95, false);
            userLinkStack.hAlign = "left";
            userLinkStack.setParent(canvas);
            presenceLink = createHyperlink(userLinkStack, (messengerPresenceID) ? "-presence" : "+presence", 55, "javascript:openPermissionScreen()", false);
            
            if (!messengerPresenceID)
            {
                presenceLink.onMouseEnter = function(sender, args) { SJ_ShowToolTip(sender.visual, "Share your Windows Messenger presence so<LineBreak/>that other users can contact you if they<LineBreak/>are not on your Windows Live contacts list.", args.getPosition(SJ.topCanvas)); }
                presenceLink.onMouseLeave = function() { SJ_HideToolTip(); }
            }
        }

        createHyperlink(linkStack, "faq", 15, "faq.html", true);
        createHyperlink(linkStack, "terms of use", 55, "legal.html", true);
        createHyperlink(linkStack, "privacy", 30, "privacy.html", true);
        createHyperlink(linkStack, "feedback", 30, "mailto:tafiti@microsoft.com", false);

        linkStack.updateLayout();
        
        offscreenAnimationHack = new OffscreenAnimationHack();
        offscreenAnimationHack.run();
        
        searchCardStack = new SJ.SearchCardStack(0, 100, searchClicked);
        searchCardStack.setParent(canvas);
        searchCardStack.onPopped = searchPopped;
        searchCardStack.animate("fadeIn");
        
        carousel = new SJ.CarouselPanel(3, 375, 260, 100);
        carousel.visual.Opacity = '0';
        carousel.setParent(canvas);
        
        clientStorage = SJ.ClientStorage.createInstance();

        // Put the shelf and resultsPanels in their own layer
        resultsLayer = new SJ.Layer(5000, 0); // offscreen
        resultsLayer.visual.Opacity = '0';
        resultsLayer.setParent(canvas);
        resultsLayer.setAnimations([
                "<Storyboard x:Name='%fadeIn%'> \
                    <DoubleAnimation \
                         Storyboard.TargetName='%name%' \
                         Storyboard.TargetProperty='Opacity' \
                         From='0' To='1' Duration='0:0:1' /> \
                </Storyboard>"]);
        
        // Shelf control is a bit smaller than its background picture because of the dropshadow
        shelf = new SJ.Shelf(823, 45, 180, 565);
        shelf.setParent(resultsLayer);
        shelf.onSlotClick = slotClicked;
        shelf.onFullScreen = onShelfSlotFullScreen;
        shelf.onShelfMouseEnter = hideShelfFirstExperience;
        shelf.visual.Opacity = '0';
        shelf.setTransform(
                "<TransformGroup> \
                    <TranslateTransform Name='%translate%' X='0' Y='0' /> \
                 </TransformGroup>");
        shelf.setAnimations([
                "<Storyboard x:Name='%slideOut%'> \
                    <DoubleAnimation \
                         Storyboard.TargetName='%translate%' \
                         Storyboard.TargetProperty='X' \
                         From='-250' To='0' Duration='0:0:1' /> \
                </Storyboard>"]);
        setTimeout(loadShelf, 500);

        currentPanel = 0;
        var domains = ["web", "images", "news", "feeds", "phonebook"];
        for (var i = 0; i < domains.length; i++) {
            resultsPanels[i] = new SJ.DomainResultsView(270, 12, 553, 625, domains[i]);
            resultsPanels[i].setParent(resultsLayer);
            resultsPanels[i].header.onWordWheelFilterChanged = onWordWheelFilterChanged;
            resultsPanels[i].header.onFullScreenClicked = onFullScreenDomainResultsClicked;
            if (i != currentPanel)
                resultsPanels[i].situate("outBack");
            var imageName = domains[i].substr(0,1).toUpperCase() + domains[i].substr(1); // uppercase first letter
            carousel.addImage("images/caro_" + domains[i] + ".png", 55, 52, imageName);
        }
        
        carousel.onSelectionChanged = function(sender, eventArgs) {
            shelf.hideTextEdits();
            var panel = resultsPanels[eventArgs.selection];
            if (sender.target == sender.selection && panel.queryText) {
                panel.onQueryComplete = decrementQueryCount;
                panel.beginQuery(panel.queryText, numRequestedResults, resultsPerChunk);
                track("scope-changed/" + panel.domain);
            }
            // To get the hiding to actually happen we have to let the event loop run once
            setTimeout(function () { animatePages(eventArgs.direction, eventArgs.selection); }, 0);
        };
        
        savedResultsPanel = new SJ.SavedResultsView(270, 12, 553, 625);
        savedResultsPanel.setParent(resultsLayer);
        savedResultsPanel.situate("outFront");
        savedResultsPanel.onSavedQueryNavigate = function(sender, eventArg) {
            searchCardStack.setQueryText(eventArg);
            searchClicked();
        }
        savedResultsPanel.onRemoveResult = function(sender, eventArgs) {
            if (slotShowing) {
                slotShowing.removeItem(eventArgs);
            }
        }
        savedResultsPanel.onCloseClicked = function(sender, eventArgs) {
            savedResultsPanel.animate("animateOutBack");
            slotShowing = null;
            shelf.selectSlot(null);
        }
        
        searchResultAggregator = new SJ.ResultAggregator();

        var slLogo = new SJ.Image(25, 550, "images/SilverlightLogo.png");
        slLogo.setParent(canvas);
        
        var liveSearchLogo = new SJ.Image(140, 550, "images/LiveSearchLogo.png");
        liveSearchLogo.setParent(canvas);

        var copyrightText = new SJ.TextBlock(80, 590, 150, 0, "&#169; 2007 Microsoft Corp.",
                                "FontFamily='Verdana' FontSize='9' Foreground='#8b9aad' Opacity='0.8'");
        copyrightText.setParent(canvas);

        if (window.addEventListener)
            window.addEventListener('DOMMouseScroll', onMouseWheelScrolled, false);
        window.onmousewheel = onMouseWheelScrolled;
        document.onmousewheel = onMouseWheelScrolled;

        windowResized();

        try {
            loadViewState(); 
        } catch (e) { }
    }
    
    function createHyperlink(parent, text, width, url, appendSeperator) {
        var link = new SJ.Hyperlink(0, 0, width, 0, text,
                                    "FontFamily='Verdana' FontSize='9' Foreground='#ffffff' Opacity='0.8'",
                                    url);
        link.setParent(parent);
        link.openWithName = "_self";
        link.vAlign = "bottom";
        link.margin = {right:5};
        link.doLayout();
        
        if (appendSeperator) {
            var sep = new SJ.TextBlock(0, 0, 5, 0, "|",
                                    "FontFamily='Verdana' FontSize='9' Foreground='#ffffff' Opacity='0.8'");
            sep.setParent(parent);
            sep.margin = {right:5};
        }
        
        return link;
    }
    
    function onFullScreenDomainResultsClicked() {
        var q = resultsPanels[currentPanel].queryText;
        searchResultAggregator.setQueries( [q] );
        changeViewMode(ViewMode.Screensaver); 
    }
    
    function onShelfSlotFullScreen(sender, args) {
        var queries = sender.getQueries();
        searchResultAggregator.setQueries( queries );
        changeViewMode(ViewMode.Screensaver);
    }
    
    function windowResized() {
        var wpfeDiv = document.getElementById("WpfeControlHost");
        if (wpfeDiv) {
            switch (viewMode) {
                case ViewMode.InstallSilverlight:
                    var firstExperience = document.getElementById("firstExperience");
                    firstExperience.style.top = (wpfeDiv.clientHeight / 2) - 100;
                    break;
                    
                case ViewMode.Minimal:
                    if (canvas) {
                        SJ.placeElement(searchCardStack.visual, wpfeDiv.clientWidth/2-165, wpfeDiv.clientHeight/2-130);
                    }
                    // fall thru!
                    
                case ViewMode.Standard:                                    
                    if (canvas) {
                        var newWidth  = Math.max(wpfeDiv.clientWidth, 975);
                        var newHeight = Math.max(wpfeDiv.clientHeight, 605);
                        for (var i = 0; i < resultsPanels.length; i++) {
                            resultsPanels[i].resize(newWidth-471, newHeight-30);
                        }
                        savedResultsPanel.resize(newWidth-471, newHeight-30);
                        shelf.move(newWidth-201, (newHeight - shelf.getHeight())/2);
                        backgroundImage.resize(newWidth,newHeight);
                    }
                    break;

                case ViewMode.Screensaver:
                    if (stage != null) {
                        resizeStage();
                    }
                    break;   
            }
        }
    }
    
    var animationSequence;
    
    function changeViewMode(newViewMode, onCompleted) {
        if (viewMode == ViewMode.Minimal && newViewMode == ViewMode.Standard) {
            animationSequence = new SJ.Sequencer();
            animationSequence.add( function() {
                    searchCardStack.animateMove(0, 100, animationSequence.invoker);
                } );
            animationSequence.add( function() {
                    carousel.unveil(animationSequence.invoker);
                } );
            animationSequence.add( function() {
                    SJ.placeElement(resultsLayer.visual, 0, 0);
                    resultsLayer.animate("fadeIn", animationSequence.invoker);
                } );
            animationSequence.add( function() {
                    shelf.visual.Opacity = '1';
                    shelf.animate("slideOut", animationSequence.invoker);
                } );            
            if (onCompleted)
                animationSequence.add(onCompleted);
            animationSequence.run();
        }
        else if (viewMode == ViewMode.Standard && newViewMode == ViewMode.Screensaver) {
            canvas.visual.Opacity = 0;
            canvas.visual.Visibility = "Collapsed";
            SJ.wpfeHost.content.onFullScreenChange = onFullScreenModeChange;
            startScreenSaver();
        }
        else if (viewMode == ViewMode.Screensaver && newViewMode == ViewMode.Standard) {
            if (SJ.wpfeHost.content.Fullscreen)
                SJ.wpfeHost.content.Fullscreen = false;
            SJ.wpfeHost.settings.maxFrameRate = 64;
            canvas.visual.Visibility = "Visible";
            canvas.animate("fadeIn");
        }
        viewMode = newViewMode;
    }
    
    function onFullScreenModeChange(sender, eventArgs) {
        if (SJ.wpfeHost.content.Fullscreen)
        {
            if(stage != null)
            {
                resizeStage();
            }
            else
            {
                startScreenSaver();
            }
        }
        else
        {
            stopScreenSaver();
            windowResized();
        }
    }

    function startScreenSaver() {            
        if (!stage) {
            var downloader = SJ.wpfeHost.createObject("downloader");
            downloader.addEventListener("completed", "onScreenSaverXamlDownloadCompleted");
            downloader.open("GET", "screensaver/xaml/SJStage.xaml");
            downloader.send();
        }
    }
    
    function stopScreenSaver() {
        if (stage)
            delete stage;
        stage = null;
        if (screenSaverCanvas)
            topCanvas.Children.Remove(screenSaverCanvas);
        changeViewMode(ViewMode.Standard);
    }

    function onScreenSaverXamlDownloadCompleted(sender, eventArgs) {
        var xamlFragment = sender.getResponseText("");
        screenSaverCanvas = SJ.wpfeHost.content.createFromXaml(xamlFragment);
        topCanvas.Children.add(screenSaverCanvas);
    
    	stage = new SLTree.Stage();
    	stage.Init(SJ.wpfeHost, null, screenSaverCanvas);
		stage.SetAddMoreResultsCallback(addMoreResults);
		resizeStage();
        addMoreResults();
	}

    function resizeStage() {
	    ScaleElement( { w: SJ.wpfeHost.content.actualWidth,
					    h: SJ.wpfeHost.content.actualHeight,
					    control: stage.control,
					    element: stage.root,
					    fMaintainAspectRatio:false } );
    }

    function addMoreResults() {
        searchResultAggregator.getMoreResults(onMoreResultsCompleted);
    }
    
    function onMoreResultsCompleted(sender, results) {
        // convert result objects to match what the screensaver expects
        var resultsToShow = [];
        for (var i = 0; i < results.length; i++) {
            var result = results[i];
            if (result.title && result.desc && result.url) {
                result.name = result.title;
                result.info = result.desc;
                result.link = result.url;
                result.weight = 1;
                resultsToShow.push(result);
            }
        }
        if (stage)
            stage.UpdateItems(resultsToShow);
    }
    
    window.onresize = windowResized;
    
    function animatePages(direction, selection) {
        if (slotShowing) {
            savedResultsPanel.animate("animateOutFront");
            slotShowing = null;
            shelf.selectSlot(null);
        }
       
        if (direction == "cw") {
            resultsPanels[currentPanel].visual["Canvas.ZIndex"] = 0;
            resultsPanels[selection].visual["Canvas.ZIndex"] = 1;
            resultsPanels[currentPanel].animate('animateOutBack');
            resultsPanels[selection].animate('animateInFront', animatePagesDone);
        }
        else {
            resultsPanels[currentPanel].visual["Canvas.ZIndex"] = 1;
            resultsPanels[selection].visual["Canvas.ZIndex"] = 0;
            resultsPanels[currentPanel].animate('animateOutFront');
            resultsPanels[selection].animate('animateInBack', animatePagesDone);
        }

        currentPanel = selection;
        saveViewState();
    }
    
    function animatePagesDone() {
        shelf.showTextEdits();
        resultsPanels[currentPanel].visual["Canvas.ZIndex"] = 0;        
    }
    
    var numRequestedResults = 30;
    var resultsPerChunk = 30;
    var lastQueryText;
    
    function searchClicked() {
        var queryText = searchCardStack.getQueryText();
        if (lastQueryText && lastQueryText != queryText)
            searchCardStack.push(lastQueryText, function () { searchClickedCore(queryText); } );
        else
            searchClickedCore(queryText);
    }
    
    function searchPopped() {
        var queryText = searchCardStack.getQueryText();
        searchClickedCore(queryText);    
        lastQueryText = queryText;
    }
    
    function moreResultsClicked(sender, eventArgs) {
        var resultsPanel = resultsPanels[sender.resultsPanel];
        resultsPanel.resultsList.removeItem(sender); // more link
        
        searchCardStack.setSearchEnabled(false);
        
        var itemCount = resultsPanel.results.length;
        resultsPanel.onQueryComplete = decrementQueryCount;
        resultsPanel.beginQuery(lastQueryText, itemCount+resultsPerChunk, resultsPerChunk, itemCount);
    }
    
    function decrementQueryCount(resultsPanel, succeeded) {
        resultsPanel.onQueryComplete = null;
        searchCardStack.setSearchEnabled(true);
        if (succeeded) {
            track("search/" + resultsPanel.domain);
        }
        else {
            var errorMsg1 = new SJ.TextBlock(0, 0, 100, 0, "Oops! We're sorry, but we encountered an error.", "FontSize='11' Foreground='#0067a6'");
            errorMsg1.hAlign = "center";
            errorMsg1.margin = {top: 10};
            var errorMsg2 = new SJ.TextBlock(0, 0, 100, 0, "Please try your search again.", "FontSize='11' Foreground='#0067a6'");
            errorMsg2.hAlign = "center";
            errorMsg2.margin = {top: 10};
            resultsPanel.resultsList.addItem(errorMsg1);
            resultsPanel.resultsList.addItem(errorMsg2);
        }
        checkAddMoreResultsLink(resultsPanel);
    }
    
    function checkAddMoreResultsLink(resultsPanel) {    
        if (resultsPanel.totalResults && 
            resultsPanel.totalResults > 0 &&
            resultsPanel.results.length > 0 &&
            resultsPanel.totalResults > resultsPanel.results.length) 
        {
            var moreLink = new SJ.Hyperlink(0, 0, 100, 0, "More...", "FontSize='11' Foreground='#0067a6'", "Get more search results");
            moreLink.onNavigate = moreResultsClicked;
            moreLink.hAlign = "center";
            moreLink.margin = {top: 10, bottom: 10};
            moreLink.resultsPanel = SJ.findFirst(resultsPanels, resultsPanel); // avoid circular references
            resultsPanel.resultsList.addItem(moreLink);
        }
    }

    function searchClickedCore(queryText) {
        searchCardStack.setSearchEnabled(false);
        lastQueryText = queryText;

        resetWordWheelFilter();

        var panel = resultsPanels[currentPanel];
        if (viewMode != ViewMode.Standard)
            changeViewMode(ViewMode.Standard);
            
        if (panel.domain == "phonebook") {
            carousel.target = 0;
            carousel.turnTowardTarget();
            panel = resultsPanels[0];
        }

        for (var i = 0; i < resultsPanels.length; i++)
            resultsPanels[i].reset(queryText);
        
        panel.onQueryComplete = decrementQueryCount;
        panel.beginQuery(queryText, numRequestedResults, resultsPerChunk);
        
        if (slotShowing) {
            savedResultsPanel.animate("animateOutFront");
            slotShowing = null;
            shelf.selectSlot(null);
        }
        
        saveViewState('queryText',queryText);
    }
    
    function slotClicked(sender, eventArgs) {
        shelf.hideTextEdits();
        // To get the hiding to actually happen we have to let the event loop run once
        setTimeout(function () {
            if (sender != slotShowing) {
                slotShowing = sender;
                shelf.selectSlot(slotShowing);
                var results = eventArgs.results;
                savedResultsPanel.animate('animateInFront', SJ.methodCaller(shelf, "showTextEdits"));
                savedResultsPanel.setResults(WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(slotShowing.shelfStackID));
                trackShelfView();
            }
        }, 0);
    }
    
    function showShelfFirstExperience() {
        shelfFirstExperience = new SJ.Image(15, 20, "images/1stUXShelf.png");
        shelfFirstExperience.setParent(shelf);
        shelfFirstExperienceCloseBtn = new SJ.Button(155, 30, 11, 11, "", 
                {idle: "images/1stUXShelf_x.png", hover: "images/1stUXShelf_x_hover.png",
                 activeDown: "images/1stUXShelf_x.png", activeUp: "images/1stUXShelf_x_hover.png"});
        shelfFirstExperienceCloseBtn.onClick = hideShelfFirstExperienceClicked;
        shelfFirstExperienceCloseBtn.setParent(shelf);
    }

    function hideShelfFirstExperienceClicked() {
        if (shelfFirstExperience) {
            shelfFirstExperience.setParent(null);
            shelfFirstExperienceCloseBtn.setParent(null);
        }
    }
    
    function hideShelfFirstExperience() {
        if (shelfFirstExperience) {
            shelfFirstExperience.setParent(null);
            shelfFirstExperienceCloseBtn.setParent(null);
        }
    }

    function loadShelf() {
        shelf.enabled = false;
        
        if (userIsAuthenticated)
            shelf.load(onShelfLoaded);
        else
            readyShelf();
    }
    
    function onShelfLoaded(sender, eventArgs) {
        window.onbeforeunload = confirmExit;
        readyShelf();
    }

    function readyShelf() {
        shelf.enabled = true;
        shelf.statusText.setText('');
//        if (shelf.isEmpty())
//            showShelfFirstExperience();
//        else
            changeViewMode(ViewMode.Standard);        
    }
    
    function confirmExit()
    {
        if (!shelf.enabled) {
            shelf.statusText.setText('Saving...');            
            return "Your saved search results are being saved. Lose your changes?";
        }
    }
    
    SJ.onOpenWindowFailed = function() {
        var contents = new SJ.StackPanel(0, 0, true);
        contents.visual.Background = "#ffffff";

        var message = new SJ.TextBlock(0, 0, 350, 0, "Tafiti is trying to show the site you chose. Please <LineBreak/>disable your popup blocker to see your selection.", "FontSize='14'");
        message.margin = {top: 50, bottom: 50, left: 64, right: 64};   
        message.hAlign = "center"; 
        message.setParent(contents);
        
        var popupBlockedDialog = new SJ.DialogBox(400, 100);
        popupBlockedDialog.setContent(contents);
        popupBlockedDialog.center();
        popupBlockedDialog.updateLayout();
        popupBlockedDialog.show();
        
        setTimeout(function() {
                popupBlockedDialog.close();
                popupBlockedDialog = null;        
            },
            9 * 1000);
    }
    
    SJ.onOpenWindowSucceeded = function() {
        if (!slotShowing) {
            var panel = resultsPanels[currentPanel];
            trackSearchResultClick(panel.domain);
        }
    }
    
    function onWordWheelFilterChanged(sender, args) {
        for (var i = 0; i < resultsPanels.length; i++) {
            var resultsPanel = resultsPanels[i];
            resultsPanel.setFilter(args);
            resultsPanel.header.setWordWheelFilter(args);
            if (args == "")
                checkAddMoreResultsLink(resultsPanel);
        }
    }
    
    function resetWordWheelFilter() {
        for (var i = 0; i < resultsPanels.length; i++) {
            resultsPanels[i].header.resetWordWheel();
            resultsPanels[i].header.hideWordWheel();
        }    
    }
    
    function getActivePanel() {
        if (slotShowing)
            return savedResultsPanel;
        else
            return resultsPanels[currentPanel];
    }

    function onKeyDown(sender, args) {
        if (args.Key == 15) {
            getActivePanel().lineDown();
        }
        else if (args.Key == 17) {
            getActivePanel().lineUp();
        }
        else if (args.Key == 9 || args.Key == 11) { // space || page up
            getActivePanel().pageUp();
        }
        else if (args.Key == 10) {
            getActivePanel().pageDown();
        }
    }

    function onMouseWheelScrolled(e) {
        var delta = 0;
        if (!e) var e = window.event;
        if (e.wheelDelta) {
            delta = event.wheelDelta / 120;
            if (window.opera)
              delta = -delta;
        }
        else if (e.detail) {
            delta = -e.detail / 3;
        }
        
        if (delta) {
            var panel = getActivePanel();
            var count = Math.min(50,Math.ceil(Math.abs(delta)));
            while (count--) {
                if (delta > 0)
                    panel.lineDown();
                else
                    panel.lineUp();
            }
        }
    }
    
    // Update view state based on the document.location query and hash args.
    function loadViewState() {
        var args = {};
        args = crackUrlArgs(args, document.location.search.substr(1));
        args = crackUrlArgs(args, document.location.hash.substr(1));
        if (args.sid) {
            changeViewMode(ViewMode.Standard);
            SJ.AsyncRequest("GET", "Snapshot/?sid=" + encodeURIComponent(args.sid), null, onLoadSnapshotResult);
        }
        if (args.q && args.q.trim()) {
            searchCardStack.setQueryText( SJ.xmlEscape(args.q.trim()) );
        }
        if (args.p) {
            carousel.target = parseInt(args.p);
            carousel.turnTowardTarget();
        }
        if (args.s) {
            searchClicked();
        }
    }
     
    function saveViewState() {
        var state;
        state = "p=" + currentPanel;

        var text = searchCardStack.getQueryText();
        if (text)
            state += "&q=" + encodeURIComponent(text);

        document.location.hash = state;

        // update LiveID signin link
        if (!userIsAuthenticated) {
            var returnUrl = "http://" + document.location.host + document.location.pathname + "?" + state + "&s=1";
            userSigninUrl = setQueryParameter(userSigninUrl, "wreply", returnUrl);
            signInSignOutLink.url = userSigninUrl;
        }
    }
    
    function onLoadSnapshotResult(result) {
        if (result.succeeded) {        
            var args = {};
            args = crackUrlArgs(args, document.location.search.substr(1));
            args = crackUrlArgs(args, document.location.hash.substr(1));
            if (args.sid) {
                var shelfStack = WLQuickApps.Tafiti.Scripting.ShelfStackManager.getShelfStack(args.sid)
                slotShowing = shelf.slots[0];
                shelf.selectSlot(slotShowing);
                savedResultsPanel.animate('animateInFront');
                savedResultsPanel.setResults(shelfStack, WLQuickApps.Tafiti.Scripting.ShelfStackManager.get_myShelfStacks());
            }
        }
    }

    function crackUrlArgs(args,s) {
        s = s.split('&');
        for (var i = 0; i < s.length; i++) {
            var pair = s[i].split('=',2);
            var key = pair[0];
            var value = decodeURIComponent(pair[1]);
            args[key] = value;
        }
        return args;
    }
    
    function assembleUrlArgs(args) {
        var s = "";
        for (var k in args)
            s += "&" + k + "=" + encodeURIComponent(args[k]);
        return s.substr(1);
    }
    
    function setQueryParameter(url,key,value) {
        var s = encodeURIComponent(key) + "=" + encodeURIComponent(value);
        var re = new RegExp("(\\?|&)"+key+"=[^&$]+");
        var match = url.search(re);
        if (match != -1)
            return  url.replace(re, url.substr(match,1) + s);
        else
            return url + "&" + s;
    }
    
    function trackSearchResultClick(scope) {
        track('resultclicked/' + scope);
    }
    
    function trackShelfView() {
        track('shelf/view/' + String(++viewedShelfCount));
    }
    
    function trackShelfSave() {
        track('shelf/save/' + String(++savedShelfCount));        
    }
    
    function track(url) {
    // Won't actually do anything for now.
//        var el = new Image();
//        el.src = "/track/" + url;
    }
    
    </script>
    
    <script type="text/xaml" id="xamlContent"><?xml version="1.0"?>
        <Canvas
          xmlns="http://schemas.microsoft.com/client/2007"
          xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
          Loaded="topXamlLoaded"
          MouseMove="SJ_ensureMouseCapture"
          x:Name="topCanvas">
        </Canvas>
    </script>
    
    <div id="WpfeControlHost" style="position: absolute; top: 0; left: 0; width: 100%; height: 100%;"></div>
    
    <script type="text/javascript">
        function startTafiti() {
            var parentElement = document.getElementById("WpfeControlHost");
            Silverlight.createObject(
                "#xamlContent",                     // Source property value.
                parentElement,                      // DOM reference to hosting DIV tag.
                "WpfeControl",                      // Unique control id value.
                {                                   // Control properties.
                    width:'100%',                   // Width of rectangular region of control in pixels.
                    height:'100%',                  // Height of rectangular region of control in pixels.
                    inplaceInstallPrompt:false,     // Determines whether to display in-place install prompt if invalid version detected.
                    background:'black',             // Background color of control.
                    isWindowless:'true',            // Determines whether to display control in Windowless mode.
                    framerate:'64',                 // MaxFrameRate property value.
                    version: wpfeVersion            // Control version to use.
                },
                {
                    onError:onSilverlightError,     // OnError property value -- event handler function name.
                    onLoad:null                     // OnLoad property value -- event handler function name.
                },
                null);                              // Context value -- event handler function name.
        }
        
        function onSilverlightError(sender, errorArgs) {
            // The error message to display.
            var errorMsg = "Silverlight Error: \n\n";
            
            // Error information common to all errors.
            errorMsg += "Error Type:    " + errorArgs.errorType + "\n";
            errorMsg += "Error Message: " + errorArgs.errorMessage + "\n";
            errorMsg += "Error Code:    " + errorArgs.errorCode + "\n";
            
            // Determine the type of error and add specific error information.
            switch(errorArgs.errorType)
            {
                case "RuntimeError":
                    // Display properties specific to RuntimeErrorEventArgs.
                    if (errorArgs.lineNumber != 0)
                    {
                        errorMsg += "Line: " + errorArgs.lineNumber + "\n";
                        errorMsg += "Position: " +  errorArgs.charPosition + "\n";
                    }
                    errorMsg += "MethodName: " + errorArgs.methodName + "\n";
                    break;
                case "ParserError":
                    // Display properties specific to ParserErrorEventArgs.
                    errorMsg += "Xaml File:      " + errorArgs.xamlFile      + "\n";
                    errorMsg += "Xml Element:    " + errorArgs.xmlElement    + "\n";
                    errorMsg += "Xml Attribute:  " + errorArgs.xmlAttribute  + "\n";
                    errorMsg += "Line:           " + errorArgs.lineNumber    + "\n";
                    errorMsg += "Position:       " + errorArgs.charPosition  + "\n";
                    break;
                default:
                    break;
            }
            SJ.log(errorMsg);
        }
        
        function showSilverlightInstallUx() {
            var parentElement = document.getElementById("firstExperienceControlHost");
            Silverlight.createObject(
                "",
                parentElement,
                "WpfeControl",
                {
                    width:'200',
                    height:'200',
                    inplaceInstallPrompt:true,
                    background:'black',
                    isWindowless:'false',
                    version: wpfeVersion
                },
                {
                    onError:onSilverlightError,
                    onLoad:null
                },
                null);
            
            for (var i = 0; i < document.images.length; i++) {
                if (document.images[i].src == "http://go.microsoft.com/fwlink/?LinkID=92802") {
                    var prevOnClick = document.images[i].onclick;
                    document.images[i].onclick = function() {
                        track("SilverlightDownload/" + BrowserDetect.browser + "-" + BrowserDetect.OS);
                        if (prevOnClick)
                            prevOnClick();
                    }
                }
            }
        }

        function showFirstUx() {
            if (BrowserDetect.OS != "Windows" && BrowserDetect.OS != "Mac" && BrowserDetect.OS != "Linux") {
                document.getElementById("firstExperience_platform_not_supported").style.visibility = 'visible';
            }
            else if (BrowserDetect.browser == "Safari" && BrowserDetect.version >= 522) {
                document.getElementById("firstExperience_safari3_not_supported").style.visibility = 'visible';
            }
            else if (!isWpfeInstalled && BrowserDetect.OS != "Linux") {
                if (BrowserDetect.browser == "Firefox" || BrowserDetect.browser == "Safari")
                    document.getElementById("firstExperience_restart").style.visibility = 'visible';

                document.getElementById("firstExperience_install_silverlight").style.visibility = 'visible';
                showSilverlightInstallUx();

                // poll for successful WPF/E install
                wpfeInstallTimer = setTimeout("wpfeInstallCheck()", 1000);
            }
            else {
                return false;
            }
            
            viewMode = ViewMode.InstallSilverlight;
            
            // dynamically load background image
            var parentElement = document.getElementById("WpfeControlHost");
            var bg = document.createElement('img');
            bg.src = "images/background.jpg";
            bg.style.width = "100%";
            bg.style.height = "100%"; 
            parentElement.appendChild(bg);
            
            // position our div and show it
            windowResized();
            document.getElementById('firstExperience').style.visibility = 'visible';
            
            return true;
        }
        
        var wpfeInstallTimer;
        
        function wpfeInstallCheck() {
            isWpfeInstalled = Silverlight.isInstalled(wpfeVersion);
            if (isWpfeInstalled) {
                document.getElementById('firstExperience').style.visibility = 'hidden';
                viewMode = ViewMode.Minimal;
                startTafiti();
            }
            else {
                wpfeInstallTimer = setTimeout("wpfeInstallCheck()", 1000);
            }
        }
        
        if (!showFirstUx()) {
            viewMode = ViewMode.Minimal;
            startTafiti();
        }

    </script>

    <script type="text/javascript" src="screensaver/js/Animations.js"></script>  
    <script type="text/javascript" src="screensaver/js/Anims/anim_color.js"></script>  
    <script type="text/javascript" src="screensaver/js/Anims/anim_generic.js"></script>  
    <script type="text/javascript" src="screensaver/js/Fragments.js"></script>  
    <script type="text/javascript" src="screensaver/js/Globals.js"></script>  
    <script type="text/javascript" src="screensaver/js/Projector.js"></script>  
    <script type="text/javascript" src="screensaver/js/PMatrix.js"></script>  
    <script type="text/javascript" src="screensaver/js/Vertex.js"></script>  
    <script type="text/javascript" src="screensaver/js/Stage.js"></script>  
    <script type="text/javascript" src="screensaver/js/Stage_utils.js"></script>  
    <script type="text/javascript" src="screensaver/js/Branch.js"></script>  
    <script type="text/javascript" src="screensaver/js/LeafNode.js"></script>  
    <script type="text/javascript" src="screensaver/js/utils.js"></script>  
    <script type="text/javascript" src="screensaver/js/Main.js"></script>  

    <ss:Scriptlet runat="server" ID="_messengerScript" EnableDebugging="true" PrecompiledScriptlet="WLQuickApps.Tafiti.Scripting.EntryPoint">
        <References>
            <ss:AssemblyReference Name="ssfx.Core" />
            <ss:AssemblyReference Name="ssfx.UI.Forms" />
            <ss:AssemblyReference Name="WLQuickApps.Tafiti.Scripting" />
        </References>
        
    </ss:Scriptlet>
 
</body>
</html>
