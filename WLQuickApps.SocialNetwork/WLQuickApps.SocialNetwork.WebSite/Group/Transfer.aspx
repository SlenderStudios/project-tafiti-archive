<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Transfer.aspx.cs" Inherits="Group_Transfer" Title="Transfer Ownership" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" Runat="Server">
    <cc:DropShadowPanel runat="server" ID="_mainPanel">
        <cc:DropShadowPanel runat="server" ID="_mainPanelTitle" SkinID="ImageGallery-title">
            Transfer to...
        </cc:DropShadowPanel>
        
        <asp:Label runat="server" ID="_noPeersLabel" Text="There is no one in this group for you to transfer ownership to." Visible="false" />
        <asp:HyperLink runat="server" ID="_noPeersLink" />
        
        <asp:RadioButtonList runat="server" ID="_groupMembersList" RepeatColumns="2" DataTextField="DisplayName" DataValueField="UserName" />
        <asp:Button runat="server" ID="_chooseMember" OnClick="_chooseMember_Click" Text="Choose" />
    </cc:DropShadowPanel>
</asp:Content>

