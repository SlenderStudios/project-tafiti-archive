<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ViewGroupForm.ascx.cs" Inherits="ViewGroupForm" %>
<%@ Register Src="../Controls/LocationLinkControl.ascx" TagName="LocationLinkControl" TagPrefix="uc" %>
<%@ Register Src="../Controls/CommentsControl.ascx" TagName="CommentsControl" TagPrefix="uc" %>
<%@ Register Src="../Controls/Tags.ascx" TagName="Tags" TagPrefix="uc" %>

<div style="width:256px;float:left">
    <cc:DropShadowPanel runat="server" ID="_mainPanel" SkinID="ViewGroupForm-main">
        <asp:Label ID="_eventLink" runat="server" Text='<%# WebUtilities.TrimLongTitles(this.GroupItem.Title) %>' SkinID="ViewGroupForm-Title" /><br /><br />
        <cc:NullablePicture ID="_eventImageLink" runat="server" 
            Item='<%# this.GroupItem %>' MaxWidth="256" MaxHeight="256" SkinID="ViewGroup-Image" />
        <cc:DropShadowPanel runat="server" SkinID="ViewEvent-Description">
            <strong>owned by</strong>
            <asp:HyperLink ID="_creatorLink" runat="server" Text='<%# this.GroupItem.Owner.Title %>' CssClass="itemLabel"
                NavigateUrl='<%# WebUtilities.GetViewItemUrl(this.GroupItem.Owner) %>' /><br />
            <strong>at</strong>
            <uc:LocationLinkControl runat="server" ID="_location" LocationItem='<%# this.GroupItem.Location %>' ShowLocationCaption="True" /><br />
            <asp:Panel runat="server" ID="_datePanel">
                <strong>from</strong>
                <asp:Label ID="_startTime" runat="server" Text='<%# this.GetStartDateTime() %>' /><br />
                <strong>until</strong>
                <asp:Label ID="_endTime" runat="server" Text='<%# this.GetEndDateTime() %>' /><br />
            </asp:Panel>
            <br /><strong>tagged with</strong><br />
            <uc:Tags runat="server" ID="_tags" BaseItemID='<%# this.GroupItem.BaseItemID %>' />
        </cc:DropShadowPanel>
    </cc:DropShadowPanel>

    <cc:DropShadowPanel runat="server" ID="_actionsPanel" SkinID="ViewGroupForm-actions">
        <h3>Actions</h3>
        <cc:DropShadowPanel runat="server" SkinID="ViewEvent-Description">
            <asp:HyperLink ID="_inviteLink" runat="server" Text="Invite Friends<br />" CssClass="groupForm-link" NavigateUrl='<%# this.GetPageURL("~/Friend/Share.aspx") %>'
                Visible='<%# GroupManager.CanModifyGroup(this.GroupItem.BaseItemID) %>' />
            <asp:HyperLink ID="_sendMessageLink" runat="server" Text="Send Message<br />" CssClass="groupForm-link" NavigateUrl='<%# this.GetPageURL("~/Friend/SendMessage.aspx") %>'
                Visible='<%# (this.GetUserStatus() == UserStatus.Joined) %>' />    
            <asp:LinkButton ID="_joinGroupLink" runat="server" CausesValidation="False" OnClick="_joinGroupLink_Click" 
                Text='<%# this.IsEvent() ? "Join Event<br />" : "Join Group<br />" %>' CssClass="groupForm-link" Visible='<%# UserManager.IsUserLoggedIn() && this.GroupItem.CanJoin %>' />
            <asp:LinkButton ID="_approveGroupLink" runat="server" CausesValidation="False" OnClick="_joinGroupLink_Click"
                Text="Approve Invitation<br />" CssClass="groupForm-link" Visible='<%# this.GetUserStatus() == UserStatus.Invited %>' />
            <asp:LinkButton ID="_rejectGroupLink" runat="server" CausesValidation="False" OnClick="_leaveGroupLink_Click"
                Text="Reject Invitation<br />" CssClass="groupForm-link" Visible='<%# this.GetUserStatus() == UserStatus.Invited %>' />
            <asp:Label runat="server" ID="_pendingApprovalLabel" Text="<strong>You are pending approval by the creator.</strong>" Visible='<%# this.GetUserStatus() == UserStatus.WaitingForApproval %>' />
            <asp:LinkButton ID="_cancelGroupLink" runat="server" CausesValidation="false" OnClick="_leaveGroupLink_Click" Visible='<%# this.GetUserStatus() == UserStatus.WaitingForApproval %>'
                Text="Cancel request<br />" CssClass="groupForm-link" />
            <asp:HyperLink ID="_transferOwnershipLink" runat="server" CssClass="groupForm-link" Text="Transfer Ownership<br />" NavigateUrl='<%# this.GetPageURL("~/Group/Transfer.aspx") %>'
                Visible='<%# GroupManager.CanModifyGroup(this.GroupItem.BaseItemID) %>' />
            <asp:LinkButton ID="_leaveGroupLink" runat="server" CausesValidation="False" OnClick="_leaveGroupLink_Click"
                Text='<%# this.IsEvent() ? "Remove from Calendar<br />" : "Leave Group<br />" %>' CssClass="groupForm-link" Visible='<%# (this.GetUserStatus() == UserStatus.Joined) && !((this.GroupItem.Owner == UserManager.LoggedInUser) && (this.GroupItem.Users.Count > 1)) %>' />
            <asp:HyperLink ID="_editGroupLink" runat="server" CssClass="groupForm-link" NavigateUrl='<%# WebUtilities.GetEditItemUrl(this.GroupItem) %>'
                Text='<%# this.IsEvent() ? "Edit Event Details<br />" : "Edit Group Details<br />" %>' Visible='<%# GroupManager.CanModifyGroup(this.GroupItem.BaseItemID) %>' />
            <asp:HyperLink ID="_manageGroupLink" runat="server" CssClass="groupForm-link" NavigateUrl='<%# this.GetPageURL("~/Group/ManageGroup.aspx") %>'
                Text='<%# this.IsEvent() ? "Manage Event<br />" : "Manage Group<br />" %>' Visible='<%# GroupManager.CanModifyGroup(this.GroupItem.BaseItemID) %>' />
            <asp:LinkButton ID="_deleteGroupLink" runat="server" CssClass="groupForm-link" CausesValidation="False" OnClick="_deleteGroupLink_Click"
                Text='<%# this.IsEvent() ? "Cancel Event" : "Delete Group<br />" %>' Visible='<%# GroupManager.CanModifyGroup(this.GroupItem.BaseItemID) %>' />
            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteGroupLink"
                ConfirmText='<%# String.Format("Are you sure you want to delete this {0}?", this.IsEvent() ? "event" : "group") %>' />
        </cc:DropShadowPanel>
    </cc:DropShadowPanel>
