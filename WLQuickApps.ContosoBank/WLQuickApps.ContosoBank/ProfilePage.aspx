<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True"
    CodeBehind="ProfilePage.aspx.cs" Inherits="WLQuickApps.ContosoBank.ProfilePage"
    Title="Australian Small Business Portal - Profile" %>

<%@ Register Assembly="Microsoft.Live.ServerControls" Namespace="Microsoft.Live.ServerControls"
    TagPrefix="live" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="Medium">Your Profile</h1>
    
    <div class="ProfileDetail">
        <table class="ProfileDetailTable">
            <tr>
                <td class="ProfileDetailTitle">Display Name:</td>
                <td class="ProfileDetailValue">
                    <asp:TextBox ID="DisplayNameTextBox" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="DisplayNameRequiredFieldValidator" runat="server"
                        ErrorMessage="You must choose a display name." ControlToValidate="DisplayNameTextBox"
                        ValidationGroup="ProfileValidation">*</asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="DisplayNameCustomValidator" runat="server" ControlToValidate="DisplayNameTextBox"
                        ErrorMessage="Selected Displayname already exists." ValidationGroup="ProfileValidation">*</asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td>Postcode:</td>
                <td>
                    <asp:TextBox ID="PostCodeTextBox" runat="server" MaxLength="4" Width="30px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PostcodeRequiredFieldValidator" runat="server" ErrorMessage="You must enter a postcode."
                        ControlToValidate="PostCodeTextBox" ValidationGroup="ProfileValidation">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="PostCodeRegularExpressionValidator" runat="server"
                        ErrorMessage="Postcode must be a 4 digit number." ValidationExpression="\d{4}"
                        ControlToValidate="PostCodeTextBox" ValidationGroup="ProfileValidation">*</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td>Country:</td>
                <td>
                    <asp:TextBox ID="CountryTextBox" runat="server" Text="Australia" MaxLength="50" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Avatar:</td>
                <td>
                    <asp:RadioButtonList ID="AvatarDataList" runat="server" 
                        RepeatDirection="Horizontal" RepeatColumns="6"></asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>Preferred Background:</td>
                <td>
                    <asp:DropDownList ID="PreferredBackgroundDropDownList" runat="server" DataTextField="Name"
                        DataValueField="Location" onchange="document.body.style.background='url(' + this.value + ')';">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>    
                <td></td>
                <td>
                    <asp:ValidationSummary ID="ProfileValidationSummary" runat="server" ValidationGroup="ProfileValidation" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <div class="ProfileActions">
                        <div class="CommandButtonSmall">
                            <asp:LinkButton ID="SaveButton" runat="server" Text="Save" OnClick="SaveButton_Click" />
                        </div>
                        <div class="CommandButtonSmall">
                            <asp:LinkButton ID="CancelButton" runat="server" Text="Cancel" OnClick="CancelButton_Click" />
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
