// register your Gadget's namespace
registerNamespace("WLQuickApps.ContosoBank");

// define the constructor for your Gadget (this must match the name in the manifest xml)
WLQuickApps.ContosoBank.ContosoCupGadget = function(p_elSource, p_args, p_namespace) {
    // always call initializeBase before anything else!
	WLQuickApps.ContosoBank.ContosoCupGadget.initializeBase(this, arguments);

    // setup private member variables
	var m_this = this;
	var m_el = p_elSource;

	/****************************************
	**          initialize Method
	****************************************/
    // initialize is always called immediately after your object is instantiated
	this.initialize = function(p_objScope)
	{
	    // always call the base object's initialize first!	
		WLQuickApps.ContosoBank.ContosoCupGadget.getBaseMethod(this, "initialize", "Web.Bindings.Base").call(this, p_objScope);

        GetFeed();
	
	};
	WLQuickApps.ContosoBank.ContosoCupGadget.registerBaseMethod(this, "initialize");
	
	
	/****************************************	
	**           dispose Method
	****************************************/
	this.dispose = function(p_blnUnload) {
	    //TODO: add your dispose code here
	    
	    // null out all member variables
	    m_this = null;
	    m_el = null;
	    
        // always call the base object's dispose last!	
		WLQuickApps.ContosoBank.ContosoCupGadget.getBaseMethod(this, "dispose", "Web.Bindings.Base").call(this, p_blnUnload);
	};
	WLQuickApps.ContosoBank.ContosoCupGadget.registerBaseMethod(this, "dispose");

	/****************************************
	** Other Methods
	****************************************/
		//----------------------------------------------------------------------------
	//	
	//	Method:		GetFeed()
	//
	//	Synopsis:	Fetches Leaderboard
	//
	//	Arguments:	none
	//
	//	Returns:	nothing
	//
	//----------------------------------------------------------------------------

	function GetFeed()
	{
        var url = "http://contosobnk.com/Services/ContosoBankService.asmx/GetContosoCupLeaderBoard?";
        var aObjHeaders = new Array();
        aObjHeaders["Content-Type"] = "text/xml";

        var strPostArgs = new Web.StringBuilder();
        var r = Web.Network.createRequest( Web.Network.Type.XMLGet,                      // Network type
                                           url,                                          // URL of web service
                                           null,                                         // Object passed to callback
                                           OnFeedReceived);                              // Callback for webservice call
		r.execute();
	}
	
	//----------------------------------------------------------------------------
	//
	//	Method:		OnFeedReceived()
	//
	//	Synopsis:	Callback handler for fetching of rss data
	//
	//	Arguments:	response			- xml response
	//
	//	Returns:	nothing
	//
	//----------------------------------------------------------------------------

	function OnFeedReceived(response)
	{
        // Check response code to make sure it was successful
        if ( response.status == 200 )
        {
            // make sure you have the proper casing in the line below.
            // IE6 will also tolerate response.responseXml, but it will not work in other browsers.
            var root = response.responseXML.documentElement;
            if (root)
            {
               ProcessSoapBody(root);
            }
        }
        else
        {
            m_el.innerHTML = "Error";
        }
        
	}
		
	function ProcessSoapBody(node)
	{
        for (var i = 0; i < node.childNodes.length; ++i)
        {
          if (node.childNodes[i].nodeName == "Ladder")
          {
            ProcessLader(node.childNodes[i]);
          }
          else if (node.childNodes[i].nodeName == "PlayerOfWeek")
          {
            ProcessPlayerofWeek(node.childNodes[i]);
          }
          else
          {            
            ProcessSoapBody(node.childNodes[i]);
          }
        }
	}

    function ProcessLader(ladder)
    {
        var div1 = document.createElement("div");
        div1.className = "CupLeader";
        m_el.appendChild(div1);   

        var table = document.createElement("table");
        div1.appendChild(table);
        
        var tablebody = document.createElement("tbody");
        table.appendChild(tablebody);
        
        var headerRow = document.createElement("tr");
        tablebody.appendChild(headerRow);
        
        var header1 = document.createElement("th");
        headerRow.appendChild(header1);
        
        var header2 = document.createElement("th");
        header2.innerHTML = "P";
        headerRow.appendChild(header2);
        
        var header3 = document.createElement("th");
        header3.innerHTML = "W";
        headerRow.appendChild(header3);
        
        var header4 = document.createElement("th");
        header4.innerHTML = "L";
        headerRow.appendChild(header4);
        
        var header5 = document.createElement("th");
        header5.innerText = "D";
        headerRow.appendChild(header5);

        for (var i = 0; i < ladder.childNodes.length; ++i)
        {
           var row = document.createElement("tr");
           tablebody.appendChild(row);

            for (var j = 0; j < ladder.childNodes[i].childNodes.length; ++j)
            {
                if (ladder.childNodes[i].childNodes[j].nodeName == "Name")
                {
                    var team = document.createElement("td");
                    team.className = "Team";
                    team.innerHTML = ladder.childNodes[i].childNodes[j].text;
                    row.appendChild(team);                
                }
                else if (ladder.childNodes[i].childNodes[j].nodeName == "Played" ||
                    ladder.childNodes[i].childNodes[j].nodeName == "Won" ||
                    ladder.childNodes[i].childNodes[j].nodeName == "Lost" ||
                    ladder.childNodes[i].childNodes[j].nodeName == "Draw" )
                {
                    var score = document.createElement("td");
                    score.className = "Score";
                    score.innerHTML = ladder.childNodes[i].childNodes[j].text;
                    row.appendChild(score);
                }
            }
        }     
    }
    
    function ProcessPlayerofWeek(player)
    {
        var div1 = document.createElement("div");
        div1.className = "CupPlayer";
        div1.innerText = "Player of the week - " + player.text;
        m_el.appendChild(div1);
    }

};
WLQuickApps.ContosoBank.ContosoCupGadget.registerClass("WLQuickApps.ContosoBank.ContosoCupGadget", "Web.Bindings.Base");
