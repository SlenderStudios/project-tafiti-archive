<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Tags.ascx.cs" Inherits="Tags" %>

<asp:UpdatePanel runat="server" ID="_updatePanel" UpdateMode="Always" RenderMode="Inline">
    <ContentTemplate>
        <script type="text/javascript">
        <!--
            function <%= this.ClientID %>_newTag_keyPress(sender, e)
            {
                if ((e.keyCode == 13) && (sender.value.length > 0))
                {                    
                    <%= this.Page.ClientScript.GetPostBackEventReference(this._addTag, string.Empty) %>;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        
            function <%= this.ClientID %>_newTag_keyUp(sender, e)
            {
                var addTag = $get("<%= this._addTag.ClientID %>");
                
                if ((sender.value == 0))
                {
                    addTag.style.display = "none";
                }
                else
                {
                    addTag.style.display = "";
                }
            }            
        //-->
        </script>
        
        <asp:Label runat="server" Text="(No tags yet)<br />" ID="_emptyDataLabel" />
        <asp:Repeater ID="_tagList" runat="server" DataSourceID="_tagDataSource" OnItemDataBound="_tagList_ItemDataBound">
            <ItemTemplate>
                <asp:Panel runat="server" ID="_tagListPanel" SkinID="Tags-TagItem">
                    <asp:HyperLink runat="server" ID="_tagLink" NavigateUrl='<%# Eval("Name", "~/Search.aspx?tag={0}") %>'
                         Text='<%# Bind("Name") %>' />
                         <asp:LinkButton runat="server" ID="_removeTag" CommandName="RemoveTag" CommandArgument='<%# Bind("Name") %>'
                             OnCommand="_removeTag_Command" CausesValidation="False" Visible='<%# BaseItemManager.GetBaseItem(this._baseItemID).CanEdit %>'>
                             <asp:Image ID="_removeTagImage" runat="server" ImageUrl="~/Images/Delete.png" AlternateText="(Delete)" />
                         </asp:LinkButton>
                </asp:Panel>
            </ItemTemplate>
        </asp:Repeater><br />
        <asp:ObjectDataSource ID="_tagDataSource" runat="server" TypeName="WLQuickApps.SocialNetwork.Business.TagManager"
            SelectMethod="GetTags" InsertMethod="AddTagToBaseItem" OnSelecting="_tagDataSource_Selecting">
        </asp:ObjectDataSource>

        <asp:PlaceHolder runat="server" ID="_addTagHolder">
            <cc:SecureTextBox runat="server" ID="_newTag" Columns="15" OnPreRender="_newTag_PreRender" ValidationGroup="AddTag" />
            <asp:Button runat="server" ID="_addTag" OnClick="_addTag_Click" Text="Add" OnPreRender="_addTag_PreRender"
                ValidationGroup="AddTag" /><br />
            <asp:RegularExpressionValidator runat="server" ID="_tagValid" ControlToValidate="_newTag" ValidationGroup="AddTag"
                ValidationExpression="[a-zA-Z0-9 ,]+" Text="Enter only letters and numbers." Display="Dynamic" />
            
            <ajaxToolkit:TextBoxWatermarkExtender runat="server" ID="_newTagWatermark" TargetControlID="_newTag"
                WatermarkText="Type new tag..." WatermarkCssClass="newTagWatermark" />
        </asp:PlaceHolder>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdateProgress ID="_updateProgress" runat="server" AssociatedUpdatePanelID="_updatePanel" DisplayAfter="0">
    <ProgressTemplate>
        Updating&#0133;
    </ProgressTemplate>
</asp:UpdateProgress><br />