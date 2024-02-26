<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditProfile.aspx.cs"
    Inherits="User_EditProfile" Title="Edit Profile" %>
<%@ Register Src="../Controls/LocationControl.ascx" TagName="LocationControl" TagPrefix="uc" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" ID="_mainPanel">
        <cc:DropShadowPanel runat="Server" ID="_mainPanelTitle" SkinID="ImageGallery-Title">
            User Details
        </cc:DropShadowPanel>
        <div class="form-errorRow">
            <asp:ValidationSummary runat="server" ID="_errorSummary" DisplayMode="BulletList" />
            <asp:Label runat="server" ID="_invalidPictureError" Text="Select a valid image file to upload." />
        </div>
        
        <div class="form-label form-required">
            <asp:Label ID="_titleLabel" runat="server" AssociatedControlID="_title">Display name</asp:Label>
        </div>
        <div class="form-field">
            <cc:SecureTextBox ID="_title" runat="server" />
            <asp:RequiredFieldValidator runat="server" ID="_titleRequiredValidator" ControlToValidate="_title" Text="*"
                ErrorMessage="Enter a display name." ToolTip="Enter a display name." />
        </div>

        <div class="form-label form-required">
            <asp:Label ID="_firstNameLabel" runat="server" AssociatedControlID="_firstName">First name</asp:Label>
        </div>
        <div class="form-field">
            <cc:SecureTextBox ID="_firstName" runat="server" />
            <asp:RequiredFieldValidator ID="_firstNameRequired" runat="server" ControlToValidate="_firstName"
                ErrorMessage="Enter your first name." ToolTip="Enter your first name." Text="*" />
        </div>
        <div class="form-label form-required">
            <asp:Label ID="_lastNameLabel" runat="server" AssociatedControlID="_lastName">Last name</asp:Label>
        </div>
        <div class="form-field">
            <cc:SecureTextBox ID="_lastName" runat="server" />
            <asp:RequiredFieldValidator ID="_lastNameRequired" runat="server" ControlToValidate="_lastName"
                ErrorMessage="Enter your last name." ToolTip="Enter your last name." Text="*" />
        </div>
        <div class="form-label form-required">
            <asp:Label ID="_birthDateLabel" runat="server" AssociatedControlID="_birthMonth">Birth date</asp:Label>
        </div>
        <div class="form-field">
            <asp:DropDownList runat="server" ID="_birthMonth">
                <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem>January</asp:ListItem>
                <asp:ListItem>February</asp:ListItem>
                <asp:ListItem>March</asp:ListItem>
                <asp:ListItem>April</asp:ListItem>
                <asp:ListItem>May</asp:ListItem>
                <asp:ListItem>June</asp:ListItem>
                <asp:ListItem>July</asp:ListItem>
                <asp:ListItem>August</asp:ListItem>
                <asp:ListItem>September</asp:ListItem>
                <asp:ListItem>October</asp:ListItem>
                <asp:ListItem>November</asp:ListItem>
                <asp:ListItem>December</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="_birthMonthRequired" runat="server" ControlToValidate="_birthMonth"
                ErrorMessage="Enter your birth month." ToolTip="Enter your birth month." Text="*" Display="Dynamic" />
            &nbsp;
            <asp:DropDownList runat="server" ID="_birthDay">
                <asp:ListItem Value=""></asp:ListItem>
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
                <asp:ListItem>16</asp:ListItem>
                <asp:ListItem>17</asp:ListItem>
                <asp:ListItem>18</asp:ListItem>
                <asp:ListItem>19</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>21</asp:ListItem>
                <asp:ListItem>22</asp:ListItem>
                <asp:ListItem>23</asp:ListItem>
                <asp:ListItem>24</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>31</asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="_birthDayRequired" runat="server" ControlToValidate="_birthDay"
                ErrorMessage="Enter your birth day." ToolTip="Enter your birth day." Text="*" Display="Dynamic" />,
            <asp:DropDownList runat="server" ID="_birthYear">
                <asp:ListItem Value=""></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="_birthYearRequired" runat="server" ControlToValidate="_birthYear"
                ErrorMessage="Enter your birth date." ToolTip="Enter your birth year." Text="*" Display="Dynamic" />
            <asp:CustomValidator runat="server" ID="_birthDateValid" ValidateEmptyText="false"
                ErrorMessage="Enter a valid birth date." ToolTip="Enter a valid birth date." Text="*"
                OnServerValidate="_birthDateValid_ServerValidate" />
        </div>
        <div class="form-label form-required">
            Gender
        </div>
        <div class="form-field">
            <asp:RadioButtonList runat="server" ID="_gender" />
            <asp:RequiredFieldValidator ID="_genderRequired" runat="server" ControlToValidate="_gender"
                ErrorMessage="Select your gender." ToolTip="Select your gender." Text="*" />
        </div>
        <div class="form-label">
            Address
        </div>
        <div class="form-field">
            <uc:LocationControl runat="server" ID="_locationControl" ShowLocationCaption="False" />
        </div>
        <div class="form-label">
            Blog RSS URL
        </div>
        <div class="form-field">
            <cc:SecureTextBox ID="_rssFeedUrl" runat="server" />
            <asp:RegularExpressionValidator runat="server" ID="_rssFeedUrlValid" ControlToValidate="_rssFeedUrl"
                ErrorMessage="Enter a valid URL." ToolTip="Enter a valid URL." Text="*"
                ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?" />                
        </div>
        <div class="form-label">
            About me
        </div>
        <div class="form-field">
            <cc:SecureTextBox runat="server" ID="_aboutMe" TextMode="MultiLine" Columns="30" Rows="5" />
        </div>

        <!-- Messenger Presence - Start -->
        <script type="text/javascript" language="javascript">
             // Open the window.
            function openPermissionScreen()
            {
                var returnUrl = window.location.href.replace('/User/EditProfile.aspx', '/ProcessMessengerConsent.aspx');
                
                var privacyUrl = "<%# SettingsWrapper.PrivacyStatementOverrideUrl %>"
                
                // Check if there is a privacy override
                if(privacyUrl == '')
                {
                    // No privacy statement - go with /Privacy.aspx
                    privacyUrl = window.location.href.replace('/User/EditProfile.aspx', '/Privacy.aspx');
                    
                }
                
                var PermissionGrantingString = "";
                PermissionGrantingString = "http://settings.messenger.live.com/Applications/WebSignup.aspx?returnURL=" +
                                           returnUrl + "&privacyURL=" + privacyUrl;

                // If you send a # to the Consent screen it raises an error.
                PermissionGrantingString = PermissionGrantingString.replace("#", '');
                
                window.open(PermissionGrantingString, "ConsentScreen", 'width=800,height=610,scrollbars=yes');
            }
            
            // Get hte response and set the textbox value
            function handleMessengerPermissionResponse(theMessengerID)
            {
                document.getElementById('<%= this._MessengerIdentifier.ClientID %>').value = theMessengerID;
            }
        </script>
   
        <div class="form-label">
            Windows Live Messenger Identifier
        </div>
        <div class="form-field">
            <cc:SecureTextBox runat="server" ID="_MessengerIdentifier" ToolTip="This can be your Windows Live ID (not recommended) or set via the Set button for a unique identifier." />  
            <a href="#" onclick="openPermissionScreen()">Set</a> | <a href="#" onclick="handleMessengerPermissionResponse('')">Clear</a><br />
            <strong>IMPORTANT</strong>: Your presence image will be displayed and users will be able to send you instant messages.
        </div>
        <!-- Messenger Presence - End -->
        
        
        <!-- Photo Permissions - Start -->    
        <script type="text/javascript" language="javascript">
            ///
            /// constructs and opens a window to the PGUX
            ///
            /// ASP.NET integration (required in ASPX)
            function openPGUX()
            {
            
                var isReturnUrlSSL = false;
                
                // use the current path
                var returnUrl = window.location.href;
                
                // return to the Photo album permissions page.
                returnUrl = window.location.href.replace('/User/EditProfile.aspx', '/PhotoAlbumPermission.aspx');

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
                    privacyUrl = privacyUrl.substring(0, privacyUrl.lastIndexOf("/")) + '/Privacy.aspx';
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
            
            ///
            /// Will set the inner text of the wrapping div
            /// also sets the hidden text boxes values
            ///
            /// ASP.NET integration (required in ASPX)
            function photoPermissionSet(theDomainAuthenticationToken, ownerHandle)
            {
                document.getElementById('<%= this._DomainAuthenticationToken.ClientID %>').value = theDomainAuthenticationToken;
                document.getElementById('<%= this._OwnerHandle.ClientID %>').value = ownerHandle;
            }
        </script>
        <div class="form-label">
            Grant permissions to your photos in Windows Live Spaces
        </div>
        <div class="form-field">
                <cc:SecureTextBox runat="server" ID="_DomainAuthenticationToken" ToolTip="This is your Domain Authentication Token which is used to access your Photos." />              
                <cc:SecureTextBox runat="server" ID="_OwnerHandle" ToolTip="This is the owner handle (i.e. your Windows Live ID)." />             
                <a href="#" onclick="openPGUX()">Set</a> | <a href="#" onclick="photoPermissionSet('','')">Clear</a>
                <br />
            <strong>IMPORTANT</strong>: You can revoke your photo permissions my clicking <a href="#" onclick="photoPermissionSet('','')">clear</a> or by going to the <a href="https://ux.cumulus.services.live.com/prux/" target="_blank">Revocation Page</a>.
        </div>
        <!-- Photo Permissions - End -->
        
        <asp:Panel runat="server" ID="_liveAlertsPanel">
            <div class="form-label">
                Windows Live Alerts
            </div>
            <div class="form-field">
                <asp:HyperLink runat="server" ID="_liveAlertsSignUpHyperlink" Text="Sign up" SkinID="Sidebar-BlogThisLink" />
                <asp:HyperLink runat="server" ID="_liveAlertsChangeHyperlink" Text="Change Delivery Settings" SkinID="Sidebar-BlogThisLink" /><br />
                <asp:LinkButton runat="server" ID="_liveAlertsUnsubscribeHyperlink" Text="Unsubscribe" SkinID="Sidebar-BlogThisLink" OnCommand="_liveAlertsUnsubscribe_Command" />
            </div>
        </asp:Panel>
        <div class="form-label">
            Profile Picture
        </div>
        <div class="form-field">
            <asp:FileUpload runat="server" ID="_pictureUpload" /><br />
            Old picture:<br />
            <cc:NullablePicture runat="server" ID="_pictureOldImage" MaxHeight="128" MaxWidth="128" NullImageUrl="~/Images/missing-user-128x128.png" /><br />
        </div>
       
        <div class="form-field">
            <br />
            <asp:Button runat="server" ID="_saveButton" Text="Save" OnClick="_saveButton_Click" />
            <asp:Button runat="server" ID="_cancelButton" Text="Cancel" OnClick="_cancelButton_Click" />
        </div>
    </cc:DropShadowPanel>
</asp:Content>
