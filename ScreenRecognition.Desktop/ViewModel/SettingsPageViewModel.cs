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

        #region Shared
        private Visibility _disconnectedPanelVisibility;
        public Visibility DisconnectedPanelVisibility
        {
            get => _disconnectedPanelVisibility;
            set
            {
                _disconnectedPanelVisibility = value;
                OnPropertyChanged(nameof(DisconnectedPanelVisibility));
            }
        }
        private Visibility _profilePanelVisibility;
        public Visibility ProfilePanelVisibility
        {
            get => _profilePanelVisibility;
            set
            {
                _profilePanelVisibility = value;
                OnPropertyChanged(nameof(ProfilePanelVisibility));
            }
        }
        #endregion
        #region Program Settings
        private Setting _settings;
        public Setting Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                OnPropertyChanged(nameof(Settings));
            }
        }

        private List<Language?> _languageList;
        public List<Language?> LanguageList
        {
            get => _languageList;
            set
            {
                _languageList = value;
                OnPropertyChanged(nameof(LanguageList));
            }
        }

        private Language? _ocrLanguage;
        public Language? OcrLanguage
        {
            get => _ocrLanguage;
            set
            {
                _ocrLanguage = value;
                OnPropertyChanged(nameof(OcrLanguage));
            }
        }
        private Language? _translatorLanguage;
        public Language? TranslatorLanguage
        {
            get => _translatorLanguage;
            set
            {
                _translatorLanguage = value;
                OnPropertyChanged(nameof(TranslatorLanguage));
            }
        }

        private List<Translator?> _translatorList;
        public List<Translator?> TranslatorList
        {
            get => _translatorList;
            set
            {
                _translatorList = value;
                OnPropertyChanged(nameof(TranslatorList));
            }
        }
        private Translator? _selectedTranslator;
        public Translator? SelectedTranslator
        {
            get => _selectedTranslator;
            set
            {
                _selectedTranslator = value;
                OnPropertyChanged(nameof(SelectedTranslator));
            }
        }

        private List<Ocr?> _ocrList;
        public List<Ocr?> OcrList
        {
            get => _ocrList;
            set
            {
                _ocrList = value;
                OnPropertyChanged(nameof(OcrList));
            }
        }
        private Ocr? _selectedOcr;
        public Ocr? SelectedOcr
        {
            get => _selectedOcr;
            set
            {
                _selectedOcr = value;
                OnPropertyChanged(nameof(SelectedOcr));
            }
        }
        private string _resultColor;
        public string ResultColor
        {
            get => _resultColor;
            set
            {
                _resultColor = value;
                OnPropertyChanged(nameof(ResultColor));
            }
        }
        private string _translatorApiKey;
        public string TranslatorApiKey
        {
            get => _translatorApiKey;
            set
            {
                _translatorApiKey = value;
                OnPropertyChanged(nameof(TranslatorApiKey));
            }
        }
        private string _currentProfileName;
        public string CurrentProfileName
        {
            get => _currentProfileName;
            set
            {
                _currentProfileName = value;
                OnPropertyChanged(nameof(CurrentProfileName));
            }
        }

        private bool _startupWithSystem;
        public bool StartupWithSystem
        {
            get => _startupWithSystem;
            set
            {
                _startupWithSystem = value;
                OnPropertyChanged(nameof(StartupWithSystem));
            }
        }
        private bool _minimizeToTray;
        public bool MinimizeToTray
        {
            get => _minimizeToTray;
            set
            {
                _minimizeToTray = value;
                OnPropertyChanged(nameof(MinimizeToTray));
            }
        }
        private bool _t9Enable;
        public bool T9Enable
        {
            get => _t9Enable;
            set
            {
                _t9Enable = value;
                OnPropertyChanged(nameof(T9Enable));
            }
        }

        private List<GlobalHotKeys.Native.Types.VirtualKeyCode> _hotkeyKeyList;
        public List<GlobalHotKeys.Native.Types.VirtualKeyCode> HotkeyKeyList
        {
            get => _hotkeyKeyList;
            set
            {
                _hotkeyKeyList = value;
                OnPropertyChanged(nameof(HotkeyKeyList));
            }
        }
        private GlobalHotKeys.Native.Types.VirtualKeyCode _selectedHotkeyKey;
        public GlobalHotKeys.Native.Types.VirtualKeyCode SelectedHotkeyKey
        {
            get => _selectedHotkeyKey;
            set
            {
                _selectedHotkeyKey = value;
                OnPropertyChanged(nameof(SelectedHotkeyKey));
            }
        }

        private List<GlobalHotKeys.Native.Types.Modifiers> _hotkeyModifiersList;
        public List<GlobalHotKeys.Native.Types.Modifiers> HotkeyModifiersList
        {
            get => _hotkeyModifiersList;
            set
            {
                _hotkeyModifiersList = value;
                OnPropertyChanged(nameof(HotkeyModifiersList));
            }
        }
        private GlobalHotKeys.Native.Types.Modifiers _selectedHotkeyModifier;
        public GlobalHotKeys.Native.Types.Modifiers SelectedHotkeyModifier
        {
            get => _selectedHotkeyModifier;
            set
            {
                _selectedHotkeyModifier = value;
                OnPropertyChanged(nameof(SelectedHotkeyModifier));
            }
        }
        #endregion
        #region Profile Settings
        private User? _user;
        public User? User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private List<Country?> _countryList;
        public List<Country?> CountryList
        {
            get => _countryList;
            set
            {
                _countryList = value;
                OnPropertyChanged(nameof(CountryList));
            }
        }

        private List<Setting?> _settingsList;
        public List<Setting?> SettingsList
        {
            get => _settingsList;
            set
            {
                _settingsList = value;
                OnPropertyChanged(nameof(SettingsList));
            }
        }
        private Setting? _selectedSetting;
        public Setting? SelectedSetting
        {
            get => _selectedSetting;
            set
            {
                _selectedSetting = value;
                OnPropertyChanged(nameof(SelectedSetting));
            }
        }
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
        public PropShieldModel<User?> UserCustom = new();

        public PropShieldModel<List<Country?>> CountryListCustom = new();
        public PropShieldModel<Country?> SelectedCountryCustom = new();

        public PropShieldModel<List<Setting?>> SettingsListCustom = new();
        public PropShieldModel<Setting?> SelectedSettingCustom = new();
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
            // Custom

            HotkeyModifiersListCustom.Property = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.Modifiers)).Cast<GlobalHotKeys.Native.Types.Modifiers>().ToList();
            HotkeyKeyListCustom.Property = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.VirtualKeyCode)).Cast<GlobalHotKeys.Native.Types.VirtualKeyCode>().ToList();

            // Default

            HotkeyModifiersList = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.Modifiers)).Cast<GlobalHotKeys.Native.Types.Modifiers>().ToList();
            HotkeyKeyList = Enum.GetValues(typeof(GlobalHotKeys.Native.Types.VirtualKeyCode)).Cast<GlobalHotKeys.Native.Types.VirtualKeyCode>().ToList();

            // Получение списка листов
            await Task.Run(async () =>
            {
                // Custom

                LanguageListCustom.Property = await _controller.Get<List<Language?>, List<Language?>>("Settings/Languages");
                CountryListCustom.Property = await _controller.Get<List<Country?>, List<Country?>>("Settings/Countries");
                SettingsListCustom.Property = await _controller.Get<List<Setting?>, List<Setting?>>("Settings/Settings");
                OcrListCustom.Property = await _controller.Get<List<Ocr?>, List<Ocr?>>("Settings/Ocrs");
                TranslatorListCustom.Property = await _controller.Get<List<Translator?>, List<Translator?>>("Settings/Translators");

                // Default

                LanguageList = await _controller.Get<List<Language?>, List<Language?>>("Settings/Languages");
                CountryList = await _controller.Get<List<Country?>, List<Country?>>("Settings/Countries");
                SettingsList = await _controller.Get<List<Setting?>, List<Setting?>>("Settings/Settings");
                OcrList = await _controller.Get<List<Ocr?>, List<Ocr?>>("Settings/Ocrs");
                TranslatorList = await _controller.Get<List<Translator?>, List<Translator?>>("Settings/Translators");
            });

            if (ConnectedUserSingleton.ConnectionStatus == false)
                return;

            // Получение выбранных результатов
            await Task.Run(async () =>
            {
                // Custom

                SettingsCustom.Property = await _controller.Get<Setting?, Setting?>($"Settings/ProfileSettings?userId={ConnectedUserSingleton.User.Id}&name=default");

                // Default

                Settings = await _controller.Get<Setting?, Setting?>($"Settings/ProfileSettings?userId={ConnectedUserSingleton.User.Id}&name=default");
            });

            ApplyDefaultSettings();
        }

        private void ApplyDefaultSettings()
        {
            // Custom

            SelectedOcrCustom.Property = OcrListCustom.Property.FirstOrDefault(e => e.Id == Settings.SelectedOcrid);
            SelectedTranslatorCustom.Property = TranslatorListCustom.Property.FirstOrDefault(e => e.Id == Settings.SelectedTranslatorId);
            TranslatorApiKeyCustom.Property = SettingsCustom.Property.TranslatorApiKey;
            OcrLanguageCustom.Property = LanguageListCustom.Property.FirstOrDefault(e => e.Id == Settings.InputLanguageId);
            TranslatorLanguageCustom.Property = LanguageListCustom.Property.FirstOrDefault(e => e.Id == Settings.OutputLanguageId);
            ResultColorCustom.Property = SettingsCustom.Property.ResultColor;

            // Default

            SelectedOcr = OcrList.FirstOrDefault(e => e.Id == Settings.SelectedOcrid);
            SelectedTranslator = TranslatorList.FirstOrDefault(e=>e.Id == Settings.SelectedTranslatorId);
            TranslatorApiKey = Settings.TranslatorApiKey;
            OcrLanguage = LanguageList.FirstOrDefault(e=>e.Id == Settings.InputLanguageId);
            TranslatorLanguage = LanguageList.FirstOrDefault(e => e.Id == Settings.OutputLanguageId);
            ResultColor = Settings.ResultColor;
        }

        public async void SaveSettings()
        {
            if (string.IsNullOrEmpty(ConnectedUserSingleton.Password))
            {
                HandyControl.Controls.MessageBox.Show("Вы не авторизованы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            // Переделать 90 строчку
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
