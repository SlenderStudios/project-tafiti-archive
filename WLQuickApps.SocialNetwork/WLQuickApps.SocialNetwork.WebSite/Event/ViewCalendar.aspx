<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewCalendar.aspx.cs" Inherits="Event_ViewCalendar" Title="Untitled Page" %>

<%@ Register Src="../Controls/UserCalendar.ascx" TagName="UserCalendar" TagPrefix="uc" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <uc:UserCalendar ID="_calendar" runat="server" DisplayMode="Full" OnPreRender="_calendar_PreRender" />
    
    <cc:DropShadowPanel runat="server" ID="_noEvents" Visible="False">
        You aren't registered for any events.
    </cc:DropShadowPanel>
    <asp:HyperLink ID="_createEventLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Event/AddEvent.aspx">Create Event &gt;&gt;</asp:HyperLink>
</asp:Content>

