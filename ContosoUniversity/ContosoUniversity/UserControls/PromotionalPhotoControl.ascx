<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PromotionalPhotoControl.ascx.cs" Inherits="ContosoUniversity.UserControls.PromotionalPhotoControl" EnableViewState="false" %>
<%@ OutputCache Duration="300" VaryByParam="None" %>

<asp:XmlDataSource ID="promotionalPhotoDataSource" 
    runat="server"
    DataFile="<%$ AppSettings:PromotionalPhotoFeed %>" 
    XPath="/rss/channel/item[1]"
    EnableCaching="true">
</asp:XmlDataSource>
    
<asp:DataList ID="promotionalPhotoDataList" runat="server">
    <ItemTemplate>
        <img id="PromotionalPhoto" width="160" height="210" src= "<%# XPath("enclosure/@url") %>" alt="<%# XPath("title") %>" /> 
    </ItemTemplate>
</asp:DataList>
<div id="ErrorDiv" runat="server" visible="false">
    ERROR: <%= errorMessage %>
</div>