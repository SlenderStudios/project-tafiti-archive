/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com
 

Type.registerNamespace("Contoso");

Contoso.ModalPopup = function() {
    
    // Properties
    this._BackgroundCssClass = "modalBackground";  
    this._foregroundCssClass = "modalPopup";
    this._contentCssClass = "modalPopupContent";
    this._OkCssClass = "modalPopupOK";
    this._xCoordinate = -1;
    this._yCoordinate = -1;

    // Variables
    this._backgroundElement = null;
    this._foregroundElement = null;
    this._contentElement = null;
    this._okElement = null;
    this._okHandler = null;
    this._scrollHandler = null;
    this._resizeHandler = null;
    this._windowHandlersAttached = false;

    this._saveTabIndexes = new Array();
    this._saveDesableSelect = new Array();
    this._tagWithTabIndex = new Array('A','AREA','BUTTON','INPUT','OBJECT','SELECT','TEXTAREA','IFRAME');
    
    this.init();
}
Contoso.ModalPopup.prototype = {
    init : function() {
        /// <summary>
        /// Initialize
        /// </summary>
       
        this._foregroundElement = document.createElement('div');
        this._foregroundElement.className = this._foregroundCssClass;
        document.body.appendChild(this._foregroundElement);
        
        this._contentElement = document.createElement('div');
        this._contentElement.className = this._contentCssClass;
        this._foregroundElement.appendChild(this._contentElement);
        
        this._okElement = document.createElement('div');
        this._okElement.innerHTML = "Close";
        this._okElement.className = this._OkCssClass;
        this._foregroundElement.appendChild(this._okElement);       
        
        this._backgroundElement = document.createElement('div');
        this._backgroundElement.style.display = 'none';
        this._backgroundElement.style.position = 'absolute';
        this._backgroundElement.style.left = '0px';
        this._backgroundElement.style.top = '0px';
        // Want zIndex to big enough that the background sits above everything else
        // CSS 2.1 defines no bounds for the <integer> type, so pick arbitrarily
        this._backgroundElement.style.zIndex = 10000;
        if (this._BackgroundCssClass) {
            this._backgroundElement.className = this._BackgroundCssClass;
        }
        this._foregroundElement.parentNode.appendChild(this._backgroundElement);

        this._foregroundElement.style.display = 'none';
        this._foregroundElement.style.position = 'absolute';
        this._foregroundElement.style.zIndex = CommonToolkitScripts.getCurrentStyle(this._backgroundElement, 'zIndex', this._backgroundElement.style.zIndex) + 1;
        
        this._okHandler = Function.createDelegate(this, this._onOk);
        $addHandler(this._okElement, 'click', this._okHandler);

        this._scrollHandler = Function.createDelegate(this, this._onLayout);
        this._resizeHandler = Function.createDelegate(this, this._onLayout);
        
        this.hide();

    },

    dispose : function() {
        /// <summary>
        /// Dispose the behavior
        /// </summary>
        this._detachPopup();

        this._scrollHandler = null;
        this._resizeHandler = null;

        if (this._okHandler && $get(this._OkControlID)) {
            $removeHandler($get(this._OkControlID), 'click', this._okHandler);
            this._okHandler = null;
        }
        
    },

    _attachPopup : function() {
        /// <summary>
        /// Attach the event handlers for the popup
        /// </summary>
      
        $addHandler(window, 'resize', this._resizeHandler);
        $addHandler(window, 'scroll', this._scrollHandler);
        this._windowHandlersAttached = true;
    },

    _detachPopup : function() {
        /// <summary>
        /// Detach the event handlers for the popup
        /// </summary>

        if (this._windowHandlersAttached) {
            if (this._scrollHandler) {
                $removeHandler(window, 'scroll', this._scrollHandler);
            }

            if (this._resizeHandler) {
                $removeHandler(window, 'resize', this._resizeHandler);
            }
            this._windowHandlersAttached = false;
        }    
    },

    _onOk : function(e) {
        /// <summary>
        /// Handler for the modal dialog's OK button click
        /// </summary>
        /// <param name="e" type="Sys.UI.DomEvent">
        /// Event info
        /// </param>

        this.hide();
        this._contentElement.innerHTML = "";
        e.preventDefault();
        return false;

    },

    _onLayout : function() {
        /// <summary>
        /// Handler for scrolling and resizing events that would require a repositioning of the modal dialog
        /// </summary>
        this._layout();
    },

    show : function(messageHTML) {
        /// <summary>
        /// Display the element referenced by PopupControlID as a modal dialog
        /// </summary>
        
        //TODO: set content to messageHTML
        this._contentElement.innerHTML = messageHTML;
        this._contentElement.className = "modalPopupContent";

        this._attachPopup();

        this._backgroundElement.style.display = '';
        this._foregroundElement.style.display = '';

        // Disable TAB
        this.disableTab();

        this._layout();
        // On pages that don't need scrollbars, Firefox and Safari act like
        // one or both are present the first time the layout code runs which
        // obviously leads to display issues - run the layout code a second
        // time to work around this problem
        this._layout();
        
    },

    disableTab : function() {
        /// <summary>
        /// Change the tab indices so we only tab through the modal popup
        /// (and hide SELECT tags in IE6)
        /// </summary>

        var i = 0;
        var tagElements;
        var tagElementsInPopUp = new Array();
        Array.clear(this._saveTabIndexes);

        //Save all popup's tag in tagElementsInPopUp
        for (var j = 0; j < this._tagWithTabIndex.length; j++) {
            tagElements = this._foregroundElement.getElementsByTagName(this._tagWithTabIndex[j]);
            for (var k = 0 ; k < tagElements.length; k++) {
                tagElementsInPopUp[i] = tagElements[k];
                i++;
            }
        }

        i = 0;
        for (var j = 0; j < this._tagWithTabIndex.length; j++) {
            tagElements = document.getElementsByTagName(this._tagWithTabIndex[j]);
            for (var k = 0 ; k < tagElements.length; k++) {
                if (Array.indexOf(tagElementsInPopUp, tagElements[k]) == -1)  {
                    this._saveTabIndexes[i] = {tag: tagElements[k], index: tagElements[k].tabIndex};
                    tagElements[k].tabIndex="-1";
                    i++;
                }
            }
        }

        //IE6 Bug with SELECT element always showing up on top
        i = 0;
        if ((Sys.Browser.agent === Sys.Browser.InternetExplorer) && (Sys.Browser.version < 7)) {
            //Save SELECT in PopUp
            var tagSelectInPopUp = new Array();
            for (var j = 0; j < this._tagWithTabIndex.length; j++) {
                tagElements = this._foregroundElement.getElementsByTagName('SELECT');
                for (var k = 0 ; k < tagElements.length; k++) {
                    tagSelectInPopUp[i] = tagElements[k];
                    i++;
                }
            }

            i = 0;
            Array.clear(this._saveDesableSelect);
            tagElements = document.getElementsByTagName('SELECT');
            for (var k = 0 ; k < tagElements.length; k++) {
                if (Array.indexOf(tagSelectInPopUp, tagElements[k]) == -1)  {
                    this._saveDesableSelect[i] = {tag: tagElements[k], visib: CommonToolkitScripts.getCurrentStyle(tagElements[k], 'visibility')} ;
                    tagElements[k].style.visibility = 'hidden';
                    i++;
                }
            }
        }
    },

    restoreTab : function() {
        /// <summary>
        /// Restore the tab indices so we tab through the page like normal
        /// (and restore SELECT tags in IE6)
        /// </summary>

        for (var i = 0; i < this._saveTabIndexes.length; i++) {
            this._saveTabIndexes[i].tag.tabIndex = this._saveTabIndexes[i].index;
        }

        //IE6 Bug with SELECT element always showing up on top
        if ((Sys.Browser.agent === Sys.Browser.InternetExplorer) && (Sys.Browser.version < 7)) {
            for (var k = 0 ; k < this._saveDesableSelect.length; k++) {
                this._saveDesableSelect[k].tag.style.visibility = this._saveDesableSelect[k].visib;
            }
        }
    },

    hide : function() {
        /// <summary>
        /// Hide the modal dialog
        /// </summary>
        
        this._contentElement.innerHTML = "";        
        
        this._backgroundElement.style.display = 'none';
        this._foregroundElement.style.display = 'none';

        this.restoreTab();

        this._detachPopup();
        
    },

    _layout : function() {
        /// <summary>
        /// Position the modal dialog in the center of the screen
        /// </summary>

        var scrollLeft = (document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft);
        var scrollTop = (document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop);
        var clientBounds = CommonToolkitScripts.getClientBounds();
        var clientWidth = clientBounds.width;
        var clientHeight = clientBounds.height;
        this._backgroundElement.style.width = Math.max(Math.max(document.documentElement.scrollWidth, document.body.scrollWidth), clientWidth)+'px';
        this._backgroundElement.style.height = Math.max(Math.max(document.documentElement.scrollHeight, document.body.scrollHeight), clientHeight)+'px';

        var isIE6 = (Sys.Browser.agent == Sys.Browser.InternetExplorer && Sys.Browser.version < 7);
        if(this._xCoordinate < 0)
        {
            this._foregroundElement.style.left = scrollLeft+((clientWidth-this._foregroundElement.offsetWidth)/2)+'px';
            this._foregroundElement.style.width = "460px";
            this._contentElement.style.width = "100%";
        }
        else
        {
            if(isIE6)
            {
                this._foregroundElement.style.position = 'absolute';
                this._foregroundElement.style.left = (this._xCoordinate + scrollLeft) + 'px';
            }
            else
            {
                this._foregroundElement.style.position = 'fixed';
                this._foregroundElement.style.left = this._xCoordinate + 'px';
            }
        }
        if(this._yCoordinate < 0)
        {
            this._foregroundElement.style.top = scrollTop+((clientHeight-this._foregroundElement.offsetHeight)/2)+'px';
            this._foregroundElement.style.height = "231px";
            this._contentElement.style.height = "95%";
        }
        else
        {
            if(isIE6)
            {
                this._foregroundElement.style.position = 'absolute';
                this._foregroundElement.style.top = (this._yCoordinate + scrollTop) + 'px';
            }
            else
            {
                this._foregroundElement.style.position = 'fixed';
                this._foregroundElement.style.top = this._yCoordinate + 'px';
            }
        }
        
    }
}
Contoso.ModalPopup.registerClass('Contoso.ModalPopup', null);

if (typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();