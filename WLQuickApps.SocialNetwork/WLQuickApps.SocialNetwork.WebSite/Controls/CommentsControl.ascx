<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommentsControl.ascx.cs" Inherits="CommentControl" %>

<asp:UpdatePanel runat="server" ID="_updatePanel" RenderMode="Inline" UpdateMode="Always">
    <ContentTemplate>
        <asp:GridView ID="DataList1" runat="server" DataSourceID="_commentsDataSource"
            AllowPaging="true" PageSize="6" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <cc:DropShadowPanel runat="server" ID="_commentPanel" SkinID="CommentPanel" Width='<%# this.HalfPanel ? new Unit(400) : new Unit() %>'>
                            <div style="float:left">
                                <cc:NullablePicture ID="NullablePictureControl1" runat="server"                 
                                    MaxHeight="64" MaxWidth="64" NullImageUrl="~/Images/missing-user-64x64.png"
                                    NavigateUrl='<%# WebUtilities.GetViewItemUrl(((Comment)Container.DataItem).UserItem) %>' 
                                    Item='<%# ((Comment)Container.DataItem).UserItem %>' />
                                <br />
                            </div>
                            <cc:DropShadowPanel ID="DropShadowPanel1" runat="server" SkinID="CommentsControl-Description">
                                <strong><asp:HyperLink runat="server" Text='<%# ((Comment)Container.DataItem).UserItem.Title %>' 
                                    NavigateUrl='<%# WebUtilities.GetViewItemUrl(((Comment)Container.DataItem).UserItem) %>' /> said</strong><br />
                                <asp:Label ID="PostDateTimeLabel" runat="server" Text='<%# Eval("PostDateTime", "on {0:D} at {0:t}" ) %>' SkinID="comment-timestamp" />&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton runat="server" ID="_deleteComment" CommandName="DeleteComment" CommandArgument='<%# Bind("CommentID") %>'
                                    Visible='<%# CommentManager.CanModifyComment(((Comment)Container.DataItem).CommentID) %>'
                                    OnCommand="_deleteComment_Command" SkinID="comment-delete" CausesValidation="False">Delete</asp:LinkButton>
                                <ajaxToolkit:ConfirmButtonExtender ID="_deleteConfirm" runat="server" 
                                    TargetControlID="_deleteComment"
                                    ConfirmText="Are you sure you want to delete this comment?" /><br />
                                <asp:Label ID="TextLabel" runat="server" Text='<%# ((Comment)Container.DataItem).Text.Replace("\n", "<br />") %>' /><br />
                            </cc:DropShadowPanel>
                            
                            <div class="clearFloats"></div>
                        </cc:DropShadowPanel>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No comments have been left yet.
            </EmptyDataTemplate>
        </asp:GridView>
        <asp:ObjectDataSource runat="server" ID="_commentsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.CommentManager"
            SelectMethod="GetComments" SelectCountMethod="GetCommentCount" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true" OnSelecting="_commentsDataSource_Selecting">
        </asp:ObjectDataSource>
        
        <script type="text/javascript">
        <!--
            function LimitText(textField, maxLength)
            {
                if (textField.value.length > maxLength)
                {
                    textField.value = textField.value.substring(0, maxLength);
                    return false;
                }
            }
            
            function ValidateComment(sender, args)
            {
                args.IsValid = !(getTextFieldObject().value.length == 0);
            }
            
            function ValidateCommentLength(sender, args)
            {
                args.IsValid = !(getTextFieldObject().value.length > 1000);
            }
            
            function getTextFieldObject()
            {
                var textField = document.getElementById("<%= this._smallContentTextBox.ClientID %>");
                if (textField == null)
                {
                    var textField = document.getElementById("<%= this._contentTextBox.ClientID %>");
                }
                return textField;
            }
        //-->
        </script>
        
        <cc:DropShadowPanel runat="server" ID="_addCommentPanel" SkinID="indentedSection">
            <asp:Panel ID="_loggedInPanel" runat="server" Visible='<%# UserManager.IsUserLoggedIn() %>'>
                <cc:SecureTextBox ID="_smallContentTextBox" runat="server" TextMode="MultiLine" SkinID="Comment-Small"
                    MaxLength="1000" Visible="False" />
                <cc:SecureTextBox ID="_contentTextBox" runat="server" TextMode="MultiLine" SkinID="Comment" MaxLength="1000"
                    Visible="False" />
                <asp:Panel runat="server" ID="_commentValidatorsPanel" SkinID="Comment-Validators">
                    <asp:CustomValidator runat="server" EnableClientScript="true" ClientValidationFunction="ValidateComment" 
                        ToolTip="Enter a comment to post." Text="Enter a comment to post." ValidationGroup="comment" /><br />
                    <asp:CustomValidator runat="server" EnableClientScript="true" ClientValidationFunction="ValidateCommentLength" 
                        ToolTip="The comment may not exceed 1000 characters." Text="The comment may not exceed 1000 characters." ValidationGroup="comment" />
                </asp:Panel>
                <br />
                <asp:Button ID="_addCommentButton" runat="server" OnClick="_addCommentButton_Click"
                    Text="Post Comment" SkinID="Comment" Visible="False" ValidationGroup="comment" />
                <asp:Button ID="_smallAddCommentButton" runat="server" OnClick="_addCommentButton_Click"
                    Text="Post Comment" SkinID="Comment-Small" Visible="False" ValidationGroup="comment" />
            </asp:Panel>
            <asp:Panel ID="_anonymousPanel" runat="server" Visible='<%# !UserManager.IsUserLoggedIn() %>'>
                Please 
                <asp:HyperLink ID="_signInHyperlink" runat="server" NavigateUrl='<%# WindowsLiveLogin.GetLoginUrl() %>' Text="sign in" />
                to leave a comment.
            </asp:Panel>
            <div class="clearFloats"></div>
        </cc:DropShadowPanel>
        <br />
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdateProgress ID="_updateProgress" runat="server" AssociatedUpdatePanelID="_updatePanel" DisplayAfter="0">
    <ProgressTemplate>
        Updating&#0133;
    </ProgressTemplate>
</asp:UpdateProgress>