<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Calculator.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.Calculator" %>

<asp:Repeater ID="CalculatorsRepeater" runat="server">
    <ItemTemplate>
        <div style="position:relative;">
            <div class="CalculatorCommandButton">
                <div class="CommandButton">
                    <asp:HyperLink ID="QuickLinkButton" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem,"CalculatorName") %>' NavigateUrl='<%#DataBinder.Eval(Container.DataItem, "DownloadLink") %>' Target="_blank"></asp:HyperLink>
                </div>
            </div>
            <div class="ToolsItemPanel">
                <div class="CalculatorDetailsItem">
                    <asp:Label ID="CalculatorDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>'></asp:Label>
                </div>
            </div>
            <div class="CalculatorIcon"></div>
        </div>
    </ItemTemplate>
    <AlternatingItemTemplate>
       <div style="position:relative">
            <div class="CalculatorCommandButton">
                <div class="CommandButton">
                    <asp:HyperLink ID="QuickLinkButton" runat="server"  Text='<%#DataBinder.Eval(Container.DataItem,"CalculatorName") %>' NavigateUrl='<%#DataBinder.Eval(Container.DataItem, "DownloadLink") %>' Target="_blank"></asp:HyperLink>
                </div>
            </div>
            <div class="ToolsItemPanel">
                <div class="CalculatorDetailsItem">
                    <asp:Label ID="CalculatorDescription" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>'></asp:Label>
                </div>
            </div>
            <div class="CalculatorIcon CalculatorIconAlternate"></div>
        </div>
    </AlternatingItemTemplate>
</asp:Repeater>