<%@ Control Language="C#" AutoEventWireup="True" CodeFile="MetaGalleryElement.ascx.cs" Inherits="MetaGalleryElement" %>
<%@ Register Src="../Controls/LocationLinkControl.ascx" TagName="LocationLinkControl" TagPrefix="uc" %>
<%@ Register Src="../Controls/MediaRating.ascx" TagName="MediaRating" TagPrefix="uc" %>

<asp:MultiView runat="server" ID="_elementView">
    <asp:View runat="server" ID="_squareView">
        <asp:FormView runat="server" ID="_squareViewForm">
            <ItemTemplate>
                <cc:DropShadowPanel runat="server" SkinID="ImageGallery-imagePanel">
                    <asp:HyperLink ID="_nameHyperLink" runat="server" SkinID="ImageGallery-label" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>'
                        NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' />
                    <cc:NullablePicture ID="_nullablePictureControl" runat="server" MaxHeight="128" MaxWidth="128"
                        Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                        NullImageUrl='<%# string.Format("~/Images/missing-{0}-128x128.png", this.GetItemType().ToLower()) %>' />
                </cc:DropShadowPanel>

                <asp:Panel runat="server" ID="_actionsPanel" SkinID="ImageGallery-toolRight" Visible='<%# UserManager.IsUserLoggedIn() %>'>
                    <asp:LinkButton runat="server" ID="_deleteLink" ToolTip="Delete" SkinID="ImageGallery-IconDelete" OnClick="_delete_Click" Visible='<%# ((BaseItem)Container.DataItem).CanDelete %>' />
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteLink"
                        ConfirmText='<%# String.Format("Are you sure you want to delete this {0}?", this.GetItemType()) %>' />
                        
                    <asp:LinkButton runat="server" ID="_rejectLink" ToolTip="Reject" SkinID="ImageGallery-IconReject" OnClick="_reject_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                    <asp:LinkButton runat="server" ID="_approveLink" ToolTip="Approve" SkinID="ImageGallery-IconApprove" OnClick="_approve_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                    <asp:LinkButton runat="server" ID="_cancelLink" ToolTip="Cancel" SkinID="ImageGallery-IconCancel" OnClick="_cancel_Click" Visible='<%# ((BaseItem)Container.DataItem).CanCancel %>' />

                    <asp:LinkButton runat="server" ID="_editLink" ToolTip="Edit" SkinID="ImageGallery-IconEdit" OnClick="_edit_Click" Visible='<%# ((BaseItem)Container.DataItem).CanEdit && !((BaseItem)Container.DataItem).CanAccept %>' />                

                    <asp:LinkButton runat="server" ID="_joinLink" ToolTip="Join" SkinID="ImageGallery-IconJoin"
                        OnClick="_join_Click" Visible='<%# ((BaseItem)Container.DataItem).CanJoin %>' OnPreRender="_joinLink_PreRender" />
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_joinConfirm" TargetControlID="_joinLink"
                        ConfirmText='<%# String.Format("Are you sure you want to join this {0}?", this.GetItemType().ToLower()) %>'
                        OnPreRender="_joinConfirm_PreRender" />   
                    <asp:LinkButton runat="server" ID="_leaveLink" ToolTip="Leave" SkinID="ImageGallery-IconLeave"
                        OnClick="_leave_Click" Visible='<%# ((BaseItem)Container.DataItem).CanLeave %>' OnPreRender="_leaveLink_PreRender" />
                    <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_leaveConfirm" TargetControlID="_leaveLink"
                        ConfirmText='<%# String.Format("Are you sure you want to leave this {0}?", this.GetItemType().ToLower()) %>'
                        OnPreRender="_leaveConfirm_PreRender" />
                </asp:Panel>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>

    <asp:View runat="server" ID="_listView">
        <asp:FormView runat="server" ID="_listViewForm">
            <ItemTemplate>
                <asp:Panel runat="server" ID="_listViewPanel" CssClass="Item-Panel">
                    <cc:NullablePicture runat="server" ID="_nullablePicture" MaxHeight="128" MaxWidth="128" CssClass="Item-Picture"
                        Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                        NullImageUrl='<%# string.Format("~/Images/missing-{0}-128x128.png", this.GetItemType().ToLower()) %>' />
                    <asp:Panel runat="server" ID="_listViewRight" CssClass="Item-DetailsPanel">
                        <asp:HyperLink runat="server" ID="_nameHyperLink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' CssClass="itemLabel" /><br />
                        <strong><asp:Label ID="_eventType" runat="server" Text='<%# string.Format("({0})", ((BaseItem)Container.DataItem).SubType) %>' 
                                Visible='<%# (((BaseItem)Container.DataItem).SubType.Length > 0) && ((Container.DataItem as WLQuickApps.SocialNetwork.Business.Group) != null) %>' /></strong>                        
                        <asp:Panel runat="server" CssClass="Item-Details">                        
                        <asp:LinkButton runat="server" ID="_deleteLink" CssClass="metagallery-link" Text="Delete" ToolTip='<%# string.Format("Delete {0}", this.GetItemType()) %>' OnClick="_delete_Click" Visible='<%# ((BaseItem)Container.DataItem).CanDelete %>' />
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteLink"
                            ConfirmText='<%# String.Format("Are you sure you want to delete this {0}?", this.GetItemType().ToLower()) %>' />
                        
                        <%--<asp:LinkButton runat="server" ID="_rejectLink" SkinID="ImageGallery-IconReject" ToolTip="Reject Invitation" OnClick="_reject_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                        <asp:LinkButton runat="server" ID="_approveLink" SkinID="ImageGallery-IconApprove" ToolTip="Approve Invitation" OnClick="_approve_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                        <asp:LinkButton runat="server" ID="_cancelLink" SkinID="ImageGallery-IconCancel" ToolTip="Cancel Request" OnClick="_cancel_Click" Visible='<%# ((BaseItem)Container.DataItem).CanCancel %>' />
