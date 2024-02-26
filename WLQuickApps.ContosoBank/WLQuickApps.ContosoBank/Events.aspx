<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True" CodeBehind="Events.aspx.cs" Inherits="WLQuickApps.ContosoBank.Events" Title="Australian Small Business Portal - Events" %>
<%@ Register src="controls/EventsListControl.ascx" tagname="EventsListControl" tagprefix="uc1" %>
<%@ Register src="controls/FeaturedEventControl.ascx" tagname="FeaturedEventControl" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="imagetoolbar" content="no" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.1" />
            <asp:ScriptReference Path="~/js/WLQuickApps.ContosoBank.Map.js" />
            <asp:ScriptReference Path="~/js/EventsListControl.ascx.js" />
            <asp:ScriptReference Path="~/js/Utility.js" />
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/Services/ContosoBankService.asmx" />
        </Services>    
    </asp:ScriptManagerProxy>
    
    <h1>Small Business Events around Australia</h1>   
    <uc1:EventsListControl ID="EventsListControl1" runat="server" />

    <h2 class="Medium">Featured Event</h2>
    <uc2:FeaturedEventControl ID="FeaturedEventControl1" runat="server" />    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="FloatingContent" runat="server">
    <div id="EventMapFrame" class="EventMapFrame">
        <div id="mymap" class="mymap" style="position:absolute;width:593px;height:454px;overflow:hidden;background:white;"></div>
        <div id="mymapminimaptoggle" class="mymapminimaptoggle mymapminimaptoggleEnabled" onclick="MapActionToggleMiniMap_onclick();"></div>
        <div id="mymapclose" class="mymapclose" onclick="MapActionCloseMap_onclick();">Close</div>
    </div> 
</asp:Content>

