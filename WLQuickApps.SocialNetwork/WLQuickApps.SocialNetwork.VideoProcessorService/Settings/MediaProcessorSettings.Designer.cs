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
    internal sealed partial class MediaProcessorSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static MediaProcessorSettings defaultInstance = ((MediaProcessorSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new MediaProcessorSettings())));
        
        public static MediaProcessorSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("c:\\program files\\microsoft expression\\media encoder 1.0\\mediaencoder.exe")]
        public string CommandLineTarget {
            get {
                return ((string)(this["CommandLineTarget"]));
            }
            set {
                this["CommandLineTarget"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("d:\\vid\\servicedir\\")]
        public string ServiceWorkingDir {
            get {
                return ((string)(this["ServiceWorkingDir"]));
            }
            set {
                this["ServiceWorkingDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".\\Private$\\TestQueue")]
        public string MediaSubmissionQueue {
            get {
                return ((string)(this["MediaSubmissionQueue"]));
            }
            set {
                this["MediaSubmissionQueue"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".")]
        public string EventLogMachineName {
            get {
                return ((string)(this["EventLogMachineName"]));
            }
            set {
                this["EventLogMachineName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Application")]
        public string EventLogName {
            get {
                return ((string)(this["EventLogName"]));
            }
            set {
                this["EventLogName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("WLQuickApps Media Processor Service")]
        public string EventLogSourceName {
            get {
                return ((string)(this["EventLogSourceName"]));
            }
            set {
                this["EventLogSourceName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int EventLogThreshold {
            get {
                return ((int)(this["EventLogThreshold"]));
            }
            set {
                this["EventLogThreshold"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("d:\\vid\\processordir\\")]
        public string ProcessorWorkingDir {
            get {
                return ((string)(this["ProcessorWorkingDir"]));
            }
            set {
                this["ProcessorWorkingDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DebugFlag {
            get {
                return ((bool)(this["DebugFlag"]));
            }
            set {
                this["DebugFlag"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int CommandLineTimeout {
            get {
                return ((int)(this["CommandLineTimeout"]));
            }
            set {
                this["CommandLineTimeout"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("BasePlayer.js, MicrosoftAjax.js, player.js, StartPlayer.js, player.xaml, preview." +
            "jpg, manifest.xml")]
        public string EMEFilesToPackage {
            get {
                return ((string)(this["EMEFilesToPackage"]));
            }
            set {
                this["EMEFilesToPackage"] = value;
            }
        }
    }
}