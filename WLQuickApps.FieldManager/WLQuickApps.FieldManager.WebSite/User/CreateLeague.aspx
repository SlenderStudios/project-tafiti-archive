<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="CreateLeague.aspx.cs" Inherits="WLQuickApps.FieldManager.WebSite.User_CreateLeague" MasterPageFile="~/AdminPages.master" %>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">

<script type="text/javascript">
    
</script>
    <div class="pageContent">
        <div class="header">Create League</div>
        <div class="leftPanel">
            <table>
                <tr>
                    <td class="tableLabel-light">
                        Title:
                    </td>
                    <td>
                        <asp:TextBox ID="_titleTextBox" runat="server" MaxLength="32"></asp:TextBox>                        
                    </td>
                </tr>
                <tr>
                    <td class="tableLabel-light">
                        Description:
                    </td>
                    <td>
                        <asp:TextBox ID="_descriptionTextBox" runat="server" MaxLength="64"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="tableLabel-light">
                        Type:
                    </td>
                    <td>
                        <asp:TextBox ID="_typeTextBox" runat="server" MaxLength="32"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:LinkButton ID="_createButton" CssClass="optionsLink" runat="server" Text="Create" 
                            onclick="_createButton_Click" />
                    </td>
                </tr>
            </table>
    </div>
</div>


</asp:Content>