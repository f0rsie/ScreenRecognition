using ScreenRecognition.Desktop.ViewModel.WindowViewModels;
using System.Windows;

namespace ScreenRecognition.Desktop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public SignUpWindow()
        {
            InitializeComponent();
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            if (Password.Password != PasswordRepeat.Password)
            {
                HandyControl.Controls.MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            (DataContext as SignUpWindowViewModel).Password.Property = Password.Password;
            (DataContext as SignUpWindowViewModel).SignUp(this);
        }
    }
}
