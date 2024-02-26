<%@ Page Language="C#" AutoEventWireup="true" Async="true" Buffer="false" CodeFile="SynchronizeAlbum.aspx.cs" Inherits="Media_SynchronizeAlbum" %>

<asp:Content ID="_content" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>   
          <cc:DropShadowPanel ID="_pictureTypePanel" runat="server" CssClassPrefix="shadow" DisplayShadow="False"> 
            <asp:Panel runat="server" id="pnlNoTokenPresent">
                <p>You must grant permission to read/write to your Windows Live Spaces photos. Click here to <asp:hyperlink runat=server NavigateUrl="~/User/EditProfile.aspx" Text="edit your profile" />.</p>
            </asp:Panel>
             <asp:Panel runat="server" id="panEmpty" Visible="false">
                <p>Your Albums are synchronized with Space Album. Click here to view <asp:hyperlink ID="Hyperlink1" runat=server NavigateUrl="Default.aspx" Text="Home" />.</p>
            </asp:Panel>
            <asp:Panel runat="server" ID="AlbumPanel">
             <table>
                <tr>
                <td align="left">Album</td>
                <td align="left">Photo Preview (select album on left)</td>
                </tr>
                <tr>
                <td>
                    <asp:ListBox ID="lstSpacesAlbums" Width="300px" Height="100px" runat="server" SelectionMode=Multiple onselectedindexchanged="lstSpacesAlbums_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                </td>
                <td>
        <asp:ListBox ID="lstPhoto" Width="300px" Height="100px" runat="server" onselectedindexchanged="lstPhoto_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                </td>
                </tr>
                <tr>
                    <td colspan="2">
                    <asp:Label ID="lblStatusUpdate" Height="40px" runat="server"  ></asp:Label><br />
                    <div style="float: right">
                        <asp:Image ID="imgPreview" runat="server" Visible="false" />
                    </div>
                    </td>
                </tr>
                <tr>
                  <td ></td><td> 
                    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Synchronize" />
                    <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" Text="Load" Visible="false" />
                    </td>
                </tr>
                <tr>
                <td ></td><td>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <div class="progress_SynchronizeAlbum">
                            Please wait while your pictures are <br />
                            downloaded by Windows Live Spaces...
                        </div> 
                    </ProgressTemplate>
                </asp:UpdateProgress>
                </td>
                </tr>
                </table>
            
            </asp:Panel>
            </cc:DropShadowPanel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>