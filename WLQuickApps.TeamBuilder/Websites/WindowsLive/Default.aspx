<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="_Default" Title="Home" %>

<%@ Register Assembly="Controls" Namespace="Controls" TagPrefix="controls" %>
<%@ Register Src="Controls/NewsControl.ascx" TagName="NewsControl" TagPrefix="uc1" %>
<%@ Register src="Controls/LatestVideoControl.ascx" tagname="LatestVideoControl" tagprefix="uc2" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <link href="App_Themes/Default/Default.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server">
        <Services>
            <asp:ServiceReference Path="~/Services/CalendarService.asmx" />
        </Services>
        <Scripts>
            <asp:ScriptReference Path="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6" />
            <asp:ScriptReference Path="~/Scripts/EventList.js" />
            <asp:ScriptReference Path="~/Scripts/VirtualEarth.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <ul id="nav">
        <li><a href="Default.aspx"><strong>Home</strong></a></li>
        <li><a href="Photos.aspx">Photos</a></li>
        <li><a href="Video.aspx">Video</a></li>
    </ul>
    <div id="subheader" class="clearfix">
        <div id="welcome">
            <h1>
                Welcome to the home of the Toronto Junior Leafs!</h1>
            <p>
                This is our site where we can see all that&#39;s happening with our team, the 
                Toronto Junior Leafs! Thanks for coming. From here any member of our page can 
                add photos, upload video and start a conversation with other members online.</p>
        </div>
        <div id="featured-player">
            <h2>
                Featured Player</h2>
            <div id="container">
                <img src="Images/Crosby.jpg" alt="Sidney Crosby #87" width="60" height="60" />
                <h3>
                    Ryan Storgaad #87</h3>
                <p>
                    Storgaad finished his rookie season with the franchise record in assists (63) and 
                    points (102).</p>
            </div>
        </div>
    </div>
    <div id="content" class="clearfix">
        <div id="content-left">
            <h2>
                Events</h2>
            <div id="events" class="clearfix">
                <div id="event-calendar">
                    <controls:Calendar runat="server" ID="calendar" CssClass="event-calendar" Animated="false" />
                </div>
                <div id="event-list" class="clearfix">
                    <ul id="event-list-left">
                    </ul>
                    <ul id="event-list-right">
                    </ul>
                </div>
                <div id="event-links">
                    <p>
                        Click the link below if you would like to receive real-time alerts sent directly 
                        to your desktop, mobile device, or email.</p>
                    <div class="alerts">
                        <a href="#">
                            <img src="Images/alert_signup_eng.gif" alt="Windows Live Alerts" /></a></div>
                    <ul>
                        <li>
                            <img src="Images/new_cal.gif" alt="Windows Live Calendar" />
                            <asp:HyperLink ID="CalendarWeb" Target="_blank" runat="server">Windows Live Calendar</asp:HyperLink></li>
                        <li>
                            <img src="Images/icon_rss.gif" alt="RSS Feed" />
                            <asp:HyperLink ID="CalendarRSS" Target="_blank" runat="server">RSS</asp:HyperLink></li>
                    </ul>
                </div>
            </div>
            <h2>
                Event Locations</h2>
            <div id="event-map" style="border: solid 1px; position: relative; width: 648px; height: 348px;">
            </div>
        </div>
        <div id="content-right">
            <div id="highlights">
                <div id="game-highlights">
                    <h2>
                        Game Highlights</h2>
                    <iframe marginwidth="0" marginheight="0" src="Slide.Show/Default.html" frameborder="0"
                        width="300" height="260" scrolling="no"></iframe>
                </div>
                <div id="video-highlights">
                    <h2>
                        Video Highlights</h2>
                    <uc2:LatestVideoControl ID="LatestVideoControl" runat="server" />
                </div>
            </div>
            <div id="news">
                <h2>
                    Latest News</h2>
                <div id="news-list">
                    <uc1:NewsControl ID="NewsControl" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
    //<![CDATA[
    Sys.Application.add_init(function() {
        $create(Controls.EventList, {"servicePath":"Services/CalendarService.asmx","serviceMethod":"GetEvents"}, null, null, $get("event-list"));
    });
    Sys.Application.add_init(function() {
        $create(Controls.VirtualEarth, {}, null, null, $get("event-map"));
    });
    //]]>
    </script>

</asp:Content>
