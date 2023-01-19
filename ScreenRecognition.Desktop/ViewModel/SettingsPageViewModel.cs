using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenRecognition.Desktop.ViewModel
{
    public class SettingsPageViewModel : BaseViewModel
    {
        #region Shields and Properties
        #region Shields only
        private UniversalController _controller;
        #endregion

        #region Shared custom
        public PropShieldModel<Visibility> DisconnectedPanelVisibilityCustom { get; set; } = new();
        public PropShieldModel<Visibility> ProfilePanelVisibilityCustom { get; set; } = new();
        #endregion
        #region Program Settings custom
        public PropShieldModel<Setting> SettingsCustom { get; set; } = new();

        public PropShieldModel<List<Language?>> LanguageListCustom { get; set; } = new();

        public PropShieldModel<Language?> OcrLanguageCustom { get; set; } = new();
        public PropShieldModel<Language?> TranslatorLanguageCustom { get; set; } = new();

        public PropShieldModel<List<Translator?>> TranslatorListCustom { get; set; } = new();
        public PropShieldModel<Translator?> SelectedTranslatorCustom { get; set; } = new();

        public PropShieldModel<List<Ocr?>> OcrListCustom { get; set; } = new();
        public PropShieldModel<Ocr?> SelectedOcrCustom { get; set; } = new();

        public PropShieldModel<string> ResultColorCustom { get; set; } = new();
        public PropShieldModel<string> TranslatorApiKeyCustom { get; set; } = new();
        public PropShieldModel<string> CurrentProfileNameCustom { get; set; } = new();

        public PropShieldModel<bool> StartupWithSystemCustom { get; set; } = new();
        public PropShieldModel<bool> MinimizeToTrayCustom { get; set; } = new();
        public PropShieldModel<bool> T9EnableCustom { get; set; } = new();

        public PropShieldModel<List<GlobalHotKeys.Native.Types.VirtualKeyCode>> HotkeyKeyListCustom { get; set; } = new();
        public PropShieldModel<GlobalHotKeys.Native.Types.VirtualKeyCode> SelectedHotkeyKeyCustom { get; set; } = new();

        public PropShieldModel<List<GlobalHotKeys.Native.Types.Modifiers>> HotkeyModifiersListCustom { get; set; } = new();
        public PropShieldModel<GlobalHotKeys.Native.Types.Modifiers> SelectedHotkeyModifierCustom { get; set; } = new();
        #endregion
        #region Profile Settings custom
        public PropShieldModel<User?> UserCustom { get; set; } = new();

        public PropShieldModel<List<Country?>> CountryListCustom { get; set; } = new();
        public PropShieldModel<Country?> SelectedCountryCustom { get; set; } = new();

        public PropShieldModel<List<Setting?>> SettingsListCustom { get; set; } = new();
        public PropShieldModel<Setting?> SelectedSettingCustom { get; set; } = new();
        #endregion
        #endregion

        public SettingsPageViewModel()
        {
            _controller = new UniversalController();
            AuthChecker();

            OnStartup();
        }

        private async void OnStartup()
        {
            // Получение списка кнопок для хоткея
            HotkeyModifiersListCustom.Property = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.Modifiers)).Cast<GlobalHotKeys.Native.Types.Modifiers>().ToList();
            HotkeyKeyListCustom.Property = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.VirtualKeyCode)).Cast<GlobalHotKeys.Native.Types.VirtualKeyCode>().ToList();

            // Получение списка листов
            await Task.Run(async () =>
            {
                LanguageListCustom.Property = await _controller.Get<List<Language?>, List<Language?>>("Settings/Languages");
                CountryListCustom.Property = await _controller.Get<List<Country?>, List<Country?>>("Settings/Countries");
                SettingsListCustom.Property = await _controller.Get<List<Setting?>, List<Setting?>>("Settings/Settings");
                OcrListCustom.Property = await _controller.Get<List<Ocr?>, List<Ocr?>>("Settings/Ocrs");
                TranslatorListCustom.Property = await _controller.Get<List<Translator?>, List<Translator?>>("Settings/Translators");
            });

            if (ConnectedUserSingleton.ConnectionStatus == false)
                return;

            // Получение выбранных результатов
            await Task.Run(async () =>
            {
                SettingsCustom.Property = await _controller.Get<Setting?, Setting?>($"Settings/ProfileSettings?userId={ConnectedUserSingleton.User.Id}&name=default");
            });

            ApplyDefaultSettings();
            await LoadUserInfo();
        }

        private async Task LoadUserInfo()
        {
            UserCustom.Property = ConnectedUserSingleton.User;
            CountryListCustom.Property = await _controller.Get<List<Country?>, List<Country?>>($"Settings/Countries");
            SelectedCountryCustom.Property = CountryListCustom.Property.FirstOrDefault(e => e.Id == UserCustom.Property.CountryId);
            SettingsListCustom.Property = await _controller.Get<List<Setting?>, List<Setting?>>($"Settings/Settings");
            SelectedSettingCustom.Property = SettingsListCustom.Property.FirstOrDefault(e => e.Name == CurrentProfileNameCustom.Property);
        }

        private void ApplyDefaultSettings()
        {
            SelectedOcrCustom.Property = OcrListCustom.Property.FirstOrDefault(e => e.Id == SettingsCustom.Property.SelectedOcrid);
            SelectedTranslatorCustom.Property = TranslatorListCustom.Property.FirstOrDefault(e => e.Id == SettingsCustom.Property.SelectedTranslatorId);
            TranslatorApiKeyCustom.Property = SettingsCustom.Property.TranslatorApiKey;
            OcrLanguageCustom.Property = LanguageListCustom.Property.FirstOrDefault(e => e.Id == SettingsCustom.Property.InputLanguageId);
            TranslatorLanguageCustom.Property = LanguageListCustom.Property.FirstOrDefault(e => e.Id == SettingsCustom.Property.OutputLanguageId);
            ResultColorCustom.Property = SettingsCustom.Property.ResultColor;
            CurrentProfileNameCustom.Property = SettingsCustom.Property.Name;
        }

        public async void SaveSettings()
        {
            if (string.IsNullOrEmpty(ConnectedUserSingleton.Password))
            {
                HandyControl.Controls.MessageBox.Show("Вы не авторизованы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            // Переделать некст строчку
            if (OcrLanguageCustom.Property == null || TranslatorLanguageCustom.Property == null || CurrentProfileNameCustom.Property == null || ResultColorCustom.Property == null || SelectedOcrCustom.Property == null || SelectedOcrCustom.Property == null || SelectedTranslatorCustom.Property == null || TranslatorApiKeyCustom.Property == null)
            {
                return;
            }

            var settings = new Setting
            {
                InputLanguageId = OcrLanguageCustom.Property.Id,
                OutputLanguageId = TranslatorLanguageCustom.Property.Id,
                Name = CurrentProfileNameCustom.Property,
                ResultColor = ResultColorCustom.Property,
                SelectedOcrid = SelectedOcrCustom.Property.Id,
                SelectedTranslatorId = SelectedTranslatorCustom.Property.Id,
                TranslatorApiKey = TranslatorApiKeyCustom.Property,
                UserId = ConnectedUserSingleton.User.Id,
            };

            await Task.Run(async () =>
            {
                await _controller.Post<Setting?, Setting?>("Settings/SaveProfile", settings);
            });
        }

        private void AuthChecker()
        {
            if (ConnectedUserSingleton.ConnectionStatus == true)
            {
                ProfilePanelVisibilityCustom.Property = Visibility.Visible;
                DisconnectedPanelVisibilityCustom.Property = Visibility.Collapsed;
            }
            else
            {
                ProfilePanelVisibilityCustom.Property = Visibility.Collapsed;
                DisconnectedPanelVisibilityCustom.Property = Visibility.Visible;
            }
        }
    }
}
