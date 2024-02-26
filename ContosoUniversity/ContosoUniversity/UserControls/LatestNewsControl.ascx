<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LatestNewsControl.ascx.cs" Inherits="ContosoUniversity.UserControls.LatestNewsControl" EnableViewState="false" %>
<%@ OutputCache Duration="300" VaryByParam="None" %>

<asp:XmlDataSource ID="latestNewsDataSource" 
    runat="server" 
    DataFile="<%$ AppSettings:LatestNewsFeed %>" 
    XPath="/rss/channel/item"
    EnableCaching="true">
</asp:XmlDataSource>

<asp:DataList ID="latestNewsDataList" runat="server">
    <ItemTemplate>
        <div id="NewsLink">
            <a href="#" onclick="window.open('NewsStory.aspx?feed=<%= latestNewsDataSource.DataFile %>&item=<%# XPath("guid")%>','story', 'width=920,scrollbars=yes')"><%# XPath("title") %></a>
            <img src="App_Themes/Default/images/arrow_off.gif" />
        </div>
    </ItemTemplate>
</asp:DataList>
<div id="ErrorDiv" runat="server" visible="false">
    ERROR: <%= errorMessage %>
</div>