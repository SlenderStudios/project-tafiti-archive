<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendMessage.aspx.cs" Inherits="Friend_SendMessage" Title="Send Message" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
<br />
    <asp:Wizard ID="_sendMessageWizard" runat="server" DisplaySideBar="False" ActiveStepIndex="0" Width="730px">
        <WizardSteps>
            <asp:WizardStep ID="_firstStep" runat="server" Title="Select Recipients" StepType="Start" AllowReturn="True">
                <cc:DropShadowPanel ID="DropShadowPanel6" runat="server">
                    <cc:DropShadowPanel ID="DropShadowPanel7" runat="server" SkinID="ImageGallery-title">
                        Select Recipients
                    </cc:DropShadowPanel>
                    <strong>Friends</strong>
                    <asp:CheckBoxList runat="server" ID="_friendsCheckList" DataValueField="UserName" DataTextField="DisplayName" 
                        RepeatColumns="2" Width="600" />   
                    <asp:DataList runat="server" ID="_groupsDataList">
                        <ItemTemplate>
                            <strong>
                                <asp:Label runat="server" ID="_groupTypeLabel" Text='<%# ((string) Container.DataItem) %>' />
                                <asp:Label runat="server" ID="_groupTypeGenericLabel" Text="Groups" Visible='<%# string.IsNullOrEmpty((string) Container.DataItem) %>' />
                            </strong>
                            <asp:CheckBoxList runat="server" ID="_groupCheckList"
                                DataSource='<%# GroupManager.GetGroupsForUser(UserManager.LoggedInUser.UserName, ((string) Container.DataItem), UserGroupStatus.Joined) %>'
                                DataValueField="BaseItemID" DataTextField="Title" RepeatColumns="3" Width="600" />
                        </ItemTemplate>
                    </asp:DataList>
                    <strong>Events</strong>
                    <asp:CheckBoxList runat="server" ID="_eventCheckList" DataValueField="BaseItemID" DataTextField="DisplayName"
                         RepeatColumns="1" Width="600" />
                </cc:DropShadowPanel>
                <br />
            </asp:WizardStep>
             <asp:WizardStep ID="_finishStep" runat="server" StepType="Complete" Title="Complete" >
                <cc:DropShadowPanel runat="server" ID="_sendMessagePanel">
                    <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
                    Send Email
                    </cc:DropShadowPanel>
                    <br />
                    <div class="form-label form-required">
                        <asp:Label ID="_messageLabel" runat="server" AssociatedControlID="_message">Message:</asp:Label>
                    </div>
                    <div class="form-field">
                        <cc:SecureTextBox ID="_message" runat="server" TextMode="MultiLine" Columns="50" Rows="9" /><br />
                        <asp:RequiredFieldValidator ID="_messageRequired" runat="server" ControlToValidate="_message" 
                            Text="* Enter a message or hit cancel." />
                    </div>
                    <div class="form-field">
                        <br />
                        <asp:Button runat="server" ID="_sendButton" Text="Send" OnClick="_sendButton_Click" />
                        <asp:Button runat="server" ID="_cancelButton" Text="Cancel" OnClick="_cancelButton_Click" CausesValidation="False" />
                    </div>    
                 </cc:DropShadowPanel>
            </asp:WizardStep>
        </WizardSteps>
    </asp:Wizard>
</asp:Content>
