<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="WLQuickApps.FieldManager.WebSite.Field_Default" MasterPageFile="~/AdminPages.master" %>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">

    <script type="text/javascript">
    
    var myFields = {<%= this.MyFields %>};
    
    var map;
    
    function onStart()
    {
        map = new VEMap("_map");
        map.onLoadMap = function() 
        {
            map.AttachEvent("onchangeview", resetFields);
            resetFields();            
        };

        map.LoadMap(null, null, VEMapStyle.Hybrid);
    }
    
    
    function searchForFields()
    {
        var address = document.getElementById("_addressTextBox");
        try
        {
            map.Find(null, address.value);
        }
        catch(e)
        {
            alert(e.message);
        }
    }

    function resetFields()
    {
        var latLong = map.GetMapView();
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
            var shape = new VEShape(VEShapeType.Pushpin, new VELatLong(fields[lcv].Latitude, fields[lcv].Longitude));
            shape.SetTitle(fields[lcv].Title);
            
            var description = getDescription(fields[lcv]);
                        
            shape.SetDescription(description);
            map.AddShape(shape);
        }
    }
    
    function addToMyFields(fieldID)
    {
        WLQuickApps.FieldManager.WebSite.SiteService.AddToMyFields(fieldID);
        document.location = "ViewField.aspx?fieldID=" + fieldID;
    }
    
    </script>
        
        <div class="pageContent">
            <div class="header">Search Fields</div>
            <div id="_map" class="MapPanel"></div>    
            
            <div  class="rightPanel">
                <div class="text-label">Near Address:</div>
                <textarea class="address-box" id="_addressTextBox" rows="3" cols="32"></textarea>
                <a class="optionsLink" href="javascript://Search for fields" onclick="searchForFields()">Search</a>
                <div id="_resultDiv"></div>
            </div>
        </div> 
</asp:Content>