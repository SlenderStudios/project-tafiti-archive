<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NewsControl.ascx.cs" Inherits="Controls_NewsControl" %>

<asp:ListView ID="NewsList" ItemPlaceholderID="NewsItemPlaceHolder" runat="server">
    <LayoutTemplate>
        <ul>
            <asp:PlaceHolder ID="NewsItemPlaceHolder" runat="server" />
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li>
            <h3><a href='<%# Eval("Link") %>'><%# Eval("Title")%></a></h3>
            <asp:ListView ID="FeedList" ItemPlaceholderID="FeedItemPlaceHolder" DataSource='<%# Eval("Items") %>' runat="server">
                <LayoutTemplate>
                    <ul class="news-list-items">
                        <asp:PlaceHolder ID="FeedItemPlaceHolder" runat="server" />
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li>
                        <h4><a href='<%# Eval("Link") %>'><%# Eval("Title")%></a></h4>
                        <div><span><%# Eval("PublishDate") %></span></div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
        </li>
    </ItemTemplate>
</asp:ListView>
