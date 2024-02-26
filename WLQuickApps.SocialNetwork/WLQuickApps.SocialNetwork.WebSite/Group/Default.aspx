<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Group_Default" Title="Browse Groups" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
           <cc:DropShadowPanel runat="server" ID="_groupsPanel" >
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            Groups
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_groupsGallery" runat="server" DataSourceID="_groupsDataSource" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="12" RepeatColumns="4" ViewMode="Group" EmptyDataText="No groups of this type were found." />
    </cc:DropShadowPanel>
    <asp:ObjectDataSource ID="_groupsDataSource" runat="server" SelectMethod="SearchGroupsByName"
        TypeName="WLQuickApps.SocialNetwork.Business.GroupManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" SelectCountMethod="SearchGroupsByNameCount">
        <SelectParameters>
            <asp:Parameter Name="name" Type="string" DefaultValue="%" />
            <asp:Parameter Name="groupType" Type="string" DefaultValue="" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:DataList runat="server" ID="_specialGroups">
        <ItemTemplate>
            <cc:DropShadowPanel runat="server" ID="DropShadowPanel3">
                <cc:DropShadowPanel runat="server" ID="DropShadowPanel4" SkinID="ImageGallery-title">
                    <asp:Label runat="server" ID="_specialGroupLabel" Text='<%# (string) Container.DataItem %>' />s
                </cc:DropShadowPanel>
                <cc:MetaGallery ID="MetaGallery1" runat="server" DataSourceID="_repeatedGroupsDataSource" DataKeyField="BaseItemID"
                    AllowPaging="True" PageSize="12" RepeatColumns="4" ViewMode="List" EmptyDataText="No groups of this type were found." />
            </cc:DropShadowPanel>
                <asp:ObjectDataSource ID="_repeatedGroupsDataSource" runat="server" SelectMethod="SearchGroupsByName"
                    TypeName="WLQuickApps.SocialNetwork.Business.GroupManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" SelectCountMethod="SearchGroupsByNameCount">
                    <SelectParameters>
                        <asp:Parameter Name="name" Type="string" DefaultValue="%" />
                        <asp:ControlParameter Name="groupType" ControlID="_specialGroupLabel" PropertyName="Text" />
                    </SelectParameters>
                </asp:ObjectDataSource>
        </ItemTemplate>
    </asp:DataList>

    <asp:HyperLink ID="_createGroupLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Group/CreateGroup.aspx">Create Group &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_searchGroupsLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Group/SearchGroups.aspx">Search Groups &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_viewMyGroupsLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Group/ViewGroups.aspx">View My Groups &gt;&gt;</asp:HyperLink>
</asp:Content>

