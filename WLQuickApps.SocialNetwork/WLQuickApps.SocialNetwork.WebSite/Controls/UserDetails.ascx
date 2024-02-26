<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserDetails.ascx.cs" Inherits="UserDetails" %>

<%@ Register src="UserPresence.ascx" tagname="UserPresence" tagprefix="uc1" %>

<div style="float:left">
    <cc:NullablePicture ID="_userPicture" runat="server" 
        MaxHeight="256" MaxWidth="256" NullImageUrl="~/Images/missing-user-256x256.png" />
</div>
<cc:DropShadowPanel runat="server" SkinID="UserDetails-Description">
    <asp:Label ID="UserNameLabel" runat="server" SkinID="UserDetails-Title" /><br />
    <cc:DropShadowPanel runat="server" SkinID="UserDetails-Description">
        <strong>
            <asp:Label ID="FirstNameLabel" runat="server" />
            <asp:Label ID="LastNameLabel" runat="server" />
            <uc1:UserPresence ID="UserPresence1" runat="server"  />
            <br />
        </strong>
        <strong>born</strong>
            <asp:Label ID="DateOfBirthLabel" runat="server" /><br />
        <strong>last seen</strong>
            <asp:Label ID="LastLoginDateLabel" runat="server" /> <asp:Label ID="_onlineLabel" runat="server" Visible="false" Text="(Online now)" /><br />
        <strong>joined</strong>
            <asp:Label ID="CreateDateLabel" runat="server" /><br />
        <asp:Panel runat="server" ID="GenderPanel">
            <strong>is</strong>
                <asp:Label ID="GenderLabel" runat="server" />
        </asp:Panel>
    </cc:DropShadowPanel>
</cc:DropShadowPanel>


