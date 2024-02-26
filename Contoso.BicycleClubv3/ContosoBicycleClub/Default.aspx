<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WLQuickApps.ContosoBicycleClub.Default1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContainerClassPlaceHolder" runat="server">cbc-HomePage</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeadingPlaceHolder" runat="server" />

<asp:Content ID="Content4" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
	<div class="cbc-AboutContoso">
		<asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, AboutContosoLabel %>" />
	</div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">
	<div class="cbc-LatestEvents">
		<h2><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="rides/default.aspx" Text="<%$ Resources:ContosoBicycleClubWeb, LatestRidesLabel %>" /></h2>
		
		<asp:ObjectDataSource 
			ID="PastRidesDataSource" 
			TypeName="WLQuickApps.ContosoBicycleClub.Business.RideManager" 
			SelectMethod="GetPastRidesByPage" runat="server">
			<SelectParameters>
				<asp:Parameter Name="startRow" Direction="Input" Type="Int32" DefaultValue="0" />
				<asp:Parameter Name="pageSize" Direction="Input" Type="Int32" DefaultValue="3" />
			</SelectParameters>
		</asp:ObjectDataSource>

		<asp:Repeater ID="PastRidesRepeater" runat="server" DataSourceID="PastRidesDataSource">
			<HeaderTemplate><ul></HeaderTemplate>
			<FooterTemplate></ul></FooterTemplate>
		</asp:Repeater>
		
		<asp:HyperLink ID="HyperLink6" CssClass="more" runat="server" NavigateUrl="rides/default.aspx" Text="<%$ Resources:ContosoBicycleClubWeb, MoreRidesLabel %>" />
	</div>
        
	<div class="cbc-UpcomingEvents">
		<div class="column">
			<h2><asp:HyperLink runat="server" NavigateUrl="rides/default.aspx" Text="<%$ Resources:ContosoBicycleClubWeb, UpcomingRidesLabel %>" /></h2>

			<asp:ObjectDataSource 
				ID="UpcomingRidesDataSource" 
				TypeName="WLQuickApps.ContosoBicycleClub.Business.RideManager" 
				SelectMethod="GetUpcomingRidesByPage" runat="server">
				<SelectParameters>
					<asp:Parameter Name="startRow" Direction="Input" Type="Int32" DefaultValue="0" />
					<asp:Parameter Name="pageSize" Direction="Input" Type="Int32" DefaultValue="3" />
				</SelectParameters>
			</asp:ObjectDataSource>

			<asp:Repeater ID="UpcomingRidesRepeater" runat="server" DataSourceID="UpcomingRidesDataSource">
				<HeaderTemplate><ul></HeaderTemplate>
				<FooterTemplate></ul></FooterTemplate>
			</asp:Repeater>

			<asp:HyperLink ID="HyperLink4" CssClass="more" runat="server" NavigateUrl="rides/default.aspx" Text="<%$ Resources:ContosoBicycleClubWeb, MoreRidesLabel %>" />
		</div>

		<div class="column">
			<h2><asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="events/default.aspx" Text="<%$ Resources:ContosoBicycleClubWeb, UpcomingEventsLabel %>" /></h2>

			<asp:ObjectDataSource 
				ID="UpcomingEventsDataSource" 
				TypeName="WLQuickApps.ContosoBicycleClub.Business.RideManager" 
				SelectMethod="GetUpcomingEventsByPage" runat="server">
				<SelectParameters>
					<asp:Parameter Name="startRow" Direction="Input" Type="Int32" DefaultValue="0" />
					<asp:Parameter Name="pageSize" Direction="Input" Type="Int32" DefaultValue="3" />
				</SelectParameters>
			</asp:ObjectDataSource>

			<asp:Repeater ID="UpcomingEventsRepeater" runat="server" DataSourceID="UpcomingEventsDataSource">
				<HeaderTemplate><ul></HeaderTemplate>
				<FooterTemplate></ul></FooterTemplate>
			</asp:Repeater>

			<asp:HyperLink ID="HyperLink5" CssClass="more" runat="server" NavigateUrl="events/default.aspx" Text="<%$ Resources:ContosoBicycleClubWeb, MoreEventsLabel %>" />
		</div>
	</div>
</asp:Content>
