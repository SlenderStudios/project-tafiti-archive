<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddEvent.aspx.cs" Inherits="Event_AddEvent" Title="Create Event" %>
<%@ Register Src="../Controls/GroupForm.ascx" TagName="AddGroupForm" TagPrefix="uc1" %>

<asp:Content ID="_content" runat="server" ContentPlaceHolderID="MainContent">
    <cc:DropShadowPanel runat="server" ID="_mainPanel">
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            Event Details
        </cc:DropShadowPanel>
        <uc1:AddGroupForm runat="server" ID="_addEventForm" OnSave="_addEventForm_Save" />
    </cc:DropShadowPanel> 
</asp:Content>
