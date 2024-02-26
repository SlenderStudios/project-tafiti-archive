/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com

Type.registerNamespace("Contoso");

Contoso.DragSourceBehavior = function(element, DataType, ID)
{
    Contoso.DragSourceBehavior.initializeBase(this, [element]);
    this._mouseDownHandler = Function.createDelegate(this, this.mouseDownHandler);
    this._visual = null;
    this.DataType = DataType;
    this.ID = ID;
}

Contoso.DragSourceBehavior.prototype =
{

    get_DataType: function()
    {
        return this.DataType;
    },
    
    set_DataType: function(DataType)
    {
        this.DataType = DataType;
    },

    get_ID: function()
    {
        return this.ID;
    },
    
    set_ID: function(ID)
    {
        this.ID = ID;
    },
   
    // IDragSource methods
    get_dragDataType: function()
    {
        return this.DataType;
    },

    getDragData: function(context)
    {
        return this.ID;
    },

    get_dragMode: function()
    {
        return Sys.Preview.UI.DragMode.Copy;
    },

    onDragStart: function()
    {
    },

    onDrag: function()
    {
    },

    onDragEnd: function(canceled)
    {
        if (this._visual)
            document.body.removeChild(this._visual);
    },
    
    // Other methods
    initialize: function()
    {
        Contoso.DragSourceBehavior.callBaseMethod(this, 'initialize');
        $addHandler(this.get_element(), 'mousedown', this._mouseDownHandler)
    },

    mouseDownHandler: function(ev)
    {
        window._event = ev; // Needed internally by _DragDropManager

        this._visual = this.get_element().cloneNode(true);
        this._visual.style.opacity = '0.8';
        this._visual.style.zIndex = 99999;
        this._visual.style.position = "absolute";
        this._visual.style.float="none";
        document.body.appendChild(this._visual);
        var location = Sys.UI.DomElement.getLocation(this.get_element());
        Sys.UI.DomElement.setLocation(this._visual, location.x, location.y);

        Sys.Preview.UI.DragDropManager.startDragDrop(this, this._visual, null);
    },

    dispose: function()
    {
        if (this._mouseDownHandler) $removeHandler(this.get_element(), 'mousedown', this._mouseDownHandler);
        this._mouseDownHandler = null;
        Contoso.DragSourceBehavior.callBaseMethod(this, 'dispose');
    }
}

Contoso.DragSourceBehavior.registerClass('Contoso.DragSourceBehavior', Sys.UI.Behavior, Sys.Preview.UI.IDragSource);
 
if(typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();