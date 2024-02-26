<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="EventsListControl.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.EventsListControl" %>
<asp:ObjectDataSource ID="EventsDataSource" runat="server" 
    SelectMethod="GetEvents" TypeName="WLQuickApps.ContosoBank.Logic.EventLogic">
</asp:ObjectDataSource>
<asp:GridView ID="EventsGridView" runat="server" 
    AutoGenerateColumns="False" DataSourceID="EventsDataSource" 
GridLines="None" CssClass="PortalTable" >
    <RowStyle CssClass="PortalRow" />
    <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" 
            Visible="False" />
        <asp:BoundField DataField="EventDate" DataFormatString="{0:d}" 
            HeaderText="Date" SortExpression="EventDate" 
            ItemStyle-CssClass="PortalSummaryText" >
<ItemStyle CssClass="PortalSummaryText"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="EventName" HeaderText="Event" 
            SortExpression="EventName" ItemStyle-CssClass="PortalSubjectTextHighlight" >
<ItemStyle CssClass="PortalSubjectTextHighlight"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Location" HeaderText="Location" 
            SortExpression="Location" ItemStyle-CssClass="PortalSummaryText" >
<ItemStyle CssClass="PortalSummaryText"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Map">
            <ItemTemplate>
                <asp:HyperLink ID="HyperLinkLaunchMap" runat="server" onclick='<%#Eval("ID", "MapActionLaunchMap_onclick({0}, event)")%>' Text="See Map"></asp:HyperLink>
            </ItemTemplate>
            <ItemStyle CssClass="PortalSummaryText" />
        </asp:TemplateField>
    </Columns>
    <HeaderStyle CssClass="PortalSummaryHeader" />
    <AlternatingRowStyle CssClass="PortalRowAlternate" />
</asp:GridView>
<asp:XmlDataSource ID="XmlDataSource1" runat="server" 
    DataFile="~/data/events.xml"></asp:XmlDataSource> 
  