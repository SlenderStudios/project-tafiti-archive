<%@ Page Language="C#" AutoEventWireup="true"  Inherits="Item" Theme="Default"%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>News Story</title>
</head>
<body class="NewsStoryBody">
    <asp:XmlDataSource ID="itemDataSource" runat="server" EnableCaching="true" />
    <asp:DataList ID="itemDataList" runat="server" DataSourceID="itemDataSource"  >
        <ItemTemplate>
            <table border=0>
                <tr>
                    <td><div id="NewsStoryHeadline"><%# XPath("title") %></div></td>
                </tr>
                <tr>
                    <td><div id="NewsStoryPublishDate"><%# XPath("pubDate") %></div></td>
                </tr>
                <tr>
                    <td><div id="NewsStoryText"><%# XPath("description") %></td>
                </tr>
                <tr>
                    <td>                
                        <table>
                            <tr>
                                <td><a href="mailto:?subject=<%# XPath("title") %>&body=<%# XPath("guid") %>"><img border=0 src="App_Themes/Default/images/mail_icon.gif" /></a></td>
                                <td class="external">Send to a friend&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><a target=_blank href="http://del.icio.us/post"><img border=0 src="App_Themes/Default/images/delicious.gif" /></a></td>
                                <td class="external">del.ici.ous&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><a target=_blank href="http://digg.com/submit?phase=2&url=<%# XPath("guid") %>&title=<%# XPath("title") %>"><img border=0 src="App_Themes/Default/images/digg-guy-icon.gif" /></a></td>
                                <td class="external">Digg&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td><a target=_blank href="http://favorites.live.com/quickadd.aspx?top=1&url=<%# XPath("guid") %>&title=<%# XPath("title") %>"><img border=0 src="App_Themes/Default/images/fav.gif" /></a></td>
                                <td class="external">Live Favorites</td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
            
            
            </div>
        </ItemTemplate>
    </asp:DataList>
</body>
</html>