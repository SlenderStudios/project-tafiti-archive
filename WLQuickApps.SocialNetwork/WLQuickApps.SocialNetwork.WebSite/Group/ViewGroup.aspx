<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewGroup.aspx.cs" Inherits="ViewGroup" Title="View Group" %>
<%@ Register Src="../Controls/ViewGroupForm.ascx" TagName="ViewGroupForm" TagPrefix="uc" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="_doesNotExistPanel" runat="server" Visible="false">The specified group does not exist.</asp:Panel>
    <asp:Panel ID="_noPermissionPanel" runat="server" Visible="false">
    <p>To view the specified group, you must be invited. To be invited, please contact the owner.
    </asp:Panel>
    <asp:Panel ID="_anonymousPanel" runat="server" Visible="false">
    <p>If you are already a member of this group, or were invited, please
    <asp:HyperLink ID="_signInHyperlink" runat="server" Text="sign in" />
    or
    <asp:HyperLink ID="_registerHyperlink" runat="server" Text="register" />.
    </p>
    </asp:Panel>
    
    <uc:ViewGroupForm runat="server" ID="_viewGroupForm" Visible="false" />
</asp:Content>

