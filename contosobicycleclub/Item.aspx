<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Item.aspx.cs" Inherits="ContosoBicycleClub.Item" Theme="" %>
<html>
    <head>
        <style type="text/css">
            body { font-family: Arial; font-size: 10px;}
            h2 {font-size: 12px;}
            td {font-size: 10px;}
        </style>
        <title></title>
    </head>
    <body>
    <asp:XmlDataSource ID="itemDataSource" 
    EnableCaching="false"
    runat="server" >
    
</asp:XmlDataSource>
        <asp:DataList ID="itemDataList" runat="server" DataSourceID="itemDataSource"  >
            <ItemTemplate>
                <h2><%# XPath("title") %></h2>
                <p><%# XPath("description") %></p>
            </ItemTemplate>
        </asp:DataList>
    </body>
</html>