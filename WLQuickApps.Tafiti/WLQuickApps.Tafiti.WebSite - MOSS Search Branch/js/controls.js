// Copyright 2007 Microsoft Corp. All Rights Reserved.

var SJ = {};    // "namespace" for our stuff

SJ.initLogging = function () {
    // Use Firebug logging if present
    // On Safari 2.x, don't use the buggy console.log()
    var isSafari2 = BrowserDetect.browser == "Safari" && navigator.appVersion && navigator.appVersion.indexOf("5.0") == 0;
    if (!isSafari2 && typeof(console) != "undefined") {
        SJ.log = function () {
            // If only there were a console.log() that didn't treat the first
            // arg as a format string...
            var pattern = new Array(arguments.length);
            var args = new Array(arguments.length);
            for (var i = 0; i < arguments.length; i++) {
                if (typeof(arguments[i]) == "string")
                    pattern[i] = "%s ";
                else
                    pattern[i] = "%o ";
                args[i] = arguments[i];
            }
            console.log.apply(console, [pattern.join("")].concat(args));
        };
    }
    else {
        SJ.logDiv = document.getElementById("LogOutput");
        if (SJ.logDiv) {
            SJ.log = function (msg) {        
                SJ.logDiv.innerText += '\n' + msg;
            }
        }
        else {
            SJ.log = function (msg) { }
        }
    }
}

function SJ_logCall(name) {
    return;
    //if (!(name == "createFromXaml")) return;
    //if (!(name == "measure" || name == "arrange")) return;
    var msg = name + ": ";
    for (var i = 1; i < arguments.length; i++) {
        msg += arguments[i] + ", ";
    }
    for (var i = 0; i < SJ_logCall.caller.arguments.length; i++) {
        msg += SJ_logCall.caller.arguments[i] + ", ";
    }
    msg = msg.substring(0, msg.length-2);
    SJ.log(msg);
}

// Generates a closure for hooking up JS object methods to event handlers.
// (Basically like a CLR delegate.) If you just write the closure inline
// it will close over all the locals of the function it's in, which can cause
// memory leaks.
// 
// E.g., to set myObj.HandleClick as an onclick handler:
// control.onClick = SJ.methodCaller(myObj, "HandleClick");

SJ.methodCaller = function (obj, methodName) {
    return function() {
        return obj[methodName].apply(obj, arguments);
    }
};

// The page should call SJ.initialize once the WPF/E control has loaded.

SJ.initialize = function (wpfeControl) {
    SJ.initLogging();
    SJ.wpfeControl = wpfeControl;
    SJ.topCanvas = SJ.findElement("topCanvas");
    SJ.wpfeHost = SJ.topCanvas.getHost();
    SJ.mouseCaptureElement = null;
    SJ.cancelBubble = false;
    
    SJ.dragDrop = {}
    SJ.dragDrop.dragSurface = SJ.topCanvas;
    SJ.dragDrop.dragging = null; // SJ control being dragged
    SJ.dragDrop.dragVisual = null;
    SJ.dragDrop.dragSurface.addEventListener("MouseMove","SJ_DragDrop_DragSurface_MouseMove");
    SJ.dragDrop.dragSurface.addEventListener("MouseLeftButtonDown", "SJ_DragDrop_DragSurface_MouseLeftButtonDown");
    SJ.dragDrop.dragSurface.addEventListener("MouseLeftButtonUp", "SJ_DragDrop_DragSurface_MouseLeftButtonUp");
    
    SJ.toolTip = {}
    SJ.toolTip.visual = null; // SJ control using tooltip
    SJ.toolTip.text = '';
    SJ.toolTip.timer = null;  // when expired, show the tooltip
    SJ.toolTip.lastMousePosition = null;
    SJ.toolTip.toolTipHover = null;
}

// Opens a new window; calls SJ.onOpenWindowFailed on error

SJ.openWindow = function (url, name) {
    var w = window.open(url, name ? name : "_blank");
    if (!w && SJ.onOpenWindowFailed)
        SJ.onOpenWindowFailed();
    if (w && SJ.onOpenWindowSucceeded)
        SJ.onOpenWindowSucceeded();
}

// Find an element in WPF/E.

SJ.findElement = function (name) {
    return SJ.wpfeControl.content.findName(name);
}

// Partial workaround for bug#2

SJ.captureMouse = function (element) {
    SJ.mouseCaptureElement = element;
    element.CaptureMouse();
}

SJ.releaseMouseCapture = function () {
    if (SJ.mouseCaptureElement)
        SJ.mouseCaptureElement.ReleaseMouseCapture();
    SJ.mouseCaptureElement = null;
};

function SJ_ensureMouseCapture() {
    if (SJ.mouseCaptureElement)
        SJ.mouseCaptureElement.CaptureMouse();
};

// Javascript helper functions

if (!String.prototype.trim) {
    String.prototype.trim = function() {
        return this.replace(/^\s+|\s+$/g, '');
    }
}

if (!String.prototype.splitAny) {
    String.prototype.splitAny = function (splitChars) {
        var result = [];
        var spanStart = 0;
        for (i = 0; i < this.length; i++) {
            var ch = this.charAt(i);
            if (splitChars.indexOf(ch) != -1) {
                if (i > spanStart) {
                    var s = this.substring(spanStart, i).trim();
                    if (s != '')
                        result.push(s);
                }
                spanStart = i+1;
            }
        }
        var s = this.substring(spanStart).trim();
        if (s != '')
            result.push(s);
        return result;
    }
}

// Ask the WPF/E control to instantiate some XAML.
// Second argument is an expando used to hold mappings from %name%
// to generated (SJ_xxx) names.
// When used from a Control, often this.names is passed as the second
// argument, but it can be left off to save time if there are no %name%s
// in the XAML.

SJ.createFromXaml = function (xaml, names) {
    SJ_logCall("createFromXaml");
    if (names)
        xaml = SJ.generateUniqueNames(xaml, names);
    return SJ.wpfeControl.content.createFromXaml(xaml);
}

// Remove the first occurrence of element from array.
// I could put this in Array.prototype but then I would worry about conflicts...

SJ.removeOne = function (array, element) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] === element) {
            array.splice(i, 1);
            return;
        }
    }
}

// Find the first occurence of element in array.

SJ.findFirst = function (array, element) {
    for (var i = 0; i < array.length; i++) {
        if (array[i] === element)
            return i;
    }
    return -1;
}

// Escape special characters for XML insertions.

SJ.xmlEscape = function (text) {
    return text.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
}

// Convert an integer Color to #aarrggbb format.

SJ.makeHexColor = function (color) {
    var result = "";
    for (var i = 0; i < 8; i++) {
        result = (color & 0xF).toString(16) + result;
        color >>= 4;
    }
    return "#" + result;
}

// Put commas in a number. (Must be an integer.)

SJ.commaizeNumber = function (num) {
    var numStr = Math.floor(num).toString();
    
    var groups = [];
    var i = 0;
    var groupLen = (numStr.length % 3) || 3;
    while (i < numStr.length) {
        groups.push(numStr.substr(i, groupLen));
        i += groupLen;
        groupLen = 3;
    }
    return groups.join(",");
}

// Convert integer seconds to XAML duration format (hours:minutes:seconds.millis)

SJ.toDuration = function (duration) {
    var hours = Math.floor(duration / 3600);
    duration -= hours * 3600;
    var minutes = Math.floor(duration / 60);
    duration -= minutes * 60;
    var seconds = Math.floor(duration);
    duration -= seconds;
    var millis = (Math.floor(duration * 1000) + 1000).toString().substr(1);
    return hours + ":" + minutes + ":" + seconds + "." + millis;
}

// Random integer in the range [low, high)

SJ.randomInt = function (low, high) {
    return Math.floor(Math.random() * (high - low)) + low;
}

// Place and size an element.

SJ.placeElement = function (element, left, top, width, height) {
    element["Canvas.Left"] = left;
    element["Canvas.Top"] = top;
    if (width !== undefined)
        element.Width = width;
    if (height !== undefined)
        element.Height = height;
};

// Rewrite XAML to give the elements unique names.
// This is necessary because all names have global scope in WPF/E.
// Each %name% in the xaml string is replaced by %uniquename% and
// names["name"] is set to "uniquename".

SJ.generateUniqueNames = function (xaml, names) {
    xaml = xaml.replace(/%(\w+)%/g,
                        function (match, name) {
                            if (!names[name]) {
                                names[name] = "SJ_" + SJ_uniqueID++;
                            }
                            return names[name];
                        });
    return xaml;
}

// Leak checking

function SJ_countHandlers() {
    var count = 0;
    for (var x in window) {
        if (x.substr(0,3) == "SJ_" && window[x] != null)
            count++;
    }
    return count;
}

var SJ_handlerTags = {};

function SJ_tagHandler(name, tag) {
    return;
    if (tag)
        SJ_handlerTags[name] = tag;
    else
        delete SJ_handlerTags[name];
}

var SJ_controlCount = 0;

// Sequencer invokes a list of functions in sequence. We use it to run several animations in turn. 

SJ.Sequencer = function () {
    this.functions = [];
    this.next = 0;
    this.invoker = SJ.methodCaller(this, "invokeNext");
}

SJ.Sequencer.prototype.toString = function () {
    return "SJ.Sequencer";
}

SJ.Sequencer.prototype.add = function (func) {
    this.functions.push(func);
}

SJ.Sequencer.prototype.run = function () {
    this.invokeNext();
}

SJ.Sequencer.prototype.invokeNext = function () {
    if (this.next < this.functions.length)
        this.functions[this.next++]();
}

// Basic UI control

SJ.Control = function () {
    this.visual = null;
    this.parentControl = null;
    this.children = [];
    this.hAlign = "left";
    this.vAlign = "top";
    
    // Names of global functions used for event handlers
    // (these are removed by Control.dispose)
    this.globalHandlerNames = [];
    
    // Mapping from friendly element names to generated names
    this.names = {};
    
    SJ_controlCount++;
}

SJ.Control.prototype.toString = function () {
    return "SJ.Control";
}

SJ.Control.prototype.dispose = function () {
    if (this.disposed) {
        //debugger;
        return;
    }
    this.disposed = true;
    SJ_controlCount--;
    
    if (this.children) {
        while (this.children.length > 0)
            this.children[0].dispose();     // note that dispose removes the child
        this.children = null;
    }
    
    this.setParent(null);
    this.visual = null;
    
    if (this.globalHandlerNames) {
        for (var i = 0; i < this.globalHandlerNames.length; i++) {
            var info = this.globalHandlerNames[i];  // [element, event, handler, eventToken]
            SJ_tagHandler(info[2], null);
            info[0].removeEventListener(info[1], info[3]);
            // review: IE apparently doesn't support deleting slots from window.
            // So I guess we should move to a free list to avoid adding slots forever.
            //delete window[this.globalHandlerNames[i]];
            window[info[2]] = null;
        }
        this.globalHandlerNames = [];
    }
}

// Set the parent of this control to either another control or a WPF/E element.

SJ.Control.prototype.setParent = function (newParent) {
    var newParentVisual = null;
    
    // Unhook the existing parent control, if any
    if (this.parentControl != null) {
        // todo: should have removeChild()
        SJ.removeOne(this.parentControl.children, this);
        //this.parentControl.updateLayout();
        this.parentControl = null;
    }
    
    // Switch the control parentage
    
    if (newParent instanceof SJ.Control) {
        newParentVisual = newParent.visual;
        this.parentControl = newParent;
        newParent.children.push(this);
    }
    else {
        newParentVisual = newParent;
    }
    
    // Switch the WPF/E parentage
    
    var curParentVisual = this.visual.getParent();
    if (curParentVisual) {
        curParentVisual.Children.Remove(this.visual);
    }
    if (newParent) {
        newParentVisual.Children.Add(this.visual);
    }
}

// Get the parent control or WPF/E element of this control.

SJ.Control.prototype.getParent = function () {
    if (this.parentControl)
        return this.parentControl;
    else if (this.visual)
        return this.visual.GetParent();
    else
        return null;
}

SJ.Control.prototype.move = function (left, top) {
    this.visual["Canvas.Left"] = left;
    this.visual["Canvas.Top"] = top;
}

SJ.Control.prototype.resize = function (width, height) {
    var prevWidth = this.visual.Width;
    var prevHeight = this.visual.Height;
    this.visual.Width = width;
    this.visual.Height = height;
    if (prevWidth != width || prevHeight != height)
        this.sizeChanged(width, height);
}

SJ.Control.prototype.getWidth = function () {
    return this.visual.Width;
}

SJ.Control.prototype.setWidth = function (width) {
    this.resize(width, this.visual.Height);
}

SJ.Control.prototype.getHeight = function () {
    return this.visual.Height;
}

SJ.Control.prototype.setHeight = function (height) {
    this.resize(this.visual.Width, height);
}

SJ.Control.prototype.marginWidth = function () {
    return this.margin ? (this.margin.left || 0) + (this.margin.right || 0) : 0;
}

SJ.Control.prototype.marginHeight = function () {
    return this.margin ? (this.margin.top || 0) + (this.margin.bottom || 0) : 0;
}

SJ.Control.prototype.setLeft = function (left) {
    this.visual["Canvas.Left"] = left;
}

SJ.Control.prototype.setTop = function (top) {
    this.visual["Canvas.Top"] = top;
}

SJ.Control.prototype.sizeChanged = function (width, height) {
    this.updateLayout();
}

SJ.Control.prototype.moveToFront = function () {
/*    var parentVisual = this.visual.GetParent();
    parentVisual.Children.Remove(parentVisual);
    parentVisual.Children.Add(this.visual);
*/
    this.setParent(this.getParent());
}

SJ.Control.prototype.updateLayout = function () {
    this.measure(Number.POSITIVE_INFINITY, Number.POSITIVE_INFINITY);
    this.arrange(this.visual["Canvas.Left"], this.visual["Canvas.Top"],
                 this.desiredWidth, this.desiredHeight);
}

