<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True" CodeBehind="ForumThread.aspx.cs" Inherits="WLQuickApps.ContosoBank.ForumThread" Title="Australian Small Business Portal - Forum Thread" %>
<%@ Register src="controls/ForumReplies.ascx" tagname="ForumReplies" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="Medium">Forum Thread</h1>
    <uc1:ForumReplies ID="ReplyForumReply" runat="server" />
    <div class="CommandButton"><asp:HyperLink ID="BackToForumHome" runat="server"  Text='Back to Forum' NavigateUrl="~/Community.aspx" ></asp:HyperLink></div>
</asp:Content>
