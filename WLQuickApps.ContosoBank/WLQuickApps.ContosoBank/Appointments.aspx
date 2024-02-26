<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True" CodeBehind="Appointments.aspx.cs" Inherits="WLQuickApps.ContosoBank.Appointments" Title="Australian Small Business Portal - Advisor Appointments" %>
<%@ Register src="controls/AppointmentsControl.ascx" tagname="AppointmentsControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="/css/Appointments.css" type="text/css" />
<!--[if lt IE 7]>
    <link rel="stylesheet" href="/css/AppointmentsIE6.css" type="text/css" />
<![endif]-->  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Appointment Calendar - <%=GetAdvisorName()%></h1>
    <p>
        Mouse over days and click on a free appointment slot - you will then be prompted 
        to provide your information. An advisor will contact you soon, or, if you prefer,click one of the buttons below to contact them.
    </p>
    <div class="CalendarContact">
        <div class="CommandButtonSmall"><a href="#">Phone Me</a></div>
        <div class="CommandButtonSmall"><a href="#">Live meet</a></div>
        <div class="CommandButtonSmall"><a href="#">Email Me</a></div>
        <div class="CommandButtonSmall"><a href="#">IM Me</a></div>
        <div class="CommandButtonSmall"><a href="#">Video Me</a></div>
    </div>
    <div class="CalendarControl">
        <asp:UpdatePanel id="AppointmentUpdatePanel" runat="server">
            <ContentTemplate>
                <uc1:AppointmentsControl ID="AppointmentsControl1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
