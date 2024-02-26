<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchEvents.aspx.cs" Inherits="Event_SearchEvents" Title="Untitled Page" %>
<%@ Register Src="../Controls/LocationControl.ascx" TagName="LocationControl" TagPrefix="uc1" %>
<%@ Register Src="../Controls/UserCalendar.ascx" TagName="UserCalendar" TagPrefix="uc4" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel SkinID="Titled" runat="server" >
        <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
            Search by Criteria
        </cc:DropShadowPanel>
        <div class="subform-label">
            Tag
        </div>
        <div class="subform-field">
            <cc:SecureTextBox ID="_tagTextbox" runat="server" Columns="10" />
        </div>
        <div class="subform-label">
            <asp:Label ID="_startDateTimeLabel" runat="server" AssociatedControlID="_startDate">Start time</asp:Label>
        </div>
        <div class="subform-field">
            <cc:SecureTextBox ID="_startDate" runat="server" Columns="10" />
            <asp:CompareValidator ID="_startDateValid" runat="server" ControlToValidate="_startDate" Type="Date" Operator="DataTypeCheck"
                ErrorMessage="Enter a valid start date." ToolTip="Enter a valid start date." Text="*" Display="Dynamic" />at&nbsp;
            <asp:DropDownList runat="server" ID="_startHour">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
            </asp:DropDownList>
            :
            <asp:DropDownList runat="server" ID="_startMinute">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>00</asp:ListItem>
                <asp:ListItem>05</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>35</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>45</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>55</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            <asp:DropDownList runat="server" ID="_startAmPm">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>AM</asp:ListItem>
                <asp:ListItem>PM</asp:ListItem>
            </asp:DropDownList>
                                    
            <ajaxToolkit:CalendarExtender runat="server" ID="_startCalendar" TargetControlID="_startDate" Format="MM/dd/yyyy" CssClass="CalendarPopup" />
            <ajaxToolkit:MaskedEditExtender runat="server" ID="_startDateMask" TargetControlID="_startDate" MaskType="Date" Mask="99/99/9999" />
        </div>
        
        <div class="subform-label">
            <asp:Label ID="_endDateTimeLabel" runat="server" AssociatedControlID="_endDate">End time</asp:Label>
        </div>
        <div class="subform-field">
            <cc:SecureTextBox ID="_endDate" runat="server" Columns="10" />
            <asp:CompareValidator ID="_endDateValid" runat="server" ControlToValidate="_endDate" Type="Date" Operator="DataTypeCheck"
                ErrorMessage="Enter a valid end date." ToolTip="Enter a valid end date." Text="*" Display="Dynamic" />at&nbsp;
            <asp:DropDownList runat="server" ID="_endHour">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
            </asp:DropDownList>
            :
            <asp:DropDownList runat="server" ID="_endMinute">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>00</asp:ListItem>
                <asp:ListItem>05</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>35</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>45</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>55</asp:ListItem>
            </asp:DropDownList>
            &nbsp;
            <asp:DropDownList runat="server" ID="_endAmPm">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>AM</asp:ListItem>
                <asp:ListItem>PM</asp:ListItem>
            </asp:DropDownList>
            <asp:CustomValidator runat="server" ID="_endTimeValid" OnServerValidate="_endTimeFuture_ServerValidate"
                ErrorMessage="The search range must end after it starts." ToolTip="The search range must end after it starts." Text="*" />                                    
            <ajaxToolkit:CalendarExtender runat="server" ID="_endDateExtender" TargetControlID="_endDate" Format="MM/dd/yyyy" CssClass="CalendarPopup" />
            <ajaxToolkit:MaskedEditExtender runat="server" ID="_endDateMask" TargetControlID="_endDate" MaskType="Date" Mask="99/99/9999" />
        </div>
        <div class="subform-label">
            Location
        </div>
        <div class="subform-field">
            <uc1:LocationControl runat="server" ID="_locationControl" ShowLocationCaption="False" />
        </div>

        <div class="subform-field">
            <br />
            <asp:Button ID="_searchButton" runat="server" OnClick="_searchButton_Click" Text="Search Events" />
        </div>
    </cc:DropShadowPanel>
    <br />
    <cc:DropShadowPanel SkinID="Titled" ID="_searchResultsPanel" runat="server" Visible="false">
        <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
            Search Results
        </cc:DropShadowPanel>
        
        <uc4:UserCalendar ID="_userCalendar" runat="server" DisplayMode="SimpleList" />
    </cc:DropShadowPanel>
    <br />
    <asp:Label ID="_resultsLabel" runat="server" />
</asp:Content>

