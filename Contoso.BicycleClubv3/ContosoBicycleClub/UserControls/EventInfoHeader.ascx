<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventInfoHeader.ascx.cs" Inherits="WLQuickApps.ContosoBicycleClub.UserControls.EventInfoHeader" %>
<div class="vevent cbc-PageHeading">
	<h1 runat=server id="TitleHeading"><%= Title %></h1>

	<div class="floatLeft">
		<abbr class="dtend" title="<%= string.Format("{0:yyyy-MM-dd}", EventDate)%>"><%= string.Format("{0:MMMM dd, yyyy}", EventDate)%></abbr>
		<span class="separator">&#160;|&#160;</span>
		<asp:Localize runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, OrganizedByLabel %>" /><span class="vcard"><span class="fn nickname" id="cbc-EventOwner"><%= OwnerName %></span></span>
		
		<asp:LoginView runat="server">
			<LoggedInTemplate>
				<script type="text/javascript"
					src="http://messenger.services.live.com/users/<%= OwnerId %>/presence/?cb=ShowOwnerPresence">
				</script>
			</LoggedInTemplate>
		</asp:LoginView>
	</div>
	
	<asp:HyperLink ID="BackHyperLink" runat="server" CssClass="back" />
</div>