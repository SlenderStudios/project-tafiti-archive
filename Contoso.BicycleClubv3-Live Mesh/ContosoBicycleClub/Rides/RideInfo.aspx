<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="RideInfo.aspx.cs" Inherits="WLQuickApps.ContosoBicycleClub.Rides.RideInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/BlogCommentListControl.ascx" TagName="BlogCommentListControl" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/BlogPostControl.ascx" TagName="BlogPostControl" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/EventInfoHeader.ascx" TagName="EventInfoHeader" TagPrefix="uc" %>
<%@ Register Src="~/UserControls/EventMultimediaControl.ascx" TagName="EventMultimediaControl" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContainerClassPlaceHolder" runat="server">cbc-EventInfo</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ScriptManagerPlaceHolder" runat="server">
	<asp:ScriptManager ID="ScriptManager1" runat="server">
		<Scripts>
			<asp:ScriptReference Path="~/assets/scripts/Profile.js" />
			<asp:ScriptReference Path="~/assets/scripts/Presence.js" />
			<asp:ScriptReference Path="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6" />
			<asp:ScriptReference Path="http://agappdom.net/h/silverlight.js" />
			<asp:ScriptReference Path="~/assets/scripts/StartPlayer.js"/>
			<asp:ScriptReference Path="~/assets/scripts/EventMultimedia.js" />
		</Scripts>
	</asp:ScriptManager>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeadingPlaceHolder" runat="server">	
	<uc:EventInfoHeader runat="server" id="InfoHeader" EventType="ride"></uc:EventInfoHeader>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">
	<uc:EventMultimediaControl runat="server" id="EventMultimedia"></uc:EventMultimediaControl>
	<uc:BlogPostControl runat="server" id="BlogPost"></uc:BlogPostControl>
	<uc:BlogCommentListControl runat="server" id="CommentList"></uc:BlogCommentListControl>
	<asp:Button CssClass="cbc-Form-Button" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, UploadPhotoLabel %>" ID="UploadButton"
		OnClick="UploadButton_Click" />
</asp:Content>
