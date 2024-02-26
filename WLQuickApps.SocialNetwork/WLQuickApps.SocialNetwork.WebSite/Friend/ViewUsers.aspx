<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewUsers.aspx.cs" Inherits="Friend_ViewUsers" Title="View Users" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" ID="_friendsPanel">
        <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
            <asp:Label ID="_titleLabel" runat="server"></asp:Label>    
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_friendsGallery" runat="server" DataSourceID="_friendsDataSource" AllowPaging="true" ViewMode="User"
            EmptyDataText="No friends were found." PageSize="8" RepeatColumns="4" RepeatDirection="Horizontal" />
        
        <asp:ObjectDataSource runat="server" ID="_friendsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.FriendManager"
            SelectMethod="GetFriendsByUserID" SelectCountMethod="GetFriendsCountByUserID" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true">
        </asp:ObjectDataSource>
        
        <asp:ObjectDataSource runat="server" ID="_attendeesDataSource" TypeName="WLQuickApps.SocialNetwork.Business.UserManager"
            SelectMethod="GetAllUsersForGroup" SelectCountMethod="GetAllUsersForGroupCount" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true">
            <SelectParameters>
                <asp:QueryStringParameter Type="String" Name="groupID" QueryStringField="baseItemID" />
                <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
            </SelectParameters>
        </asp:ObjectDataSource>

    <asp:ObjectDataSource ID="_allUsersDataSource" runat="server" SelectMethod="GetMostRecentUsers"
        TypeName="WLQuickApps.SocialNetwork.Business.UserManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" SelectCountMethod="GetUserCount" />

    </cc:DropShadowPanel>
    
    <asp:HyperLink ID="_inviteLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Friend/Invite.aspx">Invite Friends &gt;&gt;</asp:HyperLink>
</asp:Content>

