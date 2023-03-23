using ScreenRecognition.Desktop.Models.SingletonModels;
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
    }
}
