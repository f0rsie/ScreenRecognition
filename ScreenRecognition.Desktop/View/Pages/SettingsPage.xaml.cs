using ScreenRecognition.Desktop.Models.SingletonModels;
using ScreenRecognition.Desktop.ViewModel.PageViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ScreenRecognition.Desktop.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();

            ProfilePasswordSetter();
        }

        private void ProfilePasswordSetter()
        {
            userPassword.Password = ConnectedUserSingleton.Password;
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsPageViewModel).SaveSettings();
        }

        private void SaveAccountInfo_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SettingsPageViewModel).UserCustom.Property.Password = userPassword.Password;
            (DataContext as SettingsPageViewModel).SaveAccountInfo();
        }
    }
}
