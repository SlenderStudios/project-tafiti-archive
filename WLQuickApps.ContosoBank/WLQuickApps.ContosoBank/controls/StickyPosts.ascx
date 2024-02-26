<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="StickyPosts.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.StickyPosts" %>
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
    <%@ Register src="Avatar.ascx" tagname="Avatar" tagprefix="uc1" %>

    <asp:Repeater ID="StickyPostRepeater" runat="server" 
            onitemdatabound="StickyPostRepeater_ItemDataBound">
    <ItemTemplate>
        <div class="ForumItemHeader">
                <div class="ForumItemHeaderIcon">
                    <asp:Image ID="stickyImage" runat="server" />&nbsp;            
                </div>
                <div class="ForumItemHeaderLeft">
                    <a class="ForumSubjectTitle" href='<%#Eval("ForumSubjectID", "../ForumThread.aspx?ID={0}") %>'>
                        <asp:Label ID="SubjectLabel" runat="server" CssClass="ForumSubjectTitle"></asp:Label>
                    </a>
                </div>
                <div class="ForumItemHeaderRight">
                    <asp:Label ID="textLabel" runat="server" CssClass="ForumSubjectTextHighlight" Text="Last post:"></asp:Label>
                    <asp:Label ID="lastPostLabel" runat="server" CssClass="ForumSubjectText"></asp:Label>
                    <asp:Label ID="postUserName" class="ForumSubjectTextHighlight" runat="server"></asp:Label>
                </div>
            
        </div>
        <div class="ForumContent">
            <div class="ForumContentLeft">
                <div class="ProfileDisplay">
                    <asp:Label ID="stickyDisplayName" runat="server" class="ProfileDisplayName"></asp:Label>
                    <div class="ProfileDisplayImage">
                        <uc1:Avatar ID="userAvatar" runat="server" />
                    </div>
                    <div class="ProfileDisplayPosts">
                        <asp:label ID="numberofpostsLabel" runat="server" />
                    </div>
                    <div class="ProfileDisplayRating">
                        <div class="ProfileRatingItem">
                            <cc1:Rating ID="UserRating" runat="server" RatingAlign="Horizontal" RatingDirection="LeftToRightTopToBottom" StarCssClass="RatingStar" EmptyStarCssClass="RatingEmpty" FilledStarCssClass="RatingFilled"  WaitingStarCssClass="RatingSaved"></cc1:Rating>
                        </div>
                    </div>
                </div>

            </div>
            <div class="ForumContentRight">
                <asp:Label ID="posttextLabel" runat="server" class="ForumSubjectText" />
                <br></br><br />
                <asp:Label ID="posttestTags" runat="server" class="ForumTagTitle" Text="Tags: " Visible="false"></asp:Label><asp:label id="posttagsLabel" runat="server" class="ForumSubjectTextHighlight"></asp:label>
            </div>
        </div>
    </ItemTemplate>
    <AlternatingItemTemplate>
         <div class="ForumItemHeader ForumItemHeaderAlternate">
                <div class="ForumItemHeaderIcon">
                    <asp:Image ID="stickyImage" runat="server" />&nbsp;            
                </div>
                <div class="ForumItemHeaderLeft">
                    <a Class="ForumSubjectTitle" href='<%#Eval("ForumSubjectID", "../ForumThread.aspx?ID={0}") %>'>
                        <asp:Label ID="SubjectLabel" runat="server" CssClass="ForumSubjectTitle"></asp:Label>
                    </a>
                </div>
                <div class="ForumItemHeaderRight">
                    <asp:Label ID="textLabel" runat="server" CssClass="ForumSubjectTextHighlight" Text="Last post:"></asp:Label>
                    <asp:Label ID="lastPostLabel" runat="server" CssClass="ForumSubjectText"></asp:Label>
                    <asp:Label ID="postUserName" class="ForumSubjectTextHighlight" runat="server"></asp:Label>
                </div>            
        </div>
        <div class="ForumContent ForumContentAlternate">
            <div class="ForumContentLeft">
                <div class="ProfileDisplay">
                    <asp:Label ID="stickyDisplayName" runat="server" class="ProfileDisplayName"></asp:Label>
                    <div class="ProfileDisplayImage">
                        <uc1:Avatar ID="userAvatar" runat="server" />
                    </div>
                    <div class="ProfileDisplayPosts">
                        <asp:label ID="numberofpostsLabel" runat="server" />
                    </div>
                    <div class="ProfileDisplayRating">
                        <div class="ProfileRatingItem">
                            <cc1:Rating ID="UserRating" runat="server" RatingAlign="Horizontal" RatingDirection="LeftToRightTopToBottom" StarCssClass="RatingStar" EmptyStarCssClass="RatingEmpty" FilledStarCssClass="RatingFilled"  WaitingStarCssClass="RatingSaved"></cc1:Rating>
                        </div>
                    </div>
                </div>
            </div>
            <div class="ForumContentRight">
                <asp:Label ID="posttextLabel" runat="server" class="ForumSubjectText" />
                <br></br><br />
                <asp:Label ID="posttestTags" runat="server" class="ForumTagTitle" Text="Tags: " Visible="false"></asp:Label><asp:label id="posttagsLabel" runat="server" class="ForumSubjectTextHighlight"></asp:label>
            </div>
        </div>
    </AlternatingItemTemplate>
    </asp:Repeater>
