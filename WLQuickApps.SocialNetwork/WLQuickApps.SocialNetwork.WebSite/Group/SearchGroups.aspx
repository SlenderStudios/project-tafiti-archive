<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchGroups.aspx.cs" Inherits="SearchGroups" Title="Untitled Page" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" >
        <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
            Search Groups by Name
        </cc:DropShadowPanel>
        <cc:SecureTextBox ID="_nameTextBox" runat="server"></cc:SecureTextBox>
        <asp:Button ID="_searchButton" runat="server" OnClick="_searchButton_Click" Text="Search" /><br />
        <br />
    </cc:DropShadowPanel>
        
    <cc:DropShadowPanel runat="server" ID="_groupsPanel" Visible='<%# GroupManager.SearchGroupsByNameCount(this.Request.QueryString[WebConstants.QueryVariables.SearchName], string.Empty) > 0 %>'>
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            Groups
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_groupsGallery" runat="server" DataSourceID="_groupsDataSource" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="12" RepeatColumns="3" ViewMode="List" EmptyDataText="No groups of this type were found." />
    </cc:DropShadowPanel>
    <asp:ObjectDataSource ID="_groupsDataSource" runat="server" SelectMethod="SearchGroupsByName"
        TypeName="WLQuickApps.SocialNetwork.Business.GroupManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" SelectCountMethod="SearchGroupsByNameCount">
        <SelectParameters>
            <asp:QueryStringParameter Name="name" QueryStringField="searchName" Type="String" />
            <asp:Parameter Name="groupType" Type="string" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:DataList runat="server" ID="_specialGroups" OnLoad="_specialGroups_Load">
        <ItemTemplate>
            <cc:DropShadowPanel runat="server" ID="DropShadowPanel1" Visible='<%# GroupManager.SearchGroupsByNameCount(this.Request.QueryString[WebConstants.QueryVariables.SearchName], (string) Container.DataItem) > 0 %>'>
                <cc:DropShadowPanel runat="server" ID="DropShadowPanel2" SkinID="ImageGallery-title">
                    <asp:Label runat="server" ID="_specialGroupLabel" Text='<%# (string) Container.DataItem %>' />s
                </cc:DropShadowPanel>
                <cc:MetaGallery ID="MetaGallery1" runat="server" DataSourceID="_repeatedGroupsDataSource" DataKeyField="BaseItemID"
                    AllowPaging="True" PageSize="12" RepeatColumns="3" ViewMode="List" EmptyDataText="No groups of this type were found." />
            </cc:DropShadowPanel>
                <asp:ObjectDataSource ID="_repeatedGroupsDataSource" runat="server" SelectMethod="SearchGroupsByName"
                    TypeName="WLQuickApps.SocialNetwork.Business.GroupManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" SelectCountMethod="SearchGroupsByNameCount">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="name" QueryStringField="searchName" Type="String" />
                        <asp:ControlParameter Name="groupType" ControlID="_specialGroupLabel" PropertyName="Text" />
                    </SelectParameters>
                </asp:ObjectDataSource>
        </ItemTemplate>
    </asp:DataList>
    
</asp:Content>

