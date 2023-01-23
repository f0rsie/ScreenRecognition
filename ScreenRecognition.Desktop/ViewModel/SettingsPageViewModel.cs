﻿using ScreenRecognition.Command;
using ScreenRecognition.Desktop.Controllers;
using ScreenRecognition.Desktop.Models;
using ScreenRecognition.Desktop.View.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        public PropShieldModel<TranslatorApiKeyOutputModel> TranslatorApiKeyStatusCustom { get; set; } = new();

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
        private Setting? _selectedSettingCustom;
        public Setting? SelectedSettingCustom
        {
            get => _selectedSettingCustom;
            set
            {
                if (value != null)
                {
                    _selectedSettingCustom = value;
                    OnPropertyChanged(nameof(SelectedSettingCustom));

                    SetSettings(_selectedSettingCustom.Name);
                }
            }
        }
        #endregion

        #region Commands
        public ICommand ValidateApiKeyCommand { get; private set; }
        private async void ValidateApiKeyMethod(object obj)
        {
            if (!string.IsNullOrEmpty(TranslatorApiKeyCustom.Property) && SelectedTranslatorCustom.Property != null)
            {
                await Task.Run(async () =>
                {
                    string? apiKey = TranslatorApiKeyCustom.Property;

                    bool apiKeyValidationResult = await _controller.Get<bool, bool>($"Screen/ApiKeyValidation?translatorName={SelectedTranslatorCustom.Property.Name}&apiKey={apiKey}");
                    Color validationColor = Color.Red;

                    if (apiKeyValidationResult == true)
                        validationColor = Color.Green;

                    TranslatorApiKeyStatusCustom.Property = new TranslatorApiKeyOutputModel(apiKey, apiKeyValidationResult, validationColor);
                });
            }
        }
        #endregion
        #endregion

        public SettingsPageViewModel()
        {
            _controller = new UniversalController();
            ValidateApiKeyCommand = new DelegateCommand(ValidateApiKeyMethod);

            AuthChecker();
            OnStartup();
        }

        private async void OnStartup()
        {
            // Получение списка кнопок для хоткея
            HotkeyModifiersListCustom.Property = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.Modifiers)).Cast<GlobalHotKeys.Native.Types.Modifiers>().ToList();
            HotkeyKeyListCustom.Property = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.VirtualKeyCode)).Cast<GlobalHotKeys.Native.Types.VirtualKeyCode>().ToList();

            if (ConnectedUserSingleton.ConnectionStatus == false)
                return;

            // Получение списков
            SettingsListCustom.Property = await _controller.Get<List<Setting?>, List<Setting?>>("Settings/Settings");
            LanguageListCustom.Property = await _controller.Get<List<Language?>, List<Language?>>("Settings/Languages");
            CountryListCustom.Property = await _controller.Get<List<Country?>, List<Country?>>("Settings/Countries");
            OcrListCustom.Property = await _controller.Get<List<Ocr?>, List<Ocr?>>("Settings/Ocrs");
            TranslatorListCustom.Property = await _controller.Get<List<Translator?>, List<Translator?>>("Settings/Translators");

            SetSettings();
        }

        private async void SetSettings(string settingsName = "default")
        {
            // Получение настроек
            SettingsCustom.Property = await _controller.Get<Setting?, Setting?>($"Settings/ProfileSettings?userId={ConnectedUserSingleton.User.Id}&name={settingsName}");

            SetProgramSettings();
            SetUserInfo();
        }

        private void SetUserInfo()
        {
            UserCustom.Property = ConnectedUserSingleton.User;
            SelectedCountryCustom.Property = CountryListCustom.Property.FirstOrDefault(e => e.Id == UserCustom.Property.CountryId);
        }

        private void SetProgramSettings()
        {
            if (SelectedSettingCustom == null)
                SelectedSettingCustom = SettingsListCustom.Property.FirstOrDefault(e => e.Name == SettingsCustom.Property.Name);

            SelectedOcrCustom.Property = OcrListCustom.Property.FirstOrDefault(e => e.Id == SelectedSettingCustom.SelectedOcrid);
            SelectedTranslatorCustom.Property = TranslatorListCustom.Property.FirstOrDefault(e => e.Id == SelectedSettingCustom.SelectedTranslatorId);
            TranslatorApiKeyCustom.Property = SelectedSettingCustom.TranslatorApiKey;
            TranslatorApiKeyStatusCustom.Property = new TranslatorApiKeyOutputModel(TranslatorApiKeyCustom.Property, null, null);
            OcrLanguageCustom.Property = LanguageListCustom.Property.FirstOrDefault(e => e.Id == SelectedSettingCustom.InputLanguageId);
            TranslatorLanguageCustom.Property = LanguageListCustom.Property.FirstOrDefault(e => e.Id == SelectedSettingCustom.OutputLanguageId);
            ResultColorCustom.Property = SelectedSettingCustom.ResultColor;
            CurrentProfileNameCustom.Property = SelectedSettingCustom.Name;
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
