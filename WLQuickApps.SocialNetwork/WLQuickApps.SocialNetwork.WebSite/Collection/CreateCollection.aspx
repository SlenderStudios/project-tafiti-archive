<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateCollection.aspx.cs" Inherits="Collection_CreateCollection" Title="Create Map" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel ID="_addCollectionPanel" runat="server">
        <cc:DropShadowPanel ID="_addCollectionDetails" runat="server" SkinID="ImageGallery-title">
            Map Details
        </cc:DropShadowPanel>
        <div class="form-errorRow form-field">
            <asp:ValidationSummary runat="server" ID="_errorSummary" />
            <asp:Label runat="server" ID="_invalidPictureError" Text="Select a valid image for thumbnail or leave blank." Visible="false" />
        </div>          
        <div class="form-label form-required">
            <asp:Label ID="_nameLabel" runat="server" AssociatedControlID="_name">Name</asp:Label>
        </div>
        <div class="form-field">
            <cc:SecureTextBox ID="_name" runat="server" />
            <asp:RequiredFieldValidator ID="_nameRequired" runat="server" ControlToValidate="_name"
                ErrorMessage="Enter a map name." ToolTip="Enter a map name." Text="*" Display="Dynamic" />
        </div>
        <div class="form-label">
            <asp:Label ID="_descriptionLabel" runat="server" AssociatedControlID="_description">Description</asp:Label>
        </div>
        <div class="form-field">
            <cc:SecureTextBox runat="server" ID="_description" TextMode="MultiLine" Columns="30" Rows="5" />
        </div>    
        <div class="form-label">
            <asp:Label ID="_thumbnailLabel" runat="server">Thumbnail</asp:Label>
        </div>
        <div class="form-field">
            <asp:FileUpload ID="_pictureFileUpload" runat="server" />
        </div>
        <br />
        <br />
        <div class="form-field">
            <asp:Button ID="_createButton" runat="server" OnClick="_createButton_Click" Text="Create" />
        </div>            
    </cc:DropShadowPanel>
</asp:Content>

