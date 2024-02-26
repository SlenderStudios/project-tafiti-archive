<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="FeaturedEventControl.ascx.cs" Inherits="WLQuickApps.ContosoBank.controls.FeaturedEventControl" %>

<asp:Repeater ID="FeaturedEventRepeater" runat="server" 
    onitemdatabound="FeaturedEventRepeater_ItemDataBound">
    <ItemTemplate>
        <div class="FeaturedEventHeader">
            <div class="FeaturedEventHeaderIcon">&nbsp;</div>
            <div class="FeaturedEventHeaderLeft">
                <asp:Label ID="SubjectLabel" runat="server" CssClass="PortalSubjectTitle"></asp:Label>
             </div>
            <div class="FeaturedEventHeaderRight">
                <asp:Label ID="locationLabel" runat="server" CssClass="PortalSubjectTextHighlight"></asp:Label>
                <asp:Label ID="eventDateLabel" class="PortalSubjectTextHighlight" runat="server" ></asp:Label>
            </div>
        </div>
        <div class="PortalContent">
            <table class="FeaturedEvent">
                <tr>
                    <td>Location:</td>
                    <td><%#DataBinder.Eval(Container.DataItem, "Address") %></td>
                </tr>
                <tr >
                    <td>Date and Time:</td>
                    <td><asp:Label ID="EventDateTimeLabel" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td>
                        <%#DataBinder.Eval(Container.DataItem, "EventDescription") %><br />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        
                        <div class="CommandButton"><asp:HyperLink ID="HyperLink1" NavigateUrl="#" Text="Add to Outlook Calendar" runat="server"></asp:HyperLink></div>
                        <div class="ContactLinks"><span class="PortalHighlight2">Contact: </span><span class="PortalSubjectTextHighlight"> <%#DataBinder.Eval(Container.DataItem, "ContactDetails") %></span></div>
                    </td>                
                </tr>
            </table>
 
        </div>
   </ItemTemplate>
</asp:Repeater>
