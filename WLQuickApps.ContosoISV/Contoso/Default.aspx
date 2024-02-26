<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Contoso.Default" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Register Assembly="RssToolkit" Namespace="RssToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Contoso Inc.</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <!-- AdCenter Analytics Beta -->
    <!-- Commented out every except the mslivelabs environment
    <script language="javascript" type="text/javascript" src="http://analytics.live.com/Analytics/msAnalytics.js"></script>
    <script language="javascript" type="text/javascript">
	    msAnalytics.ProfileId = 'C43A';
	    msAnalytics.TrackPage();
    </script>    
    -->
</head>
<body>
    <script language="javascript" type="text/javascript">
        var playVideos = "<%= GetLocalResourceObject("PlayVideos").ToString() %>";
    </script>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="myScriptManager" runat="server" >
        <Scripts>   
            <asp:ScriptReference Path="http://agappdom.net/h/silverlight.js" />
            <asp:ScriptReference Path="~/scripts/VideoGoRound.js" />
            <asp:ScriptReference Path="~/scripts/SilverlightContent.js" />
            <asp:ScriptReference Path="~/scripts/Default.aspx.js" />
            <asp:ScriptReference Path="~/scripts/AjaxControlToolkit.Compat.Timer.Timer.js" />
        </Scripts>
        <Services>
            <asp:ServiceReference Path="~/services/ContosoService.asmx" />
        </Services>
        </asp:ScriptManager>
        <cc1:RssDataSource ID="HomePageNewsDataSource" runat="server"
            Url="<%$ Resources:HomePageNewsFeed %>">
        </cc1:RssDataSource>
        <div id="Master">
            <div id="Logo"></div>
            <div id="NavBar">
                <div id="NavWelcome" style="background-image:url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_002.png);" 
                onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_002_roll.png)'" 
                onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_002.png)'"></div>
                <div id="NavDevelopment" style="background-image:url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_003.png);" 
                onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_003_roll.png)'" 
                onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_003.png)'"></div>
                <div id="NavSystems" style="background-image:url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_004.png);" 
                onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_004_roll.png)'" 
                onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_004.png)'"></div>
                <div id="NavCaseStudies" style="background-image:url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_005.png);" 
                onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_005_roll.png)'" 
                onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_005.png)'"></div>
                <div id="NavPartnerShips" style="background-image:url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_006.png);" 
                onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_006_roll.png)'" 
                onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_006.png)'"></div>
                <div id="NavContact" style="background-image:url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_007.png);" 
                onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_007_roll.png)'" 
                onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/nav_table_007.png)'"></div>
            </div>          
            <div id="SilverlightControlHost" class="silverlightHost"></div>
            <div id="SDControlHost" class="SDHost"></div>
            <div id="News">
                <div id="NewsTop">
                    <div id="NewsLabel" style="background-image:url(<%= GetLocalResourceObject("ImagePath").ToString()%>/home_news_title_001.png);"></div>
                </div>
                <div id="NewsLeft"></div>
                <div id="NewsLeftFill"></div>                
                <div id="NewsContent">
                    <asp:DataList ID="NewsItemsListk" runat="server" DataSourceID="HomePageNewsDataSource">
                        <ItemTemplate>
                            <h2>                                
                                <a href='<%# Eval("link")%>' target="_blank"><%# Eval("title")%></a>
                             </h2>
                            <div class="NewsItemContent"> <%# Eval("description")%> </div>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <hr />
                        </SeparatorTemplate>
                    </asp:DataList>
                </div>
                <div id="NewsRight"></div>
                <div id="NewsRightFill"></div>
                <div id="NewsBottom"></div>
            </div>
            <div id="Contact">
                <div id="ContactTop" style="background-image:url(<%= GetLocalResourceObject("ImagePath").ToString()%>/new_chat_header.png);"></div>
                <div id="ContactLeft"></div>
                <div id="ContactLeftFill"></div>
                <div id="ContactContent">
                    <div id="ChatPanel">
                        <iframe id="ChatFrame" runat="server" frameborder="0" style="width: 100%; height: 100%; border:none;"></iframe>
                    </div>      
                </div>
                <div id="ContactRight"></div>
                <div id="ContactRightFill"></div>
                <div id="ContactBottom"></div>  
            </div>
            <div id="FooterCopy">            
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:FooterCopy.Text%>" />
            </div>
         </div>
     </form>
</body>
</html>
