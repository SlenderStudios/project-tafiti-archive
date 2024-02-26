<%@ Control Language="C#" ClassName="EventsMenuControl" %>
<div id="eventsMenuHeader"></div>
<div id="eventsMenuContent">
    <script runat="server" >
        int n = 1;
    </script> 
    <asp:XmlDataSource ID="eventsDataSource" 
        runat="server"
        EnableCaching="false"
        DataFile="<%$ AppSettings:EventsFeed %>" 
        XPath="/rss/channel/item"></asp:XmlDataSource>
    <asp:DataList ID="latestNewsDataList" runat="server">
    <ItemTemplate>
        <div class="Title">
            <a href="javascript:showDiv($('contentPanel'), 400);updatePage('<%# XPath("title") %>','<%= eventsDataSource.DataFile %>', <%= n++%> ,'<%# HtmlProcessor.ExtractMapCid(XPath("description").ToString()) %>','<%# HtmlProcessor.ExtractPhotoAlbumFeed(XPath("description").ToString()) %>')">
                <%# XPath("title") %>
            </a>
        </div>
    </ItemTemplate>
</asp:DataList>
    <asp:Panel runat="server" ID="errorItem" Visible="false" CssClass="title">
        Error retrieving event data: <% =DataBindingErrorMsg %>
    </asp:Panel>
</div>
<div id="eventsMenuFooter"></div>

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