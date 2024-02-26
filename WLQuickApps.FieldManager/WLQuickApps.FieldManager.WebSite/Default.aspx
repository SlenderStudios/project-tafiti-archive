<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FieldManager.master" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>

<script language="C#" runat="server">

public string GetFields()
{
    if (!UserManager.UserIsLoggedIn()) { return string.Empty; }

    StringBuilder stringBuilder = new StringBuilder();
    foreach (WLQuickApps.FieldManager.Data.Field field in FieldsManager.GetFieldsForUser(UserManager.LoggedInUser.UserID, 0, FieldsManager.GetFieldsForUserCount(UserManager.LoggedInUser.UserID)))
    {
        if (stringBuilder.Length > 0)
        {
            stringBuilder.Append(", ");
        }

        stringBuilder.AppendFormat("{0}: true", field.FieldID);
    }

    return stringBuilder.ToString();
}

public string GetAddress()
{
    if (UserManager.UserIsLoggedIn() && !string.IsNullOrEmpty(UserManager.LoggedInUser.Address))
    {
        return AntiXss.JavaScriptEncode(UserManager.LoggedInUser.Address);
    }

    return "7901 168th Ave NE, Redmond, WA 98052";
}
    
</script>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">
    <script type="text/javascript">
    var map;
    var fieldID;
    var address;
    var myFields = {<%= this.GetFields() %>};
    var leagueFields = {};
    var startAddress = "<%= this.GetAddress() %>";
    var firstLoad = true;
    var endAddress;
    
    function onStart()
    {
        map = new VEMap("_map");
        map.onLoadMap = function() 
        {
            map.AttachEvent("onchangeview", resetFields);
            
            map.AttachEvent("onclick",
            function(e)
            {
                if((e != null) && (e.elementID != null))
                {
                    var shape = map.GetShapeByID(e.elementID);
                    if (shape.FieldID)
                    {
                        var slPlugin = $get("<%= this.Xaml1.ClientID %>");
                        slPlugin.content.FieldManagerSilverlight.ViewField(shape.FieldID);
                    }
                }
            });
            
            resetFields();
        };
        map.LoadMap(null, null, VEMapStyle.Hybrid);
        map.SetMouseWheelZoomToCenter(false);
    }
    
    function addToMyFields(fieldID)
    {
        WLQuickApps.FieldManager.WebSite.SiteService.AddToMyFields(
            fieldID, 
            function()
            {
                resetFields();
            });
    }
    
    function viewField(id, address, latitude, longitude, pan)
    {
        fieldID = id;

        if (pan || firstLoad)
        {
            firstLoad = false;
            if (map.GetZoomLevel() < 10)
                map.SetZoomLevel(10);
            PanToLatLong(latitude, longitude);
        }
    }
    
    function viewLeague(id)
    {
        WLQuickApps.FieldManager.WebSite.SiteService.GetFieldsForLeague(
            id,
            function (fields)
            {
                leagueFields = {};
                for (var lcv = 0; lcv < fields.length; lcv++)
                    leagueFields[fields[lcv].FieldID] = true;
                
                if (fields.length == 0) { return; }
                
                var nLatitude = -180;
                var sLatitude = 180;
                var eLongitude = -180;
                var wLongitude = 180;

                for (lcv = 0; lcv < fields.length; lcv++)
                {
                    if (fields[lcv].Latitude > nLatitude) { nLatitude = fields[lcv].Latitude; }
                    if (fields[lcv].Latitude < sLatitude) { sLatitude = fields[lcv].Latitude; }
                    if (fields[lcv].Longitude > eLongitude) { eLongitude = fields[lcv].Longitude; }
                    if (fields[lcv].Longitude < wLongitude) { wLongitude = fields[lcv].Longitude; }
                }
                
                map.SetMapView(new VELatLongRectangle(new VELatLong(nLatitude, wLongitude), new VELatLong(sLatitude, eLongitude)));
            }
        );
    }
        
    function resetFields()
    {
        var latLong;
        switch (map.GetMapStyle())
        {
        case VEMapStyle.Oblique:
        case VEMapStyle.Birdseye:
            latLong = map.GetBirdseyeScene().GetBoundingRectangle();
            break;        
        default:
            latLong = map.GetMapView();
            break;
        }
        
        WLQuickApps.FieldManager.WebSite.SiteService.GetFieldsInRange(
            latLong.TopLeftLatLong.Latitude,
            latLong.BottomRightLatLong.Latitude, 
            latLong.BottomRightLatLong.Longitude,
            latLong.TopLeftLatLong.Longitude,
            onFields);
    }
    
    function onFields(fields)
    {
        map.DeleteAllShapes();
        for (var lcv = 0; lcv < fields.length; lcv++)
        {
            var existingForUser = (typeof(myFields[fields[lcv].FieldID]) != "undefined");
            var existingForLeague = (typeof(leagueFields[fields[lcv].FieldID]) != "undefined");
        
            var shape = new VEShape(VEShapeType.Pushpin, new VELatLong(fields[lcv].Latitude, fields[lcv].Longitude));
            if (fields[lcv].FieldID == fieldID)
            {
                shape.SetCustomIcon("<img src='/Images/targetFieldPin.gif' />");
            }
            else if (fields[lcv].IsOpen)
            {
                if (existingForLeague)
                    shape.SetCustomIcon("<img src='/Images/inCollectionFieldPin.gif' />");
                else
                    shape.SetCustomIcon("<img src='/Images/notInCollectionFieldPin.gif' />");
            }
            else
            {
                shape.SetCustomIcon("<img src='/Images/closedFieldPin.gif' />");
            }
            
            shape.SetTitle(fields[lcv].Title);
            
            var description = getDescription(fields[lcv]);
            
            shape.SetDescription(description);
            map.AddShape(shape);
            
            shape.FieldID = fields[lcv].FieldID;
        }
    }
    
    function viewDirections(destination)
    {
        clearDirections();
        $get("_addressInput").innerText = startAddress;
        $get("_address").style.display = "block";
        endAddress = destination;
    }
    
    function confirmDirections()
    {
        var startAddressUser = $get("_addressInput").innerText;
        $get("_address").style.display = "none";
        if ((startAddressUser != endAddress) && (startAddressUser != "") && (endAddress != ""))
        {
            $get("_directions").innerHTML = "<em>Getting directions...</em>";
            $get("_directions").style.display = "block";
            map.GetRoute(
                startAddressUser, 
                endAddress, 
                null, 
                null, 
                function (route)
                {
                    $get("_directions").innerHTML = "";
                    if (route == null)
                    {
                        $get("_directions").innerHTML = "<em>The route could not be determined.</em>";
                        window.setTimeout('$get("_directions").style.display = "none"', 4000);
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
                    $get("_directions").style.display = "none";
                    $get("_directions").innerHTML = routeInfo;
                    $get("_print").style.display = "block";
                    $get("_printbg").style.display = "block";
                }
                );
        }
    }
    
    function clearDirections()
    {
        map.DeleteRoute();
        $get("_print").style.display = "none";
        $get("_printbg").style.display = "none";
        $get("_address").style.display = "none";
    }
    
    function viewTraffic(show)
    {
        if (show)   map.LoadTraffic(true);
        else        map.ClearTraffic();
    }
    
    function viewContacts(show)
    {
        $get("_contacts").style.display = ( show ? "block" : "none" );
        if (show)
        {
            var slPlugin = $get("<%= this.Xaml1.ClientID %>");
            WLQuickApps.FieldManager.WebSite.SiteService.GetUsersForField(fieldID, parseContacts);
        }
    }
    
    function parseContacts(users)
    {
        var userList = "";
        for (var lcv = 0; lcv < users.length; lcv++)
        {
            userList += '<li onclick="javascript:window.open(\'http://settings.messenger.live.com/Conversation/IMMe.aspx?invitee=' 
                     +  users[lcv].MessengerPresenceID + '\', \'\', \'height=500,width=300\')">'
                     +  '<img src="http://messenger.services.live.com/users/' + users[lcv].MessengerPresenceID + '/presenceimage/"/>'
                     +  users[lcv].DisplayName + '</li>';
        }
        $get("_contactsList").innerHTML = userList;
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
    
    function PrintDirections()
    {
        var childWindow = window.open("DirectionsPrint.aspx", "Directions");
    }
    
    function GetRouteTable()
    {
        return $get("_directions").innerHTML;
    }

    </script>

    <div id="_silverlight">
        <asp:Silverlight 
            ID="Xaml1"
            runat="server" 
            Source="~/ClientBin/WLQuickApps.FieldManager.Silverlight.xap" 
            MinumVersion="2.0.31005.0"
            Width="980px" 
            Height="580px"
            Windowless="true"
            PluginBackground="Transparent"
            />
        <div id="_map" class="SilverlightMap" style="z-index:500;"></div>
        <div id="_contacts" class="SilverlightContacts">
            <h3>People who have joined this field</h3>
            <hr />
            <ul id="_contactsList"></ul>
        </div>
        <div id="_address" class="SilverlightAddress">
            <h4>Where are you coming from?</h4>
            <textarea id="_addressInput" class="SilverlightAddressInput" rows="2" cols="30">Insert your address here</textarea>
            <input type="button" id="_addressConfirm" class="SilverlightAddressConfirm"onclick="confirmDirections()" value="Get Directions" />
        </div>
        <div id="_directions" class="SilverlightDirections"></div>
        <div id="_printbg" class="SilverlightPrintBg"></div>
        <a id="_print" class="SilverlightPrint" href="javascript:PrintDirections()">Print Directions</a>
    </div>
    
</asp:Content>