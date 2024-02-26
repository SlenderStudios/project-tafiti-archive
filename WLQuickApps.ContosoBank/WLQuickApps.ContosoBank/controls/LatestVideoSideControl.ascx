<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="LatestVideoSideControl.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.LatestVideoSideControl" %>
<div class="CommandButton"><asp:HyperLink ID="LatestVidoesButton" runat="server"  Text='Latest Videos' NavigateUrl="~/Videos.aspx" ></asp:HyperLink></div>
<div class="VideoSideImageWrapper">
<asp:HyperLink ID="LatestVideosImageLink" NavigateUrl="~/Videos.aspx" runat="server"><asp:Image ID="LatestVideosImage" runat="server" CssClass="VideoSideImage" /></asp:HyperLink>
</div>
