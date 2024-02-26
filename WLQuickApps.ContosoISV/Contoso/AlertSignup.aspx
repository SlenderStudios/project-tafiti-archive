<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlertSignup.aspx.cs" Inherits="Contoso.AlertSignup" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Alert Signup</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtEmail" runat="server" meta:resourcekey="txtEmailResource1"></asp:TextBox>&nbsp;
        <asp:Button ID="btnSignup" runat="server" Text="Signup for Alerts" 
            OnClick="btnSignup_Click" meta:resourcekey="btnSignupResource1" />
        <asp:Label ID="Label1" runat="server" 
            Text="Enter your liveid email to signup for alerts" 
            meta:resourcekey="Label1Resource1"></asp:Label>
        <br />
        <asp:TextBox ID="txtGroup" runat="server" meta:resourcekey="txtGroupResource1"></asp:TextBox>
        <asp:Button ID="btnGroup" runat="server" Text="Add to Group" 
            OnClick="btnGroup_Click" meta:resourcekey="btnGroupResource1" />
        <asp:Label ID="Label2" runat="server" 
            Text="Enter the group you want to be added to (testgroup1 is used by the hostedversion)" 
            meta:resourcekey="Label2Resource1"></asp:Label>
    </div>
    <asp:Label ID="ErrorMessage" runat="server" 
        meta:resourcekey="ErrorMessageResource1" />
    </form>
</body>
</html>
