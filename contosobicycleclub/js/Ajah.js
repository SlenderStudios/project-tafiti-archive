// AJAH: Asynchronous JavaScript and HTML Utilities

// Create the object
function createXMLHttpRequest() {
    try { return new ActiveXObject("Msxml2.XMLHTTP"); } catch (e) {}
    try { return new ActiveXObject("Microsoft.XMLHTTP"); } catch (e) {}
    try { return new XMLHttpRequest(); } catch (e) {}
    alert("This browser does not support Ajax");
    return null;
}

// Fetch and object by ID (dev performance measure)
function $(id) { return document.getElementById(id); }

// Make a HTTP call
function loadHTML( url, div) {
    var xmlHttpRequest = createXMLHttpRequest();

    var handler = function() {
        if (xmlHttpRequest.readyState==4) {
            if (xmlHttpRequest.status==200) {
                div.innerHTML = xmlHttpRequest.responseText;
            } else {
                alert('Error - LoadHTML failed.');
            }
        }
    }
    
    xmlHttpRequest.onreadystatechange = handler;
    xmlHttpRequest.open("GET", url, true);
    xmlHttpRequest.send(null);

}

if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded(); 