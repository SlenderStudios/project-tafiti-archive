<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditAlbum.aspx.cs" Inherits="Media_EditAlbum" Title="Edit Gallery" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <cc:DropShadowPanel runat="server" ID="_formPanel" >
        <cc:DropShadowPanel runat="server" ID="_formPanelTitle" SkinID="ImageGallery-title">
            Gallery Details
        </cc:DropShadowPanel>
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="baseItemID" GridLines="None"
            DataSourceID="_albumDataSource" DefaultMode="Edit" OnModeChanged="DetailsView1_ModeChanged" OnItemUpdating="DetailsView1_ItemUpdating">
            <HeaderTemplate>
                <div class="form-field form-errorRow">
                    <asp:ValidationSummary runat="server" ID="_validationSummary" DisplayMode="BulletList" />
                    <asp:Label runat="server" ID="_invalidThumbnailLabel" Text="New thumbnail selected was invalid." Visible="false" />
                </div>
            </HeaderTemplate>
            <Fields>
                <asp:TemplateField ShowHeader="false" SortExpression="Name">
                    <EditItemTemplate>
                        <div class="form-label form-required">
                            Name
                        </div>
                        <div class="form-field">
                            <cc:SecureTextBox ID="_name" runat="server" Text='<%# Bind("Title") %>' />
                            <asp:RequiredFieldValidator runat="server" ID="_nameRequired" ControlToValidate="_name"
                                ErrorMessage="Enter a gallery name." ToolTip="Enter a gallery name." Text="*" />
                        </div>
                        
                        <div class="form-label">
                            Thumbnail
                        </div>
                        <div class="form-field">
                            <asp:FileUpload ID="_pictureFileUpload" runat="server" /><br />
                            <strong><asp:Label runat="server" ID="_existingThumbnailLabel" Text="Existing thumbnail:<br />" 
                                Visible='<%# ((Album)Container.DataItem).HasThumbnail %>'/></strong>
                            <asp:Image runat="server" ID="_existingThumbnail" Visible='<%# ((Album)Container.DataItem).HasThumbnail %>' 
                                ImageUrl='<%# WebUtilities.GetViewImageUrl(((BaseItem)Container.DataItem).BaseItemID, 128, 128) %>'/>
                        </div>
                        <br />
                        <asp:Panel runat="server" ID="_associationsVisiblePanel"> 
                        <div class="form-label">
                            Associations
                         </div> 
                         <div class="form-field">
                            <cc:DropShadowPanel runat="server" ID="_associationsPanel" Width="350"> 
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
                            </cc:DropShadowPanel>
                        </div>
                        </asp:Panel>
                        <br />
                        <div class="form-field">
                            <asp:Button runat="server" ID="_update" CommandName="Update" Text="Save" />
                            <asp:Button runat="server" ID="_cancel" CausesValidation="false" CommandName="Cancel" Text="Cancel" />
                        </div>
                    </EditItemTemplate>
                </asp:TemplateField>
            </Fields>
        </asp:DetailsView>
        <br />
    </cc:DropShadowPanel>
    <br />
    <asp:ObjectDataSource ID="_albumDataSource" runat="server" SelectMethod="GetAlbum"
        TypeName="WLQuickApps.SocialNetwork.Business.AlbumManager">
        <SelectParameters>
            <asp:QueryStringParameter Name="baseItemID" QueryStringField="baseItemID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:objectdatasource id="_associatedItemDataSource" runat="server" selectmethod="GetBaseItemsAssociatedWithBaseItem"
            typename="WLQuickApps.SocialNetwork.Business.BaseItemManager" OnSelecting="_associatedItemDataSource_Selecting">
        <SelectParameters>
            <asp:Parameter Type="Object" Name="baseItem"></asp:Parameter>
        </SelectParameters>
    </asp:objectdatasource>
</asp:Content>

