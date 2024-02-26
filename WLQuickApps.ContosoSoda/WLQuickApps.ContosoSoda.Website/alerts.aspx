<%@ Page Language="C#" AutoEventWireup="true" CodeFile="alerts.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cocoa-Soda World Record Breakers 2008</title>
</head>
<body>
 <script language="javascript" type="text/javascript">
        var currentCulture = "<%= Request.UserLanguages[0]%>";
 </script>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="myScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="~/services/AlertService.asmx" />
        </Services>        
        <Scripts>
	       <asp:ScriptReference Path="~/scripts/Default.aspx.js" />
        </Scripts>
        </asp:ScriptManager>
    </form>
    <script> 
SendMessage();
</script>
</body>
</html>
