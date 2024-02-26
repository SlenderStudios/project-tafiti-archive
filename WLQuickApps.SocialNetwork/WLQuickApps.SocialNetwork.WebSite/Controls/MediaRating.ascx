<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MediaRating.ascx.cs" Inherits="MediaRating" %>

<ajaxToolkit:Rating runat="server" ID="_rating" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar"
    FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" OnChanged="_rating_Changed" />