SJ.Control.prototype.measure = function (availWidth, availHeight) {
    this.desiredWidth = (this.hAlign == "stretch") ? availWidth : this.visual.Width;
    this.desiredHeight = (this.vAlign == "stretch") ? availHeight : this.visual.Height;
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.Control.prototype.arrange = function (left, top, width, height) {
    SJ_logCall("arrange", this);
    this.visual["Canvas.Left"] = left;
    this.visual["Canvas.Top"] = top;
    this.visual.Width = width;
    this.visual.Height = height;
}

// Controls with layout optimization override this

SJ.Control.prototype.invalidateLayout = function () {
}

SJ.Control.prototype.visible = function (isVisible) {
    this.visual.Visibility = isVisible ? "Visible" : "Collapsed";
}

// Takes an array of <Storyboard> XAML strings.
// Do not include xmlns:x -- it is inserted by this function.
// XAML for this.visual must have included <Canvas.Resources/>.

SJ.Control.prototype.setAnimations = function (storyboards) {
    for (var i = 0; i < storyboards.length; i++) {
        var xaml = storyboards[i].replace("<Storyboard ", "<Storyboard xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' ");
        this.visual.Resources.Add(SJ.createFromXaml(xaml, this.names));
    }
}

// Takes a '<TransformGroup>...</TransformGroup>' XAML block and 
// sets this.visual.RenderTransform

SJ.Control.prototype.setTransform = function (transform) {
    this.visual.RenderTransform = SJ.createFromXaml(transform, this.names);
}

SJ.Control.prototype.situate = function (place) {
    // Subclass should override this to handle according to its animations
}

SJ.Control.prototype.getAnimation = function (animationName) {
    if (!this.names[animationName]) {
        SJ.log("no animation " + animationName);
        return null;
    }

    var storyboard = SJ.findElement(this.names[animationName]);
    if (!storyboard) {
        SJ.log("no storyboard " + animationName);
        return null;
    }

    return storyboard;
}

SJ.Control.prototype.animate = function (animationName, completedCallback) {
    var storyboard = this.getAnimation(animationName);
    if (!storyboard)
        return;
    
    if (completedCallback) {
        var handlerName = "SJ_" + SJ_uniqueID++;
        var me = this;
        var eventToken;
        SJ_tagHandler(handlerName, "animate");
        eventToken = storyboard.addEventListener("Completed", handlerName);
        window[handlerName] = function (sender, eventArgs) {
            SJ_tagHandler(handlerName, null);
            window[handlerName] = null;
            storyboard.removeEventListener("Completed", eventToken);
            completedCallback();
        };
    }
    
    storyboard.Begin();
}

SJ.Control.prototype.stopAnimation = function (animationName) {
    var storyboard = this.getAnimation(animationName);
    if (!storyboard)
        return;
    storyboard.Stop();
}

// Animate the control to a new location. Control must have a TranslateTransform
// called %translate% for this to work.

SJ.Control.prototype.animateMove = function (newLeft, newTop, onCompleted) {
    // Ensure we have a move animation set up
    if (!this.names["translate"]) {
        SJ.log("animateMove: missing translate");
        return;
    }
    if (!this.names["move"]) {
        this.setAnimations([
            "<Storyboard x:Name='%move%'> \
                <DoubleAnimation x:Name='%moveX%' \
                     Storyboard.TargetName='%translate%' \
                     Storyboard.TargetProperty='X' \
                     From='0' To='0' Duration='0:0:0.75' /> \
                <DoubleAnimation x:Name='%moveY%' \
                     Storyboard.TargetName='%translate%' \
                     Storyboard.TargetProperty='Y' \
                     From='0' To='0' Duration='0:0:0.75' /> \
            </Storyboard>"]);
    }
    
    // Move the control to its target location
    var left = this.visual["Canvas.Left"];
    var top = this.visual["Canvas.Top"];
    this.visual["Canvas.Left"] = newLeft;
    this.visual["Canvas.Top"] = newTop;

    // Animate from its original location to the target
    var moveX = SJ.findElement(this.names['moveX']);
    var moveY = SJ.findElement(this.names['moveY']);
    moveX.From = left - newLeft;
    moveX.To   = 0;
    moveY.From = top - newTop;
    moveY.To   = 0;
    this.animate("move", onCompleted);
}



// TextEdit controls use HTML input elements, which have to be hidden whenever a WPF/E animation
// is taking place, and must be moved when the control moves.

SJ.Control.prototype.showTextEdits = function () {
    this.eachDescendent(function (ctl) { if (ctl instanceof SJ.TextEdit) ctl.setInputVisible(true); });
}

SJ.Control.prototype.hideTextEdits = function () {
    this.eachDescendent(function (ctl) { if (ctl instanceof SJ.TextEdit) ctl.setInputVisible(false); });
}

SJ.Control.prototype.placeTextEdits = function () {
    this.eachDescendent(function (ctl) { if (ctl instanceof SJ.TextEdit) ctl.doLayout(); });
}

// Class variable for eternally-incrementing handler IDs
SJ_uniqueID = 0;

// Hook up a WPF/E event to a method of this control.
// The <eventName> event on <wpfeElement> will call this.<methodName>(sender, eventArgs).
//
// wpfeElement defaults to this.visual.
// methodName defaults to the same as eventName.
// 
// For each event handler this function creates a global function called SJ_nnn
// that is a closure that passes the call to the HandlerName method of the SJ object
// associated with the WPF/E object.
//
// The browser cleans out the window object when it navigates to another page, so these
// things don't get left around forever. The SJ object has to clean them up when it is disposed,
// though, so it keeps an array of their names.

SJ.Control.prototype.hookUpEvent = function (eventName, wpfeElement, methodName) {
    wpfeElement = wpfeElement || this.visual;
    methodName = methodName || eventName;
    
    var handlerName = "SJ_" + SJ_uniqueID++;
    var eventToken = wpfeElement.AddEventListener(eventName, handlerName);
    
    this.globalHandlerNames.push([wpfeElement, eventName, handlerName, eventToken]);
    SJ_tagHandler(handlerName, "hookUpEvent " + this.toString());
    window[handlerName] = SJ.methodCaller(this, methodName);
};

// Find the left/top in WPF/E control's top-level coordinates.
// DOES NOT take RenderTransforms into account!

SJ.Control.prototype.topCanvasPosition = function () {
    var result = {left: this.visual["Canvas.Left"], top: this.visual["Canvas.Top"]};
    var element = this.visual.GetParent();
    while (element) {
        result.left += element["Canvas.Left"];
        result.top += element["Canvas.Top"];
        element = element.GetParent();
    }
    return result;
}

// Find the left/top in browser's BODY coordinates.
// DOES NOT take RenderTransforms into account!

SJ.Control.prototype.browserPosition = function () {
    var result = this.topCanvasPosition();
    element = SJ.wpfeControl.parentNode;
    while (element && element.tagName != "BODY") {
        result.left += element.offsetLeft;
        result.top += element.offsetTop;
        element = element.parentNode;
    }
    return result;
}

// Walk the control tree doing something

SJ.Control.prototype.eachDescendent = function (operation) {
    operation(this);
    for (var i = 0; i < this.children.length; i++) {
        this.children[i].eachDescendent(operation);
    }
}

// Layer

SJ.Layer = function (top, left) {
    SJ.Control.call(this);
    
    var xaml = 
        "<Canvas \
            xmlns='http://schemas.microsoft.com/client/2007' \
            xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' \
            x:Name='%name%' > \
            <Canvas.Resources></Canvas.Resources> \
         </Canvas>";

    this.names = {};
    xaml = SJ.generateUniqueNames(xaml, this.names);    
    this.visual = SJ.createFromXaml(xaml);

    SJ.placeElement(this.visual, top, left);
}

SJ.Layer.prototype = new SJ.Control;

SJ.Layer.prototype.toString = function () {
    return "SJ.Layer";
}

// Image

SJ.Image = function (top, left, source, async, stretch, onLoad) {
    SJ.Control.call(this);
    
    this.source = source;
    var async = !!async;
    stretch = stretch || "None";
    if (onLoad)
        this.onLoad = onLoad;
    
    var xaml = "<Image ";
    if (!async)
        xaml += " Source='" + source + "'";
    if (stretch)
        xaml += " Stretch='" + stretch + "'";
    xaml += "/>";
    
    this.visual = SJ.createFromXaml(xaml);
    
    SJ.placeElement(this.visual, top, left);
    
    if (async) {
        // todo: use WPF/E downloader?
        this.img = new Image;
        this.img.onload = SJ.methodCaller(this, "imageLoaded");
        this.img.src = this.source;
    }
}

SJ.Image.prototype = new SJ.Control;

SJ.Image.prototype.toString = function () {
    return "SJ.Image";
}

SJ.Image.prototype.imageLoaded = function () {
    if (this.visual) {
        this.visual.Source = this.source;
        // review: no way to get actual image width/height from WPF/E Image?
        this.setImageSize(this.img.width, this.img.height);
        if (this.onLoad)
            this.onLoad();
    }
}

SJ.Image.prototype.setImageSize = function (width, height) {
    this.visual.Width = width;
    this.visual.Height = height;
}

// Button
// If images is supplied it should be an object with the slots
// idle, hover, activeDown, activeUp, and disabled, with each slot containing the Image.Source
// to be used for that button state. If no images is supplied the button will have a
// standard vector background. If images.rotateOnDisabled = true, the disabled image
// will rotate (for use as a progress indicator).

SJ.Button = function (left, top, width, height, label, images) {
    SJ.Control.call(this);
    
    this.images = images;
    
    var xaml =
        "<Canvas xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>";
    if (this.images)
        xaml += "<Image x:Name='%image%' />";
    else
        xaml +=
            "<Rectangle RadiusX='4' RadiusY='4' Stroke='#01b9ff' StrokeThickness='2'> \
                <Rectangle.Fill> \
                    <LinearGradientBrush StartPoint='0.5,0' EndPoint='0.5,1' > \
                        <GradientStop Color='#ffffff' Offset='0.0' /> \
                        <GradientStop Color='#e4edf2' Offset='1.0' /> \
                    </LinearGradientBrush> \
                </Rectangle.Fill> \
            </Rectangle>";
    xaml +=
        "<TextBlock FontSize='12' /> \
        </Canvas>";
    
    this.visual = SJ.createFromXaml(xaml, this.names);

    // Set event handlers
    
    this.hookUpEvent("MouseLeftButtonDown");
    this.hookUpEvent("MouseLeftButtonUp");
    this.hookUpEvent("MouseEnter");
    this.hookUpEvent("MouseLeave");
    
    this.background = this.visual.Children.GetItem(0);  // either the Rectangle or the Image
    this.label = this.visual.Children.GetItem(1);
    
    // Allow a rotating disabled image for progress indication
    
    if (images && images.rotateOnDisabled) {
        this.background.RenderTransform = SJ.createFromXaml("<RotateTransform />");
        this.setAnimations([
           "<Storyboard x:Name='%spinProgress%'> \
                <DoubleAnimation \
                     Storyboard.TargetName='%image%' \
                     Storyboard.TargetProperty='(UIElement.RenderTransform).(RotateTransform.Angle)' \
                     From='0' To='360' Duration='0:0:2' RepeatBehavior='Forever' /> \
           </Storyboard>" 
        ]);
    }

    SJ.placeElement(this.visual, left, top, width, height);

    this.setLabel(label);
    this.setState("idle");
}

SJ.Button.prototype = new SJ.Control;

SJ.Button.prototype.toString = function () {
    return "SJ.Button";
}

SJ.Button.prototype.setLabel = function (newLabel) {
    this.label.Text = newLabel;
    this.doLayout();
}

SJ.Button.prototype.setEnabled = function (enabled) {
    this.setState(enabled ? "idle" : "disabled");
}

// Set the state and change the visuals as needed.
// States = { idle, hover, activeDown, activeUp }

SJ.Button.prototype.setState = function (newState) {
    if (this.images) {
        this.label.textDecorations = (newState == "hover") ? "Underline" : "None";
        this.background.Source = this.images[newState] || "";
        if (this.images.rotateOnDisabled) {
            if (this.state == "disabled")
                this.stopAnimation("spinProgress");
            if (newState == "disabled")
                this.animate("spinProgress");
        }
    }
    else {
        var backgroundStop = this.background.Fill.GradientStops.GetItem(1);
        
        switch (newState) {
            case "idle":
                backgroundStop.Color = "#FFDFF4E3";
                break;
            case "hover":
                backgroundStop.Color = "#FF80F4E3";
                break;
            case "activeDown":
                backgroundStop.Color = "#FF00F4E3";
                break;
            case "activeUp":
                backgroundStop.Color = "#FF40D4E3";
                break;
        }
    }
    
    this.state = newState;
}

SJ.Button.prototype.doLayout = function () {
    // Background fills the Canvas
    this.background.Width = this.visual.Width;
    this.background.Height = this.visual.Height;
    
    // Position label on the background
    this.label["Canvas.Top"] = (this.background.Height - this.label.ActualHeight) / 2;
    if (!this.labelAlign || this.labelAlign == "center")
        this.label["Canvas.Left"] = (this.background.Width - this.label.ActualWidth) / 2;
    else if (this.labelAlign == "right")
        this.label["Canvas.Left"] = this.background["Canvas.Left"] + (this.background.Width + 5);
    else if (this.labelAlign == "left")
        this.label["Canvas.Left"] = this.background["Canvas.Left"] - (this.label.ActualWidth + 5);
    
    // Center the RotateTransform if needed
    if (this.images && this.images.rotateOnDisabled) {
        var xform = this.background.RenderTransform;
        xform.CenterX = this.background.Width / 2;
        xform.CenterY = this.background.Height / 2;
    }
}

SJ.Button.prototype.MouseEnter = function (sender, eventArgs) {
    SJ_logCall("MouseEnter");
    if (this.state == "disabled") {
        return;
    }
    else if (this.state == "idle") {
        this.setState("hover");
    }
    else if (this.state == "activeUp") {
        this.setState("activeDown");
    }
    
    this.doLayout();
    
    if (this.onMouseEnter)
        this.onMouseEnter(this, eventArgs);
}

SJ.Button.prototype.MouseLeave = function (sender, eventArgs) {
    SJ_logCall("MouseLeave");
    if (this.state == "disabled") {
        return;
    }
    else if (this.state == "activeDown") {
        this.setState("activeUp");
    }
    else {
        this.setState("idle");
    }
    
    this.doLayout();

    if (this.onMouseLeave)
        this.onMouseLeave(this, eventArgs);
}

SJ.Button.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble && this.state != "disabled") {
        SJ.cancelBubble = true;
        SJ.captureMouse(this.visual);

        this.setState("activeDown");
    }
}

SJ.Button.prototype.MouseLeftButtonUp = function (sender, eventArgs) {
    if (!SJ.cancelBubble && this.state != "disabled") {
        SJ.cancelBubble = true;
        SJ.releaseMouseCapture();
        
        if (this.state == "activeDown") {
            this.setState("hover");
            if (this.onClick)
                this.onClick(this, eventArgs);
        }
        else {
            this.setState("idle");
        }
    }
}

// Scrollbar

//    ScrollbarPiece -- up/down button or thumb

