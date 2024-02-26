<%@ Page Language="C#" MasterPageFile="~/ContosoBank.Master" AutoEventWireup="True" CodeBehind="Tools.aspx.cs" Inherits="WLQuickApps.ContosoBank.Tools" Title="Australian Small Business Portal - Tools" %>
<%@ Register src="controls/Calculator.ascx" tagname="Calculator" tagprefix="uc1" %>
<%@ Register src="controls/Gadgets.ascx" tagname="Gadgets" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="ToolsWatermark">
        <%--Top Left--%>
        <div class="ToolPanelTop">
           <h2 class="ToolPanelHeader">Business Software</h2>
           <div class="BusinessSoftware">
                <asp:HyperLink ID="OfficeLiveWorkspace" ImageUrl="~/images/AdOfficeLive.gif" runat="server" /><br /><br />
                <asp:Image ID="OfficeLiveSmallBusiness" runat="server" ImageUrl="~/images/AdOfficeSmallBus.gif" />
           </div> 
        </div>
        
        <%--Top Right--%>
        <div class="ToolPanelTop">
            <h2 class="ToolPanelHeader">Business Search</h2>
            <div class="BusinessSearch"></div>
            <div class="BusinesSearchItem">
                <div style="float:left">
                    <asp:TextBox ID="BusinessSearchTextBox" runat="server" Width="215px"></asp:TextBox>
                </div>
                <div class="ToolSearchBox">
                    <div class="CommandButtonMedium">
                        <asp:LinkButton ID="SearchButton" runat="server" Text="Business Search" OnClick="SearchButton_Click"></asp:LinkButton>
                    </div>
                </div>
             </div>
        </div>
        
        <%--Middle Left--%>
        <div class="ToolPanelMiddle">
            <h2 class="ToolPanelHeader">Live Data</h2>
            <div class="BusinessData">
                <img id="BusinessData" runat="server" src="~/Images/BusinessData.png" width="275" height="275" usemap="#BusinessDataMap" alt="BusinessData" />
                <map id="BusinessDataMap" name="BusinessDataMap">
                    <area onmouseover="$get('<%= BusinessData.ClientID %>').src='/Images/BusinessData1.png'" onmouseout="$get('<%= BusinessData.ClientID %>').src='/Images/BusinessData.png'" shape="rect" coords="0,0,140,140" href="#" target="_blank" alt="Run a Credit Check" />
                    <area onmouseover="$get('<%= BusinessData.ClientID %>').src='/Images/BusinessData2.png'" onmouseout="$get('<%= BusinessData.ClientID %>').src='/Images/BusinessData.png'" shape="rect" coords="140,0,275,140" href="#" target="_blank" alt="Land Title Search" />
                    <area onmouseover="$get('<%= BusinessData.ClientID %>').src='/Images/BusinessData3.png'" onmouseout="$get('<%= BusinessData.ClientID %>').src='/Images/BusinessData.png'" shape="rect" coords="0,140,140,275" href="#" target="_blank" alt="House Price Data" />
                    <area onmouseover="$get('<%= BusinessData.ClientID %>').src='/Images/BusinessData4.png'" onmouseout="$get('<%= BusinessData.ClientID %>').src='/Images/BusinessData.png'" shape="rect" coords="140,140,275,275" href="#" target="_blank" alt="Census Data" />
                </map>                 
            </div>
       </div>
        
        <%--Middle Right --%>
        <div class="ToolPanelMiddle">
           <h2 class="ToolPanelHeader">Map Search</h2>
            <div class="MapSearch"></div>
            <div class="MapSearchItem">
                <div style="float:left">
                    <asp:TextBox ID="MapSearchTextBox" runat="server" Width="215px"></asp:TextBox>
                </div>
                <div class="ToolSearchBox">
                    <div class="CommandButtonMedium">
                        <a href="#" onclick='javascript:if($get("<%=MapSearchTextBox.ClientID %>").value) window.open("http://maps.live.com/default.aspx?where1=" + $get("<%=MapSearchTextBox.ClientID %>").value + "&v=2", "", "width=800px, height=600px, resizable")'>Map Search</a>
                    </div>
                </div>
           </div>
        </div>
        
        <%--Bottom Left --%>
        <div class="ToolPanelBottom">
            <h2 class="ToolPanelHeader">Calculators</h2>
            <uc1:Calculator ID="Calculator1" runat="server" />
        </div>
        <%--Bottom Right --%>
        <div class="ToolPanelBottom">
            <h2 class="ToolPanelHeader">Gadgets</h2>
            <uc2:Gadgets ID="Gadgets1" runat="server" />
        </div>
    
    </div>

</asp:Content>