--%>
                        <asp:LinkButton runat="server" ID="_editLink" CssClass="metagallery-link" Text="Edit" ToolTip='<%# string.Format("Edit {0}", this.GetItemType()) %>' OnClick="_edit_Click" Visible='<%# ((BaseItem)Container.DataItem).CanEdit %>' />                

<%--                        <asp:LinkButton runat="server" ID="_joinLink" SkinID="ImageGallery-IconJoin" ToolTip='<%# string.Format("Join {0}", this.GetItemType()) %>'
                            OnClick="_join_Click" Visible='<%# ((BaseItem)Container.DataItem).CanJoin %>' OnPreRender="_joinLink_PreRender" />
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_joinConfirm" TargetControlID="_joinLink"
                            ConfirmText='<%# String.Format("Are you sure you want to join this {0}?", this.GetItemType().ToLower()) %>'
                            OnPreRender="_joinConfirm_PreRender" />    
                        <asp:LinkButton runat="server" ID="_leaveLink" SkinID="ImageGallery-IconLeave" ToolTip='<%# string.Format("Leave {0}", this.GetItemType()) %>'
                            OnClick="_leave_Click" Visible='<%# ((BaseItem)Container.DataItem).CanLeave %>' OnPreRender="_leaveLink_PreRender" />
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_leaveConfirm" TargetControlID="_leaveLink"
                            ConfirmText='<%# String.Format("Are you sure you want to leave this {0}?", this.GetItemType().ToLower()) %>'
                            OnPreRender="_leaveConfirm_PreRender" />--%>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>
    
    <asp:View runat="server" ID="_iconView">
        <asp:FormView runat="server" ID="_iconViewForm">
            <ItemTemplate>
                <asp:Panel runat="server" ID="_iconViewPanel" SkinID="ImageGallery-IconWrapperPanel">
                    <cc:NullablePicture runat="server" ID="_nullablePicture" MaxHeight="32" MaxWidth="32" SkinID="ImageGallery-IconPicture"
                        Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                        NullImageUrl='<%# string.Format("~/Images/missing-{0}-32x32.png", this.GetItemType().ToLower()) %>' />
                    <asp:Panel runat="server" ID="_labelPanel" SkinID="ImageGallery-IconPanel">
                        <asp:HyperLink runat="server" ID="_nameHyperLink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' SkinID="ImageGallery-IconLabel" />
                        <strong><asp:Label ID="_eventType" runat="server" Text='<%# string.Format("({0})", ((BaseItem)Container.DataItem).SubType) %>' 
                            Visible='<%# (((BaseItem)Container.DataItem).SubType.Length > 0) && ((Container.DataItem as WLQuickApps.SocialNetwork.Business.Group) != null) %>' /></strong>
                        <asp:LinkButton runat="server" ID="_deleteLink" SkinID="ImageGallery-IconDelete" ToolTip='<%# string.Format("Delete {0}", this.GetItemType()) %>' OnClick="_delete_Click" Visible='<%# ((BaseItem)Container.DataItem).CanDelete %>' />
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteLink"
                            ConfirmText='<%# String.Format("Are you sure you want to delete this {0}?", this.GetItemType().ToLower()) %>' />
                        
                        <asp:LinkButton runat="server" ID="_rejectLink" SkinID="ImageGallery-IconReject" ToolTip="Reject Invitation" OnClick="_reject_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                        <asp:LinkButton runat="server" ID="_approveLink" SkinID="ImageGallery-IconApprove" ToolTip="Approve Invitation" OnClick="_approve_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                        <asp:LinkButton runat="server" ID="_cancelLink" SkinID="ImageGallery-IconCancel" ToolTip="Cancel Request" OnClick="_cancel_Click" Visible='<%# ((BaseItem)Container.DataItem).CanCancel %>' />

                        <asp:LinkButton runat="server" ID="_editLink" SkinID="ImageGallery-IconEdit" ToolTip='<%# string.Format("Edit {0}", this.GetItemType()) %>' OnClick="_edit_Click" Visible='<%# ((BaseItem)Container.DataItem).CanEdit %>' />                

                        <asp:LinkButton runat="server" ID="_joinLink" SkinID="ImageGallery-IconJoin" ToolTip='<%# string.Format("Join {0}", this.GetItemType()) %>'
                            OnClick="_join_Click" Visible='<%# ((BaseItem)Container.DataItem).CanJoin %>' OnPreRender="_joinLink_PreRender" />
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_joinConfirm" TargetControlID="_joinLink"
                            ConfirmText='<%# String.Format("Are you sure you want to join this {0}?", this.GetItemType().ToLower()) %>'
                            OnPreRender="_joinConfirm_PreRender" />    
                        <asp:LinkButton runat="server" ID="_leaveLink" SkinID="ImageGallery-IconLeave" ToolTip='<%# string.Format("Leave {0}", this.GetItemType()) %>'
                            OnClick="_leave_Click" Visible='<%# ((BaseItem)Container.DataItem).CanLeave %>' OnPreRender="_leaveLink_PreRender" />
                        <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_leaveConfirm" TargetControlID="_leaveLink"
                            ConfirmText='<%# String.Format("Are you sure you want to leave this {0}?", this.GetItemType().ToLower()) %>'
                            OnPreRender="_leaveConfirm_PreRender" />
                    </asp:Panel>
                </asp:Panel>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>
    
    <asp:View runat="server" ID="_albumView">
        <asp:FormView runat="server" ID="_albumViewForm">
            <ItemTemplate>
                <asp:Panel runat="server" ID="_eventViewPanel" CssClass="Item-Panel">
                    <cc:NullablePicture runat="server" ID="_nullablePicture" MaxHeight="128" MaxWidth="128" CssClass="Item-Picture"
                        Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                        NullImageUrl='<%# string.Format("~/Images/missing-{0}-128x128.png", this.GetItemType().ToLower()) %>' />
                    <asp:Panel runat="server" ID="_labelPanel" CssClass="Item-DetailsPanel">
                        <asp:HyperLink runat="server" ID="_nameHyperLink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' CssClass="itemLabel" />
                        <asp:Label ID="_eventType" runat="server" Text='<%# string.Format("({0})", ((BaseItem)Container.DataItem).SubType) %>' 
                            Visible='<%# (((BaseItem)Container.DataItem).SubType.Length > 0) && ((Container.DataItem as WLQuickApps.SocialNetwork.Business.Group) != null) %>'/>       
                        <asp:Panel runat="server" CssClass="Item-Details">
                            <strong>contains</strong> <asp:Label runat="server" Text='<%# MediaManager.GetMediaCountByAlbum(((Album)Container.DataItem).BaseItemID) %>' /> item(s)<br />

                            <asp:LinkButton runat="server" ID="_deleteLink" CssClass="metagallery-link" Text="Delete" ToolTip='<%# string.Format("Delete {0}", this.GetItemType()) %>' OnClick="_delete_Click" Visible='<%# ((BaseItem)Container.DataItem).CanDelete %>' />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteLink"
                                ConfirmText='<%# String.Format("Warning!!! \n Are you sure you want to delete this {0}? \n if this album is connected to Windows Live Spaces then it will be deleted from Windows Live Spaces", this.GetItemType().ToLower()) %>' />
                            
