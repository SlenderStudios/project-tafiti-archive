<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditCollection.aspx.cs" Inherits="Collection_EditCollection" Title="Edit Map" %>
 
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel ID="_editCollectionPanel" runat="server">
        <cc:DropShadowPanel ID="_editCollectionDetails" runat="server" SkinID="ImageGallery-title">
            Map Details
        </cc:DropShadowPanel>
        <div class="form-field form-errorRow">
            <asp:ValidationSummary runat="server" ID="_errorSummary" />
            <asp:Label runat="server" ID="_invalidThumbnailLabel" Text="Select a valid image file to upload." Visible="false" />
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
            Thumbnail
        </div>
        <div class="form-field">
            <asp:FileUpload ID="_pictureFileUpload" runat="server" /><br />
            <strong><asp:Label runat="server" ID="_existingThumbnailLabel" Text="Existing thumbnail:<br />" Visible="false" /></strong>
            <asp:Image runat="server" ID="_existingThumbnail" Visible="false" />
        </div>
        <br />
        <asp:Panel runat="server" ID="_associationsVisiblePanel"> 
            <div class="form-label">
                Associations
            </div> 
            <div class="form-field">
                <asp:Panel runat="server" ID="_associationsPanel" Width="350"> 
                    <asp:Gridview id="_associationsGridView" runat="server" ShowHeader="false" autogeneratecolumns="False" 
                        datasourceid="_associatedItemDataSource" OnDataBound="_associationsGridView_DataBound">
                        <Columns>
                            <asp:TemplateField Visible="False"> 
                                <ItemTemplate>
                                    <asp:Label ID="_baseItemIDLabel" runat="server" Text='<%# Eval("BaseItemID") %>' /> 
                              </ItemTemplate> 
                            </asp:TemplateField> 
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="_keepCheckBox" Checked="true"></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="_baseItemHyperLink" runat="server" Text='<%# Bind("Title") %>'
                                        NavigateUrl='<%# this.GetViewItemUrl((BaseItem) Container.DataItem) %>'> 
                                    </asp:HyperLink>                  
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:Gridview>
                </asp:Panel>
            </div>
        </asp:Panel>
        <br />
        <div class="form-field">
            <asp:Button runat="server" ID="_updateButton" Text="Save" OnClick="_updateButton_Click" />
            <asp:Button runat="server" ID="_cancelButton" CausesValidation="false" Text="Cancel" OnClick="_cancelButton_Click" />
        </div>    
        <br />   
    </cc:DropShadowPanel>
    <asp:objectdatasource id="_associatedItemDataSource" runat="server" selectmethod="GetBaseItemsAssociatedWithBaseItem"
            typename="WLQuickApps.SocialNetwork.Business.BaseItemManager" OnSelecting="_associatedItemDataSource_Selecting">
        <SelectParameters>
            <asp:Parameter Type="Object" Name="baseItem"></asp:Parameter>
        </SelectParameters>
    </asp:objectdatasource>
</asp:Content>

