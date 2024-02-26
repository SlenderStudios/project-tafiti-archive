<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Photos.aspx.cs" Inherits="ContosoBicycleClub.Photos" Theme="" %>

<%@ Register 
    Assembly="AjaxControlToolkit" 
    Namespace="AjaxControlToolkit" 
    TagPrefix="ajaxToolkit" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body style="margin:0px;" >
    <form id="form1" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        
        <Scripts>
         
        </Scripts>
            
        </asp:ScriptManager>
    <div id="SlideShow">
            <asp:Image ID="Slides" runat="server" Height="240" 
                 ImageUrl="images/blank_slide.png"
                AlternateText="" />
   
            <ajaxToolkit:SlideShowExtender ID="slideshowextend1" runat="server" 
                TargetControlID="Slides"
                SlideShowServiceMethod="GetSlides" 
                AutoPlay="true" 
                UseContextKey = "true"
                
                
       
                Loop="true" />
                </div>

    </form>
</body>
</html>
