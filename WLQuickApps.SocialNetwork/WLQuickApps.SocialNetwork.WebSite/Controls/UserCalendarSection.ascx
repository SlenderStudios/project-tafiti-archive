<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserCalendarSection.ascx.cs" Inherits="UserCalendarSection" %>

<%@ Register Src="../Controls/LocationLinkControl.ascx" TagName="LocationLinkControl" TagPrefix="uc2" %>

<h1 style="position:relative;"><asp:Label runat="server" Text='<% #this.HeaderText %>' /></h1>
<asp:GridView ID="_dataList" runat="server" AllowPaging="true" PageSize="5" AutoGenerateColumns="false">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <cc:DropShadowPanel runat="server">
                    <cc:NullablePicture runat="server" ID="_eventPicture" NavigateUrl='<%# WebUtilities.GetViewItemUrl(((Event)Container.DataItem)) %>'
                        Item='<%# ((Event)Container.DataItem) %>' MaxWidth="256" MaxHeight="256" SkinID="UserCalendarSection-EventImage" /><br />
                    <asp:HyperLink ID="_eventLink" runat="server" Text='<%# Eval("Title") %>' 
                        NavigateUrl='<%# WebUtilities.GetViewItemUrl(((BaseItem)Container.DataItem)) %>' SkinID="UserCalendarSection-Title" />
                    <strong><asp:Label ID="_eventType" runat="server" Text='<%# string.Format("({0})", ((Event)Container.DataItem).SubType) %>' Visible='<%# ((Event)Container.DataItem).SubType.Length > 0 %>' /></strong>
                    <br />
                    <cc:DropShadowPanel runat="server" SkinID="UserCalendarSection-Description">
                        <strong>hosted by</strong> <asp:HyperLink ID="_creatorLink" runat="server" Text='<%# ((Event)Container.DataItem).Owner.Title %>'
                                     NavigateUrl='<%# WebUtilities.GetViewItemUrl(((Event)Container.DataItem).Owner) %>' /><br />
                        <strong>at</strong> <uc2:LocationLinkControl runat="server" LocationItem='<%# ((Event)Container.DataItem).Location %>' ShowLocationCaption="True" /><br />
                        <strong>from</strong> <asp:Label ID="_startTime" runat="server" Text='<%# Eval("StartDateTime", "{0:D} at {0:t}") %>' /><br />
                        <strong>until</strong> <asp:Label ID="_endTime" runat="server" Text='<%# Eval("EndDateTime", "{0:D} at {0:t}") %>' />
                    </cc:DropShadowPanel>
                    <div class="clearFloats"></div>
                </cc:DropShadowPanel>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        There are no events in this date range.
    </EmptyDataTemplate>
</asp:GridView>
<br />

<asp:ObjectDataSource runat="server" ID="_pagedEventsDataSource" TypeName="WLQuickApps.SocialNetwork.Business.EventManager"
    SelectMethod="GetEventsForUserByStartDate" SelectCountMethod="GetEventsForUserByStartDateCount" StartRowIndexParameterName="startRowIndex"
    MaximumRowsParameterName="maximumRows" EnablePaging="true" OnSelecting="_pagedEventsDataSource_Selecting">
    <SelectParameters>
        <asp:Parameter Name="userName" Type="String" />
        <asp:Parameter Name="searchRangeStart" Type="DateTime" />
        <asp:Parameter Name="searchRangeEnd" Type="DateTime" />
        <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
    </SelectParameters>
</asp:ObjectDataSource>