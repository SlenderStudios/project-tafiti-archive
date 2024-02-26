<%@ Page Language="C#" AutoEventWireup="true" CodeFile="user_data.aspx.cs" Inherits="Default2" %>

<html>
<head>
<title>Data page</title>
</head>
</html>
	
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
<title>Data page</title>
<script type="text/javascript"><!--
var someData = 
[
	[ // User Data
		<%=user_xml%>
	],
	[ // Video Data
	<%=video_xml%>
	]
];
</script>
</head>
<body onload="if(window.parent&&parent.dataOnLoaded){parent.dataOnLoaded(someData);}"/>
</html>