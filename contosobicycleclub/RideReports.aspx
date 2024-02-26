<%@ Page 
    Language="C#" 
    AutoEventWireup="true" 
    CodeFile="RideReports.aspx.cs" 
    Inherits="RideReports" 
    Theme="" %>

<asp:XmlDataSource ID="rideReportsDataSource" 
    runat="server" 
    EnableCaching="false"
    DataFile="<%$ AppSettings:LatestRidesFeed %>" 
    XPath="/rss/channel/item"></asp:XmlDataSource>
    
<h1>Rides</h1>
<asp:DataList ID="rideReportsDataList" runat="server" >
    <ItemTemplate>
        <h2><%# XPath("title") %> </h2>

        <%# XPath("description") %> 

        <hr />
    </ItemTemplate>
</asp:DataList>  
<asp:Panel id="errorItem" runat="server" visible="false">
<h2>Error retrieving ride data<</h2>
<% =DataBindingErrorMsg %>
<hr />
</asp:Panel>