<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddFriend.aspx.cs" Inherits="Friend_AddFriend" Title="Add Friend" %>
<%@ Register Src="../Controls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="_addFriendFormView" runat="server" DataSourceID="ObjectDataSource1">
        <ItemTemplate>
            <cc:DropShadowPanel runat="server" >
                <uc1:UserDetails id="UserDetails1" runat="server" UserItem='<%# (User)Container.DataItem %>' />
                <cc:DropShadowPanel runat="server" SkinID="AddFriend-ActionsBox" Visible='<%# ( UserManager.IsUserLoggedIn() ) && ( Eval("UserName", "{0}") != UserManager.LoggedInUser.UserName ) %>'>
                    <asp:LinkButton ID="_addFriendButton" runat="server" OnClick="_addFriendButton_Click" Text="Send Friend Request &gt;&gt;" />
                </cc:DropShadowPanel>
                <div class="clearFloats"></div>
            </cc:DropShadowPanel>
        </ItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetUserByUserName"
        TypeName="WLQuickApps.SocialNetwork.Business.UserManager">
        <SelectParameters>
            <asp:QueryStringParameter Name="userName" QueryStringField="userName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

