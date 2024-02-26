<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WhereAreTheyNow.aspx.cs" Inherits="ContosoUniversity.WhereAreTheyNow" Theme="Default" EnableViewState="false" %>
<%@ Register Src="~/UserControls/ContactsControl.ascx" TagName="ContactsControl" TagPrefix="ContosoUniversity" %>

<%@ Register assembly="System.Web.Extensions" namespace="System.Web.UI" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Where are they now?</title>
</head>
<body class="WhereBody" onload="OnPageLoad();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" EnablePartialRendering="true">
            <Scripts>
                <asp:ScriptReference Path="js/Cookies.js" />
                <asp:ScriptReference Path="js/Map.js" />
                <asp:ScriptReference Path="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6" />
            </Scripts>
            <Services>
                <asp:ServiceReference Path="LiveContactsService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div id="WhereContacts">
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                <ContentTemplate>
                    <ContosoUniversity:ContactsControl ID="ContactsControl1" runat="server" PageSize="13" ShowMapLinks="true" />
               </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id='myMap' style="position:absolute; width:612px; height:552px; left:268px; top: 74px;"></div>
        <asp:DropDownList ID="Locations" CssClass="CampusLocations" runat="server" onchange="LocatePoi(this.value)" ToolTip="Select a location to view on the map" />
        <div id="ShowOverlay">
            Show Campus Overlayin">
            <a href="javascript: AddCenterPushpin();" title="Click here to plot your location at the center of the map view. Hover over the pin to add a comment and save your location. Alternatively, right-click the mouse at any location.">Plot my location</a>
        </div>
    </form>
</body>
</html>