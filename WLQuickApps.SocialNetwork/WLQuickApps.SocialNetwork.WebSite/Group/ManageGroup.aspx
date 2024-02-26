<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageGroup.aspx.cs" Inherits="Group_ManageGroup" Title="Untitled Page" %>
<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" ID="_mainPanel">
        <cc:DropShadowPanel runat="server" ID="_mainPanelTitle" SkinID="ImageGallery-Title">
            Group Management
        </cc:DropShadowPanel>
        
        <div class="form-label">
            Associations 
        </div>
        <div class="form-field styledTable">
            <asp:Gridview id="_associationsGridView" runat="server" ShowHeader="false" AutoGenerateColumns="False" 
                DataSourceID="_associationsDataSource" DataKeyNames="BaseItemID">
                <Columns>
                    <asp:TemplateField> 
                        <ItemTemplate>
                            <asp:HyperLink ID="_baseItemHyperLink" runat="server" Text='<%# Bind("Title") %>'
                                NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem) Container.DataItem) %>' />                  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowDeleteButton="true" DeleteText="Remove" />
                </Columns>
                <EmptyDataTemplate>
                    (none)
                </EmptyDataTemplate>
            </asp:Gridview>
            <asp:ObjectDataSource runat="server" ID="_associationsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.BaseItemManager"
                SelectMethod="GetAssociatedBaseItemsForBaseItem" OnSelecting="_associationsDataSources_Action" 
                DeleteMethod="RemoveBaseItemAssociationFromBaseItem" OnDeleting="_associationsDataSources_Action">
                <SelectParameters>
                    <asp:Parameter Name="baseItem" Type="Object" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="baseItem" Type="Object" />
                    <asp:Parameter Name="isAssociatedBaseItemInstigator" Type="Boolean" DefaultValue="False" />
                </DeleteParameters>
            </asp:ObjectDataSource>
        </div>
        <asp:Panel runat="server" ID="_associationsParentsPanel">
            <div class="form-label">
                Associated with
            </div> 
            <div class="form-field styledTable">
                <asp:Gridview id="_associationsParentsGridView" runat="server" ShowHeader="false" AutoGenerateColumns="False"
                    DataSourceID="_associationsParentsDataSource" DataKeyNames="BaseItemID">
                    <Columns>
                        <asp:TemplateField> 
                            <ItemTemplate>
                                <asp:HyperLink ID="_baseItemHyperLink" runat="server" Text='<%# Bind("Title") %>'
                                    NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem) Container.DataItem) %>' />                  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="true" DeleteText="Remove" />
                    </Columns>
                    <EmptyDataTemplate>
                        (none)
                    </EmptyDataTemplate>
                </asp:Gridview>
                <asp:ObjectDataSource runat="server" ID="_associationsParentsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.BaseItemManager"
                    SelectMethod="GetBaseItemsAssociatedWithBaseItem" OnSelecting="_associationsParentsDataSource_Selecting"
                    DeleteMethod="RemoveAssociatedBaseItemFromBaseItem" OnDeleting="_associationsDataSources_Action">
                    <SelectParameters>
                        <asp:Parameter Name="baseItem" Type="Object" />
                    </SelectParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="baseItem" Type="Object" />
                        <asp:Parameter Name="isAssociatedBaseItemInstigator" Type="Boolean" DefaultValue="False" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </div>
        </asp:Panel>
        <div class="form-label">
            Members
        </div>
        <div class="form-field styledTable">
            <asp:GridView ID="_membersGridView" runat="server" ShowHeader="false" AutoGenerateColumns="False" AllowPaging="true"
                DataSourceID="_membersDataSource" EnableSortingAndPagingCallbacks="true" PageSize="30" DataKeyNames="BaseItemID">
                <Columns>
                    <asp:HyperLinkField DataTextField="Title" DataNavigateUrlFields="UserName" 
                        DataNavigateUrlFormatString="~/Friend/ViewProfile.aspx?userName={0}" />
                    <asp:CommandField ShowDeleteButton="true" DeleteText="Kick" />
                </Columns>
                <EmptyDataTemplate>
                    (none)
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:ObjectDataSource runat="server" ID="_membersDataSource" TypeName="WLQuickApps.SocialNetwork.WebSite.GroupManagementHelper"
                SelectMethod="GetAllUsersForGroup" OnSelecting="_membersDataSource_Selecting" EnablePaging="true"
                DeleteMethod="RemoveUserFromGroup" OnDeleting="_membersDataSource_Deleting" StartRowIndexParameterName="startRowIndex"
                MaximumRowsParameterName="maximumRows" SelectCountMethod="GetAllUsersForGroupCount">
                <SelectParameters>
                    <asp:Parameter Name="baseItemID" Type="Int32" />
                </SelectParameters>
                <DeleteParameters>
                    <asp:Parameter Name="Group" Type="Object" />
                </DeleteParameters>
            </asp:ObjectDataSource>
        </div>
        <div class="form-field">
            <br />
            <asp:Button runat="server" Text="Return" ID="_returnButton" OnClick="_returnButton_Click" />
        </div>
    </cc:DropShadowPanel>
</asp:Content>

