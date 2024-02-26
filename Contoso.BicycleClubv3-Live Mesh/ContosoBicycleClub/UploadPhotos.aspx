<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="UploadPhotos.aspx.cs"
	Inherits="WLQuickApps.ContosoBicycleClub.UploadPhotos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ChatUsersPlaceHolder" runat="server" />
<asp:Content ID="Content2" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">
	<fieldset class="cbc-Form-Fieldset">
		<div class="cbc-Form-Buttons">
		    <asp:TreeView runat="server" ID="MeshTree" Height="300" Width="400" BorderStyle="Inset" ShowLines="true" SelectedNodeStyle-BackColor="Blue"></asp:TreeView>
			<asp:Button CssClass="cbc-Form-Button" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, UploadPhotoLabel %>" ID="UploadButton"
				OnClick="UploadButton_Click" />
			<asp:Button CssClass="cbc-Form-Button" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, DoneLabel %>" ID="DoneButton"
				OnClick="DoneButton_Click" /><br />
		</div>
	</fieldset>
</asp:Content>
