<%@ Page 
    Language="C#" 
    Theme="" %>

<%@ OutputCache 
    Duration="60" 
    VaryByParam="*" %>

<asp:XmlDataSource ID="eventsDataSource" 
    runat="server" 
    EnableCaching="false"
    DataFile="<%$ AppSettings:EventsFeed %>" 
    XPath="/rss/channel/item"></asp:XmlDataSource>

<h1>Events</h1>
<asp:DataList ID="eventsDataList" runat="server">
    <ItemTemplate>
        <h2><%# XPath("title") %> </h2>
        <%# XPath("description") %> 
        <hr />
    </ItemTemplate>
</asp:DataList>  
<asp:Panel id="errorItem" runat="server" visible="false">
<h2>Error retrieving event data<</h2>
<% =DataBindingErrorMsg %>
<hr />
</asp:Panel>

<script language="C#" runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        latestNewsDataList.DataSource = eventsDataSource;
        try
        {
            DataBind();
        }
        catch (Exception ex)
        {
            // failure to bind the data, enable the error row and show the underlying cause
            errorItem.Visible = true;
            DataBindingErrorMsg = ex.Message;
        }
    }
    string DataBindingErrorMsg;
</script>