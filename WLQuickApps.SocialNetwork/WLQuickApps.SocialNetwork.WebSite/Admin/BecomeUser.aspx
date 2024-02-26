<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BecomeUser.aspx.cs"
    Inherits="Friend_RemoveFriend" Title="Become User" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
Who do you want to be today?<br />
    <cc:SecureTextBox ID="_userNameTextBox" runat="server" />
    <asp:Label ID="_userNameLabel" runat="server" />
    <asp:Button ID="_becomeButton" runat="server" OnClick="_becomeButton_Click" Text="Become" UseSubmitBehavior="true" />
</asp:Content>
