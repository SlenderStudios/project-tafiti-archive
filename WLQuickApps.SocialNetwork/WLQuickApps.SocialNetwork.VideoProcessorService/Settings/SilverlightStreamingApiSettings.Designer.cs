﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1378
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WLQuickApps.SocialNetwork.VideoProcessorService.Settings {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "9.0.0.0")]
    internal sealed partial class SilverlightStreamingApiSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static SilverlightStreamingApiSettings defaultInstance = ((SilverlightStreamingApiSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new SilverlightStreamingApiSettings())));
        
        public static SilverlightStreamingApiSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://silverlight.services.live.com/")]
        public string ServiceRoot {
            get {
                return ((string)(this["ServiceRoot"]));
            }
            set {
                this["ServiceRoot"] = value;
            }
        }
    }
}