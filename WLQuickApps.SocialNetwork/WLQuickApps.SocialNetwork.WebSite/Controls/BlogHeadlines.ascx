<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BlogHeadlines.ascx.cs" Inherits="BlogHeadlines" %>
<%@ Register Assembly="RssToolkit" Namespace="RssToolkit" TagPrefix="cc1" %>

<asp:Panel runat="server" ID="_blogHeadlinesPanel" SkinID="BlogRotator">
    <asp:DataList runat="server" ID="_blogHeadlines">
        <ItemTemplate>
            <asp:Label runat="server" ID="_blogItemDate" SkinID="BlogItemDate">
                <%# DateTime.Parse(((GenericRssElement)Container.DataItem)["PubDate"]).ToString("MMM d, yyyy") %>
            </asp:Label>
            <asp:Image runat="server" ID="_arrow" ImageUrl="~/Images/Arrow.gif" />            
            <asp:HyperLink runat="server" ID="_blogItemLink" NavigateUrl='<%# ((GenericRssElement)Container.DataItem)["Link"] %>'
                Target="_blank" Text='<%# this.GetRssItemTitle(Container.DataItem) %>' SkinID="BlogItemTitle" />
        </ItemTemplate>
    </asp:DataList>
</asp:Panel>
<asp:Label runat="server" ID="_errorLabel" Text="This blog isn't currently available." Visible="False" />
<cc1:RssDataSource id="_rssDataSource" runat="server" MaxItems="5" />
