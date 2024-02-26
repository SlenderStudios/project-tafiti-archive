<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="WLQuickApps.FieldManager.WebSite.Register" MasterPageFile="~/AdminPages.master" %>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">
<div class="pageContent">
    <div class="header">Please register:</div>
    <table>
        <tr>
            <td class="tableLabel-light">
                Email:
            </td>
            <td>
                <asp:TextBox ID="_emailTextBox" runat="server" MaxLength="256"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:LinkButton ID="_registerButton" CssClass="optionsLink" runat="server" Text="Register" 
                    onclick="_registerButton_Click" />
            </td>
        </tr>
    </table>
</div>    
</asp:Content>