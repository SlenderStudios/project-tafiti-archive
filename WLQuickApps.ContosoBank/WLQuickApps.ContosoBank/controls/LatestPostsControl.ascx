<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="LatestPostsControl.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.LatestPostsControl" %>
<h3>Latest Forum Posts</h3>
<asp:Repeater ID="LatestPostRepeater" runat="server" onitemdatabound="LatestPostRepeater_ItemDataBound">
    <ItemTemplate>
        <div class="ForumSummaryItem"><asp:Label ID="forumSubject" runat="server"></asp:Label> <div class="ForumSummaryLink"><asp:HyperLink ID="forumHyperLink" runat="server">read more</asp:HyperLink></div></div>
    </ItemTemplate>
</asp:Repeater>
                       

    