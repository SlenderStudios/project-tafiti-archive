﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WLQuickApps.FieldManager.Business.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://www.weather.gov/forecasts/xml/SOAP_server/ndfdXMLserver.php")]
        public string WLQuickApps_FieldManager_Business_WeatherService_ndfdXML {
            get {
                return ((string)(this["WLQuickApps_FieldManager_Business_WeatherService_ndfdXML"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://services.alerts.live-ppe.com/axis/services/Message")]
        public string WLQuickApps_FieldManager_Business_LiveAlertsMessageService_MessageWebServicesService {
            get {
                return ((string)(this["WLQuickApps_FieldManager_Business_LiveAlertsMessageService_MessageWebServicesServ" +
                    "ice"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://services.alerts.live-ppe.com/axis/services/Subscription")]
        public string WLQuickApps_FieldManager_Business_LiveAlertsSubscriptionService_AlertsWebServicesService {
            get {
                return ((string)(this["WLQuickApps_FieldManager_Business_LiveAlertsSubscriptionService_AlertsWebServices" +
                    "Service"]));
            }
        }
    }
}
