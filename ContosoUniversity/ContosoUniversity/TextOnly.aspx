<%@ Page Language="C#" Theme="TextOnly" %>
<%--<%@ Register Src="~/UserControls/RoomMateControl.ascx" TagName="RoomMateControl" TagPrefix="LQA" %>--%>
<%@ Register Src="~/UserControls/FeaturedSpacesControl.ascx" TagName="FeaturedSpacesControl" TagPrefix="LQA" %>
<%@ Register Src="~/UserControls/LatestNewsControl.ascx" TagName="LatestNewsControl" TagPrefix="LQA" %>
<%@ Register Src="~/UserControls/EventsControl.ascx" TagName="EventsControl" TagPrefix="LQA" %>
<%--<%@ Register Src="~/UserControls/MarketPlaceControl.ascx" TagName="MarketPlaceControl" TagPrefix="LQA" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Contoso University - Text Only</title>
</head>
<body>
    <h1>Contoso University</h1>
    <form id="form1" runat="server">
        <div>
            <h2>Latest News</h2>
            <LQA:LatestNewsControl ID="LatestNewsControl1" runat="server" />
            <h2>Campus Events</h2>
            <LQA:EventsControl ID="EventsControl1" runat="server" />
<%--            <h2>Room mates</h2>
            <LQA:RoomMateControl ID="RoomMateControl1" runat="server" />--%>
            <h2>Featured Spaces</h2>
            <LQA:FeaturedSpacesControl ID="FeaturedSpacesControl1" runat="server" TextMode="true" />
<%--            <h2>Market Place</h2>
            <LQA:MarketPlaceControl ID="MPC" TextMode="true" runat="server" />--%>
        </div>
    </form>
</body>
</html>
