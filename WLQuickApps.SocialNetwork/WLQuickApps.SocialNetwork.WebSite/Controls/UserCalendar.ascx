<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserCalendar.ascx.cs" Inherits="UserCalendar" %>

<%@ Register Src="UserCalendarSection.ascx" TagName="UserCalendarSection" TagPrefix="uc" %>
<%@ Register Src="LocationLinkControl.ascx" TagName="LocationLinkControl" TagPrefix="uc2" %>

<asp:MultiView runat="server" ID="_calendarView">
    <asp:View runat="server" ID="_fullView">
        <uc:UserCalendarSection ID="_eventsTodaySection" runat="server" />
        <uc:UserCalendarSection ID="_eventsTomorrowSection" runat="server" />
        <uc:UserCalendarSection ID="_eventsLaterThisWeekSection" HeaderText="Later This Week" runat="server" />
        <uc:UserCalendarSection ID="_eventsNextWeekSection" HeaderText="Next Week" runat="server" />
        <uc:UserCalendarSection ID="_eventsLaterSection" HeaderText="Weeks From Now" runat="server" />
    </asp:View>
    <asp:View runat="server" ID="_simpleListView">
        <asp:UpdatePanel ID="_updatePanel" runat="server">
            <ContentTemplate>
                <asp:GridView ID="_eventGrid" runat="server" AutoGenerateColumns="False" SkinID="UserCalendar-SimpleView">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <cc:NullablePicture runat="server" ID="_eventImageLink" NavigateUrl='<%# WebUtilities.GetViewItemUrl(((BaseItem)Container.DataItem)) %>'
                                    Item='<%# (BaseItem)Container.DataItem %>' Text='<%# Bind("Title") %>' MaxWidth="50" MaxHeight="70" SkinID="UserCalendar-EventImage" />

                                <asp:Panel runat="server" ID="_eventDetailsPanel" SkinID="UserCalendar-DetailsPanel">
                                    <asp:HyperLink runat="server" ID="_eventTitleLink" NavigateUrl='<%# WebUtilities.GetViewItemUrl(((BaseItem)Container.DataItem)) %>'
                                                Text='<%# Bind("Title") %>' SkinID="UserCalendar-Title" />
                                    <strong><asp:Label ID="_eventType" runat="server" Text='<%# string.Format("({0})", ((BaseItem)Container.DataItem).SubType) %>' 
                                                    Visible='<%# (((BaseItem)Container.DataItem).SubType.Length > 0) %>' /></strong>
                                                    <br />
                                    <cc:DropShadowPanel runat="server" SkinID="UserCalendar-Description">
                                        <asp:Label ID="_startTime" runat="server" Text='<%# Eval("StartDateTime", "{0:D}") %>' /><br />
                                        <uc2:LocationLinkControl runat="server" ID="_locationLinkControl" LocationItem='<%# ((BaseItem)Container.DataItem).Location %>' />
                                    </cc:DropShadowPanel>
                                </asp:Panel>
                                <div class="clearFloats"></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        There are no upcoming events.
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:ObjectDataSource runat="server" ID="_pagedEventsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.EventManager"
                    SelectMethod="GetFutureEventsForUser" SelectCountMethod="GetFutureEventsForUserCount" StartRowIndexParameterName="startRowIndex"
                    MaximumRowsParameterName="maximumRows" EnablePaging="true" OnSelecting="_pagedEventsDataSource_Selecting">
                    <SelectParameters>
                        <asp:Parameter Name="userName" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:View>
</asp:MultiView>