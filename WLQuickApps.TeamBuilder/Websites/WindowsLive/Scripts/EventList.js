/// <reference name="MicrosoftAjax.debug.js"/>
/// <reference name="AjaxControlToolkit.ExtenderBase.BaseScripts.js" assembly="AjaxControlToolkit" />
/// <reference name="AjaxControlToolkit.Common.Common.js" assembly="AjaxControlToolkit" />

Type.registerNamespace('Controls');

Controls.EventList = function(element) {
    /// <summary>
    /// The EventList replaces the contents of an element with the result of a web service or page method call.
    /// The method call returns a string of HTML that is inserted as the children of the target element.
    /// </summary>
    /// <param name="element" type="Sys.UI.DomElement" domElement="true">
    /// The DOM element that the current Control object is associated with.
    /// </param>
    
    Controls.EventList.initializeBase(this, [element]);
    
    this._servicePath = null;
    this._serviceMethod = null;
    this._customScript = null;
    
    this._calendar = null;
    this._calendarControlID = "calendar";
    this._currentMonth = null;
    this._selectedDate = null;
    this._dateSelectionChangedHandler = null;
    this._visibleDateChangedHandler = null;
    
    this._events = null;
    
    this._map = null;
    this._mapControlID = "event-map";
    this._isPendingMapUpdate = false;
    
    this._callID = 0;
    this._currentCallID = -1;
}

