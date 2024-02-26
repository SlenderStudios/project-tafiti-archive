<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True" CodeBehind="Community.aspx.cs" Inherits="WLQuickApps.ContosoBank.Community" Title="Australian Small Business Portal - Community" %>
<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.WebControls" tagprefix="asp" %>
<%@ Register src="controls/StickyPosts.ascx" tagname="StickyPosts" tagprefix="uc1" %>
<%@ Register src="controls/ArticlesResources.ascx" tagname="ArticlesResources" tagprefix="uc2" %>
<%@ Register src="controls/ForumSummaryControl.ascx" tagname="ForumSummaryControl" tagprefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <h1 class="Medium">Current Discussion Subjects</h1>
        <uc3:ForumSummaryControl ID="ForumSummaryControl1" runat="server" />
    </div>
    <hr />
    <div>
        <h2 class="Large"><asp:label style="color:red; font-weight:600;" runat="server" Text="HOT"></asp:label> Topics</h2>
        <uc1:StickyPosts ID="LatestStickyPosts" runat="server" />
    </div>
    <hr />
    <div class="ArticlesResources">
        <h2>Articles and Resources</h2>
        <uc2:ArticlesResources ID="ArticlesResources1" runat="server" />
    </div>
    <div class="TagCloud">
        <h2>Popular Tags</h2>
        <img class="TagCloudImage" src="Images/TagCloudDummy.png" alt="Popular Tags" />
    </div>
</asp:Content>
