<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IMWindow.aspx.cs" Inherits="IMWindow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    
    <style type="text/css">
        body { font-family: Arial; }
        .userText { font: 10pt Arial; color: #69C; font-weight: bold; }
        .messageText { font: 10pt Arial; }
        #messageText { font: 10pt Arial; }
        
        .addFriend{vertical-align: bottom;}
        html>/**/body #send {margin-top: 1px;}
        
    </style>
</head>
<body onload="onLoad()" onunload="closeIM()" style="background:#08254c url(images/background.jpg) no-repeat; text-align:center;">
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>

    <script type="text/javascript">
    
    var emailHash;
    
    function onLoad()
    {
        try
        {
            emailHash = window.location.search.substring(1).split("=")[1];
        }
        catch(e)
        {
            emailHash = "";
        }
        
        if (emailHash == "")
        {
            window.close();
        }
    
        setDisplayName(window.opener.getDisplayName(emailHash));
        
        if (window.opener.isContact(emailHash))
        {
            document.getElementById("addContactLink").innerHTML = "";
        }
        
        document.getElementById("messageText").focus();        
    }
    
    function onMessageKeyPress(e)
    {
        var code;
        if (window.event)
        {
            code = window.event.keyCode;
        }
        else if (e.which)
        {
            code = e.which;
        }
        else return true;
        
        if (code == 13)
        {
            sendMessage(); 
            return false;
        }
        return true;
    }

    function appendMessage(displayName, message)
    {    
        var conversationDiv = document.getElementById("conversation");

        var userDiv = document.createElement("div");
        userDiv.className = "userText";
        userDiv.appendChild(document.createTextNode(displayName + " said: "));
        conversationDiv.appendChild(userDiv);
        
        var messageDiv = document.createElement("div");
        messageDiv.className = "messageText";
        messageDiv.appendChild(document.createTextNode(message));
        conversationDiv.appendChild(messageDiv);
        
        conversationDiv.scrollTop = conversationDiv.scrollHeight;
    }
    
    function sendMessage()
    {
        var messageBox = document.getElementById("messageText");
        if (messageBox.value == "") { return; }
        var message = messageBox.value;
        messageBox.value = "";
               
        if (window.opener && window.opener.sendIM)
        {
            window.opener.sendIM(emailHash, message);
        }
        else
        {
            // TODO: Conversation is over. How to handle?
        }
        
        // TODO: Appending regardless of success for debugging.
        appendMessage("You", message);
        messageBox.focus();
    }
    
    function receiveMessage(displayName, message)
    {
        setDisplayName(displayName);
        
        appendMessage(displayName, message);
    }
    
    function addContact()
    {
        if (window.opener && window.opener.addContact)
        {
            window.opener.addContact(emailHash);
            document.getElementById("addContactLink").innerHTML = "";
        }       
    }
    
    function setDisplayName(displayName)
    {
        document.title = "IM Chat With " + displayName;
        var displayNameSpan = document.getElementById("displayName");
        displayNameSpan.innerHTML = "";
        var textNode = document.createTextNode(displayName);
        displayNameSpan.appendChild(textNode);
    }
    
    function closeIM()
    {
        if (window.opener && window.opener.closeIM)
        {
            window.opener.closeIM(emailHash);
        }
    }
    
    </script>
    
    <div id="framing" style="width:400px; max-height:600px;text-align:left;background-color:#FFF;">
        <div id="header" style="background: url(images/results_hdr.png) repeat-x top center; height: 77px; z-index: 2; text-align: center; padding-top: 15px; font-size: 8pt;">
            <span id="displayName"></span>
            <a id="addContactLink" href="javascript:addContact()"><img src="images/add.png" style="border:0" /></a>
        </div>
        <div id="conversation" style="overflow:auto;min-height:400px;max-height:400px;margin-top: -30px; padding: 5px; word-wrap: break-word; z-index: 1;"></div>
        <div id="inputSpace" style="background-color:#FFF; overflow:auto; width: 100%; border-top: solid 1px #9eb6c0">
            <textarea id="messageText" rows="10" cols="100" onkeypress="return onMessageKeyPress(event)" style="height:80px;width:330px; overflow:auto; border: transparent; float: left;"></textarea>
            <div id="send" onclick="sendMessage()" style="background: url(images/send.png) no-repeat #FFF bottom right; overflow:auto; width:58px; height:80px; cursor:hand; float: left;" />
        </div>
    </div>        
</body>
</html>
