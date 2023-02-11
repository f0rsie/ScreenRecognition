using ScreenRecognition.Desktop.ViewModel.WindowViewModels;
using System.Windows;

namespace ScreenRecognition.Desktop.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для SignUpWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        public SignInWindow()
        {
            InitializeComponent();
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SignInWindowViewModel)?.SignUp();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as SignInWindowViewModel).Password = Password.Password;

            (DataContext as SignInWindowViewModel)?.SignIn();
        }
    }
}
