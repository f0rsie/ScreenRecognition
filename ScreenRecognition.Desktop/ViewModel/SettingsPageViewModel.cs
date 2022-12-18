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
