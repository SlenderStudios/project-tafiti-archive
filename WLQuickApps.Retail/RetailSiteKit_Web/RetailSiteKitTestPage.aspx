<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head runat="server">
    <title>Test Page For RetailSiteKit</title>
    <style type="text/css">
        .footer
        {
            width: 90%;
            z-index: 999;
            color: #93958a;
            font-size: 10pt;
            bottom: 10px;
        }
    </style>
    <script language="javascript">
		    var strDownloadUrl = "/Download";
		    var strActiveXHTML = '<OBJECT height="40" width="410" ID="MsnMessengerSetupDownloadControl" CLASSID="CLSID:B38870E4-7ECB-40DA-8C6A-595F0A5519FF" codebase="../download/MsnMessengerSetupDownloader.cab#version=1,0,0,3" VIEWASTEXT><PARAM name="Url" value=""></OBJECT>';
		    function LaunchDownload(){location.href=strDownloadUrl;}
    </script>

    <script type="text/javascript" lang="javascript" src="http://messenger.msn.com/Resource/P4AppLaunch.js"></script>

    <script type="text/javascript" lang="javascript">
		strUrlExtension = '?';
		strLcid = "1033";
		strHost = "messenger.msn.com";
		strBaseUrl = "";
		fBrowser = true;
		appId = '99995719';
		emailId = "";
		
    </script>
</head>
<body style="height: 100%; margin: 0;">
    <form id="form1" runat="server" style="height: 100%;">
    <div id="silverlightControlHost" style="width:100%; height:100%;">
        <object data="data:application/x-silverlight," type="application/x-silverlight-2-b2"
            width="100%" height="100%">
            <param name="source" value="ClientBin/RetailSiteKit.xap" />
            <param name="onerror" value="onSilverlightError" />
            <param name="background" value="black" />
            <param name="initParams" value="test=a,video=<%=System.Configuration.ConfigurationManager.AppSettings["VideoAssetsURl"].ToString()%>,StaticAssetsURL=<%=System.Configuration.ConfigurationManager.AppSettings["StaticAssetsURL"].ToString()%>,Appid=<%=System.Configuration.ConfigurationManager.AppSettings["wll_appid"].ToString()%>" />
            <a href="http://go.microsoft.com/fwlink/?LinkID=116569" style="text-decoration: none;">
                <img src="http://go.microsoft.com/fwlink/?LinkId=116569" alt="Get Microsoft Silverlight"
                    style="border-style: none" />
            </a>
        </object>
        <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
    </div>
    <div class="footer">
        <div style="text-align: center;">
            This is a <a href="http://dev.live.com/QuickApps/">demonstration site</a>.
            <br />
            <br />
            The source code can be downloaded from the <a href="http://www.codeplex.com/WLQuickApps/Release/ProjectReleases.aspx">
                Windows Live Platform Quick Apps</a> CodePlex Project.<br />
            <br />
            The example companies, organizations, products, domain names, e-mail addresses,
            logos, people, places, and events depicted herein are fictitious. No association
            with any real company, organization, product, domain name, email address, logo,
            person, places, or events is intended or should be inferred.
        </div>
    </div>
    </form>
</body>
</html>
