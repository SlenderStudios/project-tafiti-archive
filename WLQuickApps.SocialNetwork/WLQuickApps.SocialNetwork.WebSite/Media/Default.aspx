<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Media_Default" Title="Browse Media" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" ID="_picturesPanel" >
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            Pictures
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_picturesGallery" runat="server" DataSourceID="_picturesDataSource" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="12" RepeatColumns="4" ViewMode="Media" EmptyDataText="No pictures were found." />
    </cc:DropShadowPanel>
    <asp:ObjectDataSource ID="_picturesDataSource" runat="server" SelectMethod="GetMostRecentMedia"
        TypeName="WLQuickApps.SocialNetwork.Business.MediaManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" SelectCountMethod="GetTotalMediaCount">
        <SelectParameters>
            <asp:Parameter Name="mediaType" Type="object" DefaultValue="Picture" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <cc:DropShadowPanel runat="server" ID="DropShadowPanel1" >
        <cc:DropShadowPanel runat="server" ID="DropShadowPanel2" SkinID="ImageGallery-title">
            Videos
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_videosGallery" runat="server" DataSourceID="_videosDataSource" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="12" RepeatColumns="4" ViewMode="Media" EmptyDataText="No videos were found." />
    </cc:DropShadowPanel>
    <asp:ObjectDataSource ID="_videosDataSource" runat="server" SelectMethod="GetMostRecentMedia"
        TypeName="WLQuickApps.SocialNetwork.Business.MediaManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" SelectCountMethod="GetTotalMediaCount">
        <SelectParameters>
            <asp:Parameter Name="mediaType" Type="object" DefaultValue="Video" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <cc:DropShadowPanel runat="server" ID="DropShadowPanel3" >
        <cc:DropShadowPanel runat="server" ID="DropShadowPanel4" SkinID="ImageGallery-title">
            Audio
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_audioGallery" runat="server" DataSourceID="_audioDataSource" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="12" RepeatColumns="4" ViewMode="Media" EmptyDataText="No audio were found." />
    </cc:DropShadowPanel>
    <asp:ObjectDataSource ID="_audioDataSource" runat="server" SelectMethod="GetMostRecentMedia"
        TypeName="WLQuickApps.SocialNetwork.Business.MediaManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" SelectCountMethod="GetTotalMediaCount">
        <SelectParameters>
            <asp:Parameter Name="mediaType" Type="object" DefaultValue="Audio" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:HyperLink ID="_createAlbumLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Media/CreateAlbum.aspx">Create Gallery &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_addPictureLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Media/AddPicture.aspx">Add Picture &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_addVideoLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Media/AddVideo.aspx">Add Video &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_addAudioLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Media/AddAudio.aspx">Add Audio &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_viewMyAlbumsLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Media/ViewAlbums.aspx">View My Galleries &gt;&gt;</asp:HyperLink>

</asp:Content>

