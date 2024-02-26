<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssociateAlbum.aspx.cs" Inherits="Media_AssociateAlbum" Title="Untitled Page" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    Select a gallery to associate to <asp:Label runat="server" ID="_groupName" />.
    <asp:UpdatePanel runat="server" ID="_updatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <cc:DropShadowPanel runat="server" SkinID="ImageGallery" ID="_galleryPanel">
                <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
                    Gallery:&nbsp;<asp:DropDownList ID="_albumList" runat="server" AutoPostBack="True" DataSourceID="_albumDataSource"
                        DataTextField="Title" DataValueField="BaseItemID" />
                    <asp:ObjectDataSource ID="_albumDataSource" runat="server" SelectMethod="GetAllAlbums"
                        TypeName="WLQuickApps.SocialNetwork.Business.AlbumManager" />
                </cc:DropShadowPanel>
                
                <br />
                <cc:MetaGallery runat="server" ID="_albumDisplay" DataSourceID="_mediaDataSource" AllowPaging="True" PageSize="12"
                    EmptyDataText="This gallery doesn't have any media yet." RepeatColumns="4" RepeatDirection="Horizontal" ViewMode="Square">
                </cc:MetaGallery>
                <asp:ObjectDataSource runat="server" ID="_mediaDataSource" TypeName="WLQuickApps.SocialNetwork.Business.MediaManager"
                    SelectMethod="GetMediaByAlbumBaseItemID" SelectCountMethod="GetMediaByAlbumBaseItemIDCount" EnablePaging="True" OldValuesParameterFormatString="original_{0}" OnSelecting="_mediaDataSource_Selecting">
                </asp:ObjectDataSource>
                <br />
                <div style="float:right">
                    <asp:Label runat="server" ID="_alreadyAssociatedErrorLabel" Text="<strong>This gallery has already been associated.</strong><br />" Visible="false" />
                    <asp:Button runat="server" ID="_createNewButton" OnClick="_createNewButton_Click" Text="Create New Gallery" />
                    <asp:Button runat="server" ID="_associateButton" OnClick="_associateButton_Click" Text="Associate this Gallery" />
                </div>
                <div class="clearFloats"></div>
            </cc:DropShadowPanel>
        </ContentTemplate>
        <%--Triggers>
            <asp:AsyncPostBackTrigger ControlID="_albumList" EventName="SelectedIndexChanged" />
        </Triggers--%>
    </asp:UpdatePanel>
</asp:Content>