﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.1433.
// 
#pragma warning disable 1591

namespace WLQuickApps.ContosoSoda.Alerts.Live.Alerts.Message {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MessageSoapBinding", Namespace="http://services.alerts.live.com/axis/services/Message")]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(RecUsersGroup))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(RecServicesInformation))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(RecServicesContact))]
    public partial class MessageWebServicesService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback DeliverOperationCompleted;
        
        private System.Threading.SendOrPostCallback GroupDeliverOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MessageWebServicesService() {
            this.Url = global::WLQuickApps.ContosoSoda.Alerts.Properties.Settings.Default.WLQuickApps_ContosoSoda_Alerts_Live_Alerts_Message_MessageWebServicesService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event DeliverCompletedEventHandler DeliverCompleted;
        
        /// <remarks/>
        public event GroupDeliverCompletedEventHandler GroupDeliverCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://soapservices.messagecast.net", ResponseNamespace="http://services.alerts.live.com/axis/services/Message")]
        [return: System.Xml.Serialization.SoapElementAttribute("DeliverReturn")]
        public RecDetailRequestResponse Deliver(RecServicesHeader h, RecServicesIdentification ID, RecServicesMessage m) {
            object[] results = this.Invoke("Deliver", new object[] {
                        h,
                        ID,
                        m});
            return ((RecDetailRequestResponse)(results[0]));
        }
        
        /// <remarks/>
        public void DeliverAsync(RecServicesHeader h, RecServicesIdentification ID, RecServicesMessage m) {
            this.DeliverAsync(h, ID, m, null);
        }
        
        /// <remarks/>
        public void DeliverAsync(RecServicesHeader h, RecServicesIdentification ID, RecServicesMessage m, object userState) {
            if ((this.DeliverOperationCompleted == null)) {
                this.DeliverOperationCompleted = new System.Threading.SendOrPostCallback(this.OnDeliverOperationCompleted);
            }
            this.InvokeAsync("Deliver", new object[] {
                        h,
                        ID,
                        m}, this.DeliverOperationCompleted, userState);
        }
        
        private void OnDeliverOperationCompleted(object arg) {
            if ((this.DeliverCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.DeliverCompleted(this, new DeliverCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("", RequestNamespace="http://soapservices.messagecast.net", ResponseNamespace="http://services.alerts.live.com/axis/services/Message")]
        [return: System.Xml.Serialization.SoapElementAttribute("GroupDeliverReturn")]
        public RecServicesRequestResponse GroupDeliver(RecServicesHeader h, RecServicesIdentification ID, RecServicesGroupMessage gm) {
            object[] results = this.Invoke("GroupDeliver", new object[] {
                        h,
                        ID,
                        gm});
            return ((RecServicesRequestResponse)(results[0]));
        }
        
        /// <remarks/>
        public void GroupDeliverAsync(RecServicesHeader h, RecServicesIdentification ID, RecServicesGroupMessage gm) {
            this.GroupDeliverAsync(h, ID, gm, null);
        }
        
        /// <remarks/>
        public void GroupDeliverAsync(RecServicesHeader h, RecServicesIdentification ID, RecServicesGroupMessage gm, object userState) {
            if ((this.GroupDeliverOperationCompleted == null)) {
                this.GroupDeliverOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGroupDeliverOperationCompleted);
            }
            this.InvokeAsync("GroupDeliver", new object[] {
                        h,
                        ID,
                        gm}, this.GroupDeliverOperationCompleted, userState);
        }
        
        private void OnGroupDeliverOperationCompleted(object arg) {
            if ((this.GroupDeliverCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GroupDeliverCompleted(this, new GroupDeliverCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecServicesHeader {
        
        private string messageIDField;
        
        private string timestampField;
        
        private string versionField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string messageID {
            get {
                return this.messageIDField;
            }
            set {
                this.messageIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string timestamp {
            get {
                return this.timestampField;
            }
            set {
                this.timestampField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecServicesGroupMessage {
        
        private string actionURLField;
        
        private string contentField;
        
        private string emailMessageField;
        
        private RecServicesContact[] fromContactsField;
        
        private string groupNameField;
        
        private string localeField;
        
        private string messengerMessageField;
        
        private string mobileMessageField;
        
        private string superToastMessageField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string actionURL {
            get {
                return this.actionURLField;
            }
            set {
                this.actionURLField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string emailMessage {
            get {
                return this.emailMessageField;
            }
            set {
                this.emailMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public RecServicesContact[] fromContacts {
            get {
                return this.fromContactsField;
            }
            set {
                this.fromContactsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string groupName {
            get {
                return this.groupNameField;
            }
            set {
                this.groupNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string locale {
            get {
                return this.localeField;
            }
            set {
                this.localeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string messengerMessage {
            get {
                return this.messengerMessageField;
            }
            set {
                this.messengerMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string mobileMessage {
            get {
                return this.mobileMessageField;
            }
            set {
                this.mobileMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string superToastMessage {
            get {
                return this.superToastMessageField;
            }
            set {
                this.superToastMessageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecServicesContact {
        
        private System.Nullable<int> sGIDField;
        
        private string fromField;
        
        private int ordField;
        
        private int sendToContactIDField;
        
        private string toField;
        
        private string transportField;
        
        private int transportIDField;
        
        private int transportTypeIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public System.Nullable<int> SGID {
            get {
                return this.sGIDField;
            }
            set {
                this.sGIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string from {
            get {
                return this.fromField;
            }
            set {
                this.fromField = value;
            }
        }
        
        /// <remarks/>
        public int ord {
            get {
                return this.ordField;
            }
            set {
                this.ordField = value;
            }
        }
        
        /// <remarks/>
        public int sendToContactID {
            get {
                return this.sendToContactIDField;
            }
            set {
                this.sendToContactIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string to {
            get {
                return this.toField;
            }
            set {
                this.toField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string transport {
            get {
                return this.transportField;
            }
            set {
                this.transportField = value;
            }
        }
        
        /// <remarks/>
        public int transportID {
            get {
                return this.transportIDField;
            }
            set {
                this.transportIDField = value;
            }
        }
        
        /// <remarks/>
        public int transportTypeID {
            get {
                return this.transportTypeIDField;
            }
            set {
                this.transportTypeIDField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecUsersGroup {
        
        private RecServicesContact[] fromContactsField;
        
        private string groupNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public RecServicesContact[] fromContacts {
            get {
                return this.fromContactsField;
            }
            set {
                this.fromContactsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string groupName {
            get {
                return this.groupNameField;
            }
            set {
                this.groupNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecServicesInformation {
        
        private string itemField;
        
        private RecServicesResponse responseField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string item {
            get {
                return this.itemField;
            }
            set {
                this.itemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public RecServicesResponse response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecServicesResponse {
        
        private string sOAPRequestField;
        
        private string sOAPResponseField;
        
        private int statusCodeField;
        
        private string statusReasonField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string SOAPRequest {
            get {
                return this.sOAPRequestField;
            }
            set {
                this.sOAPRequestField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string SOAPResponse {
            get {
                return this.sOAPResponseField;
            }
            set {
                this.sOAPResponseField = value;
            }
        }
        
        /// <remarks/>
        public int statusCode {
            get {
                return this.statusCodeField;
            }
            set {
                this.statusCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string statusReason {
            get {
                return this.statusReasonField;
            }
            set {
                this.statusReasonField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(RecGroupsRequestResponse))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(RecAlertsRequestResponse))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(RecValidRequestResponse))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(RecUsersRequestResponse))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(RecDetailRequestResponse))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecServicesRequestResponse {
        
        private RecServicesHeader headerField;
        
        private RecServicesResponse responseField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public RecServicesHeader header {
            get {
                return this.headerField;
            }
            set {
                this.headerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public RecServicesResponse response {
            get {
                return this.responseField;
            }
            set {
                this.responseField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecGroupsRequestResponse : RecServicesRequestResponse {
        
        private string[] subscriptionGroupsField;
        
        private string[] unSubscribedGroupsField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string[] subscriptionGroups {
            get {
                return this.subscriptionGroupsField;
            }
            set {
                this.subscriptionGroupsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string[] unSubscribedGroups {
            get {
                return this.unSubscribedGroupsField;
            }
            set {
                this.unSubscribedGroupsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecAlertsRequestResponse : RecServicesRequestResponse {
        
        private string uRLField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string URL {
            get {
                return this.uRLField;
            }
            set {
                this.uRLField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecValidRequestResponse : RecServicesRequestResponse {
        
        private bool validRequestField;
        
        /// <remarks/>
        public bool validRequest {
            get {
                return this.validRequestField;
            }
            set {
                this.validRequestField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecUsersRequestResponse : RecServicesRequestResponse {
        
        private System.Nullable<int> totalNumUsersField;
        
        private RecUsersGroup[] usersField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public System.Nullable<int> totalNumUsers {
            get {
                return this.totalNumUsersField;
            }
            set {
                this.totalNumUsersField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public RecUsersGroup[] users {
            get {
                return this.usersField;
            }
            set {
                this.usersField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecDetailRequestResponse : RecServicesRequestResponse {
        
        private RecServicesInformation[] infoField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public RecServicesInformation[] info {
            get {
                return this.infoField;
            }
            set {
                this.infoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecServicesMessage {
        
        private string actionURLField;
        
        private RecServicesContact[] contactsField;
        
        private string contentField;
        
        private string emailMessageField;
        
        private string localeField;
        
        private string messengerMessageField;
        
        private string methodField;
        
        private string mobileMessageField;
        
        private string superToastMessageField;
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string actionURL {
            get {
                return this.actionURLField;
            }
            set {
                this.actionURLField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public RecServicesContact[] contacts {
            get {
                return this.contactsField;
            }
            set {
                this.contactsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string emailMessage {
            get {
                return this.emailMessageField;
            }
            set {
                this.emailMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string locale {
            get {
                return this.localeField;
            }
            set {
                this.localeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string messengerMessage {
            get {
                return this.messengerMessageField;
            }
            set {
                this.messengerMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string method {
            get {
                return this.methodField;
            }
            set {
                this.methodField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string mobileMessage {
            get {
                return this.mobileMessageField;
            }
            set {
                this.mobileMessageField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string superToastMessage {
            get {
                return this.superToastMessageField;
            }
            set {
                this.superToastMessageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="messagecast.net")]
    public partial class RecServicesIdentification {
        
        private int pINIDField;
        
        private string pwField;
        
        /// <remarks/>
        public int PINID {
            get {
                return this.pINIDField;
            }
            set {
                this.pINIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string PW {
            get {
                return this.pwField;
            }
            set {
                this.pwField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void DeliverCompletedEventHandler(object sender, DeliverCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DeliverCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal DeliverCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RecDetailRequestResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RecDetailRequestResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void GroupDeliverCompletedEventHandler(object sender, GroupDeliverCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GroupDeliverCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GroupDeliverCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RecServicesRequestResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RecServicesRequestResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591