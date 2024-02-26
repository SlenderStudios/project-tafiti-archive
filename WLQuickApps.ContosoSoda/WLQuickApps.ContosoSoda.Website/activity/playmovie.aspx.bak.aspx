<%@ Page Language="C#" AutoEventWireup="true" CodeFile="playmovie.aspx.cs" Inherits="activity_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Untitled Page</title>
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
<script language="javascript">
	AC_FL_RunContent = 0;
	
	</script>
	<script src="AC_RunActiveContent.js" language="javascript"></script>
	<link rel="stylesheet" href="../css/style.css" type="text/css">
	
</head>
	
<body>
    
	
       <table border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td colspan="3"><img src="images/videosharing_top.jpg" width="480" height="100"></td>
    </tr>
  <tr>
    <td><img src="images/left.jpg" width="80" height="269"></td>
    <td><table border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td><script language="javascript">
	if (AC_FL_RunContent == 0) {
		alert("This page requires AC_RunActiveContent.js.");
	} else {
		AC_FL_RunContent(
			'codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0',
			'width', '320',
			'height', '240',
			'src', 'play_flv',
			'quality', 'high',
			'pluginspage', 'http://www.macromedia.com/go/getflashplayer',
			'align', 'middle',
			'play', 'true',
			'loop', 'true',
			'scale', 'showall',
			'wmode', 'window',
			'devicefont', 'false',
			'id', 'play_flv',
			'bgcolor', '#333333',
			'name', 'play_flv',
			'menu', 'true',
			'allowFullScreen', 'false',
			'allowScriptAccess','sameDomain',
			'movie', 'play_flv',
			'salign', ''
		); //end AC code
	}
	</script>	<noscript>
		<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=9,0,0,0" width="320" height="240" id="play_flv" align="middle">
			<param name="allowScriptAccess" value="sameDomain" />
			<param name="allowFullScreen" value="false" />
			<param name="movie" value="play_flv.swf" />
			<param name="quality" value="high" />
			<param name="bgcolor" value="#FFFFFF" />
			<embed src="play_flv.swf" quality="high" bgcolor="#FFFFFF" width="320" height="240" name="play_flv" align="middle" allowScriptAccess="sameDomain" allowFullScreen="false" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
		</object>
	</noscript></td>
      </tr>
      <tr>
        <td><img src="images/middle.jpg" width="320" height="27"></td>
      </tr>
    </table></td>
    <td><img src="images/right.jpg" width="80" height="269"></td>
  </tr>
  <tr>
    <td colspan="3"><img src="images/bottom.jpg" width="480" height="91"></td>
    </tr>
</table>

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