﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScreenRecognition.Desktop.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.6.0.0")]
    public sealed partial class ProgramSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ProgramSettings defaultInstance = ((ProgramSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ProgramSettings())));
        
        public static ProgramSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("English")]
        public string OcrLanguages {
            get {
                return ((string)(this["OcrLanguages"]));
            }
            set {
                this["OcrLanguages"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Russian")]
        public string TranslatorLanguage {
            get {
                return ((string)(this["TranslatorLanguage"]));
            }
            set {
                this["TranslatorLanguage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("TesseractOCR")]
        public string OcrName {
            get {
                return ((string)(this["OcrName"]));
            }
            set {
                this["OcrName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MyMemory")]
        public string TranslatorName {
            get {
                return ((string)(this["TranslatorName"]));
            }
            set {
                this["TranslatorName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("nothing")]
        public string TranslatorApiKey {
            get {
                return ((string)(this["TranslatorApiKey"]));
            }
            set {
                this["TranslatorApiKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("LightBlue")]
        public string ResultColor {
            get {
                return ((string)(this["ResultColor"]));
            }
            set {
                this["ResultColor"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool MinimizeToTray {
            get {
                return ((bool)(this["MinimizeToTray"]));
            }
            set {
                this["MinimizeToTray"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoStartup {
            get {
                return ((bool)(this["AutoStartup"]));
            }
            set {
                this["AutoStartup"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool HotkeyEnabledStatus {
            get {
                return ((bool)(this["HotkeyEnabledStatus"]));
            }
            set {
                this["HotkeyEnabledStatus"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Control")]
        public string HotkeyModifier {
            get {
                return ((string)(this["HotkeyModifier"]));
            }
            set {
                this["HotkeyModifier"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("KEY_Q")]
        public string HotkeyKey {
            get {
                return ((string)(this["HotkeyKey"]));
            }
            set {
                this["HotkeyKey"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool T9EnableStatus {
            get {
                return ((bool)(this["T9EnableStatus"]));
            }
            set {
                this["T9EnableStatus"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://45.138.157.71:5046/api/")]
        public string ApiServerAddress {
            get {
                return ((string)(this["ApiServerAddress"]));
            }
            set {
                this["ApiServerAddress"] = value;
            }
        }
    }
}
