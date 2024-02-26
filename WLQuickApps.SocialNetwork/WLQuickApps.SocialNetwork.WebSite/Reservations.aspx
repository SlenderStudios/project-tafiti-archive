<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Reservations.aspx.cs" Inherits="Reservations" Title="Make a Reservation" %>
<%@ Register Src="Controls/ReservationForm.ascx" TagName="ReservationForm" TagPrefix="uc1" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" Runat="Server">
    <br />
    <uc1:ReservationForm ID="_reservationForm" runat="server" />
</asp:Content>

