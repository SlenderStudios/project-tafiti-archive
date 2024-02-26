<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invite.aspx.cs" Inherits="Friend_Invite" Title="Invite" EnableEventValidation="False" ViewStateEncryptionMode="Never" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <asp:Panel runat="server" SkinID="Invite-WrapperPanel" ID="_wrapperPanel">
        <asp:Wizard ID="_inviteWizard" runat="server" DisplaySideBar="True" ActiveStepIndex="0" 
            OnNextButtonClick="_inviteWizard_NextButtonClick" OnFinishButtonClick="_inviteWizard_FinishButtonClick"
            Width="730px" DisplayCancelButton="true" CancelButtonText="Skip" OnCancelButtonClick="_inviteWizard_CancelClick">
            <SideBarTemplate>
                <asp:Panel runat="server" SkinID="Invite-SideBar">
                    <asp:DataList runat="server" ID="SideBarList" RepeatDirect="Horizontal" RepeatColumns="5">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="SideBarButton" Visible="false" />
                            <asp:Label runat="server" ID="SideBarLabel" Text='<%# string.Format((this._inviteWizard.ActiveStep == ((WizardStep)Container.DataItem)) ? "<strong>{0}</strong>" : "{0}", ((WizardStep)Container.DataItem).Title) %>' />&nbsp;&gt;&nbsp;
                        </ItemTemplate>
                    </asp:DataList>
                </asp:Panel>
            </SideBarTemplate>
            <WizardSteps>
                <asp:WizardStep runat="server" Title="Invite By Email" StepType="Start" AllowReturn="False">
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
                    </ContentTemplate>
                    <br /><br />
                </asp:WizardStep>
                <asp:WizardStep runat="server" Title="Existing User Check" StepType="Step">
                    <cc:DropShadowPanel runat="server" >
                        <strong>The following people are already members.</strong> Check the ones you want to send friend
                        requests to:<br />
                        <asp:CheckBoxList ID="_nonFriendsList" runat="server" />
                    </cc:DropShadowPanel>
                </asp:WizardStep>
                <asp:WizardStep runat="server" StepType="Finish" Title="Send Invitations">
                    <cc:DropShadowPanel runat="server" >
                        <strong>The following contacts are not registered.</strong> Check the ones you want to invite to
                        the site:<br />
                        <asp:CheckBoxList ID="_unregisteredList" runat="server" />
                        <br />
                        Personal message (optional):<br />
                        <cc:SecureTextBox ID="_personalMessage" runat="server" Columns="40" Rows="6" TextMode="MultiLine" />
                    </cc:DropShadowPanel>
                </asp:WizardStep>
                <asp:WizardStep runat="server" StepType="Complete" Title="Complete" OnActivate="_finishStep_Activate">
                    <asp:Label runat="server" ID="_completeMessage">
                        Friends have been requested and invitations have been sent out.
                    </asp:Label>
                </asp:WizardStep>
            </WizardSteps>
        </asp:Wizard>
    </asp:Panel>
</asp:Content>
