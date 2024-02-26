<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddPicture.aspx.cs" Inherits="Media_AddPhoto" Title="Untitled Page" %>

<%@ Register Src="../Controls/LocationControl.ascx" TagName="LocationControl" TagPrefix="uc1" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" language="javascript">
        function openPGUX(requiresConfirmation)
        {
            // does require confirmation?
            if(requiresConfirmation)
            {
                // go confirmation
                if(!confirm('You will now be taken out to the Permission Granting Screen\nAnything you have entered on this screen will be wiped.'))
                {
                    // hit cancel
                    return;
                }
            }
        
            var isReturnUrlSSL = false;
            
            // use the current path
            var returnUrl = window.location.href;
            
            // return to the Photo album permissions page.
            returnUrl = returnUrl.substring(0, returnUrl.lastIndexOf("/Media")) + '/PhotoAlbumPermission.aspx'

            // if the return URL is SSL append the 
            if(returnUrl.indexOf("https://"))
            {
                isReturnUrlSSL = true;
            }

            // get the privacy path.
            var privacyUrl = "<%# SettingsWrapper.PrivacyStatementOverrideUrl %>";
            
            
            // Check if there is a privacy override
            if(privacyUrl == '')
            {
                // set privacy URL to the current page
                privacyUrl = window.location.href;
                
                // No privacy statement - go with /Privacy.aspx
                privacyUrl = privacyUrl.substring(0, privacyUrl.lastIndexOf("/Media")) + '/Privacy.aspx';
            }
            
            var PermissionGrantingString = "";
            
            // join the strings
            PermissionGrantingString = "https://ux.cumulus.services.live.com/pgux/default.aspx?rl=" + 
                                        returnUrl.replace("#", '') + 
                                        "&pl=" + privacyUrl.replace("#", '') + 
                                        "&ps=SpacesPhotos.ReadWrite";
                               
            // Is it on SSL?         
            if(isReturnUrlSSL)
            {
                // Append &NoSSL
                PermissionGrantingString = PermissionGrantingString + "&NoSSL";
            }
            
            // Redirect the window.
            window.open(PermissionGrantingString, '', 'width=750,height=800','');
        }
        
        function photoPermissionSet(theDomainAuthenticationToken, ownerHandle)
        {
            window.location.reload();
        }
    </script>

    <asp:Wizard ID="_addPictureWizard" runat="server" DisplaySideBar="False" OnNextButtonClick="_addPictureWizard_NextButtonClick" OnPreviousButtonClick="_addPictureWizard_PreviousButtonClick" ActiveStepIndex="0" OnFinishButtonClick="_addPictureWizard_FinishButtonClick">
        <WizardSteps>
            <asp:WizardStep ID="_pictureTypeStep" runat="server">
                   <cc:DropShadowPanel ID="_pictureTypePanel" runat="server" CssClassPrefix="shadow" DisplayShadow="False">
                    <cc:DropShadowPanel ID="_pictureTypePanelTitle" runat="server" SkinID="ImageGallery-title" CssClassPrefix="shadow" DisplayShadow="False">
                        Upload Picture
                    </cc:DropShadowPanel>
                    
                    <asp:RadioButtonList ID="_pictureTypeList" runat="server">
                        <asp:ListItem Value="0" Selected="True">Upload a picture from my computer.</asp:ListItem>
                    </asp:RadioButtonList>
                     
                     <p>Instead of uploading images one at a time, you can <asp:hyperlink ID="Hyperlink1" runat=server NavigateUrl="SynchronizeAlbum.aspx" Text="Synchronize from Windows Live Spaces" />.</p>
                    <asp:Panel runat="server" ID="pnlNoPhotoPermissions">
                        <p><strong>NOTE: </strong>To upload a photo to Windows Live Spaces you must grant permission by <a href="#" onclick="openPGUX(false)">clicking here</a>. You can revoke permission at anytime by visiting the <a href="https://ux.cumulus.services.live.com/prux/" target="_blank">Revocation Page</a></p>
                    </asp:Panel>
                    
                </cc:DropShadowPanel>
            </asp:WizardStep>
            <asp:WizardStep ID="_uploadPictureStep" runat="server" AllowReturn="False">
                <cc:DropShadowPanel ID="_uploadPicturePanel" runat="server" CssClassPrefix="shadow" DisplayShadow="False">
                    <cc:DropShadowPanel ID="_uploadPicturePanelTitle" runat="server" SkinID="ImageGallery-title" CssClassPrefix="shadow" DisplayShadow="False">
                        Upload Picture
                    </cc:DropShadowPanel>
                    
                    <div class="form-errorRow">
                        <asp:ValidationSummary ID="_uploadErrorSummary" runat="server" />
                        <asp:Label runat="server" ID="_uploadError" Text="Choose a valid picture to upload." Visible="false" />
                    </div>
                    <div class="form-field form-required">
                        Choose a picture from your computer:<br />
                    </div>
                    <div class="form-field">
                        <asp:FileUpload ID="_pictureFileUpload"  runat="server" />
                        <asp:RequiredFieldValidator ID="_uploadRequired" runat="server" ControlToValidate="_pictureFileUpload"
                            Text="*" ErrorMessage="Select a picture to upload." ToolTip="Select a picture to upload." />
                    </div>
                </cc:DropShadowPanel>
            </asp:WizardStep>
            <asp:WizardStep ID="_importPictureStep" runat="server" AllowReturn="False">
                <script type="text/javascript">
                <!--
                
                    function ReceivePhotoData(data)
                    {
                        if (data.length > 1)
                        {
                            alert('Please select only one photo.');
                        }

                        document.getElementById("<%= this._pictureUrl.ClientID %>").value = data[0].fileExpiringURL;
                        <%= this.ClientScript.GetPostBackEventReference(this._importPicture, string.Empty) %>
                    }

                //-->
                </script>
                
                <asp:HiddenField ID="_pictureUrl" runat="server" />
                <asp:Button ID="_importPicture" runat="server" SkinID="Hidden" OnClick="_importPicture_Click" />
                
                <cc:DropShadowPanel ID="_importPicturePanel" runat="server" CssClassPrefix="shadow" DisplayShadow="False">
                    <cc:DropShadowPanel ID="_importPicturePanelTitle" runat="server" SkinID="ImageGallery-title" CssClassPrefix="shadow" DisplayShadow="False">
                        Import Picture
                    </cc:DropShadowPanel>
                    
                    <div class="form-errorRow">
                        <asp:Label ID="_importPictureLabel" runat="server" Visible="False">
                            Select a picture to import.
                            <br /><br />
                        </asp:Label>
                    </div>
                    
                    Select a picture below and click <strong>Send selected photos</strong>:
                    <br /><br />
                    <?xml namespace="" prefix="devlive" ?>
                    <devlive:spacescontrol 
                        style="width:500px;height:400px;float:left;border:solid 1px gray;"
                        devlive:channelEndpointURL="Channel.htm"
                        devlive:privacyStatementURL="Privacy.aspx"
                        devlive:dataDesired="fileExpiringUrl"
                        devlive:market="en"
                        devlive:onData="ReceivePhotoData" />
                </cc:DropShadowPanel>
            </asp:WizardStep>
            <asp:WizardStep ID="_pictureDetailsStep" runat="server">
                <cc:DropShadowPanel runat="server" ID="_metaDataPanel" >
                    <cc:DropShadowPanel runat="server" ID="_metaDataPanelTitle" SkinID="ImageGallery-title">
                        Picture Details
                    </cc:DropShadowPanel>
                    <div class="form-field form-errorRow">
                        <asp:ValidationSummary ID="_errorSummary" runat="server" DisplayMode="BulletList" />
                    </div>
                    <div class="form-label form-required">
                        <asp:Label runat="server" ID="_galleryLabel" Text="To gallery" />
                    </div>
                    <div class="form-field">
                        <asp:UpdatePanel runat="server" ID="_galleryUpdatePanel" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:DropDownList ID="_albumDropDownList" runat="server" DataSourceID="ObjectDataSource1" OnSelectedIndexChanged="_albumDropDownList_SelectedIndexChanged" AutoPostBack="true"
                                    DataTextField="Title" DataValueField="BaseItemID" OnDataBound="_albumDropDownList_DataBound">
                                </asp:DropDownList>
                                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAllAlbums"
                                    TypeName="WLQuickApps.SocialNetwork.Business.AlbumManager" />
                                <cc:SecureTextBox runat="server" ID="_createAlbumText" Visible="false" />
                                <asp:RequiredFieldValidator ID="_createAlbumRequired" runat="server" ControlToValidate="_createAlbumText"
                                    Text="*" ErrorMessage="Enter a name for a new gallery to place this picture in."
                                    ToolTip="Enter a name for a new gallery." />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-label form-required">
                        Title
                    </div>
                    <div class="form-field">
                        <cc:SecureTextBox ID="_captionTextBox" runat="server" />
                        <asp:RequiredFieldValidator ID="_captionRequired" runat="server" ControlToValidate="_captionTextBox"
                            Text="*" ErrorMessage="Enter a title for the picture." ToolTip="Enter a title." />
                    </div>
                    <div class="form-label">Storage Location </div>
                    <div class="form-field"> 
                        <asp:RadioButtonList ID="_rdoStorage" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Local" Value="0" Selected=True></asp:ListItem>
                            <asp:ListItem Text="Windows Live Spaces" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="form-label">
                        Description
                    </div>
                    <div class="form-field">
                        <cc:SecureTextBox ID="_descriptionTextBox" runat="server" Height="94px" TextMode="MultiLine" Width="276px" />
                    </div>
                    <div class="form-label">
                        Location
                    </div>
                    <div class="form-field">
                        <uc1:LocationControl id="_locationControl" runat="server" ShowLocationCaption="false" />
                    </div>
                </cc:DropShadowPanel>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>    
</asp:Content>