SJ.ScrollbarPiece = function (scrollbar, shadowDirection) {
    SJ.Control.call(this);
    
    this.scrollbar = scrollbar;
    this.shadowDirection = shadowDirection;
    
    if (SJ.wpfeControl) {
        var xaml =
            "<Canvas xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'> \
                <Rectangle RadiusX='2' RadiusY='2' Stroke='#eceef0' /> \
                <Rectangle RadiusX='2' RadiusY='2' Stroke='#4087bc' /> \
                <Rectangle RadiusX='2' RadiusY='2' Stroke='#ffffff' /> \
            </Canvas>";
    
        this.visual = SJ.createFromXaml(xaml);
    
        this.shadow = this.visual.Children.GetItem(0);
        this.outline = this.visual.Children.GetItem(1);
        this.rim = this.visual.Children.GetItem(2);
    
        this.activeFill = SJ.createFromXaml(
            "<LinearGradientBrush StartPoint='0,0' EndPoint='1,0' > \
                <GradientStop Color='#ffffff' Offset='0' /> \
                <GradientStop Color='#dedede' Offset='0.75' /> \
                <GradientStop Color='#ffffff' Offset='1' /> \
            </LinearGradientBrush>"
        );
    
        this.hoverFill = SJ.createFromXaml(
            "<LinearGradientBrush StartPoint='0,0' EndPoint='1,0' > \
                <GradientStop Color='#ffffff' Offset='0' /> \
                <GradientStop Color='#cef0ff' Offset='0.75' /> \
                <GradientStop Color='#ffffff' Offset='1' /> \
            </LinearGradientBrush>"
        );
    
        this.pressedFill = SJ.createFromXaml(
            "<LinearGradientBrush StartPoint='0,0' EndPoint='1,0' > \
                <GradientStop Color='#ffffff' Offset='0' /> \
                <GradientStop Color='#d9f4ff' Offset='0.75' /> \
                <GradientStop Color='#ffffff' Offset='1' /> \
            </LinearGradientBrush>"
        );
    
        this.hookUpEvent("MouseLeftButtonDown");
        this.hookUpEvent("MouseLeftButtonUp");
        this.hookUpEvent("MouseEnter");
        this.hookUpEvent("MouseLeave");
    
        this.setState("idle");
    }
}

SJ.ScrollbarPiece.prototype = new SJ.Control;

SJ.ScrollbarPiece.prototype.toString = function () {
    return "SJ.ScrollbarPiece";
}

SJ.ScrollbarPiece.prototype.doLayout = function () {
    var width = this.visual.Width;
    var height = this.visual.Height;
    SJ.placeElement(this.shadow, this.scrollbar.isVertical ? 1 : -1,
         this.scrollbar.isVertical ? -1 : 1, width, height);
    SJ.placeElement(this.outline, 0, 0, width, height);
    SJ.placeElement(this.rim, 1, 1, width - 2, height - 2);
};

SJ.ScrollbarPiece.prototype.setState = function (newState) {
    this.state = newState;
    
    switch (newState) {
        case "idle":
            this.outline.Stroke = "#9b9b9b";
            this.outline.Fill = this.activeFill;
            this.shadow.Stroke = "#cbcbcb";
            break;
        case "hover":
        case "activeUp":
            this.outline.Stroke = "#4087bc";
            this.outline.Fill = this.hoverFill;
            this.shadow.Stroke = "#eceef0";
            break;
        case "activeDown":
            this.outline.Stroke = "#4087bc";
            this.outline.Fill = this.pressedFill;
            this.shadow.Stroke = "#aed8ec";
            break;
    }
}

SJ.ScrollbarPiece.prototype.MouseEnter = function (sender, eventArgs) {
    SJ_logCall("MouseEnter", this.state);
    if (this.state == "idle") {
        this.setState("hover");
    }
    else if (this.state == "activeUp") {
        this.setState("activeDown");
    }
}

SJ.ScrollbarPiece.prototype.MouseLeave = function (sender, eventArgs) {
    SJ_logCall("MouseLeave", this.state);
    if (this.state != "activeDown") {
        this.setState("idle");
    }
}

SJ.ScrollbarPiece.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    // Don't check SJ.cancelBubble -- assume that the dervied class does.
    SJ.captureMouse(this.visual);
        
    this.setState("activeDown");
    this.scrollbar.setPieceActive(true);
}

SJ.ScrollbarPiece.prototype.MouseLeftButtonUp = function (sender, eventArgs) {
    // Don't check SJ.cancelBubble -- assume that the dervied class does.
    SJ.releaseMouseCapture();
    
    if (this.state == "activeDown")
        this.setState("hover");
    else
        this.setState("idle");

    this.scrollbar.setPieceActive(false);
}

SJ.ScrollbarButton = function (scrollbar, isLeft) {
    SJ.ScrollbarPiece.call(this, scrollbar);
    this.isLeft = isLeft;
    
    // todo: scale path based on height
    if (isLeft)
        var arrowPath = "M 12,4 L 7,10 L 12,16";
    else
        var arrowPath = "M 9,16 L 14,10 L 9,4";
    
    var iArrow = this.visual.Children.Add(SJ.createFromXaml("<Path Data='" + arrowPath + "' />"));
    this.arrow = this.visual.Children.GetItem(iArrow);
    
    this.inactiveArrowFill = SJ.createFromXaml(
        "<LinearGradientBrush StartPoint='1,0' EndPoint='0,0' > \
            <GradientStop Color='#909ab3' Offset='0' /> \
            <GradientStop Color='#b7c4f5' Offset='1' /> \
        </LinearGradientBrush>");

    this.activeArrowFill = SJ.createFromXaml(
        "<LinearGradientBrush StartPoint='1,0' EndPoint='0,0' > \
            <GradientStop Color='#2c4073' Offset='0' /> \
            <GradientStop Color='#7a94f5' Offset='1' /> \
        </LinearGradientBrush>");
}

SJ.ScrollbarButton.prototype = new SJ.ScrollbarPiece;

SJ.ScrollbarButton.prototype.setActive = function (isActive) {
    if (isActive) {
        this.arrow.Fill = this.activeArrowFill;
        this.shadow.Opacity = 1;
        this.outline.Opacity = 1;
        this.rim.Opacity = 1;
    }
    else {
        this.arrow.Fill = this.inactiveArrowFill;
        this.shadow.Opacity = 0;
        this.outline.Opacity = 0;
        this.rim.Opacity = 0;
    }
}

function SJ_scrollBarAutoRepeater(target, message) {
    return function () {
        target[message]();
    }
}

SJ.ScrollbarButton.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        SJ.ScrollbarPiece.prototype.MouseLeftButtonDown.call(this, sender, eventArgs);
        
        var message = this.isLeft ? "lineDown" : "lineUp";
        
        this.scrollbar[message]();

        // In case we lost track due to capture bug
        if (this.scrollbar.timerID)
            clearTimeout(this.scrollbar.timerID);
            
        this.scrollbar.timerID = window.setInterval(SJ_scrollBarAutoRepeater(this.scrollbar, message), 50);
    }
}

SJ.ScrollbarButton.prototype.MouseLeftButtonUp = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        SJ.ScrollbarPiece.prototype.MouseLeftButtonUp.call(this, sender, eventArgs);
        
        clearTimeout(this.scrollbar.timerID);
    }
}

SJ.ScrollbarThumb = function (scrollbar) {
    SJ.ScrollbarPiece.call(this, scrollbar);

    this.inactiveFill = SJ.createFromXaml(
        "<SolidColorBrush Color='#f4f4f4' />"
    );

    this.hookUpEvent("MouseMove");
}

SJ.ScrollbarThumb.prototype = new SJ.ScrollbarPiece;

SJ.ScrollbarThumb.prototype.setActive = function (isActive) {
    if (isActive)
        this.setState(this.state);
    else {
        this.outline.Stroke = "#c3c3c3";
        this.outline.Fill = this.inactiveFill;
        this.shadow.Stroke = "#dfdfdf";
    }
}

SJ.ScrollbarThumb.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        SJ.ScrollbarPiece.prototype.MouseLeftButtonDown.call(this, sender, eventArgs);
        
        this.scrollbar.thumbMouseDown(sender, eventArgs);
    }
}

SJ.ScrollbarThumb.prototype.MouseMove = function (sender, eventArgs) {
    if (this.state == "activeDown")
        this.scrollbar.thumbMouseMove(sender, eventArgs);
}

SJ.Scrollbar = function (left, top, width, height, isVertical, minValue, maxValue, pageSize, lineSize, startValue) {
    SJ.Control.call(this);
    left = left || 0;
    top = top || 0;
    this.isVertical = !!isVertical;
    width = width || (isVertical ? 22 : 100);
    height = height || (isVertical ? 100 : 22);
    this.minValue = (minValue === undefined) ? 0 : minValue;
    this.maxValue = (maxValue === undefined) ? 100 : maxValue;
    this.pageSize = (pageSize === undefined) ? 10 : pageSize;
    this.lineSize = (lineSize === undefined) ? 1 : lineSize;
    this.value = (startValue === undefined) ? this.minValue : startValue;
        
    if (SJ.wpfeControl) {
        var xaml =
            "<Canvas xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'> \
                <Canvas> \
                    <Canvas.Background> \
                        <LinearGradientBrush StartPoint='0,1' EndPoint='0,0' > \
                            <GradientStop Color='#f0f0f0' Offset='0' /> \
                            <GradientStop Color='#f4f4f4' Offset='1' /> \
                        </LinearGradientBrush> \
                    </Canvas.Background> \
                    <Line Y1='0.5' Y2='0.5' StrokeThickness='1' Stroke='#ffffff' /> \
                    <Line Y1='1.5' Y2='1.5' StrokeThickness='1' Stroke='#f1f1f1' /> \
                    <Canvas /> \
                    <Path /> \
                    <Path /> \
                </Canvas> \
            </Canvas>";
    
        this.visual = SJ.createFromXaml(xaml);
                
        this.innerCanvas = this.visual.Children.GetItem(0);
        var children = this.innerCanvas.Children;
        this.edge = children.GetItem(0);
        this.innerEdge = children.GetItem(1);
        this.pieceCanvas = children.GetItem(2);
        this.arrows = [children.GetItem(3), children.GetItem(4)];

        this.hookUpEvent("MouseEnter");
        this.hookUpEvent("MouseLeave");
        this.hookUpEvent("MouseLeftButtonDown");
    
        SJ.placeElement(this.visual, left, top, width, height);
    
        if (this.isVertical)
            this.vAlign = "stretch";
        else
            this.hAlign = "stretch";
    
        this.leftBtn = new SJ.ScrollbarButton(this, true);
        this.leftBtn.setParent(this.pieceCanvas);
        this.rightBtn = new SJ.ScrollbarButton(this, false);
        this.rightBtn.setParent(this.pieceCanvas);
        this.thumb = new SJ.ScrollbarThumb(this);
        this.thumb.setParent(this.pieceCanvas);
    
        this.doLayout();
    
        this.setState("idle");
    }
}

SJ.Scrollbar.prototype = new SJ.Control;

SJ.Scrollbar.prototype.toString = function () {
    return "SJ.Scrollbar";
}

SJ.Scrollbar.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);
    this.doLayout();
}

SJ.Scrollbar.prototype.sizeChanged = function () {
    this.doLayout();
}

SJ.Scrollbar.prototype.doLayout = function () {
    var innerWidth = this.isVertical ? this.visual.Height : this.visual.Width;
    var innerHeight = this.isVertical ? this.visual.Width : this.visual.Height;
    
    if (this.isVertical && this.visual.RenderTransform == null) {
        this.innerCanvas.RenderTransform = SJ.createFromXaml(
            "<TransformGroup> \
                <RotateTransform Angle='90'/> \
                <ScaleTransform ScaleX='-1' /> \
            </TransformGroup>");
    }
    else if (!this.isVertical && this.visual.RenderTransform != null) {
        // todo: make changing to horizontal work
        // Can't set to null -- error from WPF/E
        // this.visual.RenderTransform = null;
    }
    
    if (this.innerCanvas.Width != innerWidth || this.innerCanvas.Height != innerHeight) {
        // innerCanvas fills the control
        this.innerCanvas.Width = innerWidth;
        this.innerCanvas.Height = innerHeight;
        this.innerCanvas.Clip = SJ.createFromXaml("<RectangleGeometry Rect='" +
            "0,0," + innerWidth + "," + innerHeight + "' />");
    
        this.edge.X2 = innerWidth;
        this.innerEdge.X2 = innerWidth;
        
        SJ.placeElement(this.thumb.visual, 4, 0, innerHeight, innerHeight);
        
        SJ.placeElement(this.leftBtn.visual, 1, 1, innerHeight - 2, innerHeight - 2);
        this.leftBtn.doLayout();
        
        SJ.placeElement(this.rightBtn.visual, innerWidth - innerHeight + 1, 1, innerHeight - 2, innerHeight - 2);
        this.rightBtn.doLayout();
    }
    
    this.updateThumb();
}

SJ.Scrollbar.prototype.updateThumb = function () {
    var pinnedValue = Math.min(this.maxValue, Math.max(this.minValue, this.value));
    var height = this.innerCanvas.Height;
    var width = this.innerCanvas.Width - height * 2 - 1;
    this.thumb.visual.Width = Math.max(height, Math.min(width, width * this.pageSize / (this.pageSize + this.maxValue - this.minValue)));
    if (this.maxValue - this.minValue != 0) {
        this.thumb.visual["Canvas.Left"] = height + Math.min((width - this.thumb.visual.Width) * (pinnedValue - this.minValue) /
                                                                 (this.maxValue - this.minValue),
                                                             width - this.thumb.visual.Width);
    }
    else {
        this.thumb.visual["Canvas.Left"] = height;
    }
    this.thumb.doLayout();
}

SJ.Scrollbar.prototype.setValue = function (newValue) {
    this.value = newValue;
    this.updateThumb();
    if (this.onValueChanged) {
        this.onValueChanged(this, {newValue: newValue});
    }
}

SJ.Scrollbar.prototype.lineDown = function () {
    if (this.value > this.minValue)
        this.setValue(Math.max(this.value - this.lineSize, this.minValue));
}

SJ.Scrollbar.prototype.lineUp = function () {
    if (this.value < this.maxValue)
        this.setValue(Math.min(this.value + this.lineSize, this.maxValue));
}

SJ.Scrollbar.prototype.pageDown = function () {
    if (this.value > this.minValue)
        this.setValue(Math.max(this.value - this.pageSize, this.minValue));
}

SJ.Scrollbar.prototype.pageUp = function () {
    if (this.value < this.maxValue)
        this.setValue(Math.min(this.value + this.pageSize, this.maxValue));
}

SJ.Scrollbar.prototype.setMaxValue = function (newValue) {
    SJ_logCall("setMaxValue");
    this.maxValue = newValue;
    if (this.value > this.maxValue)
        this.setValue(this.maxValue);
    else
        this.updateThumb();
}

SJ.Scrollbar.prototype.setPageSize = function (newValue) {
    SJ_logCall("setPageSize");
    this.pageSize = newValue;
    this.updateThumb();
}

SJ.Scrollbar.prototype.setLineSize = function (newValue) {
    this.lineSize = newValue;
}

SJ.Scrollbar.prototype.getValue = function () {
    return this.value;
}

// Set the state and change the visuals as needed.

SJ.Scrollbar.prototype.setState = function (newState) {
    this.state = newState;
    
    switch (newState) {
        case "idle":
            this.edge.Opacity = 0;
            this.leftBtn.setActive(false);
            this.rightBtn.setActive(false);
            this.thumb.setActive(false);
            break;
        case "hover":
        case "trackingInside":
        case "trackingOutside":
            this.edge.Opacity = 1;
            this.leftBtn.setActive(true);
            this.rightBtn.setActive(true);
            this.thumb.setActive(true);
            break;
    }
}

SJ.Scrollbar.prototype.setPieceActive = function (newValue) {
    if (newValue) {
        if (this.state == "hover")
            this.setState("trackingInside");
        else
            this.setState("trackingOutside");
    }
    else {
        if (this.state == "trackingInside")
            this.setState("hover");
        else
            this.setState("idle");
    }

    this.pieceActive = newValue;
}

