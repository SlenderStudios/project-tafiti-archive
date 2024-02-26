﻿<%@ Page Language="C#" AutoEventWireup="true" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<!-- saved from url=(0014)about:internet -->
<head>
    <title>Silverlight Project Test Page </title>

    <style type="text/css">
    html, body {
	    height: 100%;
	    overflow: auto;
    }
    body {
	    padding: 0;
	    margin: 0;
    }
    #silverlightControlHost {
	    height: 100%;
    }
    </style>
    
    <script type="text/javascript">
        function onSilverlightError(sender, args) {
        
            var appSource = "";
            if (sender != null && sender != 0) {
                appSource = sender.getHost().Source;
            } 
            var errorType = args.ErrorType;
            var iErrorCode = args.ErrorCode;
            
            var errMsg = "Unhandled Error in Silverlight 2 Application " +  appSource + "\n" ;

            errMsg += "Code: "+ iErrorCode + "    \n";
            errMsg += "Category: " + errorType + "       \n";
            errMsg += "Message: " + args.ErrorMessage + "     \n";

            if (errorType == "ParserError")
            {
                errMsg += "File: " + args.xamlFile + "     \n";
                errMsg += "Line: " + args.lineNumber + "     \n";
                errMsg += "Position: " + args.charPosition + "     \n";
            }
            else if (errorType == "RuntimeError")
            {           
                if (args.lineNumber != 0)
                {
                    errMsg += "Line: " + args.lineNumber + "     \n";
                    errMsg += "Position: " +  args.charPosition + "     \n";
                }
                errMsg += "MethodName: " + args.methodName + "     \n";
            }

            throw new Error(errMsg);
        }
    </script>
</head>

<body>
    <!-- Runtime errors from Silverlight will be displayed here.
	This will contain debugging information and should be removed or hidden when debugging is completed -->
	<div id='errorLocation' style="font-size: small;color: Gray;"></div>

    <div id="silverlightControlHost">
		<object data="data:application/x-silverlight," type="application/x-silverlight-2-b2" width="100%" height="100%">
			<param name="source" value="ClientBin/RetailSiteKitAction.xap"/>
			<param name="onerror" value="onSilverlightError" />
			<param name="background" value="white" />
			<param name="initParams" value='test=a,VideoAssetsURL=<%=System.Configuration.ConfigurationManager.AppSettings["VideoAssetsURL"].ToString()%>' />
			
			<a href="http://go.microsoft.com/fwlink/?LinkID=115261" style="text-decoration: none;">
     			<img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight" style="border-style: none"/>
			</a>
		</object>
		<iframe style='visibility:hidden;height:0;width:0;border:0px'></iframe>
    </div>
</body>
</html>