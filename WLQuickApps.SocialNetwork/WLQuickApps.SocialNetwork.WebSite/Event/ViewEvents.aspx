<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewEvents.aspx.cs" Inherits="ViewEvents" Title="View Events" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" ID="_eventsPanel" >
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
             <asp:Label ID="_titleLabel" runat="server" Text="Events"></asp:Label>
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_eventsGallery" runat="server" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="8" RepeatColumns="4" ViewMode="Event" EmptyDataText="No events were found." /><br />
    
         <asp:ObjectDataSource ID="_baseItemEventsDataSource" runat="server" SelectMethod="GetEventsByBaseItemID"
            TypeName="WLQuickApps.SocialNetwork.Business.EventManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" SelectCountMethod="GetEventsByBaseItemIDCount">
            <SelectParameters>
                <asp:QueryStringParameter Type="String" Name="baseItemID" QueryStringField="baseItemID"></asp:QueryStringParameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        
        <asp:ObjectDataSource ID="_baseItemPastEventsDataSource" runat="server" SelectMethod="GetPastEventsByBaseItemID"
            TypeName="WLQuickApps.SocialNetwork.Business.EventManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" SelectCountMethod="GetPastEventsByBaseItemIDCount">
            <SelectParameters>
                <asp:QueryStringParameter Type="String" Name="baseItemID" QueryStringField="baseItemID"></asp:QueryStringParameter>
            </SelectParameters>
        </asp:ObjectDataSource>
        
        <asp:ObjectDataSource runat="server" ID="_userEventsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.EventManager"
            SelectMethod="GetFutureEventsForUser" SelectCountMethod="GetFutureEventsForUserCount" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true">
            <SelectParameters>
                <asp:Parameter Name="userName" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
        
        <asp:ObjectDataSource runat="server" ID="_userPastEventsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.EventManager"
            SelectMethod="GetPastEventsForUser" SelectCountMethod="GetPastEventsForUserCount" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true">
            <SelectParameters>
                <asp:Parameter Name="userName" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
      
        <asp:HyperLink ID="_pastEventsHyperLink" runat="server" Text="View Past Events<br>" Visible="false" />
        <asp:HyperLink ID="_upcomingEventsHyperLink" runat="server" Text="View Upcoming Events<br>" Visible="false" />
        
        <asp:HyperLink runat="server" ID="_associateEventLink" Visible="false">
            <strong>
                Associate an event &gt;&gt;
            </strong>
        </asp:HyperLink>  
    </cc:DropShadowPanel>
   
    <asp:HyperLink ID="_createEventLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Event/AddEvent.aspx">Create Event &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_searchEventsLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Event/SearchEvents.aspx">Search Events &gt;&gt;</asp:HyperLink><br />
    <asp:HyperLink ID="_viewMyEventsLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Event/ViewCalendar.aspx">View My Calendar &gt;&gt;</asp:HyperLink>

</asp:Content>

