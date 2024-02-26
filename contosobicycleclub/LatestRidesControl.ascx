<%@ Control 
    Language="C#" 
    ClassName="LatestRidesControl" %>
<div id="latestRidesContent">
    <script runat="server" >
    int n = 1;
    </script> 
    <!-- setup the XML Datasource - this will query the RSS feed directly -->
    <asp:XmlDataSource ID="latestRidesDataSource" 
        runat="server" 
        EnableCaching="false"
        DataFile="<%$ AppSettings:LatestRidesFeed %>" 
        XPath="/rss/channel/item"></asp:XmlDataSource>
    <table>
        <asp:DataList ID="latestRidesDataList" runat="server">
            <ItemTemplate>
                <tr class="latestRideItem">
                    <td valign="top" class="left">
                        <a href="javascript:showDiv($('contentPanel'), 100);updatePage('<%# XPath("title") %>','<%= latestRidesDataSource.DataFile %>', <%= n%> ,'<%# HtmlProcessor.ExtractMapCid(XPath("description").ToString()) %>','<%# HtmlProcessor.ExtractPhotoAlbumFeed(XPath("description").ToString()) %>')">
                            <img width="50" height="50" alt="<%# XPath("title") %>" src="<%# HtmlProcessor.ExtractImageUrl(XPath("description").ToString()) %>" />
                        </a>
                    </td>
                    <td class="right">
                        <div class="title">
                            <a href="javascript:showDiv($('contentPanel'), 100);updatePage('<%# XPath("title") %>','<%= latestRidesDataSource.DataFile %>', <%= n++ %> ,'<%# HtmlProcessor.ExtractMapCid(XPath("description").ToString()) %>','<%# HtmlProcessor.ExtractPhotoAlbumFeed(XPath("description").ToString()) %>')">
                                <%# XPath("title") %>
                            </a>
                        </div>
                        <div class="description"><%# HtmlProcessor.FirstLine(XPath("description").ToString()) %></div>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:DataList>
        <tr id="errorRow" runat="server" class="latestRideItem" visible="false" enableviewstate="false" >
                    <td valign="top" class="left">
                        
                    </td>
                    <td class="right">
                        <div class="title">Error retrieving ride data</div>
                        <div class="description"><% =DataBindingErrorMsg %></div>
                    </td>
        </tr>
    </table>
</div>

<script runat="server">
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
            errorRow.Visible = true;
            DataBindingErrorMsg = ex.Message;
        }
    }

    string DataBindingErrorMsg;
</script>
