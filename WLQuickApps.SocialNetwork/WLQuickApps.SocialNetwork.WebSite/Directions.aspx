<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Directions.aspx.cs" Inherits="Directions" Title="Get Directions" %>
<%@ Register Src="Controls/LocationControl.ascx" TagName="LocationControl" TagPrefix="uc1" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        var map = null;
        var route = null;
        
        function GetMap()
        {
            map = new VEMap("map");
            map.onLoadMap = GetRoute;
            map.LoadMap();
        }

        function GetRoute()
        {
            var startAddress = $get("<%= this._startAddress.ClientID %>").value;
            var endAddress = $get("<%= this._endAddress.ClientID %>").value;
            if ((startAddress != endAddress) && (startAddress != "") && (endAddress != ""))
            {
                $get("_directions").innerHTML = "<em>Getting directions...</em>";
                route = map.GetRoute(startAddress, endAddress, null, null, onGotRoute);
            }
        }

        function ShowDisambiguationDialog(v)
        {
            map.ShowDisambiguationDialog(v);
        }
        
        function GetDistanceString(segment, distanceUnit)
        {
            if ((segment.Distance == null) || (segment.Distance == 0)) { return ""; }
            return segment.Distance + " " + distanceUnit;
        }
        
        function onGotRoute(route)         
        {
            if (route == null)
            {
                $get("_directions").innerHTML = "<em>The route could not be determined.</em>";
                return;
            }
            
            var routeInfo = "<table><tr><th scope=\"col\">&nbsp;</th><th scope=\"col\">Instruction</th><th scope=\"col\">Distance</th></tr>\n";
            var len = route.Itinerary.Segments.length;
            if (len < 3) return;
            
            for(var i = 0; i < len; i++)
            {
                var stepNumber = i - 1;
                if ((stepNumber < 1) || (stepNumber == len - 2)) stepNumber = "";
                routeInfo += "<tr class=\"mod" + (i % 2) + "\"><th scope=\"row\" class=\"row\">" + stepNumber + "</th><td>" + 
                             "<a href='javascript:PanToLatLong(" + route.Itinerary.Segments[i].LatLong.Latitude + ", " + route.Itinerary.Segments[i].LatLong.Longitude + ")'>" + route.Itinerary.Segments[i].Instruction + 
                             "</a></td><td>" + GetDistanceString(route.Itinerary.Segments[i], route.Itinerary.DistanceUnit) + "</td></tr>\n";
            }
            routeInfo += "</table><strong>Total distance:</strong> " + route.Itinerary.Distance + " " + route.Itinerary.DistanceUnit;
            $get("_directions").innerHTML = routeInfo;
        }
        
        function PanToLatLong(latitude, longitude)
        {
            map.PanToLatLong(new VELatLong(latitude, longitude));
        }
        
        Sys.Application.add_init(GetMap);
    </script>
    
    <cc:DropShadowPanel ID="_mapPanel" runat="server" Width="450px" SkinID="ViewCollection-Map">
        <div id="map" style="position:relative; width:100%; height:400px;" ></div>
    </cc:DropShadowPanel>
    
    <cc:DropShadowPanel ID="_itemsPanel" runat="server" SkinID="ViewCollection-Items">
        <h3>Start Address</h3>
        <cc:SecureTextBox ID="_startAddress" runat="server" TextMode="MultiLine" Columns="20" Rows="3" /><br />
    </cc:DropShadowPanel>

    <cc:DropShadowPanel ID="DropShadowPanel1" runat="server" SkinID="ViewCollection-Items">
        <h3>End Address</h3>
        <cc:SecureTextBox ID="_endAddress" runat="server" TextMode="MultiLine" Columns="20" Rows="3" /><br />
    </cc:DropShadowPanel>
        
    <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ViewCollection-Items">
        <h3>Actions</h3>
        <asp:HyperLink ID="_itemTitle" runat="server" Text="Get Route"
            NavigateUrl="javascript:GetRoute()"
            SkinID="ViewCollection-ItemTitle" />
    </cc:DropShadowPanel>

    <cc:DropShadowPanel ID="DropShadowPanel3" runat="server">
        <h3>Turn-By-Turn</h3>
        <div id="_directions" class="styledTable" style="position:relative; width:100%">Please enter a route.</div>
    </cc:DropShadowPanel>
</asp:Content>