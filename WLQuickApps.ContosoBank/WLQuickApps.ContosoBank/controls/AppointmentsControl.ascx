<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="AppointmentsControl.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.AppointmentsControl" %>

<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    <Services>
        <asp:ServiceReference Path="~/services/ContosoBankService.asmx" />
    </Services>
    <Scripts>
        <asp:ScriptReference Path="~/js/AppointmentsControl.ascx.js" />
        <asp:ScriptReference Path="~/js/Utility.js" />
    </Scripts>
</asp:ScriptManagerProxy>
    <div class="CalendarLegend"></div>
    <div class="CalendarWrapper">
        <asp:Calendar ID="AppointmentCalendar" runat="server" 
            ondayrender="AppointmentCalendar_DayRender" BorderStyle="None" 
            DayNameFormat="Full" NextMonthText="| next month &gt;" 
            PrevMonthText="&lt; prev month |" TitleFormat="Month" 
            CssClass="AppointmentTable" CellPadding="0">
            <DayStyle CssClass="CalendarDay" />
            <NextPrevStyle Font-Bold="True" Font-Size="9px" ForeColor="#014F59" 
                HorizontalAlign="Right" Width="200px" />
            <DayHeaderStyle CssClass="CalendarDayHeader" />
            <TitleStyle BackColor="Transparent" CssClass="CalendarTitle" 
                HorizontalAlign="Center" />
        </asp:Calendar>
    </div>
