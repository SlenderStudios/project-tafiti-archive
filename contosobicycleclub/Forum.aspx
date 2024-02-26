<%@ Page Language="C#" 
    theme=""%>
<asp:XmlDataSource ID="latestNewsDataSource" 
    runat="server" 
    DataFile="<%$ AppSettings:ForumFeed %>" 
    XPath="/rss/channel/item"></asp:XmlDataSource>
<h1>Forum</h1>
<asp:DataList ID="latestNewsDataList" runat="server" DataSourceID="latestNewsDataSource">
    <ItemTemplate>
 
        <h2><%# XPath("title") %> </h2>

        <%# XPath("description") %> 

        <hr />
    </ItemTemplate>
</asp:DataList>  
