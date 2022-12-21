using ScreenRecognition.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenRecognition.Desktop.ViewModel
{
    public class SettingsPageViewModel : BaseViewModel
    {
        #region NotAuthPanelVisibility
        private Visibility _profilePanelVisibility;
        private Visibility _disconnectedPanelVisibility;

        public Visibility DisconnectedPanelVisibility
        {
            get=> _disconnectedPanelVisibility;
            set
            {
                _disconnectedPanelVisibility = value;
                OnPropertyChanged(nameof(DisconnectedPanelVisibility));
            }
        }
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
        #region Shields and Properties
        private List<Language> _languageList;
        private Language _selectedInputLanguage;
        private Language _selectedOutputLanguage;

        public List<Language> LanguageList
        {
            get=> _languageList;
            set
            {
                _languageList = value;
                OnPropertyChanged(nameof(LanguageList));
            }
        }
        public Language SelectedInputLanguage
        {
            get => _selectedInputLanguage;
            set
            {
                _selectedInputLanguage = value;
                OnPropertyChanged(nameof(SelectedInputLanguage));
            }
        }
        public Language SelectedOutputLanguage
        {
            get => _selectedOutputLanguage;
            set
            {
                _selectedOutputLanguage = value;
                OnPropertyChanged(nameof(SelectedOutputLanguage));
            }
        }

        private List<Translator> _translatorList;
        private Translator _selectedTranslator;

        public List<Translator> TranslatorList
        {
            get => _translatorList;
            set
            {
                _translatorList = value;
                OnPropertyChanged(nameof(TranslatorList));
            }
        }
        public Translator SelectedTranslator
        {
            get => _selectedTranslator;
            set
            {
                _selectedTranslator = value;
                OnPropertyChanged(nameof(SelectedTranslator));
            }
        }

       
        #endregion

        public SettingsPageViewModel()
        {
            AuthChecker();
        }

        private void AuthChecker()
        {
            if(ConnectedUserSingleton.ConnectionStatus == true)
            {
                ProfilePanelVisibility = Visibility.Visible;
                DisconnectedPanelVisibility = Visibility.Collapsed;
            }
            else
            {
                ProfilePanelVisibility = Visibility.Collapsed;
                DisconnectedPanelVisibility = Visibility.Visible;
            }
        }
    }
}
