<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Photos.aspx.cs"
    Inherits="Photos" Title="Photos" %>

<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <ul id="nav">
        <li><a href="Default.aspx">Home</a></li>
        <li><a href="Photos.aspx"><strong>Photos</strong></a></li>
        <li><a href="Video.aspx">Video</a></li>
    </ul>
    <div id="subheader" class="clearfix">
        <div id="welcome">
            <h1>
                Photos</h1>
            <p>
            </p>
        </div>
    </div>
    <div id="content" class="clearfix">
        <div id="content-left">
            <iframe marginwidth="0" marginheight="0" src="Slide.Show/Default.html" frameborder="0"
                width="640" height="480" scrolling="no"></iframe>
        </div>
        <div id="content-right">
            <asp:Panel ID="UploadPanel" runat="server">
                <asp:Label ID="AlbumLabel" AssociatedControlID="AlbumDropDown" Text="Album" runat="server" /><br />
                <asp:DropDownList ID="AlbumDropDown" DataTextField="Title" DataValueField="Link"
                    runat="server">
                </asp:DropDownList>
                <br />
                <asp:Label ID="TitleLabel" AssociatedControlID="TitleTextBox" Text="Title" runat="server" /><br />
                <asp:TextBox ID="TitleTextBox" TextMode="SingleLine" CssClass="lesstext" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="PhotoLabel" AssociatedControlID="PhotoUpload" Text="Photo" runat="server" /><br />
                <asp:FileUpload ID="PhotoUpload" CssClass="lesstext" runat="server" /><br />
                <br />
                <asp:Button ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" runat="server" />
            </asp:Panel>
        </div>
    </div>
</asp:Content>
