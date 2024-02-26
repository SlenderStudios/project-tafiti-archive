<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PresenceControl.ascx.cs"
    Inherits="PresenceControl" %>
<style type="text/css">
    
    .__presence
    {
        list-style-type: none;
        display: none;
        border: solid 1px;
        background-color: #fff;
        position: absolute;
        _overflow: hidden;
        top: 30px;
        right: 0;
        width: 150px;
        padding: 0;
        margin: 0;
        z-index: 100;
    }
    
    .__presence li
    {
        border: none;
        display: block;
        line-height: 1.5em;
        text-align: left;
        width: 95%;
        margin: 0 4px 0 0;
        padding: 0.25em 0.60em 0.35em 0.8em;
        overflow: hidden;
        float: left;
    }

    .__presence li a
    {
        display: inline;
        line-height: 1.5em;
        background-position: left center;
        background-repeat: no-repeat;
        padding: 0 0 0 20px;
        margin: 0;
    }
    
    .on .__presence
    {
        display: block;
        z-index: 200;
    }

</style>

<script type="text/javascript">

    function clickDropDown(el) {
        var pn = el.parentNode;
        function show() {
            document.onclick=hide;
            Sys.UI.DomElement.addCssClass(pn, "on");
        }
        function hide() {
            Sys.UI.DomElement.removeCssClass(pn, "on");
            document.onclick=null;
        }
        if (Sys.UI.DomElement.containsCssClass(pn, "on"))
            hide();
        else
            setTimeout(show, 0);
        return false;
    }
    
    function showPresenceSettings() {
        var el = document.getElementById("ctl00_PresencePanel");
        el.style.display = "block";
    }
    
</script>

<asp:LinkButton ID="IMPresence" OnClientClick="return clickDropDown(this);" runat="server">Who's online<span></span></asp:LinkButton>
<asp:ListView ID="ListView" ItemPlaceholderID="ItemPlaceHolder" runat="server">
    <LayoutTemplate>
        <ul class="__presence">
            <asp:PlaceHolder ID="ItemPlaceHolder" runat="server" />
            <li><asp:LinkButton ID="PresenceLink" OnClientClick="showPresenceSettings(); return false;" runat="server">Settings...</asp:LinkButton></li>
        </ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li><a href='http://settings.messenger.live.com/Conversation/IMMe.aspx?invitee=<%# Eval("id") %>'
            target="_blank" onclick="javascript:window.open(this.href, '_blank', 'left=50px,top=50px,height=300px,width=300px'); return false;"
            style='background-image: url(http://messenger.services.live.com/users/<%# Eval("id") %>/presenceimage)'>
            <%#Eval("DisplayName") %></a></li>
    </ItemTemplate>
    <EmptyDataTemplate>
        <ul class="__presence">
            <li><asp:LinkButton ID="PresenceLink" OnClientClick="showPresenceSettings(); return false;" runat="server">Settings...</asp:LinkButton></li>
        </ul>
    </EmptyDataTemplate>
</asp:ListView>
