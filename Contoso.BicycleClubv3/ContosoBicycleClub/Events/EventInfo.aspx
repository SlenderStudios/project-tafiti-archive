<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="EventInfo.aspx.cs" Inherits="WLQuickApps.ContosoBicycleClub.Events.EventInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/BlogCommentListControl.ascx" TagName="BlogCommentListControl" TagPrefix="CBC" %>
<%@ Register Src="~/UserControls/BlogPostControl.ascx" TagName="BlogPostControl" TagPrefix="CBC" %>
<%@ Register Src="~/UserControls/EventInfoHeader.ascx" TagName="EventInfoHeader" TagPrefix="CBC" %>
<%@ Register Src="~/UserControls/EventMultimediaControl.ascx" TagName="EventMultimediaControl" TagPrefix="CBC" %>

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
	
	<CBC:EventInfoHeader runat="server" id="InfoHeader" EventType="event"></CBC:EventInfoHeader>
	
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">

	<CBC:EventMultimediaControl runat="server" id="EventMultimedia"></CBC:EventMultimediaControl>

	<CBC:BlogPostControl runat="server" id="BlogPost"></CBC:BlogPostControl>

	<CBC:BlogCommentListControl runat="server" id="CommentList"></CBC:BlogCommentListControl>
	
</asp:Content>
