<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateAlbum.aspx.cs" Inherits="Media_CreateAlbum" Title="Untitled Page" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" ID="_createAlbumPanel" >
        <cc:DropShadowPanel runat="server" ID="_createAlbumPanelTitle" SkinID="ImageGallery-title">
            Gallery Details
        </cc:DropShadowPanel>
        <div class="form-field form-errorRow">
            <asp:ValidationSummary ID="_errorSummary" runat="server" DisplayMode="BulletList" />
            <asp:Label runat="server" ID="_invalidPictureError" Text="Select a valid image for thumbnail or leave blank." Visible="false" />
        </div>
        <div class="form-label form-required">
            Name
        </div>
        <div class="form-field">
            <cc:SecureTextBox ID="_nameTextBox" runat="server" /><asp:RequiredFieldValidator runat="server" ID="_nameTextBoxRequiredValidator"
                ControlToValidate="_nameTextBox" Display="Dynamic" Text="*" ToolTip="Enter a gallery name." ErrorMessage="Enter a gallery name." />
        </div>
        <div class="form-label">
            Thumbnail
        </div>
        <div class="form-field">
            <asp:FileUpload runat="server" ID="_thumbnailUpload" />
        </div>
         <div class="form-label">
            Create In Windows Live Spaces?
        </div>
        <div class="form-field">
            <asp:CheckBox runat="server" id="chkSaveToSpaces"></asp:CheckBox> Tick this box if you have granted permission to your Windows Live Space (<asp:hyperlink runat=server NavigateUrl="~/User/EditProfile.aspx" Target=_blank Text="grant permission here"></asp:hyperlink>) and want this album created in Windows Live Spaces.
        </div>
        <br />
        <br />
        <div class="form-field">
            <asp:Button ID="_createButton" runat="server" OnClick="_createButton_Click" Text="Create" CausesValidation="true" />
        </div>
    </cc:DropShadowPanel>
</asp:Content>

