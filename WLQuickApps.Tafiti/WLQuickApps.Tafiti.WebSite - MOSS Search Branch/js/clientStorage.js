// Copyright 2007 Microsoft Corp. All Rights Reserved.

SJ.ClientStorage = {};

SJ.ClientStorage.createInstance = function () {
    // detect browser and return appropriate storage instance
    if (BrowserDetect.browser == "Explorer" && BrowserDetect.version >= 5 && document.getElementById('IeStorage')) {
        return new SJ.ClientStorage.IeStorage(document.getElementById('IeStorage'));
    }
    else if (BrowserDetect.browser == "Firefox" && BrowserDetect.version >= 2) {
        return new SJ.ClientStorage.WhatStorage();
    }
    else {
        return new SJ.ClientStorage.VoidStorage();
    }
}

//
// Void storage
//

SJ.ClientStorage.VoidStorage = function () {
    this.contents = {};
}

SJ.ClientStorage.VoidStorage.prototype.load = function () {
}

SJ.ClientStorage.VoidStorage.prototype.save = function () {
}

SJ.ClientStorage.VoidStorage.prototype.getValue = function (key) {
    return this.contents[key];
}

SJ.ClientStorage.VoidStorage.prototype.setValue = function (key,value) {
    this.contents[key] = value;
}


//
// WHAT Working Group storage
// See http://www.whatwg.org/
//

SJ.ClientStorage.WhatStorage = function () {
    if (window.location.hostname != 'localhost') // not supported; navigate using 127.0.0.1 instead
        this.contents = globalStorage[window.location.hostname];
    else
        this.contents = {};
}

SJ.ClientStorage.WhatStorage.prototype.toString = function () {
    return "SJ.WhatStorage";
}

SJ.ClientStorage.WhatStorage.prototype.load = function () {
    // not necessary
}

SJ.ClientStorage.WhatStorage.prototype.save = function () {
    // not necessary
}

SJ.ClientStorage.WhatStorage.prototype.getValue = function(key) {
    return this.contents[key];
}

SJ.ClientStorage.WhatStorage.prototype.setValue = function(key,value) {
    this.contents[key] = value;
}



//
// Internet Explorer storage using "behaviors"
// See http://msdn.microsoft.com/workshop/author/persistence/howto/sessiondata.asp
//

SJ.ClientStorage.IeStorage = function (element) {
    this.contents = element;
}

SJ.ClientStorage.IeStorage.prototype.load = function () {
    this.contents.load('store');
}

SJ.ClientStorage.IeStorage.prototype.save = function () {
    this.contents.save('store');
}

SJ.ClientStorage.IeStorage.prototype.getValue = function(key) {
    return this.contents.getAttribute(key);
}

SJ.ClientStorage.IeStorage.prototype.setValue = function(key,value) {
    this.contents.setAttribute(key,value);
}