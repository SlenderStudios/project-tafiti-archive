<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="ApproveItems.aspx.cs" Inherits="Admin_ApproveItems" Title="Approve Items" %>
    
<%@ Register Src="~/Controls/UserCalendar.ascx" TagName="UserCalendar" TagPrefix="uc4" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel ID="_noResultsPanel" runat="server" Visible="False">
        All items have a status of approved.
    </cc:DropShadowPanel>
    <cc:DropShadowPanel runat="server" ID="_searchResultsPanel">
        <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ImageGallery-title">
            Items Pending Approval
        </cc:DropShadowPanel>
        <asp:Label ID="_unapprovedItemsTotalLabel" runat="server"></asp:Label><br /><br />
        <ajaxToolkit:TabContainer ID="_searchResultsTabs" runat="server" ActiveTabIndex="0">
            <ajaxToolkit:TabPanel ID="_pictureTabPanel" runat="server" HeaderText="Pictures">
                <ContentTemplate>
                    <br />
                    <cc:MetaGallery runat="server" ID="_picturesGallery" /><br />
                    <asp:LinkButton runat="server" ID="_approvePictures" OnCommand="_approvePictures_Command">Approve All Pictures On This Tab</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="_approvePicturesConfirm" runat="server" 
                        TargetControlID="_approvePictures" ConfirmText="Are you sure you want to approve all of these pictures?" />
                    <br />
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="_videoTabPanel" runat="server" HeaderText="Video">
                <ContentTemplate>
                    <br />
                    <cc:MetaGallery runat="server" ID="_videoGallery" /><br />
                    <asp:LinkButton runat="server" ID="_approveVideo" OnCommand="_approveVideo_Command">Approve All Video On This Tab</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="_approveVideoConfirm" runat="server" 
                        TargetControlID="_approveVideo" ConfirmText="Are you sure you want to approve all of these videos?" />
                    <br />
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="_audioTabPanel" runat="server" HeaderText="Audio">
                <ContentTemplate>
                    <br />
                    <cc:MetaGallery runat="server" ID="_audioGallery" /><br />
                    <asp:LinkButton runat="server" ID="_approveAudio" OnCommand="_approveAudio_Command">Approve All Audio On This Tab</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="_approveAudioConfirm" runat="server" 
                        TargetControlID="_approveAudio" ConfirmText="Are you sure you want to approve all of these audio?" />
                    <br />
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="_usersTabPanel" runat="server" HeaderText="Users">
                <ContentTemplate>
                    <br />
                    <cc:MetaGallery runat="server" ID="_usersGallery" />
                    <br />
                    <asp:LinkButton runat="server" ID="_approveUsers" OnCommand="_approveUsers_Command">Approve All Users On This Tab</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="_approveUsersConfirm" runat="server" 
                        TargetControlID="_approveUsers" ConfirmText="Are you sure you want to approve all of these users?" />
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="_groupsTabPanel" runat="server" HeaderText="Groups">
                <ContentTemplate>
                    <br />
                    <cc:MetaGallery runat="server" ID="_groupsGallery" /><br />
                     <asp:LinkButton runat="server" ID="_approveGroups" OnCommand="_approveGroups_Command">Approve All Groups On This Tab</asp:LinkButton>
                     <ajaxToolkit:ConfirmButtonExtender ID="_approveGroupsConfirm" runat="server" 
                        TargetControlID="_approveGroups" ConfirmText="Are you sure you want to approve all of these groups?" />
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="_eventsTabPanel" runat="server" HeaderText="Events">
                <ContentTemplate>
                    <br />
                    <uc4:usercalendar id="_eventsCalendar" runat="server" displaymode="SimpleList" /><br />
                    <asp:LinkButton runat="server" ID="_approveEvents" OnCommand="_approveEvents_Command">Approve All Events On This Tab</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="_approveEventsConfirm" runat="server" 
                        TargetControlID="_approveEvents" ConfirmText="Are you sure you want to approve all of these events?" />
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="_collectionsTabPanel" runat="server" HeaderText="Maps">
                <ContentTemplate>
                    <br />
                    <cc:MetaGallery runat="server" ID="_collectionsGallery" /><br />
                    <asp:LinkButton runat="server" ID="_approveCollections" OnCommand="_approveCollections_Command">Approve All Maps On This Tab</asp:LinkButton>
                    <ajaxToolkit:ConfirmButtonExtender ID="_approveCollectionsConfirm" runat="server" 
                        TargetControlID="_approveCollections" ConfirmText="Are you sure you want to approve all of these maps?" />
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </cc:DropShadowPanel>
</asp:Content>
