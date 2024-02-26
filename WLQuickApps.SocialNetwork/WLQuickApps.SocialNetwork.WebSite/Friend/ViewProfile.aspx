<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewProfile.aspx.cs"
    Inherits="Friend_ViewProfile" Title="Untitled Page" %>
<%@ Register Src="../Controls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc1" %>
<%@ Register Src="../Controls/CommentsControl.ascx" TagName="CommentsControl" TagPrefix="uc1" %>
<%@ Register Src="../Controls/Tags.ascx" TagName="Tags" TagPrefix="uc1" %>
<%@ Register Src="../Controls/BlogHeadlines.ascx" TagName="BlogHeadlines" TagPrefix="uc1" %>
<%@ Register Src="../Controls/UserCalendar.ascx" TagName="UserCalendar" TagPrefix="uc" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="_doesNotExistPanel" runat="server" Visible="false">The specified user does not exist.</asp:Panel>
    <asp:FormView ID="FormView1" runat="server" DataSourceID="UserDataSource">
        <ItemTemplate>
            <asp:HiddenField ID="_userIDHiddenField" runat="server" Value='<%# Eval("UserID", "{0}") %>' />
            <asp:HiddenField ID="_userNameHiddenField" runat="server" Value='<%# Eval("UserName", "{0}") %>' />
            
            <cc:DropShadowPanel runat="server" >
                <uc1:UserDetails id="UserDetails1" runat="server" UserItem='<%# (User)Container.DataItem %>' />
                <cc:DropShadowPanel runat="server" SkinID="ViewProfile-ActionsBox" Visible='<%# ( UserManager.IsUserLoggedIn() ) && ( FriendManager.CanAddFriend(UserManager.GetUserByUserName(Eval("UserName", "{0}"))) || FriendManager.CanRemoveFriend(UserManager.GetUserByUserName(Eval("UserName", "{0}"))) || ((User)Container.DataItem).CanApprove ) %>'>     
                    <asp:HyperLink ID="_addFriendHyperLink" runat="server" NavigateUrl='<%# Eval("UserName", "~/Friend/AddFriend.aspx?userName={0}") %>'
                        Text="Add Friend" Visible='<%# FriendManager.CanAddFriend(UserManager.GetUserByUserName(Eval("UserName", "{0}"))) %>'>
                    </asp:HyperLink>
                    <asp:HyperLink ID="_sendMessageHyperLink" runat="server" NavigateUrl='<%# Eval("UserName", "~/Friend/SendMessage.aspx?userName={0}") %>'
                        Text="Send Friend Message <br />" Visible='<%# FriendManager.CanRemoveFriend(UserManager.GetUserByUserName(Eval("UserName", "{0}"))) %>'>
                    </asp:HyperLink>
                    <asp:HyperLink ID="_removeFriendHyperLink" runat="server" NavigateUrl='<%# Eval("UserName", "~/Friend/RemoveFriend.aspx?userName={0}") %>'
                        Text="Remove Friend" Visible='<%# FriendManager.CanRemoveFriend(UserManager.GetUserByUserName(Eval("UserName", "{0}"))) %>'>
                    </asp:HyperLink><br />
                    <asp:LinkButton runat="server" ID="_approveUser" CommandName="ApproveUser" CommandArgument='<%# Bind("BaseItemID") %>'
                            Visible='<%# ((User) Container.DataItem).CanApprove %>'
                            OnCommand="_approveUser_Command">Approve User</asp:LinkButton>
                            <ajaxToolkit:ConfirmButtonExtender ID="_approveConfirm" runat="server" 
                                TargetControlID="_approveUser"
                                ConfirmText="Are you sure you want to approve this user?" />                 
                </cc:DropShadowPanel>
                <div class="clearFloats"></div>
            </cc:DropShadowPanel>
            
            <cc:DropShadowPanel runat="server" SkinID="ViewProfile-AboutPanel">
                <cc:DropShadowPanel ID="_aboutMeTitle" runat="server" SkinID="ImageGallery-title">
                    About Me
                </cc:DropShadowPanel>
                <cc:DropShadowPanel runat="server" SkinID="indentedSection">
                    <asp:Label ID="AboutMeLabel" runat="server" Text='<%# Bind("Description") %>' />
                    <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0} has no About Me text.",((User)Container.DataItem).Title) %>'
                        Visible='<%# (String.IsNullOrEmpty(Eval("Description", "{0}"))) %>' />
                </cc:DropShadowPanel>
            </cc:DropShadowPanel>
            
            <cc:DropShadowPanel runat="server" SkinID="ViewProfile-TagsPanel">
                <cc:DropShadowPanel ID="_tagsTitle" runat="server" SkinID="ImageGallery-title">
                    Tags
                </cc:DropShadowPanel>
                <uc1:Tags runat="server" ID="_tags" BaseItemID='<%# Eval("BaseItemID") %>' />
            </cc:DropShadowPanel>

            <cc:DropShadowPanel runat="server" SkinID="ViewProfile-GalleriesPanel">
                <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
                    Galleries
                </cc:DropShadowPanel>
                <cc:MetaGallery runat="server" ID="_albumsGallery" DataSourceID="_albumsDataSource"
                    EmptyDataText='<%# String.Format("{0} does not have any galleries.",((User)Container.DataItem).Title) %>'
                    ViewMode="Album" RepeatColumns="2" AllowPaging="true" PageSize="2" />
                <asp:HyperLink ID="_galleriesHyperLink" runat="server" Text="Full page view"  
                    NavigateUrl='<%# string.Format("~/Media/ViewAlbums.aspx?{0}={1}",WebConstants.QueryVariables.UserName,((User)Container.DataItem).UserName) %>' />
                <asp:ObjectDataSource runat="server" ID="_albumsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.AlbumManager"
                    SelectMethod="GetAlbumsByUserID" SelectCountMethod="GetAlbumsByUserIDCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:ControlParameter Name="userID" Type="Object" ControlID="_userIDHiddenField" PropertyName="Value" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </cc:DropShadowPanel>
            
            <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ViewProfile-GroupsPanel">
                <cc:DropShadowPanel ID="DropShadowPanel3" runat="server" SkinID="ImageGallery-title">
                    Groups
                </cc:DropShadowPanel>
                <cc:MetaGallery runat="server" ID="_groupsGallery" DataSourceID="_groupsDataSource"
                    EmptyDataText='<%# String.Format("{0} does not belong to any groups.",((User)Container.DataItem).Title) %>'
                    ViewMode="Group" RepeatColumns="2" PageSize="2" AllowPaging="true" />
                <asp:HyperLink ID="_groupsHyperLink" runat="server" Text="Full page view"
                    NavigateUrl='<%# string.Format("~/Group/ViewGroups.aspx?{0}={1}",WebConstants.QueryVariables.UserName,((User)Container.DataItem).UserName) %>' />
                <asp:ObjectDataSource runat="server" ID="_groupsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.GroupManager"
                    SelectMethod="GetGroupsForUser" SelectCountMethod="GetGroupsForUserCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="userName" QueryStringField="userName" Type="String" />
                        <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </cc:DropShadowPanel>
            
            <asp:DataList runat="server" ID="_specialGroups" OnLoad="_specialGroups_Load">
                <ItemTemplate>
                    <cc:DropShadowPanel runat="server" ID="DropShadowPanel6">
                        <cc:DropShadowPanel runat="server" ID="DropShadowPanel7" SkinID="ImageGallery-title">
                            <asp:Label runat="server" ID="_specialGroupLabel" Text='<%# (string) Container.DataItem %>' />s
                        </cc:DropShadowPanel>
                        <cc:MetaGallery ID="MetaGallery2" runat="server" DataSourceID="_repeatedGroupsDataSource" DataKeyField="BaseItemID"
                            AllowPaging="True" PageSize="12" RepeatColumns="2" ViewMode="Group" EmptyDataText="You haven't joined any groups yet." />
                    </cc:DropShadowPanel>
                        <asp:ObjectDataSource ID="_repeatedGroupsDataSource" runat="server" SelectMethod="GetGroupsForUser"
                            TypeName="WLQuickApps.SocialNetwork.Business.GroupManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
                            MaximumRowsParameterName="maximumRows" SelectCountMethod="GetGroupsForUserCount">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="userName" QueryStringField="userName" Type="String" />
                                <asp:ControlParameter Name="groupType" ControlID="_specialGroupLabel" PropertyName="Text" />
                                <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                </ItemTemplate>
            </asp:DataList>

            <cc:DropShadowPanel ID="DropShadowPanel9" runat="server" SkinID="ViewProfile-EventsPanel">
                <cc:DropShadowPanel ID="DropShadowPanel10" runat="server" SkinID="ImageGallery-title">
                    Upcoming Events
                </cc:DropShadowPanel>
                 <cc:MetaGallery runat="server" ID="_eventsGallery" DataSourceID="_eventsDataSource"
                    EmptyDataText='<%# String.Format("{0} does not have any upcoming events.",((User)Container.DataItem).Title) %>'
                    ViewMode="Event" RepeatColumns="2" PageSize="2" AllowPaging="true" />
                <asp:HyperLink ID="_eventsHyperLink" runat="server" Text="Full page view"
                    NavigateUrl='<%# string.Format("~/Event/ViewEvents.aspx?{0}={1}",WebConstants.QueryVariables.UserName,((User)Container.DataItem).UserName) %>' />
            </cc:DropShadowPanel>
            <asp:ObjectDataSource runat="server" ID="_eventsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.EventManager"
                SelectMethod="GetFutureEventsForUser" SelectCountMethod="GetFutureEventsForUserCount" StartRowIndexParameterName="startRowIndex"
                MaximumRowsParameterName="maximumRows" EnablePaging="true">
                <SelectParameters>
                    <asp:QueryStringParameter Name="userName" QueryStringField="userName" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            
            <cc:DropShadowPanel ID="DropShadowPanel4" runat="server" SkinID="ViewProfile-CollectionsPanel">
                <cc:DropShadowPanel ID="DropShadowPanel5" runat="server" SkinID="ImageGallery-title">
                    Maps &amp; Places
                </cc:DropShadowPanel>
                <cc:MetaGallery ID="MetaGallery1" runat="server" DataSourceID="_collectionDataSource"
                    EmptyDataText='<%# String.Format("{0} has not added any maps yet.", ((User)Container.DataItem).Title) %>'
                    ViewMode="List" RepeatColumns="2" AllowPaging="true" PageSize="2" />
                <asp:HyperLink ID="_collectionsHyperLink" runat="server" Text="Full page view" 
                    NavigateUrl='<%# string.Format("~/Collection/ViewCollections.aspx?{0}={1}",WebConstants.QueryVariables.UserName,((User)Container.DataItem).UserName) %>' />
                <asp:ObjectDataSource runat="server" ID="_collectionDataSource" TypeName="WLQuickApps.SocialNetwork.Business.CollectionManager"
                    SelectMethod="GetCollectionsByUser" SelectCountMethod="GetCollectionsByUserCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:ControlParameter Name="userName" Type="String" ControlID="_userNameHiddenField" PropertyName="Value" />
                        <asp:Parameter Name="collectionType" Type="String" DefaultValue="" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </cc:DropShadowPanel><div class="clearFloats"></div>
            
            <cc:DropShadowPanel runat="server">
                <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
                    Friends
                </cc:DropShadowPanel>
                <cc:MetaGallery runat="server" DataSourceID="_friendsDataSource"
                    EmptyDataText='<%# String.Format("{0} has not added any friends yet.",((User)Container.DataItem).Title) %>'
                    ViewMode="User" RepeatColumns="4" AllowPaging="true" PageSize="4" />
                <asp:HyperLink ID="_friendsHyperLink" runat="server" Text="Full page view" 
                    NavigateUrl='<%# string.Format("~/Friend/ViewUsers.aspx?{0}={1}",WebConstants.QueryVariables.UserName,((User)Container.DataItem).UserName) %>' />
                <asp:ObjectDataSource runat="server" ID="_friendsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.FriendManager"
                    SelectMethod="GetFriendsByUserID" SelectCountMethod="GetFriendsCountByUserID" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:ControlParameter Name="userID" Type="Object" ControlID="_userIDHiddenField" PropertyName="Value" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </cc:DropShadowPanel>
            
            <cc:DropShadowPanel runat="server" Visible='<%# !String.IsNullOrEmpty(((User)Container.DataItem).RssFeedUrl) %>'
                SkinID="Titled">
                <cc:DropShadowPanel ID="DropShadowPanel1" runat="server" SkinID="ImageGallery-title">
                    Blog
                </cc:DropShadowPanel>
                <uc1:BlogHeadlines runat="server" ID="_blogHeadlines" RssUrl='<%# Bind("RssFeedUrl") %>' />
            </cc:DropShadowPanel>
            
            <cc:DropShadowPanel runat="server" SkinID="Titled">
                <cc:DropShadowPanel ID="_commentsTitle" runat="server" SkinID="ImageGallery-title">
                    Comments
                </cc:DropShadowPanel>
                <uc1:CommentsControl ID="CommentsControl1" runat="server" BaseItemID='<%# Bind("BaseItemID") %>' />
            </cc:DropShadowPanel>
        </ItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="UserDataSource" runat="server" SelectMethod="GetUserByUserName"
        TypeName="WLQuickApps.SocialNetwork.Business.UserManager" OnSelecting="DataSource_Selecting"
        OnSelected="DataSource_Selected">
        <SelectParameters>
            <asp:Parameter Name="userName" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
