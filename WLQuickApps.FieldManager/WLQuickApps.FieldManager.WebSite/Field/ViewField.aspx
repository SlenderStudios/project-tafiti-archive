<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="ViewField.aspx.cs" Inherits="WLQuickApps.FieldManager.WebSite.Field_ViewField" MasterPageFile="~/AdminPages.master" %>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">

    <script type="text/javascript">
    
    var latitude = <%= this.Latitude %>;
    var longitude = <%= this.Longitude %>;
    var address = "<%= this.Address %>";
    var myFields = {<%= this.MyFields %>};
    var fieldID = <%= this.FieldID %>;
    
    var map;
    
    function onStart()
    {
        map = new VEMap("_map");
        map.onLoadMap = function() 
        {
            map.AttachEvent("onchangeview", resetFields);
            WLQuickApps.FieldManager.WebSite.SiteService.GetWeather(
                latitude, 
                longitude,
                function (weathers)
                {
                    $get("_weather").appendChild(document.createTextNode(weathers[0].WeatherSummary + " (High: " + weathers[0].MaxTemperature + ", Low: " + weathers[0].MinTemperature + ")"));
                }
                );
        
            resetFields();
        };

        map.LoadMap(new VELatLong(latitude, longitude), 18, VEMapStyle.Hybrid);
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
    
    
    </script>
<div class="pageContent">
    <div class="header">View Field</div>
    <div id="_map" class="MapPanel"></div>
    
    <div  class="rightPanel">
        <div class="FieldWeather"><strong>Weather: </strong><span id="_weather"></span></div>
        
        <asp:Panel ID="_adminPanel" runat="server">
            <div class="subHeader">Field Options</div>
            
            <script type="text/javascript">
            function onUpdate()
            {
                var address = $get("<%= this._addressTextBox.ClientID %>").value;
                try
                {
                    map.Find(null, address, null, null, null, null, null, null, true, null, onLocations);
                }
                catch(e)
                {
                    alert(e.message);
                }
            }
            
            function onLocations(a, b, c, d, e)
            {
                if (c == null)
                
                {
                    alert("c is null");
                    debugger;
                    return;
                }
                
                if (c.length > 1)
                {
                    var results = "More than one location was returned. Please select the location you were looking for:<br />";
                    for (var lcv = 0; lcv < c.length; lcv++)
                    {
                        var linkUrl = 'javascript:updateField(' + c[lcv].LatLong.Latitude + ', ' + c[lcv].LatLong.Longitude + ', "' + c[lcv].Name.replace(/"/g, "\"") + '")';
                        results += "<a href='" + linkUrl + "'>" + c[lcv].Name + "</a><br />";
                    }
                    document.getElementById("_resultDiv").innerHTML = results;
                }
                else if (c.length == 0)
                {
                    alert("No results");
                }
                else
                {
                    updateField(c[0].LatLong.Latitude, c[0].LatLong.Longitude, c[0].Name);
                }
            }

            function updateField(latitude, longitude, address)
            {
                document.getElementById("_resultDiv").innerHTML = "";
                
                var title = $get("<%= this._titleTextBox.ClientID %>").value;
                var description = $get("<%= this._descriptionTextBox.ClientID %>").value;
                var isOpen = $get("<%= this._isOpenCheckBox.ClientID %>").checked;
                var numberOfFields = Number.parseInvariant($get("<%= this._numberOfFieldsTextBox.ClientID %>").value);
                var parkingLot = $get("<%= this._parkingLotTextBox.ClientID %>").value;
                var phoneNumber = $get("<%= this._phoneNumberTextBox.ClientID %>").value;
                var status = $get("<%= this._statusTextBox.ClientID %>").value;
            
                WLQuickApps.FieldManager.WebSite.SiteService.UpdateField(
                    fieldID,
                    title,
                    description,
                    address,
                    latitude,
                    longitude,
                    numberOfFields,
                    parkingLot,
                    phoneNumber,
                    isOpen,
                    status,
                    function() {},
                    function()
                    {
                        alert("The field could not be updated");                
                    }
                    );
            }

            </script>
                
            <table width="100%">
                <tr><td class="tableLabel">Title</td><td><asp:TextBox ID="_titleTextBox" runat="server" MaxLength="32"></asp:TextBox></td></tr>
                <tr><td class="tableLabel">Description</td><td><asp:TextBox ID="_descriptionTextBox" runat="server" MaxLength="64"></asp:TextBox></td></tr>
                <tr><td class="tableLabel">Number Of Fields</td><td><asp:TextBox ID="_numberOfFieldsTextBox" runat="server" MaxLength="3"></asp:TextBox></td></tr>
                <tr><td class="tableLabel">Parking Lot</td><td><asp:TextBox ID="_parkingLotTextBox" runat="server" MaxLength="64"></asp:TextBox></td></tr>
                <tr><td class="tableLabel">Phone Number</td><td><asp:TextBox ID="_phoneNumberTextBox" runat="server" MaxLength="16"></asp:TextBox></td></tr>
                <tr><td class="tableLabel">Is Open?</td><td><asp:CheckBox ID="_isOpenCheckBox" runat="server"></asp:CheckBox></td></tr>
                <tr><td class="tableLabel">Notes</td><td><asp:TextBox ID="_statusTextBox" runat="server" MaxLength="32"></asp:TextBox></td></tr>
                <tr><td class="tableLabel">Address</td><td><asp:TextBox ID="_addressTextBox" runat="server" MaxLength="256"></asp:TextBox></td></tr>
            </table>
            <asp:HyperLink ID="_updateLink" runat="server" CssClass="optionsLink" NavigateUrl="javascript:onUpdate()">Update Field</asp:HyperLink>
            <asp:LinkButton ID="_deleteLink" runat="server" CssClass="optionsLink" OnClick="_deleteClick">Delete Field</asp:LinkButton>
            <br /><br />
            <div class="subHeader">Add Admin To Field</div>
            <table width="100%">
                <tr>
                    <td class="tableLabel">By Email</td>
                    <td><asp:TextBox ID="_addAdminByEmailTextBox" runat="server" MaxLength="256"></asp:TextBox></td>
                    <td><asp:LinkButton ID="_addAdminByEmailButton" CssClass="optionsLink" runat="server" OnClick="_addAdminByEmailButtonClick" Text="Add" /></td>
                </tr>
        </table>       
        <div id="_resultDiv"></div>
        </asp:Panel>
    </div>
</div>    

</asp:Content>