Controls.EventList.prototype = {

    initialize : function() {
        /// <summary>
        /// Initialize the behavior
        /// </summary>
        
        Controls.EventList.callBaseMethod(this, 'initialize');
        $common.prepareHiddenElementForATDeviceUpdate();        
        
        Sys.Application.add_load(Function.createDelegate(this, this._onLoad));
    },
    
    dispose : function() {
        /// <summary>
        /// Dispose the behavior
        /// </summary>
        
        // Clean up the date changed event.
        if (this._calendar) {
            if (this._visibleDateChangedHandler) {
                this._calendar.remove_visibleDateChanged(this._visibleDateChangedHandler);
                this._visibleDateChangedHandler = null;
            }
            if (this._dateSelectionChangedHandler) {
                this._calendar.remove_dateSelectionChanged(this._dateSelectionChangedHandler);
                this._dateSelectionChangedHandler = null;
            }
            this._calendarControlID = null;
        }
        
        this._events = null;
        
        Controls.EventList.callBaseMethod(this, 'dispose');
    },
    
    _onLoad : function(sender, eventArgs) {
        this._calendar = $find(this._calendarControlID);
        if (this._calendar) {
            // Attach date selection handler to calendar
            this._dateSelectionChangedHandler = Function.createDelegate(this, this._onDateSelectionChanged);
            this._calendar.add_dateSelectionChanged(this._dateSelectionChangedHandler);
            this._selectedDate = this._calendar.get_selectedDate();
            // Attach date change handler to calendar
            this._visibleDateChangedHandler = Function.createDelegate(this, this._onVisibleDateChanged);
            this._calendar.add_visibleDateChanged(this._visibleDateChangedHandler);
            this._currentMonth = this._calendar.get_visibleDate().getMonth();
            // Update event list
            this.populate(this._calendar.get_visibleDate());
            //this._timer.set_enabled(true);
        }
        // Attach event handler if map is not finished loading
        this._map = $find(this._mapControlID);
        if (this._map) {
            if (!this._map.get_isLoadComplete()) {
                this._map.add_mapLoad(Function.createDelegate(this, this._onMapLoad));
            }
        }
    },

    _onDateSelectionChanged : function(sender, eventArgs) {
        /// <summary>
        /// Handler for Calendar behavior's DateSelectionChanged event
        /// </summary>
        /// <param name="sender" type="Object">
        /// Calendar behavior
        /// </param>
        /// <param name="eventArgs" type="Sys.EventArgs" mayBeNull="false">
        /// Event args
        /// </param>
        this._selectedDate = sender.get_selectedDate();
        this.invalidate();
    },
    
    _onVisibleDateChanged : function(sender, eventArgs) {
        /// <summary>
        /// Handler for Calendar behavior's VisibleDateChanged event
        /// </summary>
        /// <param name="sender" type="Object">
        /// Calendar behavior
        /// </param>
        /// <param name="eventArgs" type="Sys.EventArgs" mayBeNull="false">
        /// Event args
        /// </param>
        var date = sender.get_visibleDate();
        var month = date.getMonth();
        if (this._currentMonth != month) {
            this._currentMonth = sender.get_visibleDate().getMonth();
            this.populate(date);
            //this._timer.set_enabled(true);
        }
    },
    
    _onMapLoad : function(sender, eventArgs) {
        if (this._isPendingMapUpdate) {
            this._map.set_events(this._events);
            this._isPendingMapUpdate = false;
        }
    },
    
    get_servicePath : function() {
        /// <value type="String" mayBeNull="true" optional="true">
        /// The URL of the web service to call.  If the ServicePath is not defined, then we will invoke a PageMethod instead of a web service.
        /// </value>
        return this._servicePath;
    },
    
    set_servicePath : function(value) {
        if (this._servicePath != value) {
            this._servicePath = value;
            this.raisePropertyChanged('servicePath');
        }
    },
    
    get_serviceMethod : function() {
        /// <value type="String">
        /// The name of the method to call on the page or web service
        /// </value>
        /// <remarks>
        /// The signature of the method must exactly match the following:
        ///    [WebMethod]
        ///    string PopulateMethod(string contextKey)
        ///    {
        ///        ...
        ///    }
        /// </remarks>
        return this._serviceMethod;
    },
    
    set_serviceMethod : function(value) {
        if (this._serviceMethod != value) {
            this._serviceMethod = value;
            this.raisePropertyChanged('serviceMethod');
        }
    },
    
    populate : function(contextKey) {
        /// <summary>
        /// Get the service content and use it to populate the target element
        /// </summary>
        /// <param name="contextKey" type="String" mayBeNull="true" optional="true">
        /// An arbitrary string value to be passed to the web method. For example, if the element to be
        /// populated is within a data-bound repeater, this could be the ID of the current row.
        /// </param>
        
        // Initialize the population if this is the very first call
        if (this._currentCallID == -1) {
            var eventArgs = new Sys.CancelEventArgs();
            //this.raisePopulating(eventArgs);
            //if (eventArgs.get_cancel()) {
            //    return;
            //}
            //this._setUpdating(true);
        }
        
        // Either run the custom population script or invoke the web service
        if (this._customScript) {
            // Call custom javascript call to populate control
            var scriptResult = eval(this._customScript);
            this.get_element().innerHTML = scriptResult; 
            this._setUpdating(false);
        } else {
             this._currentCallID = ++this._callID;
             if (this._servicePath && this._serviceMethod) {
                Sys.Net.WebServiceProxy.invoke(this._servicePath, this._serviceMethod, true,
                    { contextKey:(contextKey ? contextKey : this._contextKey) },
                    Function.createDelegate(this, this._onMethodComplete), Function.createDelegate(this, this._onMethodError),
                    this._currentCallID);
                $common.updateFormToRefreshATDeviceBuffer();
             }
        }
    },

    invalidate : function() {
        /// <summary>
        /// Performs layout of the behavior
        /// </summary>
        
        this._performLayout();
    },

    _performLayout : function() {
        /// <summary>
        /// Updates the event list
        /// </summary>
        
        var id = this.get_id();
        var el = this.get_element();
        if (el) {
            for (var i = 0; i < 7; i++) {
                if (el.firstChild) {
                    el.removeChild(el.firstChild);
                }
            }
            if (this._events != null) {
                var ul = $common.createElementFromTemplate({ 
                    nodeName : "ul",
                    properties : { id : id + "-left" }
                }, el);
                for (var i=0; i < this._events.length; i++) {
                    if (i==7) {
                        ul = $common.createElementFromTemplate({ 
                            nodeName : "ul",
                            properties : { id : id + "-right" }
                        }, el);
                    } else if (i>13) {
                        break;
                    }
                    var li = $common.createElementFromTemplate({ 
                        nodeName : "li",
                        properties : { },
                        cssClasses : [ id + "-item" ]
                    }, ul);
                    li.date = this._events[i].DateStart;
                    if (this._isSelected(li.date, 'd')) {
                        Sys.UI.DomElement.addCssClass(li, "on");
                    }
                    var summary = this._formatOverflow(this._events[i].Summary, 35);
                    if (this._events[i].Url != null && this._events[i].Url != "") {
                        summary = "<a href=\"" + encodeURI(this._events[i].Url) + "\" target=\"_blank\">" + summary + "</a>";
                    }
                    $common.createElementFromTemplate({ 
                        nodeName : "h3",
                        properties : {
                            innerHTML : "<strong>" + summary + "</strong>"
                        }
                    }, li);
                    $common.createElementFromTemplate({ 
                        nodeName : "div",
                        properties : {
                            innerHTML : "<strong>When:</strong> " + li.date.localeFormat("MMMM d, yyyy")
                        }
                    }, li);
                    var location = this._events[i].Location ? this._events[i].Location : "";
                    $common.createElementFromTemplate({ 
                        nodeName : "div",
                        properties : {
                            innerHTML : "<strong>Where:</strong> " + this._formatOverflow(location, 30)
                        }
                    }, li);
                }
            }
        }

    },
    
    _onMethodComplete : function (result, userContext, methodName) {
        /// <summary>
        /// Callback used when the populating service returns successfully
        /// </summary>
        /// <param name="result" type="Object" mayBeNull="">
        /// The data returned from the Web service method call
        /// </param>
        /// <param name="userContext" type="Object">
        /// The context information that was passed when the Web service method was invoked
        /// </param>        
        /// <param name="methodName" type="String">
        /// The Web service method that was invoked
        /// </param>

        // Ignore if it's not the current call.
        if (userContext != this._currentCallID) return;
        
        // Save the results for invalidation
        this._events = result;
        this._performLayout();
        
        if (this._map != null && this._map.get_isLoadComplete()) {
            this._map.set_events(this._events);
            this._isPendingMapUpdate = false;
        } else {
            this._isPendingMapUpdate = true;
        }
        
        //this._setUpdating(false);
    },

    _onMethodError : function(webServiceError, userContext, methodName) {
        /// <summary>
        /// Callback used when the populating service fails
        /// </summary>
        /// <param name="webServiceError" type="Sys.Net.WebServiceError">
        /// Web service error
        /// </param>
        /// <param name="userContext" type="Object">
        /// The context information that was passed when the Web service method was invoked
        /// </param>        
        /// <param name="methodName" type="String">
        /// The Web service method that was invoked
        /// </param>

        // ignore if it's not the current call.
        if (userContext != this._currentCallID) return;

        var e = this.get_element();
        if (e) {
            if (webServiceError.get_timedOut()) {
                e.innerHTML = AjaxControlToolkit.Resources.DynamicPopulate_WebServiceTimeout;
            } else {
                e.innerHTML = String.format(AjaxControlToolkit.Resources.DynamicPopulate_WebServiceError, webServiceError.get_statusCode());
            }
        }

        this._events = null;
        //this._setUpdating(false);
    },
    
    _isSelected : function(date, part) {
        /// <summary>
        /// Gets whether the supplied date is the currently selected date
        /// </summary>
        /// <param name="date" type="Date">The date to match</param>
        /// <param name="part" type="String">The most significant part of the date to test</param>
        /// <returns type="Boolean" />
        
        var value = this._selectedDate;
        if (!value) return false;
        switch (part) {
            case 'd':
                if (date.getDate() != value.getDate()) return false;
                // goto case 'M';
            case 'M':
                if (date.getMonth() != value.getMonth()) return false;
                // goto case 'y';
            case 'y':
                if (date.getFullYear() != value.getFullYear()) return false;
                break;
        }
        return true;
    },

    _formatOverflow : function(value, maxlength) {
        if (value && maxlength>=3 && value.length>maxlength) {
            return "<span title=\"" + value + "\">" + value.substr(0, maxlength-3) + "...</span>";
        } else {
            return value;
        }
    }
}
Controls.EventList.registerClass('Controls.EventList', Sys.UI.Control);

// Since this script is not loaded by System.Web.Handlers.ScriptResourceHandler
// invoke Sys.Application.notifyScriptLoaded to notify ScriptManager 
// that this is the end of the script.
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
