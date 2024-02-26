<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
        <title />
        <!-- Include the Windows Live Controls (Contacts & Photos) -->
        <script type="text/javascript" src="http://controls.services.live.com/scripts/base/v0.3/live.js"></script>
        <script type="text/javascript" src="http://controls.services.live.com/scripts/base/v0.3/controls.js"></script>
        
        <!-- Include Virtual Earth 6.0 control -->
        <script type="Text/javascript" src="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6"></script>
        
        <!-- Set the favorite icon -->
        <link rel="shortcut icon" href="~/App_Themes/AWR/images/Favicon.ico" />
</head>
<body>
    <form id="mainform" runat="server">
        <asp:ScriptManager ID="_scriptManager" runat="server" EnablePageMethods="True" EnablePartialRendering="True" ScriptMode="Release">
            <Services>
                <asp:ServiceReference Path="~/SiteService.asmx" />
            </Services>
        </asp:ScriptManager>  

    <div id="wrapper">
        <div id="upper">
            <div id="upperLeft">
                <div id="logo">
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Default.aspx" ImageUrl="~/App_Themes/AWR/images/logo.png" CssClass="logo" Width="250" Height="45" />
                </div>
                    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Default.aspx" ImageUrl="~/App_Themes/AWR/images/community.gif" CssClass="community" Width="74" Height="14" />                                       
                    <asp:SiteMapDataSource ID="_topMenuDataSource" runat="server" StartingNodeUrl="~/Default.aspx#" ShowStartingNode="false" SiteMapProvider="AWRSiteMap" />
                    <asp:Menu runat="server" ID="_topMenu" DataSourceID="_topMenuDataSource" SkinID="topMenu" />                                

                    <asp:SiteMapDataSource ID="_mainMenuDataSource" runat="server" StartingNodeUrl="~/Landing.aspx#" ShowStartingNode="false" SiteMapProvider="AWRSiteMap" />                        
                    <asp:Menu runat="server" ID="_mainMenu" DataSourceID="_mainMenuDataSource" SkinID="mainMenu" />

                    <asp:SiteMapPath ID="_siteMapPath" runat="server" SkinID="breadcrumb" />
            </div>
            <div id="upperRight">
                <div id="loginStatus">
                <asp:LoginView ID="LoginView1" runat="server">
                    <AnonymousTemplate>
                        Please sign in.
                    </AnonymousTemplate>
                </asp:LoginView>
                    <asp:FormView runat="server" ID="_userDetailsForm" Visible="false" DefaultMode="ReadOnly">
                        <ItemTemplate>
                            <asp:Panel ID="_status" runat="server" CssClass="status">Signed in as:
                                <asp:HyperLink runat="Server" ID="_userNameLink" CssClass="link" NavigateUrl="~/Friend/ViewProfile.aspx" Text='<%# UserManager.LoggedInUser.Title %>' /></div>
                            </asp:Panel>  
                        </ItemTemplate>
                    </asp:FormView>
                    
                    <asp:Panel runat="server" ID="_liveAuthPanel" CssClass="liveAuth">
                        -<cc:LoginLink ID="_loginLink" runat="server" CssClass="liveAuth-link" />-
                    </asp:Panel>
                </div>
                                            
                <div id="search">
                    <cc:SecureTextBox ID="_searchTextBox" runat="server" SkinID="SearchBox" AutoPostBack="false" ValidationGroup="search" />
                    <ajaxToolkit:TextBoxWatermarkExtender runat="server" ID="_searchWatermark" TargetControlID="_searchTextBox" WatermarkText="Search" WatermarkCssClass="searchWatermark" />                        
                </div>
        </div>
       </div>     
        <div id="content">
           <div id="home-welcome"> 
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/AWR/images/home-image.png" CssClass="home-image" />
               <div class="home-text"> 
                    <p>
                        <strong>Come experience the very pinnacle of all-inclusive excellence</strong> - anywhere in the world at one of our 8 exclusive destinations. AdventureWorks Resorts delights couples in love with supremely luxurious accommodations, gourmet candlelit dining for two, gorgeous tropical settings, breathtaking mountain views, urban sights and sounds, and some of the world's most exquisite beaches.
                    </p>
                    <p>
                        <strong>Make a reservation today</strong> and ensure yourself a getaway like you've <em>never</em> experienced before.
                    </p>
                    <p>
                        <strong>Not convinced?</strong>  Come join our exciting new online community of vacationers who have learned about the magic of AdventureWorks Resorts, and let their photos and videos taken at our resorts speak for themselves.  And when you're ready to dive in, see what events your new friends are planning and come join the fun!
                    </p> 
               </div> 
           </div> 

              <div id="home-right">

                <cc:DropShadowPanel ID="DropShadowPanel1" runat="server" SkinID="home-rightPanel">
                    <asp:LoginView runat="server" ID="_loginView">
                        <AnonymousTemplate>                
                                <h4>Sign In:</h4>
                                <div style="text-align:center">
                                    <strong><asp:HyperLink runat="server" ID="_registerLink" NavigateUrl="~/Register.aspx" Text="Join Now!" CssClass="link2" /></strong><br />
                                    Already a user? <strong><asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/SignIn.aspx" Text="Sign in." CssClass="link2" /></strong><br /><br />
                                </div>          
                        </AnonymousTemplate>
                        <LoggedInTemplate>
                            <h4>Quick Links:</h4>
                            <asp:SiteMapDataSource ID="_menuDataSource" runat="server" ShowStartingNode="False" StartingNodeUrl="~/Friend/ViewProfile.aspx" SiteMapProvider="AWRSiteMap" />                
                            <div id="home-menu">         
                                <asp:Menu ID="_mainMenu" runat="server" SkinID="home-menu" DataSourceID="_menuDataSource"/>
                            </div>
                        </LoggedInTemplate>              
                    </asp:LoginView>
                </cc:DropShadowPanel>  
                         
                <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="home-users">
                <h4>New Users</h4>
               <div class="home-items">
                <cc:MetaGallery runat="server" ID="_mostRecentUsersGallery" DataSourceID="_mostRecentUsersDataSource" 
                    EmptyDataText="There are no recent users." RepeatColumns="3" ViewMode="Thumbnail" />
                <asp:ObjectDataSource runat="server" ID="_mostRecentUsersDataSource" TypeName="WLQuickApps.SocialNetwork.Business.UserManager" 
                    SelectMethod="GetMostRecentUsers">
                    <SelectParameters>
                        <asp:Parameter Name="startRowIndex" Type="Int32" DefaultValue="0" />
                        <asp:Parameter Name="maximumRows" Type="Int32" DefaultValue="6" />
                    </SelectParameters>
                </asp:ObjectDataSource>
               </div>  
                <asp:HyperLink ID="HyperLink4" runat="server" Text="View More" CssClass="home-links" NavigateUrl="~/Friend/Default.aspx" />     
            </cc:DropShadowPanel >
            <cc:DropShadowPanel ID="DropShadowPanel3" runat="server" SkinID="home-pictures">
                <h4>New Pictures</h4>
               <div class="home-items"> 
                <cc:MetaGallery runat="server" ID="_mostRecentPicturesGallery" RepeatColumns="3" ViewMode="Thumbnail"
                    DataSourceID="_mostRecentPicturesDataSource" EmptyDataText="No pictures have been added." />
                <asp:ObjectDataSource runat="server" ID="_mostRecentPicturesDataSource" TypeName="WLQuickApps.SocialNetwork.Business.MediaManager"
                    SelectMethod="GetMostRecentMedia">
                    <SelectParameters>
                        <asp:Parameter Name="mediaType" Type="Object" DefaultValue="Picture" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" DefaultValue="0" />
                        <asp:Parameter Name="maximumRows" Type="Int32" DefaultValue="6" />
                    </SelectParameters>
                </asp:ObjectDataSource>
               </div> 
               <asp:HyperLink ID="HyperLink5" runat="server" Text="View More" CssClass="home-links" NavigateUrl="~/Media/Default.aspx" />  
            </cc:DropShadowPanel>
            <cc:DropShadowPanel ID="DropShadowPanel4" runat="server" SkinID="home-videos">
                <h4>New Videos</h4>
               <div class="home-items"> 
                <cc:MetaGallery runat="server" ID="_mostRecentVideosGallery" DataSourceID="_mostRecentVideosDataSource"
                    EmptyDataText="No videos have been added." RepeatColumns="3" ViewMode="Thumbnail" />
                <asp:ObjectDataSource runat="server" ID="_mostRecentVideosDataSource" TypeName="WLQuickApps.SocialNetwork.Business.MediaManager"
                    SelectMethod="GetMostRecentMedia">
                    <SelectParameters>
                        <asp:Parameter Name="mediaType" Type="Object" DefaultValue="Video" />
                        <asp:Parameter Name="startRowIndex" Type="Int32" DefaultValue="0" />
                        <asp:Parameter Name="maximumRows" Type="Int32" DefaultValue="3" />
                    </SelectParameters>
                </asp:ObjectDataSource>
               </div> 
               <asp:HyperLink ID="HyperLink6" runat="server" Text="View More" CssClass="home-links" NavigateUrl="~/Media/Default.aspx" />   
             </cc:DropShadowPanel>
           </div>
          <div style="clear:both" />
            <div id="footer">
                <a href="http://dev.live.com/QuickApps/" target="_blank">Demonstration site</a> (<a href="http://www.codeplex.com/WLQuickApps/Release/ProjectReleases.aspx" target="_blank">source code</a>) | <asp:HyperLink ID="_privacyLink" runat="server" NavigateUrl="~/Privacy.aspx" Text="Privacy" />
                <asp:Label runat="server" ID="_hostingLabel" /> 
            </div>                
        </div>
    
    </div>
    
    <asp:Panel ID="_analyticsPanel" runat="server">
        <!-- 
            ***************************************************************
            ***                       IMPORTANT                         ***
            ***     Unless you specify the Analytics ID in the          ***
            ***     web.config this panel will not be rendered          ***
            ***************************************************************
        -->
        <script language="javascript" type="text/javascript" src="http://analytics.live.com/Analytics/msAnalytics.js"></script>
        <script language="javascript" type="text/javascript">
            msAnalytics.ProfileId = '<%= SettingsWrapper.MicrosoftAnalyticsID %>';
            msAnalytics.TrackPage();
        </script>
    </asp:Panel>
    </form>
</body>
</html>