SJ.Scrollbar.prototype.MouseEnter = function (sender, eventArgs) {
    if (this.pieceActive)
        this.setState("trackingInside");
    else
        this.setState("hover");
}

SJ.Scrollbar.prototype.MouseLeave = function (sender, eventArgs) {
    if (this.pieceActive)
        this.setState("trackingOutside");
    else
        this.setState("idle");
}

SJ.Scrollbar.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        var mousePos = eventArgs.getPosition(this.visual);
        var offset;
        if (this.isVertical)
            offset = mousePos.y - this.thumb.visual["Canvas.Left"];
        else
            offset = mousePos.x - this.thumb.visual["Canvas.Left"];

        if (offset > 0) {
            if (offset > this.thumb.visual.Width)
                this.pageUp();
        } 
        else {
            this.pageDown();
        }
    }
}

SJ.Scrollbar.prototype.thumbMouseDown = function (sender, eventArgs) {
    var height = this.innerCanvas.Height;
    var mousePos = eventArgs.getPosition(this.visual);
    
    if (this.isVertical)
        this.thumbMouseDownOffset = mousePos.y - this.thumb.visual["Canvas.Left"];
    else
        this.thumbMouseDownOffset = mousePos.x - this.thumb.visual["Canvas.Left"];
}

SJ.Scrollbar.prototype.thumbMouseMove = function (sender, eventArgs) {
    // todo: spring back when mouse goes out of range
        
    var height = this.innerCanvas.Height;
    var mousePos = eventArgs.getPosition(this.visual);
    
    if (this.isVertical)
        var pos = mousePos.y;
    else
        var pos = mousePos.x;
    
    var width = this.innerCanvas.Width - height * 2 - 1;
    var divisor = width - this.thumb.getWidth();
    if (divisor > 0 || divisor < 0) { // protect against 0, NaN, ...
        this.setValue(Math.max(this.minValue, Math.min(this.maxValue,
            this.minValue +
                ((pos - height - this.thumbMouseDownOffset) / divisor) * (this.maxValue - this.minValue))));
    }
}

// ScrollViewer

SJ.ScrollViewer = function (left, top, width, height, hasVScroll, hasHScroll) {
    SJ.Control.call(this);
    
    hasVScroll = hasVScroll || false;
    hasHScroll = hasHScroll || false;
    
    this.positionX = 0;
    this.positionY = 0;
    this.content = null;
    
    var xaml =
        "<Canvas xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'>\
        <Canvas />\
        </Canvas>";
    
    this.visual = SJ.createFromXaml(xaml);
    
    this.contentHolder = this.visual.Children.GetItem(0);
    
    SJ.placeElement(this.visual, left, top, width, height);
    
    // Create scrollbars
    
    if (hasVScroll) {
        this.vScrollbar = new SJ.Scrollbar(null, 0, null, 0, true);
        this.vScrollbar.setLineSize(20);
        this.vScrollbar.setParent(this.visual);
        this.vScrollbar.onValueChanged = SJ.methodCaller(this, "vScrollValueChanged");
    }
    
    if (hasHScroll) {
        this.hScrollbar = new SJ.Scrollbar();
        this.hScrollbar.setLineSize(20);
        this.hScrollbar.setParent(this.visual);
        this.hScrollbar.onValueChanged = SJ.methodCaller(this, "hScrollValueChanged");
    }
    
    this.doLayout();
}

SJ.ScrollViewer.prototype = new SJ.Control;

SJ.ScrollViewer.prototype.toString = function () {
    return "SJ.ScrollViewer";
}

SJ.ScrollViewer.prototype.sizeChanged = function (width, height) {
    if (this.content) {
        var contentWidth = width;
        var contentHeight = height;
        if (this.vScrollbar)
            contentWidth -= this.vScrollbar.getWidth();
        if (this.hScrollbar)
            contentHeight -= this.hScrollbar.getHeight();
        this.content.resize(contentWidth, contentHeight);
    }
    SJ.Control.prototype.sizeChanged.call(this, width, height);
}

SJ.ScrollViewer.prototype.vScrollValueChanged = function (sender, eventArgs) {
    this.setPosition(this.positionX, - eventArgs.newValue);
}

SJ.ScrollViewer.prototype.hScrollValueChanged = function (sender, eventArgs) {
    this.setPosition(- eventArgs.newValue, this.positionY);
}

SJ.ScrollViewer.prototype.setContent = function (content) {
    if (this.content != null)
        this.content.dispose();
    
    this.content = content;
    this.content.setParent(this.contentHolder);
    
    this.setPosition(0, 0);
}

SJ.ScrollViewer.prototype.getContent = function () {
    return this.content;
}

SJ.ScrollViewer.prototype.measure = function (availWidth, availHeight) {
    if (this.content) {
        var contentWidth = this.hScrollbar ? Number.POSITIVE_INFINITY :
                                             (this.visual.Width - (this.vScrollbar ? this.vScrollbar.getWidth() : 0));
        var contentHeight = this.vScrollbar ? Number.POSITIVE_INFINITY :
                                              (this.visual.Height - (this.hScrollbar ? this.hScrollbar.getHeight() : 0));
        this.content.measure(contentWidth, contentHeight);
    }
    
    this.desiredWidth = this.visual.Width;
    this.desiredHeight = this.visual.Height;
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.ScrollViewer.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);

    if (this.content) {
        this.content.arrange(0, 0, this.content.desiredWidth, this.content.desiredHeight);
    }
    
    this.doLayout();
}

SJ.ScrollViewer.prototype.doLayout = function () {
    var width = this.visual.Width;
    var height = this.visual.Height;
    this.contentHolder.Width = this.content ? this.content.getWidth() : 0;
    this.contentHolder.Height = this.content ? this.content.getHeight() : 0;
    
    this.contentHolder.RenderTransform =
        SJ.createFromXaml("<TranslateTransform X='" + this.positionX + "' Y='" + this.positionY + "' />");
    
    if (this.vScrollbar) {
        var vsb = this.vScrollbar;
        vsb.setLeft(width - vsb.getWidth());
        vsb.setHeight(this.hScrollbar ? (height - this.hScrollbar.getHeight()) : height);
        vsb.setMaxValue( Math.max(0,this.contentHolder.Height - vsb.getHeight()) );
        vsb.setPageSize(height * 0.8);
    }
    
    if (this.hScrollbar) {
        var hsb = this.hScrollbar;
        hsb.setTop(height - hsb.getHeight());
        hsb.setWidth(this.vScrollbar ? (width - this.vScrollbar.getWidth()) : width);
        hsb.setMaxValue( Math.max(0,this.contentHolder.Width - hsb.getWidth()) );
        hsb.setPageSize(width * 0.8);
    }
    
    var clipWidth = this.vScrollbar ? width - this.vScrollbar.getWidth() : width;
    var clipHeight = this.hScrollbar ? height - this.hScrollbar.getHeight() : height;
    this.contentHolder.Clip = SJ.createFromXaml("<RectangleGeometry Rect='" +
        -this.positionX + "," + -this.positionY + "," + clipWidth + "," + clipHeight + "' />");
}

SJ.ScrollViewer.prototype.setPosition = function (x, y) {
    SJ_logCall("setPosition");
    this.positionX = x;
    this.positionY = y;
    this.doLayout();
}

SJ.ScrollViewer.prototype.lineUp = function() {
    if (this.vScrollbar)
        this.vScrollbar.lineUp();
}

SJ.ScrollViewer.prototype.lineDown = function() {
    if (this.vScrollbar)
        this.vScrollbar.lineDown();
}

SJ.ScrollViewer.prototype.pageUp = function() {
    if (this.vScrollbar)
        this.vScrollbar.pageUp();
}

SJ.ScrollViewer.prototype.pageDown = function() {
    if (this.vScrollbar)
        this.vScrollbar.pageDown();
}

// TextBlock
// Note: Text must be XML-escaped and may contain <Run> elements.

SJ.TextBlock = function (left, top, width, height, text, otherAttributes, background) {
    SJ.Control.call(this);

    this.otherAttributes = otherAttributes || "";
    this.ellipsis = false;
    
    if (SJ.wpfeControl) {
        // Canvas is necessary because TextBlock.Background doesn't work
        var xaml = "<Canvas \
              xmlns='http://schemas.microsoft.com/client/2007' \
              xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' \
              x:Name='%top%'><TextBlock " + this.otherAttributes + ">" + text + "</TextBlock></Canvas>";
    
        // WPF/E bug: Certain non-ASCII results cause errors in createFromXaml
        try {
            this.visual = SJ.createFromXaml(xaml, this.names);
        }
        catch (e) {
            SJ.log(e);
            SJ.log(xaml);
            this.visual = SJ.createFromXaml("<Canvas><TextBlock /></Canvas>");
        }
        
        if (background)
            this.visual.Background = background;
        
        this.textBlock = this.visual.Children.GetItem(0);
        this.fullText = this.textBlock.Text;

        SJ.placeElement(this.visual, left, top, width, height);
    }
}

SJ.TextBlock.prototype = new SJ.Control;

SJ.TextBlock.prototype.toString = function () {
    return "SJ.TextBlock";
}

SJ.TextBlock.prototype.getText = function () {
    return this.fullText;
}

SJ.TextBlock.prototype.sizeChanged = function (width, height) {
    this.textBlock.Text = this.fullText;
    this.textBlock.Width = width;
    this.textBlock.Height = height;
    SJ.Control.prototype.sizeChanged.call(this, width, height);
}

// Note: Text must be XML-escaped and may contain <Run> elements.

SJ.TextBlock.prototype.setText = function (text) {
    // There doesn't seem to be a way to set formatted text in an existing TextBlock,
    // so we replace the whole thing.
    
    var xaml = "<TextBlock " + this.otherAttributes + ">" + text + "</TextBlock>";
    
    // WPF/E bug: Certain non-ASCII results cause errors in createFromXaml
    try {
        var newTextBlock = SJ.createFromXaml(xaml);
        this.visual.Children.Remove(this.textBlock);
        this.textBlock = newTextBlock;
        this.visual.Children.Add(this.textBlock);
        this.fullText = this.textBlock.Text;
        this.prevWidth = 0;
        this.updateLayout();
    }
    catch (e) {
        SJ.log(e);
        SJ.log(xaml);
    }
}

SJ.TextBlock.prototype.measure = function (availWidth, availHeight) {
    // We are actually setting the width of the TextBlock, not just measuring! At some point
    // we need to just measure, but that will require creating a temporary TextBlock.
    if (this.hAlign == "stretch") {
        if (availWidth == Number.POSITIVE_INFINITY) {
            this.textBlock.Width = availWidth;
            this.desiredWidth = this.textBlock.ActualWidth;
        }
        else {
            this.textBlock.Width = availWidth;
            this.desiredWidth = availWidth;
        }
    }
    else if (this.hAlign == "grow") {
        this.textBlock.Width = this.textBlock.ActualWidth;
        this.desiredWidth = this.textBlock.ActualWidth;    
    }
    else {
        this.textBlock.Width = this.visual.Width;
        this.desiredWidth = this.visual.Width;
    }
    this.desiredHeight = this.textBlock.ActualHeight;
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.TextBlock.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);
    this.textBlock.Width = width;
    
    var realWidth = this.textBlock.ActualWidth;
    var realHeight = this.textBlock.ActualHeight;
    
    if (this.ellipsis && this.textBlock.ActualWidth > this.textBlock.Width &&
            this.visual.Width != this.prevWidth) {
        this.prevWidth = this.visual.Width;
        
        // Repeated text measurement is not working on Mac
        if (navigator.userAgent.indexOf('Macintosh') == -1) {
            var low = 0;
            var high = this.fullText.length;
            var x = 0;
            while (low < high) {
                if (x++ > 10) break;
                var length = Math.floor((low + high) / 2);
                this.textBlock.Text = this.fullText.substr(0, length) + "\u2026";
                if (this.textBlock.ActualWidth > this.textBlock.Width)
                    high = length;
                else
                    low = length + 1;
            }
            if (this.textBlock.ActualWidth > this.textBlock.Width)
                this.textBlock.Text = this.fullText.substr(0, length - 1) + "\u2026";

            realWidth = this.textBlock.ActualWidth;
        }
        else {
            // Instead of ellipsis, fade out on Mac
            this.textBlock.Clip = SJ.createFromXaml("<RectangleGeometry Rect='" +
                0 + "," + 0 + "," + width + "," + height + "' />");
            this.textBlock.Width = width;
            realWidth = width;
            /* Actually, the fade isn't working either
            this.visual.OpacityMask = SJ.createFromXaml("<LinearGradientBrush StartPoint='0.9,0' EndPoint='1,0' > \
                <GradientStop Color='#FF000000' Offset='0.0' /> \
                <GradientStop Color='#00000000' Offset='1.0' /> \
                </LinearGradientBrush>");
            */
        }
    }
    
    if (this.hAlign == "center")
        this.textBlock["Canvas.Left"] = (this.textBlock.Width - realWidth) / 2;
    else if (this.hAlign == "right")
        this.textBlock["Canvas.Left"] = this.textBlock.Width - realWidth;
    
    if (this.vAlign == "center")
        this.textBlock["Canvas.Top"] = (height - realHeight) / 2;
    else if (this.vAlign == "bottom")
        this.textBlock["Canvas.Top"] = height - realHeight;
}

// Hyperlink

SJ.Hyperlink = function (left, top, width, height, text, otherAttributes, url) {
    SJ.Control.call(this);
    
    this.fullText = text;
    this.url = url;
    this.state = "idle";
    
    if (SJ.wpfeControl) {
        var xaml =
            "<Canvas xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' x:Name='%top%'> \
                <TextBlock " + otherAttributes + ">" + text + "</TextBlock> \
             </Canvas>";
    
        // WPF/E bug: Certain non-ASCII results cause errors in createFromXaml
        try {
            this.visual = SJ.createFromXaml(xaml, this.names);
        }
        catch (e) {
            SJ.log(e);
            SJ.log(xaml);
            this.visual = SJ.createFromXaml("<Canvas><TextBlock /></Canvas>");
        }
    
        SJ.placeElement(this.visual, left, top, width, height);

        this.visual.Cursor = "Hand";
        
        this.textBlock = this.visual.Children.GetItem(0);

        this.hookUpEvent("MouseEnter");
        this.hookUpEvent("MouseLeave");
        this.hookUpEvent("MouseLeftButtonDown");
        this.hookUpEvent("MouseLeftButtonUp");
        
        this.doLayout();
    }
}

SJ.Hyperlink.prototype = new SJ.Control;

SJ.Hyperlink.prototype.toString = function () {
    return "SJ.Hyperlink";
}

SJ.Hyperlink.prototype.sizeChanged = function (width, height) {
    if (this.textBlock) {
        this.textBlock.Text = this.fullText;
        this.textBlock.Width = width;
        this.textBlock.Height = height;
    }
    SJ.Control.prototype.sizeChanged.call(this, width, height);
}

SJ.Hyperlink.prototype.updateLayout = function () {
    this.doLayout();
}

