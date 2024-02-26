<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewMedia.aspx.cs" Inherits="Media_ViewMedia" Title="View Media" %>
<%@ Register Src="../Controls/CommentsControl.ascx" TagName="CommentsControl" TagPrefix="uc" %>
<%@ Register Src="../Controls/MediaRating.ascx" TagName="MediaRating" TagPrefix="uc" %>
<%@ Register Src="../Controls/Tags.ascx" TagName="Tags" TagPrefix="uc" %>
<%@ Register Src="../Controls/LocationLinkControl.ascx" TagName="LocationLinkControl" TagPrefix="uc" %>

<asp:Content ID="_mainContent" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript" src="http://agappdom.net/h/silverlight.js"></script>
    <script type="text/javascript" src="CreateSilverlight.js"></script>
    
    <asp:FormView ID="_mediaFormView" runat="server" DataSourceID="_mediaDataSource">
        <ItemTemplate>
            <cc:DropShadowPanel ID="DropShadowPanel1" runat="server" >
                <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ImageGallery-title">
                    <asp:Label ID="_captionLabel" runat="server" Text='<%# Bind("Title") %>' />
                </cc:DropShadowPanel>
                <cc:DropShadowPanel ID="DropShadowPanel3" runat="server" SkinID="ImageGallery-titleRight">
                    <strong>belongs to</strong>
                    <asp:HyperLink ID="_userHyperLink" runat="server" NavigateUrl='<%# WebUtilities.GetViewItemUrl(((BaseItem)Container.DataItem).Owner) %>'
                        Text='<%# Eval("ParentAlbum.Owner.Title", "{0}") %>' />
                </cc:DropShadowPanel>
                <asp:Panel runat="server" CssClass="ViewMedia-Image">
                    <asp:Image ID="_pictureImage" runat="server" ImageUrl='<%# WebUtilities.GetViewImageUrl(((BaseItem)Container.DataItem).BaseItemID, 670, 600) %>' Visible='<%# ((Media)Container.DataItem).MediaType == MediaType.Picture %>' />
                    <asp:Panel runat="server" ID="_mediaWrapperDivLiteral" Visible='<%# (((Media)Container.DataItem).MediaType == MediaType.Audio) || (((Media)Container.DataItem).MediaType == MediaType.Video) %>'>
                        <asp:Literal runat="server" ID="_mediaLiteral" Text='<%# string.Format("<div id=\"MediaWrapper\" style=\"width:500px; height:{0}px; overflow:hidden;\"></div>", (((Media)Container.DataItem).MediaType == MediaType.Audio) ? "50" : "400") %>' />
                        <asp:Literal ID="_literal" runat="server" Text='<%# string.Format("<script type=\"text/javascript\">CreateSilverlight(\"{0}\", \"{1}\");</script>", Utilities.GenerateSilverlightID((int) Eval("BaseItemID")), SettingsWrapper.SilverlightStreamingUserName) %>' />
                    </asp:Panel>
                    <br />
                </asp:Panel>
                <br />
                <asp:Label ID="_descriptionLabel" runat="server" Text='<%# Bind("Description") %>' /><br />
                <cc:DropShadowPanel ID="DropShadowPanel5" runat="server" SkinID="MediaDescription">
                    <strong>from</strong>
                    <asp:HyperLink ID="_albumHyperLink" runat="server" NavigateUrl='<%# WebUtilities.GetViewItemUrl(((Media)Container.DataItem).ParentAlbum) %>'
                        Text='<%# Eval("ParentAlbum.Title", "{0}") %>' /><br />
                    <strong>posted</strong>
                    <asp:Label ID="_postDateTimeLabel" runat="server" Text='<%# Eval("CreateDate", "{0:D} at {0:t}") %>' /><br />
                    <strong>at</strong>
                    <uc:LocationLinkControl ID="LocationLinkControl1" runat="server" LocationItem='<%# Bind("Location") %>' ShowLocationCaption="True" /><br />
                    <cc:DropShadowPanel ID="DropShadowPanel6" runat="server" SkinID="indentedSection">
                        <uc:MediaRating ID="_rating" runat="server" MediaID='<%# Bind("BaseItemID") %>' /><br /><br />
                    </cc:DropShadowPanel>
                    <strong>tagged with</strong>
                    <uc:Tags runat="server" ID="_tags" BaseItemID='<%# Eval("BaseItemID") %>' /><br />
                </cc:DropShadowPanel>
                <br />

                <cc:DropShadowPanel ID="DropShadowPanel7" runat="server" SkinID="Media-ActionsBox" Visible='<%# MediaManager.CanModifyMedia(((Media) Container.DataItem).BaseItemID) %>'>
                    <strong>
                        <asp:LinkButton runat="server" ID="_setAsProfilePicture" OnClick="_setAsProfilePicture_Click" Text="Set as profile picture<br />"
                            Visible='<%# (((Media) Container.DataItem).MediaType == MediaType.Picture) && MediaManager.CanModifyMedia(((Media) Container.DataItem).BaseItemID) %>' />
                        <asp:LinkButton runat="server" ID="_setAsAlbumThumbnail" OnClick="_setAsAlbumThumbnail_Click" Text="Set as album thumbnail<br />"
                            Visible='<%# (((Media) Container.DataItem).MediaType == MediaType.Picture) && MediaManager.CanModifyMedia(((Media) Container.DataItem).BaseItemID) %>' />
                        <asp:LinkButton runat="server" ID="_deleteMedia" CommandName="DeleteMedia" CommandArgument='<%# Bind("BaseItemID") %>'
                            Visible='<%# MediaManager.CanModifyMedia(((Media) Container.DataItem).BaseItemID) %>'
                            OnCommand="_deleteMedia_Command">Delete</asp:LinkButton>
                            <ajaxToolkit:ConfirmButtonExtender ID="_deleteConfirm" runat="server" 
                            TargetControlID="_deleteMedia"
                            ConfirmText='<%# string.Format("Are you sure you want to delete this {0}?", ((Media) Container.DataItem).MediaType.ToString()) %>' /><br />
                        <asp:HyperLink ID="_editHyperLink" runat="server" NavigateUrl='<%# string.Format("~/Media/EditMedia.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, Eval("BaseItemID")) %>'
                            Text="Edit" Visible="<%# MediaManager.CanModifyMedia(((Media) Container.DataItem).BaseItemID) %>" /><br />
                        <asp:LinkButton runat="server" ID="_approveMedia" CommandName="ApproveMedia" CommandArgument='<%# Bind("BaseItemID") %>'
                            Visible='<%# ((Media) Container.DataItem).CanApprove %>'
                            OnCommand="_approveMedia_Command">Approve</asp:LinkButton>
                            <ajaxToolkit:ConfirmButtonExtender ID="_approveConfirm" runat="server" 
                            TargetControlID="_approveMedia"
                            ConfirmText='<%# string.Format("Are you sure you want to approve this {0}?", ((Media) Container.DataItem).MediaType.ToString()) %>' />
                    </strong>
                </cc:DropShadowPanel>
                <div class="clearFloats"></div>
            </cc:DropShadowPanel>
            <cc:DropShadowPanel ID="DropShadowPanel8" runat="server" >
                <h3>Comments</h3>
                <uc:CommentsControl ID="_viewCommentsControl" runat="server" BaseItemID='<%# Bind("BaseItemID") %>' OldestCommentsFirst="True" />
            </cc:DropShadowPanel>
        </ItemTemplate>
    </asp:FormView>
    <asp:ObjectDataSource ID="_mediaDataSource" runat="server" SelectMethod="GetMedia"
        TypeName="WLQuickApps.SocialNetwork.Business.MediaManager">
        <SelectParameters>
            <asp:QueryStringParameter Name="baseItemID" QueryStringField="baseItemID" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>