<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditItems.aspx.cs" Inherits="Collection_EditItems" Title="Edit Map" %>
<%@ Register TagPrefix="uc1" TagName="Location" Src="~/Controls/LocationControl.ascx"  %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="_updatePanel" runat="server">
        <ContentTemplate>
            <cc:DropShadowPanel ID="_addItemPanel" runat="server" SkinID="EditCollectionItems-AddItem">
                <cc:DropShadowPanel ID="_addItemPanelTitle" runat="server" SkinID="ImageGallery-title">
                    Add Place
                </cc:DropShadowPanel>
                
                <div class="form-errorRow">
                    <asp:ValidationSummary ID="_errorSummary" runat="server" DisplayMode="BulletList" />
                </div>
                
                <div class="form-label form-required addItemForm">
                    <asp:Label ID="_nameLabel" runat="server" Text="Name" AssociatedControlID="_name" />
                </div>
                <div class="form-field addItemForm">
                    <cc:SecureTextBox ID="_name" runat="server" />
                    <asp:RequiredFieldValidator ID="_nameRequired" runat="server" ControlToValidate="_name"
                        ErrorMessage="Enter a place name." ToolTip="Enter a place name." Text="*" Display="Dynamic" />
                </div>
                <div class="form-label addItemForm">
                    <asp:Label ID="_descriptionLabel" runat="server" AssociatedControlID="_description">Description</asp:Label>
                </div>
                <div class="form-field addItemForm">
                    <cc:SecureTextBox runat="server" ID="_description" TextMode="MultiLine" Columns="30" Rows="5" />
                </div>
                <div class="form-label form-required addItemForm">
                    <asp:Label ID="_locationLabel" runat="server" AssociatedControlID="_location">Location</asp:Label>
                </div>
                <div class="form-field addItemForm">
                    <uc1:Location runat="server" ID="_location" DisplayNameBox="True" />
                    <asp:CustomValidator ID="_locationRequired" runat="server" OnServerValidate="_locationRequired_ServerValidate"
                        Text="*" ToolTip="Enter a valid address." ErrorMessage="Enter a valid address." />
                </div>        
                <div class="form-field addItemForm">
                    <asp:Button ID="_addItem" runat="server" Text="Add" OnClick="_addItem_Click" />
                </div>
            </cc:DropShadowPanel>
            
            <asp:FormView ID="_collectionView" runat="server" DataSourceID="_collectionDataSource">
                <ItemTemplate>
                    <cc:DropShadowPanel ID="_itemsPanel" runat="server" SkinID="EditCollectionItems-Items">
                        <cc:DropShadowPanel ID="_itemsTitle" runat="server" SkinID="EditCollectionItems-ItemsTitle">
                            <%# ((Collection)Container.DataItem).Title %>
                        </cc:DropShadowPanel>
                        <asp:GridView ID="_itemsGrid" runat="server" DataSourceID="_collectionItemsDataSource"
                            AutoGenerateColumns="False" ShowHeader="False" DataKeyNames="BaseItemID" OnRowDeleting="_itemsGrid_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" ItemStyle-Font-Bold="True" ItemStyle-Width="100%" />
                                <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" />
                            </Columns>
                            <EmptyDataTemplate>
                                You haven't added any places yet.
                            </EmptyDataTemplate>
                        </asp:GridView><br />
                        
                        <asp:HyperLink ID="_viewLink" runat="server" Text="View Map &gt;&gt;" Font-Bold="True"
                            NavigateUrl='<%# WebUtilities.GetViewItemUrl(((Collection)Container.DataItem)) %>' />
                    </cc:DropShadowPanel>
                </ItemTemplate>
            </asp:FormView>
            
            <asp:ObjectDataSource ID="_collectionDataSource" runat="server" SelectMethod="GetCollection"
                TypeName="WLQuickApps.SocialNetwork.Business.CollectionManager">
                <SelectParameters>
                    <asp:QueryStringParameter Name="baseItemID" QueryStringField="baseItemID" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="_collectionItemsDataSource" runat="server" DataObjectTypeName="WLQuickApps.SocialNetwork.Business.CollectionItem"
                DeleteMethod="DeleteCollectionItem" SelectMethod="GetCollectionItemsForCollection" TypeName="WLQuickApps.SocialNetwork.Business.CollectionItemManager"
                OnSelecting="_collectionItemsDataSource_Selecting">
            </asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="_updateProgress" runat="server" AssociatedUpdatePanelID="_updatePanel" DisplayAfter="500">
        <ProgressTemplate>
            <cc:DropShadowPanel ID="_updateProgressPanel" runat="server" SkinID="EditCollectionItems-UpdateProgress">
                Updating&#0133;
            </cc:DropShadowPanel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
