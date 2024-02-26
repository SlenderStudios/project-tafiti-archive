<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewForum.aspx.cs" Inherits="Forum_ViewForum" Title="Untitled Page" %>
<%@ Register Src="../Controls/CommentsControl.ascx" TagName="CommentsControl" TagPrefix="uc" %>
<%@ Register Src="../Controls/MediaRating.ascx" TagName="MediaRating" TagPrefix="uc" %>
<%@ Register Src="../Controls/Tags.ascx" TagName="Tags" TagPrefix="uc" %>

<asp:Content ID="_mainContent" ContentPlaceHolderID="MainContent" runat="Server">
    
    <asp:FormView ID="_forumFormView" runat="server" DataSourceID="_forumDataSource">
        <ItemTemplate>
            <cc:DropShadowPanel ID="DropShadowPanel1" runat="server" >
                <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ImageGallery-title">
                    <asp:Label ID="_captionLabel" runat="server" Text='<%# Bind("Title") %>' />
                </cc:DropShadowPanel>
                <cc:DropShadowPanel ID="DropShadowPanel3" runat="server" SkinID="ImageGallery-titleRight">
                    <strong>begun by</strong>
                    <asp:HyperLink ID="_userHyperLink" runat="server" NavigateUrl='<%# WebUtilities.GetViewItemUrl(((Forum)Container.DataItem).Owner) %>'
                        Text='<%# Eval("Owner.Title", "{0}") %>' /><br />
                    back to
                    <asp:HyperLink ID="_forumsHyperLink" runat="server" NavigateUrl="~/Forum/ViewForums.aspx" Text="Forums" /><br />
                    <asp:LinkButton runat="server" ID="_deleteDiscussion" CommandName="DeleteDiscussion" CommandArgument='<%# ((BaseItem) Container.DataItem).BaseItemID %>'
                            Visible='<%# ((BaseItem) Container.DataItem).CanDelete %>'
                            OnCommand="_deleteDiscussion_Command" SkinID="comment-delete" CausesValidation="False">Delete</asp:LinkButton>
                </cc:DropShadowPanel>
                <br />
                From <asp:HyperLink ID="_topicLink" runat="server" NavigateUrl="~/Forum/ViewForums.aspx" 
                Text='<%# ((BaseItem)Container.DataItem).Description.Length > 0 ? ((BaseItem)Container.DataItem).Description : "General" %>' />
                <br />
                <div class="clearFloats"></div>
            </cc:DropShadowPanel>
            <cc:DropShadowPanel ID="DropShadowPanel8" runat="server" >
                <uc:CommentsControl ID="_viewCommentsControl" runat="server" BaseItemID='<%# Bind("BaseItemID") %>' OldestCommentsFirst="True" />
            </cc:DropShadowPanel>
        </ItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="_forumDataSource" runat="server" SelectMethod="GetForum"
        TypeName="WLQuickApps.SocialNetwork.Business.ForumManager">
        <SelectParameters>
            <asp:QueryStringParameter Name="baseItemID" QueryStringField="baseItemID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>