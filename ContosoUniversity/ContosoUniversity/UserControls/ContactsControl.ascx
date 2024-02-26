<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactsControl.ascx.cs" Inherits="ContosoUniversity.UserControls.ContactsControl" %>
<asp:XmlDataSource ID="XmlDataSource1" XPath="/LiveContacts/Contacts/Contact" runat="server" EnableCaching="false"></asp:XmlDataSource>
<asp:GridView ID="GridView1" OnRowDataBound="GridView1_RowDataBound" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="6" BorderStyle="None" GridLines="None" ShowHeader="False" CellPadding="0" CellSpacing="2" CssClass="ContractsGridControl">
    <EmptyDataTemplate>
        <asp:Label ID="Label2" runat="server">There are no contacts </asp:Label>
    </EmptyDataTemplate>
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/images/im_icon.gif" />
            </ItemTemplate>
            <ItemStyle CssClass="ContactCell" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# XPath("Profiles/Personal/FirstName") %>
            </ItemTemplate>
            <ItemStyle CssClass="NameCell" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <a href="msnim:chat?contact=<%# XPath("Emails/Email/Address") %>"><img alt="Chat with <%# XPath("Emails/Email/Address") %>" border=no src="App_Themes/Default/images/message_icon.gif" /></a>
            </ItemTemplate>
            <ItemStyle CssClass="ContactCell" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <a href="mailto:<%# XPath("Emails/Email/Address") %>"><img alt="Send an email to <%# XPath("Emails/Email/Address") %>" border=no src="App_Themes/Default/images/mail_icon.gif" /></a>
            </ItemTemplate>
            <ItemStyle CssClass="ContactCell" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
            </ItemTemplate>
            <ItemStyle CssClass="ContactCell" />
        </asp:TemplateField>
    </Columns>
    <PagerSettings Mode="NumericFirstLast" />
    <PagerStyle CssClass="Pager" />
</asp:GridView>
<asp:HyperLink ID="SeeAllLink" CssClass="SeeAllLink" runat="server" NavigateUrl="javascript:PlotAllLocations()">plot all contacts</asp:HyperLink>
<asp:Label Visible="false" ID="Label1" runat="server">DEBUG: OwnerHandle=<%= OwnerHandle %></asp:Label>