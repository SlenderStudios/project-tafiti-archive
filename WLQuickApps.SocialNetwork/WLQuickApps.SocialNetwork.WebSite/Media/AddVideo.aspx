<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddVideo.aspx.cs"
    Inherits="Media_AddVideo" Title="Untitled Page" %>

<%@ Register Src="../Controls/LocationControl.ascx" TagName="LocationControl" TagPrefix="uc1" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
<cc:DropShadowPanel runat="server" SkinID="ImageGallery">
    <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">Upload Video</cc:DropShadowPanel>
    <div class="form-field form-errorRow">
        <asp:ValidationSummary ID="_errorSummary" runat="server" DisplayMode="BulletList" />
        <asp:Label runat="server" ID="_uploadError" Text="Choose a valid thumbnail to upload." />
    </div>
    <div class="form-label form-required">
        Upload
    </div>
    <div class="form-field">
        <asp:FileUpload ID="_videoFileUpload" runat="server" />
        <asp:RequiredFieldValidator ID="_fileRequired" runat="server" ControlToValidate="_videoFileUpload"
            Text="*" ErrorMessage="Select a video file to upload." ToolTip="Select a file." />
	(8MB limit)
    </div>
    <div class="form-label">
        Thumbnail
    </div>
    <div class="form-field">
        <asp:FileUpload ID="_thumbnailFileUpload" runat="server" />
    </div>
    <div class="form-label form-required">
        <asp:Label runat="server" ID="_galleryLabel" Text="Gallery" />
    </div>
    <div class="form-field">
        <asp:UpdatePanel runat="server" ID="_galleryUpdatePanel" RenderMode="Inline">
            <ContentTemplate>
                <asp:DropDownList ID="_albumDropDownList" runat="server" DataSourceID="ObjectDataSource1" OnSelectedIndexChanged="_albumDropDownList_SelectedIndexChanged" AutoPostBack="true"
                    DataTextField="Title" DataValueField="BaseItemID" OnDataBound="_albumDropDownList_DataBound">
                </asp:DropDownList>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAllAlbums"
                    TypeName="WLQuickApps.SocialNetwork.Business.AlbumManager" />
                <cc:SecureTextBox runat="server" ID="_createAlbumText" Visible="False" />
                <asp:RequiredFieldValidator ID="_createAlbumRequired" runat="server" ControlToValidate="_createAlbumText"
                    Text="*" ErrorMessage="Enter a name for a new gallery to place this video in."
                    ToolTip="Enter a name for a new gallery." />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="form-label form-required">
        Video Title
    </div>
    <div class="form-field">
        <cc:SecureTextBox ID="_captionTextBox" runat="server" />
        <asp:RequiredFieldValidator ID="_captionRequired" runat="server" ControlToValidate="_captionTextBox"
            Text="*" ErrorMessage="Enter a title for the video." ToolTip="Enter a title." />
    </div>
    <div class="form-label">
        Description
    </div>
    <div class="form-field">
        <cc:SecureTextBox ID="_descriptionTextBox" runat="server" Height="94px" TextMode="MultiLine" Width="276px" />
    </div>
    <div class="form-label">
        Location
    </div>
    <div class="form-field">
        <uc1:LocationControl ID="_locationControl" runat="server" ShowLocationCaption="false" />
    </div>
    <div class="form-field">
        <br />
        <asp:Button ID="_uploadButton" runat="server" Text="Add Video" OnClick="_uploadButton_Click" />
    </div>
</cc:DropShadowPanel>    
</asp:Content>
