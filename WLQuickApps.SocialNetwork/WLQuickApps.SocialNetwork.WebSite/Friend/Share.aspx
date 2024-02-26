<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Share.aspx.cs" Inherits="Friend_Share" Title="Untitled Page" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">    
    <br />
    <asp:Panel runat="server" SkinID="Share-WrapperPanel" ID="_wrapperPanel">
        <asp:Wizard ID="_inviteWizard" runat="server" DisplaySideBar="True" ActiveStepIndex="0" DisplayCancelButton="true" 
            CancelButtonText="Skip" OnCancelButtonClick="_inviteWizard_Cancel">
            <SideBarTemplate>
                <asp:Panel runat="server" SkinID="Share-SideBar">
                    <asp:DataList runat="server" ID="SideBarList" RepeatDirect="Horizontal" RepeatColumns="4">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="SideBarButton" Visible="false" />
                            <asp:Label runat="server" ID="SideBarLabel" Text='<%# string.Format((this._inviteWizard.ActiveStep == ((WizardStep)Container.DataItem)) ? "<strong>{0}</strong>" : "{0}", ((WizardStep)Container.DataItem).Title) %>' />&nbsp;<strong>&gt;</strong>&nbsp;
                        </ItemTemplate>
                    </asp:DataList>
                </asp:Panel>
            </SideBarTemplate>
            <WizardSteps>
                <asp:WizardStep ID="_firstStep" runat="server" Title="Invite from Site" StepType="Start" AllowReturn="False">
                    <cc:DropShadowPanel ID="DropShadowPanel6" runat="server">
                        <cc:DropShadowPanel ID="DropShadowPanel7" runat="server" SkinID="ImageGallery-title">
                            Invite from Site
                        </cc:DropShadowPanel>
                        
                        <asp:Panel runat="server" ID="_friendsPanel">
                            <strong>Friends</strong>
                            <asp:CheckBoxList runat="server" ID="_friendsCheckList" DataValueField="UserName" DataTextField="DisplayName" 
                                RepeatColumns="2" DataSource='<%# FriendHelper.GetFriendSummary() %>' />
                        </asp:Panel>
                        
                        <asp:DataList runat="server" ID="_specialGroupsDataList">
                            <ItemTemplate>
                            <asp:Panel ID="_specialGroupPanel" runat="server" Visible='<%# GroupManager.GetGroupsForUserCount(UserGroupStatus.Joined, (string) Container.DataItem) > 0 %>' >
                                <strong>
                                    <asp:Label runat="server" ID="_specialGroupLabel" Text='<%# (string) Container.DataItem %>' />s
                                </strong>
                            <asp:CheckBoxList runat="server" ID="_specialGroupCheckList"
                                DataSourceID="_specialGroupDataSource" DataValueField="BaseItemID" DataTextField="Title" RepeatColumns="3" />
                                <asp:ObjectDataSource ID="_specialGroupDataSource" runat="server" SelectMethod="GetGroupsForUser"
                                    TypeName="WLQuickApps.SocialNetwork.Business.GroupManager">
                                    <SelectParameters>
                                        <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
                                        <asp:ControlParameter Name="groupType" ControlID="_specialGroupLabel" PropertyName="Text" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </asp:Panel>
                            </ItemTemplate>
                        </asp:DataList>
                        
                        <asp:Panel ID="_groupsPanel" runat="server" Visible='<%# GroupManager.GetGroupsForUserCount(UserGroupStatus.Joined) > 0 %>' >
                            <strong>Groups</strong>
                            <asp:CheckBoxList runat="server" ID="_groupCheckList"
                                DataSourceID="_groupsDataSource" DataValueField="BaseItemID" DataTextField="Title" RepeatColumns="3" Width="600" />
                            <asp:ObjectDataSource ID="_groupsDataSource" runat="server" SelectMethod="GetGroupsForUser"
                                TypeName="WLQuickApps.SocialNetwork.Business.GroupManager">
                                <SelectParameters>
                                    <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>

                        <asp:Panel ID="_eventPanel" runat="server" Visible='<%# EventManager.GetEventsForUserCount(UserGroupStatus.Joined) > 0 %>' >
                            <strong>Events</strong>
                            <asp:CheckBoxList runat="server" ID="_eventCheckList"
                                DataSourceID="_eventsDataSource" DataValueField="BaseItemID" DataTextField="Title" RepeatColumns="3" Width="600" />
                            <asp:ObjectDataSource ID="_eventsDataSource" runat="server" SelectMethod="GetEventsForUser"
                                TypeName="WLQuickApps.SocialNetwork.Business.EventManager">
                                <SelectParameters>
                                    <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </asp:Panel>

                    </cc:DropShadowPanel>
                    <br /><br />
                </asp:WizardStep>
                <asp:WizardStep ID="_secondStep" runat="server" Title="Invite by Email" StepType="Step" AllowReturn="True">
                    <asp:UpdatePanel runat="server" ID="_updatePanel">
                        <ContentTemplate>    
                            <cc:DropShadowPanel ID="DropShadowPanel1" runat="server">
                                <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ImageGallery-title">
                                    Invite by Email
                                </cc:DropShadowPanel>
                            <div class="subform-label">Emails</div>
                            <div class="subform-field">
                                <cc:SecureTextBox ID="_email" runat="server" TextMode="MultiLine" Columns="50" Rows="8" /><br />
                                Enter one email per line.
                            </div>
                            </cc:DropShadowPanel>
                            <asp:ObjectDataSource ID="_inviteDataSource" runat="server" DeleteMethod="RemoveInvite"
                                InsertMethod="AddInvite" SelectMethod="GetInviteList" TypeName="WLQuickApps.SocialNetwork.WebSite.InvitationHelper">
                                <DeleteParameters>
                                    <asp:Parameter Name="key" Type="String" />
                                </DeleteParameters>
                                <InsertParameters>
                                    <asp:Parameter Name="key" Type="String" />
                                    <asp:Parameter Name="value" Type="String" />
                                </InsertParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br /><br />
                </asp:WizardStep>
                <asp:WizardStep ID="_thirdStep" runat="server" StepType="Step" Title="Via Windows Live">
                    <script type="text/javascript">
                    <!--
                        function ReceiveContactData(data)
                        {
                            PageMethods.ReceiveContactData(data, OnSucceeded);
                        }
                        
                        function OnSucceeded(result, userContext, methodName)
                        {
                            if (methodName == "ReceiveContactData")
                            {
                                <%= this.ClientScript.GetPostBackEventReference(this._refreshGrid, string.Empty) %>;
                            }
                        }
                    //-->
                    </script>
                            
                    <asp:Button runat="server" ID="_refreshGrid" UseSubmitBehavior="false" CausesValidation="False"
                        OnClick="_refreshGrid_Click" SkinID="Hidden" />
                
                    <cc:DropShadowPanel ID="DropShadowPanel3" runat="server">
                        <cc:DropShadowPanel ID="DropShadowPanel4" runat="server" SkinID="ImageGallery-title">
                            Invite via Windows Live
                        </cc:DropShadowPanel>
                        
                        Check the contacts you want to invite and click
                        the <strong>Send selected contacts</strong> button:
                        <br /><br />
                        <devlive:contactscontrol 
                            style="width:500px;height:400px;float:left;border:solid 1px gray;"
                            devlive:channelEndpointURL="../Media/Channel.htm"
                            devlive:privacyStatementURL="Privacy.aspx"
                            devlive:dataDesired="name,email"
                            devlive:onData="ReceiveContactData" />
                        <div class="clearFloats"></div>
	                    <br /><br />
	                </cc:DropShadowPanel>
    	            
	                <asp:
                </asp:WizardStep>
                <asp:WizardStep ID="_finishStep" runat="server" StepType="Complete" Title="Complete" OnActivate="_finishStep_Activate">
                    <asp:Panel runat="server" ID="_successPanel">
                        The friends you selected have been invited to <asp:Label runat="server" ID="_itemName" Font-Bold="True" />.
                    </asp:Panel>
                    <asp:Panel runat="server" ID="_failurePanel" Visible="false">
                        You did not select any friends to invite.
                    </asp:Panel>
                    <br /><br />
                    <asp:HyperLink runat="server" ID="_itemLink">Click here</asp:HyperLink> to return
                    to the <asp:Label runat="server" ID="_itemType" />.
                </asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>
    </asp:Panel>
</asp:Content>