SJ.Hyperlink.prototype.doLayout = function () {
    if (this.textBlock) {
        this.textBlock.Width = this.visual.Width;
        this.visual.Height = this.textBlock.ActualHeight;
    }
}

SJ.Hyperlink.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);
    this.textBlock.Width = width;
    
    var realWidth = this.textBlock.ActualWidth;
    var realHeight = this.textBlock.ActualHeight;
    
    if (this.ellipsis && this.textBlock.ActualWidth > this.textBlock.Width &&
            this.visual.Width != this.prevWidth) {
        this.prevWidth = this.visual.Width;
        
        // Repeated text measurement is not working on Mac
        if (navigator.userAgent.indexOf('Macintosh') == -1) {
            var low = 0;
            var high = this.fullText.length;
            var x = 0;
            while (low < high) {
                if (x++ > 10) break;
                var length = Math.floor((low + high) / 2);
                this.textBlock.Text = this.fullText.substr(0, length) + "\u2026";
                if (this.textBlock.ActualWidth > this.textBlock.Width)
                    high = length;
                else
                    low = length + 1;
            }
            if (this.textBlock.ActualWidth > this.textBlock.Width)
                this.textBlock.Text = this.fullText.substr(0, length - 1) + "\u2026";

            realWidth = this.textBlock.ActualWidth;
        }
        else {
            // Instead of ellipsis, fade out on Mac
            this.textBlock.Clip = SJ.createFromXaml("<RectangleGeometry Rect='" +
                0 + "," + 0 + "," + width + "," + height + "' />");
            this.textBlock.Width = width;
            realWidth = width;
            /* Actually, the fade isn't working either
            this.visual.OpacityMask = SJ.createFromXaml("<LinearGradientBrush StartPoint='0.9,0' EndPoint='1,0' > \
                <GradientStop Color='#FF000000' Offset='0.0' /> \
                <GradientStop Color='#00000000' Offset='1.0' /> \
                </LinearGradientBrush>");
            */
        }
    }
    
    if (this.hAlign == "center")
        this.textBlock["Canvas.Left"] = (this.textBlock.Width - realWidth) / 2;
    else if (this.hAlign == "right")
        this.textBlock["Canvas.Left"] = this.textBlock.Width - realWidth;
    
    if (this.vAlign == "center")
        this.textBlock["Canvas.Top"] = (height - realHeight) / 2;
    else if (this.vAlign == "bottom")
        this.textBlock["Canvas.Top"] = height - realHeight;
}

// Set the state and change the visuals as needed.
// States = { idle, hover, activeDown, activeUp }

SJ.Hyperlink.prototype.setState = function (newState) {
    this.state = newState;
    
    switch (newState) {
        case "idle":
            this.textBlock.TextDecorations = "None";
            window.status = "";
            break;
        case "hover":
            this.textBlock.TextDecorations = "Underline";
            window.status = this.url;
            break;
        case "activeDown":
            this.textBlock.TextDecorations = "Underline";
            break;
        case "activeUp":
            this.textBlock.TextDecorations = "Underline";
            break;
    }
    
    this.doLayout();
}

SJ.Hyperlink.prototype.MouseEnter = function (sender, eventArgs) {
    if (this.state == "idle") {
        this.setState("hover");
    }
    else if (this.state == "activeUp") {
        this.setState("activeDown");
    }

    if (this.onMouseEnter)
        this.onMouseEnter(this, eventArgs);    
}

SJ.Hyperlink.prototype.MouseLeave = function (sender, eventArgs) {
    if (this.state == "activeDown") {
        this.setState("activeUp");
    }
    else {
        this.setState("idle");
    }

    if (this.onMouseLeave)
        this.onMouseLeave(this, eventArgs);    
}

SJ.Hyperlink.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        SJ.captureMouse(this.visual);
        
        this.setState("activeDown");
    }
}

SJ.Hyperlink.prototype.MouseLeftButtonUp = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        SJ.releaseMouseCapture();
        
        if (this.state == "activeDown") {
            this.setState("hover");
            if (this.url) {
                if (this.onNavigate) {
                    this.onNavigate(this,this.url);
                } 
                else {
                    SJ.openWindow(this.url, this.openWithName);
                }
            }
        }
        else {
            this.setState("idle");
        }
    }
}

// TextEdit

// deferVisible = true means you must call setInputVisible(true) after setParent
// to make the HTML INPUT element visible. This allows animations to take place
// before the INPUT is placed on top of the WPF/E control.

SJ.TextEdit = function (left, top, width, height, cssStyle, border, deferVisible, multiline) {
    SJ.Control.call(this);
    
    this.border = (border === undefined) ? true : border;
    this.cssStyle = cssStyle || "";
    this.deferVisible = !!deferVisible;
    this.multiline = !!multiline;
    
    if (SJ.wpfeControl) {
        var xaml = "<Canvas>";
        if (border)
            xaml += "<Rectangle StrokeThickness='1' Fill='#ffffff' Stroke='#d0d0d0' />";
        xaml += "</Canvas>";
        
        this.visual = SJ.createFromXaml(xaml);
        
        SJ.placeElement(this.visual, left, top, width, height);

        if (this.border)
            this.outline = this.visual.Children.GetItem(0);
    }
}

SJ.TextEdit.prototype = new SJ.Control;

SJ.TextEdit.prototype.toString = function () {
    return "SJ.TextEdit";
}

SJ.TextEdit.prototype.setParent = function (parent) {
    SJ.Control.prototype.setParent.call(this, parent);
    if (this.visual.getParent()) {
        this.doLayout();
    }
    else if (this.input) {
        document.body.removeChild(this.input);
        this.input = null;
    }
}

SJ.TextEdit.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);
    this.doLayout();
}

SJ.TextEdit.prototype.doLayout = function () {
    if (this.outline) {
        this.outline.Width = this.visual.Width;
        this.outline.Height = this.visual.Height;
    }
    if (!this.input) {
        if (this.multiline)
            this.input = document.createElement("textarea");
        else
            this.input = document.createElement("input");
        this.input.style.cssText = (this.border ? "border: 1px solid #ddd;" : "") + this.cssStyle;
        this.input.style.position = "absolute";
        if (this.deferVisible)
            this.input.style.display = "none";
        document.body.appendChild(this.input);
    }
    var browserPos = this.browserPosition();
    this.input.style.left = browserPos.left;
    this.input.style.top = browserPos.top;
    this.input.style.width = this.visual.Width;
    this.input.style.height = this.visual.Height;
}

SJ.TextEdit.prototype.getInputVisible = function () {
    return this.input ? this.input.style.display == "" : false;
}

SJ.TextEdit.prototype.setInputVisible = function (visible) {
    if (this.input)
        this.input.style.display = visible ? "" : "none";
}

SJ.TextEdit.prototype.getText = function () {
    if (this.input)
        return this.input.value;
    else
        return "";
}

SJ.TextEdit.prototype.setText = function (text) {
    if (this.input)
        this.input.value = text;
}

// styles = {attr: value}

SJ.TextEdit.prototype.setCssStyles = function (styles) {
    if (this.input) {
        for (var attr in styles)
            this.input.style[attr] = styles[attr];
    }
}

// LabelEdit
//
// LabelEdit is like TextEdit, but only uses an HTML INPUT element when it has 
// keyboard focus. At other times, it shows its value using a XAML TextBlock.
// Consequently, we can animate them without the text disappearing.

SJ.LabelEdit = function (left, top, width, height, helpText, border) {
    SJ.Control.call(this);

    if (!border)
        border = {left: 0, right: 0, top: 0, bottom: 0};
    
    // set a background to get events
    var xaml = "<Canvas Background='Transparent'></Canvas>";
    this.visual = SJ.createFromXaml(xaml);
    
    this.label = new SJ.TextBlock(left, top, width, height, '', "FontFamily='Courier New' FontSize='12.5'");
    this.label.ellipsis = true;
    this.labelBorder = new SJ.Border(border, "Cursor='IBeam' Background='White'");
    this.labelBorder.setContent(this.label);
    this.labelBorder.setParent(this.visual);

    SJ.placeElement(this.visual, left, top, width, height);
    SJ.placeElement(this.label.visual, border.left, border.top, width, height);
    SJ.placeElement(this.labelBorder.visual, 0, 0);
    
    this.hookUpEvent("MouseLeftButtonDown");
    this.hookUpEvent("MouseEnter", this.labelBorder.visual, "onBorderMouseEnter");
    this.hookUpEvent("MouseLeave", this.labelBorder.visual, "onBorderMouseLeave");

    this.border = border;
    this.helpText = helpText;
    this.onKeyPressed = null; // fired on key events
    this.onTextChanged = null; // fired on loss of focus or 'enter' key event
    this.onEnterKeyPressed = null; // fired on an 'enter' key event    
    this.onInputFocus = null;
    this.onInputBlur = null;
    
    this.setText('');
}

SJ.LabelEdit.prototype = new SJ.Control;

SJ.LabelEdit.prototype.toString = function () {
    return "SJ.LabelEdit";
}

SJ.LabelEdit.prototype.getText = function () {
    if (this.editLabel)
        return this.editLabel.getText();
    else
        return this.getLabelText();
}

SJ.LabelEdit.prototype.setText = function (text) {
    if (text.trim() == '') {
        this.label.hAlign = "center";
        this.label.otherAttributes = "FontFamily='Courier New' FontSize='13' FontStyle='Italic' Foreground='#444444'";
        this.label.setText(this.helpText);
    }
    else {
        this.label.hAlign = "left";
        this.label.otherAttributes = "FontFamily='Courier New' FontSize='13' FontStyle='Normal' Foreground='#000000'";
        this.label.setText(text);
    }
}

SJ.LabelEdit.prototype.setBorderBackgroundImage = function (images) {
    this.borderBackgroundImages = images;
    this.labelBorder.setBackgroundImage(images.idle);
}

SJ.LabelEdit.prototype.onBorderMouseEnter = function () {
    if (this.borderBackgroundImages)
        this.labelBorder.setBackgroundImage(this.borderBackgroundImages.hover);    
}

SJ.LabelEdit.prototype.onBorderMouseLeave = function () {
    if (this.borderBackgroundImages)
        this.labelBorder.setBackgroundImage(this.borderBackgroundImages.idle);
}

SJ.LabelEdit.prototype.hasInputFocus = function () {
    return this.editLabel != null;
}

SJ.LabelEdit.prototype.checkTextChange = function (usedEnterKey) {
    var newValue = this.editLabel.getText();
    if (this.getLabelText() != newValue) {
        this.setText(newValue);
        if (this.onTextChanged)
            this.onTextChanged(this, newValue);
    }
    if (usedEnterKey && this.onEnterKeyPressed)
        this.onEnterKeyPressed(this, newValue);
}

SJ.LabelEdit.prototype.getLabelText = function () {
    var text = this.label.getText();
    return (text != this.helpText) ? text : '';
}

SJ.LabelEdit.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        
        if (this.onInputFocus)
            this.onInputFocus(this, null);
            
        if (!this.editLabel) {
            this.editLabel = new SJ.TextEdit(this.border.left, this.border.top, this.label.getWidth(), this.label.getHeight()+4,
                "font: 10pt Courier New; border: 0; margin: -1 0 0 0;",
                false, false);
            this.editLabel.setParent(this.labelBorder);
            this.editLabel.doLayout();
            this.editLabel.input.onkeydown = SJ.methodCaller(this, "onInputKeyPress");
            this.editLabel.input.onblur = SJ.methodCaller(this, "onInputBlurred");
        }
        
        this.editLabel.setText( this.getLabelText() );
        this.editLabel.setInputVisible(true);
        this.editLabel.input.focus();
        this.editLabel.input.select();
        
        this.label.setParent(null);
    }
}

SJ.LabelEdit.prototype.onInputKeyPress = function (e) {
	var code;
	if (!e) var e = window.event;
	if (e.keyCode) code = e.keyCode;
	else if (e.which) code = e.which;
	this.lastKeyCode = code;
	if (this.onKeyPressed)
	    this.onKeyPressed(this, code);
    if (code == 13) // enter key
        this.editLabel.input.blur();
}

SJ.LabelEdit.prototype.onInputBlurred = function () {
    this.checkTextChange(this.lastKeyCode == 13);
    this.label.setParent(this.labelBorder);
    this.editLabel.setInputVisible(false);
    this.editLabel.setParent(null);
    this.editLabel = null;
    
    if (this.onInputBlur)
        this.onInputBlur(this, null);    
}

// StackPanel
//
// To optimize performance when items are being added to a stack over time (as in
// ListView), the StackPanel assumes that once a child is placed, it doesn't need
// to look at it again. That is, when measure/arrange happens, the children that
// were there the last time are not processed. If the size of a child changes, or
// if children come and go, invalidateLayout() must be called to tell the StackPanel
// to redo the entire layout on the next measure/arrange pass.

SJ.StackPanel = function (left, top, isVertical) {
    SJ.Control.call(this);
    this.isVertical = !!isVertical;
    
    this.placedChildren = 0;
    
    if (SJ.wpfeControl) {
        var xaml = '<Canvas \
              xmlns="http://schemas.microsoft.com/client/2007" \
              xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" \
              x:Name="%top%"><Canvas.Resources/></Canvas>';
    
        this.visual = SJ.createFromXaml(xaml, this.names);
    
        SJ.placeElement(this.visual, left, top);
    }
}

SJ.StackPanel.prototype = new SJ.Control;

SJ.StackPanel.prototype.toString = function () {
    return "SJ.StackPanel";
}

SJ.StackPanel.prototype.sizeChanged = function (width, height) {
    this.invalidateLayout();
    if (this.isVertical) {
        for (var i = 0; i < this.children.length; i++) {
            var child = this.children[i];
            var margin = child.marginWidth();
            child.setWidth(width - margin);
        }
    }
    else {
        for (var i = 0; i < this.children.length; i++) {
            var child = this.children[i];
            var margin = child.marginHeight();
            child.setHeight(height - margin);
        }
    }
    // SJ.Control.prototype.sizeChanged.call(this,width,height);
}

SJ.StackPanel.prototype.invalidateLayout = function () {
    this.placedChildren = 0;
    for (var i = 0; i < this.children.length; i++) {
        this.children[i].invalidateLayout();
    }    
}

