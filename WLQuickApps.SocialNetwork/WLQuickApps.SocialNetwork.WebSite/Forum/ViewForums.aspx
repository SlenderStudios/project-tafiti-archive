<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewForums.aspx.cs" Inherits="Forum_ViewForums" Title="Untitled Page" %>

<asp:Content ID="_mainContent" ContentPlaceHolderID="MainContent" runat="Server">
    
    <cc:DropShadowPanel runat="server" ID="_forumsPanel">
        <cc:DropShadowPanel runat="server" ID="_titlePanel" SkinID="ImageGallery-title">
            General
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_forumsGallery" runat="server" DataSourceID="_generalForumsDataSource" DataKeyField="BaseItemID"
            AllowPaging="True" PageSize="12" RepeatColumns="1" ViewMode="Text" EmptyDataText="This forum doesn't have any discussions yet." />
        <asp:ObjectDataSource ID="_generalForumsDataSource" runat="server" SelectMethod="GetForumsByTopic"
                    TypeName="WLQuickApps.SocialNetwork.Business.ForumManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" SelectCountMethod="GetForumsByTopicCount">
                    <SelectParameters>
                        <asp:Parameter Name="topic" Type="String" DefaultValue="" />
                    </SelectParameters>
         </asp:ObjectDataSource>
    </cc:DropShadowPanel>
    
    <asp:DataList runat="server" ID="_specialForumTopics" OnLoad="_specialForumTopics_Load">
        <ItemTemplate>
            <cc:DropShadowPanel runat="server" ID="DropShadowPanel1">
                <cc:DropShadowPanel runat="server" ID="DropShadowPanel2" SkinID="ImageGallery-title">
                    <asp:Label runat="server" ID="_specialTopicLabel" Text='<%# (string) Container.DataItem %>' />
                </cc:DropShadowPanel>
                
                <cc:MetaGallery ID="MetaGallery1" runat="server" DataSourceID="_repeatedForumsDataSource" DataKeyField="BaseItemID"
                    AllowPaging="True" PageSize="12" RepeatColumns="3" ViewMode="Text" EmptyDataText="This forum doesn't have any discussions yet." />

            </cc:DropShadowPanel>
                <asp:ObjectDataSource ID="_repeatedForumsDataSource" runat="server" SelectMethod="GetForumsByTopic"
                    TypeName="WLQuickApps.SocialNetwork.Business.ForumManager" EnablePaging="True" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" SelectCountMethod="GetForumsByTopicCount">
                    <SelectParameters>
                        <asp:ControlParameter Name="topic" ControlID="_specialTopicLabel" PropertyName="Text" />
                    </SelectParameters>
                </asp:ObjectDataSource>
        </ItemTemplate>
    </asp:DataList>


    <asp:HyperLink ID="_createForumLink" SkinID="ActionLink" runat="server" NavigateUrl="~/Forum/CreateForum.aspx">Create Discussion &gt;&gt;</asp:HyperLink>

</asp:Content>