<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewAlbum.aspx.cs"
    Inherits="Media_ViewAlbum" Title="View Gallery" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <cc:DropShadowPanel runat="server" >
        <asp:FormView ID="_albumFormView" runat="server" DataSourceID="ObjectDataSource4">
            <ItemTemplate>
                <cc:DropShadowPanel runat="server" SkinID="ImageGallery-title">
                    <asp:Label ID="_albumNameLabel" runat="server" Text='<%# WebUtilities.TrimLongTitles(Eval("Title", "{0}")) %>' />
                </cc:DropShadowPanel>
                <cc:DropShadowPanel runat="server" SkinID="ImageGallery-titleRight">
                    <strong>belongs to</strong>
                    <asp:HyperLink ID="_albumOwnerHyperLink" runat="server" NavigateUrl='<%# WebUtilities.GetViewItemUrl(((BaseItem)Container.DataItem).Owner) %>'
                        Text='<%# Eval("Owner.Title", "{0}") %>' />
                </cc:DropShadowPanel>
                <strong><asp:HyperLink ID="_editHyperLink" runat="server" NavigateUrl='<%# string.Format("~/Media/EditAlbum.aspx?{0}={1}", WebConstants.QueryVariables.BaseItemID, Eval("BaseItemID")) %>'
                    Text="Edit Gallery &gt;&gt;" Visible="<%# AlbumManager.CanModifyAlbum(((Album) Container.DataItem).BaseItemID) %>" /></strong>             
            </ItemTemplate>
        </asp:FormView>
        <asp:ObjectDataSource ID="ObjectDataSource4" runat="server" SelectMethod="GetAlbum"
            TypeName="WLQuickApps.SocialNetwork.Business.AlbumManager">
            <SelectParameters>
                <asp:QueryStringParameter Name="baseItemID" QueryStringField="baseItemID" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <ajaxToolkit:TabContainer ID="_albumTabContainer" runat="server">
            <ajaxToolkit:TabPanel ID="_pictureTabPanel" runat="server" CssClass="TabPanel-Tab">
                <ContentTemplate>
                    <cc:MetaGallery runat="server" ID="_pictureGallery" AllowPaging="true" ViewMode="Media" RepeatColumns="4" PageSize="12" DataSourceID="ObjectDataSource1" />
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMediaOfTypeByAlbumBaseItemID"
                        TypeName="WLQuickApps.SocialNetwork.Business.MediaManager" SelectCountMethod="GetMediaOfTypeByAlbumBaseItemIDCount"
                        StartRowIndexParameterName="startRowIndex" MaximumRowsParameterName="maximumRows" EnablePaging="true">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="albumBaseItemID" QueryStringField="baseItemID" Type="Int32" />
                            <asp:Parameter Name="mediaType" Type="object" DefaultValue="Picture" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br />
                    <asp:Label ID="_noPictures" runat="server" Text="There are no pictures in this gallery.<br>" Visible="false" />
                    <asp:HyperLink ID="_addPictureHyperLink" SkinID="ActionLink" runat="server">Add Picture &gt;&gt;</asp:HyperLink>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="_videoTabPanel" runat="server">
                <ContentTemplate>
                    <cc:MetaGallery runat="server" ID="_videoGallery" AllowPaging="true" PageSize="12"  ViewMode="Media" RepeatColumns="4" DataSourceID="ObjectDataSource2" />
                    <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" SelectMethod="GetMediaOfTypeByAlbumBaseItemID"
                        TypeName="WLQuickApps.SocialNetwork.Business.MediaManager" SelectCountMethod="GetMediaOfTypeByAlbumBaseItemIDCount"
                        StartRowIndexParameterName="startRowIndex" MaximumRowsParameterName="maximumRows" EnablePaging="true">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="albumBaseItemID" QueryStringField="baseItemID" Type="Int32" />
                            <asp:Parameter Name="mediaType" Type="object" DefaultValue="Video" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br />
                    <asp:Label ID="_noVideo" runat="server" Text="There are no video in this gallery.<br>" Visible="false" />
                    <asp:HyperLink ID="_addVideoHyperLink" SkinID="ActionLink" runat="server">Add Video &gt;&gt;</asp:HyperLink>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="_audioTabPanel" runat="server">
                <ContentTemplate>
                    <cc:MetaGallery runat="server" ID="_audioGallery" AllowPaging="true" PageSize="12" ViewMode="Media" RepeatColumns="4" DataSourceID="ObjectDataSource3" />
                    <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" SelectMethod="GetMediaOfTypeByAlbumBaseItemID"
                        TypeName="WLQuickApps.SocialNetwork.Business.MediaManager" SelectCountMethod="GetMediaOfTypeByAlbumBaseItemIDCount"
                        StartRowIndexParameterName="startRowIndex" MaximumRowsParameterName="maximumRows" EnablePaging="true">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="albumBaseItemID" QueryStringField="baseItemID" Type="Int32" />
                            <asp:Parameter Name="mediaType" Type="object" DefaultValue="Audio" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <br />
                    <asp:Label ID="_noAudio" runat="server" Text="There are no audio in this gallery.<br>" Visible="false" />
                    <asp:HyperLink ID="_addAudioHyperLink" SkinID="ActionLink" runat="server">Add Audio &gt;&gt;</asp:HyperLink>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
        <asp:Label ID="_noMedia" runat="server" Text="This gallery does not contain any media." Visible="false" />
    </cc:DropShadowPanel>
</asp:Content>
