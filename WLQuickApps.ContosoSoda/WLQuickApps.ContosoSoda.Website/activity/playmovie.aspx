<%@ Page Language="C#" AutoEventWireup="true" CodeFile="playmovie.aspx.cs" Inherits="activity_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>Share Video</title>
<script language="javascript1.4" type="text/javascript">
var Channel = window.external.Channel;

	function Channel_OnDataReceived(){
	 samplename.innerHTML=Channel.Data;

    }
    function Channel_SendData(message){
		Channel.SendData(message);
    }
    function getCookie(c_name){
        if (document.cookie.length>0)
        {
            c_start=document.cookie.indexOf(c_name + "=");
            if (c_start!=-1)
            { 
                c_start=c_start + c_name.length+1; 
                c_end=document.cookie.indexOf(";",c_start);
                if (c_end==-1) c_end=document.cookie.length;
                return unescape(document.cookie.substring(c_start,c_end));
            } 
        }
    return "";
    }
</script>
<script type="text/javascript" src="../js/swfobject.js"></script>
<link rel="stylesheet" href="../css/style.css" type="text/css">
	
</head>
	
<body>
<div id="flashcontent">
				<h3>You need to upgrade your Flash Player</h3>
				<p>This site requires a more recent version of the Flash plugin than you currently have. This plugin is free and can be downloaded <a href="http://www.adobe.com/products/flashplayer/" target="_blank">here</a>.</p>
				<p>Or, if you're absolutely positive you have the most recent plugin, then click <a href="flash_home.html?detectflash=false" target="_self">here</a> to force the site to load.</p>
			</div>
			<script type="text/javascript">
			// <![CDATA[
			var so = new SWFObject("../swf/share_video.swf"+"?v="+Math.random(), "share_video", "483", "463", "8", "#ffffff"); // swf, id, width, height, version, background-color
			//so.addParam("wmode", "transparent");
			so.addParam("scale", "noscale");
			so.useExpressInstall("swf/expressinstall.swf");
			so.write("flashcontent");
			// ]]>
			</script>

<span id="samplename"></span>
<script language="javascript1.4" type="text/javascript">

if (getCookie("CONTENTID") == ""){
    samplename.innerHTML="eeeeeeeeeeee";
}else{
    Channel_SendData(getCookie("CONTENTID"));
    samplename.innerHTML=getCookie("CONTENTID");
}
</script>
<map name="Map"><area shape="rect" coords="74,327,212,353" href="#"></map></body>
</html>