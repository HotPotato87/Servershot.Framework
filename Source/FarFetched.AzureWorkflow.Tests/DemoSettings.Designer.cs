﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Azure.Workflow.Tests {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class DemoSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static DemoSettings defaultInstance = ((DemoSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new DemoSettings())));
        
        public static DemoSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Endpoint=sb://whatsonglobal.servicebus.windows.net/;SharedSecretIssuer=owner;Shar" +
            "edSecretValue=kVWNOEp5cdNS8rZytOc02Cvp1gr0gh0AEpOLWzejWU4=")]
        public string ServiceBusConnectionString {
            get {
                return ((string)(this["ServiceBusConnectionString"]));
            }
            set {
                this["ServiceBusConnectionString"] = value;
            }
        }
    }
}
