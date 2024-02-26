<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssociateEvent.aspx.cs" Inherits="Event_AssociateEvent" Title="Untitled Page" %>
<%@ Register Src="../Controls/LocationLinkControl.ascx" TagName="LocationLinkControl" TagPrefix="uc" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    Select an event to associate to <asp:Label runat="server" ID="_groupName" />.
    <asp:UpdatePanel runat="server" ID="_updatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <cc:DropShadowPanel runat="server" SkinID="ImageGallery" ID="_galleryPanel">
                <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
                    Event:&nbsp;<asp:DropDownList ID="_eventList" runat="server" AutoPostBack="True" DataSourceID="_dataSource"
                        DataTextField="Title" DataValueField="BaseItemID" />
                    <asp:ObjectDataSource ID="_dataSource" runat="server" SelectMethod="GetEventsForUser"
                        TypeName="WLQuickApps.SocialNetwork.Business.EventManager">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="userName" QueryStringField="userName" Type="String" />
                            <asp:Parameter Name="status" Type="Object" DefaultValue="Joined" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </cc:DropShadowPanel>
                <br />
                <asp:FormView runat="server" ID="_eventDetails" DataSourceID="_eventDetailsDataSource" DefaultMode="ReadOnly">
                    <ItemTemplate>
                        <div class="subform-field">
                            <strong>at</strong>
                            <uc:LocationLinkControl runat="server" LocationItem='<%# ((Event)Container.DataItem).Location %>' />,
                            <strong>runs from</strong>
                            <asp:Label runat="server" Text='<%# String.Format("{0:D} at {0:t}", ((Event)Container.DataItem).StartDateTime) %>' />
                            <strong>to</strong>
                            <asp:Label runat="server" Text='<%# String.Format("{0:D} at {0:t}", ((Event)Container.DataItem).EndDateTime) %>' />
                        </div><br />
                    </ItemTemplate>
                </asp:FormView>
                <asp:ObjectDataSource ID="_eventDetailsDataSource" runat="server" SelectMethod="GetEvent"
                    TypeName="WLQuickApps.SocialNetwork.Business.EventManager" OnSelecting="_eventDetailsDataSource_Selecting">
                    <SelectParameters>
                        <asp:Parameter Type="Int32" Name="baseItemID" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <div style="float:right">
                    <asp:Label runat="server" ID="_alreadyAssociatedErrorLabel" Text="<strong>This event has already been associated.</strong><br />" Visible="false" />
                    <asp:Button runat="server" ID="_createNewButton" OnClick="_createNewButton_Click" Text="Create New Event" />
                    <asp:Button runat="server" ID="_associateButton" OnClick="_associateButton_Click" Text="Associate this Event" />
                </div>
                <div class="clearFloats"></div>
            </cc:DropShadowPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>