<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewAlbums.aspx.cs" Inherits="Media_ViewAlbums" Title="View Galleries" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
    <cc:DropShadowPanel ID="DropShadowPanel1" runat="server">
        <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ImageGallery-title">
             <asp:Label ID="_titleLabel" runat="server" Text="Galleries"></asp:Label> 
        </cc:DropShadowPanel>
        <cc:MetaGallery runat="server" ID="_albumsGallery" AllowPaging="true" ViewMode="Album" PageSize="8" RepeatColumns="4"
            EmptyDataText="No galleries were found." /><br />
        
        <asp:ObjectDataSource runat="server" ID="_userAlbumsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.AlbumManager"
            SelectMethod="GetAlbumsByUserID" SelectCountMethod="GetAlbumsByUserIDCount" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true">
            <SelectParameters>
                <asp:Parameter Name="userID" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
        <asp:ObjectDataSource runat="server" ID="_baseItemAlbumsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.AlbumManager"
            SelectMethod="GetAlbumsByBaseItemID" SelectCountMethod="GetAlbumsByBaseItemIDCount" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true">
            <SelectParameters>
                <asp:QueryStringParameter Name="baseItemID" QueryStringField="baseItemID" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
        <asp:HyperLink runat="server" ID="_associateAlbumLink" Visible="false">
            <strong>
                Associate a gallery &gt;&gt;
            </strong>
        </asp:HyperLink> 
        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div style="z-index: -100; position: absolute; width: 100%; height: 100%; left: 0px; top: 0px; background-color: Gray; filter:alpha(opacity=60); -moz-opacity: 0.6; opacity: 0.6; font-size: large; text-align: center;">Please wait...</div> 
        </ProgressTemplate>
    </asp:UpdateProgress>   
    </cc:DropShadowPanel>
</ContentTemplate>
</asp:UpdatePanel> 
   
<asp:HyperLink ID="_addAlbumHyperLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Media/CreateAlbum.aspx">Create Gallery &gt;&gt;</asp:HyperLink>
</asp:Content>