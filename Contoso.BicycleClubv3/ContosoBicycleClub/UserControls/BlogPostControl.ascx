<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlogPostControl.ascx.cs" Inherits="WLQuickApps.ContosoBicycleClub.UserControls.BlogPostControl" %>
<asp:Panel ID="PostPanel" CssClass="cbc-EventPost" runat="server">
	<h2 runat="server" id="TitleLabel">Ride Description</h2>
	<asp:Label runat="server" ID="DescriptionLabel"></asp:Label>
</asp:Panel>