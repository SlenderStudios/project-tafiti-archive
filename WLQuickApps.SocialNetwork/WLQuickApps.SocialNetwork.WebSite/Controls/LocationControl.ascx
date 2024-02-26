<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LocationControl.ascx.cs" Inherits="LocationControl" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel runat="server" ID="_locationMiniPanel">
            <div style="margin-top:5px;">
                <asp:Label runat="server" ID="_initialNameLabel" Font-Bold="True" />
                <asp:LinkButton runat="server" ID="_chooseLocation" Font-Bold="True" OnClick="_chooseLocation_Click" CausesValidation="False" />
            </div>
        </asp:Panel>
        <cc:DropShadowPanel ID="_currentLocationPanel" runat="server" Visible="False" SkinID="LocationPanel">
            <div class="subform-label">
                <asp:Label ID="_nameLabel" runat="server" Text="Name" />
            </div>
            <div class="subform-field">
                <cc:SecureTextBox ID="_nameTextBox" runat="server" />
                <ajaxToolkit:AutoCompleteExtender ID="_autoComplete" runat="server" BehaviorID="AutoComplete"
                    TargetControlID="_nameTextBox" ServiceMethod="GetLocationAutoCompleteList" ServicePath="~/SiteService.asmx"
                    FirstRowSelected="True" MinimumPrefixLength="2" CompletionListCssClass="autocomplete_completionListElement"
                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem">
                    <Animations>
                        <OnShow>
                            <Sequence>
                                <%-- Make the completion list transparent and then show it --%>
                                <OpacityAction Opacity="0" />
                                <HideAction Visible="true" />
                                
                                <%--Cache the original size of the completion list the first time
                                    the animation is played and then set it to zero --%>
                                <ScriptAction Script="
                                    // Cache the size and setup the initial size
                                    var behavior = $find('AutoComplete');
                                    if (!behavior._height) {
                                        var target = behavior.get_completionList();
                                        behavior._height = target.offsetHeight - 2;
                                        target.style.height = '0px';
                                    }" />
                                
                                <%-- Expand from 0px to the appropriate size while fading in --%>
                                <Parallel Duration=".4">
                                    <FadeIn />
                                    <Length PropertyKey="height" StartValue="0" EndValueScript="$find('AutoComplete')._height" />
                                </Parallel>
                            </Sequence>
                        </OnShow>
                        <OnHide>
                            <%-- Collapse down to 0px and fade out --%>
                            <Parallel Duration=".4">
                                <FadeOut />
                                <Length PropertyKey="height" StartValueScript="$find('AutoComplete')._height" EndValue="0" />
                            </Parallel>
                        </OnHide>
                    </Animations>
                </ajaxToolkit:AutoCompleteExtender>
            </div>
            <div class="subform-label">
                <asp:Label ID="_address1Label" runat="server" Text="Address" />
            </div>
            <div class="subform-field">
                <cc:SecureTextBox ID="_address1TextBox" runat="server" />
            </div>
            <div class="subform-label">
                <asp:Label ID="_address2Label" runat="server" Text="&nbsp;" />
            </div>
            <div class="subform-field">
                <cc:SecureTextBox ID="_address2TextBox" runat="server" />
            </div>
            <div class="subform-label">
                <asp:Label ID="_cityLabel" runat="server" Text="City" />
            </div>
            <div class="subform-field">
                <cc:SecureTextBox ID="_cityTextBox" runat="server" />
            </div>
            <div class="subform-label">
                <asp:Label ID="_regionLabel" runat="server" Text="State" />
            </div>
            <div class="subform-field">
                <cc:SecureTextBox ID="_regionTextBox" runat="server" />
            </div>
            <div class="subform-label">
                <asp:Label ID="_postalCodeLabel" runat="server" Text="Postal Code" />
            </div>
            <div class="subform-field">
                <cc:SecureTextBox ID="_postalCodeTextBox" runat="server" />
            </div>
            <div class="subform-label">
                <asp:Label ID="_countryLabel" runat="server" Text="Country" />
            </div>
            <div class="subform-field">
                <asp:DropDownList ID="_countryDropDown" runat="server">
                    <asp:ListItem Text="-- Select a Country --" Value=""></asp:ListItem>
                    <asp:ListItem Text="Andorra" Value="Andorra"></asp:ListItem>
                    <asp:ListItem Text="Australia" Value="Australia"></asp:ListItem>
                    <asp:ListItem Text="Austria" Value="Austria"></asp:ListItem>
                    <asp:ListItem Text="Belgium" Value="Belgium"></asp:ListItem>
                    <asp:ListItem Text="Brazil" Value="Brazil"></asp:ListItem>
                    <asp:ListItem Text="Canada" Value="Canada"></asp:ListItem>
                    <asp:ListItem Text="Czech Republic" Value="Czech Republic"></asp:ListItem>
                    <asp:ListItem Text="Denmark" Value="Denmark"></asp:ListItem>
                    <asp:ListItem Text="Finland" Value="Finland"></asp:ListItem>
                    <asp:ListItem Text="France" Value="France"></asp:ListItem>
                    <asp:ListItem Text="Germany" Value="Germany"></asp:ListItem>
                    <asp:ListItem Text="Greece" Value="Greece"></asp:ListItem>
                    <asp:ListItem Text="Hong Kong" Value="Hong Kong"></asp:ListItem>
                    <asp:ListItem Text="Ireland" Value="Ireland"></asp:ListItem>
                    <asp:ListItem Text="Italy" Value="Italy"></asp:ListItem>
                    <asp:ListItem Text="Liechtenstein" Value="Liechtenstein"></asp:ListItem>
                    <asp:ListItem Text="Luxembourg" Value="Luxembourg"></asp:ListItem>
                    <asp:ListItem Text="Malaysia" Value="Malaysia"></asp:ListItem>
                    <asp:ListItem Text="Mexico" Value="Mexico"></asp:ListItem>
                    <asp:ListItem Text="Monaco" Value="Monaco"></asp:ListItem>
                    <asp:ListItem Text="New Zealand" Value="New Zealand"></asp:ListItem>
                    <asp:ListItem Text="Norway" Value="Norway"></asp:ListItem>
                    <asp:ListItem Text="Portugal" Value="Portugal"></asp:ListItem>
                    <asp:ListItem Text="Puerto Rico" Value="Puerto Rico"></asp:ListItem>
                    <asp:ListItem Text="San Marino" Value="San Marino"></asp:ListItem>
		            <asp:ListItem Text="Singapore" Value="Singapore"></asp:ListItem>
                    <asp:ListItem Text="Slovakia" Value="Slovakia"></asp:ListItem>
                    <asp:ListItem Text="Spain" Value="Spain"></asp:ListItem>
                    <asp:ListItem Text="Sweden" Value="Sweden"></asp:ListItem>
                    <asp:ListItem Text="Switzerland" Value="Switzerland"></asp:ListItem>
                    <asp:ListItem Text="Taiwan" Value="Taiwan"></asp:ListItem>
                    <asp:ListItem Text="The Netherlands" Value="The Netherlands"></asp:ListItem>
                    <asp:ListItem Text="United Kingdom" Value="United Kingdom"></asp:ListItem>
                    <asp:ListItem Text="USA" Value="USA"></asp:ListItem>
                    <asp:ListItem Text="Vatican City" Value="Vatican City"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="subform-field" style="text-align:right;padding-right: 40px;">
                <asp:LinkButton ID="_clearButton" runat="Server" OnClick="_clearButton_Click" Text="(Clear form)" CausesValidation="false" />
            </div>
        </cc:DropShadowPanel>

