<%@ Page Language="C#" AutoEventWireup="true" Title="Page not found" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <cc:DropShadowPanel runat="server" ID="_mainPanel">
        <h3>Page not found</h3>
        <hr />
        <p>
            Sorry, but the page you requested could not be found.  To find what you are looking for, start with either our
            <asp:HyperLink runat="server" ID="_homeLink" NavigateUrl="~/" Text="home" /> or
            <asp:HyperLink runat="server" ID="_latestLink" NavigateUrl="~/Landing.aspx" Text="latest activity" /> pages.
        </p>
    </cc:DropShadowPanel>
</asp:Content>

