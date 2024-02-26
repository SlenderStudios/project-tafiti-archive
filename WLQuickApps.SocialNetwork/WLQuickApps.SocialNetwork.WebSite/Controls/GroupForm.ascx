<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GroupForm.ascx.cs" Inherits="AddGroupForm" %>
<%@ Register Src="LocationControl.ascx" TagName="LocationControl" TagPrefix="uc1" %>

<div class="form-field form-errorRow">
    <asp:ValidationSummary runat="server" ID="_errorSummary" />
</div>

<div class="form-label form-required">
    <asp:Label ID="_nameLabel" runat="server" AssociatedControlID="_name">Name</asp:Label>
</div>
<div class="form-field">
    <cc:SecureTextBox ID="_name" runat="server" />
    <asp:RequiredFieldValidator ID="_nameRequired" runat="server" ControlToValidate="_name"
        ErrorMessage="Enter a name." ToolTip="Enter a name." Text="*" Display="Dynamic" />
</div>

<div class="form-label">
    <asp:Label ID="_descriptionLabel" runat="server" AssociatedControlID="_description">Description</asp:Label>
</div>
<div class="form-field">
    <cc:SecureTextBox runat="server" ID="_description" TextMode="MultiLine" Columns="30" Rows="5" />
</div>

<asp:Panel ID="_typePanel" runat="server" Visible="false">
    <div class="form-label">
        <asp:Label ID="Label1" runat="server" AssociatedControlID="_description">Type</asp:Label>
    </div>
    <div class="form-field">
        <asp:DropDownList ID="_typeDropDownList" runat="server" />
    </div>
</asp:Panel>

<asp:Panel runat="server" ID="_dateTimes">
    <div class="form-label form-required">
        <asp:Label ID="_startDateTimeLabel" runat="server" AssociatedControlID="_startDate">Start time</asp:Label>
    </div>
    <div class="form-field">
        <cc:SecureTextBox ID="_startDate" runat="server" Columns="10" />
        <asp:RequiredFieldValidator ID="_startDateRequired" runat="server" ControlToValidate="_startDate"
            ErrorMessage="Enter a start date." ToolTip="Enter a start date." Text="*" Display="Dynamic" />
        <asp:CompareValidator ID="_startDateValid" runat="server" ControlToValidate="_startDate" Type="Date" Operator="DataTypeCheck"
            ErrorMessage="Enter a valid start date." ToolTip="Enter a valid start date." Text="*" Display="Dynamic" />at&nbsp;
        <asp:DropDownList runat="server" ID="_startHour">
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
            <asp:ListItem Selected="True">12</asp:ListItem>
        </asp:DropDownList>
        :
        <asp:DropDownList runat="server" ID="_startMinute">
            <asp:ListItem Selected="True">00</asp:ListItem>
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
            <asp:ListItem>AM</asp:ListItem>
            <asp:ListItem Selected="True">PM</asp:ListItem>
        </asp:DropDownList>
        <asp:CustomValidator runat="server" ID="_startTimeRequired" OnServerValidate="_startTimeRequired_ServerValidate"
            ErrorMessage="Select a start time." ToolTip="Select a start time." Text="*" Display="Dynamic" />
        <asp:CustomValidator runat="server" ID="_startTimeFuture" OnServerValidate="_startTimeFuture_ServerValidate"
            ErrorMessage="The event cannot start in the past." ToolTip="The event cannot start in the past." Text="*" />
            
        
        <ajaxToolkit:CalendarExtender runat="server" ID="_startCalendar" TargetControlID="_startDate" Format="MM/dd/yyyy" CssClass="CalendarPopup" />
        <ajaxToolkit:MaskedEditExtender runat="server" ID="_startDateMask" TargetControlID="_startDate" MaskType="Date" Mask="99/99/9999" />
    </div>
    
    <div class="form-label form-required">
        <asp:Label ID="_endDateTimeLabel" runat="server" AssociatedControlID="_endDate">End time</asp:Label>
    </div>
    <div class="form-field">
        <cc:SecureTextBox ID="_endDate" runat="server" Columns="10" />
        <asp:RequiredFieldValidator ID="_endDateRequired" runat="server" ControlToValidate="_endDate"
            ErrorMessage="Enter an end date." ToolTip="Enter an end date." Text="*" Display="Dynamic" />
        <asp:CompareValidator ID="_endDateValid" runat="server" ControlToValidate="_endDate" Type="Date" Operator="DataTypeCheck"
            ErrorMessage="Enter a valid end date." ToolTip="Enter a valid end date." Text="*" Display="Dynamic" />at&nbsp;
        <asp:DropDownList runat="server" ID="_endHour">
            <asp:ListItem Selected="True">1</asp:ListItem>
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
            <asp:ListItem Selected="True">00</asp:ListItem>
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
            <asp:ListItem>AM</asp:ListItem>
            <asp:ListItem Selected="True">PM</asp:ListItem>
        </asp:DropDownList>
        <asp:CustomValidator runat="server" ID="_endTimeRequired" OnServerValidate="_endTimeRequired_ServerValidate"
            ErrorMessage="Select an end time." ToolTip="Select an end time." Text="*" Display="Dynamic" />
        <asp:CustomValidator runat="server" ID="_endTimeValid" OnServerValidate="_endTimeFuture_ServerValidate"
            ErrorMessage="The event must end after it starts." ToolTip="The event must end after it starts." Text="*" />
            
        
        <ajaxToolkit:CalendarExtender runat="server" ID="_endDateExtender" TargetControlID="_endDate" Format="MM/dd/yyyy" CssClass="CalendarPopup" />
        <ajaxToolkit:MaskedEditExtender runat="server" ID="_endDateMask" TargetControlID="_endDate" MaskType="Date" Mask="99/99/9999" />
    </div>
