var CHATWINDOWS = null; // collection of open chat windows
var CHATUSERS = null; // collection of chat users
var CHATUSERSLIST = null; // HTML parent container of chat user list


function InitPresence() {
	CHATWINDOWS = new Array();
	CHATUSERS = new Array();
	CHATUSERSLIST = $get("cbc-ChatUsersList");
}


function OpenChatWindow(id) {
	var token = id.substring(0, id.indexOf("@"));

	// is the chat window already captured?
	if (!CHATWINDOWS[token] || CHATWINDOWS[token].closed || !CHATWINDOWS[token].location) {
		// a blank URL gets an existing window if it exists, otherwise opens a new blank window
		CHATWINDOWS[token] = window.open('','chat' + token,'width=300,height=400');
		// IE does not allow you to read the location of a window if it's not in the same domain, so we use a try catch.
		try {
			// if the location is blank, load in the IM client
			if (CHATWINDOWS[token].location.href == 'about:blank') {
				CHATWINDOWS[token].location.href = 'http://settings.messenger.live.com/Conversation/IMMe.aspx?invitee=' + id + '&mkt=en-CA&useTheme=true&foreColor=333333&backColor=f2ece3&linkColor=333333&borderColor=ffffff&buttonForeColor=333333&buttonBackColor=f2ece3&buttonBorderColor=e2d7bf&buttonDisabledColor=ffffff&headerForeColor=333333&headerBackColor=e2d7bf&menuForeColor=333333&menuBackColor=f2ece3&chatForeColor=333333&chatBackColor=FFFFFF&chatDisabledColor=F6F6F6&chatErrorColor=760502&chatLabelColor=6E6C6C';
			}
		} catch (err) {
		}
	}

	if (window.focus) CHATWINDOWS[token].focus();
}


function ShowPresence(presence)
{
  var noUsers = $get("cbc-NoChatUsers");

  var statusIcon = document.createElement("img");
  statusIcon.src = presence.icon.url;
  statusIcon.width = presence.icon.width;
  statusIcon.height = presence.icon.height;
  statusIcon.alt = presence.statusText;
  statusIcon.title = presence.statusText;
  
  var displayName = document.createElement('span');
  displayName.innerHTML = SecureHtml(CHATUSERS[presence.id].displayName); //BugID: 170705

  var li = document.createElement("li");
  li.appendChild(statusIcon);
  li.appendChild(displayName);
  li.onclick = function () {
		OpenChatWindow(presence.id);
  };
  li.onmouseover = function () {
		Sys.UI.DomElement.addCssClass(this, "hover");
  };
  li.onmouseout = function () {
		Sys.UI.DomElement.removeCssClass(this, "hover");
  };
  
  
	if (presence.statusText == "Online") {
		if (noUsers) noUsers.parentNode.removeChild(noUsers);
		if (CHATUSERSLIST) CHATUSERSLIST.appendChild(li);
	}
}


function ShowOwnerPresence(presence)
{
  var owner = $get("cbc-EventOwner");

  var statusIcon = document.createElement("img");
  statusIcon.src = presence.icon.url;
  statusIcon.width = presence.icon.width;
  statusIcon.height = presence.icon.height;
  statusIcon.alt = presence.statusText;
  statusIcon.title = presence.statusText;
  
  var displayName = document.createElement('span');
  displayName.innerHTML = SecureHtml(owner.innerHTML);

  RemoveChildNodes(owner);

  owner.appendChild(statusIcon);
  owner.appendChild(displayName);

  if (presence.statusText == "Online") {
		owner.onclick = function () {
			OpenChatWindow(presence.id);
		};
		owner.onmouseover = function () {
			Sys.UI.DomElement.addCssClass(this, "hover");
		};
		owner.onmouseout = function () {
			Sys.UI.DomElement.removeCssClass(this, "hover");
		};
	}
}



/* removes all child nodes from a given element */
function RemoveChildNodes(el) {
  while (el.childNodes[0]) {
    el.removeChild(el.childNodes[0]);
  }
}



function SecureHtml (s) {
	var secured = s;
	secured = secured.replace(/</g, "&lt;").replace(/>/g, "&gt;");
  secured = secured.replace(/script(.*)/g, "");    
  secured = secured.replace(/eval\((.*)\)/g, "");
	return (secured);
}



if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded(); 