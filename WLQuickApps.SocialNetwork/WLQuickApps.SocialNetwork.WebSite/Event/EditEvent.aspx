<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditEvent.aspx.cs" Inherits="Event_EditEvent" Title="Edit Event" %>
<%@ Register Src="../Controls/GroupForm.ascx" TagName="AddGroupForm" TagPrefix="uc1" %>

<asp:Content runat="server" ID="Content" ContentPlaceHolderID="MainContent">
    <br />
    <cc:DropShadowPanel runat="server" ID="_mainPanel">
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            Event Details
        </cc:DropShadowPanel>
        <uc1:AddGroupForm runat="server" ID="_editEventForm" IsEditing="true" OnSave="_editEventForm_Save" />
    </cc:DropShadowPanel> 
</asp:Content>
