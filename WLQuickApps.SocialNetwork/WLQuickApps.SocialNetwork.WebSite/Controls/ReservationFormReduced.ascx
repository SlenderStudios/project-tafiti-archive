<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReservationFormReduced.ascx.cs" Inherits="Controls_ReservationFormReduced" %>
<div style="float:left;position:relative;top:50px;">
    <div class="subform-label" style="width:90px">
        Destination
    </div>
    <div class="subform-field" style="margin-left:110px">
        <asp:DropDownList runat="server" ID="_destinationList">
            <asp:ListItem Text=" " Selected="true" Value="Generic" />
            <asp:ListItem Text="Chicago" Value="Chicago" />
            <asp:ListItem Text="Honolulu" Value="Honolulu" />
            <asp:ListItem Text="Juneau" Value="Juneau" />
            <asp:ListItem Text="Los Angeles" Value="LosAngeles" />
            <asp:ListItem Text="Miami" Value="Miami" />
            <asp:ListItem Text="New Orleans" Value="NewOrleans" />
            <asp:ListItem Text="New York" Value="NewYork" />
            <asp:ListItem Text="Seattle" Value="Seattle" />
        </asp:DropDownList>
    </div>
    <div class="subform-label" style="width:100px">
        Arrival
    </div>
    <div class="subform-field" style="margin-left:120px">
        <cc:SecureTextBox ID="_startDate" runat="server" Columns="10" />
        <asp:Image runat="server" ID="_startDateIcon" SkinID="CalendarIcon" />
        <ajaxToolkit:CalendarExtender runat="server" ID="_startCalendar" TargetControlID="_startDate" Format="MM/dd/yyyy" CssClass="CalendarPopup" />
        <ajaxToolkit:MaskedEditExtender runat="server" ID="_startDateMask" TargetControlID="_startDate" MaskType="Date" Mask="99/99/9999" />
    </div>
    <div class="subform-label" style="width:120px">
        Departure
    </div>
    <div class="subform-field" style="margin-left:140px">
        <cc:SecureTextBox ID="_endDate" runat="server" Columns="10" />
        <asp:Image runat="server" ID="_endDateIcon" SkinID="CalendarIcon" />
        <ajaxToolkit:CalendarExtender runat="server" ID="_endCalendar" TargetControlID="_endDate" Format="MM/dd/yyyy" CssClass="CalendarPopup" />
        <ajaxToolkit:MaskedEditExtender runat="server" ID="_endDateMask" TargetControlID="_endDate" MaskType="Date" Mask="99/99/9999" />
    </div>
    <div class="form-label">
        Number of Guests
    </div>
    <div class="form-field">
        <cc:SecureTextBox ID="_numberGuests" runat="server" Columns="2" />
    </div>
    <div class="form-field" style="margin-left:64px;margin-top:-14px">
        <br />
        <asp:Button runat="server" Text="Book" UseSubmitBehavior="false" />
    </div>
</div>