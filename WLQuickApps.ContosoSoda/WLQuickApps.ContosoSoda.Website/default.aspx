<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cocoa-Soda World Record Breakers 2008</title>
    <link href="css/style.css" type="text/css" rel="stylesheet" media="all">

    <script type="text/javascript">
	<!--
	function openNewWindow(URLtoOpen, windowName, windowFeatures) { 
	newWindow=window.open(URLtoOpen, windowName, windowFeatures);
	}
	//-->
</script>

    <script type="text/javascript" src="js/swfobject.js"></script>

    <script type="text/javascript" src="js/showvideo.js"></script>

    <script type="text/javascript">
	SaveCookie('CONTENTID', "testing WL Function", 10);
</script>

    <%--<script src="http://www.google-analytics.com/urchin.js" type="text/javascript"></script>
<script type="text/javascript">
_uacct = "UA-4076261-1";
urchinTracker();
</script>--%>

    <script language="javascript" type="text/javascript" src="http://analytics.live.com/Analytics/msAnalytics.js"></script>

    <script language="javascript" type="text/javascript">
        msAnalytics.ProfileId = 'CF8F';
        msAnalytics.TrackPage();
    </script>

</head>
<body bgcolor="#FFFFFF">
    <div align="center">
        <table width="990" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td>
                    <div id="flashcontent">
                        <h3>
                            You need to upgrade your Flash Player</h3>
                        <form id="form1" runat="server">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:void(0);" onclick="window.open('join/join.aspx','_blank','height=500, location=0, menubar=0, resizable=0, scrollbars=0, status=0, titlebar=0, toolbar=0, width=700');">Come join my team</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="javascript:void(0);" onclick="window.open('vote/vote.aspx','_blank','height=500, location=0, menubar=0, resizable=0, scrollbars=0, status=0, titlebar=0, toolbar=0, width=700');">Gets my vote</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="HyperLink7" runat="server" NavigateUrl="http://messenger.msn.com/Resource/games.aspx?appID=10331527"
                            Target="_blank">Watch with a friend</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="javascript:void(0);" onclick="window.open('chat.aspx','_blank','height=500, location=0, menubar=0, resizable=0, scrollbars=0, status=0, titlebar=0, toolbar=0, width=700');">Chat with me</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="http://signup.alerts.live-ppe.com/alerts/login.do?PINID=28161233&returnURL=http://www.kcptest.com"
                            Target="_blank">Get alerts for new videos</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="javascript:void(0);" onclick="window.open('contact/contact.aspx','_blank','height=500, location=0, menubar=0, resizable=0, scrollbars=0, status=0, titlebar=0, toolbar=0, width=700');">Share with a friend</asp:HyperLink>
                        <br />
                        <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/upload.aspx">Upload your video</asp:HyperLink>
                        </form>
                    </div>

                    <script type="text/javascript">
			// <![CDATA[
			var so = new SWFObject("swf/flash_home.swf"+"?v="+Math.random(), "home", "1002", "650", "8", "#ffffff"); // swf, id, width, height, version, background-color
			//so.addParam("wmode", "transparent");
			so.addParam("scale", "noscale");
			so.useExpressInstall("swf/expressinstall.swf");
			so.write("flashcontent");
			// ]]>
		</script>

                </td>
            </tr>
        </table>
    </div>
</body>
</html>
