<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LocationLinkControl.ascx.cs" Inherits="LocationLinkControl" %>

<asp:Label ID="_unknownLocationLabel" runat="server" Text="Unknown" Visible="false" />
<asp:HyperLink ID="_locationHyperLink" runat="server" ToolTip="Search for other events in this location" CssClass="link" />
<asp:HyperLink ID="_directionsHyperLink" runat="server" Text="Get Directions" ImageUrl="~/Images/Directions.png" CssClass="LocationIcon" ToolTip="Click for Directions" />