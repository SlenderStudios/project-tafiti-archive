<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventsControl.ascx.cs" Inherits="ContosoUniversity.UserControls.EventsControl" EnableViewState="false" %>
<%@ OutputCache Duration="300" VaryByParam="None" %>

<asp:XmlDataSource ID="eventsDataSource" 
    runat="server" 
    DataFile="<%$ AppSettings:EventsFeed %>" 
    XPath="/rss/channel/item"
    EnableCaching="false">
</asp:XmlDataSource>

<asp:DataList ID="EventsDataList" runat="server">
    <ItemTemplate>
        <%# XPath("title") %> 
    </ItemTemplate>
</asp:DataList>
<div runat="server" id="ErrorDiv" visible="false">Failed to load events: <%=ErrorMessage %></div>
