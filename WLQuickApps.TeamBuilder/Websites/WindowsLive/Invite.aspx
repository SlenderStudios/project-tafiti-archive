<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Invite.aspx.cs"
    Inherits="Invite" Title="Invite your friends" %>

<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <link href="App_Themes/Default/Invite.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript">
        function ToggleAllCheckboxes(chkbox) {
            var els = document.forms[0].elements;
            for (i=0; i<els.length; i++) {
                if (els[i].type == "checkbox" && els[i].disabled == false && els[i].id != chkbox.id) {
                    els[i].checked=chkbox.checked;
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="Content" runat="Server">
    <ul id="nav">
        <li><a href="Default.aspx">Home</a></li>
        <li><a href="Photos.aspx">Photos</a></li>
        <li><a href="Video.aspx">Video</a></li>
    </ul>
    <div id="subheader" class="clearfix">
        <div id="welcome">
            <h1>
                Invite your friends</h1>
            <asp:Panel ID="InviteRequest" runat="server">
                <p>
                    You will need to first click
                    <asp:HyperLink ID="AuthLink" runat="server">here</asp:HyperLink>
                    to allow us to read your contact list</p>
            </asp:Panel>
        </div>
    </div>
    <div id="content" class="clearfix">
        <asp:ListView ID="ListView" ItemPlaceholderID="ItemPlaceHolder" DataKeyNames="Name,Email,Enabled,Owner"
            runat="server">
            <LayoutTemplate>
                <div id="contacts-header">
                    <table cellspacing="0" cellpadding="0" border="0">
                        <thead>
                            <tr>
                                <th class="checkbox">
                                    <asp:CheckBox ID="CheckBox" AutoPostBack="false" onclick="ToggleAllCheckboxes(this)" runat="server" />
                                </th>
                                <th class="name-column">
                                    <span>Name</span>
                                </th>
                                <th class="mail-column">
                                    <span>Email</span>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div id="contacts-wrapper">
                    <table cellspacing="0" cellpadding="0" border="0">
                        <tbody>
                            <asp:PlaceHolder ID="ItemPlaceHolder" runat="server" />
                        </tbody>
                    </table>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td class="checkbox">
                        <asp:CheckBox ID="CheckBox" Enabled='<%#Eval("Enabled") %>' Visible='<%#((bool)Eval("AlreadyMember")==false) %>' AutoPostBack="false" runat="server" />

                        <!-- if the user is registered and is sharing their presence, show it and link to an IM Control conversation -->
                        <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible='<%#((string)Eval("PresenceID")!=String.Empty) %>'>
                            <a href="" onclick="window.open('http://settings.messenger.live.com/conversation/imme.aspx?invitee=<%#Eval("PresenceID")%>', '', 'height=300, width=300');">
                                <img alt="Messenger Presence" title="Click to start an IM conversation" src="http://messenger.services.live.com/users/<%#Eval("PresenceID")%>/presenceimage" />
                            </a>
                        </asp:PlaceHolder>
                    </td>
                    <td class="name-column">
                        <span><a href="#"><%#Eval("Name") %></a></span>
                    </td>
                    <td class="mail-column">
                        <span>
                            <%#Eval("Email") %><asp:label runat=server ID="lblAlreadyMember" Visible='<%#Eval("AlreadyMember") %>' Text=" - Already a member" />
                            </span>
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        <div style="padding-top: 10px;">
            <asp:Button ID="PostBack" Text="Invite (DEMO ONLY: we will not send email - but their address will be activated)" Visible="false" OnClick="PostBack_Click"
                runat="server" />
        </div>
    </div>
</asp:Content>
