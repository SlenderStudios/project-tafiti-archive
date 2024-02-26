<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LatestVideoControl.ascx.cs"
    Inherits="Controls_LatestVideoControl" %>
    
<style type="text/css">

    #video-highlight
    {
        height: 67px;
        padding-bottom: 10px;
    }

    #video-highlight a img 
    {
        border: none;
    }

    #video-highlight img
    {
        float: left;
        margin: 0 10px 3px 0;
    }

    #video-highlight span
    {
        margin: 0.2em 0 0 0;
    }

</style>

<div id="video-highlight" class="clearfix">
    <a href="Video.aspx">
        <asp:Image ID="ThumbnailImage" runat="server" /></a>
    <a href="Video.aspx"><strong>
        <asp:Literal ID="Title" runat="server"></asp:Literal></strong></a>
    <p><asp:Literal ID="Description" runat="server"></asp:Literal></p>
</div>
