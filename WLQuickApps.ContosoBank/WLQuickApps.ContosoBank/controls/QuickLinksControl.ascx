<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="QuickLinksControl.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.QuickLinksControl" %>
<asp:GridView ID="QuickLinksGridView" runat="server" runat="server" AutoGenerateColumns="False" ShowHeader="false" >
    <Columns>
        <asp:TemplateField >
            <ItemTemplate>
                    <div class="CommandButton"><asp:HyperLink ID="QuickLinkButton" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem,"Title") %>' NavigateUrl='<%#DataBinder.Eval(Container.DataItem, "Link") %>' Target="_blank"></asp:HyperLink></div>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
