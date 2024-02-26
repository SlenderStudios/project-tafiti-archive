<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Collection_Default" Title="Browse Maps" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server"><cc:DropShadowPanel runat="server" ID="_collectionsPanel">
    <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
        Maps &amp; Places
    </cc:DropShadowPanel>
    <cc:MetaGallery ID="_collectionsGallery" runat="server" DataSourceID="_collectionsDataSource" DataKeyField="BaseItemID"
        AllowPaging="True" PageSize="12" RepeatColumns="4" ViewMode="List" EmptyDataText="There are no maps available." />
    </cc:DropShadowPanel>
    <asp:ObjectDataSource ID="_collectionsDataSource" runat="server" SelectMethod="GetCollectionsByCollectionType"
        TypeName="WLQuickApps.SocialNetwork.Business.CollectionManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" SelectCountMethod="GetCollectionsByCollectionTypeCount">
        <SelectParameters>
            <asp:Parameter Name="collectionType" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
    
    <asp:HyperLink ID="_createCollectionLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Collection/CreateCollection.aspx">Create Map &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_viewMyCollectionsLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Collection/ViewCollections.aspx">View My Maps &amp; Places &gt;&gt;</asp:HyperLink>

</asp:Content>
