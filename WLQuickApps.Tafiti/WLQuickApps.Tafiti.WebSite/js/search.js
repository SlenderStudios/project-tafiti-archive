// Copyright 2007 Microsoft Corp. All Rights Reserved.

function SearchClient() {

  // public methods
  this.search = search;
  
  function search(scope, query, offset, count, resultCallback) {
    var queryString = 
          "?q=" + encodeURIComponent(query) +
          "&first=" + encodeURIComponent(offset) +
          "&count=" + encodeURIComponent(count) +
          "&format=xml&adlt=strict";
    SJ.AsyncRequest("GET", "Search.aspx/" + encodeURIComponent(scope) + queryString, null, resultCallback);
  }
    
}
