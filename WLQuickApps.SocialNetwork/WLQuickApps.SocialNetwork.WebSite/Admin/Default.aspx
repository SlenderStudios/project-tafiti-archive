<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" Title="Admin Links" %>

<asp:content id="_content" contentplaceholderid="MainContent" runat="server">
    <cc:DropShadowPanel ID="DropShadowPanel1" runat="server" >
         <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ImageGallery-title">
            Admin Links
        </cc:DropShadowPanel>
        <asp:hyperlink id="_approveHyperLink" runat="server" navigateurl="~/Admin/ApproveItems.aspx">Approve Items</asp:hyperlink><br />
        <asp:hyperlink id="_becomeUserHyperLink" runat="server" navigateurl="~/Admin/BecomeUser.aspx">Become User</asp:hyperlink><br />
        <asp:hyperlink id="_monitorLogHyperLink" runat="server" navigateurl="~/Admin/MonitorWebLog.aspx">Monitor Web Log</asp:hyperlink><br />
        <asp:hyperlink id="_unbecomeUserHyperLink" runat="server" navigateurl="~/Admin/UnbecomeUser.aspx">Unbecome User</asp:hyperlink><br />
        <asp:hyperlink id="_statisticsHyperLink" runat="server" navigateurl="~/Admin/Statistics.aspx">View Statistics</asp:hyperlink><br />
    </cc:DropShadowPanel>
</asp:content>
