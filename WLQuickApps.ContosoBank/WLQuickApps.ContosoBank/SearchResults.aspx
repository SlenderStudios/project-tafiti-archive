<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True" CodeBehind="SearchResults.aspx.cs" Inherits="WLQuickApps.ContosoBank.SearchResults" Title="Australian Small Business Portal - Business Search Results" %>
<%@ Register src="controls/SearchResultsControl.ascx" tagname="SearchResultsControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Business Search Results</h1>
    
    <uc1:SearchResultsControl ID="businessSearchResultsControl" runat="server" />

</asp:Content>