</asp:Panel>

<div class="form-label form-required">
    Privacy Level
    [<asp:LinkButton runat="server" ID="_showPrivacyHelpLink" Text="?" />]
</div>
<div class="form-field">
    <asp:RadioButtonList runat="server" ID="_privacyLevel" />
</div>

<asp:Panel ID="_privacyPanel" runat="server" SkinID="ModalPopupPanel">
    <asp:Panel runat="server" SkinID="ModalPopup-content">
        <h3>Privacy Levels:</h3>
        <p><strong>Public</strong> - Anyone may see and join the group.</p>
        <p><strong>Private</strong> - Anyone may see the group, but must request an invitation to join.</p>
        <p><strong>Invisible</strong> - One must recieve an invitation to see or join the group.</p>
    </asp:Panel>
    <asp:Panel runat="server" SkinID="ModalPopup-buttons">
        <asp:Button ID="_okPrivacy" runat="server" Text="OK" CausesValidation="False" />
    </asp:Panel>
</asp:Panel>

<asp:Button runat="server" ID="_cancelPopupButton" SkinID="Hidden" />
<ajaxToolkit:ModalPopupExtender runat="server" ID="_privacyPopup" TargetControlID="_showPrivacyHelpLink"
    PopupControlID="_privacyPanel" BackgroundCssClass="modalBackground" OkControlID="_okPrivacy" />

<div class="form-label">
    <asp:Label ID="_locationLabel" runat="server" AssociatedControlID="_location">Location</asp:Label>
</div>
<div class="form-field">
    <uc1:LocationControl runat="server" ID="_location" ShowLocationCaption="False" />
</div>
<div class="form-errorRow">
    <asp:Label runat="server" ID="_invalidPictureError" Text="Select a valid image file to upload." />
</div>

<div class="form-label">
    Thumbnail
</div>
<div class="form-field">
    <asp:FileUpload ID="_pictureFileUpload" runat="server" /><br />
    <strong><asp:Label runat="server" ID="_existingThumbnailLabel" Text="Existing thumbnail:<br />" Visible="false" /></strong>
    <asp:Image runat="server" ID="_existingThumbnail" Visible="false" />
</div>
<div class="form-field">
    <asp:Button runat="server" ID="_submitButton" OnClick="_submitButton_Click" Text="Submit" />
</div>