<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Friend_Default" Title="Browse Users" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
           <cc:DropShadowPanel runat="server" ID="_usersPanel" >
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            Users
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_usersGallery" runat="server" DataSourceID="_usersDataSource" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="12" RepeatColumns="4" ViewMode="User" EmptyDataText="No users were found." />
    </cc:DropShadowPanel>
    <asp:ObjectDataSource ID="_usersDataSource" runat="server" SelectMethod="GetMostRecentUsers"
        TypeName="WLQuickApps.SocialNetwork.Business.UserManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" SelectCountMethod="GetUserCount" />

    <asp:HyperLink ID="_searchProfilesLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Friend/SearchProfiles.aspx">Search Profiles &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_inviteLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Friend/Invite.aspx">Invite People &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_viewMyFriendsLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Friend/ViewUsers.aspx">View My Friends &gt;&gt;</asp:HyperLink>

</asp:Content>

