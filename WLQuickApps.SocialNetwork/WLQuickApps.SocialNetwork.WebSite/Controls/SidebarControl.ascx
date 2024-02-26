<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SidebarControl.ascx.cs" Inherits="SocialNetwork_SidebarControl" %>
<div id="sidebar-content">
    <asp:LoginView runat="server" ID="_loginView" Visible="false">
        <AnonymousTemplate>
            <div id="anonymous">
                    <asp:HyperLink runat="server" ID="_registerLink" NavigateUrl="~/Register.aspx" Text="Join Now" CssClass="link2" />              
                    <hr />
                    <div id="sidebar-notLogged">Already a user? <asp:HyperLink runat="server" NavigateUrl="~/SignIn.aspx" Text="Sign in" CssClass="link2" /></div>
            </div>
        </AnonymousTemplate>
    </asp:LoginView>
     
    <asp:FormView runat="server" ID="_baseItemFormView" Visible="false" Width="150">              
        <ItemTemplate>
        <div id="Item">
            <div id="Item-picture"> 
                <cc:NullablePicture ID="NullablePicture1" runat="server" Item='<%# Container.DataItem %>'
                    NullImageUrl='<%# string.Format("~/Images/missing-{0}-64x64.png", Container.DataItem.GetType().Name.ToLower()) %>' MaxHeight="64" MaxWidth="64"
                    NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' />
            </div> 
           <div id="Item-name"> 
                <asp:HyperLink runat="Server" ID="_titleLink" CssClass="Item-name" NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                    Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' />
            </div> 
        </div>  
        </ItemTemplate>  
    </asp:FormView> 
    <asp:FormView runat="server" ID="_userRequestsForm" Visible="false" DefaultMode="ReadOnly">
        <ItemTemplate>
            <asp:Panel ID="_requests" runat="server" CssClass="sidebar-notifications">
                <cc:RequestNotification ID="_friendRequests" runat="server" RequestsLabelText="friend requests" CssClass="link2"
                    RequestCount='<%# FriendManager.GetPendingFriendInvitations().Count %>'
                    SingularRequestsLabelText="friend request" NavigateUrl="~/User/Dashboard.aspx" />
                <cc:RequestNotification ID="_groupRequests" runat="server" RequestsLabelText="group invitations"  CssClass="link2"
                    RequestCount='<%# GroupManager.GetGroupsForUser(UserManager.LoggedInUser.UserName, UserGroupStatus.Invited).Count %>'
                    SingularRequestsLabelText="group invitation" NavigateUrl="~/User/Dashboard.aspx" />
                <cc:RequestNotification ID="_eventRequests" runat="server" RequestsLabelText="event invitations"  CssClass="link2"
                    RequestCount='<%# EventManager.GetEventsForUser(UserManager.LoggedInUser.UserName, UserGroupStatus.Invited).Count %>'
                    SingularRequestsLabelText="event invitation" NavigateUrl="~/User/Dashboard.aspx" />
            </asp:Panel>    
        </ItemTemplate>  
    </asp:FormView>    
    
    <asp:ObjectDataSource ID="_baseItemDataSource" runat="server" SelectMethod="GetBaseItem" TypeName="WLQuickApps.SocialNetwork.Business.BaseItemManager">
        <SelectParameters>
            <asp:Parameter Name="baseItemID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
     
    <asp:Panel ID="_mainPanel" runat="server" Visible="false" CssClass="sidebar-button">
        <asp:HyperLink ID="_mainHyperLink" runat="server" Text="Main" CssClass="sidebar-link" />    
    </asp:Panel>
    <asp:Panel ID="_usersPanel" runat="server" Visible="false" CssClass="sidebar-button">
        <asp:HyperLink ID="_usersHyperLink" runat="server" Text="Friends" CssClass="sidebar-link" />
    </asp:Panel>
    <asp:Panel ID="_groupsPanel" runat="server" Visible="false" CssClass="sidebar-button">
        <asp:HyperLink ID="_groupsHyperLink" runat="server" Text="Groups" CssClass="sidebar-link" />
    </asp:Panel>
    <asp:Panel ID="_eventsPanel" runat="server" Visible="false" CssClass="sidebar-button">
        <asp:HyperLink ID="_eventsHyperLink" runat="server" Text="Events" CssClass="sidebar-link" />
    </asp:Panel>
    <asp:Panel ID="_galleriesPanel" runat="server" Visible="false" CssClass="sidebar-button">
        <asp:HyperLink ID="_galleriesHyperLink" runat="server" Text="Galleries" CssClass="sidebar-link" />
    </asp:Panel>
    <asp:Panel ID="_collectionsPanel" runat="server" Visible="false" CssClass="sidebar-button">
        <asp:HyperLink ID="_collectionsHyperLink" runat="server" Text="Maps &#38; Places" CssClass="sidebar-link" />
    </asp:Panel>
    <asp:Panel ID="_calendarPanel" runat="server" Visible="false" CssClass="sidebar-button">
        <asp:HyperLink ID="HyperLink1" runat="server" Text="My Calendar" CssClass="sidebar-link"
            NavigateUrl="~/Event/ViewCalendar.aspx" />
    </asp:Panel>
    <asp:Panel ID="_dashboardPanel" runat="server" Visible="false" CssClass="sidebar-button">
        <asp:HyperLink ID="_dashboardHyperLink" runat="server" Text="Manage Requests" CssClass="sidebar-link"
            NavigateUrl="~/User/Dashboard.aspx" />
    </asp:Panel>
    <asp:Panel ID="_editProfilePanel" runat="server" Visible="false" CssClass="sidebar-button">
        <asp:HyperLink ID="_editProfileHyperLink" runat="server" Text="Edit Profile" CssClass="sidebar-link"
            NavigateUrl="~/User/EditProfile.aspx" />
    </asp:Panel>
</div>