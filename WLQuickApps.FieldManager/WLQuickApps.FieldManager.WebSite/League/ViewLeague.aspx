<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="ViewLeague.aspx.cs" Inherits="WLQuickApps.FieldManager.WebSite.League_ViewLeague" MasterPageFile="~/AdminPages.master" %>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">
    <script type="text/javascript">
    
    var myFields = {<%= this.MyFields %>};
    var leagueFields = {<%= this.LeagueFields %>};
    var leagueID = <%= Convert.ToInt32(this.Request.QueryString["leagueID"]) %>;
    var userIsLeagueAdmin = <%= this.IsLeagueAdmin %>;
    var map;
    var initialized = false;
    
    function onStart()
    {   
        map = new VEMap("_map");
        map.onLoadMap = function() 
        {
            map.AttachEvent("onchangeview", resetFields);
            
            WLQuickApps.FieldManager.WebSite.SiteService.GetFieldsForLeague(
                leagueID,
                function (fields)
                {
                    if (fields.length == 0) { return; }
                    
                    var nLatitude = -180;
                    var sLatitude = 180;
                    var eLongitude = -180;
                    var wLongitude = 180;

                    for (var lcv = 0; lcv < fields.length; lcv++)
                    {
                        if (fields[lcv].Latitude > nLatitude) { nLatitude = fields[lcv].Latitude; }
                        if (fields[lcv].Latitude < sLatitude) { sLatitude = fields[lcv].Latitude; }
                        if (fields[lcv].Longitude > eLongitude) { eLongitude = fields[lcv].Longitude; }
                        if (fields[lcv].Longitude < wLongitude) { wLongitude = fields[lcv].Longitude; }
                    }

                    if (!initialized)
                    {
                        initialized = true;
                        map.SetMapView(new VELatLongRectangle(new VELatLong(nLatitude, wLongitude), new VELatLong(sLatitude, eLongitude)));
                    }
                }
                );

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
        var view = map.GetMapView();
        var nLatitude = view.TopLeftLatLong.Latitude;
        var sLatitude = view.BottomRightLatLong.Latitude;
        var eLongitude = view.BottomRightLatLong.Longitude;
        var wLongitude = view.TopLeftLatLong.Longitude;

        WLQuickApps.FieldManager.WebSite.SiteService.GetFieldsInRange(
            nLatitude,
            sLatitude,
            eLongitude,
            wLongitude,
            function onFields(fields)
            {
                map.DeleteAllShapes();
                for (var lcv = 0; lcv < fields.length; lcv++)
                {
                    var existingForUser = (typeof(myFields[fields[lcv].FieldID]) != "undefined");
                    var existingForLeague = (typeof(leagueFields[fields[lcv].FieldID]) != "undefined");
                
                    var shape = new VEShape(VEShapeType.Pushpin, new VELatLong(fields[lcv].Latitude, fields[lcv].Longitude));
                    shape.SetTitle(fields[lcv].Title);

                    if (fields[lcv].IsOpen)
                    {
                        if (existingForLeague)
                        {
                            shape.SetCustomIcon("<img src='/Images/inCollectionFieldPin.png' />");
                        }
                        else
                        {
                            shape.SetCustomIcon("<img src='/Images/notInCollectionFieldPin.png' />");
                        }
                    }
                    else
                    {
                        shape.SetCustomIcon("<img src='/Images/closedFieldPin.png' />");
                    }
                    
                    var description = getDescription(fields[lcv]);
                                        
                    if (userIsLeagueAdmin && !existingForLeague)
                    {
                        description += "<br /><a href='javascript://Add to this league' onclick='addToThisLeague(" + fields[lcv].FieldID + ")'>Add To This League</a>";
                    }
                    
                    shape.SetDescription(description);
                    map.AddShape(shape);
                }
            }
            );        
    }
    
    function addToMyFields(fieldID)
    {
        WLQuickApps.FieldManager.WebSite.SiteService.AddToMyFields(
            fieldID, 
            function()
            {
                document.location = "ViewLeague.aspx?leagueID=" + leagueID;
            });
    }

    function addToThisLeague(fieldID)
    {
        WLQuickApps.FieldManager.WebSite.SiteService.AddFieldToLeague(
            fieldID, 
            leagueID,
            function()
            {
                document.location = "ViewLeague.aspx?leagueID=" + leagueID;
            });
    }
    
    </script>
    <div class="pageContent">
        <div class="header">View League</div>
        <div id="_map" class="LeagueMap"></div>
        
        <div class="ViewLeagueTableDiv">
            <div class="subHeader">Fields in this League</div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                AllowPaging="true" PageSize="5" DataSourceID="_leagueFieldDataSource"
                PagerStyle-HorizontalAlign="Center" PagerSettings-PageButtonCount="10" PagerStyle-CssClass="PagerStyle"
                CssClass="ViewLeagueTable" AlternatingRowStyle-CssClass="LeagueAltRow" HeaderStyle-CssClass="LeagueHeader" GridLines="Horizontal">
                <Columns>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="_nameLink" runat="server" NavigateUrl='<%# string.Format("~/Field/ViewField.aspx?fieldID={0}", Eval("FieldID")) %>'><%# Eval("Title") %></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Description" HeaderText="Description" 
                        SortExpression="Description" />
                    <asp:BoundField DataField="Address" HeaderText="Address"
                        SortExpression="Address"  />
                    <asp:CheckBoxField DataField="IsOpen" HeaderText="IsOpen" 
                        SortExpression="IsOpen" />
                    <asp:BoundField DataField="Status" HeaderText="Status" 
                        SortExpression="Status" />
                    <asp:TemplateField HeaderText="Remove">
                        <ItemTemplate>
                            <asp:LinkButton ID="_removeLink" runat="server" CommandArgument='<%# Eval("FieldID") %>' Visible='<%# this._adminPanel.Visible %>'  OnClick="_removeFieldClick">Remove</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="_leagueFieldDataSource" runat="server" 
            SelectMethod="GetFieldsForLeague" 
            TypeName="WLQuickApps.FieldManager.Business.FieldsManager">
            <SelectParameters>
                <asp:QueryStringParameter Name="leagueID" QueryStringField="leagueID" 
                    Type="Int32" />
            </SelectParameters>
            </asp:ObjectDataSource>
        </div>             
        <asp:Panel ID="_adminPanel" runat="server">        
            <div  class="rightPanel">
                <div class="subHeader">League Options</div>
                <table width="100%">
                    <tr><td class="tableLabel">Title</td><td><asp:TextBox ID="_titleTextBox" runat="server" MaxLength="32"></asp:TextBox></td></tr>
                    <tr><td class="tableLabel">Description</td><td><asp:TextBox ID="_descriptionTextBox" runat="server" MaxLength="64"></asp:TextBox></td></tr>
                    <tr><td class="tableLabel">Type</td><td><asp:TextBox ID="_typeTextBox" runat="server" MaxLength="32"></asp:TextBox></td></tr>
                </table>
                <asp:LinkButton ID="_updateLink" runat="server" CssClass="optionsLink" OnClick="_updateClick">Update League</asp:LinkButton>
                <asp:LinkButton ID="_deleteLink" runat="server" CssClass="optionsLink" OnClick="_deleteClick">Delete League</asp:LinkButton>
                <br /><br />
                <div class="subHeader">Add Admin To League</div>
                <table width="100%">
                    <tr>
                        <td class="tableLabel">By Email</td>
                        <td><asp:TextBox ID="_addAdminByEmailTextBox" runat="server" MaxLength="256"></asp:TextBox></td>
                        <td><asp:LinkButton ID="_addAdminByEmailButton" CssClass="optionsLink" runat="server" OnClick="_addAdminByEmailButtonClick" Text="Add" /></td>
                    </tr>
                </table>
            </div>
        </asp:Panel>              
    </div>       
</asp:Content>