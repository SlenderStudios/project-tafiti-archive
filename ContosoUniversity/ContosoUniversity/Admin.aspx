<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Admin.aspx.cs" Inherits="Admin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Contoso University Administration</title>
    <style type="text/css">
        body {background-color:#E8E8E6; font-family: Tahoma, Arial; color:#4F271B}
        li {list-style: disc url(App_Themes/Default/images/arrow_off.gif) outside; margin-top:5px;}
        a {color: #4F271B}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <img src="App_Themes/Default/images/logo.jpg" />
        <div style="padding-left:30px;">
            <br />
            <h2>Admnistration Links</h2>
            <ul>
                <li>
            <a href="http://contosouniversity.spaces.live.com/?_c11_BlogPart_BlogPart=blogmgmt&_c=BlogPart&_c02_owner=1">Manage blog entries</a></li>
                <li>
            <a href="http://contosouniversity.spaces.live.com/Lists/cns!ADA4B9BF1C07AC5!154/?_c02_owner=1">Manage campus locations list</a></li>
                <li>
            <a href="expoCategories.aspx">Show all Expo Classified Categories</a></li>
            </ul>
        </div>
    </form>
</body>
</html>
