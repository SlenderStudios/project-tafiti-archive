<%@ Page Language="C#" AutoEventWireup="true" Title="Unexpected error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <cc:DropShadowPanel runat="server" ID="_mainPanel">
        <h3>Unexpected error</h3>
        <hr />
        <p>
            We seem to have run into an unexpected error while serving you this page.  We apologize for the inconvenience, and ask that if you could,
            please send us <asp:HyperLink runat="server" NavigateUrl="~/Feedback.aspx" Text="feedback" /> to report to us what you were doing when this occurred.
        </p>
        <p>
            In the meantime, why not check out what's going on over on the
            <asp:HyperLink runat="server" ID="_homeLink" NavigateUrl="~/" Text="home" /> or
            <asp:HyperLink runat="server" ID="_latestLink" NavigateUrl="~/Landing.aspx" Text="latest activity" /> pages?
        </p>
    </cc:DropShadowPanel>
</asp:Content>