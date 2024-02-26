/// <reference path="VEJS/VeJavaScriptIntellisenseHelper.js" />
/// <reference name="MicrosoftAjax.js" />

Type.registerNamespace("WLQuickApps.ContosoBank") ; 

WLQuickApps.ContosoBank.DashBoard = function(element) {
    /// <summary>
    ///   A Custom Dashboard framework for Virtual Earth.
    /// </summary>
    /// <param name="element">The div element to be made into the dashboard.</param>
    WLQuickApps.ContosoBank.DashBoard.initializeBase(this, [element]) ;
    
    //Optional Custom ZoomBar (ASP.NET AJAX Toolkit Slider)
    this._zoombarElement;
    
    //Current Dashboard settings to stored to improve performance
    this._currentMapStyle;
    this._currentMapMode;
    
    //properties
    this._map;
    this._selectedCSSClass;
    this._disabledCSSClass;
    this._road;
    this._aerial;
    this._hybrid;
    this._shaded;
    this._birdseye;
    this._birdseyeHybrid;
    this._mode2D;
    this._mode3D;
    this._zoomIn;
    this._zoomOut;
    //Optional Custom ZoomBar (ASP.NET AJAX Toolkit Slider)
    this._zoombar;
} 

WLQuickApps.ContosoBank.DashBoard.prototype = {

    //initialize / setup
    initialize : function() {
        /// <summary>
        ///   Initialises the Dashboard.
        /// </summary>       
        WLQuickApps.ContosoBank.DashBoard.callBaseMethod(this, 'initialize') ;
        
        //add dashboard to the map
        this._map.AddControl(this.get_element());
               
        //expect to only need the "onchangeview" event but not well supported in 3D so instead bind to the 3 events seperatly    
        this._map.get_VEMap().AttachEvent("onendzoom", Function.createDelegate(this, this._updateDashBoard));
        this._map.get_VEMap().AttachEvent("oninitmode", Function.createDelegate(this, this._updateDashBoard));
        this._map.get_VEMap().AttachEvent("onchangemapstyle", Function.createDelegate(this, this._updateDashBoard));
        
        this._updateDashBoard();
       
    },
    
    _setupZoomSlider: function(ctrl) {
        this._zoombarElement = ctrl;
        this._zoombarElement.add_valueChanged(Function.createDelegate(this, this._zoomTextChange));
        this._zoombarElement.set_Value(this._zoomLevel);
    },    
    
    _zoomTextChange: function(e) {
        this._map.get_VEMap().SetZoomLevel(parseInt(e.get_Value()));
    },
    
    _updateDashBoard: function() {
        var map = this._map.get_VEMap();
        if (map) {
            
            //ZoomBar
            if (this._zoombar) {
                if (!this._zoombarElement) {
                    var ctrl = $find(this._zoombar);
                    //Control may not be created yet at this point.
                    if (ctrl) {
                        this._setupZoomSlider(ctrl);
                    }else {
                        //we need to delay and then try this again, zoombar must be setup.
                        setTimeout(Function.createDelegate(this,function(){this._updateDashBoard()}),1000); 
                    }
                }
                if (this._zoombarElement) {
                    if (this._zoombarElement.get_Value() != map.GetZoomLevel()) {
                        this._zoombarElement.set_Value(map.GetZoomLevel());
                    }
                }
            }
            //only if something has actually changed
            if (map.GetMapStyle() != this._currentMapStyle || map.GetMapMode() != this._currentMapMode) {
            
                //reset all
                this._resetAllCssClass();
                
                //Map Style
                this._currentMapStyle = map.GetMapStyle();
                switch (this._currentMapStyle) {
                    case VEMapStyle.Road:
                        this._toggleCssClass(this._road,this._selectedCSSClass,true);
                        break;
                    case VEMapStyle.Aerial:
                        this._toggleCssClass(this._aerial,this._selectedCSSClass,true);
                        break;
                    case VEMapStyle.Hybrid:
                        this._toggleCssClass(this._hybrid,this._selectedCSSClass,true);
                        break;
                    case VEMapStyle.Shaded:
                        this._toggleCssClass(this._shaded,this._selectedCSSClass,true);
                        break;
                    case VEMapStyle.Birdseye:
                        this._toggleCssClass(this._birdseye,this._selectedCSSClass,true);
                        break;
                    case VEMapStyle.BirdseyeHybrid:
                        this._toggleCssClass(this._birdseyeHybrid,this._selectedCSSClass,true);
                        break;      
                }
                //Map Mode
                this._currentMapMode = map.GetMapMode();
                switch (this._currentMapMode) {
                    case VEMapMode.Mode2D:
                        this._toggleCssClass(this._mode2D,this._selectedCSSClass,true);
                        break;
                    case VEMapMode.Mode3D:
                        this._toggleCssClass(this._mode3D,this._selectedCSSClass,true);
                        break;
                }
            }
            //Is Birdseye available?
            if (this._currentMapMode == VEMapMode.Mode3D || map.IsBirdseyeAvailable()) {
                this._toggleCssClass(this._birdseye,this._disabledCSSClass,false);
                this._toggleCssClass(this._birdseyeHybrid,this._disabledCSSClass,false);
            }else {
                this._toggleCssClass(this._birdseye,this._disabledCSSClass,true);
                this._toggleCssClass(this._birdseyeHybrid,this._disabledCSSClass,true);
            }
        }
    },
    
    _toggleCssClass : function(element, cssClass, turnOn) {
        if (element) {
            if (turnOn) {
                Sys.UI.DomElement.addCssClass(element,cssClass);
            }else {
                Sys.UI.DomElement.removeCssClass(element,cssClass);            
            }
        }
    },
    
    _resetAllCssClass : function() {
        this._toggleCssClass(this._road,this._selectedCSSClass,false);
        this._toggleCssClass(this._aerial,this._selectedCSSClass,false);
        this._toggleCssClass(this._hybrid,this._selectedCSSClass,false);
        this._toggleCssClass(this._shaded,this._selectedCSSClass,false);
        this._toggleCssClass(this._birdseye,this._selectedCSSClass,false);
        this._toggleCssClass(this._birdseyeHybrid,this._selectedCSSClass,false);    
        this._toggleCssClass(this._mode2D,this._selectedCSSClass,false);    
        this._toggleCssClass(this._mode3D,this._selectedCSSClass,false);    
    },
    
    //Events
    _onRoad : function() {
        this._map.get_VEMap().SetMapStyle(VEMapStyle.Road);
    },
    
    _onAerial : function() {
        this._map.get_VEMap().SetMapStyle(VEMapStyle.Aerial);
    },
    
    _onHybrid : function() {
        this._map.get_VEMap().SetMapStyle(VEMapStyle.Hybrid);
    },
    
    _onShaded : function() {
        this._map.get_VEMap().SetMapStyle(VEMapStyle.Shaded);
    },
    
    _onBirdseye : function() {
        this._map.get_VEMap().SetMapStyle(VEMapStyle.Birdseye);
        //3D mode fix for Birdseye - uses different methods for some reason.
        if (this._currentMapMode == VEMapMode.Mode3D) {
            this._map.get_VEMap().Show3DBirdseye(true);
        }        
    },
    
    _onBirdseyeHybrid : function() {
        this._map.get_VEMap().SetMapStyle(VEMapStyle.BirdseyeHybrid);
        //3D mode fix for Birdseye - uses different methods for some reason.
        if (this._currentMapMode == VEMapMode.Mode3D) {
            this._map.get_VEMap().Show3DBirdseye(true);
        }        
    },
    
    _onMode2D : function() {
        this._map.get_VEMap().SetMapMode(VEMapMode.Mode2D);
    },  
    
    _onMode3D : function() {
        this._map.get_VEMap().SetMapMode(VEMapMode.Mode3D);
    },    
    
    _onZoomIn : function() {
        this._map.get_VEMap().ZoomIn();
    },
    
    _onZoomOut : function() {
        this._map.get_VEMap().ZoomOut();
    },                                  

    //Public Property Getters/Setters  
    get_Map : function() {
        return this._map;
    }, 

    set_Map : function(value) {
        this._map = value;
    },
    
    get_SelectedCSSClass : function() {
        return this._selectedCSSClass;
    }, 

    set_SelectedCSSClass : function(value) {
        this._selectedCSSClass = value;
    },  
    
    get_DisabledCSSClass : function() {
        return this._disabledCSSClass;
    }, 

    set_DisabledCSSClass : function(value) {
        this._disabledCSSClass = value;
    },       
    
    get_Zoombar : function() {
        return this._zoombar;
    }, 

    set_Zoombar : function(value) {
        this._zoombar = value;
    },    
    
    get_Road : function() {
        return this._road;
    }, 

    set_Road : function(value) {
        this._road = value;
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onRoad));        
    },     
     
    get_Aerial : function() {
        return this._aerial;
    }, 

    set_Aerial : function(value) {
        this._aerial = value;
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onAerial));        
    }, 
    
    get_Hybrid : function() {
        return this._hybrid;
    }, 

    set_Hybrid : function(value) {
        this._hybrid = value;
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onHybrid));        
    },          
    
    get_Shaded : function() {
        return this._shaded;
    }, 

    set_Shaded : function(value) {
        this._shaded = value;
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onShaded));        
    }, 
    
    get_Birdseye : function() {
        return this._birdseye;
    }, 

    set_Birdseye : function(value) {
        this._birdseye = value;
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onBirdseye));        
    },                       

    get_BirdseyeHybrid : function() {
        return this._birdseyeHybrid;
    }, 

    set_BirdseyeHybrid : function(value) {   
        this._birdseyeHybrid = value;
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onBirdseyeHybrid));        
    },
    
    get_Mode2D : function() {
        return this._mode2D;
    }, 

    set_Mode2D : function(value) { 
        this._mode2D = value;  
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onMode2D));        
    },
    
    get_Mode3D : function() {
        return this._mode3D;
    }, 

    set_Mode3D : function(value) {
        this._mode3D = value;
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onMode3D));        
    },
    
    get_ZoomIn : function() {
        return this._zoomIn;
    }, 

    set_ZoomIn: function(value) {   
        this._zoomIn = value;
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onZoomIn));        
        
    },
    
    get_ZoomOut: function() {
        return this._zoomOut;
    }, 

    set_ZoomOut : function(value) {
        this._zoomOut = value;
        //attach events
        $addHandler(value, "click", Function.createDelegate(this, this._onZoomOut));        
        
    },
            
    dispose : function() {
        /// <summary>
        ///   Dispose all events and objects.
        /// </summary>
        if (this._road) $clearHandlers(this._road);
        if (this._aerial) $clearHandlers(this._aerial);
        if (this._hybrid) $clearHandlers(this._hybrid);
        if (this._shaded) $clearHandlers(this._shaded);
        if (this._birdseye) $clearHandlers(this._birdseye);
        if (this._birdseyeHybrid) $clearHandlers(this._birdseyeHybrid);
        if (this._mode2D) $clearHandlers(this._mode2D);
        if (this._mode3D) $clearHandlers(this._mode3D);
        if (this._zoomIn) $clearHandlers(this._zoomIn);
        if (this._zoomOut) $clearHandlers(this._zoomOut);
        WLQuickApps.ContosoBank.DashBoard.callBaseMethod(this, 'dispose') ;
    }          
    
} 

WLQuickApps.ContosoBank.DashBoard.registerClass("WLQuickApps.ContosoBank.DashBoard", Sys.UI.Control);

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();