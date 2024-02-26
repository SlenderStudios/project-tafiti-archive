<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditGroup.aspx.cs" Inherits="Group_EditGroup" Title="Untitled Page" %>
<%@ Register Src="../Controls/GroupForm.ascx" TagName="AddGroupForm" TagPrefix="uc1" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    <cc:DropShadowPanel runat="server" ID="_mainPanel">
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            Group Details
        </cc:DropShadowPanel>
        <uc1:AddGroupForm runat="server" ID="_editGroupForm" OnSave="_editGroupForm_Save" IsEvent="false" />
    </cc:DropShadowPanel>
</asp:Content>

