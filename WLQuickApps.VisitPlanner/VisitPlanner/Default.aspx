<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="VisitPlanner._Default" MasterPageFile="~/VisitPlannerMaster.master" %>

<asp:Content id="LoginSection" ContentPlaceHolderID="LoginArea" runat="server">
    <asp:Literal ID="WelcomeMessage" runat="server" />
    <div onclick="Logout();">
        <asp:HyperLink Text="-Sign In-" ID="SignInLink" ForeColor="#667e5b" Font-Underline="false" Font-Size="10pt" Font-Bold="true" NavigateUrl="http://login.live.com/wlogin.srf?appid=00163FFF80004E5F&alg=wsignin1.0" runat="server" />                  
    </div>
</asp:Content>

<asp:Content ID="MainContentSection" ContentPlaceHolderID="MainContentArea" runat="server">
    <div class="SearchBar">        
        <table width="525" border="0" cellpadding="0" cellspacing="0">
            <tr> 
                <td>
                    Home&nbsp;|&nbsp;Reservations&nbsp;|&nbsp;Packages&nbsp;|&nbsp;<span style="text-decoration:underline;">City 
                    Guides</span>&nbsp;|&nbsp;Contact Us &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;City Guide: <select ID="DestinationSelect" runat="server" onchange="SelectDestination(this.options[selectedIndex].value, this.options[selectedIndex].text);"></select>
                </td>
            </tr>
        </table>
    </div>
   
    <div id="SilverlightControlHost" style="position:absolute;top:0px; z-index:0;width:100%;height:100%;">          
        <script type="text/javascript">
            createSilverlight();
        </script>    
    </div>

    <div id='myMap' style="position: relative; width: 757px; height: 425px;top: 103px;left: 22px;z-index: 1;"></div>
    <div id="TourControlHost" style="z-index:2;width:757px;height:42px;position:absolute;top: -500px;left: 22px;"></div>
    <div id="TourPopupHost" style="z-index:1000;position:absolute;left:-500px;top:-500px"></div>
    <div id="InfoPopup" style="position:absolute;z-index:1000;top:-500px"></div>
    
    <div id="PlaceListHoverHost" style="position:absolute;top:-500px;left:-500px;z-index:35;width:284px;height:50px;"></div>
    <div id="floatingPin" style="position:absolute;z-index:1002;top:-500px"></div>
    <div id="AttractionDropBoxHost" style="position:absolute;top:-500px;left:-500px;z-index:-1;width:87px;height:75px;"></div>
    <div id="DirectionsButtonHost" style="position:absolute;top:103px;left:588px;z-index:-1;width:190px;height:36px;"></div>
    <div id="DirectionsDialogHost" style="position:absolute;top:103px;left:588px;z-index:-1;width:190px;height:330px;"></div>
    
    <div id="AddCustomPin" style="position:absolute;display:none;background-color:#07375D;top:-500px;z-index:1000;width:350px;height:280px;filter:alpha(opacity=90);opacity:0.9 ">
       <div class="innerDiv">
        <table>
            <tr class="AddCustomPinHeader">
                <td colspan="2"><center><b>Add Custom Item</b></center></td>   
            </tr>

            <tr class="spacer5">
                <td></td>
            </tr>

            <tr class="AddCustomPinHeader">
                <td colspan="2">Title:</td> 
            </tr>
            <tr>
                <td colspan="2">
                    <input id="CustomTitle" type="text" value="Custom Place" class="textbox" style="width:98%"/>
                </td>
            </tr>

            <tr class="spacer5">
                <td></td>
            </tr>
            
            <tr class="AddCustomPinHeader">
                <td>Description:</td> 
                <td>Notes:</td> 
            </tr>
            
            <tr>
                <td>
                    <textarea id="CustomDescription" rows="4" cols="24" class="textarea" ></textarea>
                </td>
                <td>
                    <textarea id="CustomNotes" rows="4" cols="24" class="textarea" ></textarea>
                </td>
            </tr>
            
            <tr class="spacer5">
                <td></td>
            </tr>
            
            <tr class="AddCustomPinHeader">
                <td>Image URL:</td> 
                <td>Video URL:</td>
            </tr>
            <tr>
                <td>
                    <input id="CustomImageURL" type="text" class="textbox" />
                </td>
                 <td>
                    <input id="CustomVideoURL" type="text" class="textbox" />
                </td>               
            </tr>
            
            <tr class="spacer5">
                <td></td>
            </tr>
            
            <tr>
                <td><center><input id="SaveCustom" type="button" value="Save" style="width:60%" onclick="javascript: saveCustomClick();" /></center></td>
                <td><center><input id="CancelCustom" type="button" value="Cancel" style="width:60%" onclick="javascript: cancelCustomClick();" /></center></td>   
            </tr>    
            
            <tr class="spacer5">
                <td></td>
            </tr>
        </table>
        </div>
    </div>

    <div id="AddConciergeItem" style="position:absolute;display:none;background-color:#07375D;top:-500px;z-index:1000;width:340px;height:400px;filter:alpha(opacity=90);opacity:0.9">
        <div class="innerDiv">
        <table>
            <tr class="AddCustomPinHeader">
                <td colspan="2"><center><b>Add New Concierge Item</b></center></td>   
            </tr>

            <tr class="spacer5">
                <td></td>
            </tr>

            <tr class="AddCustomPinHeader">
                <td>Title:</td> 
                <td>Category:</td> 
            </tr>
            <tr>
                <td>
                    <input id="AddConciergeTitle" type="text" class="textbox"/>
                </td>
                <td>
                    <select id="AddConciergeCategory">                         
                        <option value="p1">Accomodation</option>
                        <option value="p2">Art</option>
                        <option value="p3">Food</option>
                        <option value="p4">Music</option>
                        <option value="p5">Movie</option>
                        <option value="p6">Misc</option>
                    </select>
                </td>
            </tr>

            <tr class="spacer5">
                <td></td>
            </tr>
            
            <tr class="AddCustomPinHeader">
                <td>Short Description:</td> 
                <td>Long Description:</td> 
            </tr>
            <tr>
                <td>
                    <textarea id="ConciergeShortDesc" rows="5" cols="18" class="textarea"></textarea>
                </td>
                <td>
                    <textarea id="ConciergeLongDesc" rows="5" cols="18" class="textarea"></textarea>
                </td>                
            </tr>
                     
            <tr class="spacer5">
                <td></td>
            </tr>
            
            <tr class="AddCustomPinHeader">
                <td>Street Address:</td> 
                <td>City, State:</td> 
            </tr>
            <tr>
                <td>
                    <input id="ConciergeStreet" type="text" class="textbox"/>
                </td>
                <td>
                    <input id="ConciergeCityState" type="text" class="textbox"/>
                </td>
            </tr>
            
            <tr class="spacer5">
                <td></td>
            </tr>
            
            <tr class="AddCustomPinHeader">
                <td>Recurrence:</td> 
                <td>Keywords:</td> 
            </tr>
            <tr>
                <td>
                    <select id="AddConciergeRecurrence">                         
                        <option value="p1">Daily</option>
                        <option value="p2">Weekly</option>
                        <option value="p3">Monthly</option>
                        <option value="p4">Yearly</option>
                    </select>
                </td>
                <td>
                    <input id="AddConciergeKeywords" type="text" class="textbox"/>
                </td>
            </tr>
            
            <tr class="spacer5">
                <td></td>
            </tr>
            
            <tr class="AddCustomPinHeader">
                <td>Image URL:</td> 
                <td>Video URL:</td> 
            </tr>
            
            <tr>
                <td>
                    <input id="AddConciergeImageURL" type="text" class="textbox"/>
                </td>
                <td>
                    <input id="AddConciergeVideoURL" type="text" class="textbox"/>
                </td>
            </tr>
            
            <tr class="spacer5">
                <td></td>
            </tr>
            
            <tr>
                <td><center><input id="SaveNewConcierge" type="button" value="Save" style="width:60%" onclick="javascript: AddConciergeItemClick();" /></center></td>
                <td><center><input id="CancelNewConcierge" type="button" value="Cancel" style="width:60%" onclick="javascript: cancelAddConcierge();" /></center></td>   
            </tr>    
            
        </table>
        </div>
    </div>    
    
    <div id="Register" style="position:absolute;display:none;background-color:#07375D;top:-500px;z-index:1000;width:324px;height:175px;filter:alpha(opacity=90);opacity:0.9;border:solid 2px black; ">
        <table>
            <tr class="AddCustomPinHeader">
                <td colspan="2"><center><b>Welcome! <br />Register with Contoso Hotel!</b></center></td>   
            </tr>

            <tr class="spacer5">
                <td></td>
            </tr>

            <tr class="AddCustomPinHeader">
                <td>First Name:</td> 
                <td>Last Name:</td>
            </tr>
            
            <tr>
                <td>
                    <input id="EnterFirstName" type="text" class="textbox"/>
                </td>
                <td>
                    <input id="EnterLastName" type="text"  class="textbox"/>                
                </td>
            </tr>

            <tr class="spacer5">
                <td></td>
            </tr>
                       
            <tr class="spacer5">
                <td></td>
            </tr>

            <tr>
                <td><center><input id="registerOK" type="button" value="OK" style="width:50%" onclick="javascript:registerClick();" /></center></td> 
                <td><center><input id="registerNO" type="button" value="Cancel" style="width:50%" onclick="javascript:registerCancel();" /></center></td>  
            </tr>
            
            <tr class="spacer5">
                <td></td>
            </tr>
            <tr class="AddCustomPinHeader">
                <td id="registerError" colspan="2" style="color:Red;"></td> 
            </tr>
            
        </table>
    </div>
    
    
    <div id="ConciergeAddRemove" style="position:absolute;display:none;top:-500px;z-index:1000;filter:alpha(opacity=90);opacity:0.9">
        <div style="border:solid 2px black;">
            <div id="ConciergeAddLink" style="background-color:#07375D">
                <a class="AddRemoveConciergeLinks" href="javascript:ConciergeAddLinkClicked()">
                Add an attraction</a>
            </div>
            <div id="ConciergeRemoveLink" style="background-color:#07375D">
                <a class="AddRemoveConciergeLinks" href="javascript:ConciergeRemoveLinkClicked()">
                Delete this attraction</a>
            </div>
        </div>
    </div>
    
    
    <iframe frameborder="0" scrolling="no" 
        style="position:absolute;top: -500px;left: -500px;z-index:1; background-color:Transparent;width:10px;height:10px;" id="3DFrame">
    </iframe>
    <asp:Literal ID="UserIdArea" runat="server" Text=""  />
    <asp:Literal ID="UserTypeArea" runat="server" Text=""  />
    <asp:Literal ID="UserFirstNameArea" runat="server" Text=""  />
    <asp:Literal ID="UserLastNameArea" runat="server" Text=""  />
    <asp:Literal ID="SharedUserIdArea" runat="server" Text=""  />
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>
    <div style="visibility:hidden;">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
                <fieldset>
                    <asp:Literal ID="ConciergeStatusLiteral" runat="server" Text=""  />
                    <asp:Timer ID="ConciergeStatusTimer" runat="server"></asp:Timer>
                </fieldset>
            </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>
<asp:Content ID="FooterSection" ContentPlaceHolderID="FooterArea" runat="server">
    <div style="text-align: center">
        This is a <a href="http://dev.live.com/QuickApps/">demonstration site</a>. <br /><br />
        The source code can be downloaded from the <a href="http://www.codeplex.com/WLQuickApps/Release/ProjectReleases.aspx">Windows Live Platform Quick Apps</a> CodePlex Project.<br /><br />
        The example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious.  No association with any real company, organization, product, domain name, email address, logo, person, places, or events is intended or should be inferred.
    </div>
</asp:Content>
