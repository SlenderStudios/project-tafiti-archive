<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventMultimediaControl.ascx.cs" Inherits="WLQuickApps.ContosoBicycleClub.UserControls.EventMultimediaControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<div class="cbc-EventMultimedia">
	<div class="cbc-TabSwitcherContainer">
		<ul class="cbc-TabSwitcher">
			<li id="SlideShowTab"><asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, PhotosLabel %>" /></li>
			<li id="VideoTab"><asp:Localize runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, VideoLabel %>" /></li>
			<li id="DirectionsTab"><asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, RouteInfoLabel %>" /></li>
		</ul>
		<div id="SlideShowPanel">
			<asp:Image ID="Slides" runat="server" Height="240" 
			ImageUrl="~/assets/images/blank_slide.png"
			AlternateText="" />

			<asp:Button ID="SlidesPrev" Text="&lt;" runat="server" />
			<asp:Button ID="SlidesPlay" runat="server" />
			<asp:Button ID="SlidesNext" Text="&gt;" runat="server" />

			<ajaxToolkit:SlideShowExtender ID="SlidesExtender" runat="server" 
				TargetControlID="Slides"
				SlideShowServiceMethod="GetSlides" 
				AutoPlay="true" 
				UseContextKey="true"
				PlayButtonID="SlidesPlay"
				PlayButtonText="Play"
				StopButtonText="Stop"
				NextButtonID="SlidesNext"
				PreviousButtonID="SlidesPrev"
				Loop="true" />
		</div>		
		<div id="VideoPanel" class="silverlightHost"></div>
		<div id="DirectionsPanel"></div>		
	</div>		
	<div id="MapPanel"></div>
</div>