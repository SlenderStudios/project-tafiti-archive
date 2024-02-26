<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ArticlesResources.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.ArticlesResources" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
 <cc1:Accordion ID="MyAccordion" CssClass="ResourcesAccordion"  runat="server" SelectedIndex="0"
    HeaderCssClass="ResourcesAccordionHeader" HeaderSelectedCssClass="ResourcesAccordionHeaderSelected" ContentCssClass="ResourcesContent" FadeTransitions="false"
    AutoSize="Fill" Height="258">
    <Panes>              
        <cc1:AccordionPane ID="AccordionPaneArticles" runat="server">
            <Header>Articles</Header>
            <Content>
                <asp:Repeater ID="ArticlesRepeater" runat="server">
                    <ItemTemplate>
                        <div class="ResourcesColumns">
                            <div class="ResourcesColumn1"><%#DataBinder.Eval(Container.DataItem,"Title") %></div>
                            <div class="ResourcesColumn2"> <%#DataBinder.Eval(Container.DataItem, "Area")%> </div>
                            <div class="ResourcesColumn3"><a href='<%#DataBinder.Eval(Container.DataItem,"Link") %>' target="_blank"">read more</a></div>
                        </div>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <div class="ResourcesColumnsAlternate"><div class="ResourcesColumn1"><%#DataBinder.Eval(Container.DataItem,"Title") %></div><div class="ResourcesColumn2"> <%#DataBinder.Eval(Container.DataItem, "Area")%> </div><div class="ResourcesColumn3"><a href='<%#DataBinder.Eval(Container.DataItem,"Link") %>' target="_blank"">read more</a></div></div>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </Content>
        </cc1:AccordionPane>
        <cc1:AccordionPane ID="AccordionPaneResources" runat="server">
            <Header>Resources</Header>
            <Content>
                <asp:Repeater ID="ResourcesRepeater" runat="server">
                    <ItemTemplate>
                        <div class="ResourcesColumns"><div class="ResourcesColumn1"><%#DataBinder.Eval(Container.DataItem,"Title") %></div><div class="ResourcesColumn2"> <%#DataBinder.Eval(Container.DataItem, "Area")%> </div><div class="ResourcesColumn3"><a href='<%#DataBinder.Eval(Container.DataItem,"Link") %>' target="_blank"">read more</a></div></div>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <div class="ResourcesColumnsAlternate"><div class="ResourcesColumn1"><%#DataBinder.Eval(Container.DataItem,"Title") %></div><div class="ResourcesColumn2"> <%#DataBinder.Eval(Container.DataItem, "Area")%> </div><div class="ResourcesColumn3"><a href='<%#DataBinder.Eval(Container.DataItem,"Link") %>' target="_blank"">read more</a></div></div>
                    </AlternatingItemTemplate>                    
                </asp:Repeater>                
            </Content>
        </cc1:AccordionPane>
    </Panes>
</cc1:Accordion>

