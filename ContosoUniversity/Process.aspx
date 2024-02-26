<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Process.aspx.cs" Inherits="ContosoUniversity.Process" Theme="TextOnly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head id="Head1" runat="server">
    <title>Contoso University</title>
    <style>
        body { 
            background-image: url(app_themes/default/images/cu_logo_background.jpg); 
            background-repeat: no-repeat; 
            background-color: #E8E8E6;
            color: #5F2519;
            font-family: Tahoma;
            }
            a {color:#A4222C;}
    </style>
    
    <!-- HTTPS Tracking code 
    <script language="javascript" type="text/javascript" src="https://analytics.live.com/Analytics/msAnalytics.js"></script>
    <script language="javascript" type="text/javascript">
        msAnalytics.ProfileId = 'C43B';
        msAnalytics.TrackPage();
    </script>
    -->
    
</head>
<body>
    <form id="form1" runat="server">
        <div style="position: absolute; left:30px; top: 100px;">
            <b><asp:Label ID="Label1" runat="server"></asp:Label></b>
            <p>To revoke any permissions given, go to the <a href="https://consent.live.com/ManageConsent.aspx" target="_blank">Revocation Page</a>.</p>
            
            <p><asp:HyperLink ID="HyperLink1" runat="server">Go back to the Contoso University Home Page</asp:HyperLink></p>
        </div>
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </form>
</body>
</html>