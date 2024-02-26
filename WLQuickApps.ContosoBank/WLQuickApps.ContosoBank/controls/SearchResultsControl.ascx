<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="SearchResultsControl.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.SearchResultsControl" %>
<asp:UpdatePanel ID="SearchUpdatePanel" runat="server">
    <ContentTemplate>      
        <div>
            <div style="float:left">
                <asp:TextBox ID="BusinessSearchTextBox" runat="server" Width="215px"></asp:TextBox>
            </div>
            <div class="ToolSearchBox">
                <div class="CommandButtonMedium">
                    <asp:LinkButton ID="SearchButton" runat="server" Text="Business Search" OnClick="SearchButton_Click"></asp:LinkButton>
                </div>
            </div>
        </div>        
        <div style="float:right">
            <asp:Image ID="toppreviousImage" runat="server" ImageUrl="~/images/ArrowBack.png" /> 
            <asp:LinkButton ID="toppreviousLink" runat="server" Text="previous" onclick="previousLink_Click" ></asp:LinkButton>
            <asp:LinkButton ID="topnextLink" runat="server" Text="next" onclick="nextLink_Click"></asp:LinkButton>
            <asp:Image ID="topnextImage" runat="server" ImageUrl="~/images/ArrowFwd.png" />
        </div>
        <div style="clear:both;">
        <table class="PortalTable" cellpadding="0" cellspacing="0">
            <asp:Repeater ID="SearchResultsRepeater" runat="server">
            <HeaderTemplate>
                <tr class="PortalSummaryHeader">
                    <th>Title</th>
                    <th>Description</th>
                    <th>Map</th>
                </tr>
            </HeaderTemplate>
                <ItemTemplate>
                    <tr class="PortalRow">
                        <td><%#DataBinder.Eval(Container.DataItem, "Title") %></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Description")%></td>
                        <td><asp:HyperLink ID="mapLink" runat="server" Text="See Map" NavigateUrl='<%#DataBinder.Eval(Container.DataItem, "Url") %>' Target="_blank"></asp:HyperLink></td>
                    </tr>
               </ItemTemplate>
               <AlternatingItemTemplate>
                    <tr class="PortalRowAlternate">
                        <td><%#DataBinder.Eval(Container.DataItem, "Title") %></td>
                        <td><%#DataBinder.Eval(Container.DataItem, "Description")%></td>
                        <td><asp:HyperLink ID="mapLink" runat="server" Text="See Map" NavigateUrl='<%#DataBinder.Eval(Container.DataItem, "Url") %>' Target="_blank"></asp:HyperLink></td>
                    </tr>
               </AlternatingItemTemplate>
            </asp:Repeater>
        </table>
        </div>
        <div style="float:right">
            <asp:Image ID="previousImage" runat="server" ImageUrl="~/images/ArrowBack.png" /> 
            <asp:LinkButton ID="previousLink" runat="server" Text="previous"
                onclick="previousLink_Click" ></asp:LinkButton>
            <asp:LinkButton ID="nextLink" runat="server" Text="next" 
                onclick="nextLink_Click"></asp:LinkButton>
                <asp:Image ID="nextImage" runat="server" ImageUrl="~/images/ArrowFwd.png" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>