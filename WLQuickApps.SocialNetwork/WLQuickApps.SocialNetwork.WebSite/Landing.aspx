<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Landing.aspx.cs" Inherits="Register" Title="Welcome!" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="_updatePanel" runat="server">
        <ContentTemplate>
            <div style="width:180px;float:left">
             <cc:DropShadowPanel ID="DropShadowPanel9" runat="server" SkinID="Landing-Left">
                <h3>New Users</h3>
                <cc:MetaGallery runat="server" ID="_mostRecentUsersGallery" DataSourceID="_mostRecentUsersDataSource" 
                    EmptyDataText="There are no recent users." ViewMode="User" RepeatColumns="1" PageSize="3" AllowPaging="true" />
                <asp:ObjectDataSource runat="server" ID="_mostRecentUsersDataSource" TypeName="WLQuickApps.SocialNetwork.Business.UserManager" 
                    SelectMethod="GetMostRecentUsers" SelectCountMethod="GetMostRecentUsersCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true" />
             </cc:DropShadowPanel>
             <cc:DropShadowPanel ID="DropShadowPanel3" runat="server" SkinID="Landing-Left">
                <h3>Today's Events</h3>
                <cc:MetaGallery runat="server" ID="_todaysEventsGallery" DataSourceID="_todaysEventsDataSource"
                    EmptyDataText="There are no upcoming events today." ViewMode="Event" RepeatColumns="1" AllowPaging="true" PageSize="3" />
                <asp:ObjectDataSource runat="server" ID="_todaysEventsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.EventManager"
                    SelectMethod="GetUpcomingEventsForToday" SelectCountMethod="GetUpcomingEventsForTodayCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true" />
             </cc:DropShadowPanel>
            </div>
             <cc:DropShadowPanel ID="DropShadowPanel1" runat="server" SkinID="Landing-Right">
                <h3>Recently Added Pictures</h3>
                <cc:MetaGallery runat="server" ID="_mostRecentPicturesGallery" RepeatColumns="3" ViewMode="Media"
                    DataSourceID="_mostRecentPicturesDataSource" EmptyDataText="No pictures have been added." AllowPaging="true" PageSize="3" />
                <asp:ObjectDataSource runat="server" ID="_mostRecentPicturesDataSource" TypeName="WLQuickApps.SocialNetwork.Business.MediaManager"
                    SelectMethod="GetMostRecentMedia" SelectCountMethod="GetMostRecentMediaCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:Parameter Name="mediaType" Type="Object" DefaultValue="Picture" />
                    </SelectParameters>
                </asp:ObjectDataSource>
             </cc:DropShadowPanel>
             <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="Landing-Right">
                <h3>Recently Added Maps</h3>
                <cc:MetaGallery runat="server" ID="_mostRecentCollectionsGallery" RepeatColumns="3" ViewMode="List"
                    DataSourceID="_mostRecentCollectionsDataSource" EmptyDataText="No maps have been added." AllowPaging="true" PageSize="3" />
                <asp:ObjectDataSource runat="server" ID="_mostRecentCollectionsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.CollectionManager"
                    SelectMethod="GetMostRecentCollections" SelectCountMethod="GetMostRecentCollectionsCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:Parameter Name="collectionType" Type="String" DefaultValue="" />
                    </SelectParameters>
                </asp:ObjectDataSource>
             </cc:DropShadowPanel>
             <cc:DropShadowPanel ID="DropShadowPanel5" runat="server" SkinID="Landing-Right">
                <h3>Recently Added Videos</h3>
                <cc:MetaGallery runat="server" ID="_mostRecentVideosGallery" DataSourceID="_mostRecentVideosDataSource" ViewMode="Media"
                    EmptyDataText="No videos have been added." RepeatColumns="3" AllowPaging="true" PageSize="3" />
                <asp:ObjectDataSource runat="server" ID="_mostRecentVideosDataSource" TypeName="WLQuickApps.SocialNetwork.Business.MediaManager"
                    SelectMethod="GetMostRecentMedia" SelectCountMethod="GetMostRecentMediaCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:Parameter Name="mediaType" Type="Object" DefaultValue="Video" />
                    </SelectParameters>
                </asp:ObjectDataSource>
             </cc:DropShadowPanel>
             <cc:DropShadowPanel ID="DropShadowPanel7" runat="server" SkinID="Landing-Right">
                <h3>Recently Added Audio</h3>
                <cc:MetaGallery runat="server" ID="_mostRecentAudioGallery" DataSourceID="_mostRecentAudioDataSource" ViewMode="Media"
                    EmptyDataText="No audio files have been added." RepeatColumns="3" AllowPaging="true" PageSize="3" />
                <asp:ObjectDataSource runat="server" ID="_mostRecentAudioDataSource" TypeName="WLQuickApps.SocialNetwork.Business.MediaManager"
                    SelectMethod="GetMostRecentMedia" SelectCountMethod="GetMostRecentMediaCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true">
                    <SelectParameters>
                        <asp:Parameter Name="mediaType" Type="Object" DefaultValue="Audio" />
                    </SelectParameters>
                </asp:ObjectDataSource>
             </cc:DropShadowPanel>
             <div class="clearFloats"></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
