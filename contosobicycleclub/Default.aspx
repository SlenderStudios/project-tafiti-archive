<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="RidesMenuControl.ascx" TagName="RidesMenuControl" TagPrefix="Live" %>
<%@ Register Src="EventsMenuControl.ascx" TagName="EventsMenuControl" TagPrefix="Live" %>
<%@ Register Src="LatestRidesControl.ascx" TagName="LatestRidesControl" TagPrefix="Live" %>
<%@ Register Src="EventsControl.ascx" TagName="EventsControl" TagPrefix="Live" %>
<%@ OutputCache Duration="300" VaryByParam="None" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Contoso Bicycle Club</title>
    <link href="App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />
    
    <!-- Tracking Code 
    <script language="javascript" type="text/javascript" src="http://analytics.live.com/Analytics/msAnalytics.js"></script>
    <script language="javascript" type="text/javascript">
        msAnalytics.ProfileId = 'C43C';
        msAnalytics.TrackPage();
    </script>
    -->

</head>
<body  onload="onBodyLoad();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Path="js/ajah.js" />
                <asp:ScriptReference Path="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6" />
                <asp:ScriptReference Path="js/default.aspx.js" />
                <asp:ScriptReference Path="http://agappdom.net/h/silverlight.js" />
		        <asp:ScriptReference Path="js/StartPlayer.js"/>
            </Scripts>
        </asp:ScriptManager>
        
        <div id="MasterContainer">
            
            <div id="latestRides">
                <span class="Title">RIDES</span>
                <Live:LatestRidesControl ID="LatestRidesControl1" runat="server" />
            </div>
            
            <div id="ridesAndEvents">
                <span class="Title">EVENTS</span>
                <Live:EventsControl ID="EventsControl1" runat="server" />
            </div>
            
            
            <div id="eventsMenuDiv">
                <Live:EventsMenuControl ID="EventsMenuControl1" runat="server" />
            </div>
            
            <div id="ridesMenuDiv">
                <Live:RidesMenuControl ID="RidesMenuControl1" runat="server" />
            </div>
            
            <div id="MenuPanel" >
                <span id="RideReportTitle">RIDE REPORT</span>
                <span id="RideTheRouteTitle">RIDE THE ROUTE</span>
                <div id="Title"><% Response.Write( Spaces.LatestTitle(ConfigurationManager.AppSettings["LatestRidesFeed"])); %></div>
                <a id="BikeCamLink" href="javascript:viewBikeCam();" title="Start the Bike Cam and synchronise the map.">Bike CAM</a>
                <a id="PhotosFromRideLink" href="javascript:viewPhotos();" title="Display photos of the ride.">Photos from Ride</a>
                <a id="RideReportLink" href="javascript:viewRideReport();" title="Show the ride report.">Ride Report</a>
                <a id="ViewRouteLink" href="JavaScript:viewRoute()" title="Toggle the display of the route on the map below.">View Route</a>
                <a id="MapLink" href="JavaScript:viewMap()" title="Open the map with details.">Map</a>
                <a id="DirectionsLink" href="JavaScript:viewDirections()" title="Show the directions and total distance.">Directions</a>
            </div>
           
            <!-- Top Menu Bar -->
            <div id="topMenuBar">
                <a class="TabLinkLeft" id="HomeLink" href="javascript:showPage('home');">Home</a>
                <a class="TabLink" id="RidesLink" href="javascript:showPage('ridereports');">Rides</a>
                <a class="TabLink" id="EventsLink" href="javascript:showPage('events');">Events</a>
                <a class="TabLink" id="ForumLink" href="javascript:showPage('forum');">Forum</a>
                <%--<a class="TabLinkRight" id="ClassifiedsLink" href="javascript:showPage('classifieds');">Bikes &amp; Kit</a>--%>
            </div>
            
            <div id="contentPanel" style="visibility: hidden;">
                <div id="mapContainerPanel">
                    <div id='MapPanel' style="position:relative; width:691px; height:345px;z-index:1;"></div>
                </div>
            </div>
                 
            <iframe id="SlideShowPanel" scrolling="no" frameborder="0"></iframe>

            <!-- The silverlight wrapper DIV -->
            <div id="VideoWrapper" class="silverlightHost"></div>
            
            <div id="DirectionsPanel"></div>
            <div id="TextPanel"></div>
            <div id="MainPanel"></div>
  
            <div id="footer">This is a <a href="http://dev.live.com/QuickApps/" target=_blank>demonstration site</a>. <br />The source code can be downloaded from the <A href="http://www.codeplex.com/WLQuickApps/Release/ProjectReleases.aspx" target=_blank>Windows Live Platform Quick Apps</A> CodePlex Project.</div>
        </div>

    </form>
</body>
</html>
