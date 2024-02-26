<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MonitorWebLog.aspx.cs" Inherits="Admin_MonitorWebLog"
    Title="Monitor Web Log" %>
    
<asp:content id="_content" contentplaceholderid="MainContent" runat="server">
    <asp:gridview id="_webLogGridView" runat="server" allowpaging="True" 
        autogeneratecolumns="False" datasourceid="_webEventDataSource" Font-Size="X-Small" CellPadding="4" ForeColor="#333333" GridLines="None" PageSize="30">
        <RowStyle BackColor="#EFF3FB"></RowStyle>
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" Font-Size="Small" Font-Underline="True"></PagerStyle>
        <HeaderStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></HeaderStyle>
        <AlternatingRowStyle BackColor="White"></AlternatingRowStyle>
        <FooterStyle BackColor="#507CD1" ForeColor="White" Font-Bold="True"></FooterStyle>
        <Columns>
            <asp:TemplateField SortExpression="EventTime" HeaderText="Event Time">
                <ItemTemplate>
                    <asp:HyperLink ID="_detailsHyperLink" runat="server" Text='<%# Bind("EventTime") %>'
                        NavigateUrl='<%# Eval("EventID", "~/Admin/WebLogDetail.aspx?EventID={0}") %>'> 
                    </asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ReadOnly="True" DataField="ExceptionType" SortExpression="ExceptionType" HeaderText="Exception Type"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="Message" SortExpression="Message" HeaderText="Message"></asp:BoundField>
            <asp:BoundField ReadOnly="True" DataField="RequestURL" SortExpression="RequestURL" HeaderText="Request URL"></asp:BoundField>
        </Columns>
    </asp:gridview>
    <asp:objectdatasource id="_webEventDataSource" runat="server" selectmethod="GetWebEvents"
        typename="WLQuickApps.SocialNetwork.Business.WebEventManager" SelectCountMethod="GetWebEventCount"  
        StartRowIndexParameterName="startRowIndex" MaximumRowsParameterName="maximumRows" EnablePaging="true">
    </asp:objectdatasource>
</asp:content>