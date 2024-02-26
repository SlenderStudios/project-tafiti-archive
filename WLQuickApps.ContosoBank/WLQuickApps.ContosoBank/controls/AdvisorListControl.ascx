<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="AdvisorListControl.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.AdvisorListControl" %>
<%@ Register assembly="Microsoft.Live.ServerControls" namespace="Microsoft.Live.ServerControls" tagprefix="live" %>
<asp:GridView ID="AdvisorGridView" runat="server" AutoGenerateColumns="False" ShowHeader="false" 
    onrowdatabound="AdvisorGridView_RowDataBound" >
    <Columns>
        <asp:TemplateField >
            <ItemTemplate>
                <asp:HyperLink ID="advisorHyperLink" runat="server" Visible="false"><asp:Image ID="advisorImage" runat="server" style="border-style: none;" width="16" height="16" /></asp:HyperLink>
                <live:MessengerChat ID="MessengerChat2" runat="server" View="Icon" PrivacyStatementURL="~/privacyPolicy.htm">
                    <ColorTheme Name="Green"></ColorTheme>
                </live:MessengerChat>  <asp:Label ID="Label1" runat="server" class="Advisor" Text='<%# Bind("AdvisorName") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                 <live:IDLoginView ID="IDLoginViewAdvisor" runat="server">
                    <LoggedInAllTemplate>
                       <asp:Label ID="appointmentLabel" runat="server" class="Advisor" Text="|" visible='<%# Bind("AvailableAppointment") %>'></asp:Label> <asp:HyperLink ID="appointmentHyperlink" runat="server" class="Advisor" Visible='<%# Bind("AvailableAppointment") %>' NavigateUrl='<%#Eval("ID", "../Appointments.aspx?id={0}")%>'>make appt</asp:HyperLink>
                    </LoggedInAllTemplate>
                </live:IDLoginView>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>








