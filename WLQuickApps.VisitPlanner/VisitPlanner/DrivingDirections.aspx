<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrivingDirections.aspx.cs" Inherits="VisitPlanner.DrivingDirections" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:devlive="http://dev.live.com">
<head id="Head1" runat="server">
    <title>Visit Planner Directions</title>
    
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    
<META HTTP-EQUIV="Pragma" CONTENT="no-cache">
<META HTTP-EQUIV="Expires" CONTENT="-1">

    <link rel="StyleSheet" href="style/VisitPlanner.css" type="text/css" /> 
    
    <script type="text/javascript" src="script/Silverlight.js"></script>
    <script type="text/javascript" src="script/VisitPlanner.js"></script>
    <script type="text/javascript" src="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6"></script>
      <script type="text/javascript">
         var map = null;
         
         function GetRouteMap(LocationFrom,LocationTo)
         {
            map = new VEMap('myMap');
            map.LoadMap();
            map.SetDashboardSize(VEDashboardSize.Small);
		
			var options = new VERouteOptions();
		    
		    options.RouteCallback = onGotRoute;
			map.GetDirections([LocationFrom, LocationTo], options);
		 }

         function onGotRoute(route)
         {
           // Unroll route
           var legs     = route.RouteLegs;
           var turns    = "Total distance: " + route.Distance.toFixed(1) + " mi\n";
           var numTurns = 0;
           var leg      = null;
		   var objDiv = document.getElementById("divText")
		   
		   objDiv.innerHTML = '<span class="PrintHeader2">' + "Directions" + '</span>' + '<br>';
		   
           // Get intermediate legs
            for(var i = 0; i < legs.length; i++)
            {
               // Get this leg so we don't have to derefernce multiple times
               leg = legs[i];  // Leg is a VERouteLeg object
                  
               // Unroll each intermediate leg
               var turn = null;  // The itinerary leg
                  
               for(var j = 0; j < leg.Itinerary.Items.length; j ++)
               {
                  turn = leg.Itinerary.Items[j];
                  numTurns++;
                  objDiv.innerHTML += '<b>' + numTurns + '</b>' + ".\t" + XssEncode(turn.Text) + " (" + turn.Distance.toFixed(1) + " mi)\n" + '<br>';
               }
            }

        }
      </script>
 </head>
<body id="body" runat="server" >
    
    <form id="VisitPlannerForm" runat="server">

    <table width=100%>
        <tr>
            <td>
                <table width=100%>
                    <tr>
                        <td>
                            <asp:Image ID="VisitPlannerLogo" runat="server" ImageUrl="images/Main_Logo.jpg" />
                        </td>
                        <td>
                            <span class="PrintHeader1" style="vertical-align:text-top;">Driving Directions</span>
                        </td>
                        <td style="vertical-align:top;">
                            <a class="closelink" href="javascript: window.close();">Close This Window</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                
                <table>
                    <tr>
                        <td colspan="2" style="vertical-align:top;">
                            <div id='myMap' style="position:relative; width:500px; height:400px;border:solid 2px black;"></div>
                               
                        </td>
                    </tr>
                    <tr>
                        
                        <td style="width:50%;vertical-align:top;"><span class="PrintHeader2">START</span><br />
                            <asp:Label ID="AddressFrom" runat="server" Text="Label" Font-Italic="true"></asp:Label></td>
                        <td style="width:50%;vertical-align:top;"><span class="PrintHeader2">END</span><br />
                            <asp:Label ID="AddressTo" runat="server" Text="Label" Font-Italic="true"></asp:Label>
                          
                         <br />  <br />  
                        </td>
                    </tr>
                    
                    <tr>
                   
                        <td colspan="2"><div id="divText"></div></td>
                    </tr>
                </table>          
            </td>
        </tr>
    </table>
           
            
    </form>
    
</body>
</html>