<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Video.aspx.cs"
    Inherits="Video" Title="Video" %>

<%@ Register Assembly="Microsoft.Live.ServerControls" Namespace="Microsoft.Live.ServerControls"
    TagPrefix="live" %>
<asp:Content ID="Head" ContentPlaceHolderID="Head" runat="server">
    <link href="App_Themes/Default/Video.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content" runat="server">
    <ul id="nav">
        <li><a href="Default.aspx">Home</a></li>
        <li><a href="Photos.aspx">Photos</a></li>
        <li><a href="Video.aspx"><strong>Video</strong></a></li>
    </ul>
    <div id="subheader" class="clearfix">
        <div id="welcome">
            <h1>
                Video</h1>
            <p>
            </p>
        </div>
    </div>
    <div id="content" class="clearfix">
        <div id="content-left">
            <live:SilverlightStreamingMediaPlayer ID="MediaPlayer" runat="server"
                Height="320px" Width="430px" AutoPlay="True" 
                MediaSkinSource="~/Xaml/Simple.xaml" Windowless="True">
            </live:SilverlightStreamingMediaPlayer>
            <asp:ListView ID="FileSetList" ItemPlaceholderID="FileSetItem" OnItemCommand="FileSetList_ItemCommand"
                DataKeyNames="FileSet" runat="server">
                <LayoutTemplate>
                    <ul id="video-list" class="clearfix">
                        <asp:PlaceHolder ID="FileSetItem" runat="server" />
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li>
                        <div class="video-thumbnail">
                            <!-- <asp:CheckBox ID="VideoDelete" /> -->
                            <a href='<%#Eval("MediaSource") %>' onclick='ctl00_Content_MediaPlayer_set_mediaUrl("<%#Eval("MediaSource") %>"); return false;'>
                                <img src='<%#Eval("Thumbnail") %>' alt='<%#Eval("Title") %>' /></a>
                            <a href='<%#Eval("MediaSource") %>' onclick='ctl00_Content_MediaPlayer_set_mediaUrl("<%#Eval("MediaSource") %>"); return false;'>
                                <strong><%#Eval("Title") %></strong></a>
                            <p>
                                <%#Eval("Description") %></p>
                            <!--
                            <asp:LinkButton ID="DeleteButton" CommandName="Delete" CommandArgument='<%#Eval("FileSet") %>'
                                runat="server">Delete</asp:LinkButton> -->
                        </div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <div id="content-right">
            <asp:Panel ID="UploadPanel" runat="server">
                <asp:Label ID="TitleLabel" AssociatedControlID="TitleTextBox" Text="Title" runat="server" /><br />
                <asp:TextBox ID="TitleTextBox" TextMode="SingleLine" CssClass="lesstext" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="DescLabel" AssociatedControlID="DescTextBox" Text="Description" runat="server" /><br />
                <asp:TextBox ID="DescTextBox" TextMode="MultiLine" CssClass="lesstext" runat="server"></asp:TextBox>
                <br />
                <asp:Label ID="ThumbUploadLabel" AssociatedControlID="ThumbUpload" Text="Thumbnail" runat="server" /><br />
                <asp:FileUpload ID="ThumbUpload" CssClass="lesstext" runat="server" /><br />
                <br />
                <asp:Label ID="FileUploadLabel" AssociatedControlID="FileUpload" Text="Video" runat="server" /><br />
                <asp:FileUpload ID="FileUpload" CssClass="lesstext" runat="server" /><br />
                <br />
                <asp:Button ID="UploadButton" Text="Upload" OnClick="UploadButton_Click" runat="server" />
            </asp:Panel>
            <p><img src="Images/Silverlight.png" alt="Powered by Silverlight" /></p>
        </div>
    </div>
</asp:Content>
