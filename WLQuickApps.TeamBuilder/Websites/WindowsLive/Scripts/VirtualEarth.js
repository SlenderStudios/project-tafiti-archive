/// <reference name="MicrosoftAjax.js"/>

Type.registerNamespace("Controls");

Controls.VirtualEarth = function(element) {
    Controls.VirtualEarth.initializeBase(this, [element]);
    
    this._map = null;
    this._isLoadComplete = false;
    this._initialCenter = null;
    this._initialZoomLevel = 4;
    
    this._events = null;
    this._currentEvent = null;
    this._points = null;
}

Controls.VirtualEarth.prototype = {

    initialize : function() {
    
        Controls.VirtualEarth.callBaseMethod(this, 'initialize');
        
        Sys.Application.add_load(Function.createDelegate(this, this._onLoad));
    },
    
    dispose : function() {
    
        this._map.Dispose();
        this._events = null;
        this._currentEvent = null;
        this._points = null;
        
        Controls.VirtualEarth.callBaseMethod(this, 'dispose');
    },
    
    _onLoad : function(sender, eventArgs) {
        var id = this.get_id();
        this._map = new VEMap(id);
        this._map.onLoadMap = Function.createDelegate(this, this._onLoadMap);
        this._map.LoadMap();
    },
    
    _onLoadMap : function(sender, eventArgs) {
        this._isLoadComplete = true;
        this._initialCenter = this._map.GetCenter();
        this._initialZoomLevel = this._map.GetZoomLevel();
        this.raiseMapLoad();
    },
    
    get_isLoadComplete : function() {
        return this._isLoadComplete;
    },
    
    get_map : function() {
        return this._map;
    },
    
    add_mapLoad : function(handler) {
        /// <summary>
        /// Adds an event handler for the <code>mapLoad</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to add to the event.
        /// </param>
        /// <returns />

        this.get_events().addHandler("mapLoad", handler);
    },
    
    remove_mapLoad : function(handler) {
        /// <summary>
        /// Removes an event handler for the <code>mapLoad</code> event.
        /// </summary>
        /// <param name="handler" type="Function">
        /// The handler to remove from the event.
        /// </param>
        /// <returns />

        this.get_events().removeHandler("mapLoad", handler);
    },
    
    raiseMapLoad : function() {
        /// <summary>
        /// Raise the <code>mapLoad</code> event
        /// </summary>
        /// <returns />

        var handlers = this.get_events().getHandler("mapLoad");
        if (handlers) {
            handlers(this, Sys.EventArgs.Empty);
        }
    },
    
    set_events : function(events) {
        /// <summary>
        /// </summary>
        
        if (!events || !this._isLoadComplete)
            return;
        
        // Clear map and reset points
        this._map.Clear();
        this._points = new Array();
        
        if (events.length != 0) {
            this._events = events.slice(); // Copy array
            this._currentEvent = this._events.shift();
            this._findLocation(this._currentEvent.Location);
        } else {
            this._map.SetCenter(this._initialCenter);
            this._map.SetZoomLevel(this._initialZoomLevel);
            this._events = null;
        }
    },
    
    _findLocation : function(location) {
        this._map.Find(
            null, /* what */
            location, /* where */
            null, /* findType */
            null, /* shapeLayer */
            0, /* startIndex */
            1, /* numberOfResults */
            false, /* showResults */
            false, /* createResults */
            false, /* useDefaultDisambiguation */
            false, /* setBestMapView */
            Function.createDelegate(this, this._onFindComplete)) /* callback */
    },
    
    _onFindComplete : function(shapeLayer, findResults, places) {
        if (places != null && places.length != 0) {
            // Create the pushpin shape
            var point = places[0].LatLong;
            var shape = new VEShape(VEShapeType.Pushpin, point);
            
            // Set pushpin properties
            shape.SetTitle(this._currentEvent.Summary);
            shape.SetDescription(places[0].Name); // Use map value instead of event location
            
            // Add the shape to the map
            this._map.AddShape(shape);
            
            // Add to points for map view
            this._points.push(point);
        }
        
        if (this._events == null) {
            this._map.Clear();
            this._map.SetCenter(this._initialCenter);
            this._map.SetZoomLevel(this._initialZoomLevel);
        } else if (this._events.length != 0) {
            this._currentEvent = this._events.shift();
            this._findLocation(this._currentEvent.Location);
        } else {
            this._map.SetMapView(this._points);
        }
    }
}
Controls.VirtualEarth.registerClass('Controls.VirtualEarth', Sys.UI.Control);

// Since this script is not loaded by System.Web.Handlers.ScriptResourceHandler
// invoke Sys.Application.notifyScriptLoaded to notify ScriptManager 
// that this is the end of the script.
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
