<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserPresence.ascx.cs" Inherits="Controls_UserPresence" EnableViewState="false" %>
<asp:HyperLink runat=server ID="hypMessengerIMControl" Visible=false Target=_blank NavigateUrl="http://settings.messenger.live.com/Conversation/IMMe.aspx?invitee={0}">
    <asp:Image runat=server ID="imgMessengerPresence" BorderWidth="0" ImageUrl="http://messenger.services.live.com/users/{0}/presenceimage" />            
</asp:HyperLink>