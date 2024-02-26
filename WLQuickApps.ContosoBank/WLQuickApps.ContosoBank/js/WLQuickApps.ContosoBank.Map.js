/// <reference path="VEJS/VeJavaScriptIntellisenseHelper.js" />
/// <reference name="MicrosoftAjax.js" />

Type.registerNamespace("WLQuickApps.ContosoBank") ; 

WLQuickApps.ContosoBank.Map = function(element) {
    /// <summary>
    ///   The Map object that enhances the VE Map object making it more ASP.NET AJAX and supporting Server Side Clustering and Filtering.
    /// </summary>
    /// <param name="element">The div element to be made into the VE map.</param>
    WLQuickApps.ContosoBank.Map.initializeBase(this, [element]) ;
    
    //The VEMap object
    this._map = null;
    
    //The service to get our map data
    this._service;
    
    //Keep track of all overlay controls for 3D Shims
    this._mapControls = new Array();
    
    //data layer
    this._layer = null; 
    
    //private VE properties and default values as per VE spec
    //do not change here - change using object getters and setters or in the object constructor.
    this._getClusteredData = false;
    this._vEAPINotFound = "The Virtual Earth API failed to load";
    this._browsersNotSupported = "Your Browser does not support Virtual Earth";
    this._height = 400;
    this._width = 400;
    this._center = new VELatLong(40.0, -104.0);
    this._mapMode = VEMapMode.Mode2D;
    this._mapStyle = VEMapStyle.Road;
    this._navigationControl3D = true;
    this._dashboard = true;
    this._dashboardSize = VEDashboardSize.Normal;
    this._miniMap = false;
    this._miniMapXoffset = 90;
    this._miniMapYoffset = 40;
    this._miniMapSize = VEMiniMapSize.Small;
    this._trafficLegend = false;
    this._trafficLegendX = -1;
    this._trafficLegendY = -1;
    this._traffic = false;
    this._trafficFlow = true;
    this._enableShapeDisplayThreshold = true;
    this._mouseWheelZoomToCenter = true;
    this._scaleBarDistanceUnit = VEDistanceUnit.Miles;
    this._shapesAccuracy = VEShapeAccuracy.None;
    this._shapesAccuracyRequestLimit = 50;
    this._tileBuffer = 0;
    this._trafficLegendText = "";
    this._zoomLevel = 4;
    this._disambiguationDialog = true;
    this._clearInfoBoxStyles = false;
    this._fixedMap = false;
    this._showMapModeSwitch = true;
    this._showFindControl = false;
    this._altitude = 0.0;
    this._heading = 0.0;
    this._pitch = -90.0;
    this._failedShapeRequest = VEFailedShapeRequest.DrawInaccurately;
    this._mapOptions = new VEMapOptions();    
    this._mapOptions.EnableBirdseye = true;
    this._mapOptions.EnableDashboardLabels = true;
} 

