<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs"
    Inherits="Friend_ViewFriendRequests" Title="Dashboard" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="Server" ID="_requestsPanel">
        <cc:DropShadowPanel ID="DropShadowPanel1" runat="server" SkinID="ImageGallery-title">
            Requests from others
        </cc:DropShadowPanel>
        
        <asp:Label ID="_friendRequestsLabel" runat="server" SkinID="Dashboard-Requests">Friend Requests</asp:Label>
        <cc:MetaGallery runat="server" ID="_friendRequests" DataSourceID="_friendInvitationsDataSource"
            EmptyDataText="You don't have any new friend requests." SkinID="Dashboard-Requests" ViewMode="User" />
        
        <asp:Label ID="_groupRequestsLabel" runat="server" SkinID="Dashboard-Requests">Group Invitations</asp:Label>
        <cc:MetaGallery runat="server" ID="_groupGallery"
            DataSource='<%# GroupManager.GetGroupsForUser(UserManager.LoggedInUser.UserName, string.Empty, UserGroupStatus.Invited) %>'
            EmptyDataText="You don't have any new group invitations." SkinID="Dashboard-Requests" ViewMode="Group" />
        
        <asp:DataList runat="server" ID="_groupRequests">
            <ItemTemplate>
                <asp:PlaceHolder ID="_groupRequestsPlaceHolder" runat="server" Visible="False">
                    <asp:Label runat="server" ID="_groupTypeLabel" Text='<%# string.Format("{0} Invitations", Container.DataItem) %>'
                        SkinID="Dashboard-Requests" />
                    <asp:Label runat="server" ID="_specialGroupLabel" Text='<%# (string) Container.DataItem %>' Visible="False"/>                   
                    <cc:MetaGallery runat="server" ID="_specialGroupGallery" SkinID="Dashboard-Requests"
                        DataSourceID="_repeatedGroupsDataSource"
                        EmptyDataText="You don't have any new invitations" 
                        OnItemDataBound="Gallery_ItemDataBound" ViewMode="Group" />
                </asp:PlaceHolder>
                <asp:ObjectDataSource ID="_repeatedGroupsDataSource" runat="server" SelectMethod="GetGroupsForUser"
                    TypeName="WLQuickApps.SocialNetwork.Business.GroupManager">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="userName" QueryStringField="userName" Type="String" />
                        <asp:Parameter Name="status" Type="Object" DefaultValue="Invited" />
                        <asp:ControlParameter Name="groupType" ControlID="_specialGroupLabel" PropertyName="Text" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ItemTemplate>
        </asp:DataList>
        
        <asp:Label ID="_eventRequestsLabel" runat="server" SkinID="Dashboard-Requests">Event Invitations</asp:Label>
        <cc:MetaGallery runat="server" ID="_eventGallery" SkinID="Dashboard-Requests"
            DataSource='<%# EventManager.GetEventsForUser(UserManager.LoggedInUser.UserName, UserGroupStatus.Invited) %>'
            EmptyDataText="You don't have any new event invitations." ViewMode="Event" />
        
    </cc:DropShadowPanel>
    
    <cc:DropShadowPanel runat="server" ID="_invitationsPanel">
        <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ImageGallery-title">
            Requests I've sent
        </cc:DropShadowPanel>
        
        <asp:Label ID="_outgoingFriendRequestsLabel" runat="server" SkinID="Dashboard-Requests">Friend Requests</asp:Label>
        <cc:MetaGallery runat="server" ID="_friendInvitations" DataSourceID="_friendRequestsDataSource"
            EmptyDataText="You don't have any outgoing friend requests." SkinID="Dashboard-Requests" ViewMode="User" />
    </cc:DropShadowPanel>
    
    <asp:ObjectDataSource runat="server" ID="_friendRequestsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.FriendManager"
        SelectMethod="GetPendingFriendRequests" />
    <asp:ObjectDataSource runat="server" ID="_friendInvitationsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.FriendManager"
        SelectMethod="GetPendingFriendInvitations" />
</asp:Content>
