<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WLQuickApps.ContosoBicycleClub.Rides.Default" %>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeadingPlaceHolder" runat="server" />

<asp:Content ID="Content2" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">

	<asp:HyperLink ID="RidesFeedLink" CssClass="cbc-RSS cbc-EventsRSS" Text="<%$ Resources:ContosoBicycleClubWeb, RidesRSSLabel %>" Tooltip="<%$ Resources:ContosoBicycleClubWeb, RidesRSSTooltip %>" runat="server" />

	<div class="cbc-LatestEvents">
		<h2><asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, LatestRidesLabel %>" /></h2>

		<asp:Repeater ID="PastRidesRepeater" runat="server" >
			<HeaderTemplate><ul></HeaderTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
	</div>

	<div class="cbc-UpcomingEvents">
		<h2><asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, UpcomingRidesLabel %>" /></h2>

		<asp:Repeater ID="UpcomingRidesRepeater" runat="server">
			<HeaderTemplate><ul></HeaderTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
	</div>
</asp:Content>
