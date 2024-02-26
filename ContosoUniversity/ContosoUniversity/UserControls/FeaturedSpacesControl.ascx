<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FeaturedSpacesControl.ascx.cs" Inherits="ContosoUniversity.UserControls.FeaturedSpacesControl" EnableViewState="false" %>
<%@ OutputCache Duration="300" VaryByParam="None" %>

<asp:XmlDataSource ID="featuredSpacesDataSource" 
    runat="server" 
    DataFile="<%$ AppSettings:FeaturedSpacesFeed %>" 
    XPath="/rss/channel"
    EnableCaching="true">
</asp:XmlDataSource>
 
<asp:Panel ID="Panel1" runat="server" >
    <asp:DataList ID="featuredSpacesDataList" runat="server" DataSourceID="featuredSpacesDataSource"  >
        <ItemTemplate>
            <a target="_blank" href="<%# HtmlProcessor.ExtractLink(XPath("item[1]/description").ToString()) %>">
                <img id="FeaturedSpace1" src= "<%# HtmlProcessor.ExtractImageUrl(XPath("item[1]/description").ToString()) %>" alt="<%# HtmlProcessor.RemoveTags(XPath("item[1]/description").ToString()) %>" />
            </a>
            <a target="_blank" href="<%# HtmlProcessor.ExtractLink(XPath("item[2]/description").ToString()) %>">
                <img id="FeaturedSpace2" src= "<%# HtmlProcessor.ExtractImageUrl(XPath("item[2]/description").ToString()) %>" alt="<%# HtmlProcessor.RemoveTags(XPath("item[2]/description").ToString()) %>" />
            </a>
            <a target="_blank" href="<%# HtmlProcessor.ExtractLink(XPath("item[3]/description").ToString()) %>">
                <img id="FeaturedSpace3" src= "<%# HtmlProcessor.ExtractImageUrl(XPath("item[3]/description").ToString()) %>" alt="<%# HtmlProcessor.RemoveTags(XPath("item[3]/description").ToString()) %>" />
            </a>
            <a target="_blank" href="<%# HtmlProcessor.ExtractLink(XPath("item[4]/description").ToString()) %>">
                <img id="FeaturedSpace4" src= "<%# HtmlProcessor.ExtractImageUrl(XPath("item[4]/description").ToString()) %>" alt="<%# HtmlProcessor.RemoveTags(XPath("item[4]/description").ToString()) %>" />
            </a>
        </ItemTemplate>
    </asp:DataList>
</asp:Panel>
   
<asp:Panel ID="Panel2" runat="server">
    <asp:DataList ID="DataList1" runat="server" DataSourceID="featuredSpacesDataSource"  >
        <ItemTemplate>
            <%# HtmlProcessor.RemoveTags(XPath("item[1]/description").ToString()) %>
            <br /><br /><a target="_blank" href="<%# HtmlProcessor.ExtractLink(XPath("item[1]/description").ToString()) %>"><%# HtmlProcessor.ExtractLink(XPath("item[1]/description").ToString()) %></a>
            <br /><br /><%# HtmlProcessor.RemoveTags(XPath("item[2]/description").ToString()) %>
            <br /><br /><a target="_blank" href="<%# HtmlProcessor.ExtractLink(XPath("item[2]/description").ToString()) %>"><%# HtmlProcessor.ExtractLink(XPath("item[2]/description").ToString()) %></a>
            <br /><br /><%# HtmlProcessor.RemoveTags(XPath("item[3]/description").ToString()) %>
            <br /><br /><a target="_blank" href="<%# HtmlProcessor.ExtractLink(XPath("item[3]/description").ToString()) %>"><%# HtmlProcessor.ExtractLink(XPath("item[3]/description").ToString()) %></a>
            <br /><br /><%# HtmlProcessor.RemoveTags(XPath("item[4]/description").ToString()) %>
            <br /><br /><a target="_blank" href="<%# HtmlProcessor.ExtractLink(XPath("item[4]/description").ToString()) %>"><%# HtmlProcessor.ExtractLink(XPath("item[4]/description").ToString()) %></a>
        </ItemTemplate>
    </asp:DataList>
</asp:Panel>
