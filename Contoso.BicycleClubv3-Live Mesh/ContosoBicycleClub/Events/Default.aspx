<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WLQuickApps.ContosoBicycleClub.Events.Default" %>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeadingPlaceHolder" runat="server" />

<asp:Content ID="Content2" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">

	<asp:HyperLink ID="EventsFeedLink" CssClass="cbc-RSS cbc-EventsRSS" Text="<%$ Resources:ContosoBicycleClubWeb, EventsRSSLabel %>" Tooltip="<%$ Resources:ContosoBicycleClubWeb, EventsRSSTooltip %>" runat="server" />

	<div class="cbc-LatestEvents">
		<h2><asp:Localize runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, LatestEventsLabel %>" /></h2>
		<asp:Repeater ID="PastEventsRepeater" runat="server">
			<HeaderTemplate><ul></HeaderTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
	</div>

	<div class="cbc-UpcomingEvents">
		<h2><asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, UpcomingEventsLabel %>" /></h2>

		<asp:Repeater ID="UpcomingEventsRepeater" runat="server">
			<HeaderTemplate><ul></HeaderTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
	</div>
        
</asp:Content>
