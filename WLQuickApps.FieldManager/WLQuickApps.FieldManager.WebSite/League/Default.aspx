<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="WLQuickApps.FieldManager.WebSite.League_Default" MasterPageFile="~/AdminPages.master" %>

<asp:Content ContentPlaceHolderID="_mainContent" runat="server">
<script type="text/javascript">
    function addToMyLeagues(leagueID)
    {
        WLQuickApps.FieldManager.WebSite.SiteService.AddToMyLeagues(
            leagueID, 
            function()
            {
                document.location = "ViewLeague.aspx?leagueID=" + leagueID;
            });
    }
</script>
    <div class="pageContent">
        <div class="header">Leagues</div>
        <div class="LeaguesTableDiv">
            <asp:GridView ID="_leagueGridView" runat="server" AutoGenerateColumns="False" 
                DataSourceID="_leaguesDataSource" PageSize="20" AllowPaging="True"
                PagerStyle-HorizontalAlign="Center" PagerSettings-PageButtonCount="10" PagerStyle-CssClass="PagerStyle"
                CssClass="LeaguesTable" AlternatingRowStyle-CssClass="LeagueAltRow" HeaderStyle-CssClass="LeagueHeader" GridLines="Horizontal">
                <Columns>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="_nameLink" runat="server" NavigateUrl='<%# string.Format("~/League/ViewLeague.aspx?leagueID={0}", Eval("LeagueID")) %>'><%# Eval("Title") %></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Description" HeaderText="Description" 
                        SortExpression="Description" />
                    <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                    <asp:TemplateField HeaderText="Add">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# string.Format("javascript:addToMyLeagues({0})", Eval("LeagueID")) %>' Visible='<%# UserManager.UserIsLoggedIn() && !LeagueManager.IsLeagueUser(Convert.ToInt32(Eval("LeagueID"))) %>'>Add</asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:ObjectDataSource ID="_leaguesDataSource" runat="server" 
            SelectMethod="GetAllLeagues" 
            TypeName="WLQuickApps.FieldManager.Business.LeagueManager" 
            EnablePaging="True" 
            SelectCountMethod="GetAllLeaguesCount"
            StartRowIndexParameterName="startRowIndex"
            MaximumRowsParameterName="maximumRows"
            >
        </asp:ObjectDataSource>
    </div>

</asp:Content>