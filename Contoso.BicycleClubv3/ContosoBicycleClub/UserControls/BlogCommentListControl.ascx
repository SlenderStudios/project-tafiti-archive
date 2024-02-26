<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlogCommentListControl.ascx.cs" Inherits="WLQuickApps.ContosoBicycleClub.UserControls.BlogCommentListControl" %>

<asp:Panel ID="CommentsPanel" CssClass="cbc-EventComments" runat="server">
	<h2>Comments</h2>
	<div ID="EventCommentsLinks" class="cbc-EventCommentsLinks" runat="server">
		<asp:HyperLink ID="AddCommentLink" Text="Add a comment" runat="server" />
		<span class="separator">&#160;|&#160;</span>
		<asp:HyperLink ID="CommentsFeedLink" CssClass="cbc-RSS" Text="Comments RSS" Tooltip="Subscribe to the comments RSS feed" runat="server" />
	</div>

	<asp:Repeater ID="RideCommentsItems" runat="server">
		<ItemTemplate>
			<div class="comment">
				<!-- commentBody is not HTMLEncoded. We can assme the HTML is safe since it come from Windows Spaces. -->
				<div class="commentBody"><%# XPath("live:commentBody", commentsNsManager)%></div>
				<div class="commentMeta">
					<span class="pubDate"><%# string.Format("{0:MMMM dd, yyyy}", Convert.ToDateTime(XPath("pubDate", commentsNsManager)))%></span>
					<span class="separator">&#160;|&#160;</span>
					<span class="authorName"><%# Server.HtmlEncode(XPath("live:authorName", commentsNsManager).ToString()) %></span>
				</div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</asp:Panel>
