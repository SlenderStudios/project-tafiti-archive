<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ForumSummaryControl.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.ForumSummaryControl" %>
    <asp:GridView ID="ForumSubjectGridView" runat="server" AutoGenerateColumns="False" onrowdatabound="ForumSubjectGridView_RowDataBound" CssClass="PortalTable" >
        <RowStyle CssClass="PortalRow" />
        <Columns>
            <asp:TemplateField HeaderText="Subject">
                <ItemTemplate>
                    <div class="PortalItemHeaderIcon">
                        <asp:Image ID="SubjectTypeImage" runat="server"/>&nbsp;
                    </div>
                    <div class="PortalItemHeader">
                        <asp:HyperLink ID="forumpostLink" runat="server" ></asp:HyperLink>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="NumViews" HeaderText="Views" ItemStyle-CssClass="PortalSummaryText" >
           </asp:BoundField>
            <asp:TemplateField HeaderText="Replies">
               <ItemTemplate>
                    <asp:Label ID="RepliesLabel" runat="server" CssClass="PortalSummaryText"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Post" >
                <ItemTemplate>
                    <asp:Label ID="LastPostLabel" runat="server" CssClass="PortalSubjectTextHighlight"></asp:Label><asp:Label ID="byLabel" runat="server" Text=" by " CssClass="PortalSummaryText"></asp:Label><asp:Label ID="PostByLabel" runat="server" CssClass="PortalSubjectTextHighlight"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle CssClass="PortalSummaryHeader" />
        <AlternatingRowStyle CssClass="PortalRowAlternate" />
    </asp:GridView>

