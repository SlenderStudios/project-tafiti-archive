<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True" CodeBehind="Administrator.aspx.cs" Inherits="WLQuickApps.ContosoBank.Admin.Administrator" Title="Australian Small Business Portal - Administrator" %>

<%@ Register assembly="Microsoft.Live.ServerControls" namespace="Microsoft.Live.ServerControls" tagprefix="live" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="Medium">Site Statistics</h2>
        <div class="SiteStatistics">
            <div class="StatsList"></div>
            <div class="StatsGraph"></div>
        </div>

    <hr />
    <h2 class="Medium"><live:IDLoginView ID="IDLoginView4" runat="server" 
            PromptOnAssociation="False">
            <LoggedInTemplate><%=HttpContext.Current.User.Identity.Name %> - 
            </LoggedInTemplate>
        </live:IDLoginView> Tasks and Workflow</h2>
        
        <div class="TasksWorkflow"></div>

</asp:Content>
