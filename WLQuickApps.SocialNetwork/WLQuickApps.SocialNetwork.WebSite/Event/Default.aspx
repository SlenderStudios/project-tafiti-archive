<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Event_Default" Title="Browse Events" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
           <cc:DropShadowPanel runat="server" ID="_eventsPanel" >
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            Events
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_eventsGallery" runat="server" DataSourceID="_eventsDataSource" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="12" RepeatColumns="4" ViewMode="Event" EmptyDataText="No events were found." />
    </cc:DropShadowPanel>
    <asp:ObjectDataSource ID="_eventsDataSource" runat="server" SelectMethod="GetAllEventsInSystem"
        TypeName="WLQuickApps.SocialNetwork.Business.EventManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" SelectCountMethod="GetAllEventsInSystemCount" />

    <asp:HyperLink ID="_createEventLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Event/AddEvent.aspx">Create Event &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_searchEventsLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Event/SearchEvents.aspx">Search Events &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_viewMyEventsLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Event/ViewCalendar.aspx">View My Calendar &gt;&gt;</asp:HyperLink>

</asp:Content>