</div>

<cc:DropShadowPanel runat="server" ID="_attendeesPanel" SkinID="ViewGroupForm-attendees">
    <h3><%= this.IsEvent() ? "Attendees" : "Members" %></h3>
    <cc:SecureTextBox runat="server" ID="_baseItemIDField" Text='<%# this.GroupItem.BaseItemID %>' Visible="false" />
    <ajaxToolkit:TabContainer ID="_attendeesTabContainer" runat="server">
        <ajaxToolkit:TabPanel ID="_attendeesTabPanel" runat="server" HeaderText='<%# this.IsEvent() ? "Attending" : "Members" %>'>
            <ContentTemplate>
                <cc:MetaGallery runat="server" ID="_attendees" DataSourceID="_attendeesDataSource"  ViewMode="Icon"
                    RepeatColumns="2" AllowPaging="true" PageSize="4" />
                <asp:HyperLink ID="_membersHyperLink" runat="server" Text="Full page view" 
                    NavigateUrl='<%# this.GetPageURL("~/Friend/ViewUsers.aspx") %>' />
                <asp:ObjectDataSource runat="server" ID="_attendeesDataSource" TypeName="WLQuickApps.SocialNetwork.Business.UserManager"
                    SelectMethod="GetAllUsersForGroup" SelectCountMethod="GetAllUsersForGroupCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:ControlParameter Name="groupID" Type="Int32" ControlID="_baseItemIDField" PropertyName="Text" />
                        <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="_inviteesTabPanel" runat="Server" HeaderText="Invited">
            <ContentTemplate>
                <cc:MetaGallery runat="server" ID="_invitees" DataSourceID="_inviteesDataSource"  ViewMode="Icon" RepeatColumns="2" AllowPaging="true" PageSize="4" 
                    EmptyDataText='<%# string.Format("There are no outstanding invitations to this {0}.", this.IsEvent() ? "event" : "group") %>' />
                <asp:ObjectDataSource runat="server" ID="_inviteesDataSource" TypeName="WLQuickApps.SocialNetwork.Business.UserManager"
                    SelectMethod="GetAllUsersForGroup" SelectCountMethod="GetAllUsersForGroupCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:ControlParameter Name="groupID" Type="Int32" ControlID="_baseItemIDField" PropertyName="Text" />
                        <asp:Parameter Name="status" Type="Object" DefaultValue="Invited" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="_requesteesTabPanel" runat="server" HeaderText='<%# GroupManager.CanModifyGroup(this.GroupItem.BaseItemID) ? "Requesting Invites" : string.Empty %>'
            Visible='<%# GroupManager.CanModifyGroup(this.GroupItem.BaseItemID) %>'>
            <ContentTemplate>
                <asp:Label runat="server" ID="_noRequesteesLabel" Text="There are currently no pending requests." Visible="false" />
                <asp:CheckBoxList runat="server" ID="_requesteesCheckList" DataValueField="UserName" DataTextField="DisplayName" 
                    RepeatColumns="2" Width="350" OnDataBound="_requesteesCheckList_DataBound" OnDataBinding="_requesteesCheckList_DataBinding"
                    DataSource='<%# FriendHelper.GetFriendSummary(UserManager.GetAllUsersForGroup(this.GroupItem.BaseItemID, UserGroupStatus.WaitingForApproval)) %>' /><br />
                <asp:Button runat="server" ID="_approveRequestees" Text="Approve" OnClick="_approveRequestees_Click" />
                <asp:Button runat="server" ID="_rejectRequestees" Text="Reject" OnClick="_rejectRequestees_Click" />
            </ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
