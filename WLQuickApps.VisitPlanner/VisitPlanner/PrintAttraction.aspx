<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintAttraction.aspx.cs" Inherits="VisitPlanner.PrintAttraction" Title="Untitled Page" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:devlive="http://dev.live.com">
<head id="Head1" runat="server">
    <title>Visit Planner</title>
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="-1" />

    <link rel="StyleSheet" href="style/VisitPlanner.css" type="text/css" /> 
    
    <script type="text/javascript" src="script/Silverlight.js"></script>
    <script type="text/javascript" src="script/VisitPlanner.js"></script>
    <script type="text/javascript" src="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6"></script>

 </head>
<body>

    <form id="VisitPlannerForm" runat="server">
    <table style="width:800px;">
    <tr>
    <td>
    <table>
        <tr>
            <td><asp:Image ID="VisitPlannerLogo" runat="server" ImageUrl="images/Main_Logo.jpg" /></td>
            <td><asp:Label ID="TitleLabel" runat="server" Text="" CssClass="PrintHeader1"></asp:Label></td>
        </tr>
    
    
    </table>
    <table>
        <tr>
            <td style="vertical-align:top;"><span class="PrintHeader2">Address</span><br />
                <i><asp:Label ID="Address1" runat="server" Text="Label"></asp:Label><br />
                <asp:Label ID="Address2" runat="server" Text="Label"></asp:Label></i>
                <br /><br />
                <span class="PrintHeader2">Details</span><br />
                <asp:Label ID="DescriptionLabel" runat="server" Text="Label"></asp:Label>
              
              
            </td>
        </tr>
        <tr>
       
            <td colspan="2"><asp:Image ID="AttractionImage" runat="server" /> </td>
        </tr>
    </table>
    
       
        </td></tr>
    </table>
    
    <div class="spacer"></div>

 
              
            
    </form>
    
</body>
</html>

