<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Feedback.aspx.cs" Inherits="Feedback" Title="Untitled Page" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <cc:DropShadowPanel runat="server" ID="_feedbackPanel">
        <cc:DropShadowPanel runat="server" ID="_feedbackTitle" SkinID="ImageGallery-Title">
            Feedback Details
        </cc:DropShadowPanel>
        
        <div class="form-label">
            Referring page
        </div>
        <div class="form-field">
            <asp:Label runat="server" ID="_referrerUrl" />
        </div>
        <div class="form-label form-required">
            Feedback type
        </div>
        <div class="form-field">
            <asp:DropDownList runat="server" ID="_feedbackType">
                <asp:ListItem Text="Inappropriate Content" />
                <asp:ListItem Text="Unauthorized Use of Copyrighted Content" />
                <asp:ListItem Text="Spam" />
                <asp:ListItem Text="Bug" />
                <asp:ListItem Text="Feature Request" />
                <asp:ListItem Text="Other" />
            </asp:DropDownList>
        </div>
        <div class="form-label form-required">
            Details
        </div>
        <div class="form-field">
            <cc:SecureTextBox runat="server" TextMode="MultiLine" ID="_feedbackDescription" Rows="6" Columns="55" />
            <ajaxToolkit:TextBoxWatermarkExtender runat="server" ID="_feedbackWatermark" TargetControlID="_feedbackDescription"
                        WatermarkText="Please describe the issue as thoroughly as possible." WatermarkCssClass="feedbackWatermark" />
        </div>
        <div class="form-field">
            <asp:Button runat="server" ID="_submitFeedback" OnClick="_submitFeedback_Click" Text="Send" />
        </div>
    </cc:DropShadowPanel>
    <cc:DropShadowPanel runat="server" Visible="false" ID="_sentPanel">
        <p>
            Your feedback has been sent and will be reviewed shortly.  Thank you.
        </p>
    </cc:DropShadowPanel>
</asp:Content>