<%@ Page Language="C#" ValidateRequest="false" EnableViewState="true" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="WLQuickApps.ContosoBicycleClub.Edit"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="ChatUsersPlaceHolder" runat="server" />

<asp:Content ID="Content2" ContentPlaceHolderID="PageHeadingPlaceHolder" runat="server">
	<div class="cbc-PageHeading">
		<h1>
			<asp:Localize ID="Localize1" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, OrganizeYourLabel %>" />
			<%= EventLabel %>
		</h1>
	</div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="PrimaryContentPlaceHolder" runat="server">

<script type="text/javascript">
function showProgress(statusDivId)
{
	if (!Page_IsValid) return;
	
	var div = $get("cbc-LoadingPanel");
	div.style.display = "block";
	
	var statusDiv = $get(statusDivId);
	if (statusDiv)
		statusDiv.style.display = "none";
}

function hideLoadingPanel(src, args)  
{  
	if (args.Value == "") {
		var div = $get("cbc-LoadingPanel");
		div.style.display = "none";
	}
}  
</script>

<fieldset class="cbc-Form-Fieldset">
    
  <div class="cbc-Form-Menu">
			<h2>
				<asp:Label AssociatedControlID="RideDropDownList" runat="server">
					<asp:Localize ID="Localize2" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, SelectYourLabel %>" />
					<%= EventLabel %>
				</asp:Label>
			</h2>
			<asp:DropDownList ID="RideDropDownList" runat="server" AutoPostBack="true" 
									onselectedindexchanged="RideDropDownList_SelectedIndexChanged" >
			</asp:DropDownList>
	</div>

<div id="cbc-LoadingPanel">
	<asp:Image ImageUrl="~/assets/images/loading.gif" AlternateText="<%$ Resources:ContosoBicycleClubWeb, UploadingLabel %>" runat="server" />
	<asp:Localize runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, UploadingLabel %>" />
</div>
	
			<div runat="server" ID="StatusMessage" Visible="false"></div>
			    
			<asp:TextBox ID="RideIdTextBox" runat="server" Visible="false" />

			<div class="cbc-Form-LabelValue">
				<asp:Label ID="Label1" AssociatedControlID="RideIdTextBox" Text="<%$ Resources:ContosoBicycleClubWeb, TitleLabel %>" runat="server" /><br />
				<asp:TextBox CssClass="cbc-Form-Input-Long" ID="RideNameTextBox" runat="server" />
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="RideNameTextBox" ErrorMessage="<%$ Resources:ContosoBicycleClubWeb, TitleRequiredLabel %>" />
				<asp:CustomValidator ID="CustomValidator1" runat="server"   
           ControlToValidate="RideNameTextBox" Display="Dynamic"  
           ErrorMessage="CustomValidator"   
           ClientValidationFunction="hideLoadingPanel"   
           ValidateEmptyText="True"></asp:CustomValidator>
			</div>

			<div class="cbc-Form-LabelValue">
				<asp:Label ID="Label2" AssociatedControlID="RideDateTextBox" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, DateLabel %>" /><br />
				<asp:TextBox CssClass="cbc-Form-Input-Long" ID="RideDateTextBox" runat="server" />
				<asp:Button ID="RideDateButton" runat="server" Text="..." />
				<ajx:CalendarExtender ID="CalendarExtender1" runat="server"
					EnabledOnClient="true" TargetControlID="RideDateTextBox" PopupButtonID="RideDateButton" />
					
				<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="RideDateTextBox" ErrorMessage="<%$ Resources:ContosoBicycleClubWeb, DateRequiredLabel %>" />
				<asp:CustomValidator ID="CustomValidator2" runat="server"   
           ControlToValidate="RideDateTextBox" Display="Dynamic"  
           ErrorMessage="CustomValidator"   
           ClientValidationFunction="hideLoadingPanel"   
           ValidateEmptyText="True"></asp:CustomValidator>
					
			</div>

			<div class="cbc-Form-LabelValue">
				<asp:Label ID="Label3" AssociatedControlID="RideDescTextBox" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, DescriptionLabel %>" /><br />
				<asp:TextBox CssClass="cbc-Form-Input-ExtraLong" ID="RideDescTextBox" runat="server" TextMode="MultiLine" Rows="10" Columns="60" />
			</div>

			<div class="cbc-Form-LabelValue">
				<asp:Label ID="Label4" AssociatedControlID="VECollectionIdTextBox" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, VECollectionIDLabel %>" /><br />
				<asp:TextBox CssClass="cbc-Form-Input-Long" ID="VECollectionIdTextBox" runat="server" />
			</div>

			<div class="cbc-Form-LabelValue">
				<asp:Label ID="Label5" AssociatedControlID="PhotoAlbumLinkTextBox" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, PhotoAlbumUrlLabel %>" /><br />
				<asp:TextBox CssClass="cbc-Form-Input-Long" ID="PhotoAlbumLinkTextBox" runat="server" />
			</div>

			<div class="cbc-Form-LabelValue">
				<asp:Label ID="Label6" AssociatedControlID="PhotoLinkTextBox" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, ThumbnailUrlLabel %>" /><br />
				<asp:TextBox CssClass="cbc-Form-Input-Long" ID="PhotoLinkTextBox" runat="server" />
			</div>

			<div class="cbc-Form-LabelValue">
				<asp:Label ID="Label7" AssociatedControlID="VideoLinkTextBox" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, SilverlightStreamingVideoLabel %>" /><br />
				<asp:TextBox CssClass="cbc-Form-Input-Long" ID="VideoLinkTextBox" runat="server" ReadOnly="true" />				
				
				<ajx:TextBoxWatermarkExtender ID="VideoLinkTextBoxWatermarkExtender1" runat="server"
					TargetControlID="VideoLinkTextBox"
					WatermarkText="Please select a video to up load." WatermarkCssClass=".cbc-Form-Input-Long-Greyout">
				</ajx:TextBoxWatermarkExtender>
		
				<asp:FileUpload runat="server" ID="FileUploadTextbox"/>
			</div>
   
			<div class="cbc-Form-Buttons">
				<asp:Button CssClass="cbc-Form-Button" ID="SaveButton" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, SaveLabel %>" onclick="SaveButton_Click" UseSubmitBehavior="true" />
				<asp:Button CssClass="cbc-Form-Button" ID="CancelButton" runat="server" Text="<%$ Resources:ContosoBicycleClubWeb, CancelLabel %>" onclick="CancelButton_Click" CausesValidation="False" />
			</div>
</fieldset>
</asp:Content>
