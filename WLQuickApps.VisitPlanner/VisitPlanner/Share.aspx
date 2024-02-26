<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Share.aspx.cs" Inherits="VisitPlanner.Share" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:devlive="http://dev.live.com">
<head id="Head1" runat="server">
    <title>Windows Live Contacts</title>
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <META HTTP-EQUIV="Pragma" CONTENT="no-cache">
    <META HTTP-EQUIV="Expires" CONTENT="-1">
    <link rel="StyleSheet" href="style/VisitPlanner.css" type="text/css" /> 
    <script type="text/javascript" src="http://controls.services.live.com/scripts/base/v0.3/live.js"></script>
    <script type="text/javascript" src="http://controls.services.live.com/scripts/base/v0.3/controls.js"></script>
    
</head>
<body id="body" runat="server" >
    <form id="VisitPlannerForm" runat="server">
        <center>
            <div style="height:335px;width:100%;">
            <table width=100%>
                <tr>
                    <td style="text-align:left">
                        <asp:Image ID="VisitPlannerLogo" runat="server" ImageUrl="images/Main_Logo.jpg" />
                    </td>
                    <td style="vertical-align:top;">
                        <a class="closelink" href="javascript: window.close();">Close This Window</a>
                    </td>
                </tr>
            </table>
            <devlive:contactscontrol style="border:none;" 
                devlive:channelEndpointURL="Channel.htm"
                devlive:view="tilelist" 
                devlive:market="" 
                devlive:message = "Check out the places I'd like to Visit at <%= ShareLink %>"
                />
             </div>
        </center>          
    </form>
</body>
</html>