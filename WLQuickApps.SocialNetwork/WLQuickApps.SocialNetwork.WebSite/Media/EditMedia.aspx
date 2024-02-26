<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditMedia.aspx.cs" Inherits="Media_EditMedia" Title="Untitled Page" %>
<%@ Register Src="../Controls/LocationControl.ascx" TagName="LocationControl" TagPrefix="uc" %>

<asp:Content ID="_mainContent" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" SkinID="ImageGallery">
        <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">Edit Media</cc:DropShadowPanel>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="BaseItemID" GridLines="None"
            DataSourceID="_mediaDataSource" DefaultMode="Edit" OnItemUpdating="DetailsView1_ItemUpdating" OnModeChanged="DetailsView1_ModeChanged">
            <HeaderTemplate>
                <asp:ValidationSummary runat="server" ID="_validationSummary" DisplayMode="BulletList" />
                <asp:Label runat="server" ID="_invalidThumbnailLabel" Text="Select a valid image to set as the thumbnail" Visible="false" />
            </HeaderTemplate>
            <Fields>
                <asp:TemplateField ShowHeader="false" SortExpression="Title">
                    <EditItemTemplate>
                        <div class="form-label">
                            <asp:Label runat="server" ID="_imageLabel" Text='<%# (((Media)Container.DataItem).MediaType == MediaType.Picture) ? "Picture" : "Thumbnail" %>' />
                        </div>
                        <div class="form-field">
                            <cc:NullablePicture runat="server" ID="_image" Item='<%# (BaseItem)Container.DataItem %>' MaxHeight="256" MaxWidth="256" />
                        </div>
                        <asp:Panel runat="server" ID="_uploadThumbnailPanel" Visible='<%# ((Media)Container.DataItem).MediaType != MediaType.Picture %>'>
                            <div class="form-label">
                                New Thumbnail
                            </div>
                            <div class="form-field">
                                <asp:FileUpload runat="server" ID="_newThumbnailUpload" />
                            </div>
                        </asp:Panel>
                        <div class="form-label form-required">
                            Title
                        </div>
                        <div class="form-field">
                            <cc:SecureTextBox ID="_title" runat="server" Text='<%# Bind("Title") %>' />
                            <asp:RequiredFieldValidator runat="server" ID="_titleRequired" ControlToValidate="_title"
                                ErrorMessage="Enter a title." ToolTip="Enter a title" Text="*" />
                        </div>
                        <div class="form-label">
                            Description
                        </div>
                        <div class="form-field">
                            <cc:SecureTextBox ID="_descriptionTextBox" runat="server" Height="94px" TextMode="MultiLine" Width="276px"
                                Text='<%# Bind("Description") %>' />
                        </div>
                        <div class="form-label">
                            Location
                        </div>
                        <div class="form-field">
                            <uc:LocationControl id="_locationControl" runat="server" ShowLocationCaption="false" OnInit="_locationControl_Init" />
                        </div>
                        <div class="form-field">
                            <br /><br />
                            <asp:Button runat="server" ID="_update" CommandName="Update" Text="Save" />&nbsp;
                            <asp:Button runat="server" ID="_cancel" CausesValidation="false" CommandName="Cancel" Text="Cancel" />
                        </div>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Fields>
        </asp:DetailsView>
        <asp:ObjectDataSource ID="_mediaDataSource" runat="server" SelectMethod="GetMedia"
            TypeName="WLQuickApps.SocialNetwork.Business.MediaManager">
            <SelectParameters>
                <asp:QueryStringParameter Name="baseItemID" QueryStringField="baseItemID" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </cc:DropShadowPanel>
</asp:Content>