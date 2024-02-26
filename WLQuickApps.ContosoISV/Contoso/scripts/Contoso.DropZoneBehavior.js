/// Authors:   Bronwen Zande - bronwen.zande@aptovita.com
///            John O'Brien - john.obrien@aptovita.com

Type.registerNamespace("Contoso");

Contoso.DropZoneBehavior = function(value, ServiceMethod) {
    this.ServiceMethod = ServiceMethod;
    this._onSuccessDelegate = Function.createDelegate(this, this.onSuccess);
    this._onFailedDelegate = Function.createDelegate(this, this.onFailed);
    Contoso.DropZoneBehavior.initializeBase(this, [value]);
    }  
    
Contoso.DropZoneBehavior.prototype = {
    initialize: function(){
        Contoso.DropZoneBehavior.callBaseMethod(this, 'initialize');
        // Register ourselves as a drop target.
        Sys.Preview.UI.DragDropManager.registerDropTarget(this);        
    },
   
    dispose: function(){
        Contoso.DropZoneBehavior.callBaseMethod(this, 'dispose');
    },
    
    get_ServiceMethod: function() {
        return this.ServiceMethod;  
    },
    
    set_ServiceMethod: function(serviceMethod) {
      this.ServiceMethod = serviceMethod;  
    },
    
    // IDropTarget members.
    get_dropTargetElement: function() {
        return this.get_element();
    },
    drop: function(dragMode, type, data) { 
       Sys.UI.DomElement.toggleCssClass(this.get_dropTargetElement(), "dragover");
        this.get_dropTargetElement().innerHTML = "loading...";
        this.ServiceMethod(type,data,this._onSuccessDelegate,this._onFailedDelegate);
    },
    canDrop: function(dragMode, dataType) {
        return true;
    },
    onDragEnterTarget: function(dragMode, type, data) {
       Sys.UI.DomElement.toggleCssClass(this.get_dropTargetElement(), "dragover");
    },
    onDragLeaveTarget: function(dragMode, type, data) {
       Sys.UI.DomElement.toggleCssClass(this.get_dropTargetElement(), "dragover");
    },
    onDragInTarget: function(dragMode, type, data) {
    },
    onSuccess: function(result) {
        this.get_dropTargetElement().innerHTML = result;
    },    
    onFailed: function(error) {
        var stackTrace = error.get_stackTrace();
        var message = error.get_message();
        var statusCode = error.get_statusCode();
        var exceptionType = error.get_exceptionType();
        var timedout = error.get_timedOut();

        // Display the error.    
        var RsltElem = 
            "Stack Trace: " +  stackTrace + "<br/>" +
            "Service Error: " + message + "<br/>" +
            "Status Code: " + statusCode + "<br/>" +
            "Exception Type: " + exceptionType + "<br/>" +
            "Timedout: " + timedout;
            
            alert(RsltElem);    
    }
}

Contoso.DropZoneBehavior.registerClass('Contoso.DropZoneBehavior', Sys.UI.Behavior, Sys.Preview.UI.IDragSource, Sys.Preview.UI.IDropTarget, Sys.IDisposable);

if(typeof(Sys) !== "undefined") Sys.Application.notifyScriptLoaded();







