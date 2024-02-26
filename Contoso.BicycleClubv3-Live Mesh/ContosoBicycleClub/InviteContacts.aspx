<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="InviteContacts.aspx.cs"
	Inherits="WLQuickApps.ContosoBicycleClub.InviteContacts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ChatUsersPlaceHolder" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">
    <asp:CheckBoxList ID="ContactList" runat="server"></asp:CheckBoxList><br />
    <asp:ListBox ID="ResultList" runat="server" Visible="false"></asp:ListBox><br />
	<div class="cbc-Form-Buttons">
		<asp:Button CssClass="cbc-Form-Button" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, SendLabel %>" ID="SaveButton"
			OnClick="SaveButton_Click" />
		<asp:Button CssClass="cbc-Form-Button" runat="server" Visible="false" Text="<%$ Resources:ContosoBicycleClubWeb, DoneLabel %>" ID="DoneButton"
			OnClick="DoneButton_Click" />
		<asp:Button CssClass="cbc-Form-Button" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, CancelLabel %>" ID="CancelButton"
			OnClick="CancelButton_Click" />
	</div>
</asp:Content>
