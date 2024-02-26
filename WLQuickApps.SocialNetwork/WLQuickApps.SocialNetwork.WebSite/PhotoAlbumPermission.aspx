<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhotoAlbumPermission.aspx.cs" Inherits="Media_PhotoAlbumPermission" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>Site Permission Page to Edit Photo from User's Space</title>
    </head>
    <body>
    <asp:Panel runat="server" ID="pnlCloseAndRefresh" Visible="false">
        <script language="javascript" type="text/javascript">
            // Refresh the parent
            window.opener.photoPermissionSet('<asp:Literal runat=server ID="litDomainAuthenticationToken" />', '<asp:Literal runat=server ID="litOwnerHandle" />');

            // close this window.
            window.close();
        </script>
    </asp:Panel>
    
    
    </body>
</html>
