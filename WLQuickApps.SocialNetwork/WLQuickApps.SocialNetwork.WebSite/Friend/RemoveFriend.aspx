<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RemoveFriend.aspx.cs"
    Inherits="Friend_RemoveFriend" Title="Remove Friend" %>
<%@ Register Src="../Controls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="_removeFriendFormView" runat="server" DataSourceID="ObjectDataSource1">
        <ItemTemplate>
            <cc:DropShadowPanel ID="Panel1" runat="server" >
                <uc1:UserDetails id="UserDetails1" runat="server" UserItem='<%# (User)Container.DataItem %>' />
                <cc:DropShadowPanel ID="Panel6" runat="server" SkinID="RemoveFriend-ActionsBox" Visible='<%# ( UserManager.IsUserLoggedIn() ) && ( Eval("UserName", "{0}") != UserManager.LoggedInUser.UserName ) %>'>
                    <asp:LinkButton ID="_removeFriendButton" runat="server" OnClick="_removeFriendButton_Click"
                        Text="Confirm Friend Removal &gt;&gt;" />
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
