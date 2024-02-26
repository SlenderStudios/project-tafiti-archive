<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="CreateField.aspx.cs" Inherits="WLQuickApps.FieldManager.WebSite.User_CreateField" MasterPageFile="~/AdminPages.master" %>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">

<script type="text/javascript">
    var map = null;
    
    function onStart()
    {
        map = new VEMap("_map");
        map.LoadMap(null, null, VEMapStyle.Hybrid);
    }
    
    function onCreate()
    {
        var address = document.getElementById("_addressTextBox");
        try
        {
            map.Find(null, address.value, null, null, null, null, null, null, true, null, onLocations);
        }
        catch(e)
        {
            alert(e.message);
        }
    }
    
    function createField(latitude, longitude, address)
    {
        var titleBox = document.getElementById("_titleTextBox");
        if (titleBox.value.length == 0)
        {
            alert("You must provide a title");
            return;
        }
            
        var numberOfFields;
        try
        {
            numberOfFields = Number.parseInvariant(document.getElementById("_numberOfFieldsTextBox").value);
        }
        catch(e)
        {
            alert("You must provide a valid number of fields");
            document.getElementById("_numberOfFieldsTextBox").focus();
            return;
        }        
    
        WLQuickApps.FieldManager.WebSite.SiteService.CreateField(
            titleBox.value,
            document.getElementById("_descriptionTextBox").value,
            address,
            latitude,
            longitude,
            numberOfFields,
            document.getElementById("_parkingLotTextBox").value,
            document.getElementById("_phoneNumberTextBox").value,
            document.getElementById("_statusTextBox").value,
            onField);
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
                var linkUrl = 'javascript:createField(' + c[lcv].LatLong.Latitude + ', ' + c[lcv].LatLong.Longitude + ', "' + c[lcv].Name.replace(/"/g, "\"") + '")';
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
            createField(c[0].LatLong.Latitude, c[0].LatLong.Longitude, c[0].Name);
        }
    }
    
    function onField(field)
    {
        document.location = "/Field/ViewField.aspx?fieldID=" + field.FieldID;
    }

</script>
<div class="pageContent">
    <div class="header">Create Field</div>
    <div id="CreateFieldLeftPanel">
        <table>
            <tr>
                <td class="tableLabel-light">
                    Title:
                </td>
                <td>
                    <input type="text" id="_titleTextBox" />
                </td>
            </tr>
            <tr>
                <td class="tableLabel-light">
                    Description:
                </td>
                <td>
                    <input type="text" id="_descriptionTextBox" />
                </td>
            </tr>
            <tr>
                <td class="tableLabel-light">
                    Phone Number:
                </td>
                <td>
                    <input type="text" id="_phoneNumberTextBox" />
                </td>
            </tr>
            <tr>
                <td class="tableLabel-light">
                    Number Of Fields:
                </td>
                <td>
                    <input type="text" id="_numberOfFieldsTextBox" value="1" />
                </td>
            </tr>
            <tr>
                <td class="tableLabel-light">
                    Parking Lot:
                </td>
                <td>
                    <input type="text" id="_parkingLotTextBox" />
                </td>
            </tr>
            <tr>
                <td class="tableLabel-light">
                    Notes:
                </td>
                <td>
                    <input type="text" id="_statusTextBox" />
                </td>
            </tr>
            <tr>
                <td class="tableLabel-light" style="vertical-align:top">
                    Address:
                </td>
                <td>
                    <textarea id="_addressTextBox" rows="2" cols="40"></textarea><br /><br />

                    
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>

                </td>
            </tr>
        </table>
                            <a href="javascript://Create a field" class="optionsLink" onclick="onCreate()">Create</a>
    </div>
    <div id="CreateFieldRightPanel">
            <div id="_map" style="position:relative;width:100%;height:350px"></div>
            <div id="_resultDiv"></div>    
    </div>
</div>
</asp:Content>