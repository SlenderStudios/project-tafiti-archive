<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" Title="Sign Up" %>
<%@ Register Src="Controls/LocationControl.ascx" TagName="LocationControl" TagPrefix="uc" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" SkinID="ImageGallery">
        <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">Register with <%= SettingsWrapper.SiteTitle %></cc:DropShadowPanel>
        <div style="float:right; width: 220px; border: 1px solid black; padding: 5px; text-align: center;">
            This site is for demonstration purposes only.  Please do not submit any real personally identifiable information.
            <br /><br />
            All information collected in these registration fields is deleted daily.
        </div>
        <div class="form-errorRow">
            <asp:ValidationSummary runat="server" ID="_errorSummary" DisplayMode="BulletList" />
        </div>
        
        <div class="form-label form-required">
            <asp:Label ID="_userNameLabel" runat="server" AssociatedControlID="_userName">Username</asp:Label>
        </div>
        <div class="form-field">
            <cc:SecureTextBox ID="_userName" runat="server" />
            <asp:RequiredFieldValidator ID="_userNameRequired" runat="server" ControlToValidate="_userName"
                ErrorMessage="Enter a username." ToolTip="Enter a username." Text="*" Display="Dynamic" />
            <asp:RegularExpressionValidator runat="server" ID="_userNameValid" ControlToValidate="_userName"
                ErrorMessage="Enter a username with only letters and numbers." ToolTip="Enter a valid username."
                Text="*" ValidationExpression="[A-Za-z0-9]+" Display="Dynamic" />
            <asp:CustomValidator runat="server" ID="_userNameAvailable" ControlToValidate="_userName"
                ErrorMessage="Sorry, that username is already taken." ToolTip="Enter a new username."
                Text="*" OnServerValidate="_userNameAvailable_ServerValidate" />
        </div>
        <div class="form-label form-required">
            <asp:Label ID="_emailLabel" runat="server" AssociatedControlID="_email">Email</asp:Label>
        </div>
        <div class="form-field">
            <cc:SecureTextBox ID="_email" runat="server" />
            <asp:RequiredFieldValidator ID="_emailRequired" runat="server" ControlToValidate="_email"
                ErrorMessage="Enter your email address." ToolTip="Enter your email address." Text="*" Display="Dynamic" />
            <asp:RegularExpressionValidator ID="_emailValid" runat="server" ControlToValidate="_email"
                ErrorMessage="Enter a valid email address." ToolTip="Enter a valid email address." Text="*"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" />
            <asp:CustomValidator runat="server" ID="_emailAvailable" ControlToValidate="_email"
                ErrorMessage="Sorry, that email address has been registered." ToolTip="Enter your email address."
                Text="*" OnServerValidate="_emailAvailable_ServerValidate" />
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
            Gender:
        </div>
        <div class="form-field">
            <asp:RadioButtonList runat="server" ID="_gender" />
            <asp:RequiredFieldValidator ID="_genderRequired" runat="server" ControlToValidate="_gender"
                ErrorMessage="Select your gender." ToolTip="Select your gender." Text="*" />
        </div>
        
        <div class="form-label">
            <asp:Label ID="_pictureLabel" runat="server" AssociatedControlID="_picture">Picture</asp:Label>
        </div>
        <div class="form-field">
            <asp:FileUpload runat="server" ID="_picture" />
            <asp:CustomValidator runat="server" ID="_pictureValid" ControlToValidate="_picture"
                ErrorMessage="Select a valid picture file." ToolTip="Select a valid picture file." Text="*"
                OnServerValidate="_pictureValid_ServerValidate" />
        </div>
        
        <div class="form-label form-required">
            <asp:Label ID="_birthDateLabel" runat="server" AssociatedControlID="_birthMonth">Birth date:</asp:Label>
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
                ErrorMessage="Enter your birth day." ToolTip="Enter your birth day." Text="*" Display="Dynamic" />
            <asp:DropDownList runat="server" ID="_birthYear">
                <asp:ListItem Value=""></asp:ListItem>
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="_birthYearRequired" runat="server" ControlToValidate="_birthYear"
                ErrorMessage="Enter your birth date." ToolTip="Enter your birth year." Text="*" Display="Dynamic" />
            <asp:CustomValidator runat="server" ID="_birthDateValid" ValidateEmptyText="false"
                ErrorMessage="Enter a valid birth year. Note that you must be at least 13 to use this site." ToolTip="Enter a valid birth date." Text="*"
                OnServerValidate="_birthDateValid_ServerValidate" />
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
        <!-- Messenger Presence - Start -->
        <script type="text/javascript" language="javascript">
             ///
             /// Open the window to the Messenger Consent screen
             ///
             /// ASP.NET integration (required in ASPX)
            function openPermissionScreen()
            {
                var returnUrl = window.location.href.replace('/Register.aspx', '/ProcessMessengerConsent.aspx');
                
                var privacyUrl = "<%# SettingsWrapper.PrivacyStatementOverrideUrl %>"
                
                // Check if there is a privacy override
                if(privacyUrl == '')
                {
                    // No privacy statement - go with /Privacy.aspx
                    privacyUrl = window.location.href.replace('/Register.aspx', '/Privacy.aspx');
                }
                
                var PermissionGrantingString = "";
                
                PermissionGrantingString = "http://settings.messenger.live.com/Applications/WebSignup.aspx?returnURL=" +
                                           returnUrl + "&privacyURL=" + privacyUrl;

                // If you send a # to the Consent screen it raises an error.
                PermissionGrantingString = PermissionGrantingString.replace("#", '');
                
                window.open(PermissionGrantingString, "ConsentScreen", 'width=800,height=610,scrollbars=yes');
            }
            
            ///
            /// Get the response and set the textbox value
            ///
            /// ASP.NET integration (required in ASPX)
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
                returnUrl = returnUrl.substring(0, returnUrl.lastIndexOf("/")) + '/PhotoAlbumPermission.aspx'

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
                document.getElementById('PhotoPermissionsSet').innerHTML = "<font color='green'>Permissions set (<a href='#' onclick='toggleDisplayPGUXDetails()'>click to see details</a>)</font>";
                document.getElementById('<%= this._DomainAuthenticationToken.ClientID %>').value = theDomainAuthenticationToken;
                document.getElementById('<%= this._OwnerHandle.ClientID %>').value = ownerHandle;
            }
            
            ///
            /// This will toggle the text and the display style of the div
            /// which wraps the domainAuthenticationToken and ownerHandle
            ///
            function toggleDisplayPGUXDetails()
            {
                var NewStateIsToShow = false;
                
                if(document.getElementById('PGUX_Wrapper').style.display == 'block')
                {
                    NewStateIsToShow = false;
                }
                else 
                {
                    NewStateIsToShow = true;
                }
            
                var displayStyle = '';
                
                if(NewStateIsToShow)
                {
                    displayStyle = 'block';
                    showText = 'click to hide details';
                }
                else 
                {
                    displayStyle = 'none';
                    showText = 'click to see details';
                }
                
                document.getElementById('PGUX_Wrapper').style.display = displayStyle;
                document.getElementById('PhotoPermissionsSet').innerHTML = "<font color='green'>Permissions set (<a href='#' onclick='toggleDisplayPGUXDetails()'>" + showText + "</a>)</font>";

            }
        </script>
        <div class="form-label">
            Grant permissions to your photos in Windows Live Spaces
        </div>
        <div class="form-field">
            <div id="PhotoPermissionsSet"><font color="red">Permissions Not Set <a href="#" onclick="openPGUX()">Click Here to Set</a></font></div>
            <div style="display: none;" id="PGUX_Wrapper">
                <cc:SecureTextBox runat="server" ID="_DomainAuthenticationToken" ToolTip="This is your Domain Authentication Token which is used to access your Photos." />              
                <cc:SecureTextBox runat="server" ID="_OwnerHandle" ToolTip="This is the owner handle (i.e. your Windows Live ID)." />              
            </div>
            <strong>IMPORTANT</strong>: To revoke your photo permissions go to the <a href="https://ux.cumulus.services.live.com/prux/" target="_blank">Revocation Page</a>.
        </div>
        <!-- Photo Permissions - End --> 
           
        <div class="form-label">
            About me
        </div>
        <div class="form-field">
            <cc:SecureTextBox runat="server" ID="_aboutMe" TextMode="MultiLine" Columns="30" Rows="5" />
        </div>

        <div class="form-field">
            <asp:Button runat="server" ID="_signUp" Text="Register" OnClick="_signUp_Click" />
        </div>

        <asp:Panel ID="_alertsPanel" runat="server">
        <div class="form-field">
            Upon clicking <strong>Register</strong>, you will be taken to the <a href="http://dev.live.com/alerts" target=_blank>Windows Live Alerts</a> site to 
            configure how you would like to be notified from this site. Note that Alerts registration
            is currently required, although you can opt out of all communications on your 
            <strong>Edit Profile</strong> page.
        </div>
        </asp:Panel>

    </cc:DropShadowPanel>
</asp:Content>

