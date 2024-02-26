<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateGroup.aspx.cs" Inherits="CreateGroup" Title="Untitled Page" %>
<%@ Register Src="../Controls/GroupForm.ascx" TagName="AddGroupForm" TagPrefix="uc1" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" ID="_mainPanel">
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            Group Details
        </cc:DropShadowPanel>
        <uc1:AddGroupForm runat="server" ID="_addGroupForm" OnSave="_addGroupForm_Save" IsEvent="false" />
    </cc:DropShadowPanel> 
</asp:Content>

