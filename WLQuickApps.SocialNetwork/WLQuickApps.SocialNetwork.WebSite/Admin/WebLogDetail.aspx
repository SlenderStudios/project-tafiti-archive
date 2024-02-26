<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebLogDetail.aspx.cs" Inherits="Admin_WebLogDetail"
    Title="Web Log Detail" %>

<asp:content id="_content" contentplaceholderid="MainContent" runat="server">
    <asp:detailsview id="_webLogDetailsView" runat="server" autogeneraterows="False" datasourceid="_webEventDataSource" CellPadding="4" ForeColor="#333333" GridLines="None">
        <RowStyle BackColor="#EFF3FB"></RowStyle>
        <FieldHeaderStyle BackColor="#DEE8F5" Font-Bold="True"  VerticalAlign="Top"></FieldHeaderStyle>
        <HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>
        <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
        <Fields>
            <asp:BoundField ReadOnly="True" DataField="EventID" SortExpression="EventID" HeaderText="EventID"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="EventTime" SortExpression="EventTime" HeaderText="EventTime"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="ExceptionType" SortExpression="ExceptionType" HeaderText="ExceptionType"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="Message" SortExpression="Message" HeaderText="Message"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="RequestURL" SortExpression="RequestURL" HeaderText="RequestURL"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="EventType" SortExpression="EventType" HeaderText="EventType"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="ApplicationPath" SortExpression="ApplicationPath" HeaderText="ApplicationPath"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="ApplicationVirtualPath" SortExpression="ApplicationVirtualPath" HeaderText="ApplicationVirtualPath"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="MachineName" SortExpression="MachineName" HeaderText="MachineName"></asp:BoundField>
            <asp:TemplateField SortExpression="Details" HeaderText="Details">
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Eval("Details").ToString().Replace("\n","<BR>") %>' id="Label1"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Fields>
    </asp:detailsview>
    <asp:objectdatasource id="_webEventDataSource" runat="server" selectmethod="GetWebEvent"
        typename="WLQuickApps.SocialNetwork.Business.WebEventManager">
        <SelectParameters>
            <asp:QueryStringParameter Type="String" Name="eventID" QueryStringField="EventID"></asp:QueryStringParameter>
        </SelectParameters>
    </asp:objectdatasource>
</asp:content>
