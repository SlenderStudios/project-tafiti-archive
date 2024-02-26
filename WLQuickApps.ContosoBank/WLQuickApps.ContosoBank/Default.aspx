<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True" CodeBehind="Default.aspx.cs" Inherits="WLQuickApps.ContosoBank.Default" Title="Australian Small Business Portal - Home" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Assembly="Microsoft.Live.ServerControls" Namespace="Microsoft.Live.ServerControls"
    TagPrefix="live" %>
<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<%@ Register src="controls/LatestPostsControl.ascx" tagname="LatestPostsControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="imagetoolbar" content="no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    <Scripts>
            <asp:ScriptReference Path="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1" />
            <asp:ScriptReference Path="~/js/WLQuickApps.ContosoBank.Map.js" />
            <asp:ScriptReference Path="~/js/WLQuickApps.ContosoBank.DashBoard.js" />
            <asp:ScriptReference Path="~/js/Default.aspx.js" />
            <asp:ScriptReference Path="~/js/Utility.js" />
            <asp:ScriptReference Path="~/js/GlobalCallQueue.js" />
    </Scripts>
    <Services>
        <asp:ServiceReference Path="~/Services/ContosoBankService.asmx" />
    </Services>
</asp:ScriptManagerProxy>

<script type="text/javascript">
var cUserLatitude = <%= UserLatitude %>;
var cUserLongitude = <%= UserLongitude %>;
var cUserZoomLevel = <%= UserZoomLevel %>;
</script>

<h1>WELCOME to the Australian Small Business Community Portal</h1>
<p>Within these pages you will find a &#39;wealth&#39; of information to help you combat the 
    financial pressures of an increasingly complicated world.</p>
    <ul>
        <li>Join for free, just use your Windows Live ID</li>
        <li>Solid business advice from thousands of your peers delivered via forums, feeds and videos</li>
        <li>Experience live chat with experienced wealth, loan and insurance advisors</li>
    </ul>
                        
    <div class="ForumSummary">
        <uc1:LatestPostsControl ID="LatestPostsControl1" runat="server" />
    </div>
    <div class="HomeAd">
        <a href="http://www.codeplex.com/WLQuickApps/Release/ProjectReleases.aspx" target="_blank">
            <img src="Images/Ad.jpg" alt="Get your source code here" /></a>
    </div>
    <div class="HomeMapFrame">
        <div id="mymap" class="mymap" style="position:relative;width:680px;height:535px;overflow:hidden;background:white;"></div>
        <div id="mymapdashboard" class="mymapdashboard">
            <div class="mapdashboardHandle" onclick="MapActionToggleDashboard_onclick();">&lt;&lt;</div>
            <div id="MapActionShaded" class="MapAction">Road</div>
            <div id="MapActionHybrid" class="MapAction">Aerial</div>
            <div id="MapActionBirdseyeHybrid" class="MapAction">Birdseye</div>
            <hr />
            <div id="MapActionMode2D" class="MapAction">2D</div>
            <div id="MapActionMode3D" class="MapAction">3D</div>
            <div class="mapdashboardSubTitle">Filter:</div>
            <div id="MapActionFilterOn" class="MapAction MapActionEnabled" onclick="MapActionFilterOn_onclick();">Member</div>
            <div id="MapActionFilterOff" class="MapAction" onclick="MapActionFilterOff_onclick();">Branch</div>
            <hr />
            <div class="MapZoomBar">
                Zoom
                <div id="MapActionZoomIn" class="ZoomIn"></div>
                <asp:TextBox ID="ZoomSlider" runat="server" style="display:none;"></asp:TextBox>
                <cc1:SliderExtender ID="SliderExtender1" runat="server" BehaviorID="MapZoomSlider" TargetControlID="ZoomSlider" HandleCssClass="ZoomBarMarker" RailCssClass="ZoomBar" RaiseChangeOnlyOnMouseUp="true" TooltipText="Zoom Level {0}" Minimum="1" Maximum="17" Steps="18" Decimals="0" Orientation="Vertical"  HandleImageUrl="~/images/ZoomBarMarker.gif"></cc1:SliderExtender>
                <div id="MapActionZoomOut" class="ZoomOut"></div>
            </div>                      
        </div>
        <div id="mymapminimaptoggle" class="mymapminimaptoggle" onclick="MapActionToggleMiniMap_onclick();"></div>
    </div>          

</asp:Content>
