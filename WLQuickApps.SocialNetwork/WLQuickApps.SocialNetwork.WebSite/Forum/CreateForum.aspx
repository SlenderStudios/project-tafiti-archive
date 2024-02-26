<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CreateForum.aspx.cs" Inherits="Forum_CreateForum" Title="Untitled Page" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <cc:DropShadowPanel runat="server" ID="_createForumPanel" >

            <div class="form-label form-required">
                <asp:Label ID="_nameLabel" runat="server" AssociatedControlID="_nameTextBox">Discussion Name</asp:Label>
            </div>
            <div class="form-field">
                <cc:SecureTextBox ID="_nameTextBox" runat="server" />
                <asp:RequiredFieldValidator ID="_nameRequired" runat="server" ControlToValidate="_nameTextBox"
                    ErrorMessage="Enter a name." ToolTip="Enter a name." Text="*" Display="Dynamic" />
            </div>
        
            <asp:Panel ID="_forumPanel" runat="server" Visible="false">
                <div class="form-label form-required">
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="_forumDropDownList">Forum</asp:Label>
                </div>
                <div class="form-field">
                    <asp:DropDownList ID="_forumDropDownList" runat="server">
                        <asp:ListItem Selected="True" Value="" >General</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </asp:Panel>

            <div class="form-label">
            </div>
            <div class="form-field">
                <asp:Button runat="server" ID="_createForum" Text="Create" OnClick="_createForum_Click" /><br />
            </div>



    </cc:DropShadowPanel>


</asp:Content>

