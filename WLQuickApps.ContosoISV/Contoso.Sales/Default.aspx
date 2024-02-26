<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Contoso.Sales._Default" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Sales Activity</title>
    <link type="text/css" rel="stylesheet" href="css/style.css" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript" src="http://dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6"></script> 
    <!-- AdCenter Analytics Beta -->
    <!-- commented out except the hosted environment
    <script language="javascript" type="text/javascript" src="http://analytics.live.com/Analytics/msAnalytics.js"></script>
    <script language="javascript" type="text/javascript">
	    msAnalytics.ProfileId = 'C43A';
	    msAnalytics.TrackPage();
    </script>
    --->
</head>
<body>
    <script language="javascript" type="text/javascript">
        var routeError = "<%= GetLocalResourceObject("RouteError.Text").ToString() %>";
        var distanceText = "<%= GetLocalResourceObject("Distance.Text").ToString() %>";
        var defaultLatLong = [<%= GetLocalResourceObject("DefaultLatLong").ToString() %>];
        var officeLatLong = [<%= GetLocalResourceObject("OfficeLatLong").ToString() %>];
        var currentCulture = "<%= Request.UserLanguages[0]%>";
        var appointmentSentText = "<%= GetLocalResourceObject("AppointmentSent.Text").ToString() %>";
        var clientTitleText = "<%= GetLocalResourceObject("ClientTitle.Text").ToString() %>";
    </script>
    <form id="MainForm" runat="server">
        <asp:ScriptManager ID="myScriptManager" runat="server">
        <Services>
            <asp:ServiceReference Path="~/services/AlertService.asmx" />
        </Services>        
        <Scripts>
	       <asp:ScriptReference Path="~/scripts/Default.aspx.js" />
	       <asp:ScriptReference Path="~/scripts/Contoso.ModalPopup.js" />
	       <asp:ScriptReference Path="~/scripts/AjaxControlToolkit.Common.Common.js" />
        </Scripts>
        </asp:ScriptManager>
        <div id="HeaderPanel" style="background-image: url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_header_bg_001.png);">
            <div id="HeaderNavBar">
                <div id="NavOffice" style="background-image: url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_office_button_001.png);"
                    onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_office_button_roll_001.png)'"
                    onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_office_button_001.png)'">
                </div>
                <div id="NavDiary" style="background-image: url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_diary_button_001.png);"
                    onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_diary_button_roll_001.png)'"
                    onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_diary_button_001.png)'">
                </div>
                <div id="NavDirection" style="background-image: url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_directions_button_001.png);"
                    onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_directions_button_roll_.png)'"
                    onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_directions_button_001.png)'">
                </div>
                <div id="NavSend" style="background-image: url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_send_button_001.png);"
                    onmouseover="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_send_button_roll_001.png)'"
                    onmouseout="this.style.backgroundImage = 'url(<%= GetLocalResourceObject("ImagePath").ToString()%>/act_send_button_001.png)'">
                </div>
            </div>
            <input type="text" id="SearchText" />
            <div id="SearchGO"></div>
        </div>
        <div id="myMap" style="position:absolute;width:480px;height:450px;"></div>
        <div id="CalendarPanel">
            <div id="CalendarPanelClose"></div>
            <asp:UpdatePanel ID="CalendarUpdatePanel" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <input id="CalendarSelectedDate" runat="server" type="hidden" >
                        <asp:Calendar ID="myCalendar" runat="server" BackColor="Transparent" 
                            BorderStyle="None" CellPadding="1" CssClass="Calendar" Height="210px" 
                            meta:resourcekey="myCalendarResource1" OnDayRender="myCalendar_DayRender" 
                            OnSelectionChanged="myCalendar_SelectionChanged" Width="404px">
                            <DayHeaderStyle CssClass="DayHeaderStyle" />
                            <DayStyle CssClass="DayStyle" />
                            <NextPrevStyle CssClass="NextPrevStyle" />
                            <OtherMonthDayStyle CssClass="OtherMonthDayStyle" />
                            <SelectedDayStyle BackColor="Transparent" CssClass="SelectedDayStyle" 
                                ForeColor="Black" />
                            <TitleStyle BackColor="Transparent" CssClass="TitleStyle" />
                        </asp:Calendar>
                        <div id="AppointmentCount">
                            <asp:Label ID="AppointmentCountLabel" runat="server" 
                                meta:resourcekey="AppointmentCountLabelResource1"></asp:Label>
                        </div>
                    </input>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="CustomerDetailsPanel">
            <div id="CustomerDetailsPanelClose"></div>
            <div id="CustomerDetailsPanelContent">
                <div><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:ClientName.Text%>" /></div><div><input ID="ClientName" type="text" /></div>
                <div><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:ClientCompany.Text%>" /></div><div><input ID="ClientCompany" type="text" /></div>
                <div><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:ClientPhone.Text%>" /></div><div><input ID="ClientPhone" type="text" /></div>
                <div><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:ClientAddress.Text%>" /></div><div><input ID="ClientAddress" type="text" /></div>
                <div id="CustomerSearchGO"></div>
            </div>
        </div>
       <div id="DrivingDirectionsPanel">
            <div id="DrivingDirectionsPanelClose"></div>
            <div id="DrivingDirectionsPanelContent"></div>
        </div>        
    </form>
</body>
</html>