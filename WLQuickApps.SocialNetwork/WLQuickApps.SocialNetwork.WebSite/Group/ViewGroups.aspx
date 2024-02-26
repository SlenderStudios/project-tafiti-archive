<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewGroups.aspx.cs" Inherits="Group" Title="View Groups" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" ID="_eventsPanel" >
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
             <asp:Label ID="_titleLabel" runat="server"></asp:Label>
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_gallery" runat="server" DataSourceID="_dataSource" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="8" RepeatColumns="4" ViewMode="Group" EmptyDataText="No groups were found." />
    </cc:DropShadowPanel>
    <asp:ObjectDataSource runat="server" ID="_dataSource" TypeName="WLQuickApps.SocialNetwork.Business.GroupManager"
        SelectMethod="GetGroupsForUser" SelectCountMethod="GetGroupsForUserCount" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" EnablePaging="true">
        <SelectParameters>
            <asp:QueryStringParameter Name="userName" QueryStringField="userName" Type="String" />
            <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:HyperLink ID="_createGroupLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Group/CreateGroup.aspx">Create Group &gt;&gt;</asp:HyperLink>

</asp:Content>

