<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Search.aspx.cs"
    Inherits="Search" Title="Search Network" %>
<%@ Register Src="Controls/LocationControl.ascx" TagName="LocationControl" TagPrefix="uc5" %>
<%@ Register Src="Controls/LocationLinkControl.ascx" TagName="LocationLinkControl" TagPrefix="uc5" %>
<%@ Register Src="Controls/UserCalendar.ascx" TagName="UserCalendar" TagPrefix="uc4" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <br />
        <cc:DropShadowPanel ID="_searchPanel" runat="server">
            <cc:DropShadowPanel ID="DropShadowPanel2" runat="server" SkinID="ImageGallery-title" >
                Search
            </cc:DropShadowPanel>
        <div class="subform-label">
            Search for
        </div>
        <div class="subform-field">
            <cc:SecureTextBox ID="_tagTextBox" runat="server" />
        </div>
        <div class="subform-label">
            Location
        </div>
        <div class="subform-field">
            <uc5:LocationControl runat="server" ID="_locationControl" ShowLocationCaption="False" />
        </div>
        <div class="subform-field">
            <br />
            <asp:Button ID="_searchButton" runat="server" OnClick="_searchButton_Click" Text="Search" />
        </div>
    </cc:DropShadowPanel>
    
    <cc:DropShadowPanel ID="_noResultsPanel" runat="server" Visible="False">
        Your search didn't return any results. You may want to:
        <asp:BulletedList ID="_searchTips" runat="server">
            <asp:ListItem>Make sure that all words are spelled correctly.</asp:ListItem>
            <asp:ListItem>Try searching for different keywords.</asp:ListItem>
            <asp:ListItem>Try a more general query.</asp:ListItem>
        </asp:BulletedList>
    </cc:DropShadowPanel>
    
    <asp:UpdatePanel ID="_updatePanel" runat="server">
        <ContentTemplate>
            <cc:DropShadowPanel runat="server" ID="_searchResultsPanel" >
                <ajaxToolkit:TabContainer ID="_searchResultsTabs" runat="server" ActiveTabIndex="0">
                    <ajaxToolkit:TabPanel ID="_pictureTabPanel" runat="server" HeaderText="Pictures">
                        <ContentTemplate>
                            <cc:MetaGallery runat="server" ID="_picturesGallery" ViewMode="Media" />
                            <br />
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="_videoTabPanel" runat="server" HeaderText="Video">
                        <ContentTemplate>
                            <cc:MetaGallery runat="server" ID="_videoGallery" ViewMode="Media" />
                            <br />
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="_audioTabPanel" runat="server" HeaderText="Audio">
                        <ContentTemplate>
                            <cc:MetaGallery runat="server" ID="_audioGallery" ViewMode="Media" />
                            <br />
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="_usersTabPanel" runat="server" HeaderText="Users">
                        <ContentTemplate>
                            <br />
                            <cc:MetaGallery runat="server" ID="_usersGallery" ViewMode="User"/>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="_groupsTabPanel" runat="server" HeaderText="Groups">
                        <ContentTemplate>
                            <br />
                            <cc:MetaGallery runat="server" ID="_groupsGallery" ViewMode="Group" />
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="_eventsTabPanel" runat="server" HeaderText="Events">
                        <ContentTemplate>
                            <br />
                            <uc4:UserCalendar ID="_eventsCalendar" runat="server" DisplayMode="SimpleList" />
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="_collectionsTabPanel" runat="server" HeaderText="Maps">
                        <ContentTemplate>
                            <br />
                            <cc:MetaGallery runat="server" ID="_collectionsGallery" ViewMode="List" />
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
            </cc:DropShadowPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
