<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs"
	Inherits="WLQuickApps.ContosoBicycleClub.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ChatUsersPlaceHolder" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">
	<fieldset class="cbc-Form-Fieldset">
		<asp:TextBox CssClass="cbc-Form-Input-Long" ID="FirstNameTextBox" runat="server" Visible="false" />
		<asp:TextBox CssClass="cbc-Form-Input-Long" ID="LastNameTextBox" runat="server" Visible="false" />
		<asp:TextBox CssClass="cbc-Form-Input-Long" ID="EmailTextBox" runat="server" Visible="false" />

		<div class="cbc-Form-LabelValue">
			<asp:Label AssociatedControlID="DisplayNameTextBox" ID="Label5" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, ContosoUsernameLabel %>" /><br />
			<asp:TextBox CssClass="cbc-Form-Input-Long" ID="DisplayNameTextBox" runat="server" />
		</div>
<%--
		<div class="cbc-Form-LabelValue">
			<asp:Label AssociatedControlID="FirstNameTextBox" ID="Label1" runat="server" Text="First Name" /><br />
		</div>
		<div class="cbc-Form-LabelValue">
			<asp:Label AssociatedControlID="LastNameTextBox" ID="Label2" runat="server" Text="Last Name" /><br />
		</div>
		<div class="cbc-Form-LabelValue">
			<asp:Label AssociatedControlID="EmailTextBox" ID="Label4" runat="server" Text="Live Email" /><br />
		</div>
--%>		
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
				OnClick="CancelButton_Click" />
		</div>
	</fieldset>
</asp:Content>