SJ.StackPanel.prototype.measure = function (availWidth, availHeight) {
    var x = 0;
    var y = 0;
    var width = 0;
    var height = 0;
    
    // If this.placedChildren is set, assume the first N children are fine already.
    
    if (this.placedChildren > 0) {
        if (this.isVertical) {
            width = this.cachedWidth;
            y = this.cachedHeight;
        }
        else {
            height = this.cachedHeight;
            x = this.cachedWidth;
        }
    }
    
    for (var i = this.placedChildren; i < this.children.length; i++) {
        var child = this.children[i];
        var margin = child.margin;
        
        if (this.isVertical) {
            var adjustedAvailWidth = availWidth;
            if (margin) {
                if (margin.left)
                    adjustedAvailWidth -= margin.left;
                if (margin.right)
                    adjustedAvailWidth -= margin.right;
            }
            
            child.measure(Math.max(0, adjustedAvailWidth), Number.POSITIVE_INFINITY);
            
            var adjustedWidth = child.desiredWidth;
            if (margin)
                adjustedWidth += (margin.left || 0) + (margin.right || 0);
            width = Math.max(width, adjustedWidth);
            
            if (margin && margin.top)
                y += margin.top;
            y += child.desiredHeight;
            if (margin && margin.bottom)
                y += margin.bottom;
        }
        else {
            var adjustedAvailHeight = availHeight;
            if (margin) {
                if (margin.top)
                    adjustedAvailHeight -= margin.top;
                if (margin.bottom)
                    adjustedAvailHeight -= margin.bottom;
            }
            
            child.measure(Number.POSITIVE_INFINITY, Math.max(0, adjustedAvailHeight));
            
            var adjustedHeight = child.desiredHeight;
            if (margin)
                adjustedHeight += (margin.top || 0) + (margin.bottom || 0);
            height = Math.max(height, adjustedHeight);
            
            if (margin && margin.left)
                x += margin.left;
            x += child.desiredWidth;
            if (margin && margin.right)
                x += margin.right;
        }
    }
    
    if (this.isVertical) {
        this.desiredWidth = width;
        this.desiredHeight = y;
    }
    else {
        this.desiredWidth = x;
        this.desiredHeight = height;
    }
    
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.StackPanel.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);
    
    var x = 0;
    var y = 0;
    
    // If this.placedChildren is set, assume the first N children are fine already.
    
    if (this.placedChildren > 0) {
        if (this.isVertical)
            y = this.cachedHeight;
        else
            x = this.cachedWidth;
    }
    
    for (var i = this.placedChildren; i < this.children.length; i++) {
        var child = this.children[i];
        var margin = child.margin;
        var childX;
        var childY;
        
        if (child.hAlign == "right") {
            childX = x + width - child.desiredWidth;
            if (margin && margin.right)
                childX -= margin.right;
        }
        else if (child.hAlign == "center") {
            var adjChildWidth = child.desiredWidth;
            if (margin)
                adjChildWidth += (margin.left || 0) + (margin.right || 0);
            childX = x + (width - adjChildWidth) / 2;
            if (margin && margin.left)
                childX += margin.left;
        }
        else {
            childX = x;
            if (margin && margin.left)
                childX += margin.left;
        }
        
        if (child.vAlign == "bottom") {
            childY = y + height - child.desiredHeight;
            if (margin && margin.bottom)
                childY -= margin.bottom;
        }
        else if (child.vAlign == "center") {
            var adjChildHeight = child.desiredHeight;
            if (margin)
                adjChildHeight += (margin.top || 0) + (margin.bottom || 0);
            childY = y + (height - adjChildHeight) / 2;
        }
        else {
            childY = y;
            if (margin && margin.top)
                childY += margin.top;
        }

        child.arrange(childX, childY, child.desiredWidth, child.desiredHeight);
        
        if (this.isVertical) {
            y += child.desiredHeight;
            if (margin)
                y += (margin.top || 0) + (margin.bottom || 0);
        }
        else {
            x += child.desiredWidth;
            if (margin)
                 x += (margin.left || 0) + (margin.right || 0);
        }
    }
    
    this.placedChildren = this.children.length;
    
    this.cachedWidth = width;
    this.cachedHeight = height;
}

// FlowPanel

SJ.FlowPanel = function (left, top, width, height, rowAlign, defaultXaml) {
    SJ.Control.call(this);
    this.rowAlign = (rowAlign === undefined) ? "left" : rowAlign;
    this.placedChildren = 0;
    
    if (SJ.wpfeControl) {
        var xaml = defaultXaml || "<Canvas/>";
    
        this.visual = SJ.createFromXaml(xaml);
        
        SJ.placeElement(this.visual, left, top, width, height);
    }
}

SJ.FlowPanel.prototype = new SJ.Control;

SJ.FlowPanel.prototype.toString = function () {
    return "SJ.FlowPanel";
}

SJ.FlowPanel.prototype.sizeChanged = function (width, height) {
    this.invalidateLayout();
}

SJ.FlowPanel.prototype.invalidateLayout = function () {
    this.placedChildren = 0;
}

SJ.FlowPanel.prototype.measure = function (availWidth, availHeight) {
    var x = 0;
    var y = 0;
    var width = 0;
    var height = 0;
    var row = 0;
    var rowWidth = 0;
    var rowHeight = 0;
    this.rowHeights = [];
    this.rowStarts = [];
    
    if (this.placedChildren > 0) {
        // Start over on the last row
        row = this.cachedRow;
        y = this.cachedY;
        width = this.cachedWidth;
        height = y;
    }
    
    for (var i = this.placedChildren; i < this.children.length; i++) {
        var child = this.children[i];
        var margin = child.margin;
        
        child.measure(Number.POSITIVE_INFINITY, Number.POSITIVE_INFINITY);
        
        var adjustedHeight = child.desiredHeight;
        if (margin)
            adjustedHeight += (margin.top || 0) + (margin.bottom || 0);
        var adjustedWidth = child.desiredWidth;
        if (margin)
            adjustedWidth += (margin.left || 0) + (margin.right || 0);
        
        if (x > 0 && (x + adjustedWidth > availWidth)) {
            // Put this item on next row
            x = adjustedWidth;
            width = Math.max(width, adjustedWidth);
            rowWidth = adjustedWidth;
            y += this.rowHeights[row];
            row++;
            this.rowHeights[row] = adjustedHeight;
        }
        else {
            // Put this item on the current row
            x += adjustedWidth;
            width = Math.max(width, x);
            rowWidth = Math.max(rowWidth, x);
            this.rowHeights[row] = Math.max(this.rowHeights[row] || 0, adjustedHeight);
        }
        
        height = Math.max(height, y + adjustedHeight);

        if (this.rowAlign == "left")
            this.rowStarts[row] = 0;
        else if (this.rowAlign == "center")
            this.rowStarts[row] = Math.max(0, (availWidth - rowWidth) / 2);
        else if (this.rowAlign == "right")
            this.rowStarts[row] = Math.max(0, availWidth - rowWidth);
    }
    
    this.desiredWidth = Math.min(width, availWidth);
    this.desiredHeight = height;
    
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.FlowPanel.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);
    
    if (this.children.length == 0)
        return;
    
    var x = 0;
    var y = 0;
    var row = 0;

    if (this.placedChildren > 0) {
        x = 0;
        y = this.cachedY;
        row = this.cachedRow;
    }
    
    for (var i = this.placedChildren; i < this.children.length; i++) {
        var child = this.children[i];
        var margin = child.margin;
        var childX;
        var childY;

        var adjustedHeight = child.desiredHeight;
        if (margin)
            adjustedHeight += (margin.top || 0) + (margin.bottom || 0);
        var adjustedWidth = child.desiredWidth;
        if (margin)
            adjustedWidth += (margin.left || 0) + (margin.right || 0);
        
        if (x > 0 && (x + adjustedWidth > width)) {
            // Move to next row
            y += this.rowHeights[row];
            row++;
            x = 0;
            this.placedChildren = i;
        }

        childX = x;
        if (margin && margin.left)
            childX += margin.left;
        
        if (child.vAlign == "bottom") {
            childY = y + this.rowHeights[row] - child.desiredHeight;
            if (margin && margin.bottom)
                childY -= margin.bottom;
        }
        else if (child.vAlign == "center") {
            var adjChildHeight = child.desiredHeight;
            if (margin)
                adjChildHeight += (margin.top || 0) + (margin.bottom || 0);
            childY = y + (this.rowHeights[row] - adjChildHeight) / 2;
        }
        else {
            childY = y;
            if (margin && margin.top)
                childY += margin.top;
        }

        child.arrange(childX + this.rowStarts[row], childY, child.desiredWidth, child.desiredHeight);
        
        x += child.desiredWidth;
        if (margin)
             x += (margin.left || 0) + (margin.right || 0);
    }

    this.cachedRow = row;
    this.cachedY = y;
    this.cachedWidth = width;
}

// ListView

SJ.ListView = function (left, top, width, height, panel, isVertical) {
    this.isVertical = (isVertical === undefined) ? true : isVertical;
    
    SJ.Control.call(this);
    
    if (SJ.wpfeControl) {
        var xaml = "<Canvas />";
        this.visual = SJ.createFromXaml(xaml);
    
        SJ.placeElement(this.visual, left, top, width, height);
    
        this.scroller = new SJ.ScrollViewer(0, 0, width, height, this.isVertical, !this.isVertical);
        this.scroller.setParent(this);
        this.stack = panel || new SJ.StackPanel(0, 0, true);
        this.scroller.setContent(this.stack);
        this.listChanged();
    }
}

SJ.ListView.prototype = new SJ.Control;

SJ.ListView.prototype.toString = function () {
    return "SJ.ListView";
}

SJ.ListView.prototype.hideScrollbar = function () {
    this.stack.setParent(this);
    this.scroller.setParent(null);
}

SJ.ListView.prototype.updateLayout = function () {
    this.scroller.updateLayout();
}

SJ.ListView.prototype.sizeChanged = function (width, height) {
    this.scroller.resize(width,height);
}

SJ.ListView.prototype.addItem = function (item) {
    item.setWidth(this.getWidth());
    item.setParent(this.stack);
    this.listChanged();
}

SJ.ListView.prototype.removeItem = function (item) {
    item.setParent(null);
    this.stack.invalidateLayout();
    this.listChanged();
}

SJ.ListView.prototype.clearItems = function () {
    var children = this.stack.children;
    while (children.length > 0) {
        children[0].dispose();      // note that dispose removes the child
    }
    this.stack.invalidateLayout();
    this.updateLayout();
    this.scroller.setPosition(0, 0);
}

SJ.ListView.prototype.listChanged = function (item) {
    this.updateLayout();
}

SJ.ListView.prototype.itemCount = function () {
    return this.stack.children.length;
}

SJ.ListView.prototype.itemAt = function (n) {
    return this.stack.children[n];
}

SJ.ListView.prototype.scrollToBottom = function () {
    this.scroller.vScrollbar.setValue(this.scroller.vScrollbar.maxValue);
}

SJ.ListView.prototype.visibleItems = function () {
    var top = - this.scroller.positionY;
    var bottom = top + this.scroller.getHeight();
    var items = [];
    for (var i = 0, n = this.stack.children.length; i < n; i++) {
        var childVisual = this.stack.children[i].visual;
        var childTop = childVisual["Canvas.Top"];
        if (childTop >= bottom)
            break;
        if (childTop < bottom &&
                (childTop + childVisual.Height) >= top) {
            items.push(this.stack.children[i]);
        }
    }
    return items;
}

SJ.ListView.prototype.lineUp = function () {
    this.scroller.lineUp();
}

SJ.ListView.prototype.lineDown = function () {
    this.scroller.lineDown();
}

SJ.ListView.prototype.pageUp = function () {
    this.scroller.pageUp();
}

SJ.ListView.prototype.pageDown = function () {
    this.scroller.pageDown();
}

// AnimatePanel

SJ.AnimatePanel = function (left, top) {
    SJ.Control.call(this);
    
    if (SJ.wpfeControl) {
        var xaml =
            '<Canvas \
                  xmlns="http://schemas.microsoft.com/client/2007" \
                  xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"> \
               <Canvas.RenderTransform> \
                <TransformGroup> \
                 <ScaleTransform x:Name="%scale%" ScaleX="1" ScaleY="1" /> \
                 <TranslateTransform x:Name="%translate%" X="0" Y="0" /> \
                </TransformGroup> \
               </Canvas.RenderTransform> \
               <Canvas.Resources /> \
            </Canvas>';
        
        this.visual = SJ.createFromXaml(xaml, this.names);
    
        SJ.placeElement(this.visual, left, top);
    }
}

SJ.AnimatePanel.prototype = new SJ.Control;

SJ.AnimatePanel.prototype.toString = function () {
    return "SJ.AnimatePanel";
}

SJ.AnimatePanel.prototype.invalidateLayout = function () {
    if (this.content) {
        this.content.invalidateLayout();
    }
}

SJ.AnimatePanel.prototype.sizeChanged = function (width, height) {
    if (this.content) {
        this.content.resize(width, height);
    }
    SJ.Control.prototype.sizeChanged.call(this,width,height);
}

SJ.AnimatePanel.prototype.setContent = function (content) {
    if (this.content != null) {
        this.content.dispose();
    }
    
    this.content = content;
    this.content.setParent(this);
    
    this.updateLayout();
}

SJ.AnimatePanel.prototype.measure = function (availWidth, availHeight) {
    if (this.content) {
        this.content.measure(availWidth, availHeight);
        this.desiredWidth = this.content.desiredWidth;
        this.desiredHeight = this.content.desiredHeight;
    }
    else {
        this.desiredWidth = 0;
        this.desiredHeight = 0;
    }
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.AnimatePanel.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);

    if (this.content) {
        this.content.arrange(0, 0, this.content.desiredWidth, this.content.desiredHeight);
    }
    
    var scaleAnimation = SJ.findElement(this.names["scale"]);
    if (scaleAnimation) {
        scaleAnimation.CenterX = width / 2;
        scaleAnimation.CenterY = height / 2;
    }
}

// DialogBox

SJ.DialogBox = function (left, top) {
    SJ.AnimatePanel.call(this, left, top);
    
    if (SJ.wpfeControl) {
        var xaml =
            '<Rectangle Canvas.Left="-10" Canvas.Top="-10" RadiusX="10" RadiusY="10"> \
                <Rectangle.Fill> \
                    <LinearGradientBrush StartPoint="1,0" EndPoint="0,1" > \
                        <GradientStop Color="#20000000" Offset="0" /> \
                        <GradientStop Color="#80000000" Offset="1" /> \
                    </LinearGradientBrush> \
                </Rectangle.Fill> \
            </Rectangle>';
        this.shadow = SJ.createFromXaml(xaml);
        this.visual.Children.Add(this.shadow);
        
        this.setAnimations([
            '<Storyboard x:Name="%animateIn%"> \
                  <DoubleAnimation \
                     Storyboard.TargetName="%translate%" \
                     Storyboard.TargetProperty="Y" \
                     BeginTime="0:0:0" From="-1000" To="0"   \
                     Duration="0:0:0.5" AutoReverse="false" /> \
                </Storyboard>',
            '<Storyboard x:Name="%animateOut%"> \
                <DoubleAnimation \
                   Storyboard.TargetName="%translate%" \
                   Storyboard.TargetProperty="Y" \
                   BeginTime="0:0:0" From="0" To="-1000"   \
                   Duration="0:0:0.5" AutoReverse="false" /> \
                </Storyboard>'
        ]);
    }
}

SJ.DialogBox.prototype = new SJ.AnimatePanel;

SJ.DialogBox.prototype.toString = function () {
    return "SJ.DialogBox";
}

SJ.DialogBox.prototype.arrange = function (left, top, width, height) {
    SJ.AnimatePanel.prototype.arrange.call(this, left, top, width, height);

    this.shadow.Width = width + 20;
    this.shadow.Height = height + 20;
}