<%--                            <asp:LinkButton runat="server" ID="_rejectLink" SkinID="ImageGallery-IconReject" ToolTip="Reject Invitation" OnClick="_reject_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_approveLink" SkinID="ImageGallery-IconApprove" ToolTip="Approve Invitation" OnClick="_approve_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_cancelLink" SkinID="ImageGallery-IconCancel" ToolTip="Cancel Request" OnClick="_cancel_Click" Visible='<%# ((BaseItem)Container.DataItem).CanCancel %>' />--%>

                            <asp:LinkButton runat="server" ID="_editLink" CssClass="metagallery-link" Text="Edit"  ToolTip='<%# string.Format("Edit {0}", this.GetItemType()) %>' OnClick="_edit_Click" Visible='<%# ((BaseItem)Container.DataItem).CanEdit %>' />                

<%--                            <asp:LinkButton runat="server" ID="_joinLink" SkinID="ImageGallery-IconJoin" ToolTip='<%# string.Format("Join {0}", this.GetItemType()) %>'
                                OnClick="_join_Click" Visible='<%# ((BaseItem)Container.DataItem).CanJoin %>' OnPreRender="_joinLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_joinConfirm" TargetControlID="_joinLink"
                                ConfirmText='<%# String.Format("Are you sure you want to join this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_joinConfirm_PreRender" />    
                            <asp:LinkButton runat="server" ID="_leaveLink" SkinID="ImageGallery-IconLeave" ToolTip='<%# string.Format("Leave {0}", this.GetItemType()) %>'
                                OnClick="_leave_Click" Visible='<%# ((BaseItem)Container.DataItem).CanLeave %>' OnPreRender="_leaveLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_leaveConfirm" TargetControlID="_leaveLink"
                                ConfirmText='<%# String.Format("Are you sure you want to leave this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_leaveConfirm_PreRender" />--%>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>
    
    <asp:View runat="server" ID="_eventView">
        <asp:FormView runat="server" ID="_eventViewForm">
            <ItemTemplate>
                <asp:Panel runat="server" ID="_eventViewPanel" CssClass="Item-Panel">
                    <cc:NullablePicture runat="server" ID="_nullablePicture" MaxHeight="128" MaxWidth="128" SkinID="Item-Picture"
                        Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                        NullImageUrl='<%# string.Format("~/Images/missing-{0}-128x128.png", this.GetItemType().ToLower()) %>' />
                    <asp:Panel runat="server" ID="_labelPanel" CssClass="Item-DetailsPanel">
                        <asp:HyperLink runat="server" ID="_nameHyperLink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' CssClass="itemLabel" />

                        <asp:Panel runat="server" CssClass="Item-Details">
                            <strong>Hosted by:</strong> <asp:HyperLink ID="_creatorLink" SkinID="metagallerylink" runat="server" Text='<%# ((Event)Container.DataItem).Owner.Title %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl(((Event)Container.DataItem).Owner) %>' />                        
                            <strong><asp:Label ID="Label2" runat="server" Text='<%# ((BaseItem)Container.DataItem).SubType %>' Visible='<%# (((BaseItem)Container.DataItem).SubType.Length > 0) %>' /></strong>
                            <asp:Literal ID="_eventTypeLiteral" runat="server" Text="<br />" Visible='<%# (((BaseItem)Container.DataItem).SubType.Length > 0) %>' />
                            <br />
                            <strong>Date:</strong> <asp:Label runat="server" Text='<%# Eval("StartDateTime", "{0:d}") %>' /><br />
                            <strong>Time:</strong> <asp:Label ID="Label1" runat="server" Text='<%# Eval("StartDateTime", "{0:t}") %>' /><br />
                            <strong>Location:</strong> <uc:LocationLinkControl runat="server" ID="_location" LocationItem='<%# ((WLQuickApps.SocialNetwork.Business.Group)Container.DataItem).Location %>' ShowLocationCaption="True" /><br />
                            <asp:LinkButton runat="server" ID="_deleteLink" CssClass="metagallery-link" Text="Delete" ToolTip='<%# string.Format("Delete {0}", this.GetItemType()) %>' OnClick="_delete_Click" Visible='<%# ((BaseItem)Container.DataItem).CanDelete %>' />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteLink"
                                ConfirmText='<%# String.Format("Are you sure you want to delete this {0}?", this.GetItemType().ToLower()) %>' />
                            <asp:LinkButton runat="server" ID="_rejectLink" CssClass="metagallery-link" Text="Reject Invitation"  ToolTip="Reject Invitation" OnClick="_reject_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_approveLink" CssClass="metagallery-link" Text="Approve Invitation" ToolTip="Approve Invitation" OnClick="_approve_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_cancelLink" CssClass="metagallery-link" Text="Cancel Request to Add Event" ToolTip="Cancel Request" OnClick="_cancel_Click" Visible='<%# ((BaseItem)Container.DataItem).CanCancel %>' />
                            <asp:LinkButton runat="server" ID="_editLink" CssClass="metagallery-link" Text="Edit" ToolTip='<%# string.Format("Edit {0}", this.GetItemType()) %>' OnClick="_edit_Click" Visible='<%# ((BaseItem)Container.DataItem).CanEdit %>' />                
                            <asp:LinkButton runat="server" ID="_joinLink" CssClass="metagallery-link" Text="Add to My Events" ToolTip='<%# string.Format("Add {0}", this.GetItemType()) %>'
                                OnClick="_join_Click" Visible='<%# ((BaseItem)Container.DataItem).CanJoin %>' OnPreRender="_joinLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_joinConfirm" TargetControlID="_joinLink"
                                ConfirmText='<%# String.Format("Are you sure you want to join this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_joinConfirm_PreRender" />    
                            <asp:LinkButton runat="server" ID="_leaveLink" CssClass="metagallery-link" Text="Remove from My Events" ToolTip='<%# string.Format("Leave {0}", this.GetItemType()) %>'
                                OnClick="_leave_Click" Visible='<%# ((BaseItem)Container.DataItem).CanLeave %>' OnPreRender="_leaveLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_leaveConfirm" TargetControlID="_leaveLink"
                                ConfirmText='<%# String.Format("Are you sure you want to leave this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_leaveConfirm_PreRender" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>
    
    <asp:View runat="server" ID="_groupView">
        <asp:FormView runat="server" ID="_groupViewForm">
            <ItemTemplate>
                <asp:Panel runat="server" ID="_eventViewPanel" CssClass="Item-Panel">
                    <cc:NullablePicture runat="server" ID="_nullablePicture" MaxHeight="128" MaxWidth="128" CssClass="Item-Picture"
                        Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                        NullImageUrl='<%# string.Format("~/Images/missing-{0}-128x128.png", this.GetItemType().ToLower()) %>' />
                    <asp:Panel runat="server" ID="_labelPanel" CssClass="Item-DetailsPanel">
                        <asp:HyperLink runat="server" ID="_nameHyperLink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' CssClass="itemLabel"/>
                            
                        <asp:Panel runat="server" CssClass="Item-Details">
                            <asp:Label runat="server" Text='<%# ((WLQuickApps.SocialNetwork.Business.Group)Container.DataItem).Users.Count %>' /> member(s)<br />
                            <asp:LinkButton runat="server" ID="_deleteLink" CssClass="metagallery-link" Text="Delete" ToolTip='<%# string.Format("Delete {0}", this.GetItemType()) %>' OnClick="_delete_Click" Visible='<%# ((BaseItem)Container.DataItem).CanDelete %>' />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteLink"
                                ConfirmText='<%# String.Format("Are you sure you want to delete this {0}?", this.GetItemType().ToLower()) %>' />
                            
                            <asp:LinkButton runat="server" ID="_rejectLink" CssClass="metagallery-link" Text="Reject Group Invitation" ToolTip="Reject Invitation" OnClick="_reject_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_approveLink"  CssClass="metagallery-link" Text="Accept Group Invitation" ToolTip="Accept Invitation" OnClick="_approve_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_cancelLink" CssClass="metagallery-link" Text="Cancel Request to Join Group" ToolTip="Cancel Request" OnClick="_cancel_Click" Visible='<%# ((BaseItem)Container.DataItem).CanCancel %>' />

                            <asp:LinkButton runat="server" ID="_editLink" CssClass="metagallery-link" Text="Edit" ToolTip='<%# string.Format("Edit {0}", this.GetItemType()) %>' OnClick="_edit_Click" Visible='<%# ((BaseItem)Container.DataItem).CanEdit %>' />                

                            <asp:LinkButton runat="server" ID="_joinLink" CssClass="metagallery-link" Text="Join Group" ToolTip='<%# string.Format("Join {0}", this.GetItemType()) %>'
                                OnClick="_join_Click" Visible='<%# ((BaseItem)Container.DataItem).CanJoin %>' OnPreRender="_joinLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_joinConfirm" TargetControlID="_joinLink"
                                ConfirmText='<%# String.Format("Are you sure you want to join this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_joinConfirm_PreRender" />    
                            <asp:LinkButton runat="server" ID="_leaveLink" CssClass="metagallery-link" Text="Leave Group" ToolTip='<%# string.Format("Leave {0}", this.GetItemType()) %>'
                                OnClick="_leave_Click" Visible='<%# ((BaseItem)Container.DataItem).CanLeave %>' OnPreRender="_leaveLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_leaveConfirm" TargetControlID="_leaveLink"
                                ConfirmText='<%# String.Format("Are you sure you want to leave this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_leaveConfirm_PreRender" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>
    
    <asp:View runat="server" ID="_mediaView">
        <asp:FormView runat="server" ID="_mediaViewForm">
            <ItemTemplate>
                <asp:Panel runat="server" ID="_eventViewPanel" CssClass="Item-Panel">
                    <cc:NullablePicture runat="server" ID="_nullablePicture" MaxHeight="128" MaxWidth="128" CssClass="Item-Picture"
                        Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                        NullImageUrl='<%# string.Format("~/Images/missing-{0}-128x128.png", this.GetItemType().ToLower()) %>' />
                    <asp:Panel runat="server" ID="_labelPanel" CssClass="Item-DetailsPanel">
                        <asp:HyperLink runat="server" ID="_nameHyperLink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' CssClass="itemLabel" />
                        <asp:Label ID="_eventType" runat="server" Text='<%# string.Format("({0})", ((BaseItem)Container.DataItem).SubType) %>' 
                            Visible='<%# (((BaseItem)Container.DataItem).SubType.Length > 0) && ((Container.DataItem as WLQuickApps.SocialNetwork.Business.Group) != null) %>' />
                            
                        <asp:Panel runat="server" CssClass="Item-Details">
                            <asp:Label runat="server" Text='<%# ((BaseItem)Container.DataItem).TotalViews %>' /> views<br />
                            <uc:MediaRating ID="_rating" runat="server" MediaID='<%# Bind("BaseItemID") %>' /><br />

                            <asp:LinkButton runat="server" ID="_deleteLink" CssClass="metagallery-link" Text="Delete" ToolTip='<%# string.Format("Delete {0}", this.GetItemType()) %>' OnClick="_delete_Click" Visible='<%# ((BaseItem)Container.DataItem).CanDelete %>' />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteLink"
                                ConfirmText='<%# String.Format(" Warning!!! \n Are you sure you want to delete this {0}?\n If this picture is connected to Windows Live Spaces then it will be deleted from Windows Live Spaces ", this.GetItemType().ToLower()) %>' />
                            
