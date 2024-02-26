<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Statistics.aspx.cs" Inherits="Admin_Statistics" 
        Title="Statistics" %>

<asp:content id="_content" contentplaceholderid="MainContent" runat="server">
    Number of users currently logged in:
    <asp:Label ID="_loggedInUsers" runat="server"></asp:Label><br />
    <br />
    <br />
    <cc:DropShadowPanel runat="server" ID="_activityPanel">
        Activity in the last
        <cc:SecureTextBox ID="_hoursTextBox" runat="server" Width="60px">24</cc:SecureTextBox>
        hours:
        <asp:RequiredFieldValidator runat="server" ID="_hoursRequiredValidator" ControlToValidate="_hoursTextBox" 
            ErrorMessage="Please enter a double value" 
            Text="*" ToolTip="Please enter a double value." />
        <asp:CustomValidator runat="server" ID="_hoursValidator" ControlToValidate="_hoursTextBox"
            ErrorMessage="Please enter a double value" ToolTip="Value entered must be a double."
            Text="*" OnServerValidate="_hoursTextBox_ServerValidate" /><br />
        * created numbers represent items created and not deleted
        <br />
        <br />
        <asp:detailsview id="_statsActivityDetailsView" runat="server" autogeneraterows="False" datasourceid="_statsActivityDataSource" CellSpacing="2" Width="250px">
            <RowStyle HorizontalAlign="Right"></RowStyle>
            <FieldHeaderStyle HorizontalAlign="Left"></FieldHeaderStyle>
            <Fields>
                <asp:BoundField ReadOnly="True" DataField="ActiveUserCount" SortExpression="ActiveUserCount" HeaderText="Distinct Active Users"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="AlbumCount" SortExpression="AlbumCount" HeaderText="Albums Created"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="AudioCount" SortExpression="AudioCount" HeaderText="Audios Created"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="CollectionCount" SortExpression="CollectionCount" HeaderText="Maps Created"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="CommentCount" SortExpression="CommentCount" HeaderText="Comments Created"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="EventCount" SortExpression="EventCount" HeaderText="Events Created"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="FileCount" SortExpression="FileCount" HeaderText="Files Created"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="FriendRequestedCount" SortExpression="FriendRequestedCount" HeaderText="Friend Requests Made"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="GroupCount" SortExpression="GroupCount" HeaderText="Groups Created"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="PictureCount" SortExpression="PictureCount" HeaderText="Pictures Created"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="UserCount" SortExpression="UserCount" HeaderText="Users Created"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="VideoCount" SortExpression="VideoCount" HeaderText="Videos Created"></asp:BoundField>
            </Fields>
        </asp:detailsview>
        <br />
        <asp:table id="_specialActivityTable" runat="server" CellSpacing="2" Caption="Special Groups/Events" Width="250px"></asp:table>
         <br />
        <asp:Button ID="_getActivityButton" runat="server" Text="Get Activity" OnClick="_getActivityButton_Click" /><br />
        <asp:objectdatasource id="_statsActivityDataSource" runat="server" selectmethod="GetStatistics"
            typename="WLQuickApps.SocialNetwork.Business.StatisticsManager" OnSelecting="_statsActivityDataSource_Selecting">
            <SelectParameters>
                <asp:Parameter Type="DateTime" Name="startDateTime"></asp:Parameter>
                <asp:Parameter Type="DateTime" Name="endDateTime"></asp:Parameter>
            </SelectParameters>
        </asp:objectdatasource>
    </cc:DropShadowPanel>
    <br />
    <br />
    <cc:DropShadowPanel runat="server" ID="DropShadowPanel1">
        <asp:label id="Label1" runat="server" font-size="Medium" text="Current Totals"></asp:label>
        <br />
        <br />
        <asp:detailsview id="_statsTotalsDetailsView" runat="server" autogeneraterows="False" datasourceid="_statsTotalsDataSource" CellSpacing="2" Width="250px">
            <RowStyle HorizontalAlign="Right"></RowStyle>
            <FieldHeaderStyle HorizontalAlign="Left"></FieldHeaderStyle>
            <Fields>
                <asp:BoundField ReadOnly="True" DataField="AlbumCount" SortExpression="AlbumCount" HeaderText="Albums"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="AudioCount" SortExpression="AudioCount" HeaderText="Audios"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="CollectionCount" SortExpression="CollectionCount" HeaderText="Maps"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="CommentCount" SortExpression="CommentCount" HeaderText="Comments"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="EventCount" SortExpression="EventCount" HeaderText="Events"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="FileCount" SortExpression="FileCount" HeaderText="Files"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="FriendConfirmedCount" SortExpression="FriendConfirmedCount" HeaderText="Friend Requests Confirmed"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="FriendRequestedCount" SortExpression="FriendRequestedCount" HeaderText="Friend Requests Made"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="GroupCount" SortExpression="GroupCount" HeaderText="Groups"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="PictureCount" SortExpression="PictureCount" HeaderText="Pictures"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="RatingCount" SortExpression="RatingCount" HeaderText="Ratings Made"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="TagCount" SortExpression="TagCount" HeaderText="Tags"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="UserCount" SortExpression="UserCount" HeaderText="Users"></asp:BoundField>
                <asp:BoundField ReadOnly="True" DataField="VideoCount" SortExpression="VideoCount" HeaderText="Videos"></asp:BoundField>
            </Fields>
        </asp:detailsview>
        <br />
        <asp:table id="_specialTotalsTable" runat="server" CellSpacing="2" Caption="Special Groups/Events" Width="250px"></asp:table>
        <br />
        <asp:Button ID="_getTotalsButton" runat="server" Text="Refresh Totals" OnClick="_getTotalsButton_Click" CausesValidation="False" />
        <asp:objectdatasource id="_statsTotalsDataSource" runat="server" selectmethod="GetStatistics"
            typename="WLQuickApps.SocialNetwork.Business.StatisticsManager"></asp:objectdatasource>    
    </cc:DropShadowPanel>
    <br />
</asp:content>