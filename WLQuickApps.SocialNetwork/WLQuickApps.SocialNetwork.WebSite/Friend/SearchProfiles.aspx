<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchProfiles.aspx.cs" Inherits="Friend_SearchProfiles" Title="Untitled Page" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" >
        <asp:RadioButton ID="_userNameRadioButton" runat="server" GroupName="Search" Text="By user name" Checked="true" />
        <asp:RadioButton ID="_emailRadioButton" runat="server" GroupName="Search" Text="By email" />
        <asp:RadioButton ID="_fullNameRadioButton" runat="server" GroupName="Search" Text="By full name" /><br /><br />
        <div class="subform-field">
            <cc:SecureTextBox ID="_searchTextBox" runat="server" />
            <asp:RequiredFieldValidator ID="_nameRequired" runat="server" ControlToValidate="_searchTextBox"
                ErrorMessage="Enter something to search by." ToolTip="Enter something to search by." Text="*" Display="Dynamic" />
            <asp:Button ID="_searchButton" runat="server" OnClick="_searchButton_Click" Text="Find" />
        </div><br />
    </cc:DropShadowPanel>
    <br /><br />
    <cc:DropShadowPanel ID="_searchResultsPanel" runat="server"  Visible="false">
        <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
            Search Results
        </cc:DropShadowPanel>
        <cc:MetaGallery ID="_searchResultsUserGroup" runat="server" AllowPaging="true" PageSize="20" DataSourceID="_searchResultsDataSource"
            EmptyDataText="No Results." />
        <asp:ObjectDataSource runat="server" ID="_searchResultsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.UserManager"
            SelectMethod="GetUsersByFullName" SelectCountMethod="GetUsersByFullNameCount" StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows" EnablePaging="true">
            <SelectParameters>
                <asp:Parameter Name="firstName" Type="String" DefaultValue=" " />
                <asp:Parameter Name="lastName" Type="String" DefaultValue=" " />
            </SelectParameters>
        </asp:ObjectDataSource>
    </cc:DropShadowPanel>
    <br />
    <asp:Label ID="_resultsLabel" runat="server" />
</asp:Content>

