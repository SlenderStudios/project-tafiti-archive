<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Directions.aspx.cs" Inherits="WLQuickApps.FieldManager.WebSite.Directions" MasterPageFile="~/AdminPages.master" %>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">

<script type="text/javascript">

var map = null;

function GetMap()
{
    map = new VEMap("map");
    map.onLoadMap = GetRoute;
    map.LoadMap(null, null, VEMapStyle.Hybrid);
}

function GetRoute()
{
    var startAddress = $get("<%= this._startAddress.ClientID %>").value;
    var endAddress = $get("<%= this._endAddress.ClientID %>").value;
    if ((startAddress != endAddress) && (startAddress != "") && (endAddress != ""))
    {
        $get("_directions").innerHTML = "<em>Getting directions...</em>";
        map.GetRoute(
            startAddress, 
            endAddress, 
            null, 
            null, 
            function (route)
            {
                if (route == null)
                {
                    $get("_directions").innerHTML = "<em>The route could not be determined.</em>";
                    return;
                }
                
                map.LoadTraffic(true);
                    
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
            );
    }
}

function GetDistanceString(segment, distanceUnit)
{
    if ((segment.Distance == null) || (segment.Distance == 0)) { return ""; }
    return segment.Distance + " " + distanceUnit;
}

function PanToLatLong(latitude, longitude)
{
    map.PanToLatLong(new VELatLong(latitude, longitude));
}

Sys.Application.add_init(GetMap);

</script>
<div class="pageContent">
    <div class="header">Directions</div>
    <div id="map" class="MapPanel"></div>
        
    <div class="rightPanel">
        <div class="text-label">Start Address</div>
        <asp:TextBox ID="_startAddress" CssClass="address-box" runat="server" TextMode="MultiLine" Columns="33" Rows="3" /><br />
        <div class="text-label">End Address</div>
        <asp:TextBox ID="_endAddress" CssClass="address-box" runat="server" TextMode="MultiLine" Columns="33" Rows="3" /><br />
        <asp:HyperLink ID="_itemTitle" runat="server" Text="Get Route"
            NavigateUrl="javascript:GetRoute()" CssClass="optionsLink" />
        <br /><br />
        <div class="DirectionsInstructionDiv">
            <div class="text-label">Turn-By-Turn</div>
            <div id="_directions" class="DirectionsInstruction">Please enter a route.</div>
        </div>
    </div>
</div>
</asp:Content>