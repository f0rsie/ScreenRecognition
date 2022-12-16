using ScreenRecognition.Desktop.View.Pages;
using ScreenRecognition.Desktop.View.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ScreenRecognition.Desktop.ViewModel
{
    public class SignInWindowViewModel : BaseViewModel
    {
        private Visibility _currentWindowVisibility;

        private string? _login;
        private string? _password;

        public Visibility CurrentWindowVisibility
        {
            get => _currentWindowVisibility;
            set
            {
                _currentWindowVisibility = value;
                OnPropertyChanged(nameof(CurrentWindowVisibility));
            }
        }

        public string? Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public SignInWindowViewModel()
        {

        }

        public void SignUp()
        {

        }
    }
}
