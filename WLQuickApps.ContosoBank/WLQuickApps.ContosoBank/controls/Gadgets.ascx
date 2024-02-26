<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="Gadgets.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.Gadgets" %>
<asp:Repeater ID="GadgetsRepeater" runat="server">
    <ItemTemplate>
        <div class="GadgetItem">
            <div class="GadgetTitleItem">
                <asp:Label ID="GadgetName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"GadgetName") %>'></asp:Label>
            </div>
            <div class="ToolsItemPanel">
                <div class="GadgetDetailsItem">
                    <asp:Label ID="Description" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>'></asp:Label>
                </div>
            </div>
            <div class="GadgetImageItem">
                <asp:HyperLink ID="GadgetLink" runat="server" NavigateUrl='<%#DataBinder.Eval(Container.DataItem,"Link") %>' ImageUrl='<%#DataBinder.Eval(Container.DataItem,"Thumbnail") %>' Target="_blank"></asp:HyperLink>
            </div>
        </div>
    </ItemTemplate>
    <AlternatingItemTemplate>
       <div class="GadgetItemAlternate">
            <div class="GadgetTitleItem">
                <asp:Label ID="GadgetName" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"GadgetName") %>'></asp:Label>
            </div>
            <div class="ToolsItemPanel">
                <div class="GadgetDetailsItem">
                    <asp:Label ID="Description" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"Description") %>'></asp:Label>
                </div>
            </div>
            <div class="GadgetImageItem">
                <asp:HyperLink ID="GadgetLink" runat="server" NavigateUrl='<%#DataBinder.Eval(Container.DataItem,"Link") %>' ImageUrl='<%#DataBinder.Eval(Container.DataItem,"Thumbnail") %>' Target="_blank"></asp:HyperLink>
            </div>
        </div>
    </AlternatingItemTemplate>
</asp:Repeater>