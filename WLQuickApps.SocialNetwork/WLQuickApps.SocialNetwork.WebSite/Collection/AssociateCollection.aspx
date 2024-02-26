<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssociateCollection.aspx.cs" Inherits="Collection_AssociateCollection" Title="Untitled Page" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    Select a map to associate to <asp:Label runat="server" ID="_groupName" />.
    <asp:UpdatePanel runat="server" ID="_updatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <cc:DropShadowPanel runat="server" SkinID="ImageGallery" ID="_galleryPanel">
                <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
                    Map:&nbsp;<asp:DropDownList ID="_collectionList" runat="server" AutoPostBack="True" DataSourceID="_dataSource"
                        DataTextField="Title" DataValueField="BaseItemID" />
                    <asp:ObjectDataSource ID="_dataSource" runat="server" SelectMethod="GetAllCollections"
                        TypeName="WLQuickApps.SocialNetwork.Business.CollectionManager" />
                </cc:DropShadowPanel>
                <br />
                <asp:Panel runat="server" ID="_collectionDetailsPanel">
                    <asp:HiddenField runat="server" ID="_geoRSSSourceURLField" />
                    <script type="text/javascript">
                        var map = null;

                        function GetMap()
                        {
                            map = new VEMap("map");
                            map.onLoadMap = LoadCollection;
                            map.LoadMap();                    
                        }   

                        function LoadCollection()
                        {
                            var l = new VEShapeLayer();
                            var veLayerSpec = new VEShapeSourceSpecification(VEDataType.GeoRSS, $get('<%= _geoRSSSourceURLField.ClientID %>').value, l);
                            map.ImportShapeLayerData(veLayerSpec);
                        }
                        
                        function PanToLatLong(latitude, longitude)
                        {
                            map.PanToLatLong(new VELatLong(latitude, longitude));
                        }
                        
                        Sys.Application.add_init(GetMap);
                        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(GetMap);
                    </script>
                    <div id="map" style="position:relative; width:100%; height:400px;"></div><br />
                </asp:Panel>
                <div style="float:right">
                    <asp:Label runat="server" ID="_alreadyAssociatedErrorLabel" Text="<strong>This map has already been associated.</strong><br />" Visible="false" />
                    <asp:Button runat="server" ID="_createNewButton" OnClick="_createNewButton_Click" Text="Create New Map" />
                    <asp:Button runat="server" ID="_associateButton" OnClick="_associateButton_Click" Text="Associate this Map" />
                </div>
                <div class="clearFloats"></div>
            </cc:DropShadowPanel>
        </ContentTemplate>
        <%--Triggers>
            <asp:AsyncPostBackTrigger ControlID="_albumList" EventName="SelectedIndexChanged" />
        </Triggers--%>
    </asp:UpdatePanel>
</asp:Content>