<%--                            <asp:LinkButton runat="server" ID="_rejectLink" SkinID="ImageGallery-IconReject" ToolTip="Reject Invitation" OnClick="_reject_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_approveLink" SkinID="ImageGallery-IconApprove" ToolTip="Approve Invitation" OnClick="_approve_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_cancelLink" SkinID="ImageGallery-IconCancel" ToolTip="Cancel Request" OnClick="_cancel_Click" Visible='<%# ((BaseItem)Container.DataItem).CanCancel %>' />--%>

                            <asp:LinkButton runat="server" ID="_editLink" CssClass="metagallery-link" Text="Edit" ToolTip='<%# string.Format("Edit {0}", this.GetItemType()) %>' OnClick="_edit_Click" Visible='<%# ((BaseItem)Container.DataItem).CanEdit %>' />                

<%--                            <asp:LinkButton runat="server" ID="_joinLink" SkinID="ImageGallery-IconJoin" ToolTip='<%# string.Format("Join {0}", this.GetItemType()) %>'
                                OnClick="_join_Click" Visible='<%# ((BaseItem)Container.DataItem).CanJoin %>' OnPreRender="_joinLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_joinConfirm" TargetControlID="_joinLink"
                                ConfirmText='<%# String.Format("Are you sure you want to join this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_joinConfirm_PreRender" />    
                            <asp:LinkButton runat="server" ID="_leaveLink" SkinID="ImageGallery-IconLeave" ToolTip='<%# string.Format("Leave {0}", this.GetItemType()) %>'
                                OnClick="_leave_Click" Visible='<%# ((BaseItem)Container.DataItem).CanLeave %>' OnPreRender="_leaveLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_leaveConfirm" TargetControlID="_leaveLink"
                                ConfirmText='<%# String.Format("Are you sure you want to leave this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_leaveConfirm_PreRender" />--%>
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>
    
    <asp:View runat="server" ID="_userView">
        <asp:FormView runat="server" ID="_userViewForm">
            <ItemTemplate>
                <asp:Panel runat="server" ID="_eventViewPanel" CssClass="Item-Panel">
                    <cc:NullablePicture runat="server" ID="_nullablePicture" MaxHeight="128" MaxWidth="128" CssClass="Item-Picture"
                        Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                        NullImageUrl='<%# string.Format("~/Images/missing-{0}-128x128.png", this.GetItemType().ToLower()) %>' />
                    <asp:Panel runat="server" ID="_labelPanel" CssClass="Item-DetailsPanel">
                        <asp:HyperLink runat="server" ID="_nameHyperLink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' CssClass="itemLabel" />
                        <asp:Label ID="_eventType" runat="server" Text='<%# string.Format("({0})", ((BaseItem)Container.DataItem).SubType) %>' 
                            Visible='<%# (((BaseItem)Container.DataItem).SubType.Length > 0) && ((Container.DataItem as WLQuickApps.SocialNetwork.Business.Group) != null) %>' />
                            
                        <asp:Panel runat="server" CssClass="Item-Details">
                            <!--<asp:Label runat="server" Text='<%# FriendManager.GetFriendsCountByUserID(((User)Container.DataItem).UserID) %>' /> friends<br /> -->
                            <strong>Last Login:</strong> <asp:Label ID="_lastLoginDateLabel" runat="server" Text='<%# this.FormatLastSeen(((User)Container.DataItem).LastLoginDate) %>' />                                                    
                            <asp:LinkButton runat="server" ID="_deleteLink" CssClass="metagallery-link" Text="Delete" ToolTip='<%# string.Format("Delete {0}", this.GetItemType()) %>' OnClick="_delete_Click" Visible='<%# ((BaseItem)Container.DataItem).CanDelete %>' />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_deleteConfirm" TargetControlID="_deleteLink"
                                ConfirmText='<%# String.Format("Are you sure you want to delete this {0}?", this.GetItemType().ToLower()) %>' />
                            <asp:LinkButton runat="server" ID="_editLink" CssClass="metagallery-link" Text="Edit" ToolTip='<%# string.Format("Edit {0}", this.GetItemType()) %>' OnClick="_edit_Click" Visible='<%# ((BaseItem)Container.DataItem).CanEdit %>' />                
                                
                            <asp:LinkButton runat="server" ID="_rejectLink" CssClass="metagallery-link" Text="Reject Friend Request" ToolTip="Reject Friend Request" OnClick="_reject_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_approveLink" CssClass="metagallery-link" Text="Approve Friend Request" ToolTip="Approve Invitation" OnClick="_approve_Click" Visible='<%# ((BaseItem)Container.DataItem).CanAccept %>' />
                            <asp:LinkButton runat="server" ID="_cancelLink" CssClass="metagallery-link" Text="Cancel Friend Request" ToolTip="Cancel Request" OnClick="_cancel_Click" Visible='<%# ((BaseItem)Container.DataItem).CanCancel %>' />


                            <asp:LinkButton runat="server" ID="_joinLink"  CssClass="metagallery-link" Text="Add to My Friends" ToolTip='<%# string.Format("Join {0}", this.GetItemType()) %>'
                                OnClick="_join_Click" Visible='<%# ((BaseItem)Container.DataItem).CanJoin %>' OnPreRender="_joinLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_joinConfirm" TargetControlID="_joinLink"
                                ConfirmText='<%# String.Format("Are you sure you want to join this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_joinConfirm_PreRender" />    
                            <asp:LinkButton runat="server" ID="_leaveLink" CssClass="metagallery-link" Text="Remove Friend" ToolTip='<%# string.Format("Leave {0}", this.GetItemType()) %>'
                                OnClick="_leave_Click" Visible='<%# ((BaseItem)Container.DataItem).CanLeave %>' OnPreRender="_leaveLink_PreRender" />
                            <ajaxToolkit:ConfirmButtonExtender runat="server" ID="_leaveConfirm" TargetControlID="_leaveLink"
                                ConfirmText='<%# String.Format("Are you sure you want to leave this {0}?", this.GetItemType().ToLower()) %>'
                                OnPreRender="_leaveConfirm_PreRender" />
                        </asp:Panel>
                    </asp:Panel>
                </asp:Panel>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>
    
    <asp:View runat="server" ID="_textView">
        <asp:FormView runat="server" ID="_textViewForm">
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="_nameHyperLink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' />
                <strong><asp:Label ID="_eventType" runat="server" Text='<%# string.Format("({0})", ((BaseItem)Container.DataItem).SubType) %>' 
                Visible='<%# (((BaseItem)Container.DataItem).SubType.Length > 0) && ((Container.DataItem as WLQuickApps.SocialNetwork.Business.Group) != null) %>' /></strong>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>
    
    <asp:View runat="server" ID="_thumbnailView">
        <asp:FormView runat="server" ID="_thumbnailViewForm">
            <ItemTemplate>
                <ajaxToolkit:HoverMenuExtender ID="hme2" runat="Server"
                        TargetControlID="_nullablePicture"
                        PopupControlID="_thumbnailDetails"
                        PopupPosition="Center"
                        OffsetX="0"
                        OffsetY="0"
                        PopDelay="50" />
                <asp:Panel ID="_thumbnailDetails" runat="server" SkinID="MetaGallery-ThumbnailPopup-Panel">
                    <asp:Panel runat="server" SkinID="MetaGallery-ThumbnailPopup-PicturePanel">
                        <cc:NullablePicture ID="_nullablePictureControl" runat="server" MaxHeight="128" MaxWidth="128"
                            Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                            NullImageUrl='<%# string.Format("~/Images/missing-{0}-128x128.png", this.GetItemType().ToLower()) %>' />
                    </asp:Panel> 
                    <asp:Panel runat="server" SkinID="MetaGallery-ThumbnailPopup-Details">
                        <asp:HyperLink ID="_nameHyperLink" runat="server" SkinID="MetaGallery-ThumbnailPopup-Hyperlink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>'
                            NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' />
                    </asp:Panel>    
                </asp:Panel>
                
                <cc:NullablePicture runat="server" ID="_nullablePicture" MaxHeight="32" MaxWidth="32" BorderWidth="0"
                    Item='<%# Container.DataItem %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>'
                    NullImageUrl='<%# string.Format("~/Images/missing-{0}-32x32.png", this.GetItemType().ToLower()) %>' />
            </ItemTemplate>
        </asp:FormView>
    </asp:View>

    <asp:View runat="server" ID="_mobileView">
        <asp:FormView runat="server" ID="_mobileViewForm">
            <ItemTemplate>
                <br />
                <asp:Panel runat="server" ID="_mobilePanel">
                    <cc:NullablePicture runat="server" ID="_nullablePicture" MaxHeight="48" MaxWidth="48"
                        Item='<%# Container.DataItem %>'
                        NullImageUrl='<%# string.Format("~/Images/missing-{0}-32x32.png", this.GetItemType().ToLower()) %>' />
                    <asp:Panel runat="server" ID="_labelPanel">
                        <asp:HyperLink runat="server" ID="_nameHyperLink" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' NavigateUrl='<%# WebUtilities.GetViewItemUrl((BaseItem)Container.DataItem) %>' />
                    </asp:Panel>
                </asp:Panel>
            </ItemTemplate>
        </asp:FormView>
    </asp:View>
</asp:MultiView>