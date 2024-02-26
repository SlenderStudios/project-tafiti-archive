<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ContosoUniversity.Default"
    Theme="Default" EnableViewState="false" %>

<%@ Register Src="~/UserControls/ContactsControl.ascx" TagName="ContactsControl"
    TagPrefix="LQA" %>
<%@ Register Src="~/UserControls/LatestNewsControl.ascx" TagName="LatestNewsControl"
    TagPrefix="LQA" %>
<%@ Register Src="~/UserControls/EventsControl.ascx" TagName="EventsControl" TagPrefix="LQA" %>
<%@ Register Src="~/UserControls/FeaturedSpacesControl.ascx" TagName="FeaturedSpacesControl"
    TagPrefix="LQA" %>
<%@ Register Src="~/UserControls/PromotionalPhotoControl.ascx" TagName="PromotionalPhotoControl"
    TagPrefix="LQA" %>
<%@ Register Src="~/UserControls/LiveSearch.ascx" TagName="LiveSearch" TagPrefix="LQA" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Contoso University</title>
    <!-- Tracking Code 
    <script language="javascript" type="text/javascript" src="http://analytics.live.com/Analytics/msAnalytics.js"></script>
    <script language="javascript" type="text/javascript">
        msAnalytics.ProfileId = 'C43B';
        msAnalytics.TrackPage();
    </script>
    -->
</head>
<body id="Home" onload="OnLoad();">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" EnablePartialRendering="true" runat="server">
        <Scripts>
            <asp:ScriptReference Path="js/Default.js" />
            <asp:ScriptReference Path="js/Cookies.js" />
        </Scripts>
    </asp:ScriptManager>
    <div id="MasterContainer">
        <div id="SignIn">
            <iframe id="Iframe1" name="WebAuthControl" src="http://login.live.com/controls/WebAuth.htm?appid=<%=AppId%>"
                width="70px" height="18px" marginwidth="0" marginheight="0" align="middle" frameborder="0"
                scrolling="no"></iframe>
        </div>
        <LQA:PromotionalPhotoControl ID="PromotionalPhotoControl1" runat="server" />
        <div id="Search">
            <LQA:LiveSearch ID="LiveSearch1" runat="server" />
        </div>
        <div id="TVWrapper">
            <iframe id="VideoFrame" frameborder="no" scrolling="no" src="http://silverlight.services.live.com/invoke/4650/Drey/iframe.html">
            </iframe>
        </div>
        <input type="button" id="channel0" value="1" onclick="playVideo('0')" />
        <input type="button" id="channel1" value="2" onclick="playVideo('1')" />
        <input type="button" id="channel2" value="3" onclick="playVideo('2')" />
        <%--<LQA:RoomMateControl ID="RoomMateControl1" runat="server" />--%>
        <div id="ExpoContent">
        </div>
        <div id="Today">
        </div>
        <div id="AlertSignup">
            <a href="http://signup.alerts.live.com/alerts/login.do?PINID=41271940&returnURL=http://contosouniversity.mslivelabs.com/default.aspx?alertsignup=true">
                <img src="App_Themes/Default/images/alerts_button.gif" alt="Stay up to date by using Windows Live Alerts."
                    border="0" /></a></div>
        <a id="NewsSeeAll" target="_blank" href="http://contosouniversity.spaces.live.com/?_c11_BlogPart_BlogPart=blogview&_c=BlogPart&partqs=cat%3dNews">
            See all</a> <a id="TextOnly" href="TextOnly.aspx">Text Only</a>
        <div id="Events">
            <LQA:EventsControl ID="EventsControl1" runat="server" />
        </div>
        <div id="FeaturedSpaces">
            <LQA:FeaturedSpacesControl ID="FeaturedSpacesControl1" runat="server" />
        </div>
        <div id="LatestNews">
            <LQA:LatestNewsControl ID="LatestNewsControl1" runat="server" />
        </div>
        <%--    <div id="MarketPlace">
        <LQA:MarketPlaceControl ID="MarketPlaceControl1" runat="server" />
    </div>--%>
        <div id="Contacts">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <LQA:ContactsControl ID="ContactsControl1" PageSize="5" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <asp:Panel ID="pnlWhereAretheyNow" runat="server" Visible="false">
            <div id="WhereAreTheyNow">
                <a href="#" onclick="javascript:window.open('wherearetheynow.aspx','_blank','width=912,height=660,menubar=no,resizable=no,scrollbars=no,status=no,location=no')"
                    onmouseover="SwapImage(true)" onmouseout="SwapImage(false)">
                    <img id="watn" src="App_Themes/Default/images/watn_button_off.png" border="0" alt="Click here to map your contacts" />
                </a>
            </div>
        </asp:Panel>
        <div id="footer">
            This is a <a href="http://dev.live.com/QuickApps/" target="_blank">demonstration site</a>.
            <br />
            The source code can be downloaded from the <a href="http://www.codeplex.com/WLQuickApps/Release/ProjectReleases.aspx"
                target="_blank">Windows Live Platform Quick Apps</a> CodePlex Project.</div>
    </div>
    </form>
</body>
</html>
