using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenRecognition.Desktop.ViewModel
{
    public class SettingsPageViewModel
    {
        #region Shields and Properties
        #region Shared
        public PropShieldModel<Visibility> DisconnectedPanelVisibility { get; set; } = new();
        public PropShieldModel<Visibility> ProfilePanelVisibility { get; set; } = new();

        #endregion
        #region Program Settings
        public PropShieldModel<List<Language>> LanguageList { get; set; } = new();
        public PropShieldModel<Language> OcrLanguage { get; set; } = new();
        public PropShieldModel<Language> TranslatorLanguage { get; set; } = new();

        public PropShieldModel<List<Translator>> TranslatorList { get; set; } = new();
        public PropShieldModel<Translator> SelectedTranslator { get; set; } = new();

        public PropShieldModel<List<Ocr>> OcrList { get; set; } = new();
        public PropShieldModel<Ocr> SelectedOcr { get; set; } = new();

        public PropShieldModel<string> ResultColor { get; set; } = new();
        public PropShieldModel<string> TranslatorApiKey { get; set; } = new();
        public PropShieldModel<string> CurrentProfileName { get; set; } = new();

        public PropShieldModel<bool> StartupWithSystem { get; set; } = new();
        public PropShieldModel<bool> MinimizeToTray { get; set; } = new();
        public PropShieldModel<bool> T9Enable { get; set; } = new();

        public PropShieldModel<List<GlobalHotKeys.Native.Types.VirtualKeyCode>> HotkeyKeyList { get; set; } = new();
        public PropShieldModel<GlobalHotKeys.Native.Types.VirtualKeyCode> SelectedHotkeyKey { get; set; } = new();

        public PropShieldModel<List<GlobalHotKeys.Native.Types.Modifiers>> HotkeyModifiersList { get; set; } = new();
        public PropShieldModel<GlobalHotKeys.Native.Types.Modifiers> SelectedHotkeyModifier { get; set; } = new();
        #endregion
        #region Profile Settings
        public PropShieldModel<User> User { get; set; } = new();

        public PropShieldModel<List<Country>> CountryList { get; set; } = new();

        public PropShieldModel<List<Setting>> SettingsList { get; set; } = new();
        public PropShieldModel<Setting> SelectedSetting { get; set; } = new();
        #endregion

        private UniversalController _controller;
        #endregion

        public SettingsPageViewModel()
        {
            _controller = new UniversalController();
            AuthChecker();

            HotkeyModifiersList.Property = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.Modifiers)).Cast<GlobalHotKeys.Native.Types.Modifiers>().ToList();
            HotkeyKeyList.Property = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.VirtualKeyCode)).Cast<GlobalHotKeys.Native.Types.VirtualKeyCode>().ToList();

            OnStartup();
        }

        private async void OnStartup()
        {
            await Task.Run(async() =>
            {
                LanguageList.Property = await _controller.Get<List<Language>, List<Language>>("");
            });
        }

        private void AuthChecker()
        {
            if (ConnectedUserSingleton.ConnectionStatus == true)
            {
                ProfilePanelVisibility.Property = Visibility.Visible;
                DisconnectedPanelVisibility.Property = Visibility.Collapsed;
            }
            else
            {
                ProfilePanelVisibility.Property = Visibility.Collapsed;
                DisconnectedPanelVisibility.Property = Visibility.Visible;
            }
        }
    }
}
