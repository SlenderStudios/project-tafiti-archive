<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LiveSearch.ascx.cs" Inherits="ContosoUniversity.UserControls.LiveSearch" EnableViewState="false" %>

<asp:Panel ID="Panel1" runat="server" Height="50px" Width="125px">
    <!-- Live Search -->
    <meta name="Search.WLSearchBox" content="1.1, en-GB" />
    <div id="WLSearchBoxDiv">
    <table cellpadding="0" cellspacing="0" style="width: 180px">
        <tr id="WLSearchBoxPlaceholder">
            <td style="width: 100%; border: #DBD8D1 1px solid;">
                <input id="WLSearchBoxInput" type="text" value="&#x4c;&#x6f;&#x61;&#x64;&#x69;&#x6e;&#x67;&#x2e;&#x2e;&#x2e;" disabled="disabled" style="padding:0;background-image: url(http://search.live.com/s/siteowner/searchbox_background.png);background-position: right;background-repeat: no-repeat;height: 16px; width: 100%; border:none 0 Transparent" />
            </td>
            <td>
                <input id="WLSearchBoxButton" type="image" src="App_Themes/Default/images/go_off.gif" align="absBottom" style="padding:0;border-style: none" />
            </td>
        </tr>
    </table>
	    <script type="text/javascript" charset="utf-8">
	    var WLSearchBoxConfiguration=
	    {
		    "global":{
			    "serverDNS":"search.live.com",
			    "market":"en-GB"
		    },
		    "appearance":{
			    "autoHideTopControl":false,
			    "width":600,
			    "height":400,
			    "theme":"Yellow"
		    },
		    "scopes":[
			    {
				    "type":"web",
				    "caption":"&#x57;&#x65;&#x62;",
				    "searchParam":""
			    }
		    ]
	    }
	    </script>
	    <script type="text/javascript" charset="utf-8" src="http://search.live.com/bootstrap.js?market=en-GB&ServId=SearchBox&ServId=SearchBoxWeb&Callback=WLSearchBoxScriptReady"></script>
    </div>
    <!-- Live Search -->
</asp:Panel>
<asp:Label ID="Label1" runat="server" Text="Label">Disabled when running locally</asp:Label>