<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewCollection.aspx.cs" Inherits="Collection_ViewCollection" Title="View Map" %>
<%@ Register TagPrefix="uc1" TagName="Tags" Src="~/Controls/Tags.ascx" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="_collectionView" runat="server" DataSourceID="_collectionDataSource">
        <ItemTemplate>
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
                    if (<%= CollectionManager.GetCollection(Convert.ToInt32(this.Request.QueryString["baseItemID"])).Items.Count %> > 0)
                    {
                        var l = new VEShapeLayer();
                        var veLayerSpec = new VEShapeSourceSpecification(VEDataType.GeoRSS,
                            '<%= Convert.ToInt32(this.Request.QueryString["baseItemID"]) %>.axd', l);
                        map.ImportShapeLayerData(veLayerSpec);
                    }
                }
                
                function PanToLatLong(latitude, longitude)
                {
                    map.PanToLatLong(new VELatLong(latitude, longitude));
                }
                
                Sys.Application.add_init(GetMap);
            </script>
            
            <cc:DropShadowPanel ID="_mapPanel" runat="server" Width="450px" SkinID="ViewCollection-Map">
                <div id="map" style="position:relative; width:100%; height:400px;"></div>
            </cc:DropShadowPanel>
            
            <asp:Panel runat="server" ID="_rightPanel" SkinID="ViewCollection-Sidebar">
                <cc:DropShadowPanel runat="server" ID="_mainPanel" SkinID="ViewCollection-Details">
                    <asp:Label ID="_collectionTitle" runat="server" Text='<%# ((Collection)Container.DataItem).Title %>'
                        SkinID="ViewCollection-Title" />
                    <br />
                    <cc:NullablePicture ID="_collectionImage" runat="server" Item='<%# Container.DataItem %>' MaxWidth="150" MaxHeight="256"
                        SkinID="ViewCollection-Image" />
                    <br />
                    <strong>created by</strong>
                    <asp:HyperLink ID="_creatorLink" runat="server" Text='<%# ((Collection)Container.DataItem).Owner.Title %>'
                        NavigateUrl='<%# WebUtilities.GetViewItemUrl(((Collection)Container.DataItem).Owner) %>' /><br />
                    
                    <asp:Panel ID="_editPanel" runat="server" Visible='<%# UserManager.IsUserLoggedIn() %>'>
                        <asp:LinkButton ID="_copyLink" runat="server" Text="Copy Map" Visible="true"
                            OnCommand="_copyLink_Command" CommandArgument='<%# ((Collection)Container.DataItem).BaseItemID %>' />
                        <br />
                        <asp:HyperLink ID="_editLink" runat="server" Text="Edit Places" Visible='<%# ((Collection)Container.DataItem).CanEdit %>'
                            NavigateUrl='<%# string.Format("~/Collection/EditCollection.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, ((Collection)Container.DataItem).BaseItemID) %>' />
                        <br />
                        <asp:LinkButton ID="_deleteLink" runat="server" Text="Delete Map" Visible='<%# ((Collection)Container.DataItem).CanDelete %>'
                            OnCommand="_deleteLink_Command" CommandArgument='<%# ((Collection)Container.DataItem).BaseItemID %>' />
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteLink"
                            ConfirmText="Are you sure you want to delete this map?" />
                    </asp:Panel><br />
                    
                    <strong>tagged with</strong><br />
                    <uc1:Tags runat="server" ID="_tags" BaseItemID='<%# ((Collection)Container.DataItem).BaseItemID %>' />
                </cc:DropShadowPanel>
                
                <cc:DropShadowPanel ID="_itemsPanel" runat="server" SkinID="ViewCollection-Items">
                    <h3>In This Map</h3>
                    <div style="overflow-x:scroll">
                        <asp:DataList ID="_itemList" runat="server" DataSource='<%# ((Collection)Container.DataItem).Items %>' OnPreRender="_itemList_PreRender">
                            <HeaderTemplate>
                                This map doesn't contain any places.
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink ID="_itemTitle" runat="server" Text='<%# ((CollectionItem)Container.DataItem).Title %>'
                                    NavigateUrl='<%# string.Format("javascript:PanToLatLong({0}, {1})", ((CollectionItem)Container.DataItem).Location.Latitude, ((CollectionItem)Container.DataItem).Location.Longitude) %>'
                                    SkinID="ViewCollection-ItemTitle" />
                                
                                <asp:Label ID="_itemDescription" runat="server" SkinID="ViewCollection-ItemDescription"
                                    Text='<%# ((CollectionItem)Container.DataItem).Description %>' />
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    
                    <asp:HyperLink ID="_editItemsLink" runat="server" Visible='<%# Eval("CanEdit") %>' Text="<br />Add/Remove Places"
                        NavigateUrl='<%# string.Format("~/Collection/EditItems.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, ((Collection)Container.DataItem).BaseItemID) %>' />
                </cc:DropShadowPanel>
            </asp:Panel>
        </ItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="_collectionDataSource" runat="server" SelectMethod="GetCollection"
        TypeName="WLQuickApps.SocialNetwork.Business.CollectionManager">
        <SelectParameters>
            <asp:QueryStringParameter Name="baseItemID" QueryStringField="baseItemID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
