<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewCollections.aspx.cs" Inherits="Collection_ViewCollections" Title="View Maps &amp; Places"%>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel ID="DropShadowPanel1" runat="server">
        <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ImageGallery-title">
             <asp:Label ID="_titleLabel" runat="server" Text="Maps"></asp:Label>   
        </cc:DropShadowPanel>
        <cc:MetaGallery runat="server" ID="_collectionsGallery"  ViewMode="List" AllowPaging="true" PageSize="8"
            RepeatColumns="4" RepeatDirection="Horizontal" EmptyDataText="No maps were found." /><br />
        
        <asp:ObjectDataSource runat="server" ID="_userCollectionsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.CollectionManager"
            SelectMethod="GetCollectionsByUser" SelectCountMethod="GetCollectionsByUserCount" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true">
            <SelectParameters>
                <asp:QueryStringParameter Name="userName" QueryStringField="userName" Type="String" />
                <asp:Parameter Name="collectionType" Type="String" DefaultValue="" />
            </SelectParameters>
        </asp:ObjectDataSource>
                
        <asp:ObjectDataSource runat="server" ID="_baseItemCollectionsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.CollectionManager"
            SelectMethod="GetCollectionsByBaseItemID" SelectCountMethod="GetCollectionsByBaseItemIDCount" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true">
            <SelectParameters>
                <asp:QueryStringParameter Name="baseItemID" QueryStringField="baseItemID" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    
        <asp:HyperLink runat="server" ID="_associateCollectionLink" Visible="false">
            <strong>
                Associate a map &gt;&gt;
            </strong>
        </asp:HyperLink>
    </cc:DropShadowPanel>
    
    <asp:HyperLink ID="_createCollectionLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Collection/CreateCollection.aspx">Create Map &gt;&gt;</asp:HyperLink><br />
</asp:Content>