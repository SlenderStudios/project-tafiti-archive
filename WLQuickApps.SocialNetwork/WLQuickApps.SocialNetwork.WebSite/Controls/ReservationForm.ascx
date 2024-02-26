<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReservationForm.ascx.cs" Inherits="ReservationForm" %>
<asp:UpdatePanel runat="server" ID="_updatePanel" RenderMode="Inline" UpdateMode="Always">
    <ContentTemplate>
        <cc:DropShadowPanel runat="server">
            <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
                When will <em>your</em> getaway be?
            </cc:DropShadowPanel>
            <div style="float:left;position:relative;top:50px;">
                <div class="form-errorRow">
                    <asp:ValidationSummary runat="server" ID="_errorSummary" />
                </div>
                <div class="form-label">
                    Destination
                </div>
                <div class="form-field">
                    <asp:DropDownList runat="server" ID="_destinationList" AutoPostBack="true" OnSelectedIndexChanged="_destinationList_SelectedIndexChanged">
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
                    <asp:CustomValidator runat="server" ID="_destinationRequired" OnServerValidate="_destinationRequired_ServerValidate"
                        ErrorMessage="Select a destination." ToolTip="Select a destination." Text="*" />
                </div>
                <div class="form-label">
                    Arrival
                </div>
                <div class="form-field">
                    <cc:SecureTextBox ID="_startDate" runat="server" Columns="10" />
                    <asp:RequiredFieldValidator ID="_startDateRequired" runat="server" ControlToValidate="_startDate"
                        ErrorMessage="Enter an arrival date." ToolTip="Enter an arrival date." Text="*" Display="Dynamic" />
                    <asp:CompareValidator ID="_startDateValid" runat="server" ControlToValidate="_startDate" Type="Date" Operator="DataTypeCheck"
                        ErrorMessage="Enter a valid arrival date." ToolTip="Enter a arrival start date." Text="*" Display="Dynamic" />
                    <ajaxToolkit:CalendarExtender runat="server" ID="_startCalendar" TargetControlID="_startDate" Format="MM/dd/yyyy" CssClass="CalendarPopup" />
                    <ajaxToolkit:MaskedEditExtender runat="server" ID="_startDateMask" TargetControlID="_startDate" MaskType="Date" Mask="99/99/9999" />
                </div>
                <div class="form-label">
                    Departure
                </div>
                <div class="form-field">
                    <cc:SecureTextBox ID="_endDate" runat="server" Columns="10" />
                    <asp:RequiredFieldValidator ID="_endDateRequired" runat="server" ControlToValidate="_endDate"
                        ErrorMessage="Enter a departure date." ToolTip="Enter a departure date." Text="*" Display="Dynamic" />
                    <asp:CompareValidator ID="_endDateValid" runat="server" ControlToValidate="_endDate" Type="Date" Operator="DataTypeCheck"
                        ErrorMessage="Enter a valid departure date." ToolTip="Enter a arrival departure date." Text="*" Display="Dynamic" />
                    <ajaxToolkit:CalendarExtender runat="server" ID="_endCalendar" TargetControlID="_endDate" Format="MM/dd/yyyy" CssClass="CalendarPopup" />
                    <ajaxToolkit:MaskedEditExtender runat="server" ID="_endDateMask" TargetControlID="_endDate" MaskType="Date" Mask="99/99/9999" />
                </div>
                <div class="form-label">
                    Number of Guests
                </div>
                <div class="form-field">
                    <cc:SecureTextBox ID="_numberGuests" runat="server" Columns="2" />
                    <asp:RequiredFieldValidator ID="_guestsRequired" runat="server" ControlToValidate="_numberGuests"
                        ErrorMessage="Enter the number of guests." ToolTip="Enter the number of guests." Text="*" Display="Dynamic" />
                    <asp:RangeValidator runat="server" ID="_guestsValid" ControlToValidate="_numberGuests" MinimumValue="1" MaximumValue="20"
                        ErrorMessage="Between 1 and 20 guests are allowed." ToolTip="Between 1 and 20 guests are allowed." Display="Dynamic"
                        Text="*" Type="Integer" />
                </div>
                <div class="form-field">
                    <br />
                    <asp:Button runat="server" Text="Search" UseSubmitBehavior="true" CausesValidation="true" />
                </div>
            </div>
            <cc:DropShadowPanel runat="server" SkinID="ReservationForm-ImageBox">
                <asp:Image runat="server" ID="_sideImage" />
            </cc:DropShadowPanel>
            <div class="clearFloats"></div>
            <br /><br />
        </cc:DropShadowPanel>
    </ContentTemplate>
</asp:UpdatePanel>