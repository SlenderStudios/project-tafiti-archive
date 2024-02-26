<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True" CodeBehind="Videos.aspx.cs" Inherits="WLQuickApps.ContosoBank.Videos" Title="Australian Small Business Portal - Latest Videos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register assembly="Microsoft.Live.ServerControls" namespace="Microsoft.Live.ServerControls" tagprefix="live" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="/css/Videos.css" type="text/css" />
<!--[if lt IE 7]>
    <link rel="stylesheet" href="/css/VideosIE6.css" type="text/css" />
<![endif]-->  
<script type="text/javascript">
    function updatevideo(url)
    {
        var slMediaPlayer = $find("<%=CurrentSilverlightStreamingMediaPlayer.ClientID %>");
        slMediaPlayer.set_mediaSource(url);
        slMediaPlayer.set_autoPlay(true);
        return false;
    }
</script></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 class="Large">Videos</h1>
    <div class="VideoFrame">
                <live:SilverlightStreamingMediaPlayer ID="CurrentSilverlightStreamingMediaPlayer" 
            runat="server" 
            MediaSource="streaming:/56451/ButterflyEncoded/Butterfly.wmv" 
            MediaSourceProvider="SilverlightStreaming" Width="600px" Height="454px" MediaSkinSource="~/css/player.xml"
            Windowless="true" BorderStyle="None">
        </live:SilverlightStreamingMediaPlayer>  

    </div>
    <h2 class="Medium">Latest Videos</h2>
    <div style="width:700px">  
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>      
                <div class="VideoDataList">     
                    <asp:DataList ID="VideoDataList" runat="server" RepeatColumns="4" 
                            RepeatDirection="Horizontal" DataSourceID="LinqDataSource1" 
                        CellPadding="0" ShowFooter="False" ShowHeader="False">
                        <ItemStyle CssClass="VideoPortalContent" Font-Bold="False" 
                            Font-Italic="False" Font-Overline="False" Font-Strikeout="False" 
                            Font-Underline="False" />
                        <ItemTemplate>
                            <div class="VideoWrapper">
                                <div class="VideoImageWrapper">
                                    <asp:ImageButton ID="VideoImage" runat="server" CssClass="VideoSideImage" AlternateText='<%# Bind("VideoTitle") %>' ImageUrl='<%# Bind("FrameImage") %>' OnClientClick='<%# "return updatevideo(\"" + DataBinder.Eval(Container.DataItem,"VideoURL").ToString() + "\");" %>' /><br />  
                                </div>
                                <div class="VideoRatingBorder">
                                    <div class="VideoRating">
                                        <cc1:Rating ID="Rating1" runat="server" RatingAlign="Horizontal" RatingDirection="LeftToRightTopToBottom" StarCssClass="RatingStar" EmptyStarCssClass="RatingEmpty" FilledStarCssClass="RatingFilled" WaitingStarCssClass="RatingSaved" CurrentRating='<%# Bind("Rating") %>'></cc1:Rating>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>          
                    <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
                        ContextTypeName="WLQuickApps.ContosoBank.ContosoBankDataContext" 
                        OrderBy="UploadDate desc" TableName="Videos">
                    </asp:LinqDataSource>
                </div> 
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </asp:Content>
