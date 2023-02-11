namespace ScreenRecognition.Desktop.Models
{
    public class DefaultProgramSettings
    {
        public string OcrLanguage { get; set; } = null!;
        public string TranslatorLanguage { get; set; } = null!;
        public string OcrName { get; set; } = null!;
        public string TranslatorName { get; set; } = null!;
        public string? TranslationApiKey { get; set; }
        public string ResultColor { get; set; } = null!;
        public bool MinimizeToTray { get; set; } = false;
        public bool AutoStartup { get; set; } = false;
        public bool HotkeyEnabledStatus { get; set; } = true;
        public string HotkeyModifier { get; set; } = null!;
        public string HotkeyKey { get; set; } = null!;

        public DefaultProgramSettings()
        {
            GetDefaultSettings();
        }

        private void GetDefaultSettings()
        {
            OcrLanguage = Properties.ProgramSettings.Default.OcrLanguages;
            TranslatorLanguage = Properties.ProgramSettings.Default.TranslatorLanguage;
            TranslationApiKey = Properties.ProgramSettings.Default.TranslatorApiKey;
            OcrName = Properties.ProgramSettings.Default.OcrName;
            TranslatorName = Properties.ProgramSettings.Default.TranslatorName;
            TranslationApiKey = Properties.ProgramSettings.Default.TranslatorApiKey;
            ResultColor = Properties.ProgramSettings.Default.ResultColor;
            MinimizeToTray = Properties.ProgramSettings.Default.MinimizeToTray;
            AutoStartup = Properties.ProgramSettings.Default.AutoStartup;
            HotkeyEnabledStatus = Properties.ProgramSettings.Default.HotkeyEnabledStatus;
            HotkeyModifier = Properties.ProgramSettings.Default.HotkeyModifier;
            HotkeyKey = Properties.ProgramSettings.Default.HotkeyKey;
        }
    }
}