</cc:DropShadowPanel>
    
<cc:DropShadowPanel runat="server" ID="_descriptionPanel" SkinID="ViewGroupForm-description">
    <h3>Description</h3>
    <asp:Label runat="server" Text='<%# this.GroupItem.Description %>' ID="_descriptionLabel" />
    <asp:Label runat="server" Text="There is currently no description." Visible='<%# String.IsNullOrEmpty(this.GroupItem.Description) %>' ID="_descriptionEmptyText" />
</cc:DropShadowPanel>

<cc:DropShadowPanel runat="server" ID="_albumsPanel" SkinID="ViewGroupForm-albums">
    <h3>Galleries</h3>
    <cc:MetaGallery runat="server" ID="_albums" DataSourceID="_albumsDataSource" RepeatColumns="2" AllowPaging="true" PageSize="4"  ViewMode="Icon"
        EmptyDataText='<%# String.Format("This {0} is not currently associated with any galleries.", this.IsEvent() ? "event" : "group") %>' /><br />
    <asp:ObjectDataSource runat="server" ID="_albumsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.AlbumManager"
        SelectMethod="GetAlbumsByBaseItemID" SelectCountMethod="GetAlbumsByBaseItemIDCount" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" EnablePaging="true">
        <SelectParameters>
            <asp:ControlParameter Name="baseItemID" Type="Int32" ControlID="_baseItemIDField" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:HyperLink ID="_galleriesHyperLink" runat="server" Text="Full page view" 
        NavigateUrl='<%# this.GetPageURL("~/Media/ViewAlbums.aspx") %>' /><br />
    <asp:HyperLink runat="server" ID="_associateAlbumLink" 
        NavigateUrl='<%# this.GetPageURL("~/Media/AssociateAlbum.aspx") %>'
        Visible='<%# this.GroupItem.CanAssociate %>' >
        <strong>
            Associate a gallery &gt;&gt;
        </strong>
    </asp:HyperLink>
