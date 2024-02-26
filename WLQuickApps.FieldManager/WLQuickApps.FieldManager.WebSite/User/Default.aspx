<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="WLQuickApps.FieldManager.WebSite.User_Default" MasterPageFile="~/AdminPages.master" %>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">
    <div class="pageContent">
        <div class="header">Manage Account</div>
        <div class="leftPanel">
            <div id="ManageMyLeagues">
                <div class="subHeader">My Leagues:</div>
                <asp:GridView ID="_leagueGridView" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="_leaguesDataSource" PageSize="5" AllowPaging="True"
                    PagerStyle-HorizontalAlign="Center" PagerSettings-PageButtonCount="10" PagerStyle-CssClass="PagerStyle"
                    CssClass="LeaguesTable" AlternatingRowStyle-CssClass="LeagueAltRow" HeaderStyle-CssClass="LeagueHeader" GridLines="Horizontal">
                    <Columns>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:HyperLink ID="_nameLink" runat="server" NavigateUrl='<%# string.Format("~/League/ViewLeague.aspx?leagueID={0}", Eval("LeagueID")) %>'><%# Eval("Title") %></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Description" HeaderText="Description" 
                            SortExpression="Description" />
                        <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                        <asp:TemplateField HeaderText="Remove">
                            <ItemTemplate>
                                <asp:LinkButton ID="_removeLink" runat="server" CommandArgument='<%# Eval("LeagueID") %>' OnClick="_removeLeagueClick">Remove</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="_deleteLink" runat="server" CommandArgument='<%# Eval("LeagueID") %>' OnClick="_deleteLeagueClick" Visible='<%# LeagueManager.IsLeagueAdmin((int) Eval("LeagueID")) %>'>Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="_leaguesDataSource" runat="server" 
                    SelectMethod="GetLeaguesForUser" 
                    TypeName="WLQuickApps.FieldManager.Business.LeagueManager" 
                    EnablePaging="True" 
                    SelectCountMethod="GetLeaguesForUserCount"
                    StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows"
                    >
                    <SelectParameters>
                        <asp:Parameter Name="userID" Type="Object" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <div class="ManageOptionsLinkDiv">
                    <asp:HyperLink ID="_createLeagueLink" runat="server" CssClass="optionsLink" NavigateUrl="~/User/CreateLeague.aspx">Create League</asp:HyperLink>
                </div>
            </div>
            <div id="ManageMyFields">
                <div class="subHeader">My Fields:</div>
                <asp:GridView ID="_leagueGridView0" runat="server" AutoGenerateColumns="False" 
                    DataSourceID="_fieldDataSource" PageSize="8" AllowPaging="True"
                    PagerStyle-HorizontalAlign="Center" PagerSettings-PageButtonCount="10" PagerStyle-CssClass="PagerStyle"
                    CssClass="LeaguesTable" AlternatingRowStyle-CssClass="LeagueAltRow" HeaderStyle-CssClass="LeagueHeader" GridLines="Horizontal">
                    <Columns>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:HyperLink ID="_nameLink" runat="server" NavigateUrl='<%# string.Format("~/Field/ViewField.aspx?fieldID={0}", Eval("FieldID")) %>'><%# Eval("Title") %></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Description" HeaderText="Description" 
                            SortExpression="Description" />
                        <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Width="200px"
                            SortExpression="Address" />
                        <asp:CheckBoxField DataField="IsOpen" HeaderText="Status" 
                            SortExpression="IsOpen" />
                        <asp:TemplateField HeaderText="Remove">
                            <ItemTemplate>
                                <asp:LinkButton ID="_removeLink" runat="server" CommandArgument='<%# Eval("FieldID") %>' OnClick="_removeFieldClick">Remove</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:LinkButton ID="_deleteLink" runat="server" CommandArgument='<%# Eval("FieldID") %>' OnClick="_deleteFieldClick" Visible='<%# FieldsManager.IsFieldAdmin((int) Eval("FieldID")) %>'>Delete</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="_fieldDataSource" runat="server" 
                SelectMethod="GetFieldsForUser" 
                TypeName="WLQuickApps.FieldManager.Business.FieldsManager" EnablePaging="True" 
                    SelectCountMethod="GetFieldsForUserCount"
                    StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows"
                    >
                    <SelectParameters>
                        <asp:Parameter Name="userID" Type="Object" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <div class="ManageOptionsLinkDiv">
                    <asp:HyperLink ID="_createFieldLink" runat="server" CssClass="optionsLink" NavigateUrl="~/User/CreateField.aspx">Create Field</asp:HyperLink>
                </div>                    
            </div>
        </div>

        <div class="rightPanel">
            <div class="subHeader">My Details</div>
                <div class="text-label">Display Email</div>
                    Your display email on this site.
                    <asp:TextBox ID="_displayEmailTextBox" runat="server" MaxLength="32"/>
                    <br />
                    <asp:RequiredFieldValidator ID="_displayNameValidator" runat="server" ControlToValidate="_displayEmailTextBox" Text="You must provide a display email"></asp:RequiredFieldValidator>
                <hr />
                <div class="text-label">Address</div>
                    If provided, this will be the default address used when determining directions to a field.
                    <asp:TextBox ID="_addressTextBox" CssClass="address-box" runat="server" TextMode="MultiLine" Rows="2" Columns="32"  MaxLength="256" />
                <hr />
                <!-- Messenger Presence - Start -->
                <script type="text/javascript" language="javascript">
                     // Open the window.
                    function openPermissionScreen()
                    {
                        var returnUrl = window.location.href.replace('/User/Default.aspx', '/ProcessMessengerConsent.aspx');
                        
                        // No privacy statement - go with /Privacy.aspx
                        var privacyUrl = window.location.href.replace('/User/EditProfile.aspx', '/Privacy.aspx');
                        
                        var permissionGrantingString = "http://settings.messenger.live.com/Applications/WebSignup.aspx?returnURL=" +
                                                   encodeURIComponent(returnUrl) + "&privacyURL=" + encodeURIComponent(privacyUrl);

                        // If you send a # to the Consent screen it raises an error.
                        permissionGrantingString = permissionGrantingString.replace("#", '');
                        
                        window.open(permissionGrantingString, "ConsentScreen", 'width=800,height=610,scrollbars=yes');
                    }
                    
                    // Get hte response and set the textbox value
                    function handleMessengerPermissionResponse(messengerID)
                    {
                        WLQuickApps.FieldManager.WebSite.SiteService.UpdateMessengerID(messengerID);
                        var statusNode = $get('<%= this._messengerIdentifier.ClientID %>').firstChild;
                        statusNode.nodeValue = (messengerID.length > 0) ? "Enabled" : "Disabled";                        
                    }
                </script>
           
                <div class="text-label">
                    Windows Live Messenger Presence
                </div>
                If enabled, your presence image and display name will be displayed on league pages you belong to and users 
                will be able to send you instant messages.
                <br />
                <strong>Currently <asp:Label runat="server" ID="_messengerIdentifier" /></strong><br />
                <a href="#" onclick="openPermissionScreen()">Enable</a> | <a href="#" onclick="handleMessengerPermissionResponse('')">Disable</a><br />
                <hr />
                <!-- Messenger Presence - End -->
                <asp:Panel runat="server" ID="_liveAlertsPanel">
                    <div class="text-label">
                        Windows Live Alerts
                    </div>
                    If you sign up for Windows Live Alerts, you will receive notifications when the fields you've added to "My Fields" are changed
                    by an administrator.
                    <div>
                        <asp:HyperLink runat="server" ID="_liveAlertsSignUpHyperlink" Text="Sign up" />
                        <asp:HyperLink runat="server" ID="_liveAlertsChangeHyperlink" Text="Change Delivery Settings" /><br />
                        <asp:LinkButton runat="server" ID="_liveAlertsUnsubscribeHyperlink" Text="Unsubscribe" OnCommand="_liveAlertsUnsubscribe_Command" />
                    </div>
                </asp:Panel>
                <asp:LinkButton ID="_updateLinkButton" CssClass="optionsLink" runat="server" OnClick="_updateClick">Update</asp:LinkButton>
        </div>
    </div>
</asp:Content>