SJ.DialogBox.prototype.situate = function (place) {
    var translater = SJ.findElement(this.names["translate"]);
    switch (place) {
        case "in":
            translater.Y = 0;
            break;
        case "out":
            translater.Y = -1000;
            break;
    }
}


SJ.DialogBox.prototype.center = function () {
    var screenWidth = SJ.wpfeControl.content.actualWidth;
    var width = this.getWidth();
    var left = (screenWidth - width)/2;
    this.setLeft(left);
}

SJ.DialogBox.prototype.show = function () {
    this.disabler = SJ.createFromXaml("<Rectangle Fill='#60000000' Width='10000' Height='10000' />");
    SJ.topCanvas.Children.Add(this.disabler);
    this.setParent(SJ.topCanvas);
    this.situate("out");
    this.animate("animateIn", SJ.methodCaller(this, "showTextEdits"));
}

SJ.DialogBox.prototype.close = function () {
    this.hideTextEdits();
    var me = this;
    this.animate("animateOut",
        function () {
            me.dispose();
            SJ.topCanvas.Children.Remove(me.disabler);
        });
}

// Carousel

// The carousel's animations assume 1.5 seconds for a 360 degree turn. The reason
// for this is that they are synchronized with the search result animations, which
// take .3 seconds. Since we want the animation between each of the 5 items on the 
// carousel to take .3 seconds, we need a 1.5 second animation. 
//
// Initially, I tried to not hard-code this by assuming that a 360 degree turn of
// the carousel would always take 1 second, and then use a <Storyboard>'s SpeedRatio 
// and Duration to alter the speed. Unfortunately, SpeedRatio screws up the end
// position of an element -- instead of keeping the element at the position it
// has at the end of the animation, it seems to jump to the position it would have
// if SpeedRatio=1. One workaround to this would be dynamically calculate the
// animation's duration and then update all the <KeyTime> elements for each 
// <DoubleAnimationUsingKeyFrames>. Having spent a lot of time on this already, I
// opted for the simple solution. 

SJ.CarouselPanel = function (left, top, width, height) {
    SJ.Control.call(this);
    
    var xaml = 
        "<Canvas \
            xmlns='http://schemas.microsoft.com/client/2007' \
            xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' \
            x:Name='%name%' > \
            <Image Source='images/Carousel.png' /> \
            <Canvas.RenderTransform> \
                <TranslateTransform x:Name='%translate%' /> \
            </Canvas.RenderTransform> \
            <Canvas.Resources> \
                <Storyboard x:Name='%fadeIn%'> \
                    <DoubleAnimation \
                         Storyboard.TargetName='%name%' \
                         Storyboard.TargetProperty='Opacity' \
                         From='0' To='1' Duration='0:0:0.25' /> \
                </Storyboard> \
            </Canvas.Resources> \
        </Canvas>";
    
    this.names = {};
    xaml = SJ.generateUniqueNames(xaml, this.names);
    
    this.visual = SJ.createFromXaml(xaml);
    this.platform = this.visual.Children.GetItem(0); // ellipse
    
    SJ.placeElement(this.visual,left,top,width,height);    
    
    this.hookUpEvent("MouseLeftButtonDown");

    this.images = [];
    this.imageNames = [];
    this.selection = 0; // currently "in front"
    this.target = 0;    // desired to be "in front" (we are animating toward this value if it's different than this.selection)
    this.direction = 0; // 0=clockwise; 1=counterclockwise 
}

SJ.CarouselPanel.prototype = new SJ.Control;

SJ.CarouselPanel.prototype.toString = function () {
    return "SJ.CarouselPanel";
}

SJ.CarouselPanel.prototype.setContent = function (content) {
    if (this.content != null) {
        this.content.dispose();
    }
    
    this.content = content;
    this.content.setParent(this);
    
    this.updateLayout();
}

SJ.CarouselPanel.prototype.MouseLeftButtonDown = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        if (eventArgs.getPosition(this.visual).x > (this.visual.Width/2))
            this.animateClockwise();
        else
            this.animateCounterclockwise();
    }
}

SJ.CarouselPanel.prototype.measure = function (availWidth, availHeight) {
    this.desiredWidth = this.visual.Width;
    this.desiredHeight = this.visual.Height;
    SJ_logCall("measure", this, this.desiredWidth, this.desiredHeight);
}

SJ.CarouselPanel.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);

    this.platform.Width = width;
    this.platform.Height = height;

    // resize the animation elements
    this.resizeAnimation(width, height);
    
    // position images in the timeline
    this.positionImages();
    
    // run the animation for a bit so that WPF/E sets the Top/Left of each image
    this.animateTurn(0.001);    

    if (this.content) {
        this.content.arrange(0, 0, this.content.desiredWidth, this.content.desiredHeight);
    }
}

SJ.CarouselPanel.prototype.positionImages = function () {

    // Images are positioned in a clockwise manner. Position 0 corresponds to 6 o'clock 
    // and increases clockwise to 1.5 as in:
    //  (back of carrousel)
    //        0.5
    //    0.25   0.75
    //         0 (or 1.5)
    // (front of carrousel)

    // positional offset between images
    var offset = 1.5 / this.images.length;
    
    // The currently "selected" image goes at position 0 (aka 1.5) and 
    // subsequent images are placed counter-clockwise if the direction is
    // clockwise (and clockwise if the direction is counterclockwise).
    if (this.direction == 0) {
        for (var i = 0; i < this.images.length; i++) {
            var n = (this.selection + i) % this.images.length;
            var pos = 1.5 - (i * offset);
            this.positionImage(n, pos);
        }
    } else {
        for (var i = 0; i < this.images.length; i++) {
            var n = (this.selection - i + this.images.length) % this.images.length;
            var pos = 1.5 - (i * offset);
            this.positionImage(n, pos);
        }    
    }
}

SJ.CarouselPanel.prototype.positionImage = function (imageIndex, position) {
    position = Math.round(position * 1000) / 1000; // at most 3 digits after the decimal so WPF/E doesn't complain
    SJ.findElement(this.images[imageIndex].names["animateX"]).BeginTime = "-0:0:" + position;
    SJ.findElement(this.images[imageIndex].names["animateY"]).BeginTime = "-0:0:" + position;
    SJ.findElement(this.images[imageIndex].names["animateScaleX"]).BeginTime = "-0:0:" + position;
    SJ.findElement(this.images[imageIndex].names["animateScaleY"]).BeginTime = "-0:0:" + position;
}

SJ.CarouselPanel.prototype.resizeAnimation = function (width, height) {
    // set the keyframe values based on the width & height of our visual
    for (var i = 0; i < this.images.length; i++) {
        var animateX = SJ.findElement(this.images[i].names["animateX"]);
        if (this.direction == 0) {
            animateX.KeyFrames.GetItem(0).Value = width / 2;
            animateX.KeyFrames.GetItem(1).Value = 20;
            animateX.KeyFrames.GetItem(2).Value = width / 2 - 16;
            animateX.KeyFrames.GetItem(3).Value = width / 2 + 16;
            animateX.KeyFrames.GetItem(4).Value = width - 16;
            animateX.KeyFrames.GetItem(5).Value = width / 2;
        }
        else {
            animateX.KeyFrames.GetItem(0).Value = width / 2;
            animateX.KeyFrames.GetItem(1).Value = width - 16;
            animateX.KeyFrames.GetItem(2).Value = width / 2 + 16;
            animateX.KeyFrames.GetItem(3).Value = width / 2 - 16;
            animateX.KeyFrames.GetItem(4).Value = 20;
            animateX.KeyFrames.GetItem(5).Value = width / 2;
        }
        
        var animateY = SJ.findElement(this.images[i].names["animateY"]);
        animateY.KeyFrames.GetItem(0).Value = height - 10;
        animateY.KeyFrames.GetItem(1).Value = (height / 2) - 10;
        animateY.KeyFrames.GetItem(2).Value = 15;
        animateY.KeyFrames.GetItem(3).Value = 15;
        animateY.KeyFrames.GetItem(4).Value = (height / 2) - 10;
        animateY.KeyFrames.GetItem(5).Value = height - 10;
    }        
}

SJ.CarouselPanel.prototype.flipDirection = function () {

    // WPF/E doesn't support running an animation in reverse (AutoReverse doesn't cut it).
    // As a workground, we set up the X coords of the animation based on the current direction.
    
    this.direction = this.direction ? 0 : 1;

    this.resizeAnimation(this.visual.Width, this.visual.Height);
}

SJ.CarouselPanel.prototype.reset = function () {
    this.selection = 0;
    this.target = 0;
    this.updateLayout();
}

SJ.CarouselPanel.prototype.animateClockwise = function () {
    this.target++;
    this.target %= this.images.length;
    this.turnTowardTarget();
}

SJ.CarouselPanel.prototype.animateCounterclockwise = function () {
    this.target += this.images.length - 1;
    this.target %= this.images.length;
    this.turnTowardTarget();
}

SJ.CarouselPanel.prototype.turnTowardTarget = function () {
    if (this.target != this.selection) {
        // Determine the best direction to turn the carousel
        var clockwiseOffset = (this.target + this.images.length - this.selection) % this.images.length;
        var turnClockwise = clockwiseOffset < this.images.length/2;
        if (turnClockwise)
            this.animateClockwiseCore();
        else
            this.animateCounterclockwiseCore();
    }
}

SJ.CarouselPanel.prototype.animateClockwiseCore = function () {

    if (this.direction == 1) {
        this.flipDirection();
    }

    // update position based on current selection
    this.positionImages();

    // It takes 1.5 seconds to animate the carrousel 360 degrees; thus to turn 1 image, run the animation 1.5 / (# of images) seconds
    var duration = 1.5 / this.images.length;
    this.animateTurn(duration);

    this.selection++;
    this.selection %= this.images.length;
    
    if (this.onSelectionChanged && !this.unveiling)
        this.onSelectionChanged(this, {direction: "cw", selection: this.selection});
}

SJ.CarouselPanel.prototype.animateCounterclockwiseCore = function () {

    if (this.direction == 0) {
        this.flipDirection();
    }
    
    // update position based on current selection
    this.positionImages();

    // It takes 1.5 seconds to animate the carrousel 360 degrees; thus to turn 1 image, run the animation 1.5 / (# of images) seconds
    var duration = 1.5 / this.images.length;
    this.animateTurn(duration);

    this.selection += this.images.length - 1;
    this.selection %= this.images.length;
    
    if (this.onSelectionChanged && !this.unveiling)
        this.onSelectionChanged(this, {direction: "ccw", selection: this.selection});
}

SJ.CarouselPanel.prototype.animateTurn = function (duration) {
    duration = Math.round(duration * 1000) / 1000; // only 3 decimal places to keep WPF/E happy
    for (var i = 0; i < this.images.length; i++) {
        var image = this.images[i];
        var storyBoard = SJ.findElement(image.names["storyboard"]);
        storyBoard["Duration"] = "0:0:" + duration; // assumes duration <= 60 seconds
        storyBoard.Begin();
    }
}

SJ.CarouselPanel.prototype.onStoryboardCompleted = function (sender, args) {
    // Supports multiple turns of the carousel. When an animation completes, we 
    // start another one if we haven't yet reached our target.
    this.turnTowardTarget();
}

// Unveils the carousel for the first time by revealing images while the carousel is turning

SJ.CarouselPanel.prototype.unveil = function (onCompleted) {

    // hide all images
    for (var i = 0; i < this.images.length; i++) {
        var image = this.images[i];
        image.element.Opacity = 0;
    }

    // setup our multi-part animation
    this.unveiling = {} 
    this.unveiling.next = 0;
    this.unveiling.callback = SJ.methodCaller(this, "unveilNextSpinStep");
    this.unveiling.storyboard = this.images[0].element.resources.GetItem(0);
    this.unveiling.eventToken = this.unveiling.storyboard.addEventListener("Completed", this.unveiling.callback);
    this.unveiling.onCompleted = onCompleted;
    
    this.animate("fadeIn", this.unveiling.callback);
}

SJ.CarouselPanel.prototype.unveilNextSpinStep = function () {
    if (!this.unveiling)
        return;
        
    if (this.unveiling.next == this.images.length) {
        // cleanup
        var onCompleted = this.unveiling.onCompleted;
        this.unveiling.storyboard.removeEventListener("Completed", this.unveiling.eventToken);
        this.unveiling = null;
        delete this.unveiling;
        if (onCompleted)
            onCompleted(this, null);
    }
    else {
        // unhide the next image
        var image = this.images[this.unveiling.next];
        image.element.Opacity = 1;
        
        this.unveiling.next++;

        // animate one turn
        this.animateClockwise();
    }
}