</cc:DropShadowPanel>

<cc:DropShadowPanel runat="server" ID="DropShadowPanel2" SkinID="ViewGroupForm-albums">
    <h3>Upcoming Events</h3>
    <cc:MetaGallery runat="server" ID="MetaGallery2" DataSourceID="_eventsDataSource" RepeatColumns="2" AllowPaging="true" PageSize="4"  ViewMode="Icon"
        EmptyDataText='<%# String.Format("This {0} is not currently associated with any upcoming events.", this.IsEvent() ? "event" : "group") %>' /><br />
    <asp:ObjectDataSource runat="server" ID="_eventsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.EventManager"
        SelectMethod="GetEventsByBaseItemID" SelectCountMethod="GetEventsByBaseItemIDCount" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" EnablePaging="true">
        <SelectParameters>
            <asp:ControlParameter Name="baseItemID" Type="Int32" ControlID="_baseItemIDField" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:HyperLink ID="_eventsHyperLink" runat="server" Text="Full page view" 
        NavigateUrl='<%# this.GetPageURL("~/Event/ViewEvents.aspx") %>' /><br />
    <asp:HyperLink runat="server" ID="HyperLink2" 
        NavigateUrl='<%# this.GetPageURL("~/Event/AssociateEvent.aspx") %>'
        Visible='<%# this.GroupItem.CanAssociate %>' >
        <strong>
            Associate an event &gt;&gt;
        </strong>
    </asp:HyperLink>
</cc:DropShadowPanel>

<cc:DropShadowPanel runat="server" ID="DropShadowPanel1" SkinID="ViewGroupForm-albums">
    <h3>Maps</h3>
    <cc:MetaGallery runat="server" ID="MetaGallery1" DataSourceID="_collectionsDataSource" RepeatColumns="2" AllowPaging="true" PageSize="4"  ViewMode="Icon"
        EmptyDataText='<%# String.Format("This {0} is not currently associated with any maps.", this.IsEvent() ? "event" : "group") %>' /><br />
    <asp:ObjectDataSource runat="server" ID="_collectionsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.CollectionManager"
        SelectMethod="GetCollectionsByBaseItemID" SelectCountMethod="GetCollectionsByBaseItemIDCount" StartRowIndexParameterName="startRowIndex"
        MaximumRowsParameterName="maximumRows" EnablePaging="true">
        <SelectParameters>
            <asp:ControlParameter Name="baseItemID" Type="Int32" ControlID="_baseItemIDField" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:HyperLink ID="_collectionsHyperLink" runat="server" Text="Full page view" 
        NavigateUrl='<%# this.GetPageURL("~/Collection/ViewCollections.aspx") %>' /><br />
    <asp:HyperLink runat="server" ID="HyperLink1" 
        NavigateUrl='<%# this.GetPageURL("~/Collection/AssociateCollection.aspx") %>'
        Visible='<%# this.GroupItem.CanAssociate %>' >
        <strong>
            Associate a Map &gt;&gt;
        </strong>
    </asp:HyperLink>
</cc:DropShadowPanel>

<cc:DropShadowPanel runat="server" ID="_commentsPanel" SkinID="ViewGroupForm-comments">
    <h3>Comments</h3>
    <uc:CommentsControl ID="_CommentsControl" runat="server" BaseItemID='<%# this.GroupItem.BaseItemID %>' HalfPanel="true" />
</cc:DropShadowPanel>


<div class="clearFloats"></div>