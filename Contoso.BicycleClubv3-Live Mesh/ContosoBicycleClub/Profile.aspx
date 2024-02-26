<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs"
	Inherits="WLQuickApps.ContosoBicycleClub.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ChatUsersPlaceHolder" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">
	<fieldset class="cbc-Form-Fieldset">
		<div class="cbc-Form-Buttons">
			<asp:Button CssClass="cbc-Form-Button" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, ProfileLabel %>" ID="ProfileButton"
				OnClick="ProfileButton_Click" />
		</div>
		<div>
		    <br />
		</div>
		<div class="cbc-Form-LabelValue">
			<asp:Label AssociatedControlID="DisplayNameTextBox" ID="Label5" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, ContosoUsernameLabel %>" /><br />
			<asp:TextBox CssClass="cbc-Form-Input-Long" ID="DisplayNameTextBox" runat="server" />
		</div>
		<div class="cbc-Form-LabelValue">
			<asp:Label AssociatedControlID="FirstNameTextBox" ID="Label1" runat="server" Text="First Name" /><br />
			<asp:TextBox CssClass="cbc-Form-Input-Long" ID="FirstNameTextBox" runat="server" />
		</div>
		<div class="cbc-Form-LabelValue">
			<asp:Label AssociatedControlID="LastNameTextBox" ID="Label2" runat="server" Text="Last Name" /><br />
			<asp:TextBox CssClass="cbc-Form-Input-Long" ID="LastNameTextBox" runat="server" />
		</div>
		<div class="cbc-Form-LabelValue">
			<asp:Label AssociatedControlID="EmailTextBox" ID="Label4" runat="server" Text="Email" /><br />
			<asp:TextBox CssClass="cbc-Form-Input-Long" ID="EmailTextBox" runat="server" />
		</div>
		<div class="cbc-Form-LabelValue">
			<asp:Label AssociatedControlID="AddressTextBox" ID="Label3" runat="server" Text="Address" /><br />
			<asp:TextBox CssClass="cbc-Form-Input-Long" ID="AddressTextBox" runat="server" /><br />
			<asp:Label AssociatedControlID="CityTextBox" ID="Label6" runat="server" Text="City" /><br />
			<asp:TextBox CssClass="cbc-Form-Input-Long" ID="CityTextBox" runat="server" /><br />
			<asp:Label AssociatedControlID="StateTextBox" ID="Label7" runat="server" Text="State" /><br />
			<asp:TextBox CssClass="cbc-Form-Input-Long" ID="StateTextBox" runat="server" /><br />
			<asp:Label AssociatedControlID="PostalCodeTextBox" ID="Label8" runat="server" Text="Postal Code" /><br />
			<asp:TextBox CssClass="cbc-Form-Input-Long" ID="PostalCodeTextBox" runat="server" /><br />
		</div>
		<div class="cbc-Form-LabelValue">
			<cbc:ShareOnlinePresenceLink runat="server" ID="ShareOnlinePresenceLink1" />
			<p class="cbc-Form-Notes">
				<asp:Localize runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, SharePresenceLabel %>" /></p>
		</div>
		<div class="cbc-Form-LabelValue">
			<asp:Label CssClass="cbc-Form-Input-Long" ID="LiveMessengerIDLabel" runat="server" ReadOnly="true" />
		</div>
		<div class="cbc-Form-Buttons">
			<asp:Button CssClass="cbc-Form-Button" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, SaveLabel %>" ID="SaveButton"
				OnClick="SaveButton_Click" />
			<asp:Button CssClass="cbc-Form-Button" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, CancelLabel %>" ID="CancelButton"
				OnClick="CancelButton_Click" /><br />
			<asp:Button CssClass="cbc-Form-Button" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, InviteLabel %>" ID="InviteButton"
				OnClick="InviteButton_Click" />
		</div>
	</fieldset>
</asp:Content>
