﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="upload.aspx.cs" Inherits="Default2" %>
        var currentCulture = "<%= Request.UserLanguages[0]%>";
        </script>
        <Services>
            <asp:ServiceReference Path="~/services/AlertService.asmx" />
        </Services>        
        <Scripts>
	       <asp:ScriptReference Path="~/js/Default.aspx.js" />
        </Scripts>
        </asp:ScriptManager>
SendMessage();
</script>