SJ.CarouselPanel.prototype.addImage = function (imagePath, imageWidth, imageHeight, imageName) {
   // WPF/E BUG: setting Image.Source doesn't seem to work in IE7. Stick it in the xaml for now.
   var xaml =
      '<Image x:Name="%name%" \
              Source="' + imagePath + '" \
              xmlns="http://schemas.microsoft.com/client/2007" \
              xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml" \
              Canvas.Left="-26" Canvas.Top="-52"> \
        <Image.RenderTransform> \
            <TransformGroup> \
                <ScaleTransform Name="%scale%" ScaleX="1" ScaleY="1" /> \
                <TranslateTransform Name="%translate%" X="0" Y="0" /> \
            </TransformGroup> \
        </Image.RenderTransform> \
        <Image.Resources> \
            <Storyboard x:Name="%storyboard%"> \
                <DoubleAnimationUsingKeyFrames \
                  x:Name="%animateX%" \
                  Storyboard.TargetName="%translate%" \
                  Storyboard.TargetProperty="X" \
                  RepeatBehavior="Forever" \
                  > \
                  <SplineDoubleKeyFrame KeyTime="0:0:0.00" KeySpline="0.5,0.25 0.5,0.25" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:0.33" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:0.68" KeySpline="0.5,0.25 0.5,0.25" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:0.83" KeySpline="0.5,0.25 0.5,0.25" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:1.17" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:1.50" KeySpline="0.5,0.25 0.5,0.25" /> \
                </DoubleAnimationUsingKeyFrames> \
                <DoubleAnimationUsingKeyFrames \
                  x:Name="%animateY%" \
                  Storyboard.TargetName="%translate%" \
                  Storyboard.TargetProperty="Y" \
                  RepeatBehavior="Forever" \
                  > \
                  <SplineDoubleKeyFrame KeyTime="0:0:0.00" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:0.33" KeySpline="0.5,0.25 0.5,0.25" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:0.68" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:0.83" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:1.17" KeySpline="0.5,0.25 0.5,0.25" /> \
                  <SplineDoubleKeyFrame KeyTime="0:0:1.50" KeySpline="0.5,0.75 0.5,0.75" /> \
                </DoubleAnimationUsingKeyFrames> \
                <DoubleAnimationUsingKeyFrames \
                  x:Name="%animateScaleX%" \
                  Storyboard.TargetName="%scale%" \
                  Storyboard.TargetProperty="ScaleX" \
                  RepeatBehavior="Forever" \
                  > \
                  <SplineDoubleKeyFrame Value="1"    KeyTime="0:0:0.00" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame Value="0.95" KeyTime="0:0:0.33" KeySpline="0.5,0.25 0.5,0.25" /> \
                  <SplineDoubleKeyFrame Value="0.90" KeyTime="0:0:0.68" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame Value="0.90" KeyTime="0:0:0.83" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame Value="0.95" KeyTime="0:0:1.17" KeySpline="0.5,0.25 0.5,0.25" /> \
                  <SplineDoubleKeyFrame Value="1"    KeyTime="0:0:1.50" KeySpline="0.5,0.75 0.5,0.75" /> \
                </DoubleAnimationUsingKeyFrames> \
                <DoubleAnimationUsingKeyFrames \
                  x:Name="%animateScaleY%" \
                  Storyboard.TargetName="%scale%" \
                  Storyboard.TargetProperty="ScaleY" \
                  RepeatBehavior="Forever" \
                  > \
                  <SplineDoubleKeyFrame Value="1"    KeyTime="0:0:0.00" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame Value="0.95" KeyTime="0:0:0.33" KeySpline="0.5,0.25 0.5,0.25" /> \
                  <SplineDoubleKeyFrame Value="0.90" KeyTime="0:0:0.68" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame Value="0.90" KeyTime="0:0:0.83" KeySpline="0.5,0.75 0.5,0.75" /> \
                  <SplineDoubleKeyFrame Value="0.95" KeyTime="0:0:1.17" KeySpline="0.5,0.25 0.5,0.25" /> \
                  <SplineDoubleKeyFrame Value="1"    KeyTime="0:0:1.50" KeySpline="0.5,0.75 0.5,0.75" /> \
                </DoubleAnimationUsingKeyFrames> \
            </Storyboard> \
        </Image.Resources> \
      </Image>';

    var names = {};
    xaml = SJ.generateUniqueNames(xaml, names);
    
    var element = SJ.createFromXaml(xaml);
    this.visual.Children.Add(element);
    
    this.hookUpEvent("MouseLeftButtonDown", element, "onImageClick");
    this.hookUpEvent("MouseEnter", element, "onImageMouseEnter");
    this.hookUpEvent("MouseLeave", element, "onImageMouseLeave");
    
    // element.Source = imagePath;  // Fails in IE7
    SJ.findElement(names["scale"]).CenterX = imageWidth / 2;
    SJ.findElement(names["scale"]).CenterY = imageHeight / 2;
    
    image = {}
    image.element = element;
    image.names = names;
    
    this.images.push(image);
    this.imageNames.push(imageName);

    if (this.images.length == 1) {
        // Hook the storyboard's 'Completed' event for just the first image.
        // We use this event to turn the carousel multiple times.
        var storyboard = element.resources.GetItem(0);
        this.hookUpEvent("Completed", storyboard, "onStoryboardCompleted");
    }
        
    this.updateLayout();
}

SJ.CarouselPanel.prototype.onImageClick = function (sender, args) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;

        SJ_HideToolTip();
        
        var target = this.locateImage(sender);
        if (target != -1 && target != this.selection) {
            this.target = target;
            this.turnTowardTarget();
        }
    }
}

SJ.CarouselPanel.prototype.onImageMouseEnter = function (sender, args) {
    var target = this.locateImage(sender);
    if (target != -1)
        SJ_ShowToolTip(sender, this.imageNames[target], args.getPosition(SJ.topCanvas));
}

SJ.CarouselPanel.prototype.onImageMouseLeave = function (sender, args) {
    SJ_HideToolTip();
}

SJ.CarouselPanel.prototype.locateImage = function (image) {
    for (var i = 0; i < this.images.length; i++) {
        if (this.images[i].element.Source == image.Source)
            return i;
    }
    return -1;
}

// Border
// margins is an integer for equal margins, or {left,top,bottom,right}

SJ.Border = function (margins, otherAttributes, shadow) {
    SJ.Control.call(this);
    
    if (margins instanceof Object) {
        if (!margins.left)
            margins.left = 0;
        if (!margins.top)
            margins.top = 0;
        if (!margins.right)
            margins.right = 0;
        if (!margins.bottom)
            margins.bottom = 0;
        this.margins = margins;
    }
    else {
        this.margins = {left: margins, top: margins, right: margins, bottom: margins};
    }
    shadow = !!shadow;
    
    otherAttributes = otherAttributes || "";
    
    var xaml = "<Canvas " + otherAttributes + ">";
    if (shadow) {
        xaml +=
            "<Rectangle Canvas.Top='" + 6 + "' Fill='#30000000'> \
                <Rectangle.RenderTransform> \
                 <TransformGroup> \
                  <RotateTransform Angle='-1' /> \
                 </TransformGroup> \
                </Rectangle.RenderTransform> \
            </Rectangle>";
    }
    xaml += "</Canvas>";
    this.visual = SJ.createFromXaml(xaml);
    
    if (shadow)
        this.shadow = this.visual.Children.GetItem(0);
    
    this.updateLayout();
}

SJ.Border.prototype = new SJ.Control;

SJ.Border.prototype.toString = function () {
    return "SJ.Border";
}

SJ.Border.prototype.setContent = function (content) {
    if (this.content != null) {
        this.content.dispose();
    }
    
    this.content = content;
    this.content.setParent(this);
    
    this.updateLayout();
}

SJ.Border.prototype.setBackgroundImage = function (imageSource) {
    this.visual.Background = SJ.createFromXaml("<ImageBrush ImageSource='" + SJ.xmlEscape(imageSource) + "' Stretch='Fill' />");
}

SJ.Border.prototype.measure = function (availWidth, availHeight) {
    if (this.content) {
        this.content.measure(availWidth - this.margins.left - this.margins.right,
                             availHeight - this.margins.top - this.margins.bottom);
        this.desiredWidth = this.content.desiredWidth + this.margins.left + this.margins.right;
        this.desiredHeight = this.content.desiredHeight + this.margins.top + this.margins.bottom;
    }
    else {
        this.desiredWidth = this.margins.left + this.margins.right;
        this.desiredHeight = this.margins.top + this.margins.bottom;
    }
}

SJ.Border.prototype.arrange = function (left, top, width, height) {
    SJ.Control.prototype.arrange.call(this, left, top, width, height);

    if (this.content) {
        this.content.arrange(this.margins.left, this.margins.top, this.content.desiredWidth, this.content.desiredHeight);
        if (this.shadow) {
            this.shadow.Width = this.content.desiredWidth;
            this.shadow.Height = this.content.desiredHeight;
        }
    }
    else if (this.shadow) {
        this.shadow.Width = 0;
        this.shadow.Height = 0;
    }
}

// Animated Image
// Loads an image strip and shows the 1st frame. When animate() is called, it 
// cycles thru the other frames.

SJ.AnimatedImage = function (top, left, width, height, imageStrip, numFrames, delayBetweenFrames) {
    SJ.Control.call(this);

    this.visual = SJ.createFromXaml("<Canvas />");
    
    this.numFrames = numFrames;
    this.frameWidth = width / numFrames;
    this.frameHeight = height;
    this.delayBetweenFrames = delayBetweenFrames ? delayBetweenFrames : 100;
    
    this.image = new SJ.Image(0, 0, imageStrip);
    this.image.visual.Clip = SJ.createFromXaml("<RectangleGeometry Rect='" +
            "0,0," + this.frameWidth + "," + this.frameHeight + "' />");
    this.image.setParent(this);

    SJ.placeElement(this.visual,left,top);
    SJ.placeElement(this.image.visual,0,0);
}

SJ.AnimatedImage.prototype = new SJ.Control;

SJ.AnimatedImage.prototype.toString = function () {
    return "SJ.AnimatedImage";
}

SJ.AnimatedImage.prototype.animate = function () {
    this.showing = 0;
    this.nextFrame();
}

SJ.AnimatedImage.prototype.nextFrame = function () {
    if (++this.showing < this.numFrames) {
        var left = this.showing * this.frameWidth;
        SJ.placeElement(this.image.visual,-left,0);
        this.image.visual.Clip.Rect = left + ",0," + this.frameWidth + "," + this.frameHeight;
        this.timer = setTimeout(SJ.methodCaller(this,"nextFrame"), this.delayBetweenFrames);
    }
    else {
        SJ.placeElement(this.image.visual,0,0);
        this.image.visual.Clip.Rect = "0,0," + this.frameWidth + "," + this.frameHeight;
        this.timer = null;
    }
}

// DragDrop

SJ_DragDrop_Droppable_MouseEnter = function (sender, eventArgs) {
    if (SJ.dragDrop.dragging && this.droppable && this.droppable.active) {
        if (this.OnDragDropMouseEnter) {
            this.OnDragDropMouseEnter(SJ.dragDrop.dragging, eventArgs);
        }
    }
}

SJ_DragDrop_Droppable_MouseLeave = function (sender, eventArgs) {
    if (SJ.dragDrop.dragging && this.droppable && this.droppable.active) {
        if (this.OnDragDropMouseLeave) {
            this.OnDragDropMouseLeave(SJ.dragDrop.dragging, eventArgs);
        }
    }
}

SJ_DragDrop_Droppable_MouseLeftButtonUp = function (sender, eventArgs) {
    if (!SJ.cancelBubble) {
        SJ.cancelBubble = true;
        if (SJ.dragDrop.dragging && this.droppable && this.droppable.active) {
            if (this.OnDragDropItemDropped) {
                var e = { 
                    mouseEventArgs: eventArgs,
                    item: SJ.dragDrop.dragging, 
                    dragImage: SJ.dragDrop.dragVisual
                };
                this.OnDragDropItemDropped(sender, e);
            }
        }
    }
}

SJ_DragDrop_DragSurface_MouseMove = function (sender, eventArgs) {
    if (SJ.dragDrop && SJ.dragDrop.dragging) {
        var mousePos = eventArgs.getPosition(null);
        var dragVisual = SJ.dragDrop.dragVisual;
        SJ.placeElement(dragVisual, mousePos.x - 25, mousePos.y - 25);
        
        if (!SJ.dragDrop.visible &&
            (Math.abs(mousePos.x - SJ.dragDrop.mousePos.x) > 2 ||
             Math.abs(mousePos.y - SJ.dragDrop.mousePos.y) > 2)) {
            SJ.dragDrop.dragSurface.Children.Add(dragVisual);
            SJ.dragDrop.visible = true;
            SJ.releaseMouseCapture(); // dragging trumps mouse capture
        }
    }
}

SJ_DragDrop_DragSurface_MouseLeftButtonDown = function (sender, eventArgs) {
    SJ.cancelBubble = false; // reset for next event
}

SJ_DragDrop_DragSurface_MouseLeftButtonUp = function (sender, eventArgs) {
    if (SJ.dragDrop && SJ.dragDrop.dragging) {
        SJ.endDragDrop();
    }
    SJ.cancelBubble = false; // reset for next event
}

SJ.beginDragDrop = function (item, dragVisual, eventArgs) {
    if (SJ.dragDrop.dragging == null) {
        SJ.dragDrop.mousePos = eventArgs.getPosition(null);
        SJ.dragDrop.dragging = item;
        SJ.dragDrop.dragVisual = dragVisual;
        SJ.dragDrop.visible = false;
    }
}

SJ.endDragDrop = function () {
    if (SJ.dragDrop.dragging) {
        SJ.dragDrop.dragging = null;
        if (SJ.dragDrop.visible) {
            SJ.dragDrop.dragSurface.Children.Remove(SJ.dragDrop.dragVisual);
            SJ.dragDrop.visible = false;
        }
    }
}

SJ.makeDroppable = function (control) {
    control.droppable = {};
    control.droppable.active = true;
    control.DragDropMouseEnter = SJ_DragDrop_Droppable_MouseEnter;
    control.DragDropMouseLeave = SJ_DragDrop_Droppable_MouseLeave;
    control.DragDropMouseLeftButtonUp = SJ_DragDrop_Droppable_MouseLeftButtonUp;
    control.hookUpEvent("MouseEnter", control.visual, "DragDropMouseEnter");
    control.hookUpEvent("MouseLeave", control.visual, "DragDropMouseLeave");
    control.hookUpEvent("MouseLeftButtonUp", control.visual, "DragDropMouseLeftButtonUp");
}

// ToolTips

function SJ_ShowToolTip(visual, text, position) {
    SJ.toolTip.visual = visual;
    SJ.toolTip.text = text;
    SJ.toolTip.lastMousePosition = position;
    SJ.toolTip.eventToken = visual.addEventListener("MouseMove", "SJ_ToolTip_OnMouseMove");
    SJ.toolTip.timer = setTimeout(SJ_ToolTip_OnTimeout, 500);
}

function SJ_HideToolTip() {
    if (SJ.toolTip.visual) {
        SJ.toolTip.visual.removeEventListener("MouseMove", SJ.toolTip.eventToken);
        SJ.toolTip.visual = null;
    }
    if (SJ.toolTip.timer) {
        clearTimeout(SJ.toolTip.timer);
        SJ.toolTip.timer = null;
    }
    if (SJ.toolTip.toolTipHover) {
        SJ.toolTip.toolTipHover.setParent(null);
        SJ.toolTip.toolTipHover = null;
    }
}

function SJ_ToolTip_OnMouseMove(sender, eventArgs) {
    SJ.toolTip.lastMousePosition = eventArgs.getPosition(SJ.topCanvas);
}

function SJ_ToolTip_OnTimeout() {    
    var textBlock = new SJ.TextBlock(0, 0, 0, 0, SJ.toolTip.text,
                                "TextWrapping='NoWrap' FontSize='11' Foreground='#000000'"); 
    textBlock.hAlign = "grow";
    
    var border = new SJ.Border(4, "Background='#ffffe1'");
    border.setContent(textBlock);
    border.setParent(SJ.topCanvas);

    var pt = SJ.toolTip.lastMousePosition;
    SJ.placeElement(border.visual, pt.x+10, pt.y+20);

    SJ.toolTip.toolTipHover = border;
}

// AsyncRequest: helper for sending async requests

SJ.AsyncRequest = function (httpMethod, service, body, onResult, userState) {
    try {
        var s = window.location.protocol + "//" + window.location.host + window.location.pathname;
        s = s.split('/');
        s[s.length - 1] = service;
        var url = s.join('/');
        var request = new Sys.Net.WebRequest();
        request.set_httpVerb(httpMethod);
        request.set_url(url);
        if (httpMethod != "GET")
            request.set_body(body);
        request.add_completed(SJ.OnAsyncRequestCompleted);
        request.set_userContext( { onResult: onResult, userState: userState } );
        request.set_timeout(15 * 1000);
        request.invoke();
    } catch (e) {
        onResult( { succeeded: false, exception: e, userState: userState } );
    }
}

SJ.OnAsyncRequestCompleted = function (executor, eventArgs) {
    var request = executor.get_webRequest();
    var userContext = request.get_userContext();
    var result = { executor: executor, userState: userContext.userState };
    
    if (executor.get_responseAvailable()) {
        result.succeeded = (executor.get_statusCode() == 200);
        result.response = executor.get_responseData();
    }
    else {
        result.succeeded = false;
    }
    
    if (userContext.onResult)
        userContext.onResult(result);
}

