<%@ Control Language="C#" ClassName="RidesMenuControl" %>
<div id="ridesMenuHeader"></div>
<div id="ridesMenuContent">
    <script runat="server" >
        int n = 1;
    </script> 

    <asp:XmlDataSource ID="latestRidesDataSource" 
        runat="server" 
        EnableCaching="false"
        DataFile="<%$ AppSettings:LatestRidesFeed %>" 
        XPath="/rss/channel/item"></asp:XmlDataSource>
    <asp:DataList ID="latestRidesDataList" runat="server">
        <ItemTemplate>
            <div class="Title">
                <a href="javascript:showDiv($('contentPanel'), 400);updatePage('<%# XPath("title") %>','<%= latestRidesDataSource.DataFile %>', <%= n++ %> ,'<%# HtmlProcessor.ExtractMapCid(XPath("description").ToString()) %>','<%# HtmlProcessor.ExtractPhotoAlbumFeed(XPath("description").ToString()) %>')">
                    <%# XPath("title") %>
                </a>
            </div>
        </ItemTemplate>
    </asp:DataList>
    <asp:Panel runat="server" ID="errorItem" Visible="false" CssClass="Title">
        Error retrieving ride data: <% =DataBindingErrorMsg %>
    </asp:Panel>
</div>
<div id="ridesMenuFooter"></div>
<script language="C#" runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        latestRidesDataList.DataSource = latestRidesDataSource;
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