<script type="text/javascript">
        <!--
            var autoCompleteBehavior;
            var address1;
            var address2;
            var city;
            var region;
            var postalCode;
            var country;
            
            function SetAutoCompleteHandler()
            {
                autoCompleteBehavior = $find("<%= this._autoComplete.BehaviorID %>");
            
                if (autoCompleteBehavior == null)
                {
                    return;
                }

                autoCompleteBehavior.add_itemSelected(AutoComplete_ItemSelected);
                
                address1 = $get("<%= this._address1TextBox.ClientID %>");
                address2 = $get("<%= this._address2TextBox.ClientID %>");
                city = $get("<%= this._cityTextBox.ClientID %>");
                region = $get("<%= this._regionTextBox.ClientID %>");
                postalCode = $get("<%= this._postalCodeTextBox.ClientID %>");
                country = $get("<%= this._countryDropDown.ClientID %>");
            }
            
            function AutoComplete_ItemSelected(sender, eventArgs)
            {
                WLQuickApps.SocialNetwork.WebSite.SiteService.GetNamedLocation(eventArgs.get_text(), OnLocationRetrieved);
            }
            
            function OnLocationRetrieved(newLocation)
            {
                if (newLocation == null)
                {
                    return;
                }
                                
                address1.value = newLocation.Address1;
                address2.value = newLocation.Address2;
                city.value = newLocation.City;
                region.value = newLocation.Region;
                postalCode.value = newLocation.PostalCode;
                country.value = newLocation.Country;
            }
            
            Sys.Application.add_load(SetAutoCompleteHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(SetAutoCompleteHandler);
        //-->
        </script>

    </ContentTemplate>
</asp:UpdatePanel>