WLQuickApps.ContosoBank.Map.prototype = {

    //initialize / setup
    initialize : function() {
        /// <summary>
        ///   Initialises the Map.
        /// </summary>       
        WLQuickApps.ContosoBank.Map.callBaseMethod(this, 'initialize') ;
                
        //verify VEAPI has loaded
        if (typeof(VEMap) != "undefined") {
            //verify Browser is supported
            if (
                //IE6 or above
                (Sys.Browser.agent == Sys.Browser.InternetExplorer && Sys.Browser.version >= 6) || 
                //FF2 or above
                (Sys.Browser.agent == Sys.Browser.Firefox && Sys.Browser.version >= 2) || 
                //Safari 2 or above
                (Sys.Browser.agent == Sys.Browser.Safari && Sys.Browser.version >= 2)
                ) {
                this._createVEMap();
            }else {
                this.get_element().innerHTML = this._browsersNotSupported;
            }
        }else {
            this.get_element().innerHTML = this._vEAPINotFound;
        } 
    },
     
    _createVEMap: function() {
        this._disposeVEMap();
        this._map = new VEMap(this.get_element().id);
        //Dashboard selections
        if (this._dashboard) this._map.SetDashboardSize(this._dashboardSize);
        //Listen for when map is loaded to perform operations
        this._map.onLoadMap = Function.createDelegate(this, this._onLoadVEMap);        
        this._map.LoadMap(this._center, this._zoomLevel, this._mapStyle, this._fixedMap, this._mapMode, this._showMapModeSwitch, this._tileBuffer, this._mapOptions);
    },
    
    _disposeVEMap: function() {
       if (this._map) {     
            this._map.Dispose();
            this._map = null;
        }    
    },    
    
    _onLoadVEMap: function() {
        /// <summary>
        ///   Executes on map load, sets the basic map properties, binds event handlers.
        /// </summary>  
        
        //set properties  
        this._map.SetScaleBarDistanceUnit(this._scaleBarDistanceUnit);
        this._map.EnableShapeDisplayThreshold(this._enableShapeDisplayThreshold);
        this._map.SetMouseWheelZoomToCenter(this._mouseWheelZoomToCenter);
        this._map.SetFailedShapeRequest(this._failedShapeRequest);
        this._map.SetShapesAccuracy(this._shapesAccuracy);
        this._map.SetShapesAccuracyRequestLimit(this._shapesAccuracyRequestLimit);
        this._map.ShowDisambiguationDialog(this._disambiguationDialog);  
                
        if (!this._dashboard) {
            this._map.HideDashboard();
        }
        
        if (this._miniMap){
            this._map.ShowMiniMap(this._miniMapXoffset, this._miniMapYoffset, this._miniMapSize);
        }
        
        if (this._clearInfoBoxStyles) {
            this._map.ClearInfoBoxStyles();
        }
        
        if (!this._navigationControl3D) {
            this._map.Hide3DNavigationControl();
        }
        
        if (this._showFindControl) {
            this._map.ShowFindControl();
        }        
        
        if (this._traffic && this._clientToken) {
            this._map.LoadTraffic(this._trafficFlow);
            if (this._trafficLegend) {
                if (this._trafficLegendX > -1 && this._trafficLegendY > -1) {
                    this._map.ShowTrafficLegend(this._trafficLegendX, this._trafficLegendY)
                }else {
                    this._map.ShowTrafficLegend()
                }
                if (this._trafficLegendText) {
                    this._map.SetTrafficLegendText(this._trafficLegendText);
                }
            }
        }
        
        if (this._map.GetMapMode() == VEMapMode.Mode3D) {
            //Ignore default value 0m, use Zoomlevel instead.
            if (this._altitude > 0) {
                this._map.SetAltitude(this._altitude);
            }
            this._map.SetHeading(this._heading);
            this._map.SetPitch(this._pitch);
        }   
        
        this._layer = new VEShapeLayer();         
        this._map.AddShapeLayer(this._layer);
        
        //3d
        this._map.AttachEvent("oninitmode", Function.createDelegate(this, this._OnModInit));
        
        //setup the function to get new data whenever the map changes
        if (this._getClusteredData) {
            //Setup additional storage for shapes
            VEShape.prototype.Bounds = "";
            this._map.AttachEvent("onchangeview", Function.createDelegate(this, this._GetPinData));
            //get the data for the current view
            this._GetPinData();        
        }
    },
    
    _GetPinData: function() {  
        /// <summary>
        ///   Get the latest map data from the webservice.
        /// </summary>
              
        var zoom;
        if (this._map.GetMapStyle() == VEMapStyle.Birdseye) {     
            //set zoomlevel      
            zoom = 19;
        }else {        
            //get zoomlevel
            zoom = this._map.GetZoomLevel();
        }
        var bounds = Utility.GetBounds(this._map);
        if (this._zoomLevel != zoom) {
            //clear existing pins
            this._layer.DeleteAllShapes();
            this._zoomLevel = zoom;
        }
      
        //call webservice          
        this._service.GetClusteredMapData(bounds, zoom, filter, Function.createDelegate(this, this._OnMapDataSucceeded), Utility.OnFailed); 
    },
    
    onFilterChange: function() {
        this._GetPinData();
    },

    _OnMapDataSucceeded: function(results) {
        /// <summary>
        ///   Receive data for map.
        /// </summary>  
        /// <param name="result">The webservice result object - Optomised CSV string</param>  
            
        this._layer.DeleteAllShapes();
                    
        //decode pins
        var result=results.split(",")
        var locs = Utility.decodeLine(result[0]);
        var zoom = this._map.GetZoomLevel();
        var mapData = new Array();
               
        //add new pins
        for(var x = 0; x < locs.length; x++) {
            var loc = locs[x];
            var bounds = result[x+1];
            var recordcount = result[x+locs.length+1];
            this._createPin(zoom, loc, bounds, recordcount, mapData, this._layer, false);
        }
        
        if (mapData.length > 0)
        {
            this._layer.AddShape(mapData);
        }              
    },   
    
    _createPin: function(zoom, loc, bounds, recordcount, mapData, layer) {
        if (zoom < 12 || !filter) {
            var newShape = new VEShape(VEShapeType.Pushpin, loc); 
            newShape.Bounds = bounds;
            //set custom pin.
            var icon;
            if (recordcount > 5) icon = "PinLarge";
            else if (recordcount > 2) icon = "PinMedium";
            else icon = "PinSmall";
            if (filter) {
                icon = "Member" + icon;
            }else {
                icon = "ATM" + icon;
            }
            var Iconspec = new VECustomIconSpecification();
            Iconspec.CustomHTML = "<div class='" + icon + "'></div>";
            Iconspec.Image = "/images/pins/" + icon + ".png";
            newShape.SetCustomIcon(Iconspec);
            mapData.push(newShape);            
        } else{           
            var newShape = new VEShape(VEShapeType.Polygon, Utility.GetCirclePoints(loc,0.5)[0]); 
            newShape.Bounds = bounds;
            newShape.SetIconAnchor(loc);
            newShape.HideIcon();        
            newShape.SetFillColor(new VEColor(255,126,36,0.2));                 
            newShape.SetLineColor(new VEColor(255,126,36,0.9));                 
            layer.AddShape(newShape); 
        }     
    },
    
    _OnModInit: function() {
        /// <summary>
        ///   3D mode requires Iframe shims on controls.
        /// </summary>     
        if (this._map.GetMapMode()==VEMapMode.Mode3D) {
            Array.forEach(this._mapControls, this._addShim, this);
        }else {
            Array.forEach(this._mapControls, this._deleteShim, this);
        }
    },
    
    RefreshShims: function() {
        if (this._map.GetMapMode()==VEMapMode.Mode3D) {
            Array.forEach(this._mapControls, this._deleteShim, this);
            Array.forEach(this._mapControls, this._addShim, this);
        }    
    },
    
    AddControl: function(control) {
        this._mapControls.push(control);
        this._map.AddControl(control);
        //3D mode requires a shim.
        if (this._map && this._map.GetMapMode()==VEMapMode.Mode3D) this._addShim(control);
    },
    
    _addShim: function(control) {
        if (control.shimElement==null) {
            var shim = document.createElement("iframe");
            //shim.id = "myShim";
            shim.frameBorder = "0";
            shim.style.position = "absolute";
            shim.style.zIndex = "1";
            shim.style.top  = control.offsetTop;
            shim.style.left = control.offsetLeft;
            shim.width  = control.offsetWidth;
            shim.height = control.offsetHeight;
            control.shimElement = shim;
            control.parentNode.insertBefore(shim, control);
        }        
    },
    
    _deleteShim: function(control) {
          if (control.shimElement!=null) {
              control.shimElement.parentNode.removeChild(control.shimElement);      
              control.shimElement = null;     
          }
    },
    
    AddPinOffCentreAndPopup: function(loc, title, description, icon) {
        var newShape = new VEShape(VEShapeType.Pushpin, loc); 
        var Iconspec = new VECustomIconSpecification();
        Iconspec.CustomHTML = "<div class='" + icon + "'></div>";
        Iconspec.Image = "/images/pins/" + icon + ".png";
        newShape.SetCustomIcon(Iconspec);
        newShape.SetTitle(title);
        newShape.SetDescription(description);
        this._layer.AddShape(newShape);     
        this._map.SetMapView([loc]);
        this._Shape2Popup = newShape;
        this._map.Pan(-200,0);
        //The pan causes our infobox to close so we have to wait 2 secs
        setTimeout(Function.createDelegate(this,function(){ if (this && this._map && newShape) this._map.ShowInfoBox(newShape)}),2000);
    },
 
    //Public Property Getters/Setters 
    
    //Readonly get VE map object
    get_VEMap : function() {
        return this._map;
    }, 
     
    get_Service : function() {
        return this._service;
    }, 

    set_Service : function(value) {
        this._service = value;
    },
    
    get_GetClusteredData : function() {
        return this._getClusteredData;
    }, 

    set_GetClusteredData : function(value) {
        this._getClusteredData = value;
    },    
        
    get_BrowsersNotSupported : function() {
        return this._browsersNotSupported;
    }, 

    set_BrowsersNotSupported : function(value) {
        this._browsersNotSupported = value;
    }, 
    
    get_VEAPINotFound : function() {
        return this._vEAPINotFound;
    }, 

    set_VEAPINotFound : function(value) {
        this._vEAPINotFound = value;
    },          
    
    get_Width : function() {
        return this._width;
    }, 

    set_Width : function(value) {
        if (this._map && value != this._width) {
            this.Resize(value, this._height);
        }
        this._width = value;
    }, 
    
    get_Height : function() {
        return this._height;
    }, 

    set_Height : function(value) {
        if (this._map && value != this._height) {
            this.Resize(this._width, value);
        }
        this._height = value;
    },                       

    get_Center : function() {
        return this._map.GetCenter();
    }, 

    set_Center : function(value) {
        if (this._map) {
            if (this._map.GetCenter().Latitude != value.Latitude || this._map.GetCenter().Longitude != value.Longitude) {
                this._map.SetCenter(this._parseVELatLong(value));
            }
        }    
        this._center = value;
    },
    
    get_MapMode : function() {
        return this._map.GetMapMode();
    }, 

    set_MapMode : function(value) {
        if (this._map) {
            if (this._map.GetMapMode() != value) {
                this._map.SetMapMode(value);
            }
        }    
        this._mapMode = value;  
    },
    
    get_MapStyle : function() {
        return this._map.GetMapStyle();
    }, 

    set_MapStyle : function(value) {
        if (this._map) {
            if (this._map.GetMapStyle() != value) {
                this._map.SetMapStyle(value);
                //3D mode fix for Birdseye - uses different methods for some reason.
                if ((value == VEMapStyle.BirdseyeHybrid || value == VEMapStyle.Birdseye) && this._map.GetMapMode() == VEMapMode.Mode3D) {
                    this._map.Show3DBirdseye(true);
                }
            }
        }
        this._mapStyle = value;
    },
    
    get_NavigationControl3D : function() {
        return this._navigationControl3D;
    }, 

    set_NavigationControl3D : function(value) {
        if (this._map && value != this._navigationControl3D) {
            if (value) {
                this._map.Show3DNavigationControl();
            }else {
                this._map.Hide3DNavigationControl();
            }
        }    
        this._navigationControl3D = value;
    },
    
    get_Dashboard : function() {
        return this._dashboard;
    }, 

    set_Dashboard : function(value) {
        if (this._map && value != this._dashboard) {
            if (value) {
                this._map.ShowDashboard();
            }else {
                this._map.HideDashboard();
            }
        }
        this._dashboard = value;
    },
    
    get_DashboardSize : function() {
        return this._dashboardSize;
    }, 

    set_DashboardSize : function(value) {
        if (this._map && value != this._dashboardSize) {
            //have to restart the map to do this
            this._dashboardSize = value;
            this._createVEMap();
        }
        this._dashboardSize = value;
    },
    
    get_MiniMap : function() {
        return this._miniMap;
    }, 

    set_MiniMap : function(value) {
        if (this._map && value != this._miniMap) {
            if (value) {
                this._map.ShowMiniMap(this._miniMapXoffset, this._miniMapYoffset, this._miniMapSize);
            }else {
                this._map.HideMiniMap();
            }
        }
        this._miniMap = value;
    },
    
    get_MiniMapXoffset : function() {
        return this._miniMapXoffset;
    }, 

    set_MiniMapXoffset : function(value) {
        if (this._map && value != this._miniMapXoffset) {
            this._map.ShowMiniMap(value, this._miniMapYoffset, this._miniMapSize);
        }
        this._miniMapXoffset = value;
    },
    
    get_MiniMapYoffset : function() {
        return this._miniMapYoffset;
    }, 

    set_MiniMapYoffset : function(value) {
        if (this._map && value != this._miniMapYoffset) {
            this._map.ShowMiniMap(this._miniMapXoffset, value, this._miniMapSize);
        }
        this._miniMapYoffset = value;
    },
    
    get_MiniMapSize : function() {
        return this._miniMapSize;
    }, 

    set_MiniMapSize : function(value) {
        if (this._map && value != this._miniMapSize) {
            this._map.ShowMiniMap(this._miniMapXoffset, this._miniMapYoffset, value);
        }
        this._miniMapSize = value;
    },
    
    get_TrafficLegend : function() {
        return this._trafficLegend;
    }, 

    set_TrafficLegend : function(value) {
        if (this._map && value != this._trafficLegend) {
            if (value) {
                if (this._trafficLegendX > -1 && this._trafficLegendY > -1) {
                    this._map.ShowTrafficLegend(this._trafficLegendX, this._trafficLegendY);
                }else {
                    this._map.ShowTrafficLegend();
                }
            }else {
                this._map.HideTrafficLegend();
            }
        }
        this._trafficLegend = value;
    },
    
    get_TrafficLegendX : function() {
        return this._trafficLegendX;
    }, 

    set_TrafficLegendX : function(value) {
        if (this._map && value != this._trafficLegendX && this._trafficLegendY > -1) {
            this._map.ShowTrafficLegend(value, this._trafficLegendY);
        }
        this._trafficLegendX = value;
    },
    
    get_TrafficLegendY : function() {
        return this._trafficLegendY;
    }, 

    set_TrafficLegendY : function(value) {
        if (this._map && value != this._trafficLegendY && this._trafficLegendX > -1) {
            this._map.ShowTrafficLegend(this._trafficLegendX, value);
        }
        this._trafficLegendY = value;
    },
    
    get_TrafficFlow : function() {
        return this._trafficFlow;
    }, 

    set_TrafficFlow : function(value) { 
        if (this._map && value != this._trafficFlow) {
            //if traffic is shown change the flow 
            if (this._traffic && this._clientToken) {
                this._map.LoadTraffic(value);
            }
        }    
        this._trafficFlow = value;
    },
    
    get_Traffic : function() {
        return this._traffic;
    }, 

    set_Traffic : function(value) {
        if (this._map && value != this._traffic) {
            if (value) {
                this._map.LoadTraffic(this._trafficFlow);
            }else {
                this._map.ClearTraffic();
            }        
        }
        this._traffic = value;
    },
    
    get_EnableShapeDisplayThreshold : function() {
        return this._enableShapeDisplayThreshold;
    }, 

    set_EnableShapeDisplayThreshold : function(value) {
        if (this._map && value != this._enableShapeDisplayThreshold) {
            this._map.EnableShapeDisplayThreshold(value);
        }
        this._enableShapeDisplayThreshold = value;
    },
    
    get_MouseWheelZoomToCenter : function() {
        return this._mouseWheelZoomToCenter;
    }, 

    set_MouseWheelZoomToCenter : function(value) {
        if (this._map && value != this._mouseWheelZoomToCenter) {
            this._map.SetMouseWheelZoomToCenter(value);
        }
        this._mouseWheelZoomToCenter = value;
    },
    
    get_ScaleBarDistanceUnit : function() {
        return this._scaleBarDistanceUnit;
    }, 

    set_ScaleBarDistanceUnit : function(value) {
        if (this._map && value != this._scaleBarDistanceUnit) {
            this._map.SetScaleBarDistanceUnit(value);
        }
        this._scaleBarDistanceUnit = value;
    },
    
    get_ShapesAccuracy : function() {
        return this._shapesAccuracy;
    }, 

    set_ShapesAccuracy : function(value) {
        if (this._map && value != this._shapesAccuracy) {
            this._map.SetShapesAccuracy(value);
        }
        this._shapesAccuracy = value;
    },
    
    get_ShapesAccuracyRequestLimit : function() {
        return this._shapesAccuracyRequestLimit;
    }, 

    set_ShapesAccuracyRequestLimit : function(value) {
        if (this._map && value != this._shapesAccuracyRequestLimit) {
            this._map.SetShapesAccuracyRequestLimit(value);
        }
        this._shapesAccuracyRequestLimit = value;
    },
    
    get_TileBuffer : function() {
        return this._tileBuffer;
    }, 

    set_TileBuffer : function(value) {
        if (this._map && value != this._tileBuffer) {
            this._map.SetTileBuffer(value);
        }
        this._tileBuffer = value;
    },
    
    get_TrafficLegendText : function() {
        return this._trafficLegendText;
    }, 

    set_TrafficLegendText : function(value) {
        if (this._map && value != this._trafficLegendText) {
            this._map.SetTrafficLegendText(value);
        }
        this._trafficLegendText = value;
    },
    
    get_ZoomLevel : function() {
        return this._map.GetZoomLevel();
    }, 

    set_ZoomLevel : function(value) {
        if (this._map) {
            if (this._map.GetZoomLevel() != value) {
                this._map.SetZoomLevel(value);
            }
            if (value != this._zoomLevel) {
            }
        }
        this._zoomLevel = value;
    }, 
    
    get_DisambiguationDialog : function() {
        return this._disambiguationDialog;
    }, 

    set_DisambiguationDialog : function(value) {
        if (this._map && value != this._disambiguationDialog) {
            this._map.ShowDisambiguationDialog(value);
        }
        this._disambiguationDialog = value;
    },
    
    get_ClearInfoBoxStyles : function() {
        return this._clearInfoBoxStyles;
    },

    set_ClearInfoBoxStyles : function(value) {
        if (this._map && value != this._clearInfoBoxStyles) {
            if (value) {
                this._map.ClearInfoBoxStyles();
            }else {
                this._map.SetDefaultInfoBoxStyles();
            }
        }
        this._clearInfoBoxStyles = value;
    },
    
    get_FixedMap : function() {
        return this._fixedMap;
    },

    set_FixedMap : function(value) {
        if (this._map && value != this._fixedMap) {
            //have to restart the map to do this
            this._fixedMap = value;
            this._createVEMap();
        }
        this._fixedMap = value;
    },
    
    get_ShowMapModeSwitch : function() {
        return this._showMapModeSwitch;
    },

    set_ShowMapModeSwitch : function(value) {
        if (this._map && value != this._showMapModeSwitch) {
            //have to restart the map to do this
            this._showMapModeSwitch = value;
            this._createVEMap();
        }
        this._showMapModeSwitch = value;
    },
        
    get_ShowFindControl : function() {
        return this._showFindControl;
    },

    set_ShowFindControl : function(value) {
        if (this._map && value != this._showFindControl) {
            if (value) {
                this._map.ShowFindControl();
            }else {
                this._map.HideFindControl();
            }
        }
        this._showFindControl = value;
    },
              
    get_Altitude : function() {
        return this._map.GetAltitude();
    }, 

    set_Altitude : function(value) {
        if (this._map) {
            if (this._map.GetAltitude() != value) {
                this._map.SetAltitude(value);
            }
        }
        this._altitude = value;
    },  
    
    get_Heading : function() {
        return this._map.GetHeading();
    }, 

    set_Heading : function(value) {
        if (this._map) {
            if (this._map.GetHeading() != value) {
                this._map.SetHeading(value);
            }
        }
        this._heading = value;
    }, 
    
    get_Pitch : function() {
        return this._map.GetPitch();
    }, 

    set_Pitch : function(value) {
        if (this._map) {
            if (this._map.GetPitch() != value) {
                this._map.SetPitch(value);
            }
        }
        this._pitch = value;
    },           
    
    get_FailedShapeRequest : function() {
        return this._failedShapeRequest;
    },

    set_FailedShapeRequest : function(value) {
        if (this._map && value != this._failedShapeRequest) {
            this._map.SetFailedShapeRequest(value);
        }
        this._failedShapeRequest = value;
    },
    
    get_MapOptions : function() {
        return this._mapOptions;
    },

    set_MapOptions : function(value) {
        if (this._map && (value.EnableBirdseye != this._mapOptions.EnableBirdseye || value.EnableDashboardLabels != this._mapOptions.EnableDashboardLabels)) {
            //have to restart the map to do this
            this._mapOptions = value;
            this._createVEMap();
        }
        this._mapOptions = value;
    },
                
    dispose : function() {
        /// <summary>
        ///   Dispose all events and objects.
        /// </summary>     
        this._disposeVEMap();
        WLQuickApps.ContosoBank.Map.callBaseMethod(this, 'dispose') ;
    }          
    
} 

WLQuickApps.ContosoBank.Map.registerClass("WLQuickApps.ContosoBank.Map", Sys.UI